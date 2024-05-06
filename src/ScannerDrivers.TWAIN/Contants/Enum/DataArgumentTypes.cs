using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Enum
{

    /// <summary>
    /// Data Argument Types...
    /// </summary>
    public enum DataArgumentTypes : ushort//DAT
    {
        // NULL and Custom Base...
        NULL = 0x0,
        CUSTOM = 0x8000,

        // Data Argument Types for the DG_CONTROL Data Group.
        CAPABILITY = 0x1,
        EVENT = 0x2,
        IDENTITY = 0x3,
        PARENT = 0x4,
        PENDINGXFERS = 0x5,
        SETUPMEMXFER = 0x6,
        SETUPFILEXFER = 0x7,
        STATUS = 0x8,
        USERINTERFACE = 0x9,
        XFERGROUP = 0xa,
        CUSTOMDSDATA = 0xc,
        DEVICEEVENT = 0xd,
        FILESYSTEM = 0xe,
        PASSTHRU = 0xf,
        CALLBACK = 0x10,
        STATUSUTF8 = 0x11,
        CALLBACK2 = 0x12,
        METRICS = 0x13,
        TWAINDIRECT = 0x14,

        // Data Argument Types for the DG_IMAGE Data Group.
        IMAGEINFO = 0x0101,
        IMAGELAYOUT = 0x0102,
        IMAGEMEMXFER = 0x0103,
        IMAGENATIVEXFER = 0x0104,
        IMAGEFILEXFER = 0x105,
        CIECOLOR = 0x106,
        GRAYRESPONSE = 0x107,
        RGBRESPONSE = 0x108,
        JPEGCOMPRESSION = 0x109,
        PALETTE8 = 0x10a,
        EXTIMAGEINFO = 0x10b,
        FILTER = 0x10c,

        /* Data Argument Types for the DG_AUDIO Data Group. */
        AUDIOFILEXFER = 0x201,
        AUDIOINFO = 0x202,
        AUDIONATIVEXFER = 0x203,

        /* misplaced */
        ICCPROFILE = 0x401,
        IMAGEMEMFILEXFER = 0x402,
        ENTRYPOINT = 0x403
    }
}
