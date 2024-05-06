using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Generic
{

    /// <summary>
    /// Flags used in TW_MEMORY structure.
    /// </summary>
    public enum TWMF : ushort
    {
        APPOWNS = 0x0001,
        DSMOWNS = 0x0002,
        DSOWNS = 0x0004,
        POINTER = 0x0008,
        HANDLE = 0x0010
    }
}
