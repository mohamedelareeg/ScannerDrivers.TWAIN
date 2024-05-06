using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{
    /// <summary>
    /// Passthru for TWAIN Direct tasks.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
    public struct TW_TWAINDIRECT
    {
        public uint SizeOf;
        public ushort CommunicationManager;
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public nint Send;
        public uint SendSize;
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public nint Receive;
        public uint ReceiveSize;
    }
}
