using ScannerDrivers.TWAIN.Contants.DataArgumentType;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a string to an image layout structure...
        /// </summary>
        /// <param name="a_twimagelayout">A TWAIN structure</param>
        /// <param name="a_szImagelayout">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToImagelayout(ref TW_IMAGELAYOUT a_twimagelayout, string a_szImagelayout)
        {
            // Init stuff...
            a_twimagelayout = default(TW_IMAGELAYOUT);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szImagelayout);

                // Sort out the frame...
                a_twimagelayout.Frame.Left.Whole = (short)double.Parse(asz[0]);
                a_twimagelayout.Frame.Left.Frac = (ushort)((double.Parse(asz[0]) - (double)a_twimagelayout.Frame.Left.Whole) * 65536.0);
                a_twimagelayout.Frame.Top.Whole = (short)double.Parse(asz[1]);
                a_twimagelayout.Frame.Top.Frac = (ushort)((double.Parse(asz[1]) - (double)a_twimagelayout.Frame.Top.Whole) * 65536.0);
                a_twimagelayout.Frame.Right.Whole = (short)double.Parse(asz[2]);
                a_twimagelayout.Frame.Right.Frac = (ushort)((double.Parse(asz[2]) - (double)a_twimagelayout.Frame.Right.Whole) * 65536.0);
                a_twimagelayout.Frame.Bottom.Whole = (short)double.Parse(asz[3]);
                a_twimagelayout.Frame.Bottom.Frac = (ushort)((double.Parse(asz[3]) - (double)a_twimagelayout.Frame.Bottom.Whole) * 65536.0);

                // And now the counters...
                a_twimagelayout.DocumentNumber = (uint)int.Parse(asz[4]);
                a_twimagelayout.PageNumber = (uint)int.Parse(asz[5]);
                a_twimagelayout.FrameNumber = (uint)int.Parse(asz[6]);
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return (false);
            }

            // All done...
            return (true);
        }

    }
}
