using ScannerDrivers.TWAIN.Contants.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants
{

    /// <summary>
    /// Handle encoding between C# and what the DS is currently set to.
    /// NOTE: this is static for users of this object do not have to track
    ///       the encoding (i.e. let TWAIN.cs deal with all of this). This
    ///       means there is one language for all open DSes, so the last one wins.
    /// </summary>

    public static class Language
    {
        /// <summary>
        /// The encoding to use for strings to/from the DS
        /// </summary>
        /// <returns></returns>
        public static Encoding GetEncoding()
        {
            return m_encoding;
        }

        /// <summary>
        /// The current language of the DS
        /// </summary>
        /// <returns></returns>
        public static void Set(Languages a_twlg)
        {

            switch (a_twlg)
            {
                default:
                    // NOTE: can only get here if a TWLG was added, but it wasn't added here
                    m_encoding = Encoding.GetEncoding(1252);
                    break;

                case Languages.USERLOCALE:
                    // NOTE: this should never come back from the DS. only here for completeness
                    m_encoding = Encoding.GetEncoding(1252);
                    break;

                case Languages.THAI:
                    m_encoding = Encoding.GetEncoding(874);
                    break;

                case Languages.JAPANESE:
                    m_encoding = Encoding.GetEncoding(932);
                    break;

                case Languages.CHINESE:
                case Languages.CHINESE_PRC:
                case Languages.CHINESE_SINGAPORE:
                case Languages.CHINESE_SIMPLIFIED:
                    m_encoding = Encoding.GetEncoding(936);
                    break;

                case Languages.KOREAN:
                case Languages.KOREAN_JOHAB:
                    m_encoding = Encoding.GetEncoding(949);
                    break;

                case Languages.CHINESE_HONGKONG:
                case Languages.CHINESE_TAIWAN:
                case Languages.CHINESE_TRADITIONAL:
                    m_encoding = Encoding.GetEncoding(950);
                    break;

                case Languages.ALBANIA:
                case Languages.CROATIA:
                case Languages.CZECH:
                case Languages.HUNGARIAN:
                case Languages.POLISH:
                case Languages.ROMANIAN:
                case Languages.SERBIAN_LATIN:
                case Languages.SLOVAK:
                case Languages.SLOVENIAN:
                    m_encoding = Encoding.GetEncoding(1250);
                    break;

                case Languages.BYELORUSSIAN:
                case Languages.BULGARIAN:
                case Languages.RUSSIAN:
                case Languages.SERBIAN_CYRILLIC:
                case Languages.UKRANIAN:
                    m_encoding = Encoding.GetEncoding(1251);
                    break;

                case Languages.AFRIKAANS:
                case Languages.BASQUE:
                case Languages.CATALAN:
                case Languages.DAN: // DANISH
                case Languages.DUT: // DUTCH
                case Languages.DUTCH_BELGIAN:
                case Languages.ENG: // ENGLISH
                case Languages.ENGLISH_AUSTRALIAN:
                case Languages.ENGLISH_CANADIAN:
                case Languages.ENGLISH_IRELAND:
                case Languages.ENGLISH_NEWZEALAND:
                case Languages.ENGLISH_SOUTHAFRICA:
                case Languages.ENGLISH_UK:
                case Languages.USA:
                case Languages.FAEROESE:
                case Languages.FIN: // FINNISH
                case Languages.FRN: // FRENCH
                case Languages.FRENCH_BELGIAN:
                case Languages.FCF: // FRENCH_CANADIAN
                case Languages.FRENCH_LUXEMBOURG:
                case Languages.FRENCH_SWISS:
                case Languages.GER: // GERMAN
                case Languages.GERMAN_AUSTRIAN:
                case Languages.GERMAN_LIECHTENSTEIN:
                case Languages.GERMAN_LUXEMBOURG:
                case Languages.GERMAN_SWISS:
                case Languages.ICE: // ICELANDIC
                case Languages.INDONESIAN:
                case Languages.ITN: // ITALIAN
                case Languages.ITALIAN_SWISS:
                case Languages.NOR: // NORWEGIAN
                case Languages.NORWEGIAN_BOKMAL:
                case Languages.NORWEGIAN_NYNORSK:
                case Languages.POR: // PORTUGUESE
                case Languages.PORTUGUESE_BRAZIL:
                case Languages.SPA: // SPANISH
                case Languages.SPANISH_MEXICAN:
                case Languages.SPANISH_MODERN:
                case Languages.SWE: // SWEDISH
                case Languages.SWEDISH_FINLAND:
                    m_encoding = Encoding.GetEncoding(1252);//1252//"ASCII")
                    break;

                case Languages.GREEK:
                    m_encoding = Encoding.GetEncoding(1253);
                    break;

                case Languages.TURKISH:
                    m_encoding = Encoding.GetEncoding(1254);
                    break;

                case Languages.HEBREW:
                    m_encoding = Encoding.GetEncoding(1255);
                    break;

                case Languages.ARABIC:
                case Languages.ARABIC_ALGERIA:
                case Languages.ARABIC_BAHRAIN:
                case Languages.ARABIC_EGYPT:
                case Languages.ARABIC_IRAQ:
                case Languages.ARABIC_JORDAN:
                case Languages.ARABIC_KUWAIT:
                case Languages.ARABIC_LEBANON:
                case Languages.ARABIC_LIBYA:
                case Languages.ARABIC_MOROCCO:
                case Languages.ARABIC_OMAN:
                case Languages.ARABIC_QATAR:
                case Languages.ARABIC_SAUDIARABIA:
                case Languages.ARABIC_SYRIA:
                case Languages.ARABIC_TUNISIA:
                case Languages.ARABIC_UAE:
                case Languages.ARABIC_YEMEN:
                case Languages.FARSI:
                case Languages.URDU:
                    m_encoding = Encoding.GetEncoding(1256);
                    break;

                case Languages.ESTONIAN:
                case Languages.LATVIAN:
                case Languages.LITHUANIAN:
                    m_encoding = Encoding.GetEncoding(1257);
                    break;

                case Languages.VIETNAMESE:
                    m_encoding = Encoding.GetEncoding(1258);
                    break;

                case Languages.ASSAMESE:
                case Languages.BENGALI:
                case Languages.BIHARI:
                case Languages.BODO:
                case Languages.DOGRI:
                case Languages.GUJARATI:
                case Languages.HARYANVI:
                case Languages.HINDI:
                case Languages.KANNADA:
                case Languages.KASHMIRI:
                case Languages.MALAYALAM:
                case Languages.MARATHI:
                case Languages.MARWARI:
                case Languages.MEGHALAYAN:
                case Languages.MIZO:
                case Languages.NAGA:
                case Languages.ORISSI:
                case Languages.PUNJABI:
                case Languages.PUSHTU:
                case Languages.SIKKIMI:
                case Languages.TAMIL:
                case Languages.TELUGU:
                case Languages.TRIPURI:
                    // NOTE: there are no known code pages for these, so we will use English
                    m_encoding = Encoding.GetEncoding(1252);//1252
                    break;
            }
        }

        private static Encoding m_encoding = Encoding.GetEncoding(1252);//ISO-8859-1
    }
}
