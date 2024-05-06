using ScannerDrivers.TWAIN.Contants.DataArgumentType;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a setup mem xfer structure to a string that
        /// we can show in our simple GUI...
        /// </summary>
        /// <param name="a_twsetupmemxfer">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string SetupmemxferToCsv(TW_SETUPMEMXFER a_twsetupmemxfer)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twsetupmemxfer.MinBufSize.ToString());
                csv.Add(a_twsetupmemxfer.MaxBufSize.ToString());
                csv.Add(a_twsetupmemxfer.Preferred.ToString());
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }
    }
}
