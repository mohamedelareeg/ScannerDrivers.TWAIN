using ScannerDrivers.TWAIN.Contants.Enum;
using ScannerDrivers.TWAIN.Contants.Structure;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Get/Set for CIE color...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twceicolor">CIECOLOR structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatCiecolor(DataGroups a_dg, Messages a_msg, ref TW_CIECOLOR a_twciecolor)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default(ThreadData);
                    threaddata.twciecolor = a_twciecolor;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = DataArgumentTypes.CIECOLOR;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twciecolor = m_twaincommand.Get(lIndex).twciecolor;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DataArgumentTypes.CIECOLOR.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryCiecolor(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.CIECOLOR, a_msg, ref a_twciecolor);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryCiecolor(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.CIECOLOR, a_msg, ref a_twciecolor);
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
                        sts = (STS)NativeMethods.Linux64DsmEntryCiecolor(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.CIECOLOR, a_msg, ref a_twciecolor);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryCiecolor(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.CIECOLOR, a_msg, ref a_twciecolor);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryCiecolor(ref m_twidentityApp, ref m_twidentityDs, a_dg, DataArgumentTypes.CIECOLOR, a_msg, ref a_twciecolor);
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
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryCiecolor(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.CIECOLOR, a_msg, ref a_twciecolor);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryCiecolor(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.CIECOLOR, a_msg, ref a_twciecolor);
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
