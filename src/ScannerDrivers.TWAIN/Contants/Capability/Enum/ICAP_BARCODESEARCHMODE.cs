using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{
    /// <summary>
    /// ICAP_BARCODESEARCHMODE values
    /// </summary>
    public enum ICAP_BARCODESEARCHMODE : ushort
    {
        HORZ = 0,
        VERT = 1,
        HORZVERT = 2,
        VERTHORZ = 3
    }
}
