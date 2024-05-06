﻿using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Get/Set for a raw commands...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twpassthru">PASSTHRU structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatPassthru(DataGroups a_dg, Messages a_msg, ref TW_PASSTHRU a_twpassthru)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default(ThreadData);
                    threaddata.twpassthru = a_twpassthru;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = DataArgumentTypes.PASSTHRU;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twpassthru = m_twaincommand.Get(lIndex).twpassthru;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DataArgumentTypes.PASSTHRU.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryPassthru(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.PASSTHRU, a_msg, ref a_twpassthru);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryPassthru(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.PASSTHRU, a_msg, ref a_twpassthru);
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
                        sts = (STS)NativeMethods.Linux64DsmEntryPassthru(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.PASSTHRU, a_msg, ref a_twpassthru);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryPassthru(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.PASSTHRU, a_msg, ref a_twpassthru);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryPassthru(ref m_twidentityApp, ref m_twidentityDs, a_dg, DataArgumentTypes.PASSTHRU, a_msg, ref a_twpassthru);
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
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryPassthru(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.PASSTHRU, a_msg, ref a_twpassthru);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryPassthru(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.PASSTHRU, a_msg, ref a_twpassthru);
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

            // All done...
            return (stsRcOrCc);
        }
    }
}