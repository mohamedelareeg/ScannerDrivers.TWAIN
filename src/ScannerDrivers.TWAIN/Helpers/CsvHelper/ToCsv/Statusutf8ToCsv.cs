using ScannerDrivers.TWAIN.Contants.DataArgumentType;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a statusutf8 structure to a string that
        /// we can show in our simple GUI...
        /// </summary>
        /// <param name="a_twstatusutf8">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public string Statusutf8ToCsv(TW_STATUSUTF8 a_twstatusutf8)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twstatusutf8.Status.ConditionCode.ToString());
                csv.Add(a_twstatusutf8.Status.Data.ToString());
                IntPtr intptr = DsmMemLock(a_twstatusutf8.UTF8string);
                csv.Add(intptr.ToString());
                DsmMemUnlock(a_twstatusutf8.UTF8string);
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
