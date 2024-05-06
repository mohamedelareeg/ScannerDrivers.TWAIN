using ScannerDrivers.TWAIN.Contants.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{
    /// <summary>
    /// Stores a single value (item) which describes a capability.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_ONEVALUE
    {
        public TWTY ItemType;
        // public uint Item;
    }
}
