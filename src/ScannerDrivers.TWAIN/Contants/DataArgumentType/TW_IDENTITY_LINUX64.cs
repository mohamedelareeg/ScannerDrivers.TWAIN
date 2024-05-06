using ScannerDrivers.TWAIN.Contants.Type;
using System.Runtime.InteropServices;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{
    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
    public struct TW_IDENTITY_LINUX64
    {
        public ulong Id;
        public TW_VERSION Version;
        public ushort ProtocolMajor;
        public ushort ProtocolMinor;
        public ulong SupportedGroups;
        public TW_STR32 Manufacturer;
        public TW_STR32 ProductFamily;
        public TW_STR32 ProductName;
    }
}
