using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Capability.Enum
{
    /// <summary>
    /// ICAP_IMAGEFILTER values
    /// </summary>
    public enum ICAP_IMAGEFILTER : ushort
    {
        NONE = 0,
        AUTO = 1,
        LOWPASS = 2,
        BANDPASS = 3,
        HIGHPASS = 4,
        TEXT = BANDPASS,
        FINELINE = HIGHPASS
    }
}
