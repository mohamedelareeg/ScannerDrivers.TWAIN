using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Enum
{
    public static class Codes
    {
        /// <summary>
        /// We're departing from a strict translation of TWAIN.H so that
        /// we can achieve a unified status return type.  
        /// </summary>
        public const int STSCC = 0x10000; // get us past the custom space
        public enum STS
        {
            // Custom base (same for TWRC and TWCC)...
            CUSTOMBASE = 0x8000,

            // Return codes...
            SUCCESS = 0,
            FAILURE = 1,
            CHECKSTATUS = 2,
            CANCEL = 3,
            DSEVENT = 4,
            NOTDSEVENT = 5,
            XFERDONE = 6,
            ENDOFLIST = 7,
            INFONOTSUPPORTED = 8,
            DATANOTAVAILABLE = 9,
            BUSY = 10,
            SCANNERLOCKED = 11,

            // Condition codes (always associated with TWRC_FAILURE)...
            BUMMER = STSCC + 1,
            LOWMEMORY = STSCC + 2,
            NODS = STSCC + 3,
            MAXCONNECTIONS = STSCC + 4,
            OPERATIONERROR = STSCC + 5,
            BADCAP = STSCC + 6,
            BADPROTOCOL = STSCC + 9,
            BADVALUE = STSCC + 10,
            SEQERROR = STSCC + 11,
            BADDEST = STSCC + 12,
            CAPUNSUPPORTED = STSCC + 13,
            CAPBADOPERATION = STSCC + 14,
            CAPSEQERROR = STSCC + 15,
            DENIED = STSCC + 16,
            FILEEXISTS = STSCC + 17,
            FILENOTFOUND = STSCC + 18,
            NOTEMPTY = STSCC + 19,
            PAPERJAM = STSCC + 20,
            PAPERDOUBLEFEED = STSCC + 21,
            FILEWRITEERROR = STSCC + 22,
            CHECKDEVICEONLINE = STSCC + 23,
            INTERLOCK = STSCC + 24,
            DAMAGEDCORNER = STSCC + 25,
            FOCUSERROR = STSCC + 26,
            DOCTOOLIGHT = STSCC + 27,
            DOCTOODARK = STSCC + 28,
            NOMEDIA = STSCC + 29
        }

        /// <summary>
        /// bit patterns: for query the operation that are supported by the data source on a capability
        /// Application gets these through DG_CONTROL/DAT_CAPABILITY/MSG_QUERYSUPPORT
        /// </summary>
        public enum TWQC : ushort
        {
            GET = 0x0001,
            SET = 0x0002,
            GETDEFAULT = 0x0004,
            GETCURRENT = 0x0008,
            RESET = 0x0010,
            SETCONSTRAINT = 0x0020,
            CONSTRAINABLE = 0x0040
        }

        /// <summary>
        /// The TWAIN States...
        /// </summary>
        public enum STATE
        {
            S1 = 1, // Nothing loaded or open
            S2 = 2, // DSM loaded
            S3 = 3, // DSM open
            S4 = 4, // Data Source open, programmatic mode (no GUI)
            S5 = 5, // GUI up or waiting to transfer first image
            S6 = 6, // ready to start transferring image
            S7 = 7  // transferring image or transfer done
        }
    }
}
