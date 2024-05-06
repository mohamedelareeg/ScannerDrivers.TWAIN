using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{
    /// <summary>
    /// CAP_JOBCONTROL values
    /// </summary>
    public enum CAP_JOBCONTROL : ushort
    {
        NONE = 0,
        JSIC = 1,
        JSIS = 2,
        JSXC = 3,
        JSXS = 4
    }
}
