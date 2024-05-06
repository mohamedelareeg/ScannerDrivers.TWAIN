using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{
    /// <summary>
    /// ICAP_ORIENTATION values
    /// </summary>
    public enum ICAP_ORIENTATION : ushort
    {
        ROT0 = 0,
        ROT90 = 1,
        ROT180 = 2,
        ROT270 = 3,
        PORTRAIT = ROT0,
        LANDSCAPE = ROT270,
        AUTO = 4,
        AUTOTEXT = 5,
        AUTOPICTURE = 6
    }
}
