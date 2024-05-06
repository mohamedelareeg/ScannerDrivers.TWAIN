using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Get/Set for a file xfer...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twsetupfilexfer">SETUPFILEXFER structure</param>
        /// <returns>TWAIN status</returns>

        public STS DatSetupfilexfer(DataGroups a_dg, Messages a_msg, ref TW_SETUPFILEXFER a_twsetupfilexfer)
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
                        threaddata.twsetupfilexfer = a_twsetupfilexfer;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DataArgumentTypes.SETUPFILEXFER;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_twsetupfilexfer = m_twaincommand.Get(lIndex).twsetupfilexfer;
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
                Log.LogSendBefore(a_dg.ToString(), DataArgumentTypes.SETUPFILEXFER.ToString(), a_msg.ToString(), SetupfilexferToCsv(a_twsetupfilexfer));
            }

            // Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatSetupfilexfer.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntrySetupfilexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.SETUPFILEXFER, a_msg, ref a_twsetupfilexfer);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntrySetupfilexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.SETUPFILEXFER, a_msg, ref a_twsetupfilexfer);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatSetupfilexfer = default(ThreadData);
                                m_threaddataDatSetupfilexfer.blIsInuse = true;
                                m_threaddataDatSetupfilexfer.dg = a_dg;
                                m_threaddataDatSetupfilexfer.msg = a_msg;
                                m_threaddataDatSetupfilexfer.dat = DataArgumentTypes.SETUPFILEXFER;
                                m_threaddataDatSetupfilexfer.twsetupfilexfer = a_twsetupfilexfer;
                                RunInUiThread(DatSetupfilexferWindowsTwain32);
                                a_twsetupfilexfer = m_threaddataDatSetupfilexfer.twsetupfilexfer;
                                sts = m_threaddataDatSetupfilexfer.sts;
                                m_threaddataDatSetupfilexfer = default(ThreadData);
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatSetupfilexfer = default(ThreadData);
                                m_threaddataDatSetupfilexfer.blIsInuse = true;
                                m_threaddataDatSetupfilexfer.dg = a_dg;
                                m_threaddataDatSetupfilexfer.msg = a_msg;
                                m_threaddataDatSetupfilexfer.dat = DataArgumentTypes.SETUPFILEXFER;
                                m_threaddataDatSetupfilexfer.twsetupfilexfer = a_twsetupfilexfer;
                                RunInUiThread(DatSetupfilexferWindowsTwainDsm);
                                a_twsetupfilexfer = m_threaddataDatSetupfilexfer.twsetupfilexfer;
                                sts = m_threaddataDatSetupfilexfer.sts;
                                m_threaddataDatSetupfilexfer = default(ThreadData);
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
                        sts = (STS)NativeMethods.Linux64DsmEntrySetupfilexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.SETUPFILEXFER, a_msg, ref a_twsetupfilexfer);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntrySetupfilexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.SETUPFILEXFER, a_msg, ref a_twsetupfilexfer);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntrySetupfilexfer(ref m_twidentityApp, ref m_twidentityDs, a_dg, DataArgumentTypes.SETUPFILEXFER, a_msg, ref a_twsetupfilexfer);
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
                        sts = (STS)NativeMethods.MacosxTwainDsmEntrySetupfilexfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.SETUPFILEXFER, a_msg, ref a_twsetupfilexfer);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntrySetupfilexfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.SETUPFILEXFER, a_msg, ref a_twsetupfilexfer);
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
                Log.LogSendAfter(stsRcOrCc, SetupfilexferToCsv(a_twsetupfilexfer));
            }

            // All done...
            return (stsRcOrCc);
        }
    }
}
