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
    /// Used for strings that go up to 128-bytes...
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct TW_STR128
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
        private byte byItem032; private byte byItem033; private byte byItem034; private byte byItem035;
        private byte byItem036; private byte byItem037; private byte byItem038; private byte byItem039;
        private byte byItem040; private byte byItem041; private byte byItem042; private byte byItem043;
        private byte byItem044; private byte byItem045; private byte byItem046; private byte byItem047;
        private byte byItem048; private byte byItem049; private byte byItem050; private byte byItem051;
        private byte byItem052; private byte byItem053; private byte byItem054; private byte byItem055;
        private byte byItem056; private byte byItem057; private byte byItem058; private byte byItem059;
        private byte byItem060; private byte byItem061; private byte byItem062; private byte byItem063;
        private byte byItem064; private byte byItem065; private byte byItem066; private byte byItem067;
        private byte byItem068; private byte byItem069; private byte byItem070; private byte byItem071;
        private byte byItem072; private byte byItem073; private byte byItem074; private byte byItem075;
        private byte byItem076; private byte byItem077; private byte byItem078; private byte byItem079;
        private byte byItem080; private byte byItem081; private byte byItem082; private byte byItem083;
        private byte byItem084; private byte byItem085; private byte byItem086; private byte byItem087;
        private byte byItem088; private byte byItem089; private byte byItem090; private byte byItem091;
        private byte byItem092; private byte byItem093; private byte byItem094; private byte byItem095;
        private byte byItem096; private byte byItem097; private byte byItem098; private byte byItem099;
        private byte byItem100; private byte byItem101; private byte byItem102; private byte byItem103;
        private byte byItem104; private byte byItem105; private byte byItem106; private byte byItem107;
        private byte byItem108; private byte byItem109; private byte byItem110; private byte byItem111;
        private byte byItem112; private byte byItem113; private byte byItem114; private byte byItem115;
        private byte byItem116; private byte byItem117; private byte byItem118; private byte byItem119;
        private byte byItem120; private byte byItem121; private byte byItem122; private byte byItem123;
        private byte byItem124; private byte byItem125; private byte byItem126; private byte byItem127;
        private byte byItem128; private byte byItem129;

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
            byte[] abyItem = new byte[130];
            abyItem[0] = byItem000; abyItem[1] = byItem001; abyItem[2] = byItem002; abyItem[3] = byItem003;
            abyItem[4] = byItem004; abyItem[5] = byItem005; abyItem[6] = byItem006; abyItem[7] = byItem007;
            abyItem[8] = byItem008; abyItem[9] = byItem009; abyItem[10] = byItem010; abyItem[11] = byItem011;
            abyItem[12] = byItem012; abyItem[13] = byItem013; abyItem[14] = byItem014; abyItem[15] = byItem015;
            abyItem[16] = byItem016; abyItem[17] = byItem017; abyItem[18] = byItem018; abyItem[19] = byItem019;
            abyItem[20] = byItem020; abyItem[21] = byItem021; abyItem[22] = byItem022; abyItem[23] = byItem023;
            abyItem[24] = byItem024; abyItem[25] = byItem025; abyItem[26] = byItem026; abyItem[27] = byItem027;
            abyItem[28] = byItem028; abyItem[29] = byItem029; abyItem[30] = byItem030; abyItem[31] = byItem031;
            abyItem[32] = byItem032; abyItem[33] = byItem033; abyItem[34] = byItem034; abyItem[35] = byItem035;
            abyItem[36] = byItem036; abyItem[37] = byItem037; abyItem[38] = byItem038; abyItem[39] = byItem039;
            abyItem[40] = byItem040; abyItem[41] = byItem041; abyItem[42] = byItem042; abyItem[43] = byItem043;
            abyItem[44] = byItem044; abyItem[45] = byItem045; abyItem[46] = byItem046; abyItem[47] = byItem047;
            abyItem[48] = byItem048; abyItem[49] = byItem049; abyItem[50] = byItem050; abyItem[51] = byItem051;
            abyItem[52] = byItem052; abyItem[53] = byItem053; abyItem[54] = byItem054; abyItem[55] = byItem055;
            abyItem[56] = byItem056; abyItem[57] = byItem057; abyItem[58] = byItem058; abyItem[59] = byItem059;
            abyItem[60] = byItem060; abyItem[61] = byItem061; abyItem[62] = byItem062; abyItem[63] = byItem063;
            abyItem[64] = byItem064; abyItem[65] = byItem065; abyItem[66] = byItem066; abyItem[67] = byItem067;
            abyItem[68] = byItem068; abyItem[69] = byItem069; abyItem[70] = byItem070; abyItem[71] = byItem071;
            abyItem[72] = byItem072; abyItem[73] = byItem073; abyItem[74] = byItem074; abyItem[75] = byItem075;
            abyItem[76] = byItem076; abyItem[77] = byItem077; abyItem[78] = byItem078; abyItem[79] = byItem079;
            abyItem[80] = byItem080; abyItem[81] = byItem081; abyItem[82] = byItem082; abyItem[83] = byItem083;
            abyItem[84] = byItem084; abyItem[85] = byItem085; abyItem[86] = byItem086; abyItem[87] = byItem087;
            abyItem[88] = byItem088; abyItem[89] = byItem089; abyItem[90] = byItem090; abyItem[91] = byItem091;
            abyItem[92] = byItem092; abyItem[93] = byItem093; abyItem[94] = byItem094; abyItem[95] = byItem095;
            abyItem[96] = byItem096; abyItem[97] = byItem097; abyItem[98] = byItem098; abyItem[99] = byItem099;
            abyItem[100] = byItem100; abyItem[101] = byItem101; abyItem[102] = byItem102; abyItem[103] = byItem103;
            abyItem[104] = byItem104; abyItem[105] = byItem105; abyItem[106] = byItem106; abyItem[107] = byItem107;
            abyItem[108] = byItem108; abyItem[109] = byItem109; abyItem[110] = byItem110; abyItem[111] = byItem111;
            abyItem[112] = byItem112; abyItem[113] = byItem113; abyItem[114] = byItem114; abyItem[115] = byItem115;
            abyItem[116] = byItem116; abyItem[117] = byItem117; abyItem[118] = byItem118; abyItem[119] = byItem119;
            abyItem[120] = byItem120; abyItem[121] = byItem121; abyItem[122] = byItem122; abyItem[123] = byItem123;
            abyItem[124] = byItem124; abyItem[125] = byItem125; abyItem[126] = byItem126; abyItem[127] = byItem127;
            abyItem[128] = byItem128; abyItem[129] = byItem129;

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
            if (sz[0] == '\0')
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
            string sz =
                a_sz +
                "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" +
                "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" +
                "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" +
                "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" +
                "\0\0";
            if (sz.Length > 130)
            {
                sz = sz.Remove(130);
            }

            // convert string to byte array, then change the encoding of the byte array
            byte[] abyItem = Encoding.Convert(Encoding.Unicode, Language.GetEncoding(), Encoding.Unicode.GetBytes(sz));

            // concert byte array to bytes
            byItem000 = abyItem[0]; byItem001 = abyItem[1]; byItem002 = abyItem[2]; byItem003 = abyItem[3];
            byItem004 = abyItem[4]; byItem005 = abyItem[5]; byItem006 = abyItem[6]; byItem007 = abyItem[7];
            byItem008 = abyItem[8]; byItem009 = abyItem[9]; byItem010 = abyItem[10]; byItem011 = abyItem[11];
            byItem012 = abyItem[12]; byItem013 = abyItem[13]; byItem014 = abyItem[14]; byItem015 = abyItem[15];
            byItem016 = abyItem[16]; byItem017 = abyItem[17]; byItem018 = abyItem[18]; byItem019 = abyItem[19];
            byItem020 = abyItem[20]; byItem021 = abyItem[21]; byItem022 = abyItem[22]; byItem023 = abyItem[23];
            byItem024 = abyItem[24]; byItem025 = abyItem[25]; byItem026 = abyItem[26]; byItem027 = abyItem[27];
            byItem028 = abyItem[28]; byItem029 = abyItem[29]; byItem030 = abyItem[30]; byItem031 = abyItem[31];
            byItem032 = abyItem[32]; byItem033 = abyItem[33]; byItem034 = abyItem[34]; byItem035 = abyItem[35];
            byItem036 = abyItem[36]; byItem037 = abyItem[37]; byItem038 = abyItem[38]; byItem039 = abyItem[39];
            byItem040 = abyItem[40]; byItem041 = abyItem[41]; byItem042 = abyItem[42]; byItem043 = abyItem[43];
            byItem044 = abyItem[44]; byItem045 = abyItem[45]; byItem046 = abyItem[46]; byItem047 = abyItem[47];
            byItem048 = abyItem[48]; byItem049 = abyItem[49]; byItem050 = abyItem[50]; byItem051 = abyItem[51];
            byItem052 = abyItem[52]; byItem053 = abyItem[53]; byItem054 = abyItem[54]; byItem055 = abyItem[55];
            byItem056 = abyItem[56]; byItem057 = abyItem[57]; byItem058 = abyItem[58]; byItem059 = abyItem[59];
            byItem060 = abyItem[60]; byItem061 = abyItem[61]; byItem062 = abyItem[62]; byItem063 = abyItem[63];
            byItem064 = abyItem[64]; byItem065 = abyItem[65]; byItem066 = abyItem[66]; byItem067 = abyItem[67];
            byItem068 = abyItem[68]; byItem069 = abyItem[69]; byItem070 = abyItem[70]; byItem071 = abyItem[71];
            byItem072 = abyItem[72]; byItem073 = abyItem[73]; byItem074 = abyItem[74]; byItem075 = abyItem[75];
            byItem076 = abyItem[76]; byItem077 = abyItem[77]; byItem078 = abyItem[78]; byItem079 = abyItem[79];
            byItem080 = abyItem[80]; byItem081 = abyItem[81]; byItem082 = abyItem[82]; byItem083 = abyItem[83];
            byItem084 = abyItem[84]; byItem085 = abyItem[85]; byItem086 = abyItem[86]; byItem087 = abyItem[87];
            byItem088 = abyItem[88]; byItem089 = abyItem[89]; byItem090 = abyItem[90]; byItem091 = abyItem[91];
            byItem092 = abyItem[92]; byItem093 = abyItem[93]; byItem094 = abyItem[94]; byItem095 = abyItem[95];
            byItem096 = abyItem[96]; byItem097 = abyItem[97]; byItem098 = abyItem[98]; byItem099 = abyItem[99];
            byItem100 = abyItem[100]; byItem101 = abyItem[101]; byItem102 = abyItem[102]; byItem103 = abyItem[103];
            byItem104 = abyItem[104]; byItem105 = abyItem[105]; byItem106 = abyItem[106]; byItem107 = abyItem[107];
            byItem108 = abyItem[108]; byItem109 = abyItem[109]; byItem110 = abyItem[110]; byItem111 = abyItem[111];
            byItem112 = abyItem[112]; byItem113 = abyItem[113]; byItem114 = abyItem[114]; byItem115 = abyItem[115];
            byItem116 = abyItem[116]; byItem117 = abyItem[117]; byItem118 = abyItem[118]; byItem119 = abyItem[119];
            byItem120 = abyItem[120]; byItem121 = abyItem[121]; byItem122 = abyItem[122]; byItem123 = abyItem[123];
            byItem124 = abyItem[124]; byItem125 = abyItem[125]; byItem126 = abyItem[126]; byItem127 = abyItem[127];
            byItem128 = abyItem[128]; byItem129 = abyItem[129];
        }
    }

}
