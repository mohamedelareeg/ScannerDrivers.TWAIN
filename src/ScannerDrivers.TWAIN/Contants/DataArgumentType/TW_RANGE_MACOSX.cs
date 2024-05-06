using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct TW_RANGE_MACOSX
    {
        public uint ItemType;
        public uint MinValue;
        public uint MaxValue;
        public uint StepSize;
        public uint DefaultValue;
        public uint CurrentValue;
    }
}
