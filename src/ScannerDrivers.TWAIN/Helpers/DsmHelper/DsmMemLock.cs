using ScannerDrivers.TWAIN.Contants.Enum;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Lock memory used with the data source...
        /// </summary>
        /// <param name="a_intptrHandle">Handle to lock</param>
        /// <returns>Locked pointer</returns>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust", Unrestricted = false)]
        public IntPtr DsmMemLock(IntPtr a_intptrHandle)
        {
            // Validate...
            if (a_intptrHandle == IntPtr.Zero)
            {
                return (a_intptrHandle);
            }

            // Use the DSM...
            if (m_twentrypointdelegates.DSM_MemLock != null)
            {
                return (m_twentrypointdelegates.DSM_MemLock(a_intptrHandle));
            }

            // Do it ourselves, Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                return (NativeMethods.GlobalLock(a_intptrHandle));
            }

            // Do it ourselves, Linux...
            if (ms_platform == Platform.LINUX)
            {
                return (a_intptrHandle);
            }

            // Do it ourselves, Mac OS X...
            if (ms_platform == Platform.MACOSX)
            {
                IntPtr intptr = (IntPtr)Marshal.PtrToStructure(a_intptrHandle, typeof(IntPtr));
                return (intptr);
            }

            // Trouble, Log will throw an exception for us...
            Log.Assert("Unsupported platform..." + ms_platform);
            return (IntPtr.Zero);
        }
    }
}
