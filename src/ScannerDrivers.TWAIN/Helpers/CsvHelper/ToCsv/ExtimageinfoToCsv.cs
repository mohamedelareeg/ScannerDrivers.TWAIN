using ScannerDrivers.TWAIN.Contants.Generic;
using ScannerDrivers.TWAIN.Contants.Structure;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;
using static ScannerDrivers.TWAIN.Contants.Enum.ExtendedImage;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of an extimageinfo to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twextimageinfo">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string ExtimageinfoToCsv(TW_EXTIMAGEINFO a_twextimageinfo)
        {
            try
            {
                uint uTwinfo = 0;
                CSV csv = new CSV();
                csv.Add(a_twextimageinfo.NumInfos.ToString());
                for (int ii = 0; (ii < a_twextimageinfo.NumInfos) && (ii < 200); ii++)
                {
                    TWEI twei;
                    TWTY twty;
                    STS sts;
                    TW_INFO twinfo = default(TW_INFO);
                    a_twextimageinfo.Get(uTwinfo, ref twinfo);
                    twei = (TWEI)twinfo.InfoId;
                    if (twei.ToString() != twinfo.InfoId.ToString())
                    {
                        csv.Add("TWEI_" + twei.ToString());
                    }
                    else
                    {
                        csv.Add(string.Format("0x{0:X}", twinfo.InfoId));
                    }
                    twty = (TWTY)twinfo.ItemType;
                    if (twty.ToString() != twinfo.ItemType.ToString())
                    {
                        csv.Add("TWTY_" + twty.ToString());
                    }
                    else
                    {
                        csv.Add(string.Format("0x{0:X}", twinfo.ItemType));
                    }
                    csv.Add(twinfo.NumItems.ToString());
                    sts = (STS)twinfo.ReturnCode;
                    if (sts.ToString() != twinfo.ReturnCode.ToString())
                    {
                        csv.Add("TWRC_" + sts.ToString());
                    }
                    else
                    {
                        csv.Add(string.Format("0x{0:X}", twinfo.ReturnCode));
                    }
                    csv.Add(twinfo.Item.ToString());
                    uTwinfo += 1;
                }
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
