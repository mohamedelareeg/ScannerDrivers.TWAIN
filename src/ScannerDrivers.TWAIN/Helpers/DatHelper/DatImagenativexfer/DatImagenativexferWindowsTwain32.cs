using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {

        /// <summary>
        /// Issue native image transfer commands...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_bitmap">BITMAP structure</param>
        /// <returns>TWAIN status</returns>
        private void DatImagenativexferWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatImagenativexfer.sts = (STS)NativeMethods.WindowsTwain32DsmEntryImagenativexfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatImagenativexfer.dg,
                m_threaddataDatImagenativexfer.dat,
                m_threaddataDatImagenativexfer.msg,
                ref m_threaddataDatImagenativexfer.intptrBitmap
            );
        }
    }
}
