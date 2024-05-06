using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Structure
{

    /// <summary>
    /// Defines the parameters used for channel-specific transformation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_DECODEFUNCTION
    {
        public TW_FIX32 StartIn;
        public TW_FIX32 BreakIn;
        public TW_FIX32 EndIn;
        public TW_FIX32 StartOut;
        public TW_FIX32 BreakOut;
        public TW_FIX32 EndOut;
        public TW_FIX32 Gamma;
        public TW_FIX32 SampleCount;
    }
}
