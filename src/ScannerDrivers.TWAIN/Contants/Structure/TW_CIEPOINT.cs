using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Structure
{
    /// <summary>
    /// Defines a CIE XYZ space tri-stimulus value.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_CIEPOINT
    {
        public TW_FIX32 X;
        public TW_FIX32 Y;
        public TW_FIX32 Z;
    }
}
