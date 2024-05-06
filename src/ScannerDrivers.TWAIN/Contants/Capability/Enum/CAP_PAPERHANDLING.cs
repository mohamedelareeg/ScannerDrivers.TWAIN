using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{

    /// <summary>
    /// CAP_PAPERHANDLING values
    /// </summary>
    public enum CAP_PAPERHANDLING : ushort
    {
        NORMAL = 0,
        FRAGILE = 1,
        THICK = 2,
        TRIFOLD = 3,
        PHOTOGRAPH = 4
    }

}
