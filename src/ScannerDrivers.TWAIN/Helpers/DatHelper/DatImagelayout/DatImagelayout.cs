﻿using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Get/Set layout information...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twimagelayout">IMAGELAYOUT structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatImagelayout(DataGroups a_dg, Messages a_msg, ref TW_IMAGELAYOUT a_twimagelayout)
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
                        threaddata.twimagelayout = a_twimagelayout;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DataArgumentTypes.IMAGELAYOUT;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_twimagelayout = m_twaincommand.Get(lIndex).twimagelayout;
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
                Log.LogSendBefore(a_dg.ToString(), DataArgumentTypes.IMAGELAYOUT.ToString(), a_msg.ToString(), ImagelayoutToCsv(a_twimagelayout));
            }

            // Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatImagelayout.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryImagelayout(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.IMAGELAYOUT, a_msg, ref a_twimagelayout);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryImagelayout(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.IMAGELAYOUT, a_msg, ref a_twimagelayout);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatImagelayout = default(ThreadData);
                                m_threaddataDatImagelayout.blIsInuse = true;
                                m_threaddataDatImagelayout.dg = a_dg;
                                m_threaddataDatImagelayout.msg = a_msg;
                                m_threaddataDatImagelayout.dat = DataArgumentTypes.IMAGELAYOUT;
                                m_threaddataDatImagelayout.twimagelayout = a_twimagelayout;
                                RunInUiThread(DatImagelayoutWindowsTwain32);
                                a_twimagelayout = m_threaddataDatImagelayout.twimagelayout;
                                sts = m_threaddataDatImagelayout.sts;
                                m_threaddataDatImagelayout = default(ThreadData);
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatImagelayout = default(ThreadData);
                                m_threaddataDatImagelayout.blIsInuse = true;
                                m_threaddataDatImagelayout.dg = a_dg;
                                m_threaddataDatImagelayout.msg = a_msg;
                                m_threaddataDatImagelayout.dat = DataArgumentTypes.IMAGELAYOUT;
                                m_threaddataDatImagelayout.twimagelayout = a_twimagelayout;
                                RunInUiThread(DatImagelayoutWindowsTwainDsm);
                                a_twimagelayout = m_threaddataDatImagelayout.twimagelayout;
                                sts = m_threaddataDatImagelayout.sts;
                                m_threaddataDatImagelayout = default(ThreadData);
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
                        sts = (STS)NativeMethods.Linux64DsmEntryImagelayout(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.IMAGELAYOUT, a_msg, ref a_twimagelayout);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryImagelayout(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.IMAGELAYOUT, a_msg, ref a_twimagelayout);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryImagelayout(ref m_twidentityApp, ref m_twidentityDs, a_dg, DataArgumentTypes.IMAGELAYOUT, a_msg, ref a_twimagelayout);
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
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryImagelayout(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.IMAGELAYOUT, a_msg, ref a_twimagelayout);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryImagelayout(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.IMAGELAYOUT, a_msg, ref a_twimagelayout);
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
                Log.LogSendAfter(stsRcOrCc, ImagelayoutToCsv(a_twimagelayout));
            }

            // All done...
            return (stsRcOrCc);
        }
    }
}
