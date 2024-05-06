using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Issue DSM commands...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_intptrHwnd">PARENT structure</param>
        /// <returns>TWAIN status</returns>
        private void DatParentWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatParent.sts = (STS)NativeMethods.WindowsTwain32DsmEntryParent
            (
                ref m_twidentitylegacyApp,
                IntPtr.Zero,
                m_threaddataDatParent.dg,
                m_threaddataDatParent.dat,
                m_threaddataDatParent.msg,
                ref m_threaddataDatParent.intptrHwnd
            );
        }
    }
}
