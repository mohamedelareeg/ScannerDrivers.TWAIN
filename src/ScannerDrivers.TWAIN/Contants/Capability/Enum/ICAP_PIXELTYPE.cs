using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{

    /// <summary>
    /// ICAP_PIXELTYPE values (PT_ means Pixel Type)
    /// </summary>
    public enum ICAP_PIXELTYPE : ushort
    {
        BW = 0,
        GRAY = 1,
        RGB = 2,
        PALETTE = 3,
        CMY = 4,
        CMYK = 5,
        YUV = 6,
        YUVK = 7,
        CIEXYZ = 8,
        LAB = 9,
        SRGB = 10,
        SCRGB = 11,
        INFRARED = 16
    }
}
