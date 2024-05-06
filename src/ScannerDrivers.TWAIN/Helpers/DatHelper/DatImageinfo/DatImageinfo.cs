using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Get/Set image info information...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twimageinfo">IMAGEINFO structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatImageinfo(DataGroups a_dg, Messages a_msg, ref TW_IMAGEINFO a_twimageinfo)
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
                        threaddata.twimageinfo = a_twimageinfo;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DataArgumentTypes.IMAGEINFO;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_twimageinfo = m_twaincommand.Get(lIndex).twimageinfo;
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
                Log.LogSendBefore(a_dg.ToString(), DataArgumentTypes.IMAGEINFO.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatImageinfo.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryImageinfo(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.IMAGEINFO, a_msg, ref a_twimageinfo);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryImageinfo(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.IMAGEINFO, a_msg, ref a_twimageinfo);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatImageinfo = default(ThreadData);
                                m_threaddataDatImageinfo.blIsInuse = true;
                                m_threaddataDatImageinfo.dg = a_dg;
                                m_threaddataDatImageinfo.msg = a_msg;
                                m_threaddataDatImageinfo.dat = DataArgumentTypes.IMAGEINFO;
                                m_threaddataDatImageinfo.twimageinfo = a_twimageinfo;
                                RunInUiThread(DatImageinfoWindowsTwain32);
                                a_twimageinfo = m_threaddataDatImageinfo.twimageinfo;
                                sts = m_threaddataDatImageinfo.sts;
                                m_threaddataDatImageinfo = default(ThreadData);
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatImageinfo = default(ThreadData);
                                m_threaddataDatImageinfo.blIsInuse = true;
                                m_threaddataDatImageinfo.dg = a_dg;
                                m_threaddataDatImageinfo.msg = a_msg;
                                m_threaddataDatImageinfo.dat = DataArgumentTypes.IMAGEINFO;
                                m_threaddataDatImageinfo.twimageinfo = a_twimageinfo;
                                RunInUiThread(DatImageinfoWindowsTwainDsm);
                                a_twimageinfo = m_threaddataDatImageinfo.twimageinfo;
                                sts = m_threaddataDatImageinfo.sts;
                                m_threaddataDatImageinfo = default(ThreadData);
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
                        sts = (STS)NativeMethods.Linux64DsmEntryImageinfo(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.IMAGEINFO, a_msg, ref a_twimageinfo);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryImageinfo(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.IMAGEINFO, a_msg, ref a_twimageinfo);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        TW_IMAGEINFO_LINUX64 twimageinfolinux64 = default(TW_IMAGEINFO_LINUX64);
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryImageinfo(ref m_twidentityApp, ref m_twidentityDs, a_dg, DataArgumentTypes.IMAGEINFO, a_msg, ref twimageinfolinux64);
                        a_twimageinfo.XResolution = twimageinfolinux64.XResolution;
                        a_twimageinfo.YResolution = twimageinfolinux64.YResolution;
                        a_twimageinfo.ImageWidth = (int)twimageinfolinux64.ImageWidth;
                        a_twimageinfo.ImageLength = (int)twimageinfolinux64.ImageLength;
                        a_twimageinfo.SamplesPerPixel = twimageinfolinux64.SamplesPerPixel;
                        a_twimageinfo.BitsPerSample_0 = twimageinfolinux64.BitsPerSample_0;
                        a_twimageinfo.BitsPerSample_1 = twimageinfolinux64.BitsPerSample_1;
                        a_twimageinfo.BitsPerSample_2 = twimageinfolinux64.BitsPerSample_2;
                        a_twimageinfo.BitsPerSample_3 = twimageinfolinux64.BitsPerSample_3;
                        a_twimageinfo.BitsPerSample_4 = twimageinfolinux64.BitsPerSample_4;
                        a_twimageinfo.BitsPerSample_5 = twimageinfolinux64.BitsPerSample_5;
                        a_twimageinfo.BitsPerSample_6 = twimageinfolinux64.BitsPerSample_6;
                        a_twimageinfo.BitsPerSample_7 = twimageinfolinux64.BitsPerSample_7;
                        a_twimageinfo.BitsPerPixel = twimageinfolinux64.BitsPerPixel;
                        a_twimageinfo.Planar = twimageinfolinux64.Planar;
                        a_twimageinfo.PixelType = twimageinfolinux64.PixelType;
                        a_twimageinfo.Compression = twimageinfolinux64.Compression;
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
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryImageinfo(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.IMAGEINFO, a_msg, ref a_twimageinfo);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryImageinfo(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.IMAGEINFO, a_msg, ref a_twimageinfo);
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
                Log.LogSendAfter(stsRcOrCc, ImageinfoToCsv(a_twimageinfo));
            }

            // All done...
            return (stsRcOrCc);
        }
    }
}
