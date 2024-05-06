using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Enum
{
    /// <summary>
    /// All message constants are unique.
    /// Messages are grouped according to which DATs they are used with.
    /// </summary>
    public enum Messages : ushort//MSG
    {
        // Only used to clear fields...
        NULL = 0x0,

        // Generic messages may be used with any of several DATs.
        GET = 0x1,
        GETCURRENT = 0x2,
        GETDEFAULT = 0x3,
        GETFIRST = 0x4,
        GETNEXT = 0x5,
        SET = 0x6,
        RESET = 0x7,
        QUERYSUPPORT = 0x8,
        GETHELP = 0x9,
        GETLABEL = 0xa,
        GETLABELENUM = 0xb,
        SETCONSTRAINT = 0xc,

        // Messages used with DAT_NULL.
        XFERREADY = 0x101,
        CLOSEDSREQ = 0x102,
        CLOSEDSOK = 0x103,
        DEVICEEVENT = 0x104,

        // Messages used with a pointer to DAT_PARENT data.
        OPENDSM = 0x301,
        CLOSEDSM = 0x302,

        // Messages used with a pointer to a DAT_IDENTITY structure.
        OPENDS = 0x401,
        CLOSEDS = 0x402,
        USERSELECT = 0x403,

        // Messages used with a pointer to a DAT_USERINTERFACE structure.
        DISABLEDS = 0x501,
        ENABLEDS = 0x502,
        ENABLEDSUIONLY = 0x503,

        // Messages used with a pointer to a DAT_EVENT structure.
        PROCESSEVENT = 0x601,

        // Messages used with a pointer to a DAT_PENDINGXFERS structure
        ENDXFER = 0x701,
        STOPFEEDER = 0x702,

        // Messages used with a pointer to a DAT_FILESYSTEM structure
        CHANGEDIRECTORY = 0x0801,
        CREATEDIRECTORY = 0x0802,
        DELETE = 0x0803,
        FORMATMEDIA = 0x0804,
        GETCLOSE = 0x0805,
        GETFIRSTFILE = 0x0806,
        GETINFO = 0x0807,
        GETNEXTFILE = 0x0808,
        RENAME = 0x0809,
        COPY = 0x080A,
        AUTOMATICCAPTUREDIRECTORY = 0x080B,

        // Messages used with a pointer to a DAT_PASSTHRU structure
        PASSTHRU = 0x0901,

        // used with DAT_CALLBACK
        REGISTER_CALLBACK = 0x0902,

        // used with DAT_CAPABILITY
        RESETALL = 0x0A01,

        // used with DAT_TWAINDIRECT
        SETTASK = 0x0B01
    }
}
