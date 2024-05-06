using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{

    /// <summary>
    /// CAP_PRINTER values
    /// </summary>
    public enum CAP_PRINTER : ushort
    {
        IMPRINTERTOPBEFORE = 0,
        IMPRINTERTOPAFTER = 1,
        IMPRINTERBOTTOMBEFORE = 2,
        IMPRINTERBOTTOMAFTER = 3,
        ENDORSERTOPBEFORE = 4,
        ENDORSERTOPAFTER = 5,
        ENDORSERBOTTOMBEFORE = 6,
        ENDORSERBOTTOMAFTER = 7
    }
}
