using ScannerDrivers.TWAIN.Contants.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Structure
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_ENUMERATION_LINUX64
    {
        public TWTY ItemType;
        public ulong NumItems;
        public ulong CurrentIndex;
        public ulong DefaultIndex;
        //public byte[] ItemList;
    }
}
