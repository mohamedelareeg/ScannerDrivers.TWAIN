using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Issue native audio transfer commands...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_intptr">handle</param>
        /// <returns>TWAIN status</returns>
        private void DatAudionativexferWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatAudionativexfer.sts = (STS)NativeMethods.WindowsTwain32DsmEntryAudionativexfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatAudionativexfer.dg,
                m_threaddataDatAudionativexfer.dat,
                m_threaddataDatAudionativexfer.msg,
                ref m_threaddataDatAudionativexfer.intptrAudio
            );
        }
    }
}
