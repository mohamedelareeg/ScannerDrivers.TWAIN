using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Issue pendingxfers commands...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twpendingxfers">PENDINGXFERS structure</param>
        /// <returns>TWAIN status</returns>
        private void DatPendingxfersWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatPendingxfers.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryPendingxfers
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatPendingxfers.dg,
                m_threaddataDatPendingxfers.dat,
                m_threaddataDatPendingxfers.msg,
                ref m_threaddataDatPendingxfers.twpendingxfers
            );
        }
    }
}
