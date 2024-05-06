using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {

        /// <summary>
        /// Convert the contents of a string to an callback structure...
        /// </summary>
        /// <param name="a_twimageinfo">A TWAIN structure</param>
        /// <param name="a_szImageinfo">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToImageinfo(ref TW_IMAGEINFO a_twimageinfo, string a_szImageinfo)
        {
            // Init stuff...
            a_twimageinfo = default(TW_IMAGEINFO);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szImageinfo);

                // Grab the values...
                a_twimageinfo.XResolution.Whole = (short)double.Parse(asz[0]);
                a_twimageinfo.XResolution.Frac = (ushort)((double.Parse(asz[0]) - (double)a_twimageinfo.XResolution.Whole) * 65536.0);
                a_twimageinfo.YResolution.Whole = (short)double.Parse(asz[1]);
                a_twimageinfo.YResolution.Frac = (ushort)((double.Parse(asz[1]) - (double)a_twimageinfo.YResolution.Whole) * 65536.0);
                a_twimageinfo.ImageWidth = (short)double.Parse(asz[2]);
                a_twimageinfo.ImageLength = int.Parse(asz[3]);
                a_twimageinfo.SamplesPerPixel = short.Parse(asz[4]);
                a_twimageinfo.BitsPerSample_0 = short.Parse(asz[5]);
                a_twimageinfo.BitsPerSample_1 = short.Parse(asz[6]);
                a_twimageinfo.BitsPerSample_2 = short.Parse(asz[7]);
                a_twimageinfo.BitsPerSample_3 = short.Parse(asz[8]);
                a_twimageinfo.BitsPerSample_4 = short.Parse(asz[9]);
                a_twimageinfo.BitsPerSample_5 = short.Parse(asz[10]);
                a_twimageinfo.BitsPerSample_6 = short.Parse(asz[11]);
                a_twimageinfo.BitsPerSample_7 = short.Parse(asz[12]);
                a_twimageinfo.Planar = ushort.Parse(CvtCapValueFromEnum(Capabilities.ICAP_PLANARCHUNKY, asz[13]));
                a_twimageinfo.PixelType = short.Parse(CvtCapValueFromEnum(Capabilities.ICAP_PIXELTYPE, asz[14]));
                a_twimageinfo.Compression = ushort.Parse(CvtCapValueFromEnum(Capabilities.ICAP_COMPRESSION, asz[15]));
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
