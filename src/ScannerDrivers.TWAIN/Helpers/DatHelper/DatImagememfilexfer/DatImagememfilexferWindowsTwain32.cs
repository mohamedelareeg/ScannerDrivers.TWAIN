using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Issue memory file image transfer commands...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twimagememxfer">IMAGEMEMXFER structure</param>
        /// <returns>TWAIN status</returns>
        private void DatImagememfilexferWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatImagememfilexfer.sts = (STS)NativeMethods.WindowsTwain32DsmEntryImagememfilexfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatImagememfilexfer.dg,
                m_threaddataDatImagememfilexfer.dat,
                m_threaddataDatImagememfilexfer.msg,
                ref m_threaddataDatImagememfilexfer.twimagememxfer
            );
        }
    }
}
