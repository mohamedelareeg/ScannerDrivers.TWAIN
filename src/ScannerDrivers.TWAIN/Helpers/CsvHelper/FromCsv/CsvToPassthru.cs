using ScannerDrivers.TWAIN.Contants.DataArgumentType;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {

        /// <summary>
        /// Convert the contents of a string to a passthru structure...
        /// </summary>
        /// <param name="a_twpassthru">A TWAIN structure</param>
        /// <param name="a_szPassthru">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToPassthru(ref TW_PASSTHRU a_twpassthru, string a_szPassthru)
        {
            // Init stuff...
            a_twpassthru = default(TW_PASSTHRU);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szPassthru);

                // Sort out the frame...
                a_twpassthru.pCommand = (IntPtr)UInt64.Parse(asz[0]);
                a_twpassthru.CommandBytes = uint.Parse(asz[1]);
                a_twpassthru.Direction = int.Parse(asz[2]);
                a_twpassthru.pData = (IntPtr)UInt64.Parse(asz[3]);
                a_twpassthru.DataBytes = uint.Parse(asz[4]);
                a_twpassthru.DataBytesXfered = uint.Parse(asz[5]);
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
