using ScannerDrivers.TWAIN.Contants.Structure;
using System.Globalization;
using static ScannerDrivers.TWAIN.Contants.Enum.ExtendedImage;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a string to an extimageinfo structure,
        /// note that we don't have to worry about containers going in this
        /// direction...
        /// </summary>
        /// <param name="a_twextimageinfo">A TWAIN structure</param>
        /// <param name="a_szExtimageinfo">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public static bool CsvToExtimageinfo(ref TW_EXTIMAGEINFO a_twextimageinfo, string a_szExtimageinfo)
        {
            // Init stuff...
            a_twextimageinfo = default(TW_EXTIMAGEINFO);

            // Build the string...
            try
            {
                int iField;
                uint uTwinfo;
                string[] asz = CSV.Parse(a_szExtimageinfo);

                // Set the number of entries (this is the easy bit)...
                uint.TryParse(asz[0], out a_twextimageinfo.NumInfos);
                if (a_twextimageinfo.NumInfos > 200)
                {
                    Log.Error("***error*** - we're limited to 200 entries, if this is a problem, just add more, and fix this code...");
                    return (false);
                }

                // Okay, walk all the entries in steps of TW_INFO...
                uTwinfo = 0;
                for (iField = 1; iField < asz.Length; iField += 5)
                {
                    UInt64 u64;
                    TWEI twei;
                    TW_INFO twinfo = default(TW_INFO);
                    if ((iField + 5) > asz.Length)
                    {
                        Log.Error("***error*** - badly constructed list, should be: num,(twinfo),(twinfo)...");
                        return (false);
                    }
                    if (TWEI.TryParse(asz[iField + 0].Replace("TWEI_", ""), out twei))
                    {
                        twinfo.InfoId = (ushort)twei;
                    }
                    else
                    {
                        if (asz[iField + 0].ToLowerInvariant().StartsWith("0x"))
                        {
                            ushort.TryParse(asz[iField + 0].Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out twinfo.InfoId);
                        }
                        else
                        {
                            ushort.TryParse(asz[iField + 0], out twinfo.InfoId);
                        }
                    }
                    // We really don't care about these...
                    ushort.TryParse(asz[iField + 1], out twinfo.ItemType);
                    ushort.TryParse(asz[iField + 2], out twinfo.NumItems);
                    ushort.TryParse(asz[iField + 3], out twinfo.ReturnCode);
                    UInt64.TryParse(asz[iField + 4], out u64);
                    twinfo.Item = (UIntPtr)u64;
                    a_twextimageinfo.Set(uTwinfo, ref twinfo);
                    uTwinfo += 1;
                }
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
