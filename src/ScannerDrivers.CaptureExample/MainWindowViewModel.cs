using Prism.Commands;
using Prism.Mvvm;
using ScannerDrivers.CaptureExample.EventArgs;
using ScannerDrivers.CaptureExample.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;


namespace ScannerDrivers.CaptureExample
{
    public class MainWindowViewModel : BindableBase
    {
        private ObservableCollection<BitmapImage> _imageList;
        public ObservableCollection<BitmapImage> ImageList
        {
            get { return _imageList; }
            set { SetProperty(ref _imageList, value); }
        }

        private BitmapImage _selectedImage;
        public BitmapImage SelectedImage
        {
            get { return _selectedImage; }
            set { SetProperty(ref _selectedImage, value); }
        }

        private ObservableCollection<string> _scanners;
        public ObservableCollection<string> Scanners
        {
            get { return _scanners; }
            set
            {
                SetProperty(ref _scanners, value);
            }
        }

        private string _selectedScanner;
        public string SelectedScanner
        {
            get { return _selectedScanner; }
            set
            {
                if (SetProperty(ref _selectedScanner, value))
                {
                    var response = _twainService.SelectScanner(_selectedScanner).Result;
                }
            }
        }

        // Commands
        public DelegateCommand StartScanCommand { get; private set; }
        public DelegateCommand StopScanCommand { get; private set; }

        private readonly TwainService _twainService;
        private static bool _eventHandlersSubscribed = false;
        // Constructor
        public MainWindowViewModel()
        {
            _twainService = new TwainService();
            ImageList = new ObservableCollection<BitmapImage>();
            Scanners = new ObservableCollection<string>();

            // Initialize commands
            StartScanCommand = new DelegateCommand(StartScan);
            StopScanCommand = new DelegateCommand(StopScan);

            string directoryPath = @"C:\Demo-Scanning";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            try
            {
                _ = OpenDsmAndAssignScannerList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            if (!_eventHandlersSubscribed)
            {
                _twainService.ScanningCompleted += OnScanningCompleted;
                _twainService.ImageAcquired += OnImageAcquired;
                _twainService.ScanStopped += OnScanStopped;
                _twainService.ScanPaused += OnScanPaused;
                _twainService.ErrorOccurred += OnErrorOccurred;
                _twainService.TwainInitialized += OnTwainInitialized;
                _twainService.TwainClosed += OnTwainClosed;

                _eventHandlersSubscribed = true;
            }
        }

        private async Task OpenDsmAndAssignScannerList()
        {
            try
            {
                nint WindowHandle = (nint)Process.GetCurrentProcess().MainWindowHandle;
                _twainService.InitializeTwain(WindowHandle);

                var scannerListResponse = _twainService.GetScannerList();
                if (scannerListResponse != null)
                {
                    Scanners = new ObservableCollection<string>(scannerListResponse);
                    //if (Scanners.Count > 0)
                    //{
                    //    SelectedScanner = Scanners.FirstOrDefault();
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void StartScan()
        {
            var response = await _twainService.StartScanning();
        }

        private async void StopScan()
        {
            var response = await _twainService.StopScanning();
        }

        #region Callbacks
        private async void OnScanningCompleted(object sender, System.EventArgs e)
        {
            MessageBox.Show("Scanning completed.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private async void OnImageAcquired(object sender, ImageEventArgs e)
        {
            await Task.Run(() =>
            {
                string imagepath = Path.Combine("C:\\Demo-Scanning", e.ImageData.ImageGUID + ".tif");
                using (var image = SixLabors.ImageSharp.Image.Load(e.ImageData.ImageBytes))
                {
                    using (FileStream fileStream = new FileStream(imagepath, FileMode.Create))
                    {
                        image.SaveAsJpeg(fileStream, new JpegEncoder { Quality = 80 });
                    }
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.DecodePixelWidth = 800;
                    bitmapImage.DecodePixelHeight = 600;
                    bitmapImage.UriSource = new Uri(imagepath);
                    bitmapImage.EndInit();
                    SelectedImage = bitmapImage;
                    ImageList.Add(bitmapImage);
                });
            });
        }



        private void OnScanStopped(object sender, System.EventArgs e)
        {
            //MessageBox.Show("Scan stopped.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnScanPaused(object sender, System.EventArgs e)
        {
            // MessageBox.Show("Scan paused.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnErrorOccurred(object sender, string errorMessage)
        {
            // MessageBox.Show($"Error occurred: {errorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void OnTwainInitialized(object sender, System.EventArgs e)
        {
            // MessageBox.Show("TWAIN initialized.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnTwainClosed(object sender, System.EventArgs e)
        {
            // MessageBox.Show("TWAIN closed.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
    }
}
