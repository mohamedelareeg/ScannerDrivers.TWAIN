using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Structure
{

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct TW_ENUMERATION_MACOSX
    {
        public uint ItemType;
        public uint NumItems;
        public uint CurrentIndex;
        public uint DefaultIndex;
        //public byte[] ItemList;
    }
}
