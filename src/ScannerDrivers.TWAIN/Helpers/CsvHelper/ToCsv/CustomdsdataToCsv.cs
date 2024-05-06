using ScannerDrivers.TWAIN.Contants.Structure;
using System.Security.Permissions;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a custom DS data to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twcustomdsdata">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust", Unrestricted = false)]
        public string CustomdsdataToCsv(TW_CUSTOMDSDATA a_twcustomdsdata)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twcustomdsdata.InfoLength.ToString());
                IntPtr intptr = DsmMemLock(a_twcustomdsdata.hData);
                csv.Add(intptr.ToString());
                DsmMemUnlock(a_twcustomdsdata.hData);
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
