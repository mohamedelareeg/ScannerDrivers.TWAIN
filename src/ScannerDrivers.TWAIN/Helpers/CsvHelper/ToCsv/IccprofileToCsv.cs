using ScannerDrivers.TWAIN.Contants.DataArgumentType;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of an iccprofile to a string that we can
        /// show in our simple GUI...
        /// </summary>
        /// <param name="a_twmemory">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string IccprofileToCsv(TW_MEMORY a_twmemory)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twmemory.Flags.ToString());
                csv.Add(a_twmemory.Length.ToString());
                csv.Add(a_twmemory.TheMem.ToString());
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
