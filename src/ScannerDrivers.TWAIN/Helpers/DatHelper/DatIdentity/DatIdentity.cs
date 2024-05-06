using ScannerDrivers.TWAIN.Contants;
using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;
using ScannerDrivers.TWAIN.Contants.Structure;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Issue identity commands...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twidentity">IDENTITY structure</param>
        /// <returns>TWAIN status</returns>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust", Unrestricted = false)]
        public STS DatIdentity(DataGroups a_dg, Messages a_msg, ref TW_IDENTITY a_twidentity)
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
                        threaddata.twidentity = a_twidentity;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DataArgumentTypes.IDENTITY;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_twidentity = m_twaincommand.Get(lIndex).twidentity;
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
                Log.LogSendBefore(a_dg.ToString(), DataArgumentTypes.IDENTITY.ToString(), a_msg.ToString(), ((a_msg == Messages.OPENDS) ? IdentityToCsv(a_twidentity) : ""));
            }

            // Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                // Convert the identity structure...
                TW_IDENTITY_LEGACY twidentitylegacy = TwidentityToTwidentitylegacy(a_twidentity);

                // Issue the command...
                try
                {
                    if (m_threaddataDatIdentity.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            if (GetState() <= STATE.S3)
                            {
                                sts = (STS)NativeMethods.WindowsTwain32DsmEntryIdentity(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DataArgumentTypes.IDENTITY, a_msg, ref twidentitylegacy);
                            }
                            else
                            {
                                sts = (STS)NativeMethods.WindowsTwain32DsmEntryIdentityState4(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.IDENTITY, a_msg, ref twidentitylegacy);
                            }
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryIdentity(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DataArgumentTypes.IDENTITY, a_msg, ref twidentitylegacy);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatIdentity = default(ThreadData);
                                m_threaddataDatIdentity.blIsInuse = true;
                                m_threaddataDatIdentity.dg = a_dg;
                                m_threaddataDatIdentity.msg = a_msg;
                                m_threaddataDatIdentity.dat = DataArgumentTypes.IDENTITY;
                                m_threaddataDatIdentity.twidentitylegacy = twidentitylegacy;
                                RunInUiThread(DatIdentityWindowsTwain32);
                                twidentitylegacy = m_threaddataDatIdentity.twidentitylegacy;
                                sts = m_threaddataDatIdentity.sts;
                                m_threaddataDatIdentity = default(ThreadData);
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatIdentity = default(ThreadData);
                                m_threaddataDatIdentity.blIsInuse = true;
                                m_threaddataDatIdentity.dg = a_dg;
                                m_threaddataDatIdentity.msg = a_msg;
                                m_threaddataDatIdentity.dat = DataArgumentTypes.IDENTITY;
                                m_threaddataDatIdentity.twidentitylegacy = twidentitylegacy;
                                RunInUiThread(DatIdentityWindowsTwainDsm);
                                twidentitylegacy = m_threaddataDatIdentity.twidentitylegacy;
                                sts = m_threaddataDatIdentity.sts;
                                m_threaddataDatIdentity = default(ThreadData);
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
                a_twidentity = TwidentitylegacyToTwidentity(twidentitylegacy);
            }

            // Linux...
            else if (ms_platform == Platform.LINUX)
            {
                // Issue the command, we have a serious problem with 64-bit stuff
                // because TW_INT32 and TW_UINT32 were defined using long, which
                // made them 64-bit values (not a good idea for 32-bit types).  This
                // was fixed with TWAIN DSM 2.4, but it leaves us with a bit of a
                // mess.  So, unlike all of the other calls, we're going to allow
                // ourselves to access both DSMs from here.  This is only an issue
                // for 64-bit systems, and we're going to assume that the data source
                // is 2.4 or later, since that'll be the long term situation.  Note
                // that we assume the DSMs are protecting us from talking to the
                // wrong data source...
                try
                {
                    // Since life is complex, start by assuming failure...
                    sts = STS.FAILURE;

                    // Handle closeds...
                    if (a_msg == Messages.CLOSEDS)
                    {
                        // We've opened this source, and we know it's the new style...
                        if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                        {
                            TW_IDENTITY_LEGACY twidentitylegacy = TwidentityToTwidentitylegacy(a_twidentity);
                            sts = (STS)NativeMethods.Linux64DsmEntryIdentity(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DataArgumentTypes.IDENTITY, a_msg, ref twidentitylegacy);
                            a_twidentity = TwidentitylegacyToTwidentity(twidentitylegacy);
                        }
                        // We've opened this source, and we know it's the new style...
                        else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                        {
                            TW_IDENTITY_LEGACY twidentitylegacy = TwidentityToTwidentitylegacy(a_twidentity);
                            sts = (STS)NativeMethods.LinuxDsmEntryIdentity(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DataArgumentTypes.IDENTITY, a_msg, ref twidentitylegacy);
                            a_twidentity = TwidentitylegacyToTwidentity(twidentitylegacy);
                        }
                        // We've opened this source, and we know it's the old style...
                        else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                        {
                            TW_IDENTITY_LINUX64 twidentitylinux64App = TwidentityToTwidentitylinux64(m_twidentityApp);
                            TW_IDENTITY_LINUX64 twidentitylinux64 = TwidentityToTwidentitylinux64(a_twidentity);
                            sts = (STS)NativeMethods.Linux020302Dsm64bitEntryIdentity(ref twidentitylinux64App, IntPtr.Zero, a_dg, DataArgumentTypes.IDENTITY, a_msg, ref twidentitylinux64);
                            a_twidentity = Twidentitylinux64ToTwidentity(twidentitylinux64);
                        }
                        // We can't possibly have opened this source, so this had
                        // better be a sequence error...
                        else
                        {
                            sts = STS.SEQERROR;
                        }
                    }

                    // Getfirst always starts with the latest DSM, if it can't find it,
                    // or if it reports end of list, then go on to the old DSM, if we
                    // have one.  Note that it's up to the caller to handle any errors
                    // and keep searching.  We're not trying to figure out anything
                    // about the driver at this level...
                    else if (a_msg == Messages.GETFIRST)
                    {
                        m_linux64bitdsmDatIdentity = LinuxDsm.Unknown;

                        // Assume end of list for the outcome...
                        sts = STS.ENDOFLIST;

                        // Try to start with the latest DSM...
                        if (m_blFoundLatestDsm64)
                        {
                            m_linux64bitdsmDatIdentity = LinuxDsm.IsLatestDsm;
                            TW_IDENTITY_LEGACY twidentitylegacy = TwidentityToTwidentitylegacy(a_twidentity);
                            sts = (STS)NativeMethods.Linux64DsmEntryIdentity(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DataArgumentTypes.IDENTITY, a_msg, ref twidentitylegacy);
                            a_twidentity = TwidentitylegacyToTwidentity(twidentitylegacy);
                        }

                        // Try to start with the latest DSM...
                        if (m_blFoundLatestDsm)
                        {
                            m_linux64bitdsmDatIdentity = LinuxDsm.IsLatestDsm;
                            TW_IDENTITY_LEGACY twidentitylegacy = TwidentityToTwidentitylegacy(a_twidentity);
                            sts = (STS)NativeMethods.LinuxDsmEntryIdentity(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DataArgumentTypes.IDENTITY, a_msg, ref twidentitylegacy);
                            a_twidentity = TwidentitylegacyToTwidentity(twidentitylegacy);
                        }

                        // If the lastest DSM didn't work, try the old stuff...
                        if (m_blFound020302Dsm64bit && (sts == STS.ENDOFLIST))
                        {
                            m_linux64bitdsmDatIdentity = LinuxDsm.Is020302Dsm64bit;
                            TW_IDENTITY_LINUX64 twidentitylinux64App = TwidentityToTwidentitylinux64(m_twidentityApp);
                            TW_IDENTITY_LINUX64 twidentitylinux64 = TwidentityToTwidentitylinux64(a_twidentity);
                            sts = (STS)NativeMethods.Linux020302Dsm64bitEntryIdentity(ref twidentitylinux64App, IntPtr.Zero, a_dg, DataArgumentTypes.IDENTITY, a_msg, ref twidentitylinux64);
                            a_twidentity = Twidentitylinux64ToTwidentity(twidentitylinux64);
                        }
                    }

                    // Getnext gets its lead from getfirst, if we have a DSM
                    // value, we try it out, if we don't have one, we must be
                    // at the end of list.  We'll do the new DSM and then the
                    // old DSM (if we have one)...
                    else if (a_msg == Messages.GETNEXT)
                    {
                        bool blChangeToGetFirst = false;

                        // We're done, they'll have to use MSG_GETFIRST to start again...
                        if (m_linux64bitdsmDatIdentity == LinuxDsm.Unknown)
                        {
                            sts = STS.ENDOFLIST;
                        }

                        // We're working the latest DSM, if we hit end of list, then we'll
                        // try to switch over to the old DSM...
                        if (m_blFoundLatestDsm64 && (m_linux64bitdsmDatIdentity == LinuxDsm.IsLatestDsm))
                        {
                            TW_IDENTITY_LEGACY twidentitylegacy = TwidentityToTwidentitylegacy(a_twidentity);
                            sts = (STS)NativeMethods.Linux64DsmEntryIdentity(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DataArgumentTypes.IDENTITY, a_msg, ref twidentitylegacy);
                            a_twidentity = TwidentitylegacyToTwidentity(twidentitylegacy);
                            if (sts == STS.ENDOFLIST)
                            {
                                m_linux64bitdsmDatIdentity = m_blFound020302Dsm64bit ? LinuxDsm.Is020302Dsm64bit : LinuxDsm.Unknown;
                                blChangeToGetFirst = true;
                            }
                        }

                        // We're working the latest DSM, if we hit end of list, then we'll
                        // try to switch over to the old DSM...
                        if (m_blFoundLatestDsm && (m_linux64bitdsmDatIdentity == LinuxDsm.IsLatestDsm))
                        {
                            TW_IDENTITY_LEGACY twidentitylegacy = TwidentityToTwidentitylegacy(a_twidentity);
                            sts = (STS)NativeMethods.LinuxDsmEntryIdentity(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DataArgumentTypes.IDENTITY, a_msg, ref twidentitylegacy);
                            a_twidentity = TwidentitylegacyToTwidentity(twidentitylegacy);
                            if (sts == STS.ENDOFLIST)
                            {
                                m_linux64bitdsmDatIdentity = m_blFound020302Dsm64bit ? LinuxDsm.Is020302Dsm64bit : LinuxDsm.Unknown;
                                blChangeToGetFirst = true;
                            }
                        }

                        // We're working the old DSM, if we hit the end of list, then we
                        // clear the DSM indicator...
                        if (m_blFound020302Dsm64bit && (m_linux64bitdsmDatIdentity == LinuxDsm.Is020302Dsm64bit))
                        {
                            TW_IDENTITY_LINUX64 twidentitylinux64App = TwidentityToTwidentitylinux64(m_twidentityApp);
                            TW_IDENTITY_LINUX64 twidentitylinux64 = blChangeToGetFirst ? default(TW_IDENTITY_LINUX64) : TwidentityToTwidentitylinux64(a_twidentity);
                            sts = (STS)NativeMethods.Linux020302Dsm64bitEntryIdentity(ref twidentitylinux64App, IntPtr.Zero, a_dg, DataArgumentTypes.IDENTITY, blChangeToGetFirst ? Messages.GETFIRST : a_msg, ref twidentitylinux64);
                            a_twidentity = Twidentitylinux64ToTwidentity(twidentitylinux64);
                            if (sts == STS.ENDOFLIST)
                            {
                                m_linux64bitdsmDatIdentity = LinuxDsm.Unknown;
                            }
                        }
                    }

                    // Open always tries the current DSM, and then the older one, if needed...
                    else if (a_msg == Messages.OPENDS)
                    {
                        TW_IDENTITY_LEGACY twidentitylegacy = default(TW_IDENTITY_LEGACY);

                        // Prime the pump by assuming we didn't find anything...
                        sts = STS.NODS;

                        // Try with the latest DSM first, if we have one...
                        if (m_blFoundLatestDsm64)
                        {
                            twidentitylegacy = TwidentityToTwidentitylegacy(a_twidentity);
                            twidentitylegacy.Id = 0;
                            try
                            {
                                sts = (STS)NativeMethods.Linux64DsmEntryIdentity(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DataArgumentTypes.IDENTITY, a_msg, ref twidentitylegacy);
                            }
                            catch
                            {
                                sts = STS.NODS;
                            }
                        }

                        // Try with the latest DSM first, if we have one...
                        if (m_blFoundLatestDsm)
                        {
                            twidentitylegacy = TwidentityToTwidentitylegacy(a_twidentity);
                            twidentitylegacy.Id = 0;
                            try
                            {
                                sts = (STS)NativeMethods.LinuxDsmEntryIdentity(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DataArgumentTypes.IDENTITY, a_msg, ref twidentitylegacy);
                            }
                            catch
                            {
                                sts = STS.NODS;
                            }
                        }

                        // We got it...
                        if (sts == STS.SUCCESS)
                        {
                            a_twidentity = TwidentitylegacyToTwidentity(twidentitylegacy);
                            m_linuxdsm = LinuxDsm.IsLatestDsm;
                        }

                        // No joy, so try the old DSM...
                        else if (m_blFound020302Dsm64bit)
                        {
                            TW_IDENTITY_LINUX64 twidentitylinux64App = TwidentityToTwidentitylinux64(m_twidentityApp);
                            TW_IDENTITY_LINUX64 twidentitylinux64 = TwidentityToTwidentitylinux64(a_twidentity);
                            twidentitylinux64.Id = 0;
                            try
                            {
                                sts = (STS)NativeMethods.Linux020302Dsm64bitEntryIdentity(ref twidentitylinux64App, IntPtr.Zero, a_dg, DataArgumentTypes.IDENTITY, a_msg, ref twidentitylinux64);
                            }
                            catch
                            {
                                sts = STS.NODS;
                            }

                            // We got it...
                            if (sts == STS.SUCCESS)
                            {
                                a_twidentity = Twidentitylinux64ToTwidentity(twidentitylinux64);
                                m_linuxdsm = LinuxDsm.Is020302Dsm64bit;
                            }
                        }
                    }

                    // TBD: figure out how to safely do a set on Linux...
                    else if (a_msg == Messages.SET)
                    {
                        // Just pretend we did it...
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
                TW_IDENTITY_MACOSX twidentitymacosx = TwidentityToTwidentitymacosx(a_twidentity);
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryIdentity(ref m_twidentitymacosxApp, IntPtr.Zero, a_dg, DataArgumentTypes.IDENTITY, a_msg, ref twidentitymacosx);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryIdentity(ref m_twidentitymacosxApp, IntPtr.Zero, a_dg, DataArgumentTypes.IDENTITY, a_msg, ref twidentitymacosx);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
                a_twidentity = TwidentitymacosxToTwidentity(twidentitymacosx);
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
                Log.LogSendAfter(stsRcOrCc, IdentityToCsv(a_twidentity));
            }

            // If we opened, go to state 4...
            if (a_msg == Messages.OPENDS)
            {
                if (sts == STS.SUCCESS)
                {
                    // Change our state, and record the identity we picked...
                    m_state = STATE.S4;
                    m_twidentityDs = a_twidentity;
                    m_twidentitylegacyDs = TwidentityToTwidentitylegacy(m_twidentityDs);
                    m_twidentitymacosxDs = TwidentityToTwidentitymacosx(m_twidentityDs);

                    // update language
                    Language.Set(m_twidentityDs.Version.Language);

                    // Register for callbacks...

                    // Windows...
                    if (ms_platform == Platform.WINDOWS)
                    {
                        if (m_blUseCallbacks)
                        {
                            TW_CALLBACK twcallback = new TW_CALLBACK();
                            twcallback.CallBackProc = Marshal.GetFunctionPointerForDelegate(m_windowsdsmentrycontrolcallbackdelegate);
                            // Log it...
                            if (Log.GetLevel() > 0)
                            {
                                Log.LogSendBefore(a_dg.ToString(), DataArgumentTypes.CALLBACK.ToString(), a_msg.ToString(), CallbackToCsv(twcallback));
                            }
                            // Issue the command...
                            try
                            {
                                if (m_blUseLegacyDSM)
                                {
                                    sts = (STS)NativeMethods.WindowsTwain32DsmEntryCallback(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, DataGroups.CONTROL, DataArgumentTypes.CALLBACK, Messages.REGISTER_CALLBACK, ref twcallback);
                                }
                                else
                                {
                                    sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryCallback(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, DataGroups.CONTROL, DataArgumentTypes.CALLBACK, Messages.REGISTER_CALLBACK, ref twcallback);
                                }
                            }
                            catch (Exception exception)
                            {
                                // The driver crashed...
                                Log.Error("crash - " + exception.Message);
                                Log.LogSendAfter(STS.BUMMER, "");
                                return (STS.BUMMER);
                            }
                            // Log it...
                            if (Log.GetLevel() > 0)
                            {
                                Log.LogSendAfter(sts, "");
                            }
                        }
                    }

                    // Linux...
                    else if (ms_platform == Platform.LINUX)
                    {
                        TW_CALLBACK twcallback = new TW_CALLBACK();
                        twcallback.CallBackProc = Marshal.GetFunctionPointerForDelegate(m_linuxdsmentrycontrolcallbackdelegate);
                        // Log it...
                        if (Log.GetLevel() > 0)
                        {
                            Log.LogSendBefore(a_dg.ToString(), DataArgumentTypes.CALLBACK.ToString(), Messages.REGISTER_CALLBACK.ToString(), CallbackToCsv(twcallback));
                        }
                        // Issue the command...
                        try
                        {
                            if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                            {
                                sts = (STS)NativeMethods.Linux64DsmEntryCallback(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, DataGroups.CONTROL, DataArgumentTypes.CALLBACK, Messages.REGISTER_CALLBACK, ref twcallback);
                            }
                            else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                            {
                                sts = (STS)NativeMethods.LinuxDsmEntryCallback(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, DataGroups.CONTROL, DataArgumentTypes.CALLBACK, Messages.REGISTER_CALLBACK, ref twcallback);
                            }
                            else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                            {
                                sts = (STS)NativeMethods.Linux020302Dsm64bitEntryCallback(ref m_twidentityApp, ref m_twidentityDs, DataGroups.CONTROL, DataArgumentTypes.CALLBACK, Messages.REGISTER_CALLBACK, ref twcallback);
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
                        // Log it...
                        if (Log.GetLevel() > 0)
                        {
                            Log.LogSendAfter(sts, "");
                        }
                    }

                    // Mac OS X, which has to be different...
                    else if (ms_platform == Platform.MACOSX)
                    {
                        IntPtr intptr = IntPtr.Zero;
                        TW_CALLBACK twcallback = new TW_CALLBACK();
                        twcallback.CallBackProc = Marshal.GetFunctionPointerForDelegate(m_macosxdsmentrycontrolcallbackdelegate);
                        // Log it...
                        if (Log.GetLevel() > 0)
                        {
                            Log.LogSendBefore(a_dg.ToString(), DataArgumentTypes.CALLBACK.ToString(), a_msg.ToString(), CallbackToCsv(twcallback));
                        }
                        // Issue the command...
                        try
                        {
                            if (m_blUseLegacyDSM)
                            {
                                sts = (STS)NativeMethods.MacosxTwainDsmEntryCallback(ref m_twidentitymacosxApp, intptr, DataGroups.CONTROL, DataArgumentTypes.CALLBACK, Messages.REGISTER_CALLBACK, ref twcallback);
                            }
                            else
                            {
                                sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryCallback(ref m_twidentitymacosxApp, ref m_twidentityDs, DataGroups.CONTROL, DataArgumentTypes.CALLBACK, Messages.REGISTER_CALLBACK, ref twcallback);
                            }
                        }
                        catch (Exception exception)
                        {
                            // The driver crashed...
                            Log.Error("crash - " + exception.Message);
                            Log.LogSendAfter(STS.BUMMER, "");
                            return (STS.BUMMER);
                        }
                        // Log it...
                        if (Log.GetLevel() > 0)
                        {
                            Log.LogSendAfter(sts, "");
                        }
                    }
                }
            }

            // If we closed, go to state 3...
            else if (a_msg == Messages.CLOSEDS)
            {
                if (sts == STS.SUCCESS)
                {
                    m_state = STATE.S3;
                }
            }

            // All done...
            return (stsRcOrCc);
        }
    }
}
