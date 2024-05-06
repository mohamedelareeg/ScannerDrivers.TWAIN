using ScannerDrivers.TWAIN.Contants.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct TW_RANGE_FIX32_MACOSX
    {
        public uint ItemType;
        public TW_FIX32 MinValue;
        public TW_FIX32 MaxValue;
        public TW_FIX32 StepSize;
        public TW_FIX32 DefaultValue;
        public TW_FIX32 CurrentValue;
    }
}
