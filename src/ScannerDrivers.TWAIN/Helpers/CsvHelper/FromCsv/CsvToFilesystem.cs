using ScannerDrivers.TWAIN.Contants.DataArgumentType;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a string to a filesystem structure...
        /// </summary>
        /// <param name="a_twfilesystem">A TWAIN structure</param>
        /// <param name="a_szFilesystem">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToFilesystem(ref TW_FILESYSTEM a_twfilesystem, string a_szFilesystem)
        {
            // Init stuff...
            a_twfilesystem = default(TW_FILESYSTEM);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szFilesystem);

                // Grab the values...
                a_twfilesystem.InputName.Set(asz[0]);
                a_twfilesystem.OutputName.Set(asz[1]);
                a_twfilesystem.Context = (IntPtr)UInt64.Parse(asz[2]);
                a_twfilesystem.Recursive = int.Parse(asz[3]);
                a_twfilesystem.FileType = int.Parse(asz[4]);
                a_twfilesystem.Size = uint.Parse(asz[5]);
                a_twfilesystem.CreateTimeDate.Set(asz[6]);
                a_twfilesystem.ModifiedTimeDate.Set(asz[7]);
                a_twfilesystem.FreeSpace = (uint)UInt64.Parse(asz[8]);
                a_twfilesystem.NewImageSize = (uint)UInt64.Parse(asz[9]);
                a_twfilesystem.NumberOfFiles = (uint)UInt64.Parse(asz[10]);
                a_twfilesystem.NumberOfSnippets = (uint)UInt64.Parse(asz[11]);
                a_twfilesystem.DeviceGroupMask = (uint)UInt64.Parse(asz[12]);
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
