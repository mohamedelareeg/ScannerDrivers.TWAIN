using ScannerDrivers.TWAIN.Contants.Enum;
using ScannerDrivers.TWAIN.Contants.Generic;
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
    /// Used by application to get/set capability from/in a data source.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
    public struct TW_CAPABILITY
    {
        public Capabilities Cap;
        public TWON ConType;
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public nint hContainer;
    }
}
