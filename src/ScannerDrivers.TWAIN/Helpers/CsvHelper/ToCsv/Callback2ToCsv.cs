using ScannerDrivers.TWAIN.Contants.Structure;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a callback2 to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twcallback2">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string Callback2ToCsv(TW_CALLBACK2 a_twcallback2)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twcallback2.CallBackProc.ToString());
                csv.Add(a_twcallback2.RefCon.ToString());
                csv.Add(a_twcallback2.Message.ToString());
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
