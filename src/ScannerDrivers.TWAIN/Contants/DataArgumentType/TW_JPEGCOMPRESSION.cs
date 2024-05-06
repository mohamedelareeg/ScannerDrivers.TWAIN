using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{
    /// <summary>
    /// Describes the information necessary to transfer a JPEG-compressed image.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_JPEGCOMPRESSION
    {
        public ushort ColorSpace;
        public uint SubSampling;
        public ushort NumComponents;
        public ushort QuantMap_0;
        public ushort QuantMap_1;
        public ushort QuantMap_2;
        public ushort QuantMap_3;
        public TW_MEMORY QuantTable_0;
        public TW_MEMORY QuantTable_1;
        public TW_MEMORY QuantTable_2;
        public TW_MEMORY QuantTable_3;
        public ushort HuffmanMap_0;
        public ushort HuffmanMap_1;
        public ushort HuffmanMap_2;
        public ushort HuffmanMap_3;
        public TW_MEMORY HuffmanDC_0;
        public TW_MEMORY HuffmanDC_1;
        public TW_MEMORY HuffmanAC_0;
        public TW_MEMORY HuffmanAC_2;
    }
}
