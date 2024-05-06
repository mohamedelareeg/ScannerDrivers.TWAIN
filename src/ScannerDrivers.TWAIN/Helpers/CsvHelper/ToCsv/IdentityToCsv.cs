using ScannerDrivers.TWAIN.Contants.DataArgumentType;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of an identity to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twidentity">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string IdentityToCsv(TW_IDENTITY a_twidentity)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twidentity.Id.ToString());
                csv.Add(a_twidentity.Version.MajorNum.ToString());
                csv.Add(a_twidentity.Version.MinorNum.ToString());
                csv.Add(a_twidentity.Version.Language.ToString());
                csv.Add(a_twidentity.Version.Country.ToString());
                csv.Add(a_twidentity.Version.Info.Get());
                csv.Add(a_twidentity.ProtocolMajor.ToString());
                csv.Add(a_twidentity.ProtocolMinor.ToString());
                csv.Add("0x" + a_twidentity.SupportedGroups.ToString("X"));
                csv.Add(a_twidentity.Manufacturer.Get());
                csv.Add(a_twidentity.ProductFamily.Get());
                csv.Add(a_twidentity.ProductName.Get());
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
