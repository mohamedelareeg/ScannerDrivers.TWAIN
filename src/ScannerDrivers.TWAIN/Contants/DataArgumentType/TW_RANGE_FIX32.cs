using ScannerDrivers.TWAIN.Contants.Generic;
using ScannerDrivers.TWAIN.Contants.Structure;
using System.Runtime.InteropServices;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_RANGE_FIX32
    {
        public TWTY ItemType;
        public TW_FIX32 MinValue;
        public TW_FIX32 MaxValue;
        public TW_FIX32 StepSize;
        public TW_FIX32 DefaultValue;
        public TW_FIX32 CurrentValue;
    }
}
