using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{
    /// <summary>
    /// ICAP_JPEGSUBSAMPLING values
    /// </summary>
    public enum ICAP_JPEGSUBSAMPLING : ushort
    {
        X444YCBCR = 0, // 444YCBCR in TWAIN.H
        X444RGB = 1, // 444RGB in TWAIN.H
        X422 = 2, // 422 in TWAIN.H
        X421 = 3, // 421 in TWAIN.H
        X411 = 4, // 411 in TWAIN.H
        X420 = 5, // 420 in TWAIN.H
        X410 = 6, // 410 in TWAIN.H
        X311 = 7  // 311 in TWAIN.H
    }
}
