using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {

        /// <summary>
        /// Issue memory image transfer commands...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twimagememxfer">IMAGEMEMXFER structure</param>
        /// <returns>TWAIN status</returns>
        private void DatImagememxferWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatImagememxfer.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryImagememxfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatImagememxfer.dg,
                m_threaddataDatImagememxfer.dat,
                m_threaddataDatImagememxfer.msg,
                ref m_threaddataDatImagememxfer.twimagememxfer
            );
        }
    }
}
