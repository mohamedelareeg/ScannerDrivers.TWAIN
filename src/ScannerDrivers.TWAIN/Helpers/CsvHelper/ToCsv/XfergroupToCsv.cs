namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {

        /// <summary>
        /// Convert the contents of a transfer group to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_u32Xfergroup">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string XfergroupToCsv(UInt32 a_u32Xfergroup)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add("0x" + a_u32Xfergroup.ToString("X"));
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
