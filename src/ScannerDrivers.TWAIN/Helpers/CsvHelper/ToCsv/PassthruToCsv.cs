using ScannerDrivers.TWAIN.Contants.DataArgumentType;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a patthru structure to a string that
        /// we can show in our simple GUI...
        /// </summary>
        /// <param name="a_twpassthru">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string PassthruToCsv(TW_PASSTHRU a_twpassthru)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twpassthru.pCommand.ToString());
                csv.Add(a_twpassthru.CommandBytes.ToString());
                csv.Add(a_twpassthru.Direction.ToString());
                csv.Add(a_twpassthru.pData.ToString());
                csv.Add(a_twpassthru.DataBytes.ToString());
                csv.Add(a_twpassthru.DataBytesXfered.ToString());
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
