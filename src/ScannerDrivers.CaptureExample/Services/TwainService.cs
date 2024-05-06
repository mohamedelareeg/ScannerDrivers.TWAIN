
using ScannerDrivers.TWAIN;
using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Interop;
using System.Windows.Threading;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;
using static ScannerDrivers.TWAIN.TWAINSDK;

namespace ScannerDrivers.CaptureExample.Services
{

    public partial class TwainService
    {

        private string ScannedFolderPath;
        private int LastFileNumber = 0;

        #region Fields
        private TWAINSDK _Twain;
        private nint windowHandle;
        private ObservableCollection<string> scannerList;
        private Dispatcher uiDispatcher;

        private nint m_intptrHwnd;
        private bool m_blDisableDsSent = false;
        private bool m_blXferReadySent = false;
        private nint m_intptrXfer = nint.Zero;
        private nint m_intptrImage = nint.Zero;
        private int m_iImageBytes = 0;
        private TW_SETUPMEMXFER m_twsetupmemxfer;
        // Setup information...
        private bool m_blIndicators;
        public delegate void RunInUiThreadDelegate(Action a_action);//Object a_object,
        private bool m_blExit;
        #endregion

        public void InitializeTwain(nint windowHandle)
        {
            this.windowHandle = windowHandle;
            uiDispatcher = Dispatcher.CurrentDispatcher;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding(1252);
            ComponentDispatcher.ThreadFilterMessage += ComponentDispatcher_ThreadFilterMessage;

            try
            {
                DeviceEventCallback deviceeventcallback = DeviceEventCallback;
                ScanCallback scancallback = ScanCallbackTrigger;
                TWAINSDK.RunInUiThreadDelegate runinuithreaddelegate = RunInUiThread;

                _Twain = new TWAINSDK(
                    "CapturePro",
                    "TWAIN",
                    "TWAIN",
                    (ushort)TWAIN_PROTOCOL.MAJOR,
                    (ushort)TWAIN_PROTOCOL.MINOR,
                    (uint)DataGroups.APP2 | (uint)DataGroups.CONTROL | (uint)DataGroups.IMAGE,
                    Countries.USA,
                    "TWAIN",
                    Languages.ENGLISH_USA,
                    2,
                    4,
                    false,
                    false,
                    deviceeventcallback,
                    scancallback,
                    runinuithreaddelegate,
                    windowHandle
                );

                initSelection();
            }
            catch (Exception exception)
            {
                Log.Error("exception - " + exception.Message);
                _Twain = null;
            }
        }

        public void ShutdownTwain()
        {
            if (_Twain != null)
            {
                _Twain.Dispose();
                _Twain = null;
            }
        }
        #region Thread Locking - Tracking Scanner Status
        private void ComponentDispatcher_ThreadFilterMessage(ref MSG message, ref bool handled)
        {
            try
            {
                if (_Twain != null)
                {
                    _Twain.PreFilterMessage(message.hwnd, message.message, message.wParam, message.lParam);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
        #region UI Refresh
        private void ProcessEvents()//Refresh the UI >> Prevent it from Freezing
        {
            DispatcherFrame frame = new DispatcherFrame();

            // Post a callback to exit the frame after processing a certain number of messages (2 in this case)
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(parameter =>
                {
                    ((DispatcherFrame)parameter).Continue = false;
                    return null;
                }), frame);

            // Start the message loop
            Dispatcher.PushFrame(frame);
        }
        private void RunInUiThread(Action a_action)
        {
            Dispatcher.CurrentDispatcher.Invoke(a_action);
        }
        #endregion


        public void Rollback(Codes.STATE a_state)
        {
            TW_PENDINGXFERS twpendingxfers = default;
            TW_USERINTERFACE twuserinterface = default;
            TW_IDENTITY twidentity = default;

            // Make sure we have something to work with...
            if (_Twain == null)
            {
                return;
            }

            // Walk the states, we don't care about the status returns.  Basically,
            // these need to work, or we're guaranteed to hang...

            // 7 --> 6
            if (_Twain.GetState() == STATE.S7 && a_state < STATE.S7)// transferring image or transfer done
            {
                _Twain.DatPendingxfers(DataGroups.CONTROL, Messages.ENDXFER, ref twpendingxfers);
            }

            // 6 --> 5
            if (_Twain.GetState() == STATE.S6 && a_state < STATE.S6)// ready to start transferring image
            {
                _Twain.DatPendingxfers(DataGroups.CONTROL, Messages.RESET, ref twpendingxfers);
            }

            // 5 --> 4
            if (_Twain.GetState() == STATE.S5 && a_state < STATE.S5)// GUI up or waiting to transfer first image
            {
                //OnScannerPaused(EventArgs.Empty);
                _Twain.DatUserinterface(DataGroups.CONTROL, Messages.DISABLEDS, ref twuserinterface);
            }

            // 4 --> 3
            if (_Twain.GetState() == STATE.S4 && a_state < STATE.S4)// Data Source open, programmatic mode (no GUI)
            {
                TWAINSDK.CsvToIdentity(ref twidentity, _Twain.GetDsIdentity());
                _Twain.DatIdentity(DataGroups.CONTROL, Messages.CLOSEDS, ref twidentity);
            }

            // 3 --> 2
            if (_Twain.GetState() == STATE.S3 && a_state < STATE.S3)// DSM open
            {
                _Twain.DatParent(DataGroups.CONTROL, Messages.CLOSEDSM, ref m_intptrHwnd);
            }
        }

        public void ClearEvents()
        {
            m_blXferReadySent = false;
            m_blDisableDsSent = false;
        }

        public bool IsTwainInitialized()
        {
            return _Twain != null;
        }

        public ObservableCollection<string> GetScannerList()
        {
            return scannerList;
        }

        public string GetDefaultScanner()
        {
            string defaultScanner = string.Empty;
            return defaultScanner;
        }
    }

}
