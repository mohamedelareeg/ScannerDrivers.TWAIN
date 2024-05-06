using ScannerDrivers.TWAIN.Contants.Enum;
using System.Security.Permissions;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust", Unrestricted = false)]
        public STS DatAudionativexfer(DataGroups a_dg, Messages a_msg, ref IntPtr a_intptrAudio)
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
                        threaddata.intptrAudio = a_intptrAudio;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DataArgumentTypes.AUDIONATIVEXFER;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_intptrAudio = m_twaincommand.Get(lIndex).intptrAudio;
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
                Log.LogSendBefore(a_dg.ToString(), DataArgumentTypes.AUDIONATIVEXFER.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatAudionativexfer.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryAudionativexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.AUDIONATIVEXFER, a_msg, ref a_intptrAudio);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryAudionativexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.AUDIONATIVEXFER, a_msg, ref a_intptrAudio);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatAudionativexfer = default(ThreadData);
                                m_threaddataDatAudionativexfer.blIsInuse = true;
                                m_threaddataDatAudionativexfer.dg = a_dg;
                                m_threaddataDatAudionativexfer.msg = a_msg;
                                m_threaddataDatAudionativexfer.dat = DataArgumentTypes.AUDIONATIVEXFER;
                                RunInUiThread(DatAudionativexferWindowsTwain32);
                                a_intptrAudio = m_threaddataDatAudionativexfer.intptrAudio;
                                sts = m_threaddataDatAudionativexfer.sts;
                                m_threaddataDatAudionativexfer = default(ThreadData);
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatAudionativexfer = default(ThreadData);
                                m_threaddataDatAudionativexfer.blIsInuse = true;
                                m_threaddataDatAudionativexfer.dg = a_dg;
                                m_threaddataDatAudionativexfer.msg = a_msg;
                                m_threaddataDatAudionativexfer.dat = DataArgumentTypes.AUDIONATIVEXFER;
                                RunInUiThread(DatAudionativexferWindowsTwainDsm);
                                a_intptrAudio = m_threaddataDatAudionativexfer.intptrAudio;
                                sts = m_threaddataDatAudionativexfer.sts;
                                m_threaddataDatAudionativexfer = default(ThreadData);
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
                    a_intptrAudio = IntPtr.Zero;
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryAudionativexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.AUDIONATIVEXFER, a_msg, ref a_intptrAudio);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryAudionativexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.AUDIONATIVEXFER, a_msg, ref a_intptrAudio);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryAudionativexfer(ref m_twidentityApp, ref m_twidentityDs, a_dg, DataArgumentTypes.IMAGENATIVEXFER, a_msg, ref a_intptrAudio);
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
                    a_intptrAudio = IntPtr.Zero;
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryAudionativexfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.AUDIONATIVEXFER, a_msg, ref a_intptrAudio);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryAudionativexfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.AUDIONATIVEXFER, a_msg, ref a_intptrAudio);
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

            // If we had a successful transfer, then change state...
            if (sts == STS.XFERDONE)
            {
                // Bump our state...
                m_state = STATE.S7;
            }

            // All done...
            return (stsRcOrCc);
        }
    }
}
