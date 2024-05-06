using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Issue file audio transfer commands...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <returns>TWAIN status</returns>
        private void DatAudiofilexferWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatAudiofilexfer.sts = (STS)NativeMethods.WindowsTwain32DsmEntryAudiofilexfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatAudiofilexfer.dg,
                m_threaddataDatAudiofilexfer.dat,
                m_threaddataDatAudiofilexfer.msg,
                IntPtr.Zero
            );
        }
    }
}
