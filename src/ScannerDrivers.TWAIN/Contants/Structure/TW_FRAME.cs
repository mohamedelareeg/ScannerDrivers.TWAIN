using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Structure
{
    /// <summary>
    /// Defines a frame rectangle in ICAP_UNITS coordinates.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_FRAME
    {
        public TW_FIX32 Left;
        public TW_FIX32 Top;
        public TW_FIX32 Right;
        public TW_FIX32 Bottom;
    }
}
