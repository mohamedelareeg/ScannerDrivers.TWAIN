using ScannerDrivers.TWAIN.Contants.Capability.Enum;
using ScannerDrivers.TWAIN.Contants.DataArgumentType;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a image info to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twimageinfo">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string ImageinfoToCsv(TW_IMAGEINFO a_twimageinfo)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(((double)a_twimageinfo.XResolution.Whole + ((double)a_twimageinfo.XResolution.Frac / 65536.0)).ToString());
                csv.Add(((double)a_twimageinfo.YResolution.Whole + ((double)a_twimageinfo.YResolution.Frac / 65536.0)).ToString());
                csv.Add(a_twimageinfo.ImageWidth.ToString());
                csv.Add(a_twimageinfo.ImageLength.ToString());
                csv.Add(a_twimageinfo.SamplesPerPixel.ToString());
                csv.Add(a_twimageinfo.BitsPerSample_0.ToString());
                csv.Add(a_twimageinfo.BitsPerSample_1.ToString());
                csv.Add(a_twimageinfo.BitsPerSample_2.ToString());
                csv.Add(a_twimageinfo.BitsPerSample_3.ToString());
                csv.Add(a_twimageinfo.BitsPerSample_4.ToString());
                csv.Add(a_twimageinfo.BitsPerSample_5.ToString());
                csv.Add(a_twimageinfo.BitsPerSample_6.ToString());
                csv.Add(a_twimageinfo.BitsPerSample_7.ToString());
                csv.Add(a_twimageinfo.Planar.ToString());
                csv.Add("TWPT_" + (ICAP_PIXELTYPE)a_twimageinfo.PixelType);
                csv.Add("TWCP_" + (ICAP_COMPRESSION)a_twimageinfo.Compression);
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
