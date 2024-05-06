using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{

    /// <summary>
    /// ICAP_XFERMECH values (SX_ means Setup XFer)
    /// </summary>
    public enum ICAP_XFERMECH : ushort
    {
        NATIVE = 0,
        FILE = 1,
        MEMORY = 2,
        MEMFILE = 4
    }
}
