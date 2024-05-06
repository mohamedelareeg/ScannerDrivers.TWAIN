using ScannerDrivers.TWAIN.Contants.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{
    /// <summary>
    /// This structure is used by the application to specify a set of mapping values to be applied to grayscale data.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_GRAYRESPONSE
    {
        public TW_ELEMENT8 Response_00;
    }
}
