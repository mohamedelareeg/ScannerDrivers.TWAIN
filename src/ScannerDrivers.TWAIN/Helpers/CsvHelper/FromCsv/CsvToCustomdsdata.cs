using ScannerDrivers.TWAIN.Contants.Structure;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a string to a custom DS data structure...
        /// </summary>
        /// <param name="a_twcustomdsdata">A TWAIN structure</param>
        /// <param name="a_szCustomdsdata">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust", Unrestricted = false)]
        public bool CsvToCustomdsdata(ref TW_CUSTOMDSDATA a_twcustomdsdata, string a_szCustomdsdata)
        {
            // Init stuff...
            a_twcustomdsdata = default(TW_CUSTOMDSDATA);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szCustomdsdata);

                // Grab the values...
                a_twcustomdsdata.InfoLength = uint.Parse(asz[0]);
                a_twcustomdsdata.hData = DsmMemAlloc(a_twcustomdsdata.InfoLength);
                IntPtr intptr = DsmMemLock(a_twcustomdsdata.hData);
                byte[] bProfile = new byte[a_twcustomdsdata.InfoLength];
                Marshal.Copy((IntPtr)UInt64.Parse(asz[1]), bProfile, 0, (int)a_twcustomdsdata.InfoLength);
                Marshal.Copy(bProfile, 0, intptr, (int)a_twcustomdsdata.InfoLength);
                DsmMemUnlock(a_twcustomdsdata.hData);
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return (false);
            }

            // All done...
            return (true);
        }
    }
}
