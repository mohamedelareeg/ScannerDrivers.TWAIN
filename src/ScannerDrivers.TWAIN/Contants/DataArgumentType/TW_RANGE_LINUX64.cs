using ScannerDrivers.TWAIN.Contants.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_RANGE_LINUX64
    {
        public TWTY ItemType;
        public ulong MinValue;
        public ulong MaxValue;
        public ulong StepSize;
        public ulong DefaultValue;
        public ulong CurrentValue;
    }
}
