using ScannerDrivers.CaptureExample.Dtos;
using ScannerDrivers.CaptureExample.EventArgs;

namespace ScannerDrivers.CaptureExample.Services
{
    public partial class TwainService
    {
        public event EventHandler ScanningCompleted;
        public event EventHandler<ImageEventArgs> ImageAcquired;
        //public event Action<byte[]> ImageAcquired;
        public event EventHandler ScanStopped;
        public event EventHandler ScanPaused;
        public event EventHandler<string> ErrorOccurred;
        public event EventHandler TwainInitialized;
        public event EventHandler TwainClosed;
        private async void OnScanningCompleted(object sender, System.EventArgs e)
        {
            if (ScanningCompleted != null)
            {
                await Task.Run(() => ScanningCompleted.Invoke(this, System.EventArgs.Empty));
            }
        }

        private int _count = 0;

        private async void OnImageAcquired(IntPtr m_intptrImage, int imageData, byte[] imageBytes)
        {
            int currentCount;
            lock (this) // Ensure thread safety
            {
                currentCount = ++_count; // Increment count for each invocation
            }

            // Create an instance of ImageData with the byte array and current count
            ImageData imageDataDto = new ImageData(imageData, currentCount, m_intptrImage, imageBytes, Guid.NewGuid().ToString());

            // Invoke the ImageAcquired event with ImageData
            if (ImageAcquired != null)
            {
                await Task.Run(() => ImageAcquired.Invoke(this, new ImageEventArgs(imageDataDto)));
            }
        }
        private void OnScanStopped()
        {
            ScanStopped?.Invoke(this, System.EventArgs.Empty);
        }

        private void OnScanPaused()
        {
            ScanPaused?.Invoke(this, System.EventArgs.Empty);
        }

        private void OnErrorOccurred(string errorMessage)
        {
            ErrorOccurred?.Invoke(this, errorMessage);
        }

        private void OnTwainInitialized()
        {
            TwainInitialized?.Invoke(this, System.EventArgs.Empty);
        }

        private void OnTwainClosed()
        {
            TwainClosed?.Invoke(this, System.EventArgs.Empty);
        }

    }
}
