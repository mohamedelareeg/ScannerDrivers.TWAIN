using ScannerDrivers.TWAIN.Contants.DataArgumentType;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a image layout to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twimagelayout">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string ImagelayoutToCsv(TW_IMAGELAYOUT a_twimagelayout)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(((double)a_twimagelayout.Frame.Left.Whole + ((double)a_twimagelayout.Frame.Left.Frac / 65536.0)).ToString());
                csv.Add(((double)a_twimagelayout.Frame.Top.Whole + ((double)a_twimagelayout.Frame.Top.Frac / 65536.0)).ToString());
                csv.Add(((double)a_twimagelayout.Frame.Right.Whole + ((double)a_twimagelayout.Frame.Right.Frac / 65536.0)).ToString());
                csv.Add(((double)a_twimagelayout.Frame.Bottom.Whole + ((double)a_twimagelayout.Frame.Bottom.Frac / 65536.0)).ToString());
                csv.Add(a_twimagelayout.DocumentNumber.ToString());
                csv.Add(a_twimagelayout.PageNumber.ToString());
                csv.Add(a_twimagelayout.FrameNumber.ToString());
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
