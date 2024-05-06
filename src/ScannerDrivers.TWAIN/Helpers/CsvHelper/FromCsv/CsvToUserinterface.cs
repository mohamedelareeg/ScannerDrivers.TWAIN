using ScannerDrivers.TWAIN.Contants.DataArgumentType;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert a string to a userinterface...
        /// </summary>
        /// <param name="a_twuserinterface">A TWAIN structure</param>
        /// <param name="a_szUserinterface">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public bool CsvToUserinterface(ref TW_USERINTERFACE a_twuserinterface, string a_szUserinterface)
        {
            // Init stuff...
            a_twuserinterface = default(TW_USERINTERFACE);

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szUserinterface);

                // Init stuff...
                a_twuserinterface.ShowUI = 0;
                a_twuserinterface.ModalUI = 0;
                a_twuserinterface.hParent = IntPtr.Zero;

                // Sort out the values...
                ushort.TryParse(CvtCapValueFromEnumHelper<bool>(asz[0]), out a_twuserinterface.ShowUI);
                ushort.TryParse(CvtCapValueFromEnumHelper<bool>(asz[1]), out a_twuserinterface.ModalUI);

                // Really shouldn't have this test, but I'll probably break things if I remove it...
                if (asz.Length >= 3)
                {
                    if (asz[2].ToLowerInvariant() == "hwnd")
                    {
                        a_twuserinterface.hParent = m_intptrHwnd;
                    }
                    else
                    {
                        Int64 i64;
                        if (Int64.TryParse(asz[2], out i64))
                        {
                            a_twuserinterface.hParent = (IntPtr)i64;
                        }
                    }
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
