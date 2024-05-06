using ScannerDrivers.TWAIN.Contants.Enum;

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
        private static string CvtCapValueFromTwlg(string a_szValue)
        {
            // mono goes "hork", probably because the enum is wackadoodle, this
            // does work on .net, but what'cha gonna do?
            if (a_szValue.ToUpperInvariant().StartsWith("TWLG_"))
            {
                switch (a_szValue.ToUpperInvariant().Substring(5))
                {
                    default: break;
                    case "USERLOCALE": return ("65535"); // -1, kinda...
                    case "DAN": return ("0");
                    case "DUT": return ("1");
                    case "ENG": return ("2");
                    case "FCF": return ("3");
                    case "FIN": return ("4");
                    case "FRN": return ("5");
                    case "GER": return ("6");
                    case "ICE": return ("7");
                    case "ITN": return ("8");
                    case "NOR": return ("9");
                    case "POR": return ("10");
                    case "SPA": return ("11");
                    case "SWE": return ("12");
                    case "USA": return ("13");
                    case "AFRIKAANS": return ("14");
                    case "ALBANIA": return ("15");
                    case "ARABIC": return ("16");
                    case "ARABIC_ALGERIA": return ("17");
                    case "ARABIC_BAHRAIN": return ("18");
                    case "ARABIC_EGYPT": return ("19");
                    case "ARABIC_IRAQ": return ("20");
                    case "ARABIC_JORDAN": return ("21");
                    case "ARABIC_KUWAIT": return ("22");
                    case "ARABIC_LEBANON": return ("23");
                    case "ARABIC_LIBYA": return ("24");
                    case "ARABIC_MOROCCO": return ("25");
                    case "ARABIC_OMAN": return ("26");
                    case "ARABIC_QATAR": return ("27");
                    case "ARABIC_SAUDIARABIA": return ("28");
                    case "ARABIC_SYRIA": return ("29");
                    case "ARABIC_TUNISIA": return ("30");
                    case "ARABIC_UAE": return ("31");
                    case "ARABIC_YEMEN": return ("32");
                    case "BASQUE": return ("33");
                    case "BYELORUSSIAN": return ("34");
                    case "BULGARIAN": return ("35");
                    case "CATALAN": return ("36");
                    case "CHINESE": return ("37");
                    case "CHINESE_HONGKONG": return ("38");
                    case "CHINESE_PRC": return ("39");
                    case "CHINESE_SINGAPORE": return ("40");
                    case "CHINESE_SIMPLIFIED": return ("41");
                    case "CHINESE_TAIWAN": return ("42");
                    case "CHINESE_TRADITIONAL": return ("43");
                    case "CROATIA": return ("44");
                    case "CZECH": return ("45");
                    case "DANISH": return (((int)Languages.DAN).ToString());
                    case "DUTCH": return (((int)Languages.DUT).ToString());
                    case "DUTCH_BELGIAN": return ("46");
                    case "ENGLISH": return (((int)Languages.ENG).ToString());
                    case "ENGLISH_AUSTRALIAN": return ("47");
                    case "ENGLISH_CANADIAN": return ("48");
                    case "ENGLISH_IRELAND": return ("49");
                    case "ENGLISH_NEWZEALAND": return ("50");
                    case "ENGLISH_SOUTHAFRICA": return ("51");
                    case "ENGLISH_UK": return ("52");
                    case "ENGLISH_USA": return (((int)Languages.USA).ToString());
                    case "ESTONIAN": return ("53");
                    case "FAEROESE": return ("54");
                    case "FARSI": return ("55");
                    case "FINNISH": return (((int)Languages.FIN).ToString());
                    case "FRENCH": return (((int)Languages.FRN).ToString());
                    case "FRENCH_BELGIAN": return ("56");
                    case "FRENCH_CANADIAN": return (((int)Languages.FCF).ToString());
                    case "FRENCH_LUXEMBOURG": return ("57");
                    case "FRENCH_SWISS": return ("58");
                    case "GERMAN": return (((int)Languages.GER).ToString());
                    case "GERMAN_AUSTRIAN": return ("59");
                    case "GERMAN_LUXEMBOURG": return ("60");
                    case "GERMAN_LIECHTENSTEIN": return ("61");
                    case "GERMAN_SWISS": return ("62");
                    case "GREEK": return ("63");
                    case "HEBREW": return ("64");
                    case "HUNGARIAN": return ("65");
                    case "ICELANDIC": return (((int)Languages.ICE).ToString());
                    case "INDONESIAN": return ("66");
                    case "ITALIAN": return (((int)Languages.ITN).ToString());
                    case "ITALIAN_SWISS": return ("67");
                    case "JAPANESE": return ("68");
                    case "KOREAN": return ("69");
                    case "KOREAN_JOHAB": return ("70");
                    case "LATVIAN": return ("71");
                    case "LITHUANIAN": return ("72");
                    case "NORWEGIAN": return (((int)Languages.NOR).ToString());
                    case "NORWEGIAN_BOKMAL": return ("73");
                    case "NORWEGIAN_NYNORSK": return ("74");
                    case "POLISH": return ("75");
                    case "PORTUGUESE": return (((int)Languages.POR).ToString());
                    case "PORTUGUESE_BRAZIL": return ("76");
                    case "ROMANIAN": return ("77");
                    case "RUSSIAN": return ("78");
                    case "SERBIAN_LATIN": return ("79");
                    case "SLOVAK": return ("80");
                    case "SLOVENIAN": return ("81");
                    case "SPANISH": return (((int)Languages.SPA).ToString());
                    case "SPANISH_MEXICAN": return ("82");
                    case "SPANISH_MODERN": return ("83");
                    case "SWEDISH": return (((int)Languages.SWE).ToString());
                    case "THAI": return ("84");
                    case "TURKISH": return ("85");
                    case "UKRANIAN": return ("86");
                    case "ASSAMESE": return ("87");
                    case "BENGALI": return ("88");
                    case "BIHARI": return ("89");
                    case "BODO": return ("90");
                    case "DOGRI": return ("91");
                    case "GUJARATI": return ("92");
                    case "HARYANVI": return ("93");
                    case "HINDI": return ("94");
                    case "KANNADA": return ("95");
                    case "KASHMIRI": return ("96");
                    case "MALAYALAM": return ("97");
                    case "MARATHI": return ("98");
                    case "MARWARI": return ("99");
                    case "MEGHALAYAN": return ("100");
                    case "MIZO": return ("101");
                    case "NAGA": return ("102");
                    case "ORISSI": return ("103");
                    case "PUNJABI": return ("104");
                    case "PUSHTU": return ("105");
                    case "SERBIAN_CYRILLIC": return ("106");
                    case "SIKKIMI": return ("107");
                    case "SWEDISH_FINLAND": return ("108");
                    case "TAMIL": return ("109");
                    case "TELUGU": return ("110");
                    case "TRIPURI": return ("111");
                    case "URDU": return ("112");
                    case "VIETNAMESE": return ("113");
                }
            }

            // Er...
            return (a_szValue);
        }
    }
}
