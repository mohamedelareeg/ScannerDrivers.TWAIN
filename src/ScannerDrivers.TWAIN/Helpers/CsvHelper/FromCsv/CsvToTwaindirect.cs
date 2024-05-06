using ScannerDrivers.TWAIN.Contants.DataArgumentType;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert a string to a twaindirect...
        /// </summary>
        /// <param name="a_twtwaindirect">A TWAIN structure</param>
        /// <param name="a_szTwaindirect">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToTwaindirect(ref TW_TWAINDIRECT a_twtwaindirect, string a_szTwaindirect)
        {
            // Init stuff...
            a_twtwaindirect = default(TW_TWAINDIRECT);

            // Build the string...
            try
            {
                long lTmp;
                string[] asz = CSV.Parse(a_szTwaindirect);

                // Sort out the values...
                if (!uint.TryParse(asz[0], out a_twtwaindirect.SizeOf))
                {
                    return (false);
                }
                if (!ushort.TryParse(asz[1], out a_twtwaindirect.CommunicationManager))
                {
                    return (false);
                }
                if (!long.TryParse(asz[2], out lTmp))
                {
                    return (false);
                }
                a_twtwaindirect.Send = new IntPtr(lTmp);
                if (!uint.TryParse(asz[3], out a_twtwaindirect.SendSize))
                {
                    return (false);
                }
                if (!long.TryParse(asz[4], out lTmp))
                {
                    return (false);
                }
                a_twtwaindirect.Receive = new IntPtr(lTmp);
                if (!uint.TryParse(asz[5], out a_twtwaindirect.ReceiveSize))
                {
                    return (false);
                }
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
