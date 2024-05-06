using ScannerDrivers.TWAIN.Contants.Capability.Enum;
using ScannerDrivers.TWAIN.Contants.ImageAttributes;
using static ScannerDrivers.TWAIN.Contants.Enum.ExtendedImage;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert a value to the 'friendly' name, based on the TWEI...
        /// </summary>
        /// <param name="a_twei">TWEI driving the conversion</param>
        /// <param name="szValue">value to convert</param>
        /// <returns></returns>
        public static string CvtTweiValueToEnum(TWEI a_twei, string a_szValue)
        {
            switch (a_twei)
            {
                default: return (a_szValue);
                case TWEI.BARCODECONFIDENCE: return (a_szValue);
                case TWEI.BARCODECOUNT: return (a_szValue);
                case TWEI.BARCODEROTATION: return (CvtCapValueToEnumHelper<TWEI_BARCODEROTATION>(a_szValue));
                case TWEI.BARCODETEXT: return (a_szValue);
                case TWEI.BARCODETEXT2: return (a_szValue);
                case TWEI.BARCODETEXTLENGTH: return (a_szValue);
                case TWEI.BARCODETYPE: return (CvtCapValueToEnumHelper<TWBT>(a_szValue));
                case TWEI.BARCODEX: return (a_szValue);
                case TWEI.BARCODEY: return (a_szValue);
                case TWEI.BLACKSPECKLESREMOVED: return (a_szValue);
                case TWEI.BOOKNAME: return (a_szValue);
                case TWEI.CAMERA: return (a_szValue);
                case TWEI.CHAPTERNUMBER: return (a_szValue);
                case TWEI.DESHADEBLACKCOUNTNEW: return (a_szValue);
                case TWEI.DESHADEBLACKCOUNTOLD: return (a_szValue);
                case TWEI.DESHADEBLACKRLMAX: return (a_szValue);
                case TWEI.DESHADEBLACKRLMIN: return (a_szValue);
                case TWEI.DESHADECOUNT: return (a_szValue);
                case TWEI.DESHADEHEIGHT: return (a_szValue);
                case TWEI.DESHADELEFT: return (a_szValue);
                case TWEI.DESHADESIZE: return (a_szValue);
                case TWEI.DESHADETOP: return (a_szValue);
                case TWEI.DESHADEWHITECOUNTNEW: return (a_szValue);
                case TWEI.DESHADEWHITECOUNTOLD: return (a_szValue);
                case TWEI.DESHADEWHITERLAVE: return (a_szValue);
                case TWEI.DESHADEWHITERLMAX: return (a_szValue);
                case TWEI.DESHADEWHITERLMIN: return (a_szValue);
                case TWEI.DESHADEWIDTH: return (a_szValue);
                case TWEI.DESKEWSTATUS: return (CvtCapValueToEnumHelper<TWEI_DESKEWSTATUS>(a_szValue));
                case TWEI.DOCUMENTNUMBER: return (a_szValue);
                case TWEI.ENDORSEDTEXT: return (a_szValue);
                case TWEI.FILESYSTEMSOURCE: return (a_szValue);
                case TWEI.FORMCONFIDENCE: return (a_szValue);
                case TWEI.FORMHORZDOCOFFSET: return (a_szValue);
                case TWEI.FORMTEMPLATEMATCH: return (a_szValue);
                case TWEI.FORMTEMPLATEPAGEMATCH: return (a_szValue);
                case TWEI.FORMVERTDOCOFFSET: return (a_szValue);
                case TWEI.FRAME: return (a_szValue);
                case TWEI.FRAMENUMBER: return (a_szValue);
                case TWEI.HORZLINECOUNT: return (a_szValue);
                case TWEI.HORZLINELENGTH: return (a_szValue);
                case TWEI.HORZLINETHICKNESS: return (a_szValue);
                case TWEI.HORZLINEXCOORD: return (a_szValue);
                case TWEI.HORZLINEYCOORD: return (a_szValue);
                case TWEI.IAFIELDA_VALUE: return (a_szValue);
                case TWEI.IAFIELDB_VALUE: return (a_szValue);
                case TWEI.IAFIELDC_VALUE: return (a_szValue);
                case TWEI.IAFIELDD_VALUE: return (a_szValue);
                case TWEI.IAFIELDE_VALUE: return (a_szValue);
                case TWEI.IALEVEL: return (CvtCapValueToEnumHelper<CAP_IAFIELD>(a_szValue));
                case TWEI.ICCPROFILE: return (a_szValue);
                case TWEI.IMAGEMERGED: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case TWEI.LASTSEGMENT: return (CvtCapValueToEnumHelper<bool>(a_szValue));
                case TWEI.MAGDATA: return (a_szValue);
                case TWEI.MAGDATALENGTH: return (a_szValue);
                case TWEI.MAGTYPE: return (CvtCapValueToEnumHelper<TWEI_MAGTYPE>(a_szValue));
                case TWEI.PAGENUMBER: return (a_szValue);
                case TWEI.PAGESIDE: return (a_szValue);
                case TWEI.PAPERCOUNT: return (a_szValue);
                case TWEI.PATCHCODE: return (CvtCapValueToEnumHelper<TWEI_PATCHCODE>(a_szValue));
                case TWEI.PIXELFLAVOR: return (CvtCapValueToEnumHelper<ICAP_PIXELFLAVOR>(a_szValue));
                case TWEI.PRINTER: return (CvtCapValueToEnumHelper<CAP_PRINTER>(a_szValue));
                case TWEI.PRINTERTEXT: return (a_szValue);
                case TWEI.SEGMENTNUMBER: return (a_szValue);
                case TWEI.SKEWCONFIDENCE: return (a_szValue);
                case TWEI.SKEWFINALANGLE: return (a_szValue);
                case TWEI.SKEWORIGINALANGLE: return (a_szValue);
                case TWEI.SKEWWINDOWX1: return (a_szValue);
                case TWEI.SKEWWINDOWX2: return (a_szValue);
                case TWEI.SKEWWINDOWX3: return (a_szValue);
                case TWEI.SKEWWINDOWX4: return (a_szValue);
                case TWEI.SKEWWINDOWY1: return (a_szValue);
                case TWEI.SKEWWINDOWY2: return (a_szValue);
                case TWEI.SKEWWINDOWY3: return (a_szValue);
                case TWEI.SKEWWINDOWY4: return (a_szValue);
                case TWEI.SPECKLESREMOVED: return (a_szValue);
                case TWEI.TWAINDIRECTMETADATA: return (a_szValue);
                case TWEI.VERTLINECOUNT: return (a_szValue);
                case TWEI.VERTLINELENGTH: return (a_szValue);
                case TWEI.VERTLINETHICKNESS: return (a_szValue);
                case TWEI.VERTLINEXCOORD: return (a_szValue);
                case TWEI.VERTLINEYCOORD: return (a_szValue);
                case TWEI.WHITESPECKLESREMOVED: return (a_szValue);
            }
        }
    }
}
