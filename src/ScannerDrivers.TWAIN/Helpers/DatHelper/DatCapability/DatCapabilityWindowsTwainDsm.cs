using System.Security.Permissions;
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
        /// <param name="a_twcapability">CAPABILITY structure</param>
        /// <returns>TWAIN status</returns>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust", Unrestricted = false)]
        private void DatCapabilityWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatCapability.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryCapability
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatCapability.dg,
                m_threaddataDatCapability.dat,
                m_threaddataDatCapability.msg,
                ref m_threaddataDatCapability.twcapability
            );
        }
    }
}
