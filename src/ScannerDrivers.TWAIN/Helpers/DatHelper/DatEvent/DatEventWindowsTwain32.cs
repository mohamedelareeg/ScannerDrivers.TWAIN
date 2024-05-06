using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Issue event commands...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twevent">EVENT structure</param>
        /// <returns>TWAIN status</returns>
        private void DatEventWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatEvent.sts = (STS)NativeMethods.WindowsTwain32DsmEntryEvent
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatEvent.dg,
                m_threaddataDatEvent.dat,
                m_threaddataDatEvent.msg,
                ref m_threaddataDatEvent.twevent
            );
        }
    }
}
