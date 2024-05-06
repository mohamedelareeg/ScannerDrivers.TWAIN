using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{
    /// <summary>
    /// ICAP_LIGHTSOURCE values
    /// </summary>
    public enum ICAP_LIGHTSOURCE : ushort
    {
        RED = 0,
        GREEN = 1,
        BLUE = 2,
        NONE = 3,
        WHITE = 4,
        UV = 5,
        IR = 6
    }
}
