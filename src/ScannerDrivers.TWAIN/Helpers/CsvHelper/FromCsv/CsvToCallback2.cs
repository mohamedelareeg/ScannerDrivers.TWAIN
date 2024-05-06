using ScannerDrivers.TWAIN.Contants.Structure;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a string to an callback2 structure...
        /// </summary>
        /// <param name="a_twcallback2">A TWAIN structure</param>
        /// <param name="a_szCallback2">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToCallback2(ref TW_CALLBACK2 a_twcallback2, string a_szCallback2)
        {
            // Init stuff...
            a_twcallback2 = default(TW_CALLBACK2);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szCallback2);

                // Grab the values...
                a_twcallback2.CallBackProc = (IntPtr)UInt64.Parse(asz[0]);
                a_twcallback2.RefCon = (UIntPtr)UInt64.Parse(asz[1]);
                a_twcallback2.Message = ushort.Parse(asz[2]);
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
