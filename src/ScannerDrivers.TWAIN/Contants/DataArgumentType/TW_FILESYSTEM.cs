using ScannerDrivers.TWAIN.Contants.Type;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{

    /// <summary>
    /// Provides information about the currently selected device.
    /// TBD -- need a 32/64 bit solution for this mess
    /// </summary>
    [SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable", MessageId = "ModifiedTimeDate")]
    [SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable", MessageId = "CreateTimeDate")]
    [StructLayout(LayoutKind.Explicit, Pack = 2)]
    public struct TW_FILESYSTEM
    {
        [FieldOffset(0)]
        public TW_STR255 InputName;

        [FieldOffset(256)]
        public TW_STR255 OutputName;

        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        [FieldOffset(512)]
        public nint Context;

        [FieldOffset(520)]
        public int Recursive;
        [FieldOffset(520)]
        public ushort Subdirectories;

        [FieldOffset(524)]
        public int FileType;
        [FieldOffset(524)]
        public uint FileSystemType;

        [FieldOffset(528)]
        public uint Size;

        [FieldOffset(532)]
        public TW_STR32 CreateTimeDate;

        [FieldOffset(566)]
        public TW_STR32 ModifiedTimeDate;

        [FieldOffset(600)]
        public uint FreeSpace;

        [FieldOffset(604)]
        public uint NewImageSize;

        [FieldOffset(608)]
        public uint NumberOfFiles;

        [FieldOffset(612)]
        public uint NumberOfSnippets;

        [FieldOffset(616)]
        public uint DeviceGroupMask;

        [FieldOffset(620)]
        public byte Reserved;

        [FieldOffset(1127)] // 620 + 508 - 1
        private byte ReservedEnd;
    }
}
