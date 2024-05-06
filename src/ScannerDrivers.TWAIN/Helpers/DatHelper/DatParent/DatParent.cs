using ScannerDrivers.TWAIN.Contants.Enum;
using System.Security.Permissions;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Issue DSM commands...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_intptrHwnd">PARENT structure</param>
        /// <returns>TWAIN status</returns>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust", Unrestricted = false)]
        public STS DatParent(DataGroups a_dg, Messages a_msg, ref IntPtr a_intptrHwnd)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if (m_runinuithreaddelegate == null)
            {
                if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
                {
                    lock (m_lockTwain)
                    {
                        // Set our command variables...
                        ThreadData threaddata = default(ThreadData);
                        threaddata.intptrHwnd = a_intptrHwnd;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DataArgumentTypes.PARENT;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_intptrHwnd = m_twaincommand.Get(lIndex).intptrHwnd;
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
                Log.LogSendBefore(a_dg.ToString(), DataArgumentTypes.PARENT.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatParent.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryParent(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DataArgumentTypes.PARENT, a_msg, ref a_intptrHwnd);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryParent(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DataArgumentTypes.PARENT, a_msg, ref a_intptrHwnd);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatParent = default(ThreadData);
                                m_threaddataDatParent.blIsInuse = true;
                                m_threaddataDatParent.dg = a_dg;
                                m_threaddataDatParent.msg = a_msg;
                                m_threaddataDatParent.dat = DataArgumentTypes.PARENT;
                                m_threaddataDatParent.intptrHwnd = a_intptrHwnd;
                                RunInUiThread(DatParentWindowsTwain32);
                                sts = m_threaddataDatParent.sts;
                                m_threaddataDatParent = default(ThreadData);
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatParent = default(ThreadData);
                                m_threaddataDatParent.blIsInuse = true;
                                m_threaddataDatParent.dg = a_dg;
                                m_threaddataDatParent.msg = a_msg;
                                m_threaddataDatParent.dat = DataArgumentTypes.PARENT;
                                m_threaddataDatParent.intptrHwnd = a_intptrHwnd;
                                RunInUiThread(DatParentWindowsTwainDsm);
                                sts = m_threaddataDatParent.sts;
                                m_threaddataDatParent = default(ThreadData);
                            }
                        }
                    }
                    // Needed for the DF_DSM2 flag...
                    m_twidentityApp.SupportedGroups = m_twidentitylegacyApp.SupportedGroups;
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
                STS stsLatest = STS.BUMMER;
                STS sts020302Dsm64bit = STS.BUMMER;

                // We're trying both DSM's...
                sts = STS.BUMMER;

                // Load the new DSM, whatever it is, if we found one...
                if (m_blFoundLatestDsm64)
                {
                    try
                    {
                        stsLatest = (STS)NativeMethods.Linux64DsmEntryParent(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DataArgumentTypes.PARENT, a_msg, ref a_intptrHwnd);
                        // Needed for the DF_DSM2 flag...
                        m_twidentityApp.SupportedGroups = m_twidentitylegacyApp.SupportedGroups;
                    }
                    catch
                    {
                        // Forget this...
                        m_blFoundLatestDsm64 = false;
                    }
                }

                // Load the new DSM, whatever it is, if we found one...
                if (m_blFoundLatestDsm)
                {
                    try
                    {
                        stsLatest = (STS)NativeMethods.LinuxDsmEntryParent(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DataArgumentTypes.PARENT, a_msg, ref a_intptrHwnd);
                        // Needed for the DF_DSM2 flag...
                        m_twidentityApp.SupportedGroups = m_twidentitylegacyApp.SupportedGroups;
                    }
                    catch
                    {
                        // Forget this...
                        m_blFoundLatestDsm = false;
                    }
                }

                // Load libtwaindsm.so.2.3.2, if we found it...
                if (m_blFound020302Dsm64bit)
                {
                    try
                    {
                        sts020302Dsm64bit = (STS)NativeMethods.Linux020302Dsm64bitEntryParent(ref m_twidentityApp, IntPtr.Zero, a_dg, DataArgumentTypes.PARENT, a_msg, ref a_intptrHwnd);
                    }
                    catch
                    {
                        // Forget this...
                        m_blFound020302Dsm64bit = false;
                    }
                }

                // We only need one success to get through this...
                sts = STS.BUMMER;
                if ((stsLatest == STS.SUCCESS) || (sts020302Dsm64bit == STS.SUCCESS))
                {
                    sts = STS.SUCCESS;
                }
            }

            // Mac OS X, which has to be different...
            else if (ms_platform == Platform.MACOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryParent(ref m_twidentitymacosxApp, IntPtr.Zero, a_dg, DataArgumentTypes.PARENT, a_msg, ref a_intptrHwnd);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryParent(ref m_twidentitymacosxApp, IntPtr.Zero, a_dg, DataArgumentTypes.PARENT, a_msg, ref a_intptrHwnd);
                    }
                    // Needed for the DF_DSM2 flag...
                    m_twidentityApp.SupportedGroups = m_twidentitymacosxApp.SupportedGroups;
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

            // If we opened, go to state 3, and start tracking
            // TWAIN's state in the log file...
            if (a_msg == Messages.OPENDSM)
            {
                if (sts == STS.SUCCESS)
                {
                    m_state = STATE.S3;
                    Log.RegisterTwain(this);
                }
            }

            // If we closed, go to state 2, and stop tracking
            // TWAIN's state in the log file...
            else if (a_msg == Messages.CLOSEDSM)
            {
                if (sts == STS.SUCCESS)
                {
                    m_state = STATE.S2;
                    Log.RegisterTwain(null);
                }
            }

            // All done...
            return (stsRcOrCc);
        }

    }
}
