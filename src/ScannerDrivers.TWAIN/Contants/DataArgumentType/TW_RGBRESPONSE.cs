using ScannerDrivers.TWAIN.Contants.Structure;
using System.Runtime.InteropServices;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{
    /// <summary>
    /// This structure is used by the application to specify a set of mapping values to be applied to RGB color data.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_RGBRESPONSE
    {
        public TW_ELEMENT8 Response_00;
    }


}
