using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        private void DatImagelayoutWindowsTwainDsm()
        {

            /// <summary>
            /// Get/Set layout information...
            /// </summary>
            /// <param name="a_dg">Data group</param>
            /// <param name="a_msg">Operation</param>
            /// <param name="a_twimagelayout">IMAGELAYOUT structure</param>
            /// <returns>TWAIN status</returns>
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatImagelayout.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryImagelayout
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatImagelayout.dg,
                m_threaddataDatImagelayout.dat,
                m_threaddataDatImagelayout.msg,
                ref m_threaddataDatImagelayout.twimagelayout
            );
        }
    }
}
