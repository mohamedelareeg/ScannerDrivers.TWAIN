using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{
    /// <summary>
    /// CAP_INDICATORSMODE values
    /// </summary>
    public enum CAP_INDICATORSMODE : ushort
    {
        INFO = 0,
        WARNING = 1,
        ERROR = 2,
        WARMUP = 3
    }
}
