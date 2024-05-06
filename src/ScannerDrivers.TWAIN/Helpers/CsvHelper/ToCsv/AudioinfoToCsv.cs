using ScannerDrivers.TWAIN.Contants.Structure;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of an audio info to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twaudioinfo">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string AudioinfoToCsv(TW_AUDIOINFO a_twaudioinfo)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twaudioinfo.Name.Get());
                csv.Add(a_twaudioinfo.Reserved.ToString());
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
