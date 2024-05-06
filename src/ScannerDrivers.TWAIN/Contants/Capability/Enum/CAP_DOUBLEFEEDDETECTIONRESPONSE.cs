using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{

    /// <summary>
    /// CAP_DOUBLEFEEDDETECTIONRESPONSE values
    /// </summary>
    public enum CAP_DOUBLEFEEDDETECTIONRESPONSE : ushort
    {
        STOP = 0,
        STOPANDWAIT = 1,
        SOUND = 2,
        DONOTIMPRINT = 3
    }
}
