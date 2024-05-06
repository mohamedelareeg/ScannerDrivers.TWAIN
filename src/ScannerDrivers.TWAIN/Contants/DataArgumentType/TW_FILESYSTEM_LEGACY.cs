using ScannerDrivers.TWAIN.Contants.Type;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{
    [SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable", MessageId = "ModifiedTimeDate")]
    [StructLayout(LayoutKind.Explicit, Pack = 2)]
    public struct TW_FILESYSTEM_LEGACY
    {
        [FieldOffset(0)]
        public TW_STR255 InputName;

        [FieldOffset(256)]
        public TW_STR255 OutputName;

        [FieldOffset(512)]
        public uint Context;

        [FieldOffset(516)]
        public int Recursive;
        [FieldOffset(516)]
        public ushort Subdirectories;

        [FieldOffset(520)]
        public int FileType;
        [FieldOffset(520)]
        public uint FileSystemType;

        [FieldOffset(524)]
        public uint Size;

        [FieldOffset(528)]
        public TW_STR32 CreateTimeDate;

        [FieldOffset(562)]
        public TW_STR32 ModifiedTimeDate;

        [FieldOffset(596)]
        public uint FreeSpace;

        [FieldOffset(600)]
        public uint NewImageSize;

        [FieldOffset(604)]
        public uint NumberOfFiles;

        [FieldOffset(608)]
        public uint NumberOfSnippets;

        [FieldOffset(612)]
        public uint DeviceGroupMask;

        [FieldOffset(616)]
        public byte Reserved;

        [FieldOffset(1123)] // 616 + 508 - 1
        private byte ReservedEnd;
    }
}
