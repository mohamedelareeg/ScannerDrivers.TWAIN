using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{

    /// <summary>
    /// This structure tells the application how many more complete transfers the Source currently has available.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_PENDINGXFERS
    {
        public ushort Count;
        public uint EOJ;
    }
}
