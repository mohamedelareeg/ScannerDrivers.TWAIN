using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.ImageAttributes
{

    /// <summary>
    /// TWEI_DESKEWSTATUS values
    /// </summary>
    public enum TWEI_DESKEWSTATUS : ushort
    {
        SUCCESS = 0,
        REPORTONLY = 1,
        FAIL = 2,
        DISABLED = 3
    }
}
