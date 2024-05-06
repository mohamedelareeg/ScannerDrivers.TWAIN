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
        private void DatIdentityWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            if (GetState() <= STATE.S3)
            {
                m_threaddataDatIdentity.sts = (STS)NativeMethods.WindowsTwain32DsmEntryIdentity
                (
                    ref m_twidentitylegacyApp,
                    IntPtr.Zero,
                    m_threaddataDatIdentity.dg,
                    m_threaddataDatIdentity.dat,
                    m_threaddataDatIdentity.msg,
                    ref m_threaddataDatIdentity.twidentitylegacy
                );
            }
            // Man, I'm learning stupid new stuff all the time, so the old DSM
            // had to have the destination.  Argh...
            else
            {
                m_threaddataDatIdentity.sts = (STS)NativeMethods.WindowsTwain32DsmEntryIdentityState4
                (
                    ref m_twidentitylegacyApp,
                    ref m_twidentitylegacyDs,
                    m_threaddataDatIdentity.dg,
                    m_threaddataDatIdentity.dat,
                    m_threaddataDatIdentity.msg,
                    ref m_threaddataDatIdentity.twidentitylegacy
                );
            }
        }
    }
}
