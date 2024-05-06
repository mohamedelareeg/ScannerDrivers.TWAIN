using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Get some text for an error...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twstatus">STATUS structure</param>
        /// <returns>TWAIN status</returns>
        private void DatStatusWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatStatus.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryStatus
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatStatus.dg,
                m_threaddataDatStatus.dat,
                m_threaddataDatStatus.msg,
                ref m_threaddataDatStatus.twstatus
            );
        }
    }
}
