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
    /// Used for strings that go up to 256-bytes...
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct TW_STR255
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
        private byte byItem128; private byte byItem129; private byte byItem130; private byte byItem131;
        private byte byItem132; private byte byItem133; private byte byItem134; private byte byItem135;
        private byte byItem136; private byte byItem137; private byte byItem138; private byte byItem139;
        private byte byItem140; private byte byItem141; private byte byItem142; private byte byItem143;
        private byte byItem144; private byte byItem145; private byte byItem146; private byte byItem147;
        private byte byItem148; private byte byItem149; private byte byItem150; private byte byItem151;
        private byte byItem152; private byte byItem153; private byte byItem154; private byte byItem155;
        private byte byItem156; private byte byItem157; private byte byItem158; private byte byItem159;
        private byte byItem160; private byte byItem161; private byte byItem162; private byte byItem163;
        private byte byItem164; private byte byItem165; private byte byItem166; private byte byItem167;
        private byte byItem168; private byte byItem169; private byte byItem170; private byte byItem171;
        private byte byItem172; private byte byItem173; private byte byItem174; private byte byItem175;
        private byte byItem176; private byte byItem177; private byte byItem178; private byte byItem179;
        private byte byItem180; private byte byItem181; private byte byItem182; private byte byItem183;
        private byte byItem184; private byte byItem185; private byte byItem186; private byte byItem187;
        private byte byItem188; private byte byItem189; private byte byItem190; private byte byItem191;
        private byte byItem192; private byte byItem193; private byte byItem194; private byte byItem195;
        private byte byItem196; private byte byItem197; private byte byItem198; private byte byItem199;
        private byte byItem200; private byte byItem201; private byte byItem202; private byte byItem203;
        private byte byItem204; private byte byItem205; private byte byItem206; private byte byItem207;
        private byte byItem208; private byte byItem209; private byte byItem210; private byte byItem211;
        private byte byItem212; private byte byItem213; private byte byItem214; private byte byItem215;
        private byte byItem216; private byte byItem217; private byte byItem218; private byte byItem219;
        private byte byItem220; private byte byItem221; private byte byItem222; private byte byItem223;
        private byte byItem224; private byte byItem225; private byte byItem226; private byte byItem227;
        private byte byItem228; private byte byItem229; private byte byItem230; private byte byItem231;
        private byte byItem232; private byte byItem233; private byte byItem234; private byte byItem235;
        private byte byItem236; private byte byItem237; private byte byItem238; private byte byItem239;
        private byte byItem240; private byte byItem241; private byte byItem242; private byte byItem243;
        private byte byItem244; private byte byItem245; private byte byItem246; private byte byItem247;
        private byte byItem248; private byte byItem249; private byte byItem250; private byte byItem251;
        private byte byItem252; private byte byItem253; private byte byItem254; private byte byItem255;

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
            byte[] abyItem = new byte[256];
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
            abyItem[128] = byItem128; abyItem[129] = byItem129; abyItem[130] = byItem130; abyItem[131] = byItem131;
            abyItem[132] = byItem132; abyItem[133] = byItem133; abyItem[134] = byItem134; abyItem[135] = byItem135;
            abyItem[136] = byItem136; abyItem[137] = byItem137; abyItem[138] = byItem138; abyItem[139] = byItem139;
            abyItem[140] = byItem140; abyItem[141] = byItem141; abyItem[142] = byItem142; abyItem[143] = byItem143;
            abyItem[144] = byItem144; abyItem[145] = byItem145; abyItem[146] = byItem146; abyItem[147] = byItem147;
            abyItem[148] = byItem148; abyItem[149] = byItem149; abyItem[150] = byItem150; abyItem[151] = byItem151;
            abyItem[152] = byItem152; abyItem[153] = byItem153; abyItem[154] = byItem154; abyItem[155] = byItem155;
            abyItem[156] = byItem156; abyItem[157] = byItem157; abyItem[158] = byItem158; abyItem[159] = byItem159;
            abyItem[160] = byItem160; abyItem[161] = byItem161; abyItem[162] = byItem162; abyItem[163] = byItem163;
            abyItem[164] = byItem164; abyItem[165] = byItem165; abyItem[166] = byItem166; abyItem[167] = byItem167;
            abyItem[168] = byItem168; abyItem[169] = byItem169; abyItem[170] = byItem170; abyItem[171] = byItem171;
            abyItem[172] = byItem172; abyItem[173] = byItem173; abyItem[174] = byItem174; abyItem[175] = byItem175;
            abyItem[176] = byItem176; abyItem[177] = byItem177; abyItem[178] = byItem178; abyItem[179] = byItem179;
            abyItem[180] = byItem180; abyItem[181] = byItem181; abyItem[182] = byItem182; abyItem[183] = byItem183;
            abyItem[184] = byItem184; abyItem[185] = byItem185; abyItem[186] = byItem186; abyItem[187] = byItem187;
            abyItem[188] = byItem188; abyItem[189] = byItem189; abyItem[190] = byItem190; abyItem[191] = byItem191;
            abyItem[192] = byItem192; abyItem[193] = byItem193; abyItem[194] = byItem194; abyItem[195] = byItem195;
            abyItem[196] = byItem196; abyItem[197] = byItem197; abyItem[198] = byItem198; abyItem[199] = byItem199;
            abyItem[200] = byItem200; abyItem[201] = byItem201; abyItem[202] = byItem202; abyItem[203] = byItem203;
            abyItem[204] = byItem204; abyItem[205] = byItem205; abyItem[206] = byItem206; abyItem[207] = byItem207;
            abyItem[208] = byItem208; abyItem[209] = byItem209; abyItem[210] = byItem210; abyItem[211] = byItem211;
            abyItem[212] = byItem212; abyItem[213] = byItem213; abyItem[214] = byItem214; abyItem[215] = byItem215;
            abyItem[216] = byItem216; abyItem[217] = byItem217; abyItem[218] = byItem218; abyItem[219] = byItem219;
            abyItem[220] = byItem220; abyItem[221] = byItem221; abyItem[222] = byItem222; abyItem[223] = byItem223;
            abyItem[224] = byItem224; abyItem[225] = byItem225; abyItem[226] = byItem226; abyItem[227] = byItem227;
            abyItem[228] = byItem228; abyItem[229] = byItem229; abyItem[230] = byItem230; abyItem[231] = byItem231;
            abyItem[232] = byItem232; abyItem[233] = byItem233; abyItem[234] = byItem234; abyItem[235] = byItem235;
            abyItem[236] = byItem236; abyItem[237] = byItem237; abyItem[238] = byItem238; abyItem[239] = byItem239;
            abyItem[240] = byItem240; abyItem[241] = byItem241; abyItem[242] = byItem242; abyItem[243] = byItem243;
            abyItem[244] = byItem244; abyItem[245] = byItem245; abyItem[246] = byItem246; abyItem[247] = byItem247;
            abyItem[248] = byItem248; abyItem[249] = byItem249; abyItem[250] = byItem250; abyItem[251] = byItem251;
            abyItem[252] = byItem252; abyItem[253] = byItem253; abyItem[254] = byItem254; abyItem[255] = byItem255;

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
                "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" +
                "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" +
                "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0" +
                "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0";
            if (sz.Length > 256)
            {
                sz = sz.Remove(256);
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
            byItem128 = abyItem[128]; byItem129 = abyItem[129]; byItem130 = abyItem[130]; byItem131 = abyItem[131];
            byItem132 = abyItem[132]; byItem133 = abyItem[133]; byItem134 = abyItem[134]; byItem135 = abyItem[135];
            byItem136 = abyItem[136]; byItem137 = abyItem[137]; byItem138 = abyItem[138]; byItem139 = abyItem[139];
            byItem140 = abyItem[140]; byItem141 = abyItem[141]; byItem142 = abyItem[142]; byItem143 = abyItem[143];
            byItem144 = abyItem[144]; byItem145 = abyItem[145]; byItem146 = abyItem[146]; byItem147 = abyItem[147];
            byItem148 = abyItem[148]; byItem149 = abyItem[149]; byItem150 = abyItem[150]; byItem151 = abyItem[151];
            byItem152 = abyItem[152]; byItem153 = abyItem[153]; byItem154 = abyItem[154]; byItem155 = abyItem[155];
            byItem156 = abyItem[156]; byItem157 = abyItem[157]; byItem158 = abyItem[158]; byItem159 = abyItem[159];
            byItem160 = abyItem[160]; byItem161 = abyItem[161]; byItem162 = abyItem[162]; byItem163 = abyItem[163];
            byItem164 = abyItem[164]; byItem165 = abyItem[165]; byItem166 = abyItem[166]; byItem167 = abyItem[167];
            byItem168 = abyItem[168]; byItem169 = abyItem[169]; byItem170 = abyItem[170]; byItem171 = abyItem[171];
            byItem172 = abyItem[172]; byItem173 = abyItem[173]; byItem174 = abyItem[174]; byItem175 = abyItem[175];
            byItem176 = abyItem[176]; byItem177 = abyItem[177]; byItem178 = abyItem[178]; byItem179 = abyItem[179];
            byItem180 = abyItem[180]; byItem181 = abyItem[181]; byItem182 = abyItem[182]; byItem183 = abyItem[183];
            byItem184 = abyItem[184]; byItem185 = abyItem[185]; byItem186 = abyItem[186]; byItem187 = abyItem[187];
            byItem188 = abyItem[188]; byItem189 = abyItem[189]; byItem190 = abyItem[190]; byItem191 = abyItem[191];
            byItem192 = abyItem[192]; byItem193 = abyItem[193]; byItem194 = abyItem[194]; byItem195 = abyItem[195];
            byItem196 = abyItem[196]; byItem197 = abyItem[197]; byItem198 = abyItem[198]; byItem199 = abyItem[199];
            byItem200 = abyItem[200]; byItem201 = abyItem[201]; byItem202 = abyItem[202]; byItem203 = abyItem[203];
            byItem204 = abyItem[204]; byItem205 = abyItem[205]; byItem206 = abyItem[206]; byItem207 = abyItem[207];
            byItem208 = abyItem[208]; byItem209 = abyItem[209]; byItem210 = abyItem[210]; byItem211 = abyItem[211];
            byItem212 = abyItem[212]; byItem213 = abyItem[213]; byItem214 = abyItem[214]; byItem215 = abyItem[215];
            byItem216 = abyItem[216]; byItem217 = abyItem[217]; byItem218 = abyItem[218]; byItem219 = abyItem[219];
            byItem220 = abyItem[220]; byItem221 = abyItem[221]; byItem222 = abyItem[222]; byItem223 = abyItem[223];
            byItem224 = abyItem[224]; byItem225 = abyItem[225]; byItem226 = abyItem[226]; byItem227 = abyItem[227];
            byItem228 = abyItem[228]; byItem229 = abyItem[229]; byItem230 = abyItem[230]; byItem231 = abyItem[231];
            byItem232 = abyItem[232]; byItem233 = abyItem[233]; byItem234 = abyItem[234]; byItem235 = abyItem[235];
            byItem236 = abyItem[236]; byItem237 = abyItem[237]; byItem238 = abyItem[238]; byItem239 = abyItem[239];
            byItem240 = abyItem[240]; byItem241 = abyItem[241]; byItem242 = abyItem[242]; byItem243 = abyItem[243];
            byItem244 = abyItem[244]; byItem245 = abyItem[245]; byItem246 = abyItem[246]; byItem247 = abyItem[247];
            byItem248 = abyItem[248]; byItem249 = abyItem[249]; byItem250 = abyItem[250]; byItem251 = abyItem[251];
            byItem252 = abyItem[252]; byItem253 = abyItem[253]; byItem254 = abyItem[254]; byItem255 = abyItem[255];
        }
    }
}
