using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;
using ScannerDrivers.TWAIN.Contants.Structure;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.CaptureExample.Services
{
    public partial class TwainService
    {
        private async Task InitStartScanningSettings()
        {
            LastFileNumber = 1;
        }
        #region startScanning

        public async Task<bool> StartScanning()
        {
            if (_Twain == null)
            {
                throw new InvalidOperationException("TwainService is not initialized.");
            }


            try
            {
                // Start the scanning process asynchronously
                STS result = await StartScanAsync();

                // Check the result of the scanning process
                if (result != Codes.STS.SUCCESS)
                {
                    // Handle the case where scanning failed
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions that occurred during scanning
                // Optionally, log or notify about the exception
                Console.WriteLine($"An error occurred while starting scanning: {ex.Message}");
                return false;
            }

            return true;
        }

        public async Task<STS> StartScanAsync()
        {
            try
            {
                await InitStartScanningSettings();
                // Prepare TWAIN user interface settings
                //string twainUserInterfaceSettings = $"{true},{true},{windowHandle}";
                var twainUserInterfaceSettings = false + "," + false + "," + windowHandle;
                // Clear previous events
                ClearEvents();

                // Configure TWAIN user interface
                TW_USERINTERFACE twUserInterface = default(TW_USERINTERFACE);
                _Twain.CsvToUserinterface(ref twUserInterface, twainUserInterfaceSettings);

                // Execute TWAIN operation asynchronously
                Codes.STS result = await Task.Run(() => _Twain.DatUserinterface(DataGroups.CONTROL, Messages.ENABLEDS, ref twUserInterface));

                // Return the result of the operation
                return result;
            }
            catch (Exception ex)
            {
                // Log and handle exceptions
                Console.WriteLine($"An error occurred while starting the scanning process: {ex.Message}");
                return Codes.STS.FAILURE;
            }
        }

        #endregion

        #region stopScanning
        public async Task<bool> StopScanning()
        {
            if (_Twain == null)
            {
                throw new InvalidOperationException("TwainService is not initialized.");
            }
            await StopScanAsync();

            return true; // Indicate that scanning has been stopped
        }

        private async Task StopScanAsync()
        {
            TW_PENDINGXFERS twpendingxfers = default(TW_PENDINGXFERS);
            _Twain.DatPendingxfers(DataGroups.CONTROL, Messages.STOPFEEDER, ref twpendingxfers);
            return;
        }

        #endregion

        #region pauseScanning
        public async Task<bool> PauseScanning()
        {
            if (_Twain == null)
            {
                throw new InvalidOperationException("TwainService is not initialized.");
            }

            await PauseScanAsync();

            return true; // Indicate that scanning has been paused
        }

        private Task PauseScanAsync()
        {
            // Implement the logic to pause scanning asynchronously here
            throw new NotImplementedException();
        }
        #endregion
        public delegate void ScanCallbackEvent();

        private Codes.STS ScanCallbackTrigger(bool a_blClosing)
        {
            uiDispatcher.BeginInvoke((Action)(delegate
            {
                ScanCallbackEventHandler(this, new System.EventArgs());
            }));

            return Codes.STS.SUCCESS;
        }
        private void ScanCallbackEventHandler(object sender, System.EventArgs e)
        {
            ScanCallback((_Twain == null) ? true : (_Twain.GetState() <= Codes.STATE.S3)); // DSM open
        }
        public Codes.STS ScanCallback(bool a_blClosing)
        {
            //Stopwatch stopwatch = Stopwatch.StartNew();
            // count++;
            Codes.STS sts;

            // Scoot...
            if (_Twain == null)
            {
                return Codes.STS.FAILURE;
            }

            //bool scannerPausedNow = (_Twain.GetState() <= Codes.STATE.S4) || a_blClosing;
            //// Trigger OnScannerPaused event if the scanner was not previously paused
            //if (scannerPausedNow && !scannerPaused)
            //{
            //    OnScannerPaused(EventArgs.Empty);
            //}

            //// Trigger OnScannerContinued event if the scanner was paused and is now resumed
            //if (!scannerPausedNow && scannerPaused)
            //{
            //    OnScannerContinued(EventArgs.Empty);
            //}

            //scannerPaused = scannerPausedNow; // Update the paused state
            // We're superfluous...
            if (_Twain.GetState() <= STATE.S4)
            {
                return Codes.STS.SUCCESS;
            }

            // We're leaving...
            if (a_blClosing)
            {
                return Codes.STS.SUCCESS;
            }

            // Do this in the right thread, we'll usually be in the
            // right spot, save maybe on the first call...
            if (!uiDispatcher.CheckAccess())
            {
                return (Codes.STS)Dispatcher.CurrentDispatcher.Invoke((Func<Codes.STS>)(() => ScanCallback(a_blClosing)));
            }


            // Handle DAT_NULL/MSG_XFERREADY...
            if (_Twain.IsMsgXferReady() && !m_blXferReadySent)
            {
                m_blXferReadySent = true;

                // Get the amount of memory needed...
                m_twsetupmemxfer = default(TW_SETUPMEMXFER);
                sts = _Twain.DatSetupmemxfer(DataGroups.CONTROL, Messages.GET, ref m_twsetupmemxfer);
                if (sts != Codes.STS.SUCCESS || m_twsetupmemxfer.Preferred == 0)
                {
                    m_blXferReadySent = false;
                    if (!m_blDisableDsSent)
                    {
                        m_blDisableDsSent = true;
                        Rollback(STATE.S4); // Data Source open, programmatic mode (no GUI)
                    }
                }

                // Allocate the transfer memory (with a little extra to protect ourselves)...
                m_intptrXfer = Marshal.AllocHGlobal((int)m_twsetupmemxfer.Preferred + 65536);
                if (m_intptrXfer == IntPtr.Zero)
                {
                    m_blDisableDsSent = true;
                    Rollback(STATE.S4); // Data Source open, programmatic mode (no GUI)
                }
                OnScanPaused();
            }

            // Handle DAT_NULL/MSG_CLOSEDSREQ...
            if (_Twain.IsMsgCloseDsReq() && !m_blDisableDsSent)
            {
                m_blDisableDsSent = true;
                Rollback(STATE.S4); // Data Source open, programmatic mode (no GUI)
            }

            // Handle DAT_NULL/MSG_CLOSEDSOK...
            if (_Twain.IsMsgCloseDsOk() && !m_blDisableDsSent)
            {
                m_blDisableDsSent = true;
                Rollback(STATE.S4); // Data Source open, programmatic mode (no GUI)
                //OnScannerContinued(EventArgs.Empty);
            }

            // This is where the state machine transfers and optionally
            // saves the images to disk (it also displays them). It'll go back
            // and forth between states 6 and 7 until an error occurs, or until
            // we run out of images...
            if (m_blXferReadySent && !m_blDisableDsSent)
            {

                CaptureImages();
            }
            TW_CAPABILITY capability = new TW_CAPABILITY();
            capability.Cap = Capabilities.CAP_XFERCOUNT; // This is just an example capability, replace it with the appropriate one for your scanner
            if (_Twain.DatCapability(DataGroups.CONTROL, Messages.GET, ref capability) == Codes.STS.SUCCESS)
            {
                //remainingImages = capability.ConType == TWON.ONEVALUE ? capability.OneValue : 0;
            }

            if (!m_blXferReadySent)
            {
                // Close the connection
                m_blDisableDsSent = true;
                Rollback(STATE.S4); // Data Source open, programmatic mode (no GUI)
                //IsScanning = false;
            }

            ProcessEvents();


            uiDispatcher.BeginInvoke((Action)(delegate
            {
                ScanCallbackEventHandler(this, new System.EventArgs());
            }));

            //stopwatch.Stop();
            //long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            return Codes.STS.SUCCESS;
        }

    }
}
