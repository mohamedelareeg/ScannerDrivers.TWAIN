using ScannerDrivers.TWAIN.Contants.Type;
using System.Runtime.InteropServices;

namespace ScannerDrivers.TWAIN.Contants.Structure
{
    /// <summary>
    /// Information about audio data.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_AUDIOINFO
    {
        public TW_STR255 Name;
        public uint Reserved;
    }
}
