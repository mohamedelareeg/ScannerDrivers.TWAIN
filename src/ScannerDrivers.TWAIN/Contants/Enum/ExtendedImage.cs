using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Enum
{
    public static class ExtendedImage
    {
        /// <summary>
        /// Extended Image Info Attributes...
        /// </summary>
        public enum TWEI : ushort
        {
            BARCODEX = 0x1200,
            BARCODEY = 0x1201,
            BARCODETEXT = 0x1202,
            BARCODETYPE = 0x1203,
            DESHADETOP = 0x1204,
            DESHADELEFT = 0x1205,
            DESHADEHEIGHT = 0x1206,
            DESHADEWIDTH = 0x1207,
            DESHADESIZE = 0x1208,
            SPECKLESREMOVED = 0x1209,
            HORZLINEXCOORD = 0x120A,
            HORZLINEYCOORD = 0x120B,
            HORZLINELENGTH = 0x120C,
            HORZLINETHICKNESS = 0x120D,
            VERTLINEXCOORD = 0x120E,
            VERTLINEYCOORD = 0x120F,
            VERTLINELENGTH = 0x1210,
            VERTLINETHICKNESS = 0x1211,
            PATCHCODE = 0x1212,
            ENDORSEDTEXT = 0x1213,
            FORMCONFIDENCE = 0x1214,
            FORMTEMPLATEMATCH = 0x1215,
            FORMTEMPLATEPAGEMATCH = 0x1216,
            FORMHORZDOCOFFSET = 0x1217,
            FORMVERTDOCOFFSET = 0x1218,
            BARCODECOUNT = 0x1219,
            BARCODECONFIDENCE = 0x121A,
            BARCODEROTATION = 0x121B,
            BARCODETEXTLENGTH = 0x121C,
            DESHADECOUNT = 0x121D,
            DESHADEBLACKCOUNTOLD = 0x121E,
            DESHADEBLACKCOUNTNEW = 0x121F,
            DESHADEBLACKRLMIN = 0x1220,
            DESHADEBLACKRLMAX = 0x1221,
            DESHADEWHITECOUNTOLD = 0x1222,
            DESHADEWHITECOUNTNEW = 0x1223,
            DESHADEWHITERLMIN = 0x1224,
            DESHADEWHITERLAVE = 0x1225,
            DESHADEWHITERLMAX = 0x1226,
            BLACKSPECKLESREMOVED = 0x1227,
            WHITESPECKLESREMOVED = 0x1228,
            HORZLINECOUNT = 0x1229,
            VERTLINECOUNT = 0x122A,
            DESKEWSTATUS = 0x122B,
            SKEWORIGINALANGLE = 0x122C,
            SKEWFINALANGLE = 0x122D,
            SKEWCONFIDENCE = 0x122E,
            SKEWWINDOWX1 = 0x122F,
            SKEWWINDOWY1 = 0x1230,
            SKEWWINDOWX2 = 0x1231,
            SKEWWINDOWY2 = 0x1232,
            SKEWWINDOWX3 = 0x1233,
            SKEWWINDOWY3 = 0x1234,
            SKEWWINDOWX4 = 0x1235,
            SKEWWINDOWY4 = 0x1236,
            BOOKNAME = 0x1238,
            CHAPTERNUMBER = 0x1239,
            DOCUMENTNUMBER = 0x123A,
            PAGENUMBER = 0x123B,
            CAMERA = 0x123C,
            FRAMENUMBER = 0x123D,
            FRAME = 0x123E,
            PIXELFLAVOR = 0x123F,
            ICCPROFILE = 0x1240,
            LASTSEGMENT = 0x1241,
            SEGMENTNUMBER = 0x1242,
            MAGDATA = 0x1243,
            MAGTYPE = 0x1244,
            PAGESIDE = 0x1245,
            FILESYSTEMSOURCE = 0x1246,
            IMAGEMERGED = 0x1247,
            MAGDATALENGTH = 0x1248,
            PAPERCOUNT = 0x1249,
            PRINTERTEXT = 0x124A,
            TWAINDIRECTMETADATA = 0x124B,
            IAFIELDA_VALUE = 0x124C,
            IAFIELDB_VALUE = 0x124D,
            IAFIELDC_VALUE = 0x124E,
            IAFIELDD_VALUE = 0x124F,
            IAFIELDE_VALUE = 0x1250,
            IALEVEL = 0x1251,
            PRINTER = 0x1252,
            BARCODETEXT2 = 0x1253
        }
        public enum TWEJ : ushort
        {
            NONE = 0x0000,
            MIDSEPARATOR = 0x0001,
            PATCH1 = 0x0002,
            PATCH2 = 0x0003,
            PATCH3 = 0x0004,
            PATCH4 = 0x0005,
            PATCH6 = 0x0006,
            PATCHT = 0x0007
        }
    }
}
