using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Issue capabilities commands...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twuserinterface">USERINTERFACE structure</param>
        /// <returns>TWAIN status</returns>
        private void DatUserinterfaceWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatUserinterface.sts = (STS)NativeMethods.WindowsTwain32DsmEntryUserinterface
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatUserinterface.dg,
                m_threaddataDatUserinterface.dat,
                m_threaddataDatUserinterface.msg,
                ref m_threaddataDatUserinterface.twuserinterface
            );
        }
    }
}
