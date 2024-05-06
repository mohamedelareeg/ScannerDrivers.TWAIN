using ScannerDrivers.TWAIN.Contants.DataArgumentType;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a pending xfers structure to a string that
        /// we can show in our simple GUI...
        /// </summary>
        /// <param name="a_twsetupfilexfer">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string PendingxfersToCsv(TW_PENDINGXFERS a_twpendingxfers)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twpendingxfers.Count.ToString());
                csv.Add(a_twpendingxfers.EOJ.ToString());
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
