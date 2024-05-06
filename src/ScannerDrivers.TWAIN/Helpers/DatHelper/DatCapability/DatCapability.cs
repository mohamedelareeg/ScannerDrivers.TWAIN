using ScannerDrivers.TWAIN.Contants;
using ScannerDrivers.TWAIN.Contants.Enum;
using ScannerDrivers.TWAIN.Contants.Generic;
using ScannerDrivers.TWAIN.Contants.Structure;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        public STS DatCapability(DataGroups a_dg, Messages a_msg, ref TW_CAPABILITY a_twcapability)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if (m_runinuithreaddelegate == null)
            {
                if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
                {
                    lock (m_lockTwain)
                    {
                        ThreadData threaddata = default(ThreadData);
                        long lIndex = 0;

                        // TBD: sometimes this doesn't work!  Not sure why
                        // yet, but a retry takes care of it.
                        for (int ii = 0; ii < 5; ii++)
                        {
                            // Set our command variables...
                            threaddata = default(ThreadData);
                            threaddata.twcapability = a_twcapability;
                            threaddata.dg = a_dg;
                            threaddata.msg = a_msg;
                            threaddata.dat = DataArgumentTypes.CAPABILITY;
                            lIndex = m_twaincommand.Submit(threaddata);

                            // Submit the command and wait for the reply...
                            CallerToThreadSet();
                            ThreadToCallerWaitOne();

                            // Hmmm...
                            if ((a_msg == Messages.GETCURRENT)
                                && (m_twaincommand.Get(lIndex).sts == STS.SUCCESS)
                                && (m_twaincommand.Get(lIndex).twcapability.ConType == (TWON)0)
                                && (m_twaincommand.Get(lIndex).twcapability.hContainer == IntPtr.Zero))
                            {
                                Thread.Sleep(1000);
                                continue;
                            }

                            // We're done...
                            break;
                        }

                        // Return the result...
                        a_twcapability = m_twaincommand.Get(lIndex).twcapability;
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
                if ((a_msg == Messages.SET) || (a_msg == Messages.SETCONSTRAINT))
                {
                    Log.LogSendBefore(a_dg.ToString(), DataArgumentTypes.CAPABILITY.ToString(), a_msg.ToString(), CapabilityToCsv(a_twcapability, (a_msg != Messages.QUERYSUPPORT)));
                }
                else
                {
                    string szCap = a_twcapability.Cap.ToString();
                    if (!szCap.Contains("_"))
                    {
                        szCap = "0x" + ((ushort)a_twcapability.Cap).ToString("X");
                    }
                    Log.LogSendBefore(a_dg.ToString(), DataArgumentTypes.CAPABILITY.ToString(), a_msg.ToString(), szCap + ",0,0");
                }
            }

            // Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatCapability.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryCapability(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.CAPABILITY, a_msg, ref a_twcapability);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryCapability(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.CAPABILITY, a_msg, ref a_twcapability);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatCapability = default(ThreadData);
                                m_threaddataDatCapability.blIsInuse = true;
                                m_threaddataDatCapability.dg = a_dg;
                                m_threaddataDatCapability.msg = a_msg;
                                m_threaddataDatCapability.dat = DataArgumentTypes.CAPABILITY;
                                m_threaddataDatCapability.twcapability = a_twcapability;
                                RunInUiThread(DatCapabilityWindowsTwain32);
                                a_twcapability = m_threaddataDatCapability.twcapability;
                                sts = m_threaddataDatCapability.sts;
                                m_threaddataDatCapability = default(ThreadData);
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatCapability = default(ThreadData);
                                m_threaddataDatCapability.blIsInuse = true;
                                m_threaddataDatCapability.dg = a_dg;
                                m_threaddataDatCapability.msg = a_msg;
                                m_threaddataDatCapability.dat = DataArgumentTypes.CAPABILITY;
                                m_threaddataDatCapability.twcapability = a_twcapability;
                                RunInUiThread(DatCapabilityWindowsTwainDsm);
                                a_twcapability = m_threaddataDatCapability.twcapability;
                                sts = m_threaddataDatCapability.sts;
                                m_threaddataDatCapability = default(ThreadData);
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
                        sts = (STS)NativeMethods.Linux64DsmEntryCapability(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.CAPABILITY, a_msg, ref a_twcapability);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryCapability(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.CAPABILITY, a_msg, ref a_twcapability);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryCapability(ref m_twidentityApp, ref m_twidentityDs, a_dg, DataArgumentTypes.CAPABILITY, a_msg, ref a_twcapability);
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
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryCapability(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.CAPABILITY, a_msg, ref a_twcapability);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryCapability(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.CAPABILITY, a_msg, ref a_twcapability);
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
                if ((a_msg == Messages.RESETALL) || ((sts != STS.SUCCESS) && (sts != STS.CHECKSTATUS)))
                {
                    Log.LogSendAfter(stsRcOrCc, "");
                }
                else
                {
                    Log.LogSendAfter(stsRcOrCc, CapabilityToCsv(a_twcapability, (a_msg != Messages.QUERYSUPPORT)));
                }
            }

            // if MSG_SET/CAP_LANGUAGE, then we want to track it
            if ((a_twcapability.Cap == Capabilities.CAP_LANGUAGE) && (a_msg == Messages.SET) && (a_twcapability.ConType == TWON.ONEVALUE))
            {
                string str;

                // if it was successful, then go with what was set.
                // otherwise ask the DS what it is currently set to
                if (sts == STS.SUCCESS)
                {
                    str = CapabilityToCsv(a_twcapability, (a_msg != Messages.QUERYSUPPORT));
                }
                else
                {
                    TW_CAPABILITY twcapability = new TW_CAPABILITY();
                    twcapability.Cap = Capabilities.CAP_LANGUAGE;
                    twcapability.ConType = TWON.ONEVALUE;
                    sts = DatCapability(a_dg, Messages.GETCURRENT, ref twcapability);
                    if (sts == STS.SUCCESS)
                    {
                        str = CapabilityToCsv(twcapability, (a_msg != Messages.QUERYSUPPORT));
                    }
                    else
                    {
                        // couldn't get the value, so go with English
                        str = "x," + ((int)Languages.ENGLISH).ToString();
                    }
                }

                // get the value from the CSV string
                Languages twlg = Languages.ENGLISH;
                try
                {
                    string[] astr = str.Split(new char[] { ',' });
                    int result;
                    if (int.TryParse(astr[astr.Length - 1], out result))
                    {
                        twlg = (Languages)result;
                    }
                }
                catch
                {
                    twlg = Languages.ENGLISH;
                }
                Language.Set(twlg);
            }

            // All done...
            return (stsRcOrCc);
        }
    }
}
