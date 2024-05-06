using ScannerDrivers.TWAIN;
using ScannerDrivers.TWAIN.Contants.Capability.Enum;
using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;
using ScannerDrivers.TWAIN.Contants.Generic;
using System.Runtime.InteropServices;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.CaptureExample.Services
{
    public partial class TwainService
    {
        //int _imageCount = 0;
        private void CaptureImages()
        {
            Codes.STS sts;
            TW_IMAGEINFO twimageinfo = default(TW_IMAGEINFO);
            TW_IMAGEMEMXFER twimagememxfer = default(TW_IMAGEMEMXFER);
            TW_PENDINGXFERS twpendingxfers = default(TW_PENDINGXFERS);
            TW_USERINTERFACE twuserinterface = default(TW_USERINTERFACE);

            // Dispatch on the state...
            switch (_Twain.GetState())
            {
                // Not a good state, just scoot...
                default:
                    return;

                // We're on our way out...
                case STATE.S5:// GUI up or waiting to transfer first image
                    m_blDisableDsSent = true;
                    _Twain.DatUserinterface(DataGroups.CONTROL, Messages.DISABLEDS, ref twuserinterface);

                    return;

                // Memory transfers...
                case STATE.S6:// ready to start transferring image
                case STATE.S7:// transferring image or transfer done
                    TWAINSDK.CsvToImagememxfer(ref twimagememxfer, "0,0,0,0,0,0,0," + ((int)TWMF.APPOWNS | (int)TWMF.POINTER) + "," + m_twsetupmemxfer.Preferred + "," + m_intptrXfer);
                    sts = _Twain.DatImagememxfer(DataGroups.IMAGE, Messages.GET, ref twimagememxfer);

                    break;
            }

            // Handle problems...
            if ((sts != Codes.STS.SUCCESS) && (sts != Codes.STS.XFERDONE))
            {

                m_blDisableDsSent = true;
                Rollback(STATE.S4);// Data Source open, programmatic mode (no GUI)

                return;
            }

            // Allocate or grow the image memory...
            if (m_intptrImage == IntPtr.Zero)
            {
                m_intptrImage = Marshal.AllocHGlobal((int)twimagememxfer.BytesWritten);
            }
            else
            {
                m_intptrImage = Marshal.ReAllocHGlobal(m_intptrImage, (IntPtr)(m_iImageBytes + twimagememxfer.BytesWritten));
            }

            // Ruh-roh...
            if (m_intptrImage == IntPtr.Zero)
            {

                m_blDisableDsSent = true;
                Rollback(STATE.S4);// Data Source open, programmatic mode (no GUI)
                                   // IsScanning = false;

                return;
            }

            // Copy into the buffer, and bump up our byte tally...
            TWAINSDK.MemCpy(m_intptrImage + m_iImageBytes, m_intptrXfer, (int)twimagememxfer.BytesWritten);
            m_iImageBytes += (int)twimagememxfer.BytesWritten;


            // If we saw XFERDONE we can save the image, display it,
            // end the transfer, and see if we have more images...
            if (sts == Codes.STS.XFERDONE)
            {

                // Bump up our image counter, this always grows for the
                // life of the entire session...
                LastFileNumber += 1;

                // Get the image info...
                sts = _Twain.DatImageinfo(DataGroups.IMAGE, Messages.GET, ref twimageinfo);

                // Bitonal uncompressed...
                if (((ICAP_PIXELTYPE)twimageinfo.PixelType == ICAP_PIXELTYPE.BW) && ((ICAP_COMPRESSION)twimageinfo.Compression == ICAP_COMPRESSION.NONE))
                {
                    TWAINSDK.TiffBitonalUncompressed tiffbitonaluncompressed;
                    tiffbitonaluncompressed = new TWAINSDK.TiffBitonalUncompressed((uint)twimageinfo.ImageWidth, (uint)twimageinfo.ImageLength, (uint)twimageinfo.XResolution.Whole, (uint)m_iImageBytes);
                    m_intptrImage = Marshal.ReAllocHGlobal(m_intptrImage, (IntPtr)(Marshal.SizeOf(tiffbitonaluncompressed) + m_iImageBytes));
                    TWAINSDK.MemMove((IntPtr)((UInt64)m_intptrImage + (UInt64)Marshal.SizeOf(tiffbitonaluncompressed)), m_intptrImage, m_iImageBytes);
                    Marshal.StructureToPtr(tiffbitonaluncompressed, m_intptrImage, true);
                    m_iImageBytes += (int)Marshal.SizeOf(tiffbitonaluncompressed);
                }

                // Bitonal GROUP4...
                else if (((ICAP_PIXELTYPE)twimageinfo.PixelType == ICAP_PIXELTYPE.BW) && ((ICAP_COMPRESSION)twimageinfo.Compression == ICAP_COMPRESSION.GROUP4))
                {
                    TWAINSDK.TiffBitonalG4 tiffbitonalg4;
                    tiffbitonalg4 = new TWAINSDK.TiffBitonalG4((uint)twimageinfo.ImageWidth, (uint)twimageinfo.ImageLength, (uint)twimageinfo.XResolution.Whole, (uint)m_iImageBytes);
                    m_intptrImage = Marshal.ReAllocHGlobal(m_intptrImage, (IntPtr)(Marshal.SizeOf(tiffbitonalg4) + m_iImageBytes));
                    TWAINSDK.MemMove((IntPtr)((UInt64)m_intptrImage + (UInt64)Marshal.SizeOf(tiffbitonalg4)), m_intptrImage, m_iImageBytes);
                    Marshal.StructureToPtr(tiffbitonalg4, m_intptrImage, true);
                    m_iImageBytes += (int)Marshal.SizeOf(tiffbitonalg4);
                }

                // Gray uncompressed...
                else if (((ICAP_PIXELTYPE)twimageinfo.PixelType == ICAP_PIXELTYPE.GRAY) && ((ICAP_COMPRESSION)twimageinfo.Compression == ICAP_COMPRESSION.NONE))
                {
                    TWAINSDK.TiffGrayscaleUncompressed tiffgrayscaleuncompressed;
                    tiffgrayscaleuncompressed = new TWAINSDK.TiffGrayscaleUncompressed((uint)twimageinfo.ImageWidth, (uint)twimageinfo.ImageLength, (uint)twimageinfo.XResolution.Whole, (uint)m_iImageBytes);
                    m_intptrImage = Marshal.ReAllocHGlobal(m_intptrImage, (IntPtr)(Marshal.SizeOf(tiffgrayscaleuncompressed) + m_iImageBytes));
                    TWAINSDK.MemMove((IntPtr)((UInt64)m_intptrImage + (UInt64)Marshal.SizeOf(tiffgrayscaleuncompressed)), m_intptrImage, m_iImageBytes);
                    Marshal.StructureToPtr(tiffgrayscaleuncompressed, m_intptrImage, true);
                    m_iImageBytes += (int)Marshal.SizeOf(tiffgrayscaleuncompressed);
                }
                // Gray JPEG...
                else if (((ICAP_PIXELTYPE)twimageinfo.PixelType == ICAP_PIXELTYPE.GRAY) && ((ICAP_COMPRESSION)twimageinfo.Compression == ICAP_COMPRESSION.JPEG))
                {
                    TWAINSDK.TiffGrayscaleCompressed tiffgrayscalecompressed;
                    tiffgrayscalecompressed = new TWAINSDK.TiffGrayscaleCompressed((uint)twimageinfo.ImageWidth, (uint)twimageinfo.ImageLength, (uint)twimageinfo.XResolution.Whole, (uint)m_iImageBytes);
                    m_intptrImage = Marshal.ReAllocHGlobal(m_intptrImage, (IntPtr)(Marshal.SizeOf(tiffgrayscalecompressed) + m_iImageBytes));
                    TWAINSDK.MemMove((IntPtr)((UInt64)m_intptrImage + (UInt64)Marshal.SizeOf(tiffgrayscalecompressed)), m_intptrImage, m_iImageBytes);
                    Marshal.StructureToPtr(tiffgrayscalecompressed, m_intptrImage, true);
                    m_iImageBytes += (int)Marshal.SizeOf(tiffgrayscalecompressed);
                }
                //// Gray JPEG...
                //else if (((ICAP_PIXELTYPE)twimageinfo.PixelType == ICAP_PIXELTYPE.GRAY) && ((ICAP_COMPRESSION)twimageinfo.Compression == ICAP_COMPRESSION.JPEG))
                //{
                //    m_blDisableDsSent = true;
                //    Rollback(STATE.S4); // Data Source open, programmatic mode (no GUI)
                //    Console.WriteLine("Please Disable Compression From Scanner Settings");
                //    // IsScanning = false;
                //    return;
                //}

                // RGB uncompressed...
                else if (((ICAP_PIXELTYPE)twimageinfo.PixelType == ICAP_PIXELTYPE.RGB) && ((ICAP_COMPRESSION)twimageinfo.Compression == ICAP_COMPRESSION.NONE))
                {
                    TWAINSDK.TiffColorUncompressed tiffcoloruncompressed;
                    tiffcoloruncompressed = new TWAINSDK.TiffColorUncompressed((uint)twimageinfo.ImageWidth, (uint)twimageinfo.ImageLength, (uint)twimageinfo.XResolution.Whole, (uint)m_iImageBytes);
                    m_intptrImage = Marshal.ReAllocHGlobal(m_intptrImage, (IntPtr)(Marshal.SizeOf(tiffcoloruncompressed) + m_iImageBytes));
                    TWAINSDK.MemMove((IntPtr)((UInt64)m_intptrImage + (UInt64)Marshal.SizeOf(tiffcoloruncompressed)), m_intptrImage, m_iImageBytes);
                    Marshal.StructureToPtr(tiffcoloruncompressed, m_intptrImage, true);
                    m_iImageBytes += (int)Marshal.SizeOf(tiffcoloruncompressed);
                }

                // RGB JPEG...
                else if (((ICAP_PIXELTYPE)twimageinfo.PixelType == ICAP_PIXELTYPE.RGB) && ((ICAP_COMPRESSION)twimageinfo.Compression == ICAP_COMPRESSION.JPEG))
                {

                    TWAINSDK.TiffColorCompressed tiffcolorcompressed;
                    tiffcolorcompressed = new TWAINSDK.TiffColorCompressed((uint)twimageinfo.ImageWidth, (uint)twimageinfo.ImageLength, (uint)twimageinfo.XResolution.Whole, (uint)m_iImageBytes);
                    m_intptrImage = Marshal.ReAllocHGlobal(m_intptrImage, (IntPtr)(Marshal.SizeOf(tiffcolorcompressed) + m_iImageBytes));
                    TWAINSDK.MemMove((IntPtr)((UInt64)m_intptrImage + (UInt64)Marshal.SizeOf(tiffcolorcompressed)), m_intptrImage, m_iImageBytes);
                    Marshal.StructureToPtr(tiffcolorcompressed, m_intptrImage, true);
                    m_iImageBytes += (int)Marshal.SizeOf(tiffcolorcompressed);
                }

                //// RGB JPEG...
                //else if (((ICAP_PIXELTYPE)twimageinfo.PixelType == ICAP_PIXELTYPE.RGB) && ((ICAP_COMPRESSION)twimageinfo.Compression == ICAP_COMPRESSION.JPEG))
                //{

                //    m_blDisableDsSent = true;
                //    Rollback(STATE.S4); // Data Source open, programmatic mode (no GUI)
                //    Console.WriteLine("Please Disable Compression From Scanner Settings");
                //    //IsScanning = false;
                //    return;
                //}
                else
                {
                    //IsScanning = false;
                    Log.Error("unsupported format <" + twimageinfo.PixelType + "," + twimageinfo.Compression + ">");
                    m_blDisableDsSent = true;
                    Rollback(STATE.S4);// Data Source open, programmatic mode (no GUI)

                    return;
                }
                //string filePath = Path.Combine("C:\\TestingCapture", $"image_{_imageCount}.jpg");
                //File.WriteAllBytes(filePath, new byte[m_iImageBytes]);
                //string szFilename = Path.Combine("C:\\TestingCapture", "img" + string.Format("{0:D6}", _imageCount));
                //TWAIN.WriteImageFile(szFilename, m_intptrImage, m_iImageBytes, out szFilename);
                byte[] imageBytes = new byte[m_iImageBytes];
                Marshal.Copy(m_intptrImage, imageBytes, 0, m_iImageBytes);
                Marshal.FreeHGlobal(m_intptrImage);
                //string szFilename = Path.Combine(ScannedFolderPath, "img" + string.Format("{0:D6}", LastFileNumber) + ".jpg");
                //var imageData = new ImageProcessingInput
                //{
                //    ImageBytes = imageBytes,
                //    FolderPath = ScannedFolderPath,
                //    ImageName = szFilename
                //};
                ////_imageProcessingService.EnqueueImage(imageData);
                ////_barcodeProcessingService.EnqueueImage(imageData);
                OnImageAcquired(m_intptrImage, m_iImageBytes, imageBytes);
                //Reset the Values
                m_intptrImage = IntPtr.Zero;
                m_iImageBytes = 0;

                // End the transfer...
                _Twain.DatPendingxfers(DataGroups.CONTROL, Messages.ENDXFER, ref twpendingxfers);

                // Looks like we're done!
                if (twpendingxfers.Count == 0)
                {
                    OnScanningCompleted(this, System.EventArgs.Empty);
                    //IsScanning = false;
                    m_blDisableDsSent = true;
                    _Twain.DatUserinterface(DataGroups.CONTROL, Messages.DISABLEDS, ref twuserinterface);
                    return;
                }
            }

        }
    }
}
