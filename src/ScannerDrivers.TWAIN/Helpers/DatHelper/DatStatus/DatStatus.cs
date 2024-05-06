using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Get some text for an error...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twstatus">STATUS structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatStatus(DataGroups a_dg, Messages a_msg, ref TW_STATUS a_twstatus)
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
                        threaddata.twstatus = a_twstatus;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DataArgumentTypes.STATUS;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_twstatus = m_twaincommand.Get(lIndex).twstatus;
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
                Log.LogSendBefore(a_dg.ToString(), DataArgumentTypes.STATUS.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatStatus.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryStatus(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.STATUS, a_msg, ref a_twstatus);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryStatus(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.STATUS, a_msg, ref a_twstatus);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatStatus = default(ThreadData);
                                m_threaddataDatStatus.blIsInuse = true;
                                m_threaddataDatStatus.dg = a_dg;
                                m_threaddataDatStatus.msg = a_msg;
                                m_threaddataDatStatus.dat = DataArgumentTypes.STATUS;
                                m_threaddataDatStatus.twstatus = a_twstatus;
                                RunInUiThread(DatStatusWindowsTwain32);
                                a_twstatus = m_threaddataDatStatus.twstatus;
                                sts = m_threaddataDatStatus.sts;
                                m_threaddataDatStatus = default(ThreadData);
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatStatus = default(ThreadData);
                                m_threaddataDatStatus.blIsInuse = true;
                                m_threaddataDatStatus.dg = a_dg;
                                m_threaddataDatStatus.msg = a_msg;
                                m_threaddataDatStatus.dat = DataArgumentTypes.STATUS;
                                m_threaddataDatStatus.twstatus = a_twstatus;
                                RunInUiThread(DatStatusWindowsTwainDsm);
                                a_twstatus = m_threaddataDatStatus.twstatus;
                                sts = m_threaddataDatStatus.sts;
                                m_threaddataDatStatus = default(ThreadData);
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
                        sts = (STS)NativeMethods.Linux64DsmEntryStatus(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.STATUS, a_msg, ref a_twstatus);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryStatus(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.STATUS, a_msg, ref a_twstatus);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryStatus(ref m_twidentityApp, ref m_twidentityDs, a_dg, DataArgumentTypes.STATUS, a_msg, ref a_twstatus);
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
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryStatus(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.STATUS, a_msg, ref a_twstatus);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryStatus(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.STATUS, a_msg, ref a_twstatus);
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

            // Skip getting the status...  :)
            Codes.STS stsRcOrCc = sts;

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // All done...
            return (stsRcOrCc);
        }
    }
}
