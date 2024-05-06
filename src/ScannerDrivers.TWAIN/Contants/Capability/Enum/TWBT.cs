using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{
    /// <summary>
    /// ICAP_SUPPORTEDBARCODETYPES and TWEI_BARCODETYPE values
    /// </summary>
    public enum TWBT : ushort
    {
        X3OF9 = 0, // 3OF9 in TWAIN.H
        X2OF5INTERLEAVED = 1, // 2OF5INTERLEAVED in TWAIN.H
        X2OF5NONINTERLEAVED = 2, // 2OF5NONINTERLEAVED in TWAIN.H
        CODE93 = 3,
        CODE128 = 4,
        UCC128 = 5,
        CODABAR = 6,
        UPCA = 7,
        UPCE = 8,
        EAN8 = 9,
        EAN13 = 10,
        POSTNET = 11,
        PDF417 = 12,
        X2OF5INDUSTRIAL = 13, // 2OF5INDUSTRIAL in TWAIN.H
        X2OF5MATRIX = 14, // 2OF5MATRIX in TWAIN.H
        X2OF5DATALOGIC = 15, // 2OF5DATALOGIC in TWAIN.H
        X2OF5IATA = 16, // 2OF5IATA in TWAIN.H
        X3OF9FULLASCII = 17, // 3OF9FULLASCII in TWAIN.H
        CODABARWITHSTARTSTOP = 18,
        MAXICODE = 19,
        QRCODE = 20
    }
}
