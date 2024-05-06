using ScannerDrivers.TWAIN.Contants.DataArgumentType;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a userinterface to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twuserinterface">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string UserinterfaceToCsv(TW_USERINTERFACE a_twuserinterface)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(CvtCapValueToEnumHelper<bool>(a_twuserinterface.ShowUI.ToString()));
                csv.Add(CvtCapValueToEnumHelper<bool>(a_twuserinterface.ModalUI.ToString()));
                csv.Add(a_twuserinterface.hParent.ToString());
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
