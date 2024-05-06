using ScannerDrivers.TWAIN.Contants.Enum;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// This mess is what tries to turn numeric constants into something
        /// a bit more readable...
        /// </summary>
        /// <typeparam name="T">type for the conversion</typeparam>
        /// <param name="a_szValue">value to convert</param>
        /// <returns></returns>
        private static string CvtCapValueToEnumHelper<T>(string a_szValue)
        {
            T t;
            Int32 i32 = 0;
            UInt32 u32 = 0;
            string szCvt = "";

            // Adjust our value, as needed...
            if (a_szValue.StartsWith(typeof(T).Name + "_"))
            {
                a_szValue = a_szValue.Substring((typeof(T).Name + "_").Length);
            }

            // Handle enums with negative numbers...
            if (a_szValue.StartsWith("-"))
            {
                if (Int32.TryParse(a_szValue, out i32))
                {
                    t = (T)Enum.Parse(typeof(T), a_szValue, true);
                    szCvt = t.ToString();
                    if (szCvt != i32.ToString())
                    {
                        return (typeof(T).ToString().Replace("NIScan.TWAIN.TWAIN+", "") + "_" + szCvt);
                    }
                }
            }

            // Everybody else...
            else if (UInt32.TryParse(a_szValue, out u32))
            {
                // Handle bool...
                if (typeof(T) == typeof(bool))
                {
                    if ((a_szValue == "1") || (a_szValue.ToLowerInvariant() == "true"))
                    {
                        return ("TRUE");
                    }
                    return ("FALSE");
                }

                // Handle DAT (which is a weird one)..
                else if (typeof(T) == typeof(DataArgumentTypes))
                {
                    UInt32 u32Dg = u32 >> 16;
                    UInt32 u32Dat = u32 & 0xFFFF;
                    string szDg = ((DataGroups)u32Dg).ToString();
                    string szDat = ((DataArgumentTypes)u32Dat).ToString();
                    szDg = (szDg != u32Dg.ToString()) ? ("DG_" + szDg) : string.Format("0x{0:X}", u32Dg);
                    szDat = (szDat != u32Dat.ToString()) ? ("DAT_" + szDat) : string.Format("0x{0:X}", u32Dat);
                    return (szDg + "|" + szDat);
                }

                // Everybody else is on their own...
                else
                {
                    // mono hurls on this, .net doesn't, so gonna help...
                    switch (a_szValue)
                    {
                        default: break;
                        case "65535": a_szValue = "-1"; break;
                        case "65534": a_szValue = "-2"; break;
                        case "65533": a_szValue = "-3"; break;
                        case "65532": a_szValue = "-4"; break;
                    }
                    t = (T)Enum.Parse(typeof(T), a_szValue, true);
                }

                // Check to see if we changed anything...
                szCvt = t.ToString();
                if (szCvt != u32.ToString())
                {
                    // CAP is in its final form...
                    if (typeof(T) == typeof(Capabilities))
                    {
                        return (szCvt);
                    }
                    // Everybody else needs the name decoration removed...
                    else
                    {
                        return (typeof(T).ToString().Replace("NIScan.TWAIN.TWAIN+", "") + "_" + szCvt);
                    }
                }

                // We're probably a custom value...
                else
                {
                    return (string.Format("0x{0:X}", u32));
                }
            }

            // We're a string...
            return (a_szValue);
        }
    }
}
