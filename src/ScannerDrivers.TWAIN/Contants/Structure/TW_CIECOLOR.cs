using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Structure
{

    /// <summary>
    /// Defines the mapping from an RGB color space device into CIE 1931 (XYZ) color space.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_CIECOLOR
    {
        public ushort ColorSpace;
        public short LowEndian;
        public short DeviceDependent;
        public int VersionNumber;
        public TW_TRANSFORMSTAGE StageABC;
        public TW_TRANSFORMSTAGE StageLNM;
        public TW_CIEPOINT WhitePoint;
        public TW_CIEPOINT BlackPoint;
        public TW_CIEPOINT WhitePaper;
        public TW_CIEPOINT BlackInk;
        public TW_FIX32 Samples;
    }
}
