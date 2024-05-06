using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{
    /// <summary>
    /// Provides the application information about the Source’s requirements and preferences regarding allocation of transfer buffer(s).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_SETUPMEMXFER
    {
        public uint MinBufSize;
        public uint MaxBufSize;
        public uint Preferred;
    }

}
