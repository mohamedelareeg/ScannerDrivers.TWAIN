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
    /// Stores a range of individual values describing a capability.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_RANGE
    {
        public TWTY ItemType;
        public uint MinValue;
        public uint MaxValue;
        public uint StepSize;
        public uint DefaultValue;
        public uint CurrentValue;
    }
}
