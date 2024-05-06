using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{
    /// <summary>
    /// CAP_DEVICEEVENT values
    /// </summary>
    public enum CAP_DEVICEEVENT : ushort
    {
        CUSTOMEVENTS = 0x8000,
        CHECKAUTOMATICCAPTURE = 0,
        CHECKBATTERY = 1,
        CHECKDEVICEONLINE = 2,
        CHECKFLASH = 3,
        CHECKPOWERSUPPLY = 4,
        CHECKRESOLUTION = 5,
        DEVICEADDED = 6,
        DEVICEOFFLINE = 7,
        DEVICEREADY = 8,
        DEVICEREMOVED = 9,
        IMAGECAPTURED = 10,
        IMAGEDELETED = 11,
        PAPERDOUBLEFEED = 12,
        PAPERJAM = 13,
        LAMPFAILURE = 14,
        POWERSAVE = 15,
        POWERSAVENOTIFY = 16
    }
}