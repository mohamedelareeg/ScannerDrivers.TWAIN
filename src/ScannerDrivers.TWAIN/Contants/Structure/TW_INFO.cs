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
    /// This structure is used to pass specific information between the data source and the application.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_INFO
    {
        public ushort InfoId;
        public ushort ItemType;
        public ushort NumItems;
        public ushort ReturnCode;
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public nuint Item;
    }

}
