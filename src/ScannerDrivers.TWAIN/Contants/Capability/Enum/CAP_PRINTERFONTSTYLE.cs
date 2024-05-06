using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{
    /// <summary>
    /// CAP_PRINTERFONTSTYLE Added 2.3 (TWPF in TWAIN.H)
    /// </summary>
    public enum CAP_PRINTERFONTSTYLE : ushort
    {
        NORMAL = 0,
        BOLD = 1,
        ITALIC = 2,
        LARGESIZE = 3,
        SMALLSIZE = 4
    }
}
