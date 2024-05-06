using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;
using ScannerDrivers.TWAIN.Contants.Generic;
using ScannerDrivers.TWAIN.Contants.Structure;
using ScannerDrivers.TWAIN.Contants.Type;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    /// <summary>
    /// This is the base TWAIN class.  It's versioned so that developers can
    /// safely make a reference to it, if they don't want to actually include
    /// the TWAIN modules in their project.
    /// 
    /// Here are the goals of this class:
    ///
    /// - Make the interface better than raw TWAIN (like with C/C++), but don't
    ///   go as far as making a toolkit.  Expose as much of TWAIN as possible.
    ///   Part of this involves making a CSV interface to abstract away some of
    ///   the pain of the C/C++ structures, especially TW_CAPABILITY.
    ///   
    /// - Use type checking wherever possible.  This is why the TWAIN contants
    ///   tend to be enumerations and why there are multiple entry points into
    ///   the DSM_Entry function.
    ///   
    /// - Avoid unsafe code.  We use marshalling, and we're forced to use some
    ///   unmanaged memory, but that's it.
    ///   
    /// - Run thread safe.  Force as many TWAIN calls into a single thread as
    ///   possible.  The main exceptions are MSG_PROCESSEVENT and any of the
    ///   calls that unwind the state machine (ex: MSG_DISABLEDS).  Otherwise
    ///   all of the calls are serialized through a single thread.
    ///   
    /// - Avoid use of System.Windows content, so that the TWAIN assembly can be
    ///   used as broadly as possible (specifically with Mono).
    ///   
    /// - Support all platforms.  The code currently works on 32-bit and 64-bit
    ///   Windows.  It's been tested on 32-bit Linux and Mac OS X (using Mono).
    ///   It should work as 64-bit on those platforms.  We're also supporting
    ///   both TWAIN_32.DLL and TWAINDSM.DLL on Windows, and we'll support both
    ///   TWAIN.framework and a TWAINDSM.framework (whenever it gets created).
    ///   
    /// - Virtualiaze all public functions so that developers can extended the
    ///   class with a minimum of fuss.
    /// </summary>
    public partial class TWAINSDK : IDisposable
    {

        ///////////////////////////////////////////////////////////////////////////////
        // Public Functions, these are the essentials...
        ///////////////////////////////////////////////////////////////////////////////
        #region Public Functions...

        /// <summary>
        /// Our constructor...
        /// </summary>
        /// <param name="a_szManufacturer">Application manufacturer</param>
        /// <param name="a_szProductFamily">Application product family</param>
        /// <param name="a_szProductName">Name of the application</param>
        /// <param name="a_u16ProtocolMajor">TWAIN protocol major (doesn't have to match TWAINH.CS)</param>
        /// <param name="a_u16ProtocolMinor">TWAIN protocol minor (doesn't have to match TWAINH.CS)</param>
        /// <param name="a_u32SupportedGroups">Bitmask of DG_ flags</param>
        /// <param name="a_twcy">Country code for the application</param>
        /// <param name="a_szInfo">Info about the application</param>
        /// <param name="a_twlg">Language code for the application</param>
        /// <param name="a_u16MajorNum">Application's major version</param>
        /// <param name="a_u16MinorNum">Application's minor version</param>
        /// <param name="a_blUseLegacyDSM">Use the legacy DSM (like TWAIN_32.DLL)</param>
        /// <param name="a_blUseCallbacks">Use callbacks instead of Windows post message</param>
        /// <param name="a_deviceeventback">Function to receive device events</param>
        /// <param name="a_scancallback">Function to handle scanning</param>
        /// <param name="a_runinuithreaddelegate">Help us run in the GUI thread on Windows</param>
        /// <param name="a_intptrHwnd">window handle</param>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust", Unrestricted = false)]
        public TWAINSDK
        (
            string a_szManufacturer,
            string a_szProductFamily,
            string a_szProductName,
            ushort a_u16ProtocolMajor,
            ushort a_u16ProtocolMinor,
            uint a_u32SupportedGroups,
            Countries a_twcy,
            string a_szInfo,
            Languages a_twlg,
            ushort a_u16MajorNum,
            ushort a_u16MinorNum,
            bool a_blUseLegacyDSM,
            bool a_blUseCallbacks,
            DeviceEventCallback a_deviceeventback,
            ScanCallback a_scancallback,
            RunInUiThreadDelegate a_runinuithreaddelegate,
            IntPtr a_intptrHwnd
        )
        {
            TW_IDENTITY twidentity;

            // Since we're using P/Invoke in this sample, the DLL
            // is implicitly loaded as we access it, so we can
            // never go lower than state 2...
            m_state = STATE.S2;

            // Register the caller's info...
            twidentity = default(TW_IDENTITY);
            twidentity.Manufacturer.Set(a_szManufacturer);
            twidentity.ProductFamily.Set(a_szProductFamily);
            twidentity.ProductName.Set(a_szProductName);
            twidentity.ProtocolMajor = a_u16ProtocolMajor;
            twidentity.ProtocolMinor = a_u16ProtocolMinor;
            twidentity.SupportedGroups = a_u32SupportedGroups;
            twidentity.Version.Country = a_twcy;
            twidentity.Version.Info.Set(a_szInfo);
            twidentity.Version.Language = a_twlg;
            twidentity.Version.MajorNum = a_u16MajorNum;
            twidentity.Version.MinorNum = a_u16MinorNum;
            m_twidentityApp = twidentity;
            m_twidentitylegacyApp = TwidentityToTwidentitylegacy(twidentity);
            m_twidentitymacosxApp = TwidentityToTwidentitymacosx(twidentity);
            m_deviceeventcallback = a_deviceeventback;
            m_scancallback = a_scancallback;
            m_runinuithreaddelegate = a_runinuithreaddelegate;
            m_intptrHwnd = a_intptrHwnd;

            // Help for RunInUiThread...
            m_threaddataDatAudiofilexfer = default(ThreadData);
            m_threaddataDatAudionativexfer = default(ThreadData);
            m_threaddataDatCapability = default(ThreadData);
            m_threaddataDatEvent = default(ThreadData);
            m_threaddataDatExtimageinfo = default(ThreadData);
            m_threaddataDatIdentity = default(ThreadData);
            m_threaddataDatImagefilexfer = default(ThreadData);
            m_threaddataDatImageinfo = default(ThreadData);
            m_threaddataDatImagelayout = default(ThreadData);
            m_threaddataDatImagememfilexfer = default(ThreadData);
            m_threaddataDatImagememxfer = default(ThreadData);
            m_threaddataDatImagenativexfer = default(ThreadData);
            m_threaddataDatParent = default(ThreadData);
            m_threaddataDatPendingxfers = default(ThreadData);
            m_threaddataDatSetupfilexfer = default(ThreadData);
            m_threaddataDatSetupmemxfer = default(ThreadData);
            m_threaddataDatStatus = default(ThreadData);
            m_threaddataDatUserinterface = default(ThreadData);

            // We always go through a discovery process, even on 32-bit...
            m_linuxdsm = LinuxDsm.Unknown;

            // Placeholder for our DS identity...
            m_twidentityDs = default(TW_IDENTITY);
            m_twidentitylegacyDs = default(TW_IDENTITY_LEGACY);
            m_twidentitymacosxDs = default(TW_IDENTITY_MACOSX);

            // We'll normally do an automatic get of DataArgumentTypes.STATUS, but if we'd
            // like to turn it off, this is the variable to hit...
            m_blAutoDatStatus = true;

            // Our helper functions from the DSM...
            m_twentrypointdelegates = default(TW_ENTRYPOINT_DELEGATES);

            // Our events...
            m_autoreseteventCaller = new AutoResetEvent(false);
            m_autoreseteventThread = new AutoResetEvent(false);
            m_autoreseteventRollback = new AutoResetEvent(false);
            m_autoreseteventThreadStarted = new AutoResetEvent(false);
            m_lockTwain = new Object();

            // Windows only...
            if (ms_platform == Platform.WINDOWS)
            {
                m_blUseLegacyDSM = a_blUseLegacyDSM;
                m_blUseCallbacks = a_blUseCallbacks;
                m_windowsdsmentrycontrolcallbackdelegate = WindowsDsmEntryCallbackProxy;
            }

            // Linux only...
            else if (ms_platform == Platform.LINUX)
            {
                // The user can't set these value, we have to decide automatically
                // which DSM to use, and only callbacks are supported...
                m_blUseLegacyDSM = false;
                m_blUseCallbacks = true;
                m_linuxdsmentrycontrolcallbackdelegate = LinuxDsmEntryCallbackProxy;

                // We assume the new DSM for 32-bit systems...
                if (GetMachineWordBitSize() == 32)
                {
                    m_blFound020302Dsm64bit = false;
                    if (File.Exists("/usr/local/lib64/libtwaindsm.so"))
                    {
                        m_blFoundLatestDsm64 = true;
                    }
                    if (File.Exists("/usr/local/lib/libtwaindsm.so"))
                    {
                        m_blFoundLatestDsm = true;
                    }
                }

                // Check for the old DSM, but only on 64-bit systems...
                if ((GetMachineWordBitSize() == 64) && File.Exists("/usr/local/lib/libtwaindsm.so.2.3.2"))
                {
                    m_blFound020302Dsm64bit = true;
                }

                // Check for any newer DSM, but only on 64-bit systems...
                if ((GetMachineWordBitSize() == 64) && (File.Exists("/usr/local/lib/libtwaindsm.so") || File.Exists("/usr/local/lib64/libtwaindsm.so")))
                {
                    bool blCheckForNewDsm = true;

                    // Get the DSMs by their fully decorated names...
                    string[] aszDsm = Directory.GetFiles("/usr/local/lib64", "libtwaindsm.so.*.*.*");
                    if (aszDsm.Length == 0)
                    {
                        aszDsm = Directory.GetFiles("/usr/local/lib", "libtwaindsm.so.*.*.*");
                    }
                    if ((aszDsm != null) && (aszDsm.Length > 0))
                    {
                        // Check each name, we only want to launch the process if
                        // we find an old DSM...
                        foreach (string szDsm in aszDsm)
                        {
                            if (szDsm.EndsWith("so.2.0") || szDsm.Contains(".so.2.0.")
                                || szDsm.EndsWith("so.2.1") || szDsm.Contains(".so.2.1.")
                                || szDsm.EndsWith("so.2.2") || szDsm.Contains(".so.2.2.")
                                || szDsm.EndsWith("so.2.3") || szDsm.Contains(".so.2.3."))
                            {
                                // If we get a match, see if the symbolic link is
                                // pointing to old junk...
                                Process p = new Process();
                                p.StartInfo.UseShellExecute = false;
                                p.StartInfo.RedirectStandardOutput = true;
                                p.StartInfo.FileName = "readlink";
                                p.StartInfo.Arguments = "-e /usr/local/lib/libtwaindsm.so";
                                p.Start();
                                string szOutput = p.StandardOutput.ReadToEnd();
                                p.WaitForExit();
                                p.Dispose();
                                // We never did any 1.x stuff...
                                if ((szOutput != null)
                                    && (szOutput.EndsWith(".so.2.0") || szOutput.Contains(".so.2.0.")
                                    || szOutput.EndsWith(".so.2.1") || szOutput.Contains(".so.2.1.")
                                    || szOutput.EndsWith(".so.2.2") || szOutput.Contains(".so.2.2.")
                                    || szOutput.EndsWith(".so.2.3") || szOutput.Contains(".so.2.3.")))
                                {
                                    // libtwaindsm.so is pointing to an old DSM...
                                    blCheckForNewDsm = false;
                                }
                                break;
                            }
                        }
                    }

                    // Is the symbolic link pointing to a new DSM?
                    if (blCheckForNewDsm && (aszDsm != null) && (aszDsm.Length > 0))
                    {
                        foreach (string szDsm in aszDsm)
                        {
                            // I guess this is reasonably future-proof...
                            if (szDsm.Contains("so.2.4")
                                || szDsm.Contains("so.2.5")
                                || szDsm.Contains("so.2.6")
                                || szDsm.Contains("so.2.7")
                                || szDsm.Contains("so.2.8")
                                || szDsm.Contains("so.2.9")
                                || szDsm.Contains("so.2.10")
                                || szDsm.Contains("so.2.11")
                                || szDsm.Contains("so.2.12")
                                || szDsm.Contains("so.2.13")
                                || szDsm.Contains("so.2.14")
                                || szDsm.Contains("so.3")
                                || szDsm.Contains("so.4"))
                            {
                                // libtwaindsm.so is pointing to a new DSM...
                                if (szDsm.Contains("lib64"))
                                {
                                    m_blFoundLatestDsm64 = true;
                                }
                                else
                                {
                                    m_blFoundLatestDsm = true;
                                }
                                break;
                            }
                        }
                    }
                }
            }

            // Mac OS X only...
            else if (ms_platform == Platform.MACOSX)
            {
                m_blUseLegacyDSM = a_blUseLegacyDSM;
                m_blUseCallbacks = true;
                m_macosxdsmentrycontrolcallbackdelegate = MacosxDsmEntryCallbackProxy;
            }

            // Uh-oh, Log will throw an exception for us...
            else
            {
                Log.Assert("Unsupported platform..." + ms_platform);
            }

            // Activate our thread...
            /*
            if (m_threadTwain == null)
            {
                m_twaincommand = new TwainCommand();
                m_threadTwain = new Thread(Main);
                m_threadTwain.Start();
                if (!m_autoreseteventThreadStarted.WaitOne(5000))
                {
                    try
                    {
                        m_threadTwain.Abort();
                        m_threadTwain = null;
                    }
                    catch (Exception exception)
                    {
                        // Log will throw an exception for us...
                        Log.Assert("Failed to start the TWAIN background thread - " + exception.Message);
                    }
                }
            }
            */
        }


        /// <summary>
        /// Cleanup...
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase")]
        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Report the path to the DSM we're using...
        /// </summary>
        /// <returns>full path to the DSM</returns>
        public string GetDsmPath()
        {
            string szDsmPath = "";

            // Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                if (m_blUseLegacyDSM)
                {
                    szDsmPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "twain_32.dll");
                }
                else
                {
                    szDsmPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "twaindsm.dll");
                }
            }

            // Linux...
            else if (ms_platform == Platform.LINUX)
            {
                if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                {
                    szDsmPath = "/usr/local/lib64/libtwaindsm.so";
                }
                else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                {
                    szDsmPath = "/usr/local/lib/libtwaindsm.so";
                }
                else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                {
                    szDsmPath = "/usr/local/lib/libtwaindsm.so.2.3.2";
                }
            }

            // Mac OS X, which has to be different...
            else if (ms_platform == Platform.MACOSX)
            {
                if (m_blUseLegacyDSM)
                {
                    szDsmPath = "/System/Library/Frameworks/TWAIN.framework/TWAIN";
                }
                else
                {
                    szDsmPath = "/Library/Frameworks/TWAIN.framework/TWAIN";
                }
            }

            // If found...
            if (File.Exists(szDsmPath))
            {
                return (szDsmPath);
            }

            // Ruh-roh...
            if (string.IsNullOrEmpty(szDsmPath))
            {
                return ("(could not identify a DSM candidate for this platform - '" + ms_platform + "')");
            }

            // Hmmm...
            return ("(could not find '" + szDsmPath + "')");
        }

        /// <summary>
        /// Get the identity of the current application...
        /// </summary>
        /// <returns></returns>
        public string GetAppIdentity()
        {
            return (IdentityToCsv(m_twidentityApp));
        }

        /// <summary>
        /// Get the identity of the current driver, if we have one...
        /// </summary>
        /// <returns></returns>
        public string GetDsIdentity()
        {
            if (m_state < STATE.S4)
            {
                return (IdentityToCsv(default(TW_IDENTITY)));
            }
            return (IdentityToCsv(m_twidentityDs));
        }
        /// <summary>
        /// Report the current TWAIN state as we understand it...
        /// </summary>
        /// <returns>The current TWAIN state for the application</returns>
        public STATE GetState()
        {
            return (m_state);
        }

        /// <summary>
        /// True if the DSM has the DSM2 flag set...
        /// </summary>
        /// <returns>True if we detect the TWAIN Working Group open source DSM</returns>
        public bool IsDsm2()
        {
            // Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                return ((m_twidentitylegacyApp.SupportedGroups & (uint)DataGroups.DSM2) != 0);
            }

            // Linux...
            if (ms_platform == Platform.LINUX)
            {
                return ((m_twidentitylegacyApp.SupportedGroups & (uint)DataGroups.DSM2) != 0);
            }

            // Mac OS X...
            if (ms_platform == Platform.MACOSX)
            {
                return ((m_twidentitymacosxApp.SupportedGroups & (uint)DataGroups.DSM2) != 0);
            }

            // Trouble, Log will throw an exception for us...
            Log.Assert("Unsupported platform..." + ms_platform);
            return (false);
        }

        /// <summary>
        /// Have we seen the first image since MSG.ENABLEDS?
        /// </summary>
        /// <returns>True if the driver is ready to transfer images</returns>
        public bool IsMsgXferReady()
        {
            return (m_blIsMsgxferready);
        }

        /// <summary>
        /// Has the cancel button been pressed since the last MSG.ENABLEDS?
        /// </summary>
        /// <returns>True if the cancel button was pressed</returns>
        public bool IsMsgCloseDsReq()
        {
            return (m_blIsMsgclosedsreq);
        }

        /// <summary>
        /// Has the OK button been pressed since the last MSG.ENABLEDS?
        /// </summary>
        /// <returns>True if the OK button was pressed</returns>
        public bool IsMsgCloseDsOk()
        {
            return (m_blIsMsgclosedsok);
        }

        #region Public Helper Functions...

        /// <summary>
        /// Copy intptr to intptr...
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="count"></param>
        public static void MemCpy(IntPtr dest, IntPtr src, int count)
        {
            if (TWAINSDK.GetPlatform() == Platform.WINDOWS)
            {
                NativeMethods.CopyMemory(dest, src, (uint)count);
            }
            else
            {
                NativeMethods.memcpy(dest, src, (IntPtr)count);
            }
        }

        /// <summary>
        /// Safely move intptr to intptr...
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="count"></param>
        public static void MemMove(IntPtr dest, IntPtr src, int count)
        {
            if (TWAINSDK.GetPlatform() == Platform.WINDOWS)
            {
                NativeMethods.MoveMemory(dest, src, (uint)count);
            }
            else
            {
                NativeMethods.memmove(dest, src, (IntPtr)count);
            }
        }
        public static int WriteImageFileCompressed(string filename, IntPtr imageData, int imageSize, out string finalFilename)
        {
            finalFilename = "";

            try
            {
                if (imageData == IntPtr.Zero || imageSize <= 0)
                {
                    return -1; // Invalid image data
                }

                // Ensure the file has an extension based on its content
                if (!Path.HasExtension(filename))
                {
                    byte[] signature = new byte[8]; // Ensure enough capacity for TIFF signatures
                    Marshal.Copy(imageData, signature, 0, Math.Min(imageSize, signature.Length)); // Copy minimum of imageSize and signature.Length bytes

                    string extension = "";
                    if (signature[0] == 0xFF && signature[1] == 0xD8)
                    {
                        // JPEG
                        extension = ".jpg";
                    }
                    else if (signature[0] == 0x42 && signature[1] == 0x4D)
                    {
                        // BMP
                        extension = ".bmp";
                    }
                    else if (signature[0] == 0x47 && signature[1] == 0x49 && signature[2] == 0x46 && signature[3] == 0x38)
                    {
                        // GIF
                        extension = ".gif";
                    }
                    else if (signature[0] == 0x89 && signature[1] == 0x50 && signature[2] == 0x4E && signature[3] == 0x47 && signature[4] == 0x0D && signature[5] == 0x0A && signature[6] == 0x1A && signature[7] == 0x0A)
                    {
                        // PNG
                        extension = ".png";
                    }
                    else if (signature[0] == 0x49 && signature[1] == 0x49 && signature[2] == 0x2A && signature[3] == 0x00)
                    {
                        // TIFF (little-endian)
                        extension = ".tif";
                    }
                    else if (signature[0] == 0x4D && signature[1] == 0x4D && signature[2] == 0x00 && signature[3] == 0x2A)
                    {
                        // TIFF (big-endian)
                        extension = ".tif";
                    }
                    else
                    {
                        // Unsupported format
                        return -1;
                    }

                    filename += extension;
                }

                finalFilename = filename;

                // Open the file for writing
                using (FileStream fileStream = File.Create(filename))
                {
                    // Write the image data to the file
                    byte[] buffer = new byte[imageSize];
                    Marshal.Copy(imageData, buffer, 0, imageSize);
                    fileStream.Write(buffer, 0, imageSize);
                }

                return imageSize; // Successfully wrote the image file
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing image file: " + ex.Message);
                return -1; // Error occurred
            }
        }

        public static int WriteImageFileLosless(string a_szFilename, IntPtr a_intptrPtr, int a_iBytes, out string a_szFinalFilename)
        {
            // Init stuff...
            a_szFinalFilename = "";

            // Try to write our file...
            try
            {
                // For the caller...
                a_szFinalFilename = a_szFilename;

                // Handle Windows...
                if (TWAINSDK.GetPlatform() == Platform.WINDOWS)
                {
                    // Load image from memory
                    using (var memoryStream = new MemoryStream())
                    {
                        byte[] imageData = new byte[a_iBytes];
                        Marshal.Copy(a_intptrPtr, imageData, 0, a_iBytes);
                        memoryStream.Write(imageData, 0, a_iBytes);
                        using (var image = Image.FromStream(memoryStream))
                        {
                            // Save image with compression
                            image.Save(a_szFilename, ImageFormat.Png); // Use ImageFormat.Tiff for TIFF format
                        }
                    }
                }

                // Handle everybody else...
                else
                {
                    // Load image from memory
                    using (var memoryStream = new MemoryStream())
                    {
                        byte[] imageData = new byte[a_iBytes];
                        Marshal.Copy(a_intptrPtr, imageData, 0, a_iBytes);
                        memoryStream.Write(imageData, 0, a_iBytes);
                        using (var image = Image.FromStream(memoryStream))
                        {
                            // Save image with compression
                            image.Save(a_szFilename, ImageFormat.Png); // Use ImageFormat.Tiff for TIFF format
                        }
                    }
                }
                return a_iBytes; // Return original size since compression is lossless
            }
            catch (Exception exception)
            {
                Log.Error("Write file failed <" + a_szFilename + "> - " + exception.Message);
                return (-1);
            }
        }
        /// <summary>
        /// Write stuff to a file without having to rebuffer it...
        /// </summary>
        /// <param name="a_szFilename"></param>
        /// <param name="a_intptrPtr"></param>
        /// <param name="a_iBytes"></param>
        /// <returns></returns>
        public static int WriteImageFile(string a_szFilename, IntPtr a_intptrPtr, int a_iBytes, out string a_szFinalFilename)
        {
            // Init stuff...
            a_szFinalFilename = "";

            // Try to write our file...
            try
            {
                // If we don't have an extension, try to add one...
                if (!Path.GetFileName(a_szFilename).Contains(".") && (a_iBytes >= 2))
                {
                    byte[] abData = new byte[2];
                    Marshal.Copy(a_intptrPtr, abData, 0, 2);
                    // BMP
                    if ((abData[0] == 0x42) && (abData[1] == 0x4D)) // BM
                    {
                        a_szFilename += ".bmp";
                    }
                    else if ((abData[0] == 0x49) && (abData[1] == 0x49)) // II
                    {
                        a_szFilename += ".tif";
                    }
                    else if ((abData[0] == 0xFF) && (abData[1] == 0xD8))
                    {
                        a_szFilename += ".jpg";
                    }
                    else if ((abData[0] == 0x25) && (abData[1] == 0x50)) // %P
                    {
                        a_szFilename += ".pdf";
                    }
                }

                // For the caller...
                a_szFinalFilename = a_szFilename;

                // Handle Windows...
                if (TWAINSDK.GetPlatform() == Platform.WINDOWS)
                {
                    IntPtr intptrFile;
                    IntPtr intptrBytes = (IntPtr)a_iBytes;
                    IntPtr intptrCount = (IntPtr)1;
                    if (NativeMethods._wfopen_s(out intptrFile, a_szFilename, "wb") != 0)
                    {
                        return (-1);
                    }
                    intptrBytes = NativeMethods.fwriteWin(a_intptrPtr, intptrCount, intptrBytes, intptrFile);
                    NativeMethods.fcloseWin(intptrFile);
                    return ((int)intptrBytes);
                }

                // Handle everybody else...
                else
                {
                    IntPtr intptrFile;
                    IntPtr intptrBytes;
                    intptrFile = NativeMethods.fopen(a_szFilename, "w");
                    if (intptrFile == IntPtr.Zero)
                    {
                        return (-1);
                    }
                    intptrBytes = NativeMethods.fwrite(a_intptrPtr, (IntPtr)1, (IntPtr)a_iBytes, intptrFile);
                    NativeMethods.fclose(intptrFile);
                    return ((int)intptrBytes);
                }
            }
            catch (Exception exception)
            {
                Log.Error("Write file failed <" + a_szFilename + "> - " + exception.Message);
                return (-1);
            }
        }
        #endregion
        #endregion
        ///////////////////////////////////////////////////////////////////////////////
        // Public Definitions, this is where you get the callback definitions for
        // handling device events and scanning...
        ///////////////////////////////////////////////////////////////////////////////
        #region Public Definitions...

        /// <summary>
        /// The form of the device event callback used by the caller when they are
        /// running in states 4, 5, 6 and 7...
        /// </summary>
        /// <returns></returns>
        public delegate STS DeviceEventCallback();

        /// <summary>
        /// The form of the callback used by the caller when they are running
        /// in states 5, 6 and 7; anything after DG_CONTROL / DAT_USERINTERFACE /
        /// MSG_ENABLEDS* until DG_CONTROL / DAT_USERINTERFACE / MSG_DISABLEDS...
        /// </summary>
        /// <returns></returns>
        public delegate STS ScanCallback(bool a_blClosing);

        /// <summary>
        /// We use this to run code in the context of the caller's UI thread...
        /// </summary>
        /// <param name="a_action">code to run</param>
        public delegate void RunInUiThreadDelegate(Action a_action);

        /// <summary>
        /// Only one DSM can be installed on a Linux system, and it must match
        /// the architecture.  To handle the legacy problem due to the bad
        /// definition of TW_INT32/TW_UINT32, we also support access to the
        /// 64-bit 2.3.2 DSM.  This is only used if we see a driver that seems
        /// to be returning garbage for its TW_IDENTITY...
        /// </summary>
        public enum LinuxDsm
        {
            Unknown,
            IsLatestDsm,
            Is020302Dsm64bit
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Private Functions, the main thread is in here...
        ///////////////////////////////////////////////////////////////////////////////
        #region Private Functions...

        /// <summary>
        /// Cleanup...
        /// </summary>
        /// <param name="a_blDisposing">true if we need to clean up managed resources</param>
        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        internal void Dispose(bool a_blDisposing)
        {
            // Free managed resources...
            if (a_blDisposing)
            {
                // Make sure we've closed any drivers...
                Rollback(Codes.STATE.S2);

                // Make sure that our thread is gone...
                if (m_threadTwain != null)
                {
                    m_threadTwain.Join();
                    m_threadTwain = null;
                }

                // This one too...
                if (m_threadRunningDatUserinterface != null)
                {
                    m_threadRunningDatUserinterface.Join();
                    m_threadRunningDatUserinterface = null;
                }

                // Clean up our communication thingy...
                m_twaincommand = null;

                // Cleanup...
                if (m_autoreseteventCaller != null)
                {
                    m_autoreseteventCaller.Close();
                    m_autoreseteventCaller = null;
                }
                if (m_autoreseteventThread != null)
                {
                    m_autoreseteventThread.Close();
                    m_autoreseteventThread = null;
                }
                if (m_autoreseteventRollback != null)
                {
                    m_autoreseteventRollback.Close();
                    m_autoreseteventRollback = null;
                }
                if (m_autoreseteventThreadStarted != null)
                {
                    m_autoreseteventThreadStarted.Close();
                    m_autoreseteventThreadStarted = null;
                }
            }
        }

        /// <summary>
        /// This is our main loop where we issue commands to the TWAIN
        /// object on behalf of the caller.  This function runs in its
        /// own thread...
        /// </summary>
        private void Main()
        {
            bool blRunning;
            bool blScanning;
            long lIndex;
            ThreadData threaddata;

            // Okay, we're ready to run...
            m_autoreseteventThreadStarted.Set();
            Log.Info("main>>> thread started...");

            //
            // We have three different ways of driving the TWAIN driver...
            //
            // First, we can accept a direct command from the user for commands
            // that move from state 2 to state 4, and for any commands that are
            // issued in state 4.
            //
            // Second, we have a scanning callback function, that operates when
            // we are transferring images; this means that we don't want the
            // user making those calls directly.
            //
            // Third, we have a rollback function, that allows the calls to
            // move anywhere from state 7 to state 2; what this means is that we
            // don't want the user making those calls directly.
            //
            // The trick is to move smoothly between these three styles of
            // access, and what we find is that the first and second are pretty
            // easy to do, but the third one is tricky...
            //
            blRunning = true;
            blScanning = false;
            while (blRunning)
            {
                // Get the next item, if we don't have anything, then we may
                // need to wait...
                if (!m_twaincommand.GetNext(out lIndex, out threaddata))
                {
                    // If we're not scanning, then wait for a command to wake
                    // us up...
                    if (!blScanning)
                    {
                        CallerToThreadWaitOne();
                        m_twaincommand.GetNext(out lIndex, out threaddata);
                    }
                }

                // Leave...now...
                if (threaddata.blExitThread)
                {
                    m_twaincommand.Complete(lIndex, threaddata);
                    m_scancallback(true);
                    ThreadToCallerSet();
                    ThreadToRollbackSet();
                    return;
                }

                // Process device events...
                if (IsMsgDeviceEvent())
                {
                    m_deviceeventcallback();
                }

                // We don't have a direct command, it's either a rollback request,
                // a request to run the scan callback, or its a false positive,
                // which we can safely ignore...
                if (threaddata.dat == default(DataArgumentTypes))
                {
                    // The caller has asked us to rollback the state machine...
                    if (threaddata.blRollback)
                    {
                        threaddata.blRollback = false;
                        Rollback(threaddata.stateRollback);
                        blScanning = (threaddata.stateRollback >= STATE.S5);
                        blRunning = (threaddata.stateRollback > STATE.S2);
                        if (!blRunning)
                        {
                            m_scancallback(true);
                        }
                        ThreadToRollbackSet();
                    }

                    // Callback stuff here between MSG_ENABLEDS* and MSG_DISABLEDS...
                    else if (GetState() >= STATE.S5)
                    {
                        m_scancallback(false);
                        blScanning = true;
                    }

                    // We're done scanning...
                    else
                    {
                        blScanning = false;
                    }

                    // Tag the command as complete...
                    m_twaincommand.Complete(lIndex, threaddata);

                    // Go back to the top...
                    continue;
                }

                // Otherwise, directly issue the command...
                switch (threaddata.dat)
                {
                    // Unrecognized DAT...
                    default:
                        if (m_state < STATE.S4)
                        {
                            threaddata.sts = DsmEntryNullDest(threaddata.dg, threaddata.dat, threaddata.msg, threaddata.twmemref);
                        }
                        else
                        {
                            threaddata.sts = DsmEntry(threaddata.dg, threaddata.dat, threaddata.msg, threaddata.twmemref);
                        }
                        break;

                    // Audio file xfer...
                    case DataArgumentTypes.AUDIOFILEXFER:
                        threaddata.sts = DatAudiofilexfer(threaddata.dg, threaddata.msg);
                        break;

                    // Audio info...
                    case DataArgumentTypes.AUDIOINFO:
                        threaddata.sts = DatAudioinfo(threaddata.dg, threaddata.msg, ref threaddata.twaudioinfo);
                        break;

                    // Audio native xfer...
                    case DataArgumentTypes.AUDIONATIVEXFER:
                        threaddata.sts = DatAudionativexfer(threaddata.dg, threaddata.msg, ref threaddata.intptrAudio);
                        break;

                    // Negotiation commands...
                    case DataArgumentTypes.CAPABILITY:
                        threaddata.sts = DatCapability(threaddata.dg, threaddata.msg, ref threaddata.twcapability);
                        break;

                    // CIE color...
                    case DataArgumentTypes.CIECOLOR:
                        threaddata.sts = DatCiecolor(threaddata.dg, threaddata.msg, ref threaddata.twciecolor);
                        break;

                    // Snapshots...
                    case DataArgumentTypes.CUSTOMDSDATA:
                        threaddata.sts = DatCustomdsdata(threaddata.dg, threaddata.msg, ref threaddata.twcustomdsdata);
                        break;

                    // Functions...
                    case DataArgumentTypes.ENTRYPOINT:
                        threaddata.sts = DatEntrypoint(threaddata.dg, threaddata.msg, ref threaddata.twentrypoint);
                        break;

                    // Image meta data...
                    case DataArgumentTypes.EXTIMAGEINFO:
                        threaddata.sts = DatExtimageinfo(threaddata.dg, threaddata.msg, ref threaddata.twextimageinfo);
                        break;

                    // Filesystem...
                    case DataArgumentTypes.FILESYSTEM:
                        threaddata.sts = DatFilesystem(threaddata.dg, threaddata.msg, ref threaddata.twfilesystem);
                        break;

                    // Filter...
                    case DataArgumentTypes.FILTER:
                        threaddata.sts = DatFilter(threaddata.dg, threaddata.msg, ref threaddata.twfilter);
                        break;

                    // Grayscale...
                    case DataArgumentTypes.GRAYRESPONSE:
                        threaddata.sts = DatGrayresponse(threaddata.dg, threaddata.msg, ref threaddata.twgrayresponse);
                        break;

                    // ICC color profiles...
                    case DataArgumentTypes.ICCPROFILE:
                        threaddata.sts = DatIccprofile(threaddata.dg, threaddata.msg, ref threaddata.twmemory);
                        break;

                    // Enumerate and Open commands...
                    case DataArgumentTypes.IDENTITY:
                        threaddata.sts = DatIdentity(threaddata.dg, threaddata.msg, ref threaddata.twidentity);
                        break;

                    // More meta data...
                    case DataArgumentTypes.IMAGEINFO:
                        threaddata.sts = DatImageinfo(threaddata.dg, threaddata.msg, ref threaddata.twimageinfo);
                        break;

                    // File xfer...
                    case DataArgumentTypes.IMAGEFILEXFER:
                        threaddata.sts = DatImagefilexfer(threaddata.dg, threaddata.msg);
                        break;

                    // Image layout commands...
                    case DataArgumentTypes.IMAGELAYOUT:
                        threaddata.sts = DatImagelayout(threaddata.dg, threaddata.msg, ref threaddata.twimagelayout);
                        break;

                    // Memory file transfer (yes, we're using TW_IMAGEMEMXFER, that's okay)...
                    case DataArgumentTypes.IMAGEMEMFILEXFER:
                        threaddata.sts = DatImagememfilexfer(threaddata.dg, threaddata.msg, ref threaddata.twimagememxfer);
                        break;

                    // Memory transfer...
                    case DataArgumentTypes.IMAGEMEMXFER:
                        threaddata.sts = DatImagememxfer(threaddata.dg, threaddata.msg, ref threaddata.twimagememxfer);
                        break;

                    // Native transfer...
                    case DataArgumentTypes.IMAGENATIVEXFER:
                        if (threaddata.blUseBitmapHandle)
                        {
                            threaddata.sts = DatImagenativexferHandle(threaddata.dg, threaddata.msg, ref threaddata.intptrBitmap);
                        }
                        else
                        {
                            threaddata.sts = DatImagenativexfer(threaddata.dg, threaddata.msg, ref threaddata.bitmap);
                        }
                        break;

                    // JPEG compression...
                    case DataArgumentTypes.JPEGCOMPRESSION:
                        threaddata.sts = DatJpegcompression(threaddata.dg, threaddata.msg, ref threaddata.twjpegcompression);
                        break;

                    // Metrics...
                    case DataArgumentTypes.METRICS:
                        threaddata.sts = DatMetrics(threaddata.dg, threaddata.msg, ref threaddata.twmetrics);
                        break;

                    // Palette8...
                    case DataArgumentTypes.PALETTE8:
                        threaddata.sts = DatPalette8(threaddata.dg, threaddata.msg, ref threaddata.twpalette8);
                        break;

                    // DSM commands...
                    case DataArgumentTypes.PARENT:
                        threaddata.sts = DatParent(threaddata.dg, threaddata.msg, ref threaddata.intptrHwnd);
                        break;

                    // Raw commands...
                    case DataArgumentTypes.PASSTHRU:
                        threaddata.sts = DatPassthru(threaddata.dg, threaddata.msg, ref threaddata.twpassthru);
                        break;

                    // Pending transfers...
                    case DataArgumentTypes.PENDINGXFERS:
                        threaddata.sts = DatPendingxfers(threaddata.dg, threaddata.msg, ref threaddata.twpendingxfers);
                        break;

                    // RGB...
                    case DataArgumentTypes.RGBRESPONSE:
                        threaddata.sts = DatRgbresponse(threaddata.dg, threaddata.msg, ref threaddata.twrgbresponse);
                        break;

                    // Setup file transfer...
                    case DataArgumentTypes.SETUPFILEXFER:
                        threaddata.sts = DatSetupfilexfer(threaddata.dg, threaddata.msg, ref threaddata.twsetupfilexfer);
                        break;

                    // Get memory info...
                    case DataArgumentTypes.SETUPMEMXFER:
                        threaddata.sts = DatSetupmemxfer(threaddata.dg, threaddata.msg, ref threaddata.twsetupmemxfer);
                        break;

                    // Status...
                    case DataArgumentTypes.STATUS:
                        threaddata.sts = DatStatus(threaddata.dg, threaddata.msg, ref threaddata.twstatus);
                        break;

                    // Status text...
                    case DataArgumentTypes.STATUSUTF8:
                        threaddata.sts = DatStatusutf8(threaddata.dg, threaddata.msg, ref threaddata.twstatusutf8);
                        break;

                    // TWAIN Direct...
                    case DataArgumentTypes.TWAINDIRECT:
                        threaddata.sts = DatTwaindirect(threaddata.dg, threaddata.msg, ref threaddata.twtwaindirect);
                        break;

                    // Scan and GUI commands...
                    case DataArgumentTypes.USERINTERFACE:
                        threaddata.sts = DatUserinterface(threaddata.dg, threaddata.msg, ref threaddata.twuserinterface);
                        if (threaddata.sts == STS.SUCCESS)
                        {
                            if ((threaddata.dg == DataGroups.CONTROL) && (threaddata.dat == DataArgumentTypes.USERINTERFACE) && (threaddata.msg == Messages.DISABLEDS))
                            {
                                blScanning = false;
                            }
                            else if ((threaddata.dg == DataGroups.CONTROL) && (threaddata.dat == DataArgumentTypes.USERINTERFACE) && (threaddata.msg == Messages.DISABLEDS))
                            {
                                if (threaddata.twuserinterface.ShowUI == 0)
                                {
                                    blScanning = true;
                                }
                            }
                        }
                        break;

                    // Transfer group...
                    case DataArgumentTypes.XFERGROUP:
                        threaddata.sts = DatXferGroup(threaddata.dg, threaddata.msg, ref threaddata.twuint32);
                        break;
                }

                // Report to the caller that we're done, and loop back up for another...
                m_twaincommand.Complete(lIndex, threaddata);
                ThreadToCallerSet();
            }

            // Some insurance to make sure we loosen up the caller...
            m_scancallback(true);
            ThreadToCallerSet();
            return;
        }

        /// <summary>
        /// Use an event message to set the appropriate flags...
        /// </summary>
        /// <param name="a_msg">Message to process</param>
        private void ProcessEvent(Messages a_msg)
        {
            switch (a_msg)
            {
                // Do nothing...
                default:
                    break;

                // If we're in state 5, then go to state 6...
                case Messages.XFERREADY:
                    if (m_blAcceptXferReady)
                    {
                        // Protect us from driver's that spam this event...
                        m_blAcceptXferReady = false;

                        // We're still processing DAT_USERINTERFACE, that's a kick the
                        // teeth.  We can't wait for it here, so launch a thread to wait
                        // for it to finish, so we can go to the next state as soon as
                        // it's done...
                        if (m_blRunningDatUserinterface)
                        {
                            m_threadRunningDatUserinterface = new Thread(RunningDatUserinterface);
                            m_threadRunningDatUserinterface.Start();
                            return;
                        }

                        // Change our state...
                        m_state = STATE.S6;
                        m_blIsMsgxferready = true;
                        CallerToThreadSet();

                        // Kick off the scan engine...
                        if (m_scancallback != null)
                        {
                            m_scancallback(false);
                        }
                    }
                    break;

                // The cancel button was pressed...
                case Messages.CLOSEDSREQ:
                    m_blIsMsgclosedsreq = true;
                    CallerToThreadSet();
                    if (m_scancallback != null)
                    {
                        m_scancallback(false);
                    }
                    break;

                // The OK button was pressed...
                case Messages.CLOSEDSOK:
                    m_blIsMsgclosedsok = true;
                    CallerToThreadSet();
                    if (m_scancallback != null)
                    {
                        m_scancallback(false);
                    }
                    break;

                // A device event arrived...
                case Messages.DEVICEEVENT:
                    m_blIsMsgdeviceevent = true;
                    CallerToThreadSet();
                    break;
            }
        }

        /// <summary>
        /// As long as DAT_USERINTERFACE is running we'll spin here...
        /// </summary>
        private void RunningDatUserinterface()
        {
            // Wait until something kicks us out...
            while ((m_state >= STATE.S4) && m_blRunningDatUserinterface)
            {
                Thread.Sleep(20);
            }

            // If we never made it to state 5, then bail...
            if (m_state < STATE.S5)
            {
                return;
            }

            // Bump up our state...
            m_state = STATE.S6;
            m_blIsMsgxferready = true;
            CallerToThreadSet();

            // Kick off the scan engine...
            if (m_scancallback != null)
            {
                m_scancallback(false);
            }
        }

        /// <summary>
        /// TWAIN needs help, if we want it to run stuff in our main
        /// UI thread...
        /// </summary>
        /// <param name="code">the code to run</param>
        private void RunInUiThread(Action a_action)
        {
            m_runinuithreaddelegate(a_action);
        }

        /// <summary>
        /// The caller is asking the thread to wake-up...
        /// </summary>
        private void CallerToThreadSet()
        {
            m_autoreseteventCaller.Set();
        }

        /// <summary>
        /// The thread is waiting for the caller to wake it...
        /// </summary>
        private bool CallerToThreadWaitOne()
        {
            return (m_autoreseteventCaller.WaitOne());
        }

        /// <summary>
        /// The common start to every capability csv...
        /// </summary>
        /// <param name="a_cap">Capability number</param>
        /// <param name="a_twon">Container</param>
        /// <param name="a_twty">Data type</param>
        /// <returns></returns>
        private CSV Common(Capabilities a_cap, TWON a_twon, TWTY a_twty)
        {
            CSV csv = new CSV();

            // Add the capability...
            string szCap = a_cap.ToString();
            if (!szCap.Contains("_"))
            {
                szCap = "0x" + ((ushort)a_cap).ToString("X");
            }

            // Build the CSV...
            csv.Add(szCap);
            csv.Add("TWON_" + a_twon);
            csv.Add("TWTY_" + a_twty);

            // And return it...
            return (csv);
        }

        /// <summary>
        /// Has a device event arrived?  Make sure to clear it, because
        /// we can get many of these.  We don't have to worry about a
        /// race condition, because the caller is expected to drain the
        /// driver of all events.
        /// </summary>
        /// <returns>True if a device event is pending</returns>
        private bool IsMsgDeviceEvent()
        {
            if (m_blIsMsgdeviceevent)
            {
                m_blIsMsgdeviceevent = false;
                return (true);
            }
            return (false);
        }

        /// <summary>
        /// The thread is asking the caller to wake-up...
        /// </summary>
        private void ThreadToCallerSet()
        {
            m_autoreseteventThread.Set();
        }

        /// <summary>
        /// The caller is waiting for the thread to wake it...
        /// </summary>
        /// <returns>Result of the wait</returns>
        private bool ThreadToCallerWaitOne()
        {
            return (m_autoreseteventThread.WaitOne());
        }

        /// <summary>
        /// The thread is asking the rollback to wake-up...
        /// </summary>
        private void ThreadToRollbackSet()
        {
            m_autoreseteventRollback.Set();
        }

        /// <summary>
        /// The rollback is waiting for the thread to wake it...
        /// </summary>
        /// <returns>Result of the wait</returns>
        private bool ThreadToRollbackWaitOne()
        {
            return (m_autoreseteventRollback.WaitOne());
        }

        /// <summary>
        /// Automatically collect the condition code for TWRC_FAILURE's...
        /// </summary>
        /// <param name="a_sts">The return code from the last operation</param>
        /// <param name="a_sts">The return code from the last operation</param>
        /// <returns>The final statue return</returns>
        private STS AutoDatStatus(STS a_sts)
        {
            STS sts;
            TW_STATUS twstatus = new TW_STATUS();

            // Automatic system is off, or the status is not TWRC_FAILURE, so just return the status we got...
            if (!m_blAutoDatStatus || (a_sts != STS.FAILURE))
            {
                return (a_sts);
            }

            // Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        if (GetState() <= STATE.S3)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryStatusState3(ref m_twidentitylegacyApp, IntPtr.Zero, DataGroups.CONTROL, DataArgumentTypes.STATUS, Messages.GET, ref twstatus);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryStatus(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, DataGroups.CONTROL, DataArgumentTypes.STATUS, Messages.GET, ref twstatus);
                        }
                    }
                    else
                    {
                        if (GetState() <= STATE.S3)
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryStatusState3(ref m_twidentitylegacyApp, IntPtr.Zero, DataGroups.CONTROL, DataArgumentTypes.STATUS, Messages.GET, ref twstatus);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryStatus(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, DataGroups.CONTROL, DataArgumentTypes.STATUS, Messages.GET, ref twstatus);
                        }
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.Error("Driver crash...");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (ms_platform == Platform.LINUX)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        if (GetState() <= STATE.S3)
                        {
                            sts = (STS)NativeMethods.Linux64DsmEntryStatusState3(ref m_twidentitylegacyApp, IntPtr.Zero, DataGroups.CONTROL, DataArgumentTypes.STATUS, Messages.GET, ref twstatus);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.Linux64DsmEntryStatus(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, DataGroups.CONTROL, DataArgumentTypes.STATUS, Messages.GET, ref twstatus);
                        }
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        if (GetState() <= STATE.S3)
                        {
                            sts = (STS)NativeMethods.LinuxDsmEntryStatusState3(ref m_twidentitylegacyApp, IntPtr.Zero, DataGroups.CONTROL, DataArgumentTypes.STATUS, Messages.GET, ref twstatus);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.LinuxDsmEntryStatus(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, DataGroups.CONTROL, DataArgumentTypes.STATUS, Messages.GET, ref twstatus);
                        }
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        if (GetState() <= STATE.S3)
                        {
                            sts = (STS)NativeMethods.Linux020302Dsm64bitEntryStatusState3(ref m_twidentityApp, IntPtr.Zero, DataGroups.CONTROL, DataArgumentTypes.STATUS, Messages.GET, ref twstatus);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.Linux020302Dsm64bitEntryStatus(ref m_twidentityApp, ref m_twidentityDs, DataGroups.CONTROL, DataArgumentTypes.STATUS, Messages.GET, ref twstatus);
                        }
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.Error("Driver crash...");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (ms_platform == Platform.MACOSX)
            {
                // Issue the command...
                try
                {
                    if (GetState() <= STATE.S3)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryStatusState3(ref m_twidentitymacosxApp, IntPtr.Zero, DataGroups.CONTROL, DataArgumentTypes.STATUS, Messages.GET, ref twstatus);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryStatus(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, DataGroups.CONTROL, DataArgumentTypes.STATUS, Messages.GET, ref twstatus);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.Error("Driver crash...");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.Assert("Unsupported platform..." + ms_platform);
                return (STS.BUMMER);
            }

            // Uh-oh, the status call failed...
            if (sts != STS.SUCCESS)
            {
                return (a_sts);
            }

            // All done...
            return ((STS)(STSCC + twstatus.ConditionCode));
        }

        /// <summary>
        /// 32-bit or 64-bit...
        /// </summary>
        /// <returns>Number of bits in the machine word for this process</returns>
        public static int GetMachineWordBitSize()
        {
            return ((IntPtr.Size == 4) ? 32 : 64);
        }

        /// <summary>
        /// Quick access to our platform id...
        /// </summary>
        /// <returns></returns>
        public static Platform GetPlatform()
        {
            // First pass...
            if (ms_blFirstPassGetPlatform)
            {
                // Dont'c come in here again...
                ms_blFirstPassGetPlatform = false;

                // We're Windows...
                if (Environment.OSVersion.ToString().Contains("Microsoft Windows"))
                {
                    ms_platform = Platform.WINDOWS;
                    ms_processor = (GetMachineWordBitSize() == 64) ? Processor.X86_64 : Processor.X86;
                }

                // We're Mac OS X (this has to come before LINUX!!!)...
                else if (Directory.Exists("/Library/Application Support"))
                {
                    ms_platform = Platform.MACOSX;
                    ms_processor = (GetMachineWordBitSize() == 64) ? Processor.X86_64 : Processor.X86;
                }

                // We're Linux...
                else if (Environment.OSVersion.ToString().Contains("Unix"))
                {
                    string szProcessor = "";
                    ms_platform = Platform.LINUX;
                    try
                    {
                        Process process = new Process()
                        {
                            StartInfo = new ProcessStartInfo()
                            {
                                FileName = "uname",
                                Arguments = "-m",
                                UseShellExecute = false,
                                RedirectStandardOutput = true,
                                CreateNoWindow = true
                            }
                        };
                        process.Start();
                        process.WaitForExit();
                        szProcessor = process.StandardOutput.ReadToEnd();
                        process.Close();
                    }
                    catch
                    {
                        Console.Out.WriteLine("Oh dear, this isn't good...where's uname?");
                    }
                    if (szProcessor.Contains("mips64"))
                    {
                        ms_processor = Processor.MIPS64EL;
                    }
                    else
                    {
                        ms_processor = (GetMachineWordBitSize() == 64) ? Processor.X86_64 : Processor.X86;
                    }
                }

                // We have a problem, Log will throw for us...
                else
                {
                    ms_platform = Platform.UNKNOWN;
                    ms_processor = Processor.UNKNOWN;
                    Log.Assert("Unsupported platform..." + ms_platform);
                }
            }

            // All done...
            return (ms_platform);
        }

        /// <summary>
        /// Quick access to our processor id...
        /// </summary>
        /// <returns></returns>
        public static Processor GetProcessor()
        {
            // First pass...
            if (ms_blFirstPassGetPlatform)
            {
                GetPlatform();
            }

            // All done...
            return (ms_processor);
        }

        /// <summary>
        /// Convert the contents of a capability to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twty">Data type</param>
        /// <param name="a_intptr">Pointer to the data</param>
        /// <param name="a_iIndex">Index of the item in the data</param>
        /// <returns>Data in CSV form</returns>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust", Unrestricted = false)]
        public string GetIndexedItem(TW_CAPABILITY a_twcapability, TWTY a_twty, IntPtr a_intptr, int a_iIndex)
        {
            IntPtr intptr;

            // Index by type...
            switch (a_twty)
            {
                default:
                    return ("Get Capability: (unrecognized item type)..." + a_twty);

                case TWTY.INT8:
                    {
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(1 * a_iIndex));
                        sbyte i8Value = (sbyte)Marshal.PtrToStructure(intptr, typeof(sbyte));
                        return (i8Value.ToString());
                    }

                case TWTY.INT16:
                    {
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(2 * a_iIndex));
                        short i16Value = (short)Marshal.PtrToStructure(intptr, typeof(short));
                        return (i16Value.ToString());
                    }

                case TWTY.INT32:
                    {
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(4 * a_iIndex));
                        int i32Value = (int)Marshal.PtrToStructure(intptr, typeof(int));
                        return (i32Value.ToString());
                    }

                case TWTY.UINT8:
                    {
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(1 * a_iIndex));
                        byte u8Value = (byte)Marshal.PtrToStructure(intptr, typeof(byte));
                        return (u8Value.ToString());
                    }

                case TWTY.BOOL:
                case TWTY.UINT16:
                    {
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(2 * a_iIndex));
                        ushort u16Value = (ushort)Marshal.PtrToStructure(intptr, typeof(ushort));
                        return (u16Value.ToString());
                    }

                case TWTY.UINT32:
                    {
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(4 * a_iIndex));
                        uint u32Value = (uint)Marshal.PtrToStructure(intptr, typeof(uint));
                        return (u32Value.ToString());
                    }

                case TWTY.FIX32:
                    {
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(4 * a_iIndex));
                        TW_FIX32 twfix32 = (TW_FIX32)Marshal.PtrToStructure(intptr, typeof(TW_FIX32));
                        return (((double)twfix32.Whole + ((double)twfix32.Frac / 65536.0)).ToString());
                    }

                case TWTY.FRAME:
                    {
                        CSV csv = new CSV();
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(16 * a_iIndex));
                        TW_FRAME twframe = (TW_FRAME)Marshal.PtrToStructure(intptr, typeof(TW_FRAME));
                        csv.Add(((double)twframe.Left.Whole + ((double)twframe.Left.Frac / 65536.0)).ToString());
                        csv.Add(((double)twframe.Top.Whole + ((double)twframe.Top.Frac / 65536.0)).ToString());
                        csv.Add(((double)twframe.Right.Whole + ((double)twframe.Right.Frac / 65536.0)).ToString());
                        csv.Add(((double)twframe.Bottom.Whole + ((double)twframe.Bottom.Frac / 65536.0)).ToString());
                        return (csv.Get());
                    }

                case TWTY.STR32:
                    {
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(34 * a_iIndex));
                        TW_STR32 twstr32 = (TW_STR32)Marshal.PtrToStructure(intptr, typeof(TW_STR32));
                        return (twstr32.Get());
                    }

                case TWTY.STR64:
                    {
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(66 * a_iIndex));
                        TW_STR64 twstr64 = (TW_STR64)Marshal.PtrToStructure(intptr, typeof(TW_STR64));
                        return (twstr64.Get());
                    }

                case TWTY.STR128:
                    {
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(130 * a_iIndex));
                        TW_STR128 twstr128 = (TW_STR128)Marshal.PtrToStructure(intptr, typeof(TW_STR128));
                        return (twstr128.Get());
                    }

                case TWTY.STR255:
                    {
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(256 * a_iIndex));
                        TW_STR255 twstr255 = (TW_STR255)Marshal.PtrToStructure(intptr, typeof(TW_STR255));
                        return (twstr255.Get());
                    }
            }
        }

        /// <summary>
        /// Convert the value of a string into a capability...
        /// </summary>
        /// <param name="a_twcapability">All info on the capability</param>
        /// <param name="a_twty">Data type</param>
        /// <param name="a_intptr">Point to the data</param>
        /// <param name="a_iIndex">Index for item in the data</param>
        /// <param name="a_szValue">CSV value to be used to set the data</param>
        /// <returns>Empty string or an error string</returns>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust", Unrestricted = false)]
        public string SetIndexedItem(TW_CAPABILITY a_twcapability, TWTY a_twty, IntPtr a_intptr, int a_iIndex, string a_szValue)
        {
            IntPtr intptr;

            // Index by type...
            switch (a_twty)
            {
                default:
                    return ("Set Capability: (unrecognized item type)..." + a_twty);

                case TWTY.INT8:
                    {
                        // We do this to make sure the entire Item value is overwritten...
                        if (a_twcapability.ConType == TWON.ONEVALUE)
                        {
                            int i32Value = sbyte.Parse(CvtCapValueFromEnum(a_twcapability.Cap, a_szValue));
                            Marshal.StructureToPtr(i32Value, a_intptr, true);
                            return ("");
                        }
                        // These items have to be packed on the type sizes...
                        else
                        {
                            sbyte i8Value = sbyte.Parse(CvtCapValueFromEnum(a_twcapability.Cap, a_szValue));
                            intptr = (IntPtr)((ulong)a_intptr + (ulong)(1 * a_iIndex));
                            Marshal.StructureToPtr(i8Value, intptr, true);
                            return ("");
                        }
                    }

                case TWTY.INT16:
                    {
                        // We use i32Value to make sure the entire Item value is overwritten...
                        if (a_twcapability.ConType == TWON.ONEVALUE)
                        {
                            int i32Value = short.Parse(CvtCapValueFromEnum(a_twcapability.Cap, a_szValue));
                            Marshal.StructureToPtr(i32Value, a_intptr, true);
                            return ("");
                        }
                        // These items have to be packed on the type sizes...
                        else
                        {
                            short i16Value = short.Parse(CvtCapValueFromEnum(a_twcapability.Cap, a_szValue));
                            intptr = (IntPtr)((ulong)a_intptr + (ulong)(2 * a_iIndex));
                            Marshal.StructureToPtr(i16Value, intptr, true);
                            return ("");
                        }
                    }

                case TWTY.INT32:
                    {
                        // Entire value will always be overwritten, so we don't have to get fancy...
                        int i32Value = int.Parse(CvtCapValueFromEnum(a_twcapability.Cap, a_szValue));
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(4 * a_iIndex));
                        Marshal.StructureToPtr(i32Value, intptr, true);
                        return ("");
                    }

                case TWTY.UINT8:
                    {
                        // We use u32Value to make sure the entire Item value is overwritten...
                        if (a_twcapability.ConType == TWON.ONEVALUE)
                        {
                            uint u32Value = byte.Parse(CvtCapValueFromEnum(a_twcapability.Cap, a_szValue));
                            Marshal.StructureToPtr(u32Value, a_intptr, true);
                            return ("");
                        }
                        // These items have to be packed on the type sizes...
                        else
                        {
                            byte u8Value = byte.Parse(CvtCapValueFromEnum(a_twcapability.Cap, a_szValue));
                            intptr = (IntPtr)((ulong)a_intptr + (ulong)(1 * a_iIndex));
                            Marshal.StructureToPtr(u8Value, intptr, true);
                            return ("");
                        }
                    }

                case TWTY.BOOL:
                case TWTY.UINT16:
                    {
                        // We use u32Value to make sure the entire Item value is overwritten...
                        if (a_twcapability.ConType == TWON.ONEVALUE)
                        {
                            uint u32Value = ushort.Parse(CvtCapValueFromEnum(a_twcapability.Cap, a_szValue));
                            Marshal.StructureToPtr(u32Value, a_intptr, true);
                            return ("");
                        }
                        else
                        {
                            ushort u16Value = ushort.Parse(CvtCapValueFromEnum(a_twcapability.Cap, a_szValue));
                            intptr = (IntPtr)((ulong)a_intptr + (ulong)(2 * a_iIndex));
                            Marshal.StructureToPtr(u16Value, intptr, true);
                            return ("");
                        }
                    }

                case TWTY.UINT32:
                    {
                        // Entire value will always be overwritten, so we don't have to get fancy...
                        uint u32Value = uint.Parse(CvtCapValueFromEnum(a_twcapability.Cap, a_szValue));
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(4 * a_iIndex));
                        Marshal.StructureToPtr(u32Value, intptr, true);
                        return ("");
                    }

                case TWTY.FIX32:
                    {
                        // Entire value will always be overwritten, so we don't have to get fancy...
                        TW_FIX32 twfix32 = default(TW_FIX32);
                        twfix32.Whole = (short)Convert.ToDouble(a_szValue);
                        twfix32.Frac = (ushort)((Convert.ToDouble(a_szValue) - (double)twfix32.Whole) * 65536.0);
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(4 * a_iIndex));
                        Marshal.StructureToPtr(twfix32, intptr, true);
                        return ("");
                    }

                case TWTY.FRAME:
                    {
                        TW_FRAME twframe = default(TW_FRAME);
                        string[] asz = CSV.Parse(a_szValue);
                        twframe.Left.Whole = (short)Convert.ToDouble(asz[0]);
                        twframe.Left.Frac = (ushort)((Convert.ToDouble(asz[0]) - (double)twframe.Left.Whole) * 65536.0);
                        twframe.Top.Whole = (short)Convert.ToDouble(asz[1]);
                        twframe.Top.Frac = (ushort)((Convert.ToDouble(asz[1]) - (double)twframe.Top.Whole) * 65536.0);
                        twframe.Right.Whole = (short)Convert.ToDouble(asz[2]);
                        twframe.Right.Frac = (ushort)((Convert.ToDouble(asz[2]) - (double)twframe.Right.Whole) * 65536.0);
                        twframe.Bottom.Whole = (short)Convert.ToDouble(asz[3]);
                        twframe.Bottom.Frac = (ushort)((Convert.ToDouble(asz[3]) - (double)twframe.Bottom.Whole) * 65536.0);
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(16 * a_iIndex));
                        Marshal.StructureToPtr(twframe, intptr, true);
                        return ("");
                    }

                case TWTY.STR32:
                    {
                        TW_STR32 twstr32 = default(TW_STR32);
                        twstr32.Set(a_szValue);
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(34 * a_iIndex));
                        Marshal.StructureToPtr(twstr32, intptr, true);
                        return ("");
                    }

                case TWTY.STR64:
                    {
                        TW_STR64 twstr64 = default(TW_STR64);
                        twstr64.Set(a_szValue);
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(66 * a_iIndex));
                        Marshal.StructureToPtr(twstr64, intptr, true);
                        return ("");
                    }

                case TWTY.STR128:
                    {
                        TW_STR128 twstr128 = default(TW_STR128);
                        twstr128.Set(a_szValue);
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(130 * a_iIndex));
                        Marshal.StructureToPtr(twstr128, intptr, true);
                        return ("");
                    }

                case TWTY.STR255:
                    {
                        TW_STR255 twstr255 = default(TW_STR255);
                        twstr255.Set(a_szValue);
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(256 * a_iIndex));
                        Marshal.StructureToPtr(twstr255, intptr, true);
                        return ("");
                    }
            }
        }

        /// <summary>
        /// Convert strings into a range...
        /// </summary>
        /// <param name="a_twty">Data type</param>
        /// <param name="a_intptr">Pointer to the data</param>
        /// <param name="a_asz">List of strings</param>
        /// <returns>Empty string or an error string</returns>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust", Unrestricted = false)]
        public string SetRangeItem(TWTY a_twty, IntPtr a_intptr, string[] a_asz)
        {
            TW_RANGE twrange = default(TW_RANGE);
            TW_RANGE_MACOSX twrangemacosx = default(TW_RANGE_MACOSX);
            TW_RANGE_LINUX64 twrangelinux64 = default(TW_RANGE_LINUX64);
            TW_RANGE_FIX32 twrangefix32 = default(TW_RANGE_FIX32);
            TW_RANGE_FIX32_MACOSX twrangefix32macosx = default(TW_RANGE_FIX32_MACOSX);

            // Index by type...
            switch (a_twty)
            {
                default:
                    return ("Set Capability: (unrecognized item type)..." + a_twty);

                case TWTY.INT8:
                    {
                        if (ms_platform == Platform.MACOSX)
                        {
                            twrangemacosx.ItemType = (uint)a_twty;
                            twrangemacosx.MinValue = (uint)sbyte.Parse(a_asz[3]);
                            twrangemacosx.MaxValue = (uint)sbyte.Parse(a_asz[4]);
                            twrangemacosx.StepSize = (uint)sbyte.Parse(a_asz[5]);
                            twrangemacosx.DefaultValue = (uint)sbyte.Parse(a_asz[6]);
                            twrangemacosx.CurrentValue = (uint)sbyte.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangemacosx, a_intptr, true);
                        }
                        else if ((m_linuxdsm == LinuxDsm.Unknown) || (m_linuxdsm == LinuxDsm.IsLatestDsm))
                        {
                            twrange.ItemType = a_twty;
                            twrange.MinValue = (uint)sbyte.Parse(a_asz[3]);
                            twrange.MaxValue = (uint)sbyte.Parse(a_asz[4]);
                            twrange.StepSize = (uint)sbyte.Parse(a_asz[5]);
                            twrange.DefaultValue = (uint)sbyte.Parse(a_asz[6]);
                            twrange.CurrentValue = (uint)sbyte.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrange, a_intptr, true);
                        }
                        else
                        {
                            twrangelinux64.ItemType = a_twty;
                            twrangelinux64.MinValue = (uint)sbyte.Parse(a_asz[3]);
                            twrangelinux64.MaxValue = (uint)sbyte.Parse(a_asz[4]);
                            twrangelinux64.StepSize = (uint)sbyte.Parse(a_asz[5]);
                            twrangelinux64.DefaultValue = (uint)sbyte.Parse(a_asz[6]);
                            twrangelinux64.CurrentValue = (uint)sbyte.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangelinux64, a_intptr, true);
                        }
                        return ("");
                    }

                case TWTY.INT16:
                    {
                        if (ms_platform == Platform.MACOSX)
                        {
                            twrangemacosx.ItemType = (uint)a_twty;
                            twrangemacosx.MinValue = (uint)short.Parse(a_asz[3]);
                            twrangemacosx.MaxValue = (uint)short.Parse(a_asz[4]);
                            twrangemacosx.StepSize = (uint)short.Parse(a_asz[5]);
                            twrangemacosx.DefaultValue = (uint)short.Parse(a_asz[6]);
                            twrangemacosx.CurrentValue = (uint)short.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangemacosx, a_intptr, true);
                        }
                        else if ((m_linuxdsm == LinuxDsm.Unknown) || (m_linuxdsm == LinuxDsm.IsLatestDsm))
                        {
                            twrange.ItemType = a_twty;
                            twrange.MinValue = (uint)short.Parse(a_asz[3]);
                            twrange.MaxValue = (uint)short.Parse(a_asz[4]);
                            twrange.StepSize = (uint)short.Parse(a_asz[5]);
                            twrange.DefaultValue = (uint)short.Parse(a_asz[6]);
                            twrange.CurrentValue = (uint)short.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrange, a_intptr, true);
                        }
                        else
                        {
                            twrangelinux64.ItemType = a_twty;
                            twrangelinux64.MinValue = (uint)short.Parse(a_asz[3]);
                            twrangelinux64.MaxValue = (uint)short.Parse(a_asz[4]);
                            twrangelinux64.StepSize = (uint)short.Parse(a_asz[5]);
                            twrangelinux64.DefaultValue = (uint)short.Parse(a_asz[6]);
                            twrangelinux64.CurrentValue = (uint)short.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangelinux64, a_intptr, true);
                        }
                        return ("");
                    }

                case TWTY.INT32:
                    {
                        if (ms_platform == Platform.MACOSX)
                        {
                            twrangemacosx.ItemType = (uint)a_twty;
                            twrangemacosx.MinValue = (uint)int.Parse(a_asz[3]);
                            twrangemacosx.MaxValue = (uint)int.Parse(a_asz[4]);
                            twrangemacosx.StepSize = (uint)int.Parse(a_asz[5]);
                            twrangemacosx.DefaultValue = (uint)int.Parse(a_asz[6]);
                            twrangemacosx.CurrentValue = (uint)int.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangemacosx, a_intptr, true);
                        }
                        else if ((m_linuxdsm == LinuxDsm.Unknown) || (m_linuxdsm == LinuxDsm.IsLatestDsm))
                        {
                            twrange.ItemType = a_twty;
                            twrange.MinValue = (uint)int.Parse(a_asz[3]);
                            twrange.MaxValue = (uint)int.Parse(a_asz[4]);
                            twrange.StepSize = (uint)int.Parse(a_asz[5]);
                            twrange.DefaultValue = (uint)int.Parse(a_asz[6]);
                            twrange.CurrentValue = (uint)int.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrange, a_intptr, true);
                        }
                        else
                        {
                            twrangelinux64.ItemType = a_twty;
                            twrangelinux64.MinValue = (uint)int.Parse(a_asz[3]);
                            twrangelinux64.MaxValue = (uint)int.Parse(a_asz[4]);
                            twrangelinux64.StepSize = (uint)int.Parse(a_asz[5]);
                            twrangelinux64.DefaultValue = (uint)int.Parse(a_asz[6]);
                            twrangelinux64.CurrentValue = (uint)int.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangelinux64, a_intptr, true);
                        }
                        return ("");
                    }

                case TWTY.UINT8:
                    {
                        if (ms_platform == Platform.MACOSX)
                        {
                            twrangemacosx.ItemType = (uint)a_twty;
                            twrangemacosx.MinValue = (uint)byte.Parse(a_asz[3]);
                            twrangemacosx.MaxValue = (uint)byte.Parse(a_asz[4]);
                            twrangemacosx.StepSize = (uint)byte.Parse(a_asz[5]);
                            twrangemacosx.DefaultValue = (uint)byte.Parse(a_asz[6]);
                            twrangemacosx.CurrentValue = (uint)byte.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangemacosx, a_intptr, true);
                        }
                        else if ((m_linuxdsm == LinuxDsm.Unknown) || (m_linuxdsm == LinuxDsm.IsLatestDsm))
                        {
                            twrange.ItemType = a_twty;
                            twrange.MinValue = (uint)byte.Parse(a_asz[3]);
                            twrange.MaxValue = (uint)byte.Parse(a_asz[4]);
                            twrange.StepSize = (uint)byte.Parse(a_asz[5]);
                            twrange.DefaultValue = (uint)byte.Parse(a_asz[6]);
                            twrange.CurrentValue = (uint)byte.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrange, a_intptr, true);
                        }
                        else
                        {
                            twrangelinux64.ItemType = a_twty;
                            twrangelinux64.MinValue = (uint)byte.Parse(a_asz[3]);
                            twrangelinux64.MaxValue = (uint)byte.Parse(a_asz[4]);
                            twrangelinux64.StepSize = (uint)byte.Parse(a_asz[5]);
                            twrangelinux64.DefaultValue = (uint)byte.Parse(a_asz[6]);
                            twrangelinux64.CurrentValue = (uint)byte.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangelinux64, a_intptr, true);
                        }
                        return ("");
                    }

                case TWTY.BOOL:
                case TWTY.UINT16:
                    {
                        if (ms_platform == Platform.MACOSX)
                        {
                            twrangemacosx.ItemType = (uint)a_twty;
                            twrangemacosx.MinValue = (uint)ushort.Parse(a_asz[3]);
                            twrangemacosx.MaxValue = (uint)ushort.Parse(a_asz[4]);
                            twrangemacosx.StepSize = (uint)ushort.Parse(a_asz[5]);
                            twrangemacosx.DefaultValue = (uint)ushort.Parse(a_asz[6]);
                            twrangemacosx.CurrentValue = (uint)ushort.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangemacosx, a_intptr, true);
                        }
                        else if ((m_linuxdsm == LinuxDsm.Unknown) || (m_linuxdsm == LinuxDsm.IsLatestDsm))
                        {
                            twrange.ItemType = a_twty;
                            twrange.MinValue = (uint)ushort.Parse(a_asz[3]);
                            twrange.MaxValue = (uint)ushort.Parse(a_asz[4]);
                            twrange.StepSize = (uint)ushort.Parse(a_asz[5]);
                            twrange.DefaultValue = (uint)ushort.Parse(a_asz[6]);
                            twrange.CurrentValue = (uint)ushort.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrange, a_intptr, true);
                        }
                        else
                        {
                            twrangelinux64.ItemType = a_twty;
                            twrangelinux64.MinValue = (uint)ushort.Parse(a_asz[3]);
                            twrangelinux64.MaxValue = (uint)ushort.Parse(a_asz[4]);
                            twrangelinux64.StepSize = (uint)ushort.Parse(a_asz[5]);
                            twrangelinux64.DefaultValue = (uint)ushort.Parse(a_asz[6]);
                            twrangelinux64.CurrentValue = (uint)ushort.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangelinux64, a_intptr, true);
                        }
                        return ("");
                    }

                case TWTY.UINT32:
                    {
                        if (ms_platform == Platform.MACOSX)
                        {
                            twrangemacosx.ItemType = (uint)a_twty;
                            twrangemacosx.MinValue = uint.Parse(a_asz[3]);
                            twrangemacosx.MaxValue = uint.Parse(a_asz[4]);
                            twrangemacosx.StepSize = uint.Parse(a_asz[5]);
                            twrangemacosx.DefaultValue = uint.Parse(a_asz[6]);
                            twrangemacosx.CurrentValue = uint.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangemacosx, a_intptr, true);
                        }
                        else if ((m_linuxdsm == LinuxDsm.Unknown) || (m_linuxdsm == LinuxDsm.IsLatestDsm))
                        {
                            twrange.ItemType = a_twty;
                            twrange.MinValue = uint.Parse(a_asz[3]);
                            twrange.MaxValue = uint.Parse(a_asz[4]);
                            twrange.StepSize = uint.Parse(a_asz[5]);
                            twrange.DefaultValue = uint.Parse(a_asz[6]);
                            twrange.CurrentValue = uint.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrange, a_intptr, true);
                        }
                        else
                        {
                            twrangelinux64.ItemType = a_twty;
                            twrangelinux64.MinValue = uint.Parse(a_asz[3]);
                            twrangelinux64.MaxValue = uint.Parse(a_asz[4]);
                            twrangelinux64.StepSize = uint.Parse(a_asz[5]);
                            twrangelinux64.DefaultValue = uint.Parse(a_asz[6]);
                            twrangelinux64.CurrentValue = uint.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangelinux64, a_intptr, true);
                        }
                        return ("");
                    }

                case TWTY.FIX32:
                    {
                        double dMinValue = Convert.ToDouble(a_asz[3]);
                        double dMaxValue = Convert.ToDouble(a_asz[4]);
                        double dStepSize = Convert.ToDouble(a_asz[5]);
                        double dDefaultValue = Convert.ToDouble(a_asz[6]);
                        double dCurrentValue = Convert.ToDouble(a_asz[7]);
                        if (ms_platform == Platform.MACOSX)
                        {
                            twrangefix32macosx.ItemType = (uint)a_twty;
                            twrangefix32macosx.MinValue.Whole = (short)dMinValue;
                            twrangefix32macosx.MinValue.Frac = (ushort)((dMinValue - (double)twrangefix32macosx.MinValue.Whole) * 65536.0);
                            twrangefix32macosx.MaxValue.Whole = (short)dMaxValue;
                            twrangefix32macosx.MaxValue.Frac = (ushort)((dMaxValue - (double)twrangefix32macosx.MaxValue.Whole) * 65536.0);
                            twrangefix32macosx.StepSize.Whole = (short)dStepSize;
                            twrangefix32macosx.StepSize.Frac = (ushort)((dStepSize - (double)twrangefix32macosx.StepSize.Whole) * 65536.0);
                            twrangefix32macosx.DefaultValue.Whole = (short)dDefaultValue;
                            twrangefix32macosx.DefaultValue.Frac = (ushort)((dDefaultValue - (double)twrangefix32macosx.DefaultValue.Whole) * 65536.0);
                            twrangefix32macosx.CurrentValue.Whole = (short)dCurrentValue;
                            twrangefix32macosx.CurrentValue.Frac = (ushort)((dCurrentValue - (double)twrangefix32macosx.CurrentValue.Whole) * 65536.0);
                            Marshal.StructureToPtr(twrangefix32macosx, a_intptr, true);
                        }
                        else
                        {
                            twrangefix32.ItemType = a_twty;
                            twrangefix32.MinValue.Whole = (short)dMinValue;
                            twrangefix32.MinValue.Frac = (ushort)((dMinValue - (double)twrangefix32.MinValue.Whole) * 65536.0);
                            twrangefix32.MaxValue.Whole = (short)dMaxValue;
                            twrangefix32.MaxValue.Frac = (ushort)((dMaxValue - (double)twrangefix32.MaxValue.Whole) * 65536.0);
                            twrangefix32.StepSize.Whole = (short)dStepSize;
                            twrangefix32.StepSize.Frac = (ushort)((dStepSize - (double)twrangefix32.StepSize.Whole) * 65536.0);
                            twrangefix32.DefaultValue.Whole = (short)dDefaultValue;
                            twrangefix32.DefaultValue.Frac = (ushort)((dDefaultValue - (double)twrangefix32.DefaultValue.Whole) * 65536.0);
                            twrangefix32.CurrentValue.Whole = (short)dCurrentValue;
                            twrangefix32.CurrentValue.Frac = (ushort)((dCurrentValue - (double)twrangefix32.CurrentValue.Whole) * 65536.0);
                            Marshal.StructureToPtr(twrangefix32, a_intptr, true);
                        }
                        return ("");
                    }
            }
        }

        /// <summary>
        /// Our callback delegate for Windows...
        /// </summary>
        /// <param name="origin">Origin of message</param>
        /// <param name="dest">Message target</param>
        /// <param name="dg">Data group</param>
        /// <param name="dat">Data argument type</param>
        /// <param name="msg">Operation</param>
        /// <param name="twnull">NULL pointer</param>
        /// <returns>TWAIN status</returns>
        private UInt16 WindowsDsmEntryCallbackProxy
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DataGroups dg,
            DataArgumentTypes dat,
            Messages msg,
            IntPtr twnull
        )
        {
            ProcessEvent(msg);
            return ((UInt16)STS.SUCCESS);
        }

        /// <summary>
        /// Our callback delegate for Linux...
        /// </summary>
        /// <param name="origin">Origin of message</param>
        /// <param name="dest">Message target</param>
        /// <param name="dg">Data group</param>
        /// <param name="dat">Data argument type</param>
        /// <param name="msg">Operation</param>
        /// <param name="twnull">NULL pointer</param>
        /// <returns>TWAIN status</returns>
        private UInt16 LinuxDsmEntryCallbackProxy
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DataGroups dg,
            DataArgumentTypes dat,
            Messages msg,
            IntPtr twnull
        )
        {
            ProcessEvent(msg);
            return ((UInt16)STS.SUCCESS);
        }

        /// <summary>
        /// Our callback delegate for Mac OS X...
        /// </summary>
        /// <param name="origin">Origin of message</param>
        /// <param name="dest">Message target</param>
        /// <param name="dg">Data group</param>
        /// <param name="dat">Data argument type</param>
        /// <param name="msg">Operation</param>
        /// <param name="twnull">NULL pointer</param>
        /// <returns>TWAIN status</returns>
        private UInt16 MacosxDsmEntryCallbackProxy
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DataGroups dg,
            DataArgumentTypes dat,
            Messages msg,
            IntPtr twnull
        )
        {
            ProcessEvent(msg);
            return ((UInt16)STS.SUCCESS);
        }

        /// <summary>
        /// Get .NET 'Bitmap' object from memory DIB via stream constructor.
        /// This should work for most DIBs.
        /// </summary>
        /// <param name="a_platform">Our operating system</param>
        /// <param name="a_intptrNative">The pointer to something (presumably a BITMAP or a TIFF image)</param>
        /// <returns>C# Bitmap of image</returns>
        private Bitmap NativeToBitmap(Platform a_platform, IntPtr a_intptrNative)
        {
            ushort u16Magic;
            IntPtr intptrNative;

            // We need the first two bytes to decide if we have a DIB or a TIFF.  Don't
            // forget to lock the silly thing...
            intptrNative = DsmMemLock(a_intptrNative);
            u16Magic = (ushort)Marshal.PtrToStructure(intptrNative, typeof(ushort));

            // Windows uses a DIB, the first usigned short is 40...
            if (u16Magic == 40)
            {
                byte[] bBitmap;
                BITMAPFILEHEADER bitmapfileheader;
                BITMAPINFOHEADER bitmapinfoheader;

                // Our incoming DIB is a bitmap info header...
                bitmapinfoheader = (BITMAPINFOHEADER)Marshal.PtrToStructure(intptrNative, typeof(BITMAPINFOHEADER));

                // Build our file header...
                bitmapfileheader = new BITMAPFILEHEADER();
                bitmapfileheader.bfType = 0x4D42; // "BM"
                bitmapfileheader.bfSize
                    = (uint)Marshal.SizeOf(typeof(BITMAPFILEHEADER)) +
                       bitmapinfoheader.biSize +
                       (bitmapinfoheader.biClrUsed * 4) +
                       bitmapinfoheader.biSizeImage;
                bitmapfileheader.bfOffBits
                    = (uint)Marshal.SizeOf(typeof(BITMAPFILEHEADER)) +
                       bitmapinfoheader.biSize +
                       (bitmapinfoheader.biClrUsed * 4);

                // Copy the file header into our byte array...
                IntPtr intptr = Marshal.AllocHGlobal(Marshal.SizeOf(bitmapfileheader));
                Marshal.StructureToPtr(bitmapfileheader, intptr, true);
                bBitmap = new byte[bitmapfileheader.bfSize];
                Marshal.Copy(intptr, bBitmap, 0, Marshal.SizeOf(bitmapfileheader));
                Marshal.FreeHGlobal(intptr);
                intptr = IntPtr.Zero;

                // Copy the rest of the DIB into our byte array......
                Marshal.Copy(intptrNative, bBitmap, Marshal.SizeOf(typeof(BITMAPFILEHEADER)), (int)bitmapfileheader.bfSize - Marshal.SizeOf(typeof(BITMAPFILEHEADER)));

                // Now we can turn the in-memory bitmap file into a Bitmap object...
                MemoryStream memorystream = new MemoryStream(bBitmap);

                // Unfortunately the stream has to be kept with the bitmap...
                Bitmap bitmapStream = new Bitmap(memorystream);

                // So we make a copy (ick)...
                Bitmap bitmap;
                switch (bitmapinfoheader.biBitCount)
                {
                    default:
                    case 24:
                        bitmap = bitmapStream.Clone(new Rectangle(0, 0, bitmapStream.Width, bitmapStream.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                        break;
                    case 8:
                        bitmap = bitmapStream.Clone(new Rectangle(0, 0, bitmapStream.Width, bitmapStream.Height), System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                        break;
                    case 1:
                        bitmap = bitmapStream.Clone(new Rectangle(0, 0, bitmapStream.Width, bitmapStream.Height), System.Drawing.Imaging.PixelFormat.Format1bppIndexed);
                        break;
                }

                // Fix the resolution...
                bitmap.SetResolution((int)(bitmap.HorizontalResolution + 0.5), (int)(bitmap.VerticalResolution + 0.5));

                // Cleanup...
                //bitmapStream.Dispose();
                //memorystream.Close();
                bitmapStream = null;
                memorystream = null;
                bBitmap = null;

                // Return our bitmap...
                DsmMemUnlock(a_intptrNative);
                return (bitmap);
            }

            // Linux and Mac OS X use TIFF.  We'll handle a simple Intel TIFF ("II")...
            else if (u16Magic == 0x4949)
            {
                int iTiffSize;
                ulong u64;
                ulong u64Pointer;
                ulong u64TiffHeaderSize;
                ulong u64TiffTagSize;
                byte[] abTiff;
                TIFFHEADER tiffheader;
                TIFFTAG tifftag;

                // Init stuff...
                tiffheader = new TIFFHEADER();
                tifftag = new TIFFTAG();
                u64TiffHeaderSize = (ulong)Marshal.SizeOf(tiffheader);
                u64TiffTagSize = (ulong)Marshal.SizeOf(tifftag);

                // Find the size of the image so we can turn it into a memory stream...
                iTiffSize = 0;
                tiffheader = (TIFFHEADER)Marshal.PtrToStructure(intptrNative, typeof(TIFFHEADER));
                for (u64 = 0; u64 < 999; u64++)
                {
                    u64Pointer = (ulong)intptrNative + u64TiffHeaderSize + (u64TiffTagSize * u64);
                    tifftag = (TIFFTAG)Marshal.PtrToStructure((IntPtr)u64Pointer, typeof(TIFFTAG));

                    // StripOffsets...
                    if (tifftag.u16Tag == 273)
                    {
                        iTiffSize += (int)tifftag.u32Value;
                    }

                    // StripByteCounts...
                    if (tifftag.u16Tag == 279)
                    {
                        iTiffSize += (int)tifftag.u32Value;
                    }
                }

                // No joy...
                if (iTiffSize == 0)
                {
                    DsmMemUnlock(a_intptrNative);
                    return (null);
                }

                // Copy the data to our byte array...
                abTiff = new byte[iTiffSize];
                Marshal.Copy(intptrNative, abTiff, 0, iTiffSize);

                // Move the image into a memory stream...
                MemoryStream memorystream = new MemoryStream(abTiff);

                // Turn the memory stream into an in-memory TIFF image...
                Image imageTiff = Image.FromStream(memorystream);

                // Convert the in-memory tiff to a Bitmap object...
                Bitmap bitmap = new Bitmap(imageTiff);

                // Cleanup...
                abTiff = null;
                memorystream = null;
                imageTiff = null;

                // Return our bitmap...
                DsmMemUnlock(a_intptrNative);
                return (bitmap);
            }

            // Uh-oh...
            DsmMemUnlock(a_intptrNative);
            return (null);
        }

        /// <summary>
        /// Get .NET 'Bitmap' object from memory DIB via stream constructor.
        /// This should work for most DIBs.
        /// </summary>
        /// <param name="a_intptrNative">The pointer to something (presumably a BITMAP or a TIFF image)</param>
        /// <returns>C# Bitmap of image</returns>
        public byte[] NativeToByteArray(IntPtr a_intptrNative, bool a_blIsHandle, out int a_iHeaderBytes)
        {
            ushort u16Magic;
            UIntPtr uintptrBytes;
            IntPtr intptrNative;

            // Init stuff...
            a_iHeaderBytes = 0;

            // Give ourselves what protection we can...
            try
            {
                // We need the first two bytes to decide if we have a DIB or a TIFF.  Don't
                // forget to lock the silly thing...
                intptrNative = a_blIsHandle ? DsmMemLock(a_intptrNative) : a_intptrNative;
                u16Magic = (ushort)Marshal.PtrToStructure(intptrNative, typeof(ushort));

                // Windows uses a DIB, the first unsigned short is 40...
                if (u16Magic == 40)
                {
                    byte[] abBitmap;
                    BITMAPFILEHEADER bitmapfileheader;
                    BITMAPINFOHEADER bitmapinfoheader;

                    // Our incoming DIB is a bitmap info header...
                    bitmapinfoheader = (BITMAPINFOHEADER)Marshal.PtrToStructure(intptrNative, typeof(BITMAPINFOHEADER));

                    // Build our file header...
                    bitmapfileheader = new BITMAPFILEHEADER();
                    bitmapfileheader.bfType = 0x4D42; // "BM"
                    bitmapfileheader.bfSize
                        = (uint)Marshal.SizeOf(typeof(BITMAPFILEHEADER)) +
                           bitmapinfoheader.biSize +
                           (bitmapinfoheader.biClrUsed * 4) +
                           bitmapinfoheader.biSizeImage;
                    bitmapfileheader.bfOffBits
                        = (uint)Marshal.SizeOf(typeof(BITMAPFILEHEADER)) +
                           bitmapinfoheader.biSize +
                           (bitmapinfoheader.biClrUsed * 4);

                    // Copy the file header into our byte array...
                    IntPtr intptr = Marshal.AllocHGlobal(Marshal.SizeOf(bitmapfileheader));
                    Marshal.StructureToPtr(bitmapfileheader, intptr, true);
                    abBitmap = new byte[bitmapfileheader.bfSize];
                    Marshal.Copy(intptr, abBitmap, 0, Marshal.SizeOf(bitmapfileheader));
                    Marshal.FreeHGlobal(intptr);
                    intptr = IntPtr.Zero;

                    // Copy the rest of the DIB into our byte array...
                    a_iHeaderBytes = (int)bitmapfileheader.bfOffBits;
                    Marshal.Copy(intptrNative, abBitmap, Marshal.SizeOf(typeof(BITMAPFILEHEADER)), (int)bitmapfileheader.bfSize - Marshal.SizeOf(typeof(BITMAPFILEHEADER)));

                    // Unlock the handle, and return our byte array...
                    if (a_blIsHandle)
                    {
                        DsmMemUnlock(a_intptrNative);
                    }
                    return (abBitmap);
                }

                // Linux and Mac OS X use TIFF.  We'll handle a simple Intel TIFF ("II")...
                else if (u16Magic == 0x4949)
                {
                    int iTiffSize;
                    ulong u64;
                    ulong u64Pointer;
                    ulong u64TiffHeaderSize;
                    ulong u64TiffTagSize;
                    byte[] abTiff;
                    TIFFHEADER tiffheader;
                    TIFFTAG tifftag;

                    // Init stuff...
                    tiffheader = new TIFFHEADER();
                    tifftag = new TIFFTAG();
                    u64TiffHeaderSize = (ulong)Marshal.SizeOf(tiffheader);
                    u64TiffTagSize = (ulong)Marshal.SizeOf(tifftag);

                    // Find the size of the image so we can turn it into a byte array...
                    iTiffSize = 0;
                    tiffheader = (TIFFHEADER)Marshal.PtrToStructure(intptrNative, typeof(TIFFHEADER));
                    for (u64 = 0; u64 < 999; u64++)
                    {
                        u64Pointer = (ulong)intptrNative + u64TiffHeaderSize + (u64TiffTagSize * u64);
                        tifftag = (TIFFTAG)Marshal.PtrToStructure((IntPtr)u64Pointer, typeof(TIFFTAG));

                        // StripOffsets...
                        if (tifftag.u16Tag == 273)
                        {
                            iTiffSize += (int)tifftag.u32Value;
                            a_iHeaderBytes = (int)tifftag.u32Value;
                        }

                        // StripByteCounts...
                        if (tifftag.u16Tag == 279)
                        {
                            iTiffSize += (int)tifftag.u32Value;
                        }
                    }

                    // No joy...
                    if (iTiffSize == 0)
                    {
                        if (a_blIsHandle)
                        {
                            DsmMemUnlock(a_intptrNative);
                        }
                        return (null);
                    }

                    // Copy the data to our byte array...
                    abTiff = new byte[iTiffSize];
                    Marshal.Copy(intptrNative, abTiff, 0, iTiffSize);

                    // Unlock the handle, and return our byte array...
                    if (a_blIsHandle)
                    {
                        DsmMemUnlock(a_intptrNative);
                    }
                    return (abTiff);
                }

                // As long as we're here, let's handle JFIF (JPEG) too,
                // this can never be a handle...
                else if (u16Magic == 0xFFD8)
                {
                    byte[] abJfif;

                    // We need the size of this memory block...
                    switch (GetPlatform())
                    {
                        default:
                            Log.Error("Really? <" + GetPlatform() + ">");
                            return (null);
                        case Platform.WINDOWS:
                            uintptrBytes = NativeMethods._msize(a_intptrNative);
                            break;
                        case Platform.LINUX:
                            uintptrBytes = NativeMethods.malloc_usable_size(a_intptrNative);
                            break;
                        case Platform.MACOSX:
                            uintptrBytes = NativeMethods.malloc_size(a_intptrNative);
                            break;
                    }
                    abJfif = new byte[(int)uintptrBytes];
                    Marshal.Copy(a_intptrNative, abJfif, 0, (int)(int)uintptrBytes);
                    return (abJfif);
                }
            }
            catch (Exception exception)
            {
                Log.Error("NativeToByteArray threw an exceptions - " + exception.Message);
            }

            // Byte-bye...
            DsmMemUnlock(a_intptrNative);
            return (null);
        }

        /// <summary>
        /// Convert a public identity to a legacy identity...
        /// </summary>
        /// <param name="a_twidentity">Identity to convert</param>
        /// <returns>Legacy form of identity</returns>
        private TW_IDENTITY_LEGACY TwidentityToTwidentitylegacy(TW_IDENTITY a_twidentity)
        {
            TW_IDENTITY_LEGACY twidentitylegacy = new TW_IDENTITY_LEGACY();
            twidentitylegacy.Id = (uint)a_twidentity.Id;
            twidentitylegacy.Manufacturer = a_twidentity.Manufacturer;
            twidentitylegacy.ProductFamily = a_twidentity.ProductFamily;
            twidentitylegacy.ProductName = a_twidentity.ProductName;
            twidentitylegacy.ProtocolMajor = a_twidentity.ProtocolMajor;
            twidentitylegacy.ProtocolMinor = a_twidentity.ProtocolMinor;
            twidentitylegacy.SupportedGroups = a_twidentity.SupportedGroups;
            twidentitylegacy.Version.Country = a_twidentity.Version.Country;
            twidentitylegacy.Version.Info = a_twidentity.Version.Info;
            twidentitylegacy.Version.Language = a_twidentity.Version.Language;
            twidentitylegacy.Version.MajorNum = a_twidentity.Version.MajorNum;
            twidentitylegacy.Version.MinorNum = a_twidentity.Version.MinorNum;
            return (twidentitylegacy);
        }

        /// <summary>
        /// Convert a public identity to a linux64 identity...
        /// </summary>
        /// <param name="a_twidentity">Identity to convert</param>
        /// <returns>Linux64 form of identity</returns>
        private TW_IDENTITY_LINUX64 TwidentityToTwidentitylinux64(TW_IDENTITY a_twidentity)
        {
            TW_IDENTITY_LINUX64 twidentitylinux64 = new TW_IDENTITY_LINUX64();
            twidentitylinux64.Id = a_twidentity.Id;
            twidentitylinux64.Manufacturer = a_twidentity.Manufacturer;
            twidentitylinux64.ProductFamily = a_twidentity.ProductFamily;
            twidentitylinux64.ProductName = a_twidentity.ProductName;
            twidentitylinux64.ProtocolMajor = a_twidentity.ProtocolMajor;
            twidentitylinux64.ProtocolMinor = a_twidentity.ProtocolMinor;
            twidentitylinux64.SupportedGroups = a_twidentity.SupportedGroups;
            twidentitylinux64.Version.Country = a_twidentity.Version.Country;
            twidentitylinux64.Version.Info = a_twidentity.Version.Info;
            twidentitylinux64.Version.Language = a_twidentity.Version.Language;
            twidentitylinux64.Version.MajorNum = a_twidentity.Version.MajorNum;
            twidentitylinux64.Version.MinorNum = a_twidentity.Version.MinorNum;
            return (twidentitylinux64);
        }

        /// <summary>
        /// Convert a public identity to a macosx identity...
        /// </summary>
        /// <param name="a_twidentity">Identity to convert</param>
        /// <returns>Mac OS X form of identity</returns>
        public static TW_IDENTITY_MACOSX TwidentityToTwidentitymacosx(TW_IDENTITY a_twidentity)
        {
            TW_IDENTITY_MACOSX twidentitymacosx = new TW_IDENTITY_MACOSX();
            twidentitymacosx.Id = (uint)a_twidentity.Id;
            twidentitymacosx.Manufacturer = a_twidentity.Manufacturer;
            twidentitymacosx.ProductFamily = a_twidentity.ProductFamily;
            twidentitymacosx.ProductName = a_twidentity.ProductName;
            twidentitymacosx.ProtocolMajor = a_twidentity.ProtocolMajor;
            twidentitymacosx.ProtocolMinor = a_twidentity.ProtocolMinor;
            twidentitymacosx.SupportedGroups = a_twidentity.SupportedGroups;
            twidentitymacosx.Version.Country = a_twidentity.Version.Country;
            twidentitymacosx.Version.Info = a_twidentity.Version.Info;
            twidentitymacosx.Version.Language = a_twidentity.Version.Language;
            twidentitymacosx.Version.MajorNum = a_twidentity.Version.MajorNum;
            twidentitymacosx.Version.MinorNum = a_twidentity.Version.MinorNum;
            return (twidentitymacosx);
        }

        /// <summary>
        /// Convert a legacy identity to a public identity...
        /// </summary>
        /// <param name="a_twidentitylegacy">Legacy identity to convert</param>
        /// <returns>Regular form of identity</returns>
        private TW_IDENTITY TwidentitylegacyToTwidentity(TW_IDENTITY_LEGACY a_twidentitylegacy)
        {
            TW_IDENTITY twidentity = new TW_IDENTITY();
            twidentity.Id = a_twidentitylegacy.Id;
            twidentity.Manufacturer = a_twidentitylegacy.Manufacturer;
            twidentity.ProductFamily = a_twidentitylegacy.ProductFamily;
            twidentity.ProductName = a_twidentitylegacy.ProductName;
            twidentity.ProtocolMajor = a_twidentitylegacy.ProtocolMajor;
            twidentity.ProtocolMinor = a_twidentitylegacy.ProtocolMinor;
            twidentity.SupportedGroups = a_twidentitylegacy.SupportedGroups;
            twidentity.Version.Country = a_twidentitylegacy.Version.Country;
            twidentity.Version.Info = a_twidentitylegacy.Version.Info;
            twidentity.Version.Language = a_twidentitylegacy.Version.Language;
            twidentity.Version.MajorNum = a_twidentitylegacy.Version.MajorNum;
            twidentity.Version.MinorNum = a_twidentitylegacy.Version.MinorNum;
            return (twidentity);
        }

        /// <summary>
        /// Convert a linux64 identity to a public identity...
        /// </summary>
        /// <param name="a_twidentitylegacy">Legacy identity to convert</param>
        /// <returns>Regular form of identity</returns>
        private TW_IDENTITY Twidentitylinux64ToTwidentity(TW_IDENTITY_LINUX64 a_twidentitylinux64)
        {
            TW_IDENTITY twidentity = new TW_IDENTITY();
            twidentity.Id = a_twidentitylinux64.Id;
            twidentity.Manufacturer = a_twidentitylinux64.Manufacturer;
            twidentity.ProductFamily = a_twidentitylinux64.ProductFamily;
            twidentity.ProductName = a_twidentitylinux64.ProductName;
            twidentity.ProtocolMajor = a_twidentitylinux64.ProtocolMajor;
            twidentity.ProtocolMinor = a_twidentitylinux64.ProtocolMinor;
            twidentity.SupportedGroups = (uint)a_twidentitylinux64.SupportedGroups;
            twidentity.Version.Country = a_twidentitylinux64.Version.Country;
            twidentity.Version.Info = a_twidentitylinux64.Version.Info;
            twidentity.Version.Language = a_twidentitylinux64.Version.Language;
            twidentity.Version.MajorNum = a_twidentitylinux64.Version.MajorNum;
            twidentity.Version.MinorNum = a_twidentitylinux64.Version.MinorNum;
            return (twidentity);
        }

        /// <summary>
        /// Convert a macosx identity to a public identity...
        /// </summary>
        /// <param name="a_twidentitymacosx">Mac OS X identity to convert</param>
        /// <returns>Regular identity</returns>
        private TW_IDENTITY TwidentitymacosxToTwidentity(TW_IDENTITY_MACOSX a_twidentitymacosx)
        {
            TW_IDENTITY twidentity = new TW_IDENTITY();
            twidentity.Id = a_twidentitymacosx.Id;
            twidentity.Manufacturer = a_twidentitymacosx.Manufacturer;
            twidentity.ProductFamily = a_twidentitymacosx.ProductFamily;
            twidentity.ProductName = a_twidentitymacosx.ProductName;
            twidentity.ProtocolMajor = a_twidentitymacosx.ProtocolMajor;
            twidentity.ProtocolMinor = a_twidentitymacosx.ProtocolMinor;
            twidentity.SupportedGroups = a_twidentitymacosx.SupportedGroups;
            twidentity.Version.Country = a_twidentitymacosx.Version.Country;
            twidentity.Version.Info = a_twidentitymacosx.Version.Info;
            twidentity.Version.Language = a_twidentitymacosx.Version.Language;
            twidentity.Version.MajorNum = a_twidentitymacosx.Version.MajorNum;
            twidentity.Version.MinorNum = a_twidentitymacosx.Version.MinorNum;
            return (twidentity);
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Private Structures, things we need for the thread, and to support stuff
        // like DAT_IMAGENATIVEXFER...
        ///////////////////////////////////////////////////////////////////////////////
        #region Private Structures...

        /// <summary>
        /// The data we share with the thread...
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable")]
        private struct ThreadData
        {
            // The state of the structure...
            public bool blIsInuse;
            public bool blIsComplete;
            public bool blExitThread;

            // Command...
            public DataGroups dg;
            public DataArgumentTypes dat;
            public Messages msg;
            public STATE stateRollback;
            public bool blRollback;

            // Payload...
            public IntPtr intptrHwnd;
            public IntPtr intptrBitmap;
            public IntPtr intptrAudio;
            public IntPtr twmemref;
            public Bitmap bitmap;
            public bool blUseBitmapHandle;
            public UInt32 twuint32;
            public TW_AUDIOINFO twaudioinfo;
            public TW_CALLBACK twcallback;
            public TW_CALLBACK2 twcallback2;
            public TW_CAPABILITY twcapability;
            public TW_CIECOLOR twciecolor;
            public TW_CUSTOMDSDATA twcustomdsdata;
            public TW_DEVICEEVENT twdeviceevent;
            public TW_ENTRYPOINT twentrypoint;
            public TW_EVENT twevent;
            public TW_EXTIMAGEINFO twextimageinfo;
            public TW_FILESYSTEM twfilesystem;
            public TW_FILTER twfilter;
            public TW_GRAYRESPONSE twgrayresponse;
            public TW_IDENTITY twidentity;
            public TW_IDENTITY_LEGACY twidentitylegacy;
            public TW_IMAGEINFO twimageinfo;
            public TW_IMAGELAYOUT twimagelayout;
            public TW_IMAGEMEMXFER twimagememxfer;
            public TW_JPEGCOMPRESSION twjpegcompression;
            public TW_METRICS twmetrics;
            public TW_MEMORY twmemory;
            public TW_PALETTE8 twpalette8;
            public TW_PASSTHRU twpassthru;
            public TW_PENDINGXFERS twpendingxfers;
            public TW_RGBRESPONSE twrgbresponse;
            public TW_SETUPFILEXFER twsetupfilexfer;
            public TW_SETUPMEMXFER twsetupmemxfer;
            public TW_STATUS twstatus;
            public TW_STATUSUTF8 twstatusutf8;
            public TW_TWAINDIRECT twtwaindirect;
            public TW_USERINTERFACE twuserinterface;

            // Result...
            public STS sts;
        }

        /// <summary>
        /// The Windows Point structure.
        /// Needed for the PreFilterMessage function when we're
        /// handling DAT_EVENT...
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        /// <summary>
        /// The Windows MSG structure.
        /// Needed for the PreFilterMessage function when we're
        /// handling DAT_EVENT...
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct MESSAGE
        {
            public IntPtr hwnd;
            public UInt32 message;
            public IntPtr wParam;
            public IntPtr lParam;
            public UInt32 time;
            public POINT pt;
        }

        /// <summary>
        /// The header for a Bitmap file.
        /// Needed for supporting DAT.IMAGENATIVEXFER...
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct BITMAPFILEHEADER
        {
            public ushort bfType;
            public uint bfSize;
            public ushort bfReserved1;
            public ushort bfReserved2;
            public uint bfOffBits;
        }

        /// <summary>
        /// The header for a Device Independent Bitmap (DIB).
        /// Needed for supporting DAT.IMAGENATIVEXFER...
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct BITMAPINFOHEADER
        {
            public uint biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;

            public void Init()
            {
                biSize = (uint)Marshal.SizeOf(this);
            }
        }

        /// <summary>
        /// The TIFF file header.
        /// Needed for supporting DAT.IMAGENATIVEXFER...
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct TIFFHEADER
        {
            public ushort u8ByteOrder;
            public ushort u16Version;
            public uint u32OffsetFirstIFD;
            public ushort u16u16IFD;
        }

        /// <summary>
        /// An individual TIFF Tag.
        /// Needed for supporting DAT.IMAGENATIVEXFER...
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct TIFFTAG
        {
            public ushort u16Tag;
            public ushort u16Type;
            public uint u32Count;
            public uint u32Value;
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // TwainCommand...
        ///////////////////////////////////////////////////////////////////////////////
        #region TwainCommand...

        /// <summary>
        /// We have TWAIN commands that can be called by the application from any
        /// thread they want.  We do a lock, and then build a command and submit
        /// it to the Main thread.  The Main thread runs without locks, so all it
        /// is allowed to do is examine TwainCommands to see if it has work.  If
        /// it finds an item, it takes care of it, and changes it to complete.
        /// </summary>
        private sealed class TwainCommand
        {
            ///////////////////////////////////////////////////////////////////////////
            // Public Functions...
            ///////////////////////////////////////////////////////////////////////////
            #region Public Functions...

            /// <summary>
            /// Initialize an array that we'll be sharing between the TWAIN operations
            /// and the Main thread...
            /// </summary>
            public TwainCommand()
            {
                m_athreaddata = new ThreadData[8];
            }

            /// <summary>
            /// Complete a command
            /// </summary>
            /// <param name="a_lIndex">index to update</param>
            /// <param name="a_threaddata">data to use</param>
            public void Complete(long a_lIndex, ThreadData a_threaddata)
            {
                // If we're out of bounds, return an empty structure...
                if ((a_lIndex < 0) || (a_lIndex >= m_athreaddata.Length))
                {
                    return;
                }

                // We're not really a command...
                if (!m_athreaddata[a_lIndex].blIsInuse)
                {
                    return;
                }

                // Do the update and tag it complete...
                m_athreaddata[a_lIndex] = a_threaddata;
                m_athreaddata[a_lIndex].blIsComplete = true;
            }

            /// <summary>
            /// Delete a command...
            /// </summary>
            /// <returns>the requested command</returns>
            public void Delete(long a_lIndex)
            {
                // If we're out of bounds, return an empty structure...
                if ((a_lIndex < 0) || (a_lIndex >= m_athreaddata.Length))
                {
                    return;
                }

                // Clear the record...
                m_athreaddata[a_lIndex] = default(ThreadData);
            }

            /// <summary>
            /// Get a command...
            /// </summary>
            /// <returns>the requested command</returns>
            public ThreadData Get(long a_lIndex)
            {
                // If we're out of bounds, return an empty structure...
                if ((a_lIndex < 0) || (a_lIndex >= m_athreaddata.Length))
                {
                    return (new ThreadData());
                }

                // Return what we found...
                return (m_athreaddata[a_lIndex]);
            }

            /// <summary>
            /// Get the next command in the list...
            /// </summary>
            /// <param name="a_lIndex">the index of the data</param>
            /// <param name="a_threaddata">the command we'll return</param>
            /// <returns>true if we found something</returns>
            public bool GetNext(out long a_lIndex, out ThreadData a_threaddata)
            {
                long lIndex;

                // Init stuff...
                lIndex = m_lIndex;
                a_lIndex = 0;
                a_threaddata = default(ThreadData);

                // Cycle once through the commands to see if we have any...
                for (; ; )
                {
                    // We found something, copy it out, point to the next
                    // item (so we know we're looking at the whole list)
                    // and return...
                    if (m_athreaddata[lIndex].blIsInuse && !m_athreaddata[lIndex].blIsComplete)
                    {
                        a_threaddata = m_athreaddata[lIndex];
                        a_lIndex = lIndex;
                        m_lIndex = lIndex + 1;
                        if (m_lIndex >= m_athreaddata.Length)
                        {
                            m_lIndex = 0;
                        }
                        return (true);
                    }

                    // Next item...
                    lIndex += 1;
                    if (lIndex >= m_athreaddata.Length)
                    {
                        lIndex = 0;
                    }

                    // We've cycled, and we didn't find anything...
                    if (lIndex == m_lIndex)
                    {
                        a_lIndex = lIndex;
                        return (false);
                    }
                }
            }

            /// <summary>
            /// Submit a new command...
            /// </summary>
            /// <returns></returns>
            public long Submit(ThreadData a_threadata)
            {
                long ll;

                // We won't leave until we've submitted the beastie...
                for (; ; )
                {
                    // Look for a free slot...
                    for (ll = 0; ll < m_athreaddata.Length; ll++)
                    {
                        if (!m_athreaddata[ll].blIsInuse)
                        {
                            m_athreaddata[ll] = a_threadata;
                            m_athreaddata[ll].blIsInuse = true;
                            return (ll);
                        }
                    }

                    // Wait a little...
                    Thread.Sleep(0);
                }
            }

            /// <summary>
            /// Update a command
            /// </summary>
            /// <param name="a_lIndex">index to update</param>
            /// <param name="a_threaddata">data to use</param>
            public void Update(long a_lIndex, ThreadData a_threaddata)
            {
                // If we're out of bounds, return an empty structure...
                if ((a_lIndex < 0) || (a_lIndex >= m_athreaddata.Length))
                {
                    return;
                }

                // We're not really a command...
                if (!m_athreaddata[a_lIndex].blIsInuse)
                {
                    return;
                }

                // Do the update...
                m_athreaddata[a_lIndex] = a_threaddata;
            }

            #endregion


            ///////////////////////////////////////////////////////////////////////////
            // Private Attributes...
            ///////////////////////////////////////////////////////////////////////////
            #region Private Attributes...

            /// <summary>
            /// The data we're sharing.  A null in a position means its available for
            /// use.  The Main thread only consumes items, it never creates or
            /// destroys them, that's done by the various commands.
            /// </summary>
            private ThreadData[] m_athreaddata;

            /// <summary>
            /// Index for browsing m_athreaddata for work...
            /// </summary>
            private long m_lIndex;

            #endregion
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////
        // Structure Definitions...
        ///////////////////////////////////////////////////////////////////////////////
        #region Structure Definitions..
        public delegate IntPtr DSM_MEMALLOC(uint size);
        public delegate void DSM_MEMFREE(IntPtr handle);
        public delegate IntPtr DSM_MEMLOCK(IntPtr handle);
        public delegate void DSM_MEMUNLOCK(IntPtr handle);
        #endregion
        ///////////////////////////////////////////////////////////////////////////////
        // Generic Constants...
        ///////////////////////////////////////////////////////////////////////////////
        #region Generic Constants...

        /// <summary>
        /// Don't care values...
        /// </summary>
        public const byte TWON_DONTCARE8 = 0xff;
        public const ushort TWON_DONTCARE16 = 0xffff;
        public const uint TWON_DONTCARE32 = 0xffffffff;
        #endregion

    }
}


