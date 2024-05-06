using ScannerDrivers.TWAIN.Contants.Structure;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of an event to a string that
        /// we can show in our simple GUI...
        /// </summary>
        /// <param name="a_twevent">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string EventToCsv(TW_EVENT a_twevent)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twevent.pEvent.ToString());
                csv.Add(a_twevent.TWMessage.ToString());
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
