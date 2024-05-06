using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{

    /// <summary>
    /// CAP_DUPLEX values
    /// </summary>
    public enum CAP_DUPLEX : ushort
    {
        NONE = 0,
        X1PASSDUPLEX = 1, // 1PASSDUPLEX in TWAIN.H
        X2PASSDUPLEX = 2  // 2PASSDUPLEX in TWAIN.H
    }
}
