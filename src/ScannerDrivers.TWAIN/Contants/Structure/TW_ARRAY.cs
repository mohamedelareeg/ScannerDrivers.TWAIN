using ScannerDrivers.TWAIN.Contants.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Structure
{

    /// <summary>
    /// Stores a list of values for a capability, the ItemList is commented
    /// out so that the caller can collect information about it with a
    /// marshalling call...
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_ARRAY
    {
        public TWTY ItemType;
        public uint NumItems;
        //public byte[] ItemList;
    }
}
