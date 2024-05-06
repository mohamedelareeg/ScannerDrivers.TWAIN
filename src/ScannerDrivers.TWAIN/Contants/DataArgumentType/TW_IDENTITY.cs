using ScannerDrivers.TWAIN.Contants.Type;
using System.Runtime.InteropServices;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{
    /// <summary>
    /// Provides identification information about a TWAIN entity.
    /// The use of Padding is there to allow us to use the structure
    /// with Linux 64-bit systems where the TW_INT32 and TW_UINT32
    /// types were long, and therefore 64-bits in size.  This should
    /// have no impact with well-behaved systems that have these types
    /// as 32-bit, but should prevent memory corruption in all other
    /// situations...
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
    public struct TW_IDENTITY
    {
        public ulong Id;
        public TW_VERSION Version;
        public ushort ProtocolMajor;
        public ushort ProtocolMinor;
        public uint SupportedGroups;
        public TW_STR32 Manufacturer;
        public TW_STR32 ProductFamily;
        public TW_STR32 ProductName;
    }
}
