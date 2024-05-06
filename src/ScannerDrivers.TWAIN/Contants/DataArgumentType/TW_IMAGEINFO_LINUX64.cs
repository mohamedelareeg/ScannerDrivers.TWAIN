using ScannerDrivers.TWAIN.Contants.Structure;
using System.Runtime.InteropServices;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_IMAGEINFO_LINUX64
    {
        public TW_FIX32 XResolution;
        public TW_FIX32 YResolution;
        public int ImageWidth;
        public int ImageLength;
        public short SamplesPerPixel;
        public short BitsPerSample_0;
        public short BitsPerSample_1;
        public short BitsPerSample_2;
        public short BitsPerSample_3;
        public short BitsPerSample_4;
        public short BitsPerSample_5;
        public short BitsPerSample_6;
        public short BitsPerSample_7;
        public short BitsPerPixel;
        public ushort Planar;
        public short PixelType;
        public ushort Compression;
    }
}
