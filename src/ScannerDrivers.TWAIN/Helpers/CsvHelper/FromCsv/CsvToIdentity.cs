using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a string to an identity structure...
        /// </summary>
        /// <param name="a_twidentity">A TWAIN structure</param>
        /// <param name="a_szIdentity">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToIdentity(ref TW_IDENTITY a_twidentity, string a_szIdentity)
        {
            // Init stuff...
            a_twidentity = default(TW_IDENTITY);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szIdentity);

                // Grab the values...
                a_twidentity.Id = ulong.Parse(asz[0]);
                a_twidentity.Version.MajorNum = ushort.Parse(asz[1]);
                a_twidentity.Version.MinorNum = ushort.Parse(asz[2]);
                if (asz[3] != "0") a_twidentity.Version.Language = (Languages)Enum.Parse(typeof(Languages), asz[3]);
                if (asz[4] != "0") a_twidentity.Version.Country = (Countries)Enum.Parse(typeof(Countries), asz[4]);
                a_twidentity.Version.Info.Set(asz[5]);
                a_twidentity.ProtocolMajor = ushort.Parse(asz[6]);
                a_twidentity.ProtocolMinor = ushort.Parse(asz[7]);
                a_twidentity.SupportedGroups = asz[8].ToLower().StartsWith("0x") ? Convert.ToUInt32(asz[8].Remove(0, 2), 16) : Convert.ToUInt32(asz[8], 16);
                a_twidentity.Manufacturer.Set(asz[9]);
                a_twidentity.ProductFamily.Set(asz[10]);
                a_twidentity.ProductName.Set(asz[11]);
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
