using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Get info about the memory xfer...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twsetupmemxfer">SETUPMEMXFER structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatSetupmemxfer(DataGroups a_dg, Messages a_msg, ref TW_SETUPMEMXFER a_twsetupmemxfer)
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
                        threaddata.twsetupmemxfer = a_twsetupmemxfer;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DataArgumentTypes.SETUPMEMXFER;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_twsetupmemxfer = m_twaincommand.Get(lIndex).twsetupmemxfer;
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
                Log.LogSendBefore(a_dg.ToString(), DataArgumentTypes.SETUPMEMXFER.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatSetupmemxfer.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntrySetupmemxfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.SETUPMEMXFER, a_msg, ref a_twsetupmemxfer);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntrySetupmemxfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.SETUPMEMXFER, a_msg, ref a_twsetupmemxfer);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatSetupmemxfer = default(ThreadData);
                                m_threaddataDatSetupmemxfer.blIsInuse = true;
                                m_threaddataDatSetupmemxfer.dg = a_dg;
                                m_threaddataDatSetupmemxfer.msg = a_msg;
                                m_threaddataDatSetupmemxfer.dat = DataArgumentTypes.SETUPMEMXFER;
                                m_threaddataDatSetupmemxfer.twsetupmemxfer = a_twsetupmemxfer;
                                RunInUiThread(DatSetupmemxferWindowsTwain32);
                                a_twsetupmemxfer = m_threaddataDatSetupmemxfer.twsetupmemxfer;
                                sts = m_threaddataDatSetupmemxfer.sts;
                                m_threaddataDatSetupmemxfer = default(ThreadData);
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatSetupmemxfer = default(ThreadData);
                                m_threaddataDatSetupmemxfer.blIsInuse = true;
                                m_threaddataDatSetupmemxfer.dg = a_dg;
                                m_threaddataDatSetupmemxfer.msg = a_msg;
                                m_threaddataDatSetupmemxfer.dat = DataArgumentTypes.SETUPMEMXFER;
                                m_threaddataDatSetupmemxfer.twsetupmemxfer = a_twsetupmemxfer;
                                RunInUiThread(DatSetupmemxferWindowsTwainDsm);
                                a_twsetupmemxfer = m_threaddataDatSetupmemxfer.twsetupmemxfer;
                                sts = m_threaddataDatSetupmemxfer.sts;
                                m_threaddataDatSetupmemxfer = default(ThreadData);
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
                        sts = (STS)NativeMethods.Linux64DsmEntrySetupmemxfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.SETUPMEMXFER, a_msg, ref a_twsetupmemxfer);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntrySetupmemxfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.SETUPMEMXFER, a_msg, ref a_twsetupmemxfer);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntrySetupmemxfer(ref m_twidentityApp, ref m_twidentityDs, a_dg, DataArgumentTypes.SETUPMEMXFER, a_msg, ref a_twsetupmemxfer);
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
                        sts = (STS)NativeMethods.MacosxTwainDsmEntrySetupmemxfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.SETUPMEMXFER, a_msg, ref a_twsetupmemxfer);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntrySetupmemxfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.SETUPMEMXFER, a_msg, ref a_twsetupmemxfer);
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
                Log.LogSendAfter(stsRcOrCc, SetupmemxferToCsv(a_twsetupmemxfer));
            }

            // All done...
            return (stsRcOrCc);
        }
    }
}
