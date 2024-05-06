using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{
    /// <summary>
    /// ICAP_OVERSCAN values
    /// </summary>
    public enum ICAP_OVERSCAN : ushort
    {
        NONE = 0,
        AUTO = 1,
        TOPBOTTOM = 2,
        LEFTRIGHT = 3,
        ALL = 4
    }
}
