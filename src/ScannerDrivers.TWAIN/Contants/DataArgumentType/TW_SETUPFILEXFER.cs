using ScannerDrivers.TWAIN.Contants.Capability.Enum;
using ScannerDrivers.TWAIN.Contants.Type;
using System.Runtime.InteropServices;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{
    /// <summary>
    /// Describes the file format and file specification information for a transfer through a disk file.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_SETUPFILEXFER
    {
        public TW_STR255 FileName;
        public ICAP_IMAGEFILEFORMAT Format;
        public short VRefNum;
    }
}
