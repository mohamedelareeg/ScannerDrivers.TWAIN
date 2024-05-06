using ScannerDrivers.TWAIN.Contants.Structure;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a string to an callback structure...
        /// </summary>
        /// <param name="a_twcallback">A TWAIN structure</param>
        /// <param name="a_szCallback">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToCallback(ref TW_CALLBACK a_twcallback, string a_szCallback)
        {
            // Init stuff...
            a_twcallback = default(TW_CALLBACK);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szCallback);

                // Grab the values...
                a_twcallback.CallBackProc = (IntPtr)UInt64.Parse(asz[0]);
                a_twcallback.RefCon = uint.Parse(asz[1]);
                a_twcallback.Message = ushort.Parse(asz[2]);
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
