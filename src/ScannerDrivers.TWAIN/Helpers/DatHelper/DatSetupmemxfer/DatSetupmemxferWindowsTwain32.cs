using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Get info about the memory xfer...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twsetupmemxfer">SETUPMEMXFER structure</param>
        /// <returns>TWAIN status</returns>
        private void DatSetupmemxferWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatSetupmemxfer.sts = (STS)NativeMethods.WindowsTwain32DsmEntrySetupmemxfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatSetupmemxfer.dg,
                m_threaddataDatSetupmemxfer.dat,
                m_threaddataDatSetupmemxfer.msg,
                ref m_threaddataDatSetupmemxfer.twsetupmemxfer
            );
        }
    }
}
