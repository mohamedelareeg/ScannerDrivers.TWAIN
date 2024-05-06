using ScannerDrivers.TWAIN.Contants.Capability.Enum;
using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert a string to a setupfilexfer...
        /// </summary>
        /// <param name="a_twsetupfilexfer">A TWAIN structure</param>
        /// <param name="a_szSetupfilexfer">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToSetupfilexfer(ref TW_SETUPFILEXFER a_twsetupfilexfer, string a_szSetupfilexfer)
        {
            // Init stuff...
            a_twsetupfilexfer = default(TW_SETUPFILEXFER);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szSetupfilexfer);

                // Sort out the values...
                a_twsetupfilexfer.FileName.Set(asz[0]);
                a_twsetupfilexfer.Format = (ICAP_IMAGEFILEFORMAT)ushort.Parse(CvtCapValueFromEnum(Capabilities.ICAP_IMAGEFILEFORMAT, asz[1]));
                a_twsetupfilexfer.VRefNum = short.Parse(asz[2]);
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return (false);
            }

            // All done...
            return (true);
        }
    }
}
