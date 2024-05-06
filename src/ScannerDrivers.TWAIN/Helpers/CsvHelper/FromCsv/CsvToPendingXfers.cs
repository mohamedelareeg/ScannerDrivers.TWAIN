using ScannerDrivers.TWAIN.Contants.DataArgumentType;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {

        /// <summary>
        /// Convert the contents of a string to a pendingxfers structure...
        /// </summary>
        /// <param name="a_twpassthru">A TWAIN structure</param>
        /// <param name="a_szPassthru">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToPendingXfers(ref TW_PENDINGXFERS a_twpendingxfers, string a_szPendingxfers)
        {
            // Init stuff...
            a_twpendingxfers = default(TW_PENDINGXFERS);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szPendingxfers);

                // Sort out the frame...
                a_twpendingxfers.Count = ushort.Parse(asz[0]);
                a_twpendingxfers.EOJ = uint.Parse(asz[1]);
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
