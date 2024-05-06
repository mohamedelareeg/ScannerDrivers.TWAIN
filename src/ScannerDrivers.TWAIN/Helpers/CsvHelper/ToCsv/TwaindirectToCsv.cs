using ScannerDrivers.TWAIN.Contants.DataArgumentType;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a twaindirect structure to a string that
        /// we can show in our simple GUI...
        /// </summary>
        /// <param name="a_twtwaindirect">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string TwaindirectToCsv(TW_TWAINDIRECT a_twtwaindirect)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twtwaindirect.SizeOf.ToString());
                csv.Add(a_twtwaindirect.CommunicationManager.ToString());
                csv.Add(a_twtwaindirect.Send.ToString());
                csv.Add(a_twtwaindirect.SendSize.ToString());
                csv.Add(a_twtwaindirect.Receive.ToString());
                csv.Add(a_twtwaindirect.ReceiveSize.ToString());
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
