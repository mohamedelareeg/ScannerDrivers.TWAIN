using ScannerDrivers.TWAIN.Contants.Structure;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of an entry point to a string that
        /// we can show in our simple GUI...
        /// </summary>
        /// <param name="a_twentrypoint">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string EntrypointToCsv(TW_ENTRYPOINT a_twentrypoint)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twentrypoint.Size.ToString());
                csv.Add("0x" + ((a_twentrypoint.DSM_Entry == null) ? "0" : a_twentrypoint.DSM_Entry.ToString("X")));
                csv.Add("0x" + ((a_twentrypoint.DSM_MemAllocate == null) ? "0" : a_twentrypoint.DSM_MemAllocate.ToString("X")));
                csv.Add("0x" + ((a_twentrypoint.DSM_MemFree == null) ? "0" : a_twentrypoint.DSM_MemFree.ToString("X")));
                csv.Add("0x" + ((a_twentrypoint.DSM_MemLock == null) ? "0" : a_twentrypoint.DSM_MemLock.ToString("X")));
                csv.Add("0x" + ((a_twentrypoint.DSM_MemUnlock == null) ? "0" : a_twentrypoint.DSM_MemUnlock.ToString("X")));
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
