using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{
    /// <summary>
    /// ICAP_BITDEPTHREDUCTION values
    /// </summary>
    public enum ICAP_BITDEPTHREDUCTION : ushort
    {
        THRESHOLD = 0,
        HALFTONE = 1,
        CUSTHALFTONE = 2,
        DIFFUSION = 3,
        DYNAMICTHRESHOLD = 4
    }
}
