using ScannerDrivers.TWAIN.Contants.DataArgumentType;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a filesystem to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twfilesystem">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public static string FilesystemToCsv(TW_FILESYSTEM a_twfilesystem)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twfilesystem.InputName.Get());
                csv.Add(a_twfilesystem.OutputName.Get());
                csv.Add(a_twfilesystem.Context.ToString());
                csv.Add(a_twfilesystem.Recursive.ToString());
                csv.Add(a_twfilesystem.FileType.ToString());
                csv.Add(a_twfilesystem.Size.ToString());
                csv.Add(a_twfilesystem.CreateTimeDate.Get());
                csv.Add(a_twfilesystem.ModifiedTimeDate.Get());
                csv.Add(a_twfilesystem.FreeSpace.ToString());
                csv.Add(a_twfilesystem.NewImageSize.ToString());
                csv.Add(a_twfilesystem.NumberOfFiles.ToString());
                csv.Add(a_twfilesystem.NumberOfSnippets.ToString());
                csv.Add(a_twfilesystem.DeviceGroupMask.ToString());
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
