using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{
    /// <summary>
    /// Describes the status of a source.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_STATUS
    {
        public ushort ConditionCode;
        public ushort Data;
    }
}
