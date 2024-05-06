using ScannerDrivers.TWAIN.Contants.DataArgumentType;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a setup file xfer structure to a string that
        /// we can show in our simple GUI...
        /// </summary>
        /// <param name="a_twsetupfilexfer">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string SetupfilexferToCsv(TW_SETUPFILEXFER a_twsetupfilexfer)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twsetupfilexfer.FileName.Get());
                csv.Add("TWFF_" + a_twsetupfilexfer.Format);
                csv.Add(a_twsetupfilexfer.VRefNum.ToString());
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
