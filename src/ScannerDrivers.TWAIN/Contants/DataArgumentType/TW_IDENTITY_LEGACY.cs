using ScannerDrivers.TWAIN.Contants.Type;
using System.Runtime.InteropServices;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{
    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
    public struct TW_IDENTITY_LEGACY
    {
        public uint Id;
        public TW_VERSION Version;
        public ushort ProtocolMajor;
        public ushort ProtocolMinor;
        public uint SupportedGroups;
        public TW_STR32 Manufacturer;
        public TW_STR32 ProductFamily;
        public TW_STR32 ProductName;
        private ulong Padding; // accounts for Id and SupportedGroups
    }
}
