using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_IMAGEMEMXFER_MACOSX
    {
        public uint Compression;
        public uint BytesPerRow;
        public uint Columns;
        public uint Rows;
        public uint XOffset;
        public uint YOffset;
        public uint BytesWritten;
        public TW_MEMORY Memory;
    }

}
