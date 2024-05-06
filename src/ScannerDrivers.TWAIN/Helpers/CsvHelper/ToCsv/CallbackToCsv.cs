using ScannerDrivers.TWAIN.Contants.Structure;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a callback to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twcallback">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string CallbackToCsv(TW_CALLBACK a_twcallback)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twcallback.CallBackProc.ToString());
                csv.Add(a_twcallback.RefCon.ToString());
                csv.Add(a_twcallback.Message.ToString());
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
