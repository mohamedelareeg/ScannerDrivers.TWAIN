using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Structure
{
    /// <summary>
    /// Fixed point structure type.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_FIX32
    {
        public short Whole;
        public ushort Frac;
    }
}
