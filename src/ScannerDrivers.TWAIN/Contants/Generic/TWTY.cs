using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Generic
{
    /// <summary>
    /// Type values...
    /// </summary>
    public enum TWTY : ushort
    {
        INT8 = 0x0000,
        INT16 = 0x0001,
        INT32 = 0x0002,

        UINT8 = 0x0003,
        UINT16 = 0x0004,
        UINT32 = 0x0005,

        BOOL = 0x0006,

        FIX32 = 0x0007,

        FRAME = 0x0008,

        STR32 = 0x0009,
        STR64 = 0x000a,
        STR128 = 0x000b,
        STR255 = 0x000c,
        HANDLE = 0x000f
    }
}
