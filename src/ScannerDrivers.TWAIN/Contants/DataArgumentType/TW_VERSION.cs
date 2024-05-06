using ScannerDrivers.TWAIN.Contants.Enum;
using ScannerDrivers.TWAIN.Contants.Type;
using System.Runtime.InteropServices;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType
{
    /// <summary>
    /// A general way to describe the version of software that is running.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
    public struct TW_VERSION
    {
        public ushort MajorNum;
        public ushort MinorNum;
        public Languages Language;
        public Countries Country;
        public TW_STR32 Info;
    }

}
