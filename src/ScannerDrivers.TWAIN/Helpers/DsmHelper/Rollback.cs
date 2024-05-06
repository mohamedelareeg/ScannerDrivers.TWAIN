using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Rollback the TWAIN state machine to the specified value, with an
        /// automatic resync if it detects a sequence error...
        /// </summary>
        /// <param name="a_stateTarget">The TWAIN state that we want to end up at</param>
        /// <param name="a_blUseThread">Use the thread (most cases)</param>
        static int s_iCloseDsmDelay = 0;
        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public Codes.STATE Rollback(STATE a_stateTarget, bool a_blUseThread = true)
        {
            int iRetry;
            STS sts;
            STATE stateStart;

            // Nothing to do here...
            if (m_state <= STATE.S2)
            {
                return (m_state);
            }

            // Submit the work to the TWAIN thread...
            if (a_blUseThread)
            {
                if (m_runinuithreaddelegate != null)
                {
                    lock (m_lockTwain)
                    {
                        // No point in continuing...
                        if (m_twaincommand == null)
                        {
                            return (m_state);
                        }

                        // Set the command variables...
                        ThreadData threaddata = default(ThreadData);
                        threaddata.blExitThread = true;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply, the delay
                        // is needed because Mac OS X doesn't gracefully handle
                        // the loss of a mutex...
                        s_iCloseDsmDelay = 0;
                        CallerToThreadSet();
                        ThreadToRollbackWaitOne();
                        Thread.Sleep(s_iCloseDsmDelay);

                        // Clear the command variables...
                        m_twaincommand.Delete(lIndex);
                    }
                }
                else if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
                {
                    lock (m_lockTwain)
                    {
                        // No point in continuing...
                        if (m_twaincommand == null)
                        {
                            return (m_state);
                        }

                        // Set the command variables...
                        ThreadData threaddata = default(ThreadData);
                        threaddata.stateRollback = a_stateTarget;
                        threaddata.blRollback = true;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply, the delay
                        // is needed because Mac OS X doesn't gracefully handle
                        // the loss of a mutex...
                        s_iCloseDsmDelay = 0;
                        CallerToThreadSet();
                        ThreadToRollbackWaitOne();
                        Thread.Sleep(s_iCloseDsmDelay);

                        // Clear the command variables...
                        m_twaincommand.Delete(lIndex);
                    }
                    return (m_state);
                }
            }

            // If we get a sequence error, then we'll repeat the loop from
            // the highest possible state to see if we can fix the problem...
            iRetry = 2;
            stateStart = GetState();
            while (iRetry-- > 0)
            {
                // State 7 --> State 6...
                if ((stateStart >= STATE.S7) && (a_stateTarget < STATE.S7))
                {
                    TW_PENDINGXFERS twpendingxfers = default(TW_PENDINGXFERS);
                    sts = DatPendingxfers(DataGroups.CONTROL, Messages.ENDXFER, ref twpendingxfers);
                    if (sts == STS.SEQERROR)
                    {
                        stateStart = STATE.S7;
                        continue;
                    }
                    stateStart = STATE.S6;
                }

                // State 6 --> State 5...
                if ((stateStart >= STATE.S6) && (a_stateTarget < STATE.S6))
                {
                    TW_PENDINGXFERS twpendingxfers = default(TW_PENDINGXFERS);
                    sts = DatPendingxfers(DataGroups.CONTROL, Messages.RESET, ref twpendingxfers);
                    if (sts == STS.SEQERROR)
                    {
                        stateStart = STATE.S7;
                        continue;
                    }
                    stateStart = STATE.S5;
                }

                // State 5 --> State 4...
                if ((stateStart >= STATE.S5) && (a_stateTarget < STATE.S5))
                {
                    TW_USERINTERFACE twuserinterface = default(TW_USERINTERFACE);
                    sts = DatUserinterface(DataGroups.CONTROL, Messages.DISABLEDS, ref twuserinterface);
                    if (sts == STS.SEQERROR)
                    {
                        stateStart = STATE.S7;
                        continue;
                    }
                    if (m_tweventPreFilterMessage.pEvent != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(m_tweventPreFilterMessage.pEvent);
                        m_tweventPreFilterMessage.pEvent = IntPtr.Zero;
                    }
                    stateStart = STATE.S4;
                    m_blAcceptXferReady = false;
                    m_blIsMsgclosedsok = false;
                    m_blIsMsgclosedsreq = false;
                    m_blIsMsgxferready = false;
                }

                // State 4 --> State 3...
                if ((stateStart >= STATE.S4) && (a_stateTarget < STATE.S4))
                {
                    sts = DatIdentity(DataGroups.CONTROL, Messages.CLOSEDS, ref m_twidentityDs);
                    if (sts == STS.SEQERROR)
                    {
                        stateStart = STATE.S7;
                        continue;
                    }
                    stateStart = STATE.S3;
                }

                // State 3 --> State 2...
                if ((stateStart >= STATE.S3) && (a_stateTarget < STATE.S3))
                {
                    // Do this to prevent a deadlock on Mac OS X, two seconds
                    // better be enough to finish up...
                    if (GetPlatform() == Platform.MACOSX)
                    {
                        ThreadToRollbackSet();
                        s_iCloseDsmDelay = 2000;
                    }

                    // Now do the rest of it...
                    sts = DatParent(DataGroups.CONTROL, Messages.CLOSEDSM, ref m_intptrHwnd);
                    if (sts == STS.SEQERROR)
                    {
                        stateStart = STATE.S7;
                        continue;
                    }
                    stateStart = STATE.S2;
                }

                // All done...
                break;
            }

            // How did we do?
            return (m_state);
        }
    }
}
