using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Enum
{
    /// <summary>
    /// Data Groups...
    /// </summary>
    public enum DataGroups : uint
    {
        CONTROL = 0x1,
        IMAGE = 0x2,
        AUDIO = 0x4,

        // More Data Functionality may be added in the future.
        // These are for items that need to be determined before DS is opened.
        // NOTE: Supported Functionality constants must be powers of 2 as they are
        //       used as bitflags when Application asks DSM to present a list of DSs.
        //       to support backward capability the App and DS will not use the fields
        DSM2 = 0x10000000,
        APP2 = 0x20000000,
        DS2 = 0x40000000,
        MASK = 0xFFFF
    }
}
