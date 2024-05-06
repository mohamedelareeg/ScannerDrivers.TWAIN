using ScannerDrivers.TWAIN.Contants.Enum;
using ScannerDrivers.TWAIN.Contants.Structure;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Issue event commands...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twevent">EVENT structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatEvent(DataGroups a_dg, Messages a_msg, ref TW_EVENT a_twevent, bool a_blInPreFilter = false)
        {
            STS sts;

            // Log it...
            if (Log.GetLevel() > 1)
            {
                Log.LogSendBefore(a_dg.ToString(), DataArgumentTypes.EVENT.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                // Issue the command...
                try
                {
                    if (a_blInPreFilter || m_threaddataDatEvent.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryEvent(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.EVENT, a_msg, ref a_twevent);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryEvent(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.EVENT, a_msg, ref a_twevent);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatEvent = default(ThreadData);
                                m_threaddataDatEvent.blIsInuse = true;
                                m_threaddataDatEvent.dg = a_dg;
                                m_threaddataDatEvent.msg = a_msg;
                                m_threaddataDatEvent.dat = DataArgumentTypes.EVENT;
                                m_threaddataDatEvent.twevent = a_twevent;
                                RunInUiThread(DatEventWindowsTwain32);
                                a_twevent = m_threaddataDatEvent.twevent;
                                sts = m_threaddataDatEvent.sts;
                                m_threaddataDatEvent = default(ThreadData);
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatEvent = default(ThreadData);
                                m_threaddataDatEvent.blIsInuse = true;
                                m_threaddataDatEvent.dg = a_dg;
                                m_threaddataDatEvent.msg = a_msg;
                                m_threaddataDatEvent.dat = DataArgumentTypes.EVENT;
                                m_threaddataDatEvent.twevent = a_twevent;
                                RunInUiThread(DatEventWindowsTwainDsm);
                                a_twevent = m_threaddataDatEvent.twevent;
                                sts = m_threaddataDatEvent.sts;
                                m_threaddataDatEvent = default(ThreadData);
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
                        sts = (STS)NativeMethods.Linux64DsmEntryEvent(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.EVENT, a_msg, ref a_twevent);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryEvent(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.EVENT, a_msg, ref a_twevent);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryEvent(ref m_twidentityApp, ref m_twidentityDs, a_dg, DataArgumentTypes.EVENT, a_msg, ref a_twevent);
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
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryEvent(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.EVENT, a_msg, ref a_twevent);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryEvent(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.EVENT, a_msg, ref a_twevent);
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
            if (Log.GetLevel() > 1)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // Check the event for anything interesting...
            if ((sts == STS.DSEVENT) || (sts == STS.NOTDSEVENT))
            {
                ProcessEvent((Messages)a_twevent.TWMessage);
            }

            // All done...
            return (stsRcOrCc);
        }
    }
}
