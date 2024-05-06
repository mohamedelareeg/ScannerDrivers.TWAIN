using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Issue capabilities commands...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twuserinterface">USERINTERFACE structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatUserinterface(DataGroups a_dg, Messages a_msg, ref TW_USERINTERFACE a_twuserinterface)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if (this.m_runinuithreaddelegate == null)
            {
                if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
                {
                    lock (m_lockTwain)
                    {
                        // Set our command variables...
                        ThreadData threaddata = default(ThreadData);
                        threaddata.twuserinterface = a_twuserinterface;
                        threaddata.twuserinterface.hParent = m_intptrHwnd;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DataArgumentTypes.USERINTERFACE;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_twuserinterface = m_twaincommand.Get(lIndex).twuserinterface;
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
                Log.LogSendBefore(a_dg.ToString(), DataArgumentTypes.USERINTERFACE.ToString(), a_msg.ToString(), UserinterfaceToCsv(a_twuserinterface));
            }

            // We need this to handle data sources that return MSG_XFERREADY in
            // the midst of processing MSG_ENABLEDS, otherwise we'll miss it...
            m_blAcceptXferReady = (a_msg == Messages.ENABLEDS);
            m_blRunningDatUserinterface = (a_msg == Messages.ENABLEDS);

            // Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatUserinterface.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryUserinterface(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.USERINTERFACE, a_msg, ref a_twuserinterface);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryUserinterface(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.USERINTERFACE, a_msg, ref a_twuserinterface);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatUserinterface = default(ThreadData);
                                m_threaddataDatUserinterface.blIsInuse = true;
                                m_threaddataDatUserinterface.dg = a_dg;
                                m_threaddataDatUserinterface.msg = a_msg;
                                m_threaddataDatUserinterface.dat = DataArgumentTypes.USERINTERFACE;
                                m_threaddataDatUserinterface.twuserinterface = a_twuserinterface;
                                RunInUiThread(DatUserinterfaceWindowsTwain32);
                                a_twuserinterface = m_threaddataDatUserinterface.twuserinterface;
                                sts = m_threaddataDatUserinterface.sts;
                                m_threaddataDatUserinterface = default(ThreadData);
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatUserinterface = default(ThreadData);
                                m_threaddataDatUserinterface.blIsInuse = true;
                                m_threaddataDatUserinterface.dg = a_dg;
                                m_threaddataDatUserinterface.msg = a_msg;
                                m_threaddataDatUserinterface.dat = DataArgumentTypes.USERINTERFACE;
                                m_threaddataDatUserinterface.twuserinterface = a_twuserinterface;
                                RunInUiThread(DatUserinterfaceWindowsTwainDsm);
                                a_twuserinterface = m_threaddataDatUserinterface.twuserinterface;
                                sts = m_threaddataDatUserinterface.sts;
                                m_threaddataDatUserinterface = default(ThreadData);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    m_blRunningDatUserinterface = false;
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
                        sts = (STS)NativeMethods.Linux64DsmEntryUserinterface(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.USERINTERFACE, a_msg, ref a_twuserinterface);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryUserinterface(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.USERINTERFACE, a_msg, ref a_twuserinterface);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryUserinterface(ref m_twidentityApp, ref m_twidentityDs, a_dg, DataArgumentTypes.USERINTERFACE, a_msg, ref a_twuserinterface);
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
                    m_blRunningDatUserinterface = false;
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
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryUserinterface(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.USERINTERFACE, a_msg, ref a_twuserinterface);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryUserinterface(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.USERINTERFACE, a_msg, ref a_twuserinterface);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    m_blRunningDatUserinterface = false;
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                m_blRunningDatUserinterface = false;
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            Codes.STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // If successful, decide which way to jump...
            if (sts == STS.SUCCESS)
            {
                switch (a_msg)
                {
                    // No clue...
                    default:
                        break;

                    // Jump up...
                    case Messages.ENABLEDS:
                    case Messages.ENABLEDSUIONLY:
                        m_state = STATE.S5;
                        break;

                    // Jump down...
                    case Messages.DISABLEDS:
                        m_blIsMsgclosedsreq = false;
                        m_blIsMsgclosedsok = false;
                        m_state = STATE.S4;
                        break;
                }
            }

            // All done...
            m_blRunningDatUserinterface = false;
            return (stsRcOrCc);
        }
    }
}
