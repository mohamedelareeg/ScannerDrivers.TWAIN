using ScannerDrivers.TWAIN.Contants.Capability.Enum;
using ScannerDrivers.TWAIN.Contants.Structure;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a device event to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twdeviceevent">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string DeviceeventToCsv(TW_DEVICEEVENT a_twdeviceevent)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(((CAP_DEVICEEVENT)a_twdeviceevent.Event).ToString());
                csv.Add(a_twdeviceevent.DeviceName.Get());
                csv.Add(a_twdeviceevent.BatteryMinutes.ToString());
                csv.Add(a_twdeviceevent.BatteryPercentage.ToString());
                csv.Add(a_twdeviceevent.PowerSupply.ToString());
                csv.Add(((double)a_twdeviceevent.XResolution.Whole + ((double)a_twdeviceevent.XResolution.Frac / 65536.0)).ToString());
                csv.Add(((double)a_twdeviceevent.YResolution.Whole + ((double)a_twdeviceevent.YResolution.Frac / 65536.0)).ToString());
                csv.Add(a_twdeviceevent.FlashUsed2.ToString());
                csv.Add(a_twdeviceevent.AutomaticCapture.ToString());
                csv.Add(a_twdeviceevent.TimeBeforeFirstCapture.ToString());
                csv.Add(a_twdeviceevent.TimeBetweenCaptures.ToString());
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }
    }
}
