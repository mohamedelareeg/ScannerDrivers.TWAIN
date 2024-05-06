using ScannerDrivers.TWAIN.Contants.Enum;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Monitor for DAT.NULL / MSG.* stuff...
        /// </summary>
        /// <param name="a_intptrHwnd">Window handle that we're monitoring</param>
        /// <param name="a_iMsg">A message</param>
        /// <param name="a_intptrWparam">a parameter for the message</param>
        /// <param name="a_intptrLparam">another parameter for the message</param>
        /// <returns></returns>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust", Unrestricted = false)]
        public bool PreFilterMessage
        (
            IntPtr a_intptrHwnd,
            int a_iMsg,
            IntPtr a_intptrWparam,
            IntPtr a_intptrLparam
        )
        {
            STS sts;
            MESSAGE msg;

            // This is only in effect after MSG.ENABLEDS*, or if we are in
            // the middle of processing DAT_USERINTERFACE.  We don't want to
            // bump to state 5 before processing DAT_USERINTERFACE, but some
            // drivers are really eager to get going, and throw MSG_XFERREADY
            // before even get a chance to finish the command...
            if ((m_state < STATE.S5) && !m_blRunningDatUserinterface)
            {
                return (false);
            }

            // Convert the data...
            msg = new MESSAGE();
            msg.hwnd = a_intptrHwnd;
            msg.message = (uint)a_iMsg;
            msg.wParam = a_intptrWparam;
            msg.lParam = a_intptrLparam;

            // Allocate memory that we can give to the driver...
            if (m_tweventPreFilterMessage.pEvent == IntPtr.Zero)
            {
                m_tweventPreFilterMessage.pEvent = Marshal.AllocHGlobal(Marshal.SizeOf(msg) + 65536);
            }
            Marshal.StructureToPtr(msg, m_tweventPreFilterMessage.pEvent, true);

            // See if the driver wants the event...
            m_tweventPreFilterMessage.TWMessage = 0;
            sts = DatEvent(DataGroups.CONTROL, Messages.PROCESSEVENT, ref m_tweventPreFilterMessage, true);
            if ((sts != STS.DSEVENT) && (sts != STS.NOTDSEVENT))
            {
                return (false);
            }

            // All done, tell the app we consumed the event if we
            // got back a status telling us that...
            return (sts == STS.DSEVENT);
        }
    }
}
