using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Issue file image transfer commands...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <returns>TWAIN status</returns>
        private void DatImagefilexferWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatImagefilexfer.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryImagefilexfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatImagefilexfer.dg,
                m_threaddataDatImagefilexfer.dat,
                m_threaddataDatImagefilexfer.msg,
                IntPtr.Zero
            );
        }
    }
}
