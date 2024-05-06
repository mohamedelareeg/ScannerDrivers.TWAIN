using ScannerDrivers.TWAIN.Contants.Enum;
using System.Runtime.InteropServices;
using System.Text;

namespace ScannerDrivers.TWAIN.Contants.Type
{
    // Follow these rules
    /******************************************************************************

    TW_HANDLE...............IntPtr
    TW_MEMREF...............IntPtr
    TW_UINTPTR..............UIntPtr

    TW_INT8.................char
    TW_INT16................short
    TW_INT32................int (was long on Linux 64-bit)

    TW_UINT8................byte
    TW_UINT16...............ushort
    TW_UINT32...............uint (was ulong on Linux 64-bit)
    TW_BOOL.................ushort

    ******************************************************************************/
    /// <summary>
    /// Used for strings that go up to 32-bytes...
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct TW_STR32
    {
        /// <summary>
        /// We're stuck with this, because marshalling with packed alignment
        /// can't handle arrays...
        /// </summary>
        private byte byItem000; private byte byItem001; private byte byItem002; private byte byItem003;
        private byte byItem004; private byte byItem005; private byte byItem006; private byte byItem007;
        private byte byItem008; private byte byItem009; private byte byItem010; private byte byItem011;
        private byte byItem012; private byte byItem013; private byte byItem014; private byte byItem015;
        private byte byItem016; private byte byItem017; private byte byItem018; private byte byItem019;
        private byte byItem020; private byte byItem021; private byte byItem022; private byte byItem023;
        private byte byItem024; private byte byItem025; private byte byItem026; private byte byItem027;
        private byte byItem028; private byte byItem029; private byte byItem030; private byte byItem031;
        private byte byItem032; private byte byItem033;

        /// <summary>
        /// The normal get...
        /// </summary>
        /// <returns></returns>
        public string Get()
        {
            return GetValue(true);
        }

        /// <summary>
        /// Use this on Mac OS X if you have a call that uses a string
        /// that doesn't include the prefix byte...
        /// </summary>
        /// <returns></returns>
        public string GetNoPrefix()
        {
            return GetValue(false);
        }

        /// <summary>
        /// Get our value...
        /// </summary>
        /// <returns></returns>
        private string GetValue(bool a_blMayHavePrefix)
        {
            // convert what we have into a byte array
            byte[] abyItem = new byte[34];
            abyItem[0] = byItem000; abyItem[1] = byItem001; abyItem[2] = byItem002; abyItem[3] = byItem003;
            abyItem[4] = byItem004; abyItem[5] = byItem005; abyItem[6] = byItem006; abyItem[7] = byItem007;
            abyItem[8] = byItem008; abyItem[9] = byItem009; abyItem[10] = byItem010; abyItem[11] = byItem011;
            abyItem[12] = byItem012; abyItem[13] = byItem013; abyItem[14] = byItem014; abyItem[15] = byItem015;
            abyItem[16] = byItem016; abyItem[17] = byItem017; abyItem[18] = byItem018; abyItem[19] = byItem019;
            abyItem[20] = byItem020; abyItem[21] = byItem021; abyItem[22] = byItem022; abyItem[23] = byItem023;
            abyItem[24] = byItem024; abyItem[25] = byItem025; abyItem[26] = byItem026; abyItem[27] = byItem027;
            abyItem[28] = byItem028; abyItem[29] = byItem029; abyItem[30] = byItem030; abyItem[31] = byItem031;
            abyItem[32] = byItem032; abyItem[33] = byItem033;

            // Zero anything after the NUL...
            bool blNul = false;
            for (int ii = 0; ii < abyItem.Length; ii++)
            {
                if (!blNul && abyItem[ii] == 0)
                {
                    blNul = true;
                }
                else if (blNul)
                {
                    abyItem[ii] = 0;
                }
            }

            // change encoding of byte array, then convert the bytes array to a string
            string sz = Encoding.Unicode.GetString(Encoding.Convert(Language.GetEncoding(), Encoding.Unicode, abyItem));

            // If the first character is a NUL, then return the empty string...
            while (sz.Length > 0 && sz[0] == '\0')
            {
                sz = sz.Remove(0, 1);
            }

            // We have an emptry string...
            if (sz.Length == 0)
            {
                return "";
            }

            // If we're running on a Mac, take off the prefix 'byte'...
            if (a_blMayHavePrefix && TWAINSDK.GetPlatform() == Platform.MACOSX)
            {
                sz = sz.Remove(0, 1);
            }

            // If we detect a NUL, then split around it...
            if (sz.IndexOf('\0') >= 0)
            {
                sz = sz.Split(new char[] { '\0' })[0];
            }

            // All done...
            return sz;
        }

        /// <summary>
        /// The normal set...
        /// </summary>
        /// <returns></returns>
        public void Set(string a_sz)
        {
            SetValue(a_sz, true);
        }

        /// <summary>
        /// Use this on Mac OS X if you have a call that uses a string
        /// that doesn't include the prefix byte...
        /// </summary>
        /// <returns></returns>
        public void SetNoPrefix(string a_sz)
        {
            SetValue(a_sz, false);
        }

        /// <summary>
        /// Set our value...
        /// </summary>
        /// <param name="a_sz"></param>
        private void SetValue(string a_sz, bool a_blMayHavePrefix)
        {
            // If we're running on a Mac, tack on the prefix 'byte'...
            if (a_sz == null)
            {
                a_sz = "";
            }
            else if (a_blMayHavePrefix && TWAINSDK.GetPlatform() == Platform.MACOSX)
            {
                a_sz = (char)a_sz.Length + a_sz;
            }

            // Make sure that we're NUL padded...
            string sz = a_sz +
                "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" +
                "\0\0";
            if (sz.Length > 34)
            {
                sz = sz.Remove(34);
            }

            // convert string to byte array, then change the encoding of the byte array
            byte[] abyItem = Encoding.Convert(Encoding.Unicode, Language.GetEncoding(), Encoding.Unicode.GetBytes(sz));

            // convert byte array to bytes
            if (abyItem.Length > 0)
            {
                byItem000 = abyItem[0]; byItem001 = abyItem[1]; byItem002 = abyItem[2]; byItem003 = abyItem[3];
                byItem004 = abyItem[4]; byItem005 = abyItem[5]; byItem006 = abyItem[6]; byItem007 = abyItem[7];
                byItem008 = abyItem[8]; byItem009 = abyItem[9]; byItem010 = abyItem[10]; byItem011 = abyItem[11];
                byItem012 = abyItem[12]; byItem013 = abyItem[13]; byItem014 = abyItem[14]; byItem015 = abyItem[15];
                byItem016 = abyItem[16]; byItem017 = abyItem[17]; byItem018 = abyItem[18]; byItem019 = abyItem[19];
                byItem020 = abyItem[20]; byItem021 = abyItem[21]; byItem022 = abyItem[22]; byItem023 = abyItem[23];
                byItem024 = abyItem[24]; byItem025 = abyItem[25]; byItem026 = abyItem[26]; byItem027 = abyItem[27];
                byItem028 = abyItem[28]; byItem029 = abyItem[29]; byItem030 = abyItem[30]; byItem031 = abyItem[31];
                byItem032 = abyItem[32]; byItem033 = abyItem[33];
            }
        }
    }
}
