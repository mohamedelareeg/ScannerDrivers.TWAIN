using ScannerDrivers.TWAIN.Contants.Capability.Enum;
using ScannerDrivers.TWAIN.Contants.DataArgumentType;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {

        /// <summary>
        /// Convert the contents of an image mem xfer structure to a string that
        /// we can show in our simple GUI...
        /// </summary>
        /// <param name="a_twimagememxfer">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string ImagememxferToCsv(TW_IMAGEMEMXFER a_twimagememxfer)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add("TWCP_" + (ICAP_COMPRESSION)a_twimagememxfer.Compression);
                csv.Add(a_twimagememxfer.BytesPerRow.ToString());
                csv.Add(a_twimagememxfer.Columns.ToString());
                csv.Add(a_twimagememxfer.Rows.ToString());
                csv.Add(a_twimagememxfer.XOffset.ToString());
                csv.Add(a_twimagememxfer.YOffset.ToString());
                csv.Add(a_twimagememxfer.BytesWritten.ToString());
                csv.Add(a_twimagememxfer.Memory.Flags.ToString());
                csv.Add(a_twimagememxfer.Memory.Length.ToString());
                csv.Add(a_twimagememxfer.Memory.TheMem.ToString());
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
