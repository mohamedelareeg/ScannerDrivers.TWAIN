using ScannerDrivers.TWAIN.Contants.Structure;
using System.Runtime.InteropServices;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{
    /// <summary>
    /// This structure holds the color palette information.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_PALETTE8
    {
        public ushort Flags;
        public ushort Length;
        public TW_ELEMENT8 Colors_000;
    }
}
