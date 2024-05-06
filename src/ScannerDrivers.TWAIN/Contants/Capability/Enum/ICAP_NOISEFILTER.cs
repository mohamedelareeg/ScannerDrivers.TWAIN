using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{
    /// <summary>
    /// ICAP_NOISEFILTER values
    /// </summary>
    public enum ICAP_NOISEFILTER : ushort
    {
        NONE = 0,
        AUTO = 1,
        LONEPIXEL = 2,
        MAJORITYRULE = 3
    }
}
