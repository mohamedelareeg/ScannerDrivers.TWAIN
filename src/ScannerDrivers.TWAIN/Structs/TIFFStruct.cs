using System.Runtime.InteropServices;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {

        // A TIFF header is composed of tags...
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TiffTag
        {
            public TiffTag(ushort a_u16Tag, ushort a_u16Type, uint a_u32Count, uint a_u32Value)
            {
                u16Tag = a_u16Tag;
                u16Type = a_u16Type;
                u32Count = a_u32Count;
                u32Value = a_u32Value;
            }

            public ushort u16Tag;
            public ushort u16Type;
            public uint u32Count;
            public uint u32Value;
        }

        // TIFF header for Uncompressed BITONAL images...
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TiffBitonalUncompressed
        {
            // Constructor...
            public TiffBitonalUncompressed(uint a_u32Width, uint a_u32Height, uint a_u32Resolution, uint a_u32Size)
            {
                // Header...
                u16ByteOrder = 0x4949;
                u16Version = 42;
                u32OffsetFirstIFD = 8;

                // First IFD...
                u16IFD = 16;

                // Tags...
                tifftagNewSubFileType = new TiffTag(254, 4, 1, 0);
                tifftagSubFileType = new TiffTag(255, 3, 1, 1);
                tifftagImageWidth = new TiffTag(256, 4, 1, a_u32Width);
                tifftagImageLength = new TiffTag(257, 4, 1, a_u32Height);
                tifftagBitsPerSample = new TiffTag(258, 3, 1, 1);
                tifftagCompression = new TiffTag(259, 3, 1, 1);
                tifftagPhotometricInterpretation = new TiffTag(262, 3, 1, 1);
                tifftagFillOrder = new TiffTag(266, 3, 1, 1);
                tifftagStripOffsets = new TiffTag(273, 4, 1, 222);
                tifftagSamplesPerPixel = new TiffTag(277, 3, 1, 1);
                tifftagRowsPerStrip = new TiffTag(278, 4, 1, a_u32Height);
                tifftagStripByteCounts = new TiffTag(279, 4, 1, a_u32Size);
                tifftagXResolution = new TiffTag(282, 5, 1, 206);
                tifftagYResolution = new TiffTag(283, 5, 1, 214);
                tifftagT4T6Options = new TiffTag(292, 4, 1, 0);
                tifftagResolutionUnit = new TiffTag(296, 3, 1, 2);

                // Footer...
                u32NextIFD = 0;
                u64XResolution = (ulong)0x100000000 + (ulong)a_u32Resolution;
                u64YResolution = (ulong)0x100000000 + (ulong)a_u32Resolution;
            }

            // Header...
            public ushort u16ByteOrder;
            public ushort u16Version;
            public uint u32OffsetFirstIFD;

            // First IFD...
            public ushort u16IFD;

            // Tags...
            public TiffTag tifftagNewSubFileType;
            public TiffTag tifftagSubFileType;
            public TiffTag tifftagImageWidth;
            public TiffTag tifftagImageLength;
            public TiffTag tifftagBitsPerSample;
            public TiffTag tifftagCompression;
            public TiffTag tifftagPhotometricInterpretation;
            public TiffTag tifftagFillOrder;
            public TiffTag tifftagStripOffsets;
            public TiffTag tifftagSamplesPerPixel;
            public TiffTag tifftagRowsPerStrip;
            public TiffTag tifftagStripByteCounts;
            public TiffTag tifftagXResolution;
            public TiffTag tifftagYResolution;
            public TiffTag tifftagT4T6Options;
            public TiffTag tifftagResolutionUnit;

            // Footer...
            public uint u32NextIFD;
            public ulong u64XResolution;
            public ulong u64YResolution;
        }

        // TIFF header for Group4 BITONAL images...
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TiffBitonalG4
        {
            // Constructor...
            public TiffBitonalG4(uint a_u32Width, uint a_u32Height, uint a_u32Resolution, uint a_u32Size)
            {
                // Header...
                u16ByteOrder = 0x4949;
                u16Version = 42;
                u32OffsetFirstIFD = 8;

                // First IFD...
                u16IFD = 16;

                // Tags...
                tifftagNewSubFileType = new TiffTag(254, 4, 1, 0);
                tifftagSubFileType = new TiffTag(255, 3, 1, 1);
                tifftagImageWidth = new TiffTag(256, 4, 1, a_u32Width);
                tifftagImageLength = new TiffTag(257, 4, 1, a_u32Height);
                tifftagBitsPerSample = new TiffTag(258, 3, 1, 1);
                tifftagCompression = new TiffTag(259, 3, 1, 4);
                tifftagPhotometricInterpretation = new TiffTag(262, 3, 1, 0);
                tifftagFillOrder = new TiffTag(266, 3, 1, 1);
                tifftagStripOffsets = new TiffTag(273, 4, 1, 222);
                tifftagSamplesPerPixel = new TiffTag(277, 3, 1, 1);
                tifftagRowsPerStrip = new TiffTag(278, 4, 1, a_u32Height);
                tifftagStripByteCounts = new TiffTag(279, 4, 1, a_u32Size);
                tifftagXResolution = new TiffTag(282, 5, 1, 206);
                tifftagYResolution = new TiffTag(283, 5, 1, 214);
                tifftagT4T6Options = new TiffTag(293, 4, 1, 0);
                tifftagResolutionUnit = new TiffTag(296, 3, 1, 2);

                // Footer...
                u32NextIFD = 0;
                u64XResolution = (ulong)0x100000000 + (ulong)a_u32Resolution;
                u64YResolution = (ulong)0x100000000 + (ulong)a_u32Resolution;
            }

            // Header...
            public ushort u16ByteOrder;
            public ushort u16Version;
            public uint u32OffsetFirstIFD;

            // First IFD...
            public ushort u16IFD;

            // Tags...
            public TiffTag tifftagNewSubFileType;
            public TiffTag tifftagSubFileType;
            public TiffTag tifftagImageWidth;
            public TiffTag tifftagImageLength;
            public TiffTag tifftagBitsPerSample;
            public TiffTag tifftagCompression;
            public TiffTag tifftagPhotometricInterpretation;
            public TiffTag tifftagFillOrder;
            public TiffTag tifftagStripOffsets;
            public TiffTag tifftagSamplesPerPixel;
            public TiffTag tifftagRowsPerStrip;
            public TiffTag tifftagStripByteCounts;
            public TiffTag tifftagXResolution;
            public TiffTag tifftagYResolution;
            public TiffTag tifftagT4T6Options;
            public TiffTag tifftagResolutionUnit;

            // Footer...
            public uint u32NextIFD;
            public ulong u64XResolution;
            public ulong u64YResolution;
        }

        // TIFF header for Uncompressed GRAYSCALE images...
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TiffGrayscaleUncompressed
        {
            // Constructor...
            public TiffGrayscaleUncompressed(uint a_u32Width, uint a_u32Height, uint a_u32Resolution, uint a_u32Size)
            {
                // Header...
                u16ByteOrder = 0x4949;
                u16Version = 42;
                u32OffsetFirstIFD = 8;

                // First IFD...
                u16IFD = 14;

                // Tags...
                tifftagNewSubFileType = new TiffTag(254, 4, 1, 0);
                tifftagSubFileType = new TiffTag(255, 3, 1, 1);
                tifftagImageWidth = new TiffTag(256, 4, 1, a_u32Width);
                tifftagImageLength = new TiffTag(257, 4, 1, a_u32Height);
                tifftagBitsPerSample = new TiffTag(258, 3, 1, 8);
                tifftagCompression = new TiffTag(259, 3, 1, 1);
                tifftagPhotometricInterpretation = new TiffTag(262, 3, 1, 1);
                tifftagStripOffsets = new TiffTag(273, 4, 1, 198);
                tifftagSamplesPerPixel = new TiffTag(277, 3, 1, 1);
                tifftagRowsPerStrip = new TiffTag(278, 4, 1, a_u32Height);
                tifftagStripByteCounts = new TiffTag(279, 4, 1, a_u32Size);
                tifftagXResolution = new TiffTag(282, 5, 1, 182);
                tifftagYResolution = new TiffTag(283, 5, 1, 190);
                tifftagResolutionUnit = new TiffTag(296, 3, 1, 2);

                // Footer...
                u32NextIFD = 0;
                u64XResolution = (ulong)0x100000000 + (ulong)a_u32Resolution;
                u64YResolution = (ulong)0x100000000 + (ulong)a_u32Resolution;
            }

            // Header...
            public ushort u16ByteOrder;
            public ushort u16Version;
            public uint u32OffsetFirstIFD;

            // First IFD...
            public ushort u16IFD;

            // Tags...
            public TiffTag tifftagNewSubFileType;
            public TiffTag tifftagSubFileType;
            public TiffTag tifftagImageWidth;
            public TiffTag tifftagImageLength;
            public TiffTag tifftagBitsPerSample;
            public TiffTag tifftagCompression;
            public TiffTag tifftagPhotometricInterpretation;
            public TiffTag tifftagStripOffsets;
            public TiffTag tifftagSamplesPerPixel;
            public TiffTag tifftagRowsPerStrip;
            public TiffTag tifftagStripByteCounts;
            public TiffTag tifftagXResolution;
            public TiffTag tifftagYResolution;
            public TiffTag tifftagResolutionUnit;

            // Footer...
            public uint u32NextIFD;
            public ulong u64XResolution;
            public ulong u64YResolution;
        }

        // TIFF header for Uncompressed COLOR images...
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TiffColorUncompressed
        {
            // Constructor...
            public TiffColorUncompressed(uint a_u32Width, uint a_u32Height, uint a_u32Resolution, uint a_u32Size)
            {
                // Header...
                u16ByteOrder = 0x4949;
                u16Version = 42;
                u32OffsetFirstIFD = 8;

                // First IFD...
                u16IFD = 14;

                // Tags...
                tifftagNewSubFileType = new TiffTag(254, 4, 1, 0);
                tifftagSubFileType = new TiffTag(255, 3, 1, 1);
                tifftagImageWidth = new TiffTag(256, 4, 1, a_u32Width);
                tifftagImageLength = new TiffTag(257, 4, 1, a_u32Height);
                tifftagBitsPerSample = new TiffTag(258, 3, 3, 182);
                tifftagCompression = new TiffTag(259, 3, 1, 1);
                tifftagPhotometricInterpretation = new TiffTag(262, 3, 1, 2);
                tifftagStripOffsets = new TiffTag(273, 4, 1, 204);
                tifftagSamplesPerPixel = new TiffTag(277, 3, 1, 3);
                tifftagRowsPerStrip = new TiffTag(278, 4, 1, a_u32Height);
                tifftagStripByteCounts = new TiffTag(279, 4, 1, a_u32Size);
                tifftagXResolution = new TiffTag(282, 5, 1, 188);
                tifftagYResolution = new TiffTag(283, 5, 1, 196);
                tifftagResolutionUnit = new TiffTag(296, 3, 1, 2);

                // Footer...
                u32NextIFD = 0;
                u16XBitsPerSample1 = 8;
                u16XBitsPerSample2 = 8;
                u16XBitsPerSample3 = 8;
                u64XResolution = (ulong)0x100000000 + (ulong)a_u32Resolution;
                u64YResolution = (ulong)0x100000000 + (ulong)a_u32Resolution;
            }

            // Header...
            public ushort u16ByteOrder;
            public ushort u16Version;
            public uint u32OffsetFirstIFD;

            // First IFD...
            public ushort u16IFD;

            // Tags...
            public TiffTag tifftagNewSubFileType;
            public TiffTag tifftagSubFileType;
            public TiffTag tifftagImageWidth;
            public TiffTag tifftagImageLength;
            public TiffTag tifftagBitsPerSample;
            public TiffTag tifftagCompression;
            public TiffTag tifftagPhotometricInterpretation;
            public TiffTag tifftagStripOffsets;
            public TiffTag tifftagSamplesPerPixel;
            public TiffTag tifftagRowsPerStrip;
            public TiffTag tifftagStripByteCounts;
            public TiffTag tifftagXResolution;
            public TiffTag tifftagYResolution;
            public TiffTag tifftagResolutionUnit;

            // Footer...
            public uint u32NextIFD;
            public ushort u16XBitsPerSample1;
            public ushort u16XBitsPerSample2;
            public ushort u16XBitsPerSample3;
            public ulong u64XResolution;
            public ulong u64YResolution;
        }


        //Compressed
        // TIFF header for Compressed GRAYSCALE images...
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TiffGrayscaleCompressed
        {
            // Constructor...
            public TiffGrayscaleCompressed(uint a_u32Width, uint a_u32Height, uint a_u32Resolution, uint a_u32Size)
            {
                // Header...
                u16ByteOrder = 0x4949;
                u16Version = 42;
                u32OffsetFirstIFD = 8;

                // First IFD...
                u16IFD = 14;

                // Tags...
                tifftagNewSubFileType = new TiffTag(254, 4, 1, 0);
                tifftagSubFileType = new TiffTag(255, 3, 1, 1);
                tifftagImageWidth = new TiffTag(256, 4, 1, a_u32Width);
                tifftagImageLength = new TiffTag(257, 4, 1, a_u32Height);
                tifftagBitsPerSample = new TiffTag(258, 3, 1, 8);
                tifftagCompression = new TiffTag(259, 3, 1, 6); // Compression type 6 for JPEG compression
                tifftagPhotometricInterpretation = new TiffTag(262, 3, 1, 1);
                tifftagStripOffsets = new TiffTag(273, 4, 1, 198);
                tifftagSamplesPerPixel = new TiffTag(277, 3, 1, 1);
                tifftagRowsPerStrip = new TiffTag(278, 4, 1, a_u32Height);
                tifftagStripByteCounts = new TiffTag(279, 4, 1, a_u32Size);
                tifftagXResolution = new TiffTag(282, 5, 1, 182);
                tifftagYResolution = new TiffTag(283, 5, 1, 190);
                tifftagResolutionUnit = new TiffTag(296, 3, 1, 2);

                // Footer...
                u32NextIFD = 0;
                u64XResolution = (ulong)0x100000000 + (ulong)a_u32Resolution;
                u64YResolution = (ulong)0x100000000 + (ulong)a_u32Resolution;
            }

            // Header...
            public ushort u16ByteOrder;
            public ushort u16Version;
            public uint u32OffsetFirstIFD;

            // First IFD...
            public ushort u16IFD;

            // Tags...
            public TiffTag tifftagNewSubFileType;
            public TiffTag tifftagSubFileType;
            public TiffTag tifftagImageWidth;
            public TiffTag tifftagImageLength;
            public TiffTag tifftagBitsPerSample;
            public TiffTag tifftagCompression;
            public TiffTag tifftagPhotometricInterpretation;
            public TiffTag tifftagStripOffsets;
            public TiffTag tifftagSamplesPerPixel;
            public TiffTag tifftagRowsPerStrip;
            public TiffTag tifftagStripByteCounts;
            public TiffTag tifftagXResolution;
            public TiffTag tifftagYResolution;
            public TiffTag tifftagResolutionUnit;

            // Footer...
            public uint u32NextIFD;
            public ulong u64XResolution;
            public ulong u64YResolution;
        }

        // TIFF header for Compressed COLOR images...
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct TiffColorCompressed
        {
            // Constructor...
            public TiffColorCompressed(uint a_u32Width, uint a_u32Height, uint a_u32Resolution, uint a_u32Size)
            {
                // Header...
                u16ByteOrder = 0x4949;
                u16Version = 42;
                u32OffsetFirstIFD = 8;

                // First IFD...
                u16IFD = 14;

                // Tags...
                tifftagNewSubFileType = new TiffTag(254, 4, 1, 0);
                tifftagSubFileType = new TiffTag(255, 3, 1, 1);
                tifftagImageWidth = new TiffTag(256, 4, 1, a_u32Width);
                tifftagImageLength = new TiffTag(257, 4, 1, a_u32Height);
                tifftagBitsPerSample = new TiffTag(258, 3, 3, 182);
                tifftagCompression = new TiffTag(259, 3, 1, 6); // Compression type 6 JPEG compression
                tifftagPhotometricInterpretation = new TiffTag(262, 3, 1, 2); // 2 for RGB color
                tifftagStripOffsets = new TiffTag(273, 4, 1, 204);
                tifftagSamplesPerPixel = new TiffTag(277, 3, 1, 3);
                tifftagRowsPerStrip = new TiffTag(278, 4, 1, a_u32Height);
                tifftagStripByteCounts = new TiffTag(279, 4, 1, a_u32Size);
                tifftagXResolution = new TiffTag(282, 5, 1, 188);
                tifftagYResolution = new TiffTag(283, 5, 1, 196);
                tifftagResolutionUnit = new TiffTag(296, 3, 1, 2);

                // Footer...
                u32NextIFD = 0;
                u16XBitsPerSample1 = 8;
                u16XBitsPerSample2 = 8;
                u16XBitsPerSample3 = 8;
                u64XResolution = (ulong)0x100000000 + (ulong)a_u32Resolution;
                u64YResolution = (ulong)0x100000000 + (ulong)a_u32Resolution;
            }

            // Header...
            public ushort u16ByteOrder;
            public ushort u16Version;
            public uint u32OffsetFirstIFD;

            // First IFD...
            public ushort u16IFD;

            // Tags...
            public TiffTag tifftagNewSubFileType;
            public TiffTag tifftagSubFileType;
            public TiffTag tifftagImageWidth;
            public TiffTag tifftagImageLength;
            public TiffTag tifftagBitsPerSample;
            public TiffTag tifftagCompression;
            public TiffTag tifftagPhotometricInterpretation;
            public TiffTag tifftagStripOffsets;
            public TiffTag tifftagSamplesPerPixel;
            public TiffTag tifftagRowsPerStrip;
            public TiffTag tifftagStripByteCounts;
            public TiffTag tifftagXResolution;
            public TiffTag tifftagYResolution;
            public TiffTag tifftagResolutionUnit;

            // Footer...
            public uint u32NextIFD;
            public ushort u16XBitsPerSample1;
            public ushort u16XBitsPerSample2;
            public ushort u16XBitsPerSample3;
            public ulong u64XResolution;
            public ulong u64YResolution;
        }

    }
}
