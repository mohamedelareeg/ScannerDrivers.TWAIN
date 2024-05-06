using ScannerDrivers.TWAIN.Contants.DataArgumentType;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a status structure to a string that
        /// we can show in our simple GUI...
        /// </summary>
        /// <param name="a_twstatus">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string StatusToCsv(TW_STATUS a_twstatus)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twstatus.ConditionCode.ToString());
                csv.Add(a_twstatus.Data.ToString());
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
