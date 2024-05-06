using ScannerDrivers.CaptureExample.Dtos;

namespace ScannerDrivers.CaptureExample.EventArgs
{
    public class ImageEventArgs : System.EventArgs
    {
        public ImageData ImageData { get; }

        public ImageEventArgs(ImageData imageData)
        {
            ImageData = imageData;
        }
    }
}
