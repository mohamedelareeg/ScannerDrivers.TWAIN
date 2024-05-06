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
        /// Get the entrypoint data...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twentrypoint">ENTRYPOINT structure</param>
        /// <returns>TWAIN status</returns>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust", Unrestricted = false)]
        public STS DatEntrypoint(DataGroups a_dg, Messages a_msg, ref TW_ENTRYPOINT a_twentrypoint)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default(ThreadData);
                    threaddata.twentrypoint = a_twentrypoint;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = DataArgumentTypes.ENTRYPOINT;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twentrypoint = m_twaincommand.Get(lIndex).twentrypoint;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DataArgumentTypes.ENTRYPOINT.ToString(), a_msg.ToString(), EntrypointToCsv(a_twentrypoint));
            }

            // Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryEntrypoint(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.ENTRYPOINT, a_msg, ref a_twentrypoint);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryEntrypoint(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.ENTRYPOINT, a_msg, ref a_twentrypoint);
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
                    // Alright, this is super nasty.  The application issues the call to
                    // DAT_ENTRYPOINT before the call to MSG_OPENDS.  We don't know which
                    // driver they're going to use, so we don't know which DSM they're
                    // going to end up with.  This sucks in all kinds of ways.  The only
                    // reason we can hope for this to work is if all of the DSM's are in
                    // agreement about the memory functions they're using.  Lucky for us
                    // on Linux it's always been calloc/free.  So, we may be in the weird
                    // situation of using a different DSM for the memory functions, but
                    // it'l be okay.  You can stop breathing in and out of that paper bag...
                    sts = STS.BUMMER;
                    if (m_blFoundLatestDsm64)
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryEntrypoint(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.ENTRYPOINT, a_msg, ref a_twentrypoint);
                    }
                    else if (m_blFoundLatestDsm)
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryEntrypoint(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.ENTRYPOINT, a_msg, ref a_twentrypoint);
                    }
                    else if (m_blFound020302Dsm64bit)
                    {
                        TW_ENTRYPOINT_LINUX64 twentrypointlinux64 = default(TW_ENTRYPOINT_LINUX64);
                        twentrypointlinux64.Size = a_twentrypoint.Size;
                        twentrypointlinux64.DSM_MemAllocate = a_twentrypoint.DSM_MemAllocate;
                        twentrypointlinux64.DSM_MemFree = a_twentrypoint.DSM_MemFree;
                        twentrypointlinux64.DSM_MemLock = a_twentrypoint.DSM_MemLock;
                        twentrypointlinux64.DSM_MemUnlock = a_twentrypoint.DSM_MemUnlock;
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryEntrypoint(ref m_twidentityApp, ref m_twidentityDs, a_dg, DataArgumentTypes.ENTRYPOINT, a_msg, ref twentrypointlinux64);
                        a_twentrypoint = default(TW_ENTRYPOINT);
                        a_twentrypoint.Size = (uint)(twentrypointlinux64.Size & 0xFFFFFFFF);
                        a_twentrypoint.DSM_MemAllocate = twentrypointlinux64.DSM_MemAllocate;
                        a_twentrypoint.DSM_MemFree = twentrypointlinux64.DSM_MemFree;
                        a_twentrypoint.DSM_MemLock = twentrypointlinux64.DSM_MemLock;
                        a_twentrypoint.DSM_MemUnlock = twentrypointlinux64.DSM_MemUnlock;
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
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryEntrypoint(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.ENTRYPOINT, a_msg, ref a_twentrypoint);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryEntrypoint(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.ENTRYPOINT, a_msg, ref a_twentrypoint);
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

            // If we were successful, then squirrel away the data...
            if (sts == Codes.STS.SUCCESS)
            {
                m_twentrypointdelegates = default(TW_ENTRYPOINT_DELEGATES);
                m_twentrypointdelegates.Size = a_twentrypoint.Size;
                m_twentrypointdelegates.DSM_Entry = a_twentrypoint.DSM_Entry;
                if (a_twentrypoint.DSM_MemAllocate != null)
                {
                    m_twentrypointdelegates.DSM_MemAllocate = (TWAINSDK.DSM_MEMALLOC)Marshal.GetDelegateForFunctionPointer(a_twentrypoint.DSM_MemAllocate, typeof(TWAINSDK.DSM_MEMALLOC));
                }
                if (a_twentrypoint.DSM_MemFree != null)
                {
                    m_twentrypointdelegates.DSM_MemFree = (TWAINSDK.DSM_MEMFREE)Marshal.GetDelegateForFunctionPointer(a_twentrypoint.DSM_MemFree, typeof(TWAINSDK.DSM_MEMFREE));
                }
                if (a_twentrypoint.DSM_MemLock != null)
                {
                    m_twentrypointdelegates.DSM_MemLock = (TWAINSDK.DSM_MEMLOCK)Marshal.GetDelegateForFunctionPointer(a_twentrypoint.DSM_MemLock, typeof(TWAINSDK.DSM_MEMLOCK));
                }
                if (a_twentrypoint.DSM_MemUnlock != null)
                {
                    m_twentrypointdelegates.DSM_MemUnlock = (TWAINSDK.DSM_MEMUNLOCK)Marshal.GetDelegateForFunctionPointer(a_twentrypoint.DSM_MemUnlock, typeof(TWAINSDK.DSM_MEMUNLOCK));
                }
            }

            // Get DAT_STATUS, if needed...
            Codes.STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, EntrypointToCsv(a_twentrypoint));
            }

            // All done...
            return (stsRcOrCc);
        }
    }
}
