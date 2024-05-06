using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{
    /// <summary>
    /// CAP_ALARMS values
    /// </summary>
    public enum CAP_ALARMS : ushort
    {
        ALARM = 0,
        FEEDERERROR = 1,
        FEEDERWARNING = 2,
        BARCODE = 3,
        DOUBLEFEED = 4,
        JAM = 5,
        PATCHCODE = 6,
        POWER = 7,
        SKEW = 8
    }
}
