using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Get/Set image info information...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twimageinfo">IMAGEINFO structure</param>
        /// <returns>TWAIN status</returns>
        private void DatImageinfoWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatImageinfo.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryImageinfo
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatImageinfo.dg,
                m_threaddataDatImageinfo.dat,
                m_threaddataDatImageinfo.msg,
                ref m_threaddataDatImageinfo.twimageinfo
            );
        }
    }
}
