using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{

    /// <summary>
    /// ICAP_IMAGEMERGE values
    /// </summary>
    public enum ICAP_IMAGEMERGE : ushort
    {
        NONE = 0,
        FRONTONTOP = 1,
        FRONTONBOTTOM = 2,
        FRONTONLEFT = 3,
        FRONTONRIGHT = 4
    }
}
