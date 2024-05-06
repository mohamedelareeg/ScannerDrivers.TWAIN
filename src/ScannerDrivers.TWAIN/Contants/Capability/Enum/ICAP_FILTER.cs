using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{
    /// <summary>
    /// ICAP_FILTER values
    /// </summary>
    public enum ICAP_FILTER : ushort
    {
        RED = 0,
        GREEN = 1,
        BLUE = 2,
        NONE = 3,
        WHITE = 4,
        CYAN = 5,
        MAGENTA = 6,
        YELLOW = 7,
        BLACK = 8
    }
}
