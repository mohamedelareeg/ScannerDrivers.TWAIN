using ScannerDrivers.TWAIN.Contants.Enum;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Issue native image transfer commands...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_bitmap">BITMAP structure</param>
        /// <returns>TWAIN status</returns>

        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust", Unrestricted = false)]
        public STS DatImagenativexfer(DataGroups a_dg, Messages a_msg, ref Bitmap a_bitmap)
        {
            IntPtr intptrBitmapHandle = IntPtr.Zero;
            return (DatImagenativexferBitmap(a_dg, a_msg, ref a_bitmap, ref intptrBitmapHandle, false));
        }
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust", Unrestricted = false)]
        public STS DatImagenativexferHandle(DataGroups a_dg, Messages a_msg, ref IntPtr a_intptrBitmapHandle)
        {
            Bitmap bitmap = null;
            return (DatImagenativexferBitmap(a_dg, a_msg, ref bitmap, ref a_intptrBitmapHandle, true));
        }
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust", Unrestricted = false)]
        public STS DatImagenativexferBitmap(DataGroups a_dg, Messages a_msg, ref Bitmap a_bitmap, ref IntPtr a_intptrBitmapHandle, bool a_blUseBitmapHandle)
        {
            STS sts;
            IntPtr intptrBitmap = IntPtr.Zero;

            // Submit the work to the TWAIN thread...
            if (this.m_runinuithreaddelegate == null)
            {
                if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
                {
                    lock (m_lockTwain)
                    {
                        // Set our command variables...
                        ThreadData threaddata = default(ThreadData);
                        threaddata.bitmap = a_bitmap;
                        threaddata.blUseBitmapHandle = a_blUseBitmapHandle;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DataArgumentTypes.IMAGENATIVEXFER;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_bitmap = m_twaincommand.Get(lIndex).bitmap;
                        a_intptrBitmapHandle = m_twaincommand.Get(lIndex).intptrBitmap;
                        sts = m_twaincommand.Get(lIndex).sts;

                        // Clear the command variables...
                        m_twaincommand.Delete(lIndex);
                    }
                    return (sts);
                }
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DataArgumentTypes.IMAGENATIVEXFER.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatImagenativexfer.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryImagenativexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.IMAGENATIVEXFER, a_msg, ref intptrBitmap);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryImagenativexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.IMAGENATIVEXFER, a_msg, ref intptrBitmap);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatImagenativexfer = default(ThreadData);
                                m_threaddataDatImagenativexfer.blIsInuse = true;
                                m_threaddataDatImagenativexfer.dg = a_dg;
                                m_threaddataDatImagenativexfer.msg = a_msg;
                                m_threaddataDatImagenativexfer.dat = DataArgumentTypes.IMAGENATIVEXFER;
                                RunInUiThread(DatImagenativexferWindowsTwain32);
                                intptrBitmap = a_intptrBitmapHandle = m_threaddataDatImagenativexfer.intptrBitmap;
                                sts = m_threaddataDatImagenativexfer.sts;
                                m_threaddataDatImagenativexfer = default(ThreadData);
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatImagenativexfer = default(ThreadData);
                                m_threaddataDatImagenativexfer.blIsInuse = true;
                                m_threaddataDatImagenativexfer.dg = a_dg;
                                m_threaddataDatImagenativexfer.msg = a_msg;
                                m_threaddataDatImagenativexfer.dat = DataArgumentTypes.IMAGENATIVEXFER;
                                RunInUiThread(DatImagenativexferWindowsTwainDsm);
                                intptrBitmap = a_intptrBitmapHandle = m_threaddataDatImagenativexfer.intptrBitmap;
                                sts = m_threaddataDatImagenativexfer.sts;
                                m_threaddataDatImagenativexfer = default(ThreadData);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
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
                        sts = (STS)NativeMethods.Linux64DsmEntryImagenativexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.IMAGENATIVEXFER, a_msg, ref intptrBitmap);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryImagenativexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.IMAGENATIVEXFER, a_msg, ref intptrBitmap);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryImagenativexfer(ref m_twidentityApp, ref m_twidentityDs, a_dg, DataArgumentTypes.IMAGENATIVEXFER, a_msg, ref intptrBitmap);
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
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (ms_platform == Platform.MACOSX)
            {
                // Issue the command...
                try
                {
                    intptrBitmap = IntPtr.Zero;
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryImagenativexfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.IMAGENATIVEXFER, a_msg, ref intptrBitmap);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryImagenativexfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.IMAGENATIVEXFER, a_msg, ref intptrBitmap);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            Codes.STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // If we had a successful transfer, then convert the data...
            if (sts == STS.XFERDONE)
            {
                if (a_blUseBitmapHandle)
                {
                    a_intptrBitmapHandle = intptrBitmap;
                }
                else
                {
                    // Bump our state...
                    m_state = STATE.S7;

                    // Turn the DIB into a Bitmap object...
                    a_bitmap = NativeToBitmap(ms_platform, intptrBitmap);

                    // We're done with the data we got from the driver...
                    Marshal.FreeHGlobal(intptrBitmap);
                    intptrBitmap = IntPtr.Zero;
                }
            }

            // All done...
            return (stsRcOrCc);
        }
    }
}
