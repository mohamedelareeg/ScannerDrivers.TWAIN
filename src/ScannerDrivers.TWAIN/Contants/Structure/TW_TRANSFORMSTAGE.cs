using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Structure
{
    /// <summary>
    /// Stores a Fixed point number in two parts, a whole and a fractional part.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_TRANSFORMSTAGE
    {
        public TW_DECODEFUNCTION Decode_0;
        public TW_DECODEFUNCTION Decode_1;
        public TW_DECODEFUNCTION Decode_2;
        public TW_FIX32 Mix_0_0;
        public TW_FIX32 Mix_0_1;
        public TW_FIX32 Mix_0_2;
        public TW_FIX32 Mix_1_0;
        public TW_FIX32 Mix_1_1;
        public TW_FIX32 Mix_1_2;
        public TW_FIX32 Mix_2_0;
        public TW_FIX32 Mix_2_1;
        public TW_FIX32 Mix_2_2;
    }
}
