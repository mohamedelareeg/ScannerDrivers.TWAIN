namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// This mess is what tries to turn readable stuff into numeric constants...
        /// </summary>
        /// <typeparam name="T">type for the conversion</typeparam>
        /// <param name="a_szValue">value to convert</param>
        /// <returns></returns>
        private static string CvtCapValueFromEnumHelper<T>(string a_szValue)
        {
            // We can figure this one out on our own...
            if ((typeof(T).Name == "bool") || (typeof(T).Name == "Boolean"))
            {
                return (((a_szValue.ToLowerInvariant() == "true") || (a_szValue == "1")) ? "1" : "0");
            }

            // Look for the enum prefix...
            if (a_szValue.ToLowerInvariant().StartsWith(typeof(T).Name.ToLowerInvariant() + "_"))
            {
                return (a_szValue.Substring((typeof(T).Name + "_").Length));
            }

            // Er...
            return (a_szValue);
        }
    }
}
