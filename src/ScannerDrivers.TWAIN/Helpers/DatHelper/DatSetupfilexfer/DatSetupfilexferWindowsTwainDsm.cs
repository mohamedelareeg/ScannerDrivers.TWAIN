using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Get/Set for a file xfer...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twsetupfilexfer">SETUPFILEXFER structure</param>
        /// <returns>TWAIN status</returns>
        private void DatSetupfilexferWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatSetupfilexfer.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntrySetupfilexfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatSetupfilexfer.dg,
                m_threaddataDatSetupfilexfer.dat,
                m_threaddataDatSetupfilexfer.msg,
                ref m_threaddataDatSetupfilexfer.twsetupfilexfer
            );
        }
    }
}
