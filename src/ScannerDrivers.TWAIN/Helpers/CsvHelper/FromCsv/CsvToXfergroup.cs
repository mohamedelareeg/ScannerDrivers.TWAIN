namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {

        /// <summary>
        /// Convert the contents of a string to a transfer group...
        /// </summary>
        /// <param name="a_twcustomdsdata">A TWAIN structure</param>
        /// <param name="a_szCustomdsdata">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToXfergroup(ref UInt32 a_u32Xfergroup, string a_szXfergroup)
        {
            // Init stuff...
            a_u32Xfergroup = 0;

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szXfergroup);

                // Grab the values...
                a_u32Xfergroup = asz[0].ToLower().StartsWith("0x") ? Convert.ToUInt32(asz[0].Remove(0, 2), 16) : Convert.ToUInt32(asz[0], 16);
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
