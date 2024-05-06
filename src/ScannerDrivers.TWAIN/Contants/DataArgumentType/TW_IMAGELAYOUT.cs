using ScannerDrivers.TWAIN.Contants.Structure;
using System.Runtime.InteropServices;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{
    /// <summary>
    /// Involves information about the original size of the acquired image.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_IMAGELAYOUT
    {
        public TW_FRAME Frame;
        public uint DocumentNumber;
        public uint PageNumber;
        public uint FrameNumber;
    }
}
