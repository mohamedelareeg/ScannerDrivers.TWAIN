using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {

        /// <summary>
        /// Get/Set extended image info information...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twextimageinfo">EXTIMAGEINFO structure</param>
        /// <returns>TWAIN status</returns>
        private void DatExtimageinfoWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatExtimageinfo.sts = (STS)NativeMethods.WindowsTwain32DsmEntryExtimageinfo
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatExtimageinfo.dg,
                m_threaddataDatExtimageinfo.dat,
                m_threaddataDatExtimageinfo.msg,
                ref m_threaddataDatExtimageinfo.twextimageinfo
            );
        }
    }
}
