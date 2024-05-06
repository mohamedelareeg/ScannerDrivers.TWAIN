using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Structure
{
    /// <summary>
    /// DAT_FILTER...
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_FILTER_DESCRIPTOR
    {
        public uint Size;
        public uint HueStart;
        public uint HueEnd;
        public uint SaturationStart;
        public uint SaturationEnd;
        public uint ValueStart;
        public uint ValueEnd;
        public uint Replacement;
    }
}
