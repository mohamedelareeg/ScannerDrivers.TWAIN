using ScannerDrivers.TWAIN.Contants.Enum;
using ScannerDrivers.TWAIN.Contants.Structure;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Get/Set extended image info information...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twextimageinfo">EXTIMAGEINFO structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatExtimageinfo(DataGroups a_dg, Messages a_msg, ref TW_EXTIMAGEINFO a_twextimageinfo)
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
                        threaddata.twextimageinfo = a_twextimageinfo;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DataArgumentTypes.EXTIMAGEINFO;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_twextimageinfo = m_twaincommand.Get(lIndex).twextimageinfo;
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
                Log.LogSendBefore(a_dg.ToString(), DataArgumentTypes.EXTIMAGEINFO.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatExtimageinfo.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryExtimageinfo(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.EXTIMAGEINFO, a_msg, ref a_twextimageinfo);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryExtimageinfo(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.EXTIMAGEINFO, a_msg, ref a_twextimageinfo);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatExtimageinfo = default(ThreadData);
                                m_threaddataDatExtimageinfo.blIsInuse = true;
                                m_threaddataDatExtimageinfo.dg = a_dg;
                                m_threaddataDatExtimageinfo.msg = a_msg;
                                m_threaddataDatExtimageinfo.dat = DataArgumentTypes.EXTIMAGEINFO;
                                m_threaddataDatExtimageinfo.twextimageinfo = a_twextimageinfo;
                                RunInUiThread(DatExtimageinfoWindowsTwain32);
                                a_twextimageinfo = m_threaddataDatExtimageinfo.twextimageinfo;
                                sts = m_threaddataDatExtimageinfo.sts;
                                m_threaddataDatExtimageinfo = default(ThreadData);
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatExtimageinfo = default(ThreadData);
                                m_threaddataDatExtimageinfo.blIsInuse = true;
                                m_threaddataDatExtimageinfo.dg = a_dg;
                                m_threaddataDatExtimageinfo.msg = a_msg;
                                m_threaddataDatExtimageinfo.dat = DataArgumentTypes.EXTIMAGEINFO;
                                m_threaddataDatExtimageinfo.twextimageinfo = a_twextimageinfo;
                                RunInUiThread(DatExtimageinfoWindowsTwainDsm);
                                a_twextimageinfo = m_threaddataDatExtimageinfo.twextimageinfo;
                                sts = m_threaddataDatExtimageinfo.sts;
                                m_threaddataDatExtimageinfo = default(ThreadData);
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
                        sts = (STS)NativeMethods.Linux64DsmEntryExtimageinfo(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.EXTIMAGEINFO, a_msg, ref a_twextimageinfo);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryExtimageinfo(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.EXTIMAGEINFO, a_msg, ref a_twextimageinfo);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryExtimageinfo(ref m_twidentityApp, ref m_twidentityDs, a_dg, DataArgumentTypes.EXTIMAGEINFO, a_msg, ref a_twextimageinfo);
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
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryExtimageinfo(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.EXTIMAGEINFO, a_msg, ref a_twextimageinfo);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryExtimageinfo(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.EXTIMAGEINFO, a_msg, ref a_twextimageinfo);
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
