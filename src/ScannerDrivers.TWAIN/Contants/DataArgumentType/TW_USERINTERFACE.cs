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
    /// This structure is used to handle the user interface coordination between an application and a Source.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
    public struct TW_USERINTERFACE
    {
        public ushort ShowUI;
        public ushort ModalUI;
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public nint hParent;
    }

}
