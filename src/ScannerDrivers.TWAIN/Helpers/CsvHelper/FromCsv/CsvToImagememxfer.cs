using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a string to an image mem xfer structure...
        /// </summary>
        /// <param name="a_twimagememxfer">A TWAIN structure</param>
        /// <param name="a_szImagememxfer">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToImagememxfer(ref TW_IMAGEMEMXFER a_twimagememxfer, string a_szImagememxfer)
        {
            // Init stuff...
            a_twimagememxfer = default(TW_IMAGEMEMXFER);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szImagememxfer);

                // Sort out the structure...
                a_twimagememxfer.Compression = ushort.Parse(CvtCapValueFromEnum(Capabilities.ICAP_COMPRESSION, asz[0]));
                a_twimagememxfer.BytesPerRow = uint.Parse(asz[1]);
                a_twimagememxfer.Columns = uint.Parse(asz[2]);
                a_twimagememxfer.Rows = uint.Parse(asz[3]);
                a_twimagememxfer.XOffset = uint.Parse(asz[4]);
                a_twimagememxfer.YOffset = uint.Parse(asz[5]);
                a_twimagememxfer.BytesWritten = uint.Parse(asz[6]);
                a_twimagememxfer.Memory.Flags = ushort.Parse(asz[7]);
                a_twimagememxfer.Memory.Length = uint.Parse(asz[8]);
                a_twimagememxfer.Memory.TheMem = (IntPtr)ulong.Parse(asz[9]);
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
