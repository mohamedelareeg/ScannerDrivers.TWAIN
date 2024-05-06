using ScannerDrivers.TWAIN.Contants.Type;
using System.Runtime.InteropServices;

namespace ScannerDrivers.TWAIN.Contants.Structure
{
    /// <summary>
    /// Provides information about the Event that was raised by the Source.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TW_DEVICEEVENT
    {
        public uint Event;
        public TW_STR255 DeviceName;
        public uint BatteryMinutes;
        public short BatteryPercentage;
        public int PowerSupply;
        public TW_FIX32 XResolution;
        public TW_FIX32 YResolution;
        public uint FlashUsed2;
        public uint AutomaticCapture;
        public uint TimeBeforeFirstCapture;
        public uint TimeBetweenCaptures;
    }
}
