using ScannerDrivers.TWAIN.Contants.Capability.Enum;
using ScannerDrivers.TWAIN.Contants.Enum;
using ScannerDrivers.TWAIN.Contants.ImageAttributes;
using System.Globalization;
using static ScannerDrivers.TWAIN.Contants.Enum.ExtendedImage;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert a 'friendly' name to a numeric value...
        /// </summary>
        /// <param name="a_cap">capability driving the conversion</param>
        /// <param name="szValue">value to convert</param>
        /// <returns></returns>
        public static string CvtCapValueFromEnum(Capabilities a_cap, string a_szValue)
        {
            int ii;

            // Turn hex into a decimal...
            if (a_szValue.ToLowerInvariant().StartsWith("0x"))
            {
                return (int.Parse(a_szValue.Substring(2), NumberStyles.HexNumber).ToString());
            }

            // Skip numbers...
            if (int.TryParse(a_szValue, out ii))
            {
                return (a_szValue);
            }

            // Process text...
            switch (a_cap)
            {
                default: return (a_szValue);
                case Capabilities.ACAP_XFERMECH: { ICAP_XFERMECH twsx; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_XFERMECH>(a_szValue), out twsx) ? ((int)twsx).ToString() : a_szValue); };
                case Capabilities.CAP_ALARMS: { CAP_ALARMS twal; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP_ALARMS>(a_szValue), out twal) ? ((int)twal).ToString() : a_szValue); };
                case Capabilities.CAP_ALARMVOLUME: return (a_szValue);
                case Capabilities.CAP_AUTHOR: return (a_szValue);
                case Capabilities.CAP_AUTOFEED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_AUTOMATICCAPTURE: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_AUTOMATICSENSEMEDIUM: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_AUTOSCAN: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_BATTERYMINUTES: return (a_szValue);
                case Capabilities.CAP_BATTERYPERCENTAGE: return (a_szValue);
                case Capabilities.CAP_CAMERAENABLED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_CAMERAORDER: { ICAP_PIXELTYPE twpt; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_PIXELTYPE>(a_szValue), out twpt) ? ((int)twpt).ToString() : a_szValue); };
                case Capabilities.CAP_CAMERAPREVIEWUI: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_CAMERASIDE: { TWCS twcs; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWCS>(a_szValue), out twcs) ? ((int)twcs).ToString() : a_szValue); };
                case Capabilities.CAP_CAPTION: return (a_szValue);
                case Capabilities.CAP_CLEARPAGE: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_CUSTOMDSDATA: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_CUSTOMINTERFACEGUID: return (a_szValue);
                case Capabilities.CAP_DEVICEEVENT: { CAP_DEVICEEVENT twde; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP_DEVICEEVENT>(a_szValue), out twde) ? ((int)twde).ToString() : a_szValue); };
                case Capabilities.CAP_DEVICEONLINE: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_DEVICETIMEDATE: return (a_szValue);
                case Capabilities.CAP_DOUBLEFEEDDETECTION: { CAP_DOUBLEFEEDDETECTION twdf; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP_DOUBLEFEEDDETECTION>(a_szValue), out twdf) ? ((int)twdf).ToString() : a_szValue); };
                case Capabilities.CAP_DOUBLEFEEDDETECTIONLENGTH: return (a_szValue);
                case Capabilities.CAP_DOUBLEFEEDDETECTIONRESPONSE: { CAP_DOUBLEFEEDDETECTIONRESPONSE twdp; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP_DOUBLEFEEDDETECTIONRESPONSE>(a_szValue), out twdp) ? ((int)twdp).ToString() : a_szValue); };
                case Capabilities.CAP_DOUBLEFEEDDETECTIONSENSITIVITY: { CAP_DOUBLEFEEDDETECTIONSENSITIVITY twus; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP_DOUBLEFEEDDETECTIONSENSITIVITY>(a_szValue), out twus) ? ((int)twus).ToString() : a_szValue); };
                case Capabilities.CAP_DUPLEX: { CAP_DUPLEX twdx; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP_DUPLEX>(a_szValue), out twdx) ? ((int)twdx).ToString() : a_szValue); };
                case Capabilities.CAP_DUPLEXENABLED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_ENABLEDSUIONLY: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_ENDORSER: return (a_szValue);
                case Capabilities.CAP_EXTENDEDCAPS: { Capabilities cap; return (Enum.TryParse(CvtCapValueFromEnumHelper<Capabilities>(a_szValue), out cap) ? ((int)cap).ToString() : a_szValue); };
                case Capabilities.CAP_FEEDERALIGNMENT: { CAP_FEEDERALIGNMENT twfa; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP_FEEDERALIGNMENT>(a_szValue), out twfa) ? ((int)twfa).ToString() : a_szValue); };
                case Capabilities.CAP_FEEDERENABLED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_FEEDERLOADED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_FEEDERORDER: { CAP_FEEDERORDER twfo; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP_FEEDERORDER>(a_szValue), out twfo) ? ((int)twfo).ToString() : a_szValue); };
                case Capabilities.CAP_FEEDERPOCKET: { CAP_FEEDERPOCKET twfp; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP_FEEDERPOCKET>(a_szValue), out twfp) ? ((int)twfp).ToString() : a_szValue); };
                case Capabilities.CAP_FEEDERPREP: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_FEEDPAGE: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_IAFIELDA_LASTPAGE: return (a_szValue);
                case Capabilities.CAP_IAFIELDB_LASTPAGE: return (a_szValue);
                case Capabilities.CAP_IAFIELDC_LASTPAGE: return (a_szValue);
                case Capabilities.CAP_IAFIELDD_LASTPAGE: return (a_szValue);
                case Capabilities.CAP_IAFIELDA_LEVEL: { CAP_IAFIELD twia; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP_IAFIELD>(a_szValue), out twia) ? ((int)twia).ToString() : a_szValue); };
                case Capabilities.CAP_IAFIELDB_LEVEL: { CAP_IAFIELD twia; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP_IAFIELD>(a_szValue), out twia) ? ((int)twia).ToString() : a_szValue); };
                case Capabilities.CAP_IAFIELDC_LEVEL: { CAP_IAFIELD twia; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP_IAFIELD>(a_szValue), out twia) ? ((int)twia).ToString() : a_szValue); };
                case Capabilities.CAP_IAFIELDD_LEVEL: { CAP_IAFIELD twia; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP_IAFIELD>(a_szValue), out twia) ? ((int)twia).ToString() : a_szValue); };
                case Capabilities.CAP_IAFIELDA_PRINTFORMAT: return (a_szValue);
                case Capabilities.CAP_IAFIELDB_PRINTFORMAT: return (a_szValue);
                case Capabilities.CAP_IAFIELDC_PRINTFORMAT: return (a_szValue);
                case Capabilities.CAP_IAFIELDD_PRINTFORMAT: return (a_szValue);
                case Capabilities.CAP_IAFIELDA_VALUE: return (a_szValue);
                case Capabilities.CAP_IAFIELDB_VALUE: return (a_szValue);
                case Capabilities.CAP_IAFIELDC_VALUE: return (a_szValue);
                case Capabilities.CAP_IAFIELDD_VALUE: return (a_szValue);
                case Capabilities.CAP_IMAGEADDRESSENABLED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_INDICATORS: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_INDICATORSMODE: { CAP_INDICATORSMODE twci; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP_INDICATORSMODE>(a_szValue), out twci) ? ((int)twci).ToString() : a_szValue); };
                case Capabilities.CAP_JOBCONTROL: { CAP_JOBCONTROL twjc; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP_JOBCONTROL>(a_szValue), out twjc) ? ((int)twjc).ToString() : a_szValue); };
                case Capabilities.CAP_LANGUAGE: return (CvtCapValueFromTwlg(a_szValue));
                case Capabilities.CAP_MAXBATCHBUFFERS: return (a_szValue);
                case Capabilities.CAP_MICRENABLED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_PAPERDETECTABLE: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_PAPERHANDLING: { CAP_PAPERHANDLING twph; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP_PAPERHANDLING>(a_szValue), out twph) ? ((int)twph).ToString() : a_szValue); };
                case Capabilities.CAP_POWERSAVETIME: return (a_szValue);
                case Capabilities.CAP_POWERSUPPLY: { CAP_POWERSUPPLY twps; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP_POWERSUPPLY>(a_szValue), out twps) ? ((int)twps).ToString() : a_szValue); };
                case Capabilities.CAP_PRINTER: { CAP_PRINTER twpr; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP_PRINTER>(a_szValue), out twpr) ? ((int)twpr).ToString() : a_szValue); };
                case Capabilities.CAP_PRINTERCHARROTATION: return (a_szValue);
                case Capabilities.CAP_PRINTERENABLED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_PRINTERFONTSTYLE: { ICAP_PIXELFLAVOR twpf; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_PIXELFLAVOR>(a_szValue), out twpf) ? ((int)twpf).ToString() : a_szValue); };
                case Capabilities.CAP_PRINTERINDEX: return (a_szValue);
                case Capabilities.CAP_PRINTERINDEXLEADCHAR: return (a_szValue);
                case Capabilities.CAP_PRINTERINDEXMAXVALUE: return (a_szValue);
                case Capabilities.CAP_PRINTERINDEXNUMDIGITS: return (a_szValue);
                case Capabilities.CAP_PRINTERINDEXSTEP: return (a_szValue);
                case Capabilities.CAP_PRINTERINDEXTRIGGER: { CAP_PRINTERINDEXTRIGGER twct; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP_PRINTERINDEXTRIGGER>(a_szValue), out twct) ? ((int)twct).ToString() : a_szValue); };
                case Capabilities.CAP_PRINTERMODE: { CAP_PRINTERMODE twpm; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP_PRINTERMODE>(a_szValue), out twpm) ? ((int)twpm).ToString() : a_szValue); };
                case Capabilities.CAP_PRINTERSTRING: return (a_szValue);
                case Capabilities.CAP_PRINTERSTRINGPREVIEW: return (a_szValue);
                case Capabilities.CAP_PRINTERSUFFIX: return (a_szValue);
                case Capabilities.CAP_PRINTERVERTICALOFFSET: return (a_szValue);
                case Capabilities.CAP_REACQUIREALLOWED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_REWINDPAGE: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_SEGMENTED: { CAP_SEGMENTED twsg; return (Enum.TryParse(CvtCapValueFromEnumHelper<CAP_SEGMENTED>(a_szValue), out twsg) ? ((int)twsg).ToString() : a_szValue); };
                case Capabilities.CAP_SERIALNUMBER: return (a_szValue);
                case Capabilities.CAP_SHEETCOUNT: return (a_szValue);
                case Capabilities.CAP_SUPPORTEDCAPS: { Capabilities cap; return (Enum.TryParse(CvtCapValueFromEnumHelper<Capabilities>(a_szValue), out cap) ? ((int)cap).ToString() : a_szValue); };
                case Capabilities.CAP_SUPPORTEDCAPSSEGMENTUNIQUE: { Capabilities cap; return (Enum.TryParse(CvtCapValueFromEnumHelper<Capabilities>(a_szValue), out cap) ? ((int)cap).ToString() : a_szValue); };
                case Capabilities.CAP_SUPPORTEDDATS: { DataArgumentTypes dat; return (Enum.TryParse(CvtCapValueFromEnumHelper<DataArgumentTypes>(a_szValue), out dat) ? ((int)dat).ToString() : a_szValue); };
                case Capabilities.CAP_THUMBNAILSENABLED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_TIMEBEFOREFIRSTCAPTURE: return (a_szValue);
                case Capabilities.CAP_TIMEBETWEENCAPTURES: return (a_szValue);
                case Capabilities.CAP_TIMEDATE: return (a_szValue);
                case Capabilities.CAP_UICONTROLLABLE: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.CAP_XFERCOUNT: return (a_szValue);
                case Capabilities.ICAP_AUTOBRIGHT: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.ICAP_AUTODISCARDBLANKPAGES: { ICAP_AUTODISCARDBLANKPAGES twbp; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_AUTODISCARDBLANKPAGES>(a_szValue), out twbp) ? ((int)twbp).ToString() : a_szValue); };
                case Capabilities.ICAP_AUTOMATICBORDERDETECTION: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.ICAP_AUTOMATICCOLORENABLED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE: { ICAP_PIXELTYPE twpt; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_PIXELTYPE>(a_szValue), out twpt) ? ((int)twpt).ToString() : a_szValue); };
                case Capabilities.ICAP_AUTOMATICCROPUSESFRAME: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.ICAP_AUTOMATICDESKEW: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.ICAP_AUTOMATICLENGTHDETECTION: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.ICAP_AUTOMATICROTATE: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.ICAP_AUTOSIZE: { ICAP_AUTOSIZE twas; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_AUTOSIZE>(a_szValue), out twas) ? ((int)twas).ToString() : a_szValue); };
                case Capabilities.ICAP_BARCODEDETECTIONENABLED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.ICAP_BARCODEMAXRETRIES: return (a_szValue);
                case Capabilities.ICAP_BARCODEMAXSEARCHPRIORITIES: return (a_szValue);
                case Capabilities.ICAP_BARCODESEARCHMODE: { ICAP_BARCODESEARCHMODE twbd; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_BARCODESEARCHMODE>(a_szValue), out twbd) ? ((int)twbd).ToString() : a_szValue); };
                case Capabilities.ICAP_BARCODESEARCHPRIORITIES: { TWBT twbt; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWBT>(a_szValue), out twbt) ? ((int)twbt).ToString() : a_szValue); };
                case Capabilities.ICAP_BARCODETIMEOUT: return (a_szValue);
                case Capabilities.ICAP_BITDEPTH: return (a_szValue);
                case Capabilities.ICAP_BITDEPTHREDUCTION: { ICAP_BITDEPTHREDUCTION twbr; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_BITDEPTHREDUCTION>(a_szValue), out twbr) ? ((int)twbr).ToString() : a_szValue); };
                case Capabilities.ICAP_BITORDER: { ICAP_BITORDER twbo; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_BITORDER>(a_szValue), out twbo) ? ((int)twbo).ToString() : a_szValue); };
                case Capabilities.ICAP_BITORDERCODES: { ICAP_BITORDER twbo; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_BITORDER>(a_szValue), out twbo) ? ((int)twbo).ToString() : a_szValue); };
                case Capabilities.ICAP_BRIGHTNESS: return (a_szValue);
                case Capabilities.ICAP_CCITTKFACTOR: return (a_szValue);
                case Capabilities.ICAP_COLORMANAGEMENTENABLED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.ICAP_COMPRESSION: { ICAP_COMPRESSION twcp; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_COMPRESSION>(a_szValue), out twcp) ? ((int)twcp).ToString() : a_szValue); };
                case Capabilities.ICAP_CONTRAST: return (a_szValue);
                case Capabilities.ICAP_CUSTHALFTONE: return (a_szValue);
                case Capabilities.ICAP_EXPOSURETIME: return (a_szValue);
                case Capabilities.ICAP_EXTIMAGEINFO: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.ICAP_FEEDERTYPE: { ICAP_FEEDERTYPE twfe; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_FEEDERTYPE>(a_szValue), out twfe) ? ((int)twfe).ToString() : a_szValue); };
                case Capabilities.ICAP_FILMTYPE: { ICAP_FILMTYPE twfm; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_FILMTYPE>(a_szValue), out twfm) ? ((int)twfm).ToString() : a_szValue); };
                case Capabilities.ICAP_FILTER: { ICAP_FILTER twft; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_FILTER>(a_szValue), out twft) ? ((int)twft).ToString() : a_szValue); };
                case Capabilities.ICAP_FLASHUSED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.ICAP_FLASHUSED2: { ICAP_FLASHUSED2 twfl; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_FLASHUSED2>(a_szValue), out twfl) ? ((int)twfl).ToString() : a_szValue); };
                case Capabilities.ICAP_FLIPROTATION: { ICAP_FLIPROTATION twfr; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_FLIPROTATION>(a_szValue), out twfr) ? ((int)twfr).ToString() : a_szValue); };
                case Capabilities.ICAP_FRAMES: return (a_szValue);
                case Capabilities.ICAP_GAMMA: return (a_szValue);
                case Capabilities.ICAP_HALFTONES: return (a_szValue);
                case Capabilities.ICAP_HIGHLIGHT: return (a_szValue);
                case Capabilities.ICAP_ICCPROFILE: { ICAP_ICCPROFILE twic; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_ICCPROFILE>(a_szValue), out twic) ? ((int)twic).ToString() : a_szValue); };
                case Capabilities.ICAP_IMAGEDATASET: return (a_szValue);
                case Capabilities.ICAP_IMAGEFILEFORMAT: { ICAP_IMAGEFILEFORMAT twff; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_IMAGEFILEFORMAT>(a_szValue), out twff) ? ((int)twff).ToString() : a_szValue); };
                case Capabilities.ICAP_IMAGEFILTER: { ICAP_IMAGEFILTER twif; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_IMAGEFILTER>(a_szValue), out twif) ? ((int)twif).ToString() : a_szValue); };
                case Capabilities.ICAP_IMAGEMERGE: { ICAP_IMAGEMERGE twim; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_IMAGEMERGE>(a_szValue), out twim) ? ((int)twim).ToString() : a_szValue); };
                case Capabilities.ICAP_IMAGEMERGEHEIGHTTHRESHOLD: return (a_szValue);
                case Capabilities.ICAP_JPEGPIXELTYPE: { ICAP_PIXELTYPE twpt; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_PIXELTYPE>(a_szValue), out twpt) ? ((int)twpt).ToString() : a_szValue); };
                case Capabilities.ICAP_JPEGQUALITY: { ICAP_JPEGQUALITY twjq; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_JPEGQUALITY>(a_szValue), out twjq) ? ((int)twjq).ToString() : a_szValue); };
                case Capabilities.ICAP_JPEGSUBSAMPLING: { ICAP_JPEGSUBSAMPLING twjs; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_JPEGSUBSAMPLING>(a_szValue), out twjs) ? ((int)twjs).ToString() : a_szValue); };
                case Capabilities.ICAP_LAMPSTATE: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.ICAP_LIGHTPATH: { ICAP_LIGHTPATH twlp; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_LIGHTPATH>(a_szValue), out twlp) ? ((int)twlp).ToString() : a_szValue); };
                case Capabilities.ICAP_LIGHTSOURCE: { ICAP_LIGHTSOURCE twls; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_LIGHTSOURCE>(a_szValue), out twls) ? ((int)twls).ToString() : a_szValue); };
                case Capabilities.ICAP_MAXFRAMES: return (a_szValue);
                case Capabilities.ICAP_MINIMUMHEIGHT: return (a_szValue);
                case Capabilities.ICAP_MINIMUMWIDTH: return (a_szValue);
                case Capabilities.ICAP_MIRROR: { ICAP_MIRROR twmr; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_MIRROR>(a_szValue), out twmr) ? ((int)twmr).ToString() : a_szValue); };
                case Capabilities.ICAP_NOISEFILTER: { ICAP_NOISEFILTER twnf; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_NOISEFILTER>(a_szValue), out twnf) ? ((int)twnf).ToString() : a_szValue); };
                case Capabilities.ICAP_ORIENTATION: { ICAP_ORIENTATION twor; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_ORIENTATION>(a_szValue), out twor) ? ((int)twor).ToString() : a_szValue); };
                case Capabilities.ICAP_OVERSCAN: { ICAP_OVERSCAN twov; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_OVERSCAN>(a_szValue), out twov) ? ((int)twov).ToString() : a_szValue); };
                case Capabilities.ICAP_PATCHCODEDETECTIONENABLED: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.ICAP_PATCHCODEMAXRETRIES: return (a_szValue);
                case Capabilities.ICAP_PATCHCODEMAXSEARCHPRIORITIES: return (a_szValue);
                case Capabilities.ICAP_PATCHCODESEARCHMODE: { ICAP_BARCODESEARCHMODE twbd; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_BARCODESEARCHMODE>(a_szValue), out twbd) ? ((int)twbd).ToString() : a_szValue); };
                case Capabilities.ICAP_PATCHCODESEARCHPRIORITIES: { TWEI_PATCHCODE twpch; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWEI_PATCHCODE>(a_szValue), out twpch) ? ((int)twpch).ToString() : a_szValue); };
                case Capabilities.ICAP_PATCHCODETIMEOUT: return (a_szValue);
                case Capabilities.ICAP_PHYSICALHEIGHT: return (a_szValue);
                case Capabilities.ICAP_PHYSICALWIDTH: return (a_szValue);
                case Capabilities.ICAP_PIXELFLAVOR: { ICAP_PIXELFLAVOR twpf; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_PIXELFLAVOR>(a_szValue), out twpf) ? ((int)twpf).ToString() : a_szValue); };
                case Capabilities.ICAP_PIXELFLAVORCODES: { ICAP_PIXELFLAVOR twpf; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_PIXELFLAVOR>(a_szValue), out twpf) ? ((int)twpf).ToString() : a_szValue); };
                case Capabilities.ICAP_PIXELTYPE: { ICAP_PIXELTYPE twpt; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_PIXELTYPE>(a_szValue), out twpt) ? ((int)twpt).ToString() : a_szValue); };
                case Capabilities.ICAP_PLANARCHUNKY: { ICAP_PLANARCHUNKY twpc; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_PLANARCHUNKY>(a_szValue), out twpc) ? ((int)twpc).ToString() : a_szValue); };
                case Capabilities.ICAP_ROTATION: return (a_szValue);
                case Capabilities.ICAP_SHADOW: return (a_szValue);
                case Capabilities.ICAP_SUPPORTEDBARCODETYPES: { TWBT twbt; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWBT>(a_szValue), out twbt) ? ((int)twbt).ToString() : a_szValue); };
                case Capabilities.ICAP_SUPPORTEDEXTIMAGEINFO: { TWEI twei; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWEI>(a_szValue), out twei) ? ((int)twei).ToString() : a_szValue); };
                case Capabilities.ICAP_SUPPORTEDPATCHCODETYPES: { TWEI_PATCHCODE twpch; return (Enum.TryParse(CvtCapValueFromEnumHelper<TWEI_PATCHCODE>(a_szValue), out twpch) ? ((int)twpch).ToString() : a_szValue); };
                case Capabilities.ICAP_SUPPORTEDSIZES: { ICAP_SUPPORTEDSIZES twss; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_SUPPORTEDSIZES>(a_szValue), out twss) ? ((int)twss).ToString() : a_szValue); };
                case Capabilities.ICAP_THRESHOLD: return (a_szValue);
                case Capabilities.ICAP_TILES: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.ICAP_TIMEFILL: return (a_szValue);
                case Capabilities.ICAP_UNDEFINEDIMAGESIZE: return (CvtCapValueFromEnumHelper<bool>(a_szValue));
                case Capabilities.ICAP_UNITS: { ICAP_UNITS twun; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_UNITS>(a_szValue), out twun) ? ((int)twun).ToString() : a_szValue); };
                case Capabilities.ICAP_XFERMECH: { ICAP_XFERMECH twsx; return (Enum.TryParse(CvtCapValueFromEnumHelper<ICAP_XFERMECH>(a_szValue), out twsx) ? ((int)twsx).ToString() : a_szValue); };
                case Capabilities.ICAP_XNATIVERESOLUTION: return (a_szValue);
                case Capabilities.ICAP_XRESOLUTION: return (a_szValue);
                case Capabilities.ICAP_XSCALING: return (a_szValue);
                case Capabilities.ICAP_YNATIVERESOLUTION: return (a_szValue);
                case Capabilities.ICAP_YRESOLUTION: return (a_szValue);
                case Capabilities.ICAP_YSCALING: return (a_szValue);
                case Capabilities.ICAP_ZOOMFACTOR: return (a_szValue);
            }
        }
    }
}
