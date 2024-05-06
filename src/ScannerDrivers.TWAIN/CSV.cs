namespace ScannerDrivers.TWAIN
{
    /// <summary>
    /// A quick and dirty CSV reader/writer...
    /// </summary>
    public class CSV
    {
        ///////////////////////////////////////////////////////////////////////////////
        // Public Functions...
        ///////////////////////////////////////////////////////////////////////////////
        #region Public Functions...

        /// <summary>
        /// Start with an empty string...
        /// </summary>
        public CSV()
        {
            m_szCsv = "";
        }

        /// <summary>
        /// Add an item to a CSV string...
        /// </summary>
        /// <param name="a_szItem">Something to add to the CSV string</param>
        public void Add(string a_szItem)
        {
            // If the item has commas, we need to do work...
            if (a_szItem.Contains(","))
            {
                // If the item has quotes, replace them with paired quotes, then
                // quote it and add it...
                if (a_szItem.Contains("\""))
                {
                    m_szCsv += (m_szCsv != "" ? "," : "") + "\"" + a_szItem.Replace("\"", "\"\"") + "\"";
                }

                // Otherwise, just quote it and add it...
                else
                {
                    m_szCsv += (m_szCsv != "" ? "," : "") + "\"" + a_szItem + "\"";
                }
            }

            // If the item has quotes, replace them with escaped quotes, then
            // quote it and add it...
            else if (a_szItem.Contains("\""))
            {
                m_szCsv += (m_szCsv != "" ? "," : "") + "\"" + a_szItem.Replace("\"", "\"\"") + "\"";
            }

            // Otherwise, just add it...
            else
            {
                m_szCsv += (m_szCsv != "" ? "," : "") + a_szItem;
            }
        }

        /// <summary>
        /// Clear the record...
        /// </summary>
        public void Clear()
        {
            m_szCsv = "";
        }

        /// <summary>
        /// Get the current CSV string...
        /// </summary>
        /// <returns>The current value of the CSV string</returns>
        public string Get()
        {
            return m_szCsv;
        }

        /// <summary>
        /// Parse a CSV string...
        /// </summary>
        /// <param name="a_szCsv">A CSV string to parse</param>
        /// <returns>An array if items (some can be CSV themselves)</returns>
        public static string[] Parse(string a_szCsv)
        {
            int ii;
            bool blEnd;
            string[] aszCsv;
            string[] aszLeft;
            string[] aszRight;

            // Validate...
            if (a_szCsv == null || a_szCsv == "")
            {
                return new string[] { "" };
            }

            // If there are no quotes, then parse it fast...
            if (!a_szCsv.Contains("\""))
            {
                return a_szCsv.Split(new char[] { ',' });
            }

            // There's no opening quote, so split and recurse...
            if (a_szCsv[0] != '"')
            {
                aszLeft = new string[] { a_szCsv.Substring(0, a_szCsv.IndexOf(',')) };
                aszRight = Parse(a_szCsv.Remove(0, a_szCsv.IndexOf(',') + 1));
                aszCsv = new string[aszLeft.Length + aszRight.Length];
                aszLeft.CopyTo(aszCsv, 0);
                aszRight.CopyTo(aszCsv, aszLeft.Length);
                return aszCsv;
            }

            // Handle the quoted string...
            else
            {
                // Find the terminating quote...
                blEnd = true;
                for (ii = 0; ii < a_szCsv.Length; ii++)
                {
                    if (a_szCsv[ii] == '"')
                    {
                        blEnd = !blEnd;
                    }
                    else if (blEnd && a_szCsv[ii] == ',')
                    {
                        break;
                    }
                }
                ii -= 1;

                // We have a problem...
                if (!blEnd)
                {
                    throw new Exception("Error in CSV string...");
                }

                // This is the last item, remove any escaped quotes and return it...
                if (ii + 1 >= a_szCsv.Length)
                {
                    return new string[] { a_szCsv.Substring(1, a_szCsv.Length - 2).Replace("\"\"", "\"") };
                }

                // We have more data...
                if (a_szCsv[ii + 1] == ',')
                {
                    aszLeft = new string[] { a_szCsv.Substring(1, ii - 1).Replace("\"\"", "\"") };
                    aszRight = Parse(a_szCsv.Remove(0, ii + 2));
                    aszCsv = new string[aszLeft.Length + aszRight.Length];
                    aszLeft.CopyTo(aszCsv, 0);
                    aszRight.CopyTo(aszCsv, aszLeft.Length);
                    return aszCsv;
                }

                // We have a problem...
                throw new Exception("Error in CSV string...");
            }
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Private Attributes...
        ///////////////////////////////////////////////////////////////////////////////
        #region Private Attributes...

        /// <summary>
        /// Our working string for creating or parsing...
        /// </summary>
        private string m_szCsv;

        #endregion
    }
}
