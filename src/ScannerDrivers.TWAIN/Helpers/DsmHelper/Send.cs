using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;
using ScannerDrivers.TWAIN.Contants.Structure;
using System.Globalization;
using System.Runtime.InteropServices;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {

        /// <summary>
        /// Send a command to the currently loaded DSM...
        /// </summary>
        /// <param name="a_functionarguments">tokenized command and anything needed</param>
        /// <returns>true to quit</returns>
        public Codes.STS Send(string a_szDg, string a_szDat, string a_szMsg, ref string a_szTwmemref, ref string a_szResult)
        {
            int iDg;
            int iDat;
            int iMsg;
            Codes.STS sts;
            DataGroups dg = DataGroups.MASK;
            DataArgumentTypes dat = DataArgumentTypes.NULL;
            Messages msg = Messages.NULL;

            // Init stuff...
            iDg = 0;
            iDat = 0;
            iMsg = 0;
            sts = Codes.STS.BADPROTOCOL;
            a_szResult = "";

            // Look for DG...
            if (!a_szDg.ToLowerInvariant().StartsWith("dg_"))
            {
                Log.Error("Unrecognized dg - <" + a_szDg + ">");
                return (Codes.STS.BADPROTOCOL);
            }
            else
            {
                // Look for hex number (take anything)...
                if (a_szDg.ToLowerInvariant().StartsWith("dg_0x"))
                {
                    if (!int.TryParse(a_szDg.ToLowerInvariant().Substring(3), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out iDg))
                    {
                        Log.Error("Badly constructed dg - <" + a_szDg + ">");
                        return (Codes.STS.BADPROTOCOL);
                    }
                }
                else
                {
                    if (!Enum.TryParse(a_szDg.ToUpperInvariant().Substring(3), out dg))
                    {
                        Log.Error("Unrecognized dg - <" + a_szDg + ">");
                        return (Codes.STS.BADPROTOCOL);
                    }
                    iDg = (int)dg;
                }
            }

            // Look for DAT...
            if (!a_szDat.ToLowerInvariant().StartsWith("dat_"))
            {
                Log.Error("Unrecognized dat - <" + a_szDat + ">");
                return (Codes.STS.BADPROTOCOL);
            }
            else
            {
                // Look for hex number (take anything)...
                if (a_szDat.ToLowerInvariant().StartsWith("dat_0x"))
                {
                    if (!int.TryParse(a_szDat.ToLowerInvariant().Substring(4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out iDat))
                    {
                        Log.Error("Badly constructed dat - <" + a_szDat + ">");
                        return (Codes.STS.BADPROTOCOL);
                    }
                }
                else
                {
                    if (!Enum.TryParse(a_szDat.ToUpperInvariant().Substring(4), out dat))
                    {
                        Log.Error("Unrecognized dat - <" + a_szDat + ">");
                        return (Codes.STS.BADPROTOCOL);
                    }
                    iDat = (int)dat;
                }
            }

            // Look for MSG...
            if (!a_szMsg.ToLowerInvariant().StartsWith("msg_"))
            {
                TWAIN.Log.Error("Unrecognized msg - <" + a_szMsg + ">");
                return (Codes.STS.BADPROTOCOL);
            }
            else
            {
                // Look for hex number (take anything)...
                if (a_szMsg.ToLowerInvariant().StartsWith("msg_0x"))
                {
                    if (!int.TryParse(a_szMsg.ToLowerInvariant().Substring(4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out iMsg))
                    {
                        Log.Error("Badly constructed dat - <" + a_szMsg + ">");
                        return (Codes.STS.BADPROTOCOL);
                    }
                }
                else
                {
                    if (!Enum.TryParse(a_szMsg.ToUpperInvariant().Substring(4), out msg))
                    {
                        Log.Error("Unrecognized msg - <" + a_szMsg + ">");
                        return (Codes.STS.BADPROTOCOL);
                    }
                    iMsg = (int)msg;
                }
            }

            // Send the command...
            switch (iDat)
            {
                // Ruh-roh, since we can't marshal it, we have to return an error,
                // it would be nice to have a solution for this, but that will need
                // a dynamic marshalling system...
                default:
                    sts = Codes.STS.BADPROTOCOL;
                    break;

                // DAT_AUDIOFILEXFER...
                case (int)DataArgumentTypes.AUDIOFILEXFER:
                    {
                        sts = DatAudiofilexfer((DataGroups)iDg, (Messages)iMsg);
                        a_szTwmemref = "";
                    }
                    break;

                // DAT_AUDIOINFO..
                case (int)DataArgumentTypes.AUDIOINFO:
                    {
                        TW_AUDIOINFO twaudioinfo = default(TW_AUDIOINFO);
                        sts = DatAudioinfo((DataGroups)iDg, (Messages)iMsg, ref twaudioinfo);
                        a_szTwmemref = AudioinfoToCsv(twaudioinfo);
                    }
                    break;

                // DAT_AUDIONATIVEXFER..
                case (int)DataArgumentTypes.AUDIONATIVEXFER:
                    {
                        IntPtr intptr = IntPtr.Zero;
                        sts = DatAudionativexfer((DataGroups)iDg, (Messages)iMsg, ref intptr);
                        a_szTwmemref = intptr.ToString();
                    }
                    break;

                // DAT_CALLBACK...
                case (int)DataArgumentTypes.CALLBACK:
                    {
                        TW_CALLBACK twcallback = default(TW_CALLBACK);
                        CsvToCallback(ref twcallback, a_szTwmemref);
                        sts = DatCallback((DataGroups)iDg, (Messages)iMsg, ref twcallback);
                        a_szTwmemref = CallbackToCsv(twcallback);
                    }
                    break;

                // DAT_CALLBACK2...
                case (int)DataArgumentTypes.CALLBACK2:
                    {
                        TW_CALLBACK2 twcallback2 = default(TW_CALLBACK2);
                        CsvToCallback2(ref twcallback2, a_szTwmemref);
                        sts = DatCallback2((DataGroups)iDg, (Messages)iMsg, ref twcallback2);
                        a_szTwmemref = Callback2ToCsv(twcallback2);
                    }
                    break;

                // DAT_CAPABILITY...
                case (int)DataArgumentTypes.CAPABILITY:
                    {
                        // Skip symbols for msg_querysupport, otherwise 0 gets turned into false, also
                        // if the command fails the return value is whatever was sent into us, which
                        // matches the experience one should get with C/C++...
                        string szStatus = "";
                        TW_CAPABILITY twcapability = default(TW_CAPABILITY);
                        CsvToCapability(ref twcapability, ref szStatus, a_szTwmemref);
                        sts = DatCapability((DataGroups)iDg, (Messages)iMsg, ref twcapability);
                        if ((sts == Codes.STS.SUCCESS) || (sts == Codes.STS.CHECKSTATUS))
                        {
                            // Convert the data to CSV...
                            a_szTwmemref = CapabilityToCsv(twcapability, ((Messages)iMsg != Messages.QUERYSUPPORT));
                            // Free the handle if the driver created it...
                            switch ((Messages)iMsg)
                            {
                                default: break;
                                case Messages.GET:
                                case Messages.GETCURRENT:
                                case Messages.GETDEFAULT:
                                case Messages.QUERYSUPPORT:
                                case Messages.RESET:
                                    DsmMemFree(ref twcapability.hContainer);
                                    break;
                            }
                        }
                    }
                    break;

                // DAT_CIECOLOR..
                case (int)DataArgumentTypes.CIECOLOR:
                    {
                        //TW_CIECOLOR twciecolor = default(TW_CIECOLOR);
                        //sts = m_DataArgumentTypesCiecolor((DataGroups)iDg, (Messages)iMsg, ref twciecolor);
                        //a_szTwmemref = m_twain.CiecolorToCsv(twciecolor);
                    }
                    break;

                // DAT_CUSTOMDSDATA...
                case (int)DataArgumentTypes.CUSTOMDSDATA:
                    {
                        TW_CUSTOMDSDATA twcustomdsdata = default(TW_CUSTOMDSDATA);
                        CsvToCustomdsdata(ref twcustomdsdata, a_szTwmemref);
                        sts = DatCustomdsdata((DataGroups)iDg, (Messages)iMsg, ref twcustomdsdata);
                        a_szTwmemref = CustomdsdataToCsv(twcustomdsdata);
                    }
                    break;

                // DAT_DEVICEEVENT...
                case (int)DataArgumentTypes.DEVICEEVENT:
                    {
                        TW_DEVICEEVENT twdeviceevent = default(TW_DEVICEEVENT);
                        sts = DatDeviceevent((DataGroups)iDg, (Messages)iMsg, ref twdeviceevent);
                        a_szTwmemref = DeviceeventToCsv(twdeviceevent);
                    }
                    break;

                // DAT_ENTRYPOINT...
                case (int)DataArgumentTypes.ENTRYPOINT:
                    {
                        TW_ENTRYPOINT twentrypoint = default(TW_ENTRYPOINT);
                        twentrypoint.Size = (uint)Marshal.SizeOf(twentrypoint);
                        sts = DatEntrypoint((DataGroups)iDg, (Messages)iMsg, ref twentrypoint);
                        a_szTwmemref = EntrypointToCsv(twentrypoint);
                    }
                    break;

                // DAT_EVENT...
                case (int)DataArgumentTypes.EVENT:
                    {
                        TW_EVENT twevent = default(TW_EVENT);
                        sts = DatEvent((DataGroups)iDg, (Messages)iMsg, ref twevent);
                        a_szTwmemref = EventToCsv(twevent);
                    }
                    break;

                // DAT_EXTIMAGEINFO...
                case (int)DataArgumentTypes.EXTIMAGEINFO:
                    {
                        TW_EXTIMAGEINFO twextimageinfo = default(TW_EXTIMAGEINFO);
                        CsvToExtimageinfo(ref twextimageinfo, a_szTwmemref);
                        sts = DatExtimageinfo((DataGroups)iDg, (Messages)iMsg, ref twextimageinfo);
                        a_szTwmemref = ExtimageinfoToCsv(twextimageinfo);
                    }
                    break;

                // DAT_FILESYSTEM...
                case (int)DataArgumentTypes.FILESYSTEM:
                    {
                        TW_FILESYSTEM twfilesystem = default(TW_FILESYSTEM);
                        CsvToFilesystem(ref twfilesystem, a_szTwmemref);
                        sts = DatFilesystem((DataGroups)iDg, (Messages)iMsg, ref twfilesystem);
                        a_szTwmemref = FilesystemToCsv(twfilesystem);
                    }
                    break;

                // DAT_FILTER...
                case (int)DataArgumentTypes.FILTER:
                    {
                        //TW_FILTER twfilter = default(TW_FILTER);
                        //m_twain.CsvToFilter(ref twfilter, a_szTwmemref);
                        //sts = m_DataArgumentTypesFilter((DataGroups)iDg, (Messages)iMsg, ref twfilter);
                        //a_szTwmemref = m_twain.FilterToCsv(twfilter);
                    }
                    break;

                // DAT_GRAYRESPONSE...
                case (int)DataArgumentTypes.GRAYRESPONSE:
                    {
                        //TW_GRAYRESPONSE twgrayresponse = default(TW_GRAYRESPONSE);
                        //m_twain.CsvToGrayresponse(ref twgrayresponse, a_szTwmemref);
                        //sts = m_DataArgumentTypesGrayresponse((DataGroups)iDg, (Messages)iMsg, ref twgrayresponse);
                        //a_szTwmemref = m_twain.GrayresponseToCsv(twgrayresponse);
                    }
                    break;

                // DAT_ICCPROFILE...
                case (int)DataArgumentTypes.ICCPROFILE:
                    {
                        TW_MEMORY twmemory = default(TW_MEMORY);
                        sts = DatIccprofile((DataGroups)iDg, (Messages)iMsg, ref twmemory);
                        a_szTwmemref = IccprofileToCsv(twmemory);
                    }
                    break;

                // DAT_IDENTITY...
                case (int)DataArgumentTypes.IDENTITY:
                    {
                        TW_IDENTITY twidentity = default(TW_IDENTITY);
                        switch (iMsg)
                        {
                            default:
                                break;
                            case (int)Messages.SET:
                            case (int)Messages.OPENDS:
                                CsvToIdentity(ref twidentity, a_szTwmemref);
                                break;
                        }
                        sts = DatIdentity((DataGroups)iDg, (Messages)iMsg, ref twidentity);
                        a_szTwmemref = IdentityToCsv(twidentity);
                    }
                    break;

                // DAT_IMAGEFILEXFER...
                case (int)DataArgumentTypes.IMAGEFILEXFER:
                    {
                        sts = DatImagefilexfer((DataGroups)iDg, (Messages)iMsg);
                        a_szTwmemref = "";
                    }
                    break;

                // DAT_IMAGEINFO...
                case (int)DataArgumentTypes.IMAGEINFO:
                    {
                        TW_IMAGEINFO twimageinfo = default(TW_IMAGEINFO);
                        CsvToImageinfo(ref twimageinfo, a_szTwmemref);
                        sts = DatImageinfo((DataGroups)iDg, (Messages)iMsg, ref twimageinfo);
                        a_szTwmemref = ImageinfoToCsv(twimageinfo);
                    }
                    break;

                // DAT_IMAGELAYOUT...
                case (int)DataArgumentTypes.IMAGELAYOUT:
                    {
                        TW_IMAGELAYOUT twimagelayout = default(TW_IMAGELAYOUT);
                        CsvToImagelayout(ref twimagelayout, a_szTwmemref);
                        sts = DatImagelayout((DataGroups)iDg, (Messages)iMsg, ref twimagelayout);
                        a_szTwmemref = ImagelayoutToCsv(twimagelayout);
                    }
                    break;

                // DAT_IMAGEMEMFILEXFER...
                case (int)DataArgumentTypes.IMAGEMEMFILEXFER:
                    {
                        TW_IMAGEMEMXFER twimagememxfer = default(TW_IMAGEMEMXFER);
                        CsvToImagememxfer(ref twimagememxfer, a_szTwmemref);
                        sts = DatImagememfilexfer((DataGroups)iDg, (Messages)iMsg, ref twimagememxfer);
                        a_szTwmemref = ImagememxferToCsv(twimagememxfer);
                    }
                    break;

                // DAT_IMAGEMEMXFER...
                case (int)DataArgumentTypes.IMAGEMEMXFER:
                    {
                        TW_IMAGEMEMXFER twimagememxfer = default(TW_IMAGEMEMXFER);
                        CsvToImagememxfer(ref twimagememxfer, a_szTwmemref);
                        sts = DatImagememxfer((DataGroups)iDg, (Messages)iMsg, ref twimagememxfer);
                        a_szTwmemref = ImagememxferToCsv(twimagememxfer);
                    }
                    break;

                // DAT_IMAGENATIVEXFER...
                case (int)DataArgumentTypes.IMAGENATIVEXFER:
                    {
                        IntPtr intptrBitmapHandle = IntPtr.Zero;
                        sts = DatImagenativexferHandle((DataGroups)iDg, (Messages)iMsg, ref intptrBitmapHandle);
                        a_szTwmemref = intptrBitmapHandle.ToString();
                    }
                    break;

                // DAT_JPEGCOMPRESSION...
                case (int)DataArgumentTypes.JPEGCOMPRESSION:
                    {
                        //TW_JPEGCOMPRESSION twjpegcompression = default(TW_JPEGCOMPRESSION);
                        //m_twain.CsvToJpegcompression(ref twjpegcompression, a_szTwmemref);
                        //sts = m_DataArgumentTypesJpegcompression((DataGroups)iDg, (Messages)iMsg, ref twjpegcompression);
                        //a_szTwmemref = m_twain.JpegcompressionToCsv(twjpegcompression);
                    }
                    break;

                // DAT_METRICS...
                case (int)DataArgumentTypes.METRICS:
                    {
                        TW_METRICS twmetrics = default(TW_METRICS);
                        twmetrics.SizeOf = (uint)Marshal.SizeOf(twmetrics);
                        sts = DatMetrics((DataGroups)iDg, (Messages)iMsg, ref twmetrics);
                        a_szTwmemref = MetricsToCsv(twmetrics);
                    }
                    break;

                // DAT_PALETTE8...
                case (int)DataArgumentTypes.PALETTE8:
                    {
                        //TW_PALETTE8 twpalette8 = default(TW_PALETTE8);
                        //m_twain.CsvToPalette8(ref twpalette8, a_szTwmemref);
                        //sts = m_DataArgumentTypesPalette8((DataGroups)iDg, (Messages)iMsg, ref twpalette8);
                        //a_szTwmemref = m_twain.Palette8ToCsv(twpalette8);
                    }
                    break;

                // DAT_PARENT...
                case (int)DataArgumentTypes.PARENT:
                    {
                        sts = DatParent((DataGroups)iDg, (Messages)iMsg, ref m_intptrHwnd);
                        a_szTwmemref = "";
                    }
                    break;

                // DAT_PASSTHRU...
                case (int)DataArgumentTypes.PASSTHRU:
                    {
                        TW_PASSTHRU twpassthru = default(TW_PASSTHRU);
                        CsvToPassthru(ref twpassthru, a_szTwmemref);
                        sts = DatPassthru((DataGroups)iDg, (Messages)iMsg, ref twpassthru);
                        a_szTwmemref = PassthruToCsv(twpassthru);
                    }
                    break;

                // DAT_PENDINGXFERS...
                case (int)DataArgumentTypes.PENDINGXFERS:
                    {
                        TW_PENDINGXFERS twpendingxfers = default(TW_PENDINGXFERS);
                        sts = DatPendingxfers((DataGroups)iDg, (Messages)iMsg, ref twpendingxfers);
                        a_szTwmemref = PendingxfersToCsv(twpendingxfers);
                    }
                    break;

                // DAT_RGBRESPONSE...
                case (int)DataArgumentTypes.RGBRESPONSE:
                    {
                        //TW_RGBRESPONSE twrgbresponse = default(TW_RGBRESPONSE);
                        //m_twain.CsvToRgbresponse(ref twrgbresponse, a_szTwmemref);
                        //sts = m_DataArgumentTypesRgbresponse((DataGroups)iDg, (Messages)iMsg, ref twrgbresponse);
                        //a_szTwmemref = m_twain.RgbresponseToCsv(twrgbresponse);
                    }
                    break;

                // DAT_SETUPFILEXFER...
                case (int)DataArgumentTypes.SETUPFILEXFER:
                    {
                        TW_SETUPFILEXFER twsetupfilexfer = default(TW_SETUPFILEXFER);
                        CsvToSetupfilexfer(ref twsetupfilexfer, a_szTwmemref);
                        sts = DatSetupfilexfer((DataGroups)iDg, (Messages)iMsg, ref twsetupfilexfer);
                        a_szTwmemref = SetupfilexferToCsv(twsetupfilexfer);
                    }
                    break;

                // DAT_SETUPMEMXFER...
                case (int)DataArgumentTypes.SETUPMEMXFER:
                    {
                        TW_SETUPMEMXFER twsetupmemxfer = default(TW_SETUPMEMXFER);
                        sts = DatSetupmemxfer((DataGroups)iDg, (Messages)iMsg, ref twsetupmemxfer);
                        a_szTwmemref = SetupmemxferToCsv(twsetupmemxfer);
                    }
                    break;

                // DAT_STATUS...
                case (int)DataArgumentTypes.STATUS:
                    {
                        TW_STATUS twstatus = default(TW_STATUS);
                        sts = DatStatus((DataGroups)iDg, (Messages)iMsg, ref twstatus);
                        a_szTwmemref = StatusToCsv(twstatus);
                    }
                    break;

                // DAT_STATUSUTF8...
                case (int)DataArgumentTypes.STATUSUTF8:
                    {
                        TW_STATUSUTF8 twstatusutf8 = default(TW_STATUSUTF8);
                        sts = DatStatusutf8((DataGroups)iDg, (Messages)iMsg, ref twstatusutf8);
                        a_szTwmemref = Statusutf8ToCsv(twstatusutf8);
                    }
                    break;

                // DAT_TWAINDIRECT...
                case (int)DataArgumentTypes.TWAINDIRECT:
                    {
                        TW_TWAINDIRECT twtwaindirect = default(TW_TWAINDIRECT);
                        CsvToTwaindirect(ref twtwaindirect, a_szTwmemref);
                        sts = DatTwaindirect((DataGroups)iDg, (Messages)iMsg, ref twtwaindirect);
                        a_szTwmemref = TwaindirectToCsv(twtwaindirect);
                    }
                    break;

                // DAT_USERINTERFACE...
                case (int)DataArgumentTypes.USERINTERFACE:
                    {
                        TW_USERINTERFACE twuserinterface = default(TW_USERINTERFACE);
                        CsvToUserinterface(ref twuserinterface, a_szTwmemref);
                        sts = DatUserinterface((DataGroups)iDg, (Messages)iMsg, ref twuserinterface);
                        a_szTwmemref = UserinterfaceToCsv(twuserinterface);
                    }
                    break;

                // DAT_XFERGROUP...
                case (int)DataArgumentTypes.XFERGROUP:
                    {
                        uint uXferGroup = 0;
                        sts = DatXferGroup((DataGroups)iDg, (Messages)iMsg, ref uXferGroup);
                        a_szTwmemref = string.Format("0x{0:X}", uXferGroup);
                    }
                    break;
            }

            // All done...
            return (sts);
        }
    }
}
