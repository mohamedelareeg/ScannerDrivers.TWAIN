using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Issue identity commands...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twidentity">IDENTITY structure</param>
        /// <returns>TWAIN status</returns>
        private void DatIdentityWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatIdentity.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryIdentity
            (
                ref m_twidentitylegacyApp,
                IntPtr.Zero,
                m_threaddataDatIdentity.dg,
                m_threaddataDatIdentity.dat,
                m_threaddataDatIdentity.msg,
                ref m_threaddataDatIdentity.twidentitylegacy
            );
        }
    }
}
