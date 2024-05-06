using ScannerDrivers.TWAIN.Contants.DataArgumentType;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert a string to a setupmemxfer...
        /// </summary>
        /// <param name="a_twsetupfilexfer">A TWAIN structure</param>
        /// <param name="a_szSetupfilexfer">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToSetupmemxfer(ref TW_SETUPMEMXFER a_twsetupmemxfer, string a_szSetupmemxfer)
        {
            // Init stuff...
            a_twsetupmemxfer = default(TW_SETUPMEMXFER);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szSetupmemxfer);

                // Sort out the values...
                a_twsetupmemxfer.MinBufSize = uint.Parse(asz[0]);
                a_twsetupmemxfer.MaxBufSize = uint.Parse(asz[1]);
                a_twsetupmemxfer.Preferred = uint.Parse(asz[2]);
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
