using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScannerDrivers.CaptureExample
{
    /// <summary>
    /// Interaction logic for WindowHeader.xaml
    /// </summary>
    public partial class WindowHeader : UserControl
    {
        public static readonly DependencyProperty HeaderTitleProperty =
        DependencyProperty.Register("HeaderTitle", typeof(string), typeof(WindowHeader), new PropertyMetadata(string.Empty));


        public string HeaderTitle
        {
            get { return (string)GetValue(HeaderTitleProperty); }
            set { SetValue(HeaderTitleProperty, value); }
        }
        public WindowHeader()
        {
            InitializeComponent();
        }
        private bool IsMaximized = false;


        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {

                Window.GetWindow(this).DragMove();


            }
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximized)
                {

                    Window.GetWindow(this).WindowState = WindowState.Normal;
                    Window.GetWindow(this).Width = 1080;
                    Window.GetWindow(this).Height = 720;

                    IsMaximized = false;
                }
                else
                {


                    Window.GetWindow(this).WindowState = WindowState.Maximized;

                    IsMaximized = true;
                }
            }
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {

            if (Window.GetWindow(this).WindowState == WindowState.Minimized)
            {
                Window.GetWindow(this).WindowState = WindowState.Maximized;
            }
            else
            {
                Window.GetWindow(this).WindowState = WindowState.Minimized;

            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {

            Window.GetWindow(this).Close();
        }

        private void btn_restore_Click(object sender, RoutedEventArgs e)
        {

            if (Window.GetWindow(this).WindowState == WindowState.Normal)
            {
                Window.GetWindow(this).WindowState = WindowState.Maximized;
            }
            else
            {
                Window.GetWindow(this).WindowState = WindowState.Normal;
            }

        }

    }
}
