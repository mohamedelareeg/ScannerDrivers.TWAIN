using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_IMAGEMEMXFER_LINUX64
    {
        public ushort Compression;
        public ulong BytesPerRow;
        public ulong Columns;
        public ulong Rows;
        public ulong XOffset;
        public ulong YOffset;
        public ulong BytesWritten;
        public ulong MemoryFlags;
        public ulong MemoryLength;
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public nint MemoryTheMem;
    }
}
