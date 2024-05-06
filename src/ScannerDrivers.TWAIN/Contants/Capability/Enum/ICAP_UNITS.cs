using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{
    /// <summary>
    /// ICAP_UNITS values (UN_ means UNits)
    /// </summary>
    public enum ICAP_UNITS : ushort
    {
        INCHES = 0,
        CENTIMETERS = 1,
        PICAS = 2,
        POINTS = 3,
        TWIPS = 4,
        PIXELS = 5,
        MILLIMETERS = 6
    }
}
