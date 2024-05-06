using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{
    /// <summary>
    /// ICAP_COMPRESSION values
    /// </summary>
    public enum ICAP_COMPRESSION : ushort
    {
        NONE = 0,
        PACKBITS = 1,
        GROUP31D = 2,
        GROUP31DEOL = 3,
        GROUP32D = 4,
        GROUP4 = 5,
        JPEG = 6,
        LZW = 7,
        JBIG = 8,
        PNG = 9,
        RLE4 = 10,
        RLE8 = 11,
        BITFIELDS = 12,
        ZIP = 13,
        JPEG2000 = 14
    }
}
