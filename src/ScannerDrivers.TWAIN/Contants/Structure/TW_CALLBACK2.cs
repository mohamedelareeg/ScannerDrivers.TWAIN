using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.Structure
{

    /// <summary>
    /// Used to register callbacks.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_CALLBACK2
    {
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public nint CallBackProc;
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public nuint RefCon;
        public ushort Message;
    }
}
