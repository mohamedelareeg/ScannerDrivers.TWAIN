using ScannerDrivers.TWAIN.Contants.DataArgumentType;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a metrics structure to a string that
        /// we can show in our simple GUI...
        /// </summary>
        /// <param name="a_twmetrics">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string MetricsToCsv(TW_METRICS a_twmetrics)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twmetrics.SizeOf.ToString());
                csv.Add(a_twmetrics.ImageCount.ToString());
                csv.Add(a_twmetrics.SheetCount.ToString());
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
