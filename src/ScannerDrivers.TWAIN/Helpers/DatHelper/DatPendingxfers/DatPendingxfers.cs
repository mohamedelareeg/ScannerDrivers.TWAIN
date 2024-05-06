using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Issue pendingxfers commands...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twpendingxfers">PENDINGXFERS structure</param>
        /// <returns>TWAIN status</returns>

        public STS DatPendingxfers(DataGroups a_dg, Messages a_msg, ref TW_PENDINGXFERS a_twpendingxfers)
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
                        threaddata.twpendingxfers = a_twpendingxfers;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DataArgumentTypes.PENDINGXFERS;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_twpendingxfers = m_twaincommand.Get(lIndex).twpendingxfers;
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
                Log.LogSendBefore(a_dg.ToString(), DataArgumentTypes.PENDINGXFERS.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatPendingxfers.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryPendingxfers(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.PENDINGXFERS, a_msg, ref a_twpendingxfers);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryPendingxfers(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.PENDINGXFERS, a_msg, ref a_twpendingxfers);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatPendingxfers = default(ThreadData);
                                m_threaddataDatPendingxfers.blIsInuse = true;
                                m_threaddataDatPendingxfers.dg = a_dg;
                                m_threaddataDatPendingxfers.msg = a_msg;
                                m_threaddataDatPendingxfers.dat = DataArgumentTypes.PENDINGXFERS;
                                m_threaddataDatPendingxfers.twpendingxfers = a_twpendingxfers;
                                RunInUiThread(DatPendingxfersWindowsTwain32);
                                a_twpendingxfers = m_threaddataDatPendingxfers.twpendingxfers;
                                sts = m_threaddataDatPendingxfers.sts;
                                m_threaddataDatPendingxfers = default(ThreadData);
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatPendingxfers = default(ThreadData);
                                m_threaddataDatPendingxfers.blIsInuse = true;
                                m_threaddataDatPendingxfers.dg = a_dg;
                                m_threaddataDatPendingxfers.msg = a_msg;
                                m_threaddataDatPendingxfers.dat = DataArgumentTypes.PENDINGXFERS;
                                m_threaddataDatPendingxfers.twpendingxfers = a_twpendingxfers;
                                RunInUiThread(DatPendingxfersWindowsTwainDsm);
                                a_twpendingxfers = m_threaddataDatPendingxfers.twpendingxfers;
                                sts = m_threaddataDatPendingxfers.sts;
                                m_threaddataDatPendingxfers = default(ThreadData);
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
                        sts = (STS)NativeMethods.Linux64DsmEntryPendingxfers(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.PENDINGXFERS, a_msg, ref a_twpendingxfers);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryPendingxfers(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.PENDINGXFERS, a_msg, ref a_twpendingxfers);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryPendingxfers(ref m_twidentityApp, ref m_twidentityDs, a_dg, DataArgumentTypes.PENDINGXFERS, a_msg, ref a_twpendingxfers);
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
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryPendingxfers(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.PENDINGXFERS, a_msg, ref a_twpendingxfers);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryPendingxfers(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.PENDINGXFERS, a_msg, ref a_twpendingxfers);
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
                Log.LogSendAfter(stsRcOrCc, PendingxfersToCsv(a_twpendingxfers));
            }

            // If we endxfer, go to state 5 or 6...
            if (a_msg == Messages.ENDXFER)
            {
                if (sts == STS.SUCCESS)
                {
                    if (a_twpendingxfers.Count == 0)
                    {
                        m_blAcceptXferReady = true;
                        m_blIsMsgxferready = false;
                        m_state = STATE.S5;
                    }
                    else
                    {
                        m_state = STATE.S6;
                    }
                }
            }

            // If we reset, go to state 5...
            else if (a_msg == Messages.RESET)
            {
                if (sts == STS.SUCCESS)
                {
                    m_blAcceptXferReady = true;
                    m_blIsMsgxferready = false;
                    m_state = STATE.S5;
                }
            }

            // All done...
            return (stsRcOrCc);
        }
    }
}
