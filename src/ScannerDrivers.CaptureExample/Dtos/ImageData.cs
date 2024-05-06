namespace ScannerDrivers.CaptureExample.Dtos
{
    public class ImageData
    {
        public string ImageGUID { get; set; }
        public int Bytes { get; set; }
        public byte[] ImageBytes { get; set; }
        public int Number { get; set; }
        public IntPtr m_intptrImage { get; set; }

        public ImageData(int bytes, int number, IntPtr inputImage, byte[] imageBytes, string imageGUID)
        {
            Bytes = bytes;
            Number = number;
            m_intptrImage = inputImage;
            ImageBytes = imageBytes;
            ImageGUID = imageGUID;
        }
    }
}
