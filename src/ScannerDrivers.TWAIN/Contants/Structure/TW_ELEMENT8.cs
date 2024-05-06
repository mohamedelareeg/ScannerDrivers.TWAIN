using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Structure
{
    /// <summary>
    /// This structure holds the tri-stimulus color palette information for TW_PALETTE8 structures.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_ELEMENT8
    {
        public byte Index;
        public byte Channel1;
        public byte Channel2;
        public byte Channel3;
    }

}
