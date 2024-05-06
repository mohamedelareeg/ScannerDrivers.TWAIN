using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Structure
{

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_ARRAY_MACOSX
    {
        public uint ItemType;
        public uint NumItems;
        //public byte[] ItemList;
    }
}
