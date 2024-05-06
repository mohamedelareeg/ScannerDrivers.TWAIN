using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{

    /// <summary>
    /// CAP_PRINTERMODE values
    /// </summary>
    public enum CAP_PRINTERMODE : ushort
    {
        SINGLESTRING = 0,
        MULTISTRING = 1,
        COMPOUNDSTRING = 2,
        IMAGEADDRESSSTRING = 3
    }
}
