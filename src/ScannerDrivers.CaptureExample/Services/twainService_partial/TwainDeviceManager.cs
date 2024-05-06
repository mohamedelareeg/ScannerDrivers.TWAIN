using ScannerDrivers.TWAIN;
using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;
using ScannerDrivers.TWAIN.Contants.Structure;
using System.Collections.ObjectModel;
using System.Windows;

namespace ScannerDrivers.CaptureExample.Services
{
    public partial class TwainService
    {
        public void ShowScannerProfile()
        {
            TW_USERINTERFACE twUserInterface = default(TW_USERINTERFACE);
            var szTwmemref = true + "," + false + "," + windowHandle;
            // Convert CSV string to TW_USERINTERFACE directly
            if (!_Twain.CsvToUserinterface(ref twUserInterface, szTwmemref))
            {
                // Handle error
                return;
            }


            // Enable the user interface without starting scanning
            EnableUserInterface(twUserInterface);
        }

        private void EnableUserInterface(TW_USERINTERFACE twUserInterface)
        {
            try
            {
                // Send the command...
                ClearEvents();

                // Enable user interface
                Codes.STS sts = _Twain.DatUserinterface(DataGroups.CONTROL, Messages.ENABLEDS, ref twUserInterface);

                if (sts != Codes.STS.SUCCESS)
                {
                    // Handle the case where enabling the user interface failed
                    Console.WriteLine("Scanner is not Connected");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #region selectScanner
        public async Task<bool> SelectScanner(string currentScanner)
        {
            if (_Twain == null)
            {
                throw new InvalidOperationException("TwainService is not initialized.");
            }

            if (string.IsNullOrEmpty(currentScanner))
            {
                return false;
            }

            try
            {
                await SelectScannerAsync(currentScanner);
                return true;
            }
            catch (Exception ex)
            {
                // Handle exceptions if necessary
                Console.WriteLine($"An error occurred while selecting the scanner: {ex.Message}");
                return false; // Return false if an exception occurs during the selection process
            }
        }

        private async Task<bool> SelectScannerAsync(string currentScanner)
        {
            string szIdentity;
            string szDefault = "";
            string szStatus;
            List<string> lszIdentity = new List<string>();

            Codes.STS sts;
            TW_CAPABILITY twcapability;
            TW_IDENTITY twidentity = default(TW_IDENTITY);

            // Get the default driver...
            sts = _Twain.DatIdentity(DataGroups.CONTROL, Messages.GETDEFAULT, ref twidentity);
            if (sts == Codes.STS.SUCCESS)
            {
                szDefault = TWAINSDK.IdentityToCsv(twidentity);
            }

            // Enumerate the drivers...
            for (sts = _Twain.DatIdentity(DataGroups.CONTROL, Messages.GETFIRST, ref twidentity);
                 sts != Codes.STS.ENDOFLIST;
                 sts = _Twain.DatIdentity(DataGroups.CONTROL, Messages.GETNEXT, ref twidentity))
            {
                lszIdentity.Add(TWAINSDK.IdentityToCsv(twidentity));
            }

            // Ruh-roh...
            if (lszIdentity.Count == 0)
            {
                MessageBox.Show("There are no TWAIN drivers installed on this system...");
                return false;
            }

            // Get all the identities...
            szIdentity = currentScanner;
            if (szIdentity == null)
            {
                m_blExit = true;
                return false;
            }

            // Get the selected identity...
            m_blExit = true;
            foreach (string sz in lszIdentity)
            {
                if (sz.Contains(szIdentity))
                {
                    m_blExit = false;
                    szIdentity = sz;
                    break;
                }
            }
            if (m_blExit)
            {
                return false;
            }

            // Make it the default, we don't care if this succeeds...
            twidentity = default(TW_IDENTITY);
            TWAINSDK.CsvToIdentity(ref twidentity, szIdentity);
            _Twain.DatIdentity(DataGroups.CONTROL, Messages.SET, ref twidentity);

            // Open it...
            sts = _Twain.DatIdentity(DataGroups.CONTROL, Messages.OPENDS, ref twidentity);
            if (sts != Codes.STS.SUCCESS)
            {
                MessageBox.Show("Unable to open scanner (it is turned on and plugged in?)");
                m_blExit = true;
                return false;
            }

            // We're doing memory transfers...
            szStatus = "";
            twcapability = default(TW_CAPABILITY);
            _Twain.CsvToCapability(ref twcapability, ref szStatus, "ICAP_XFERMECH,TWON_ONEVALUE,TWTY_UINT16,TWSX_MEMORY");
            sts = _Twain.DatCapability(DataGroups.CONTROL, Messages.SET, ref twcapability);
            if (sts != Codes.STS.SUCCESS)
            {
                m_blExit = true;
                return false;
            }

            // Decide whether or not to show the driver's window messages...
            szStatus = "";
            twcapability = default(TW_CAPABILITY);
            _Twain.CsvToCapability(ref twcapability, ref szStatus, "CAP_INDICATORS,TWON_ONEVALUE,TWTY_BOOL," + (m_blIndicators ? "TRUE" : "FALSE"));
            sts = _Twain.DatCapability(DataGroups.CONTROL, Messages.SET, ref twcapability);
            if (sts != Codes.STS.SUCCESS)
            {
                m_blExit = true;
                return false;
            }

            return true; // Return true if all operations succeed
        }
        #endregion
        private void initSelection()
        {
            string szDefault = "";
            List<string> lszIdentity = new List<string>();
            Codes.STS sts;
            TW_IDENTITY twidentity = default(TW_IDENTITY);

            // Get the default driver...
            m_intptrHwnd = windowHandle;
            sts = _Twain.DatParent(DataGroups.CONTROL, Messages.OPENDSM, ref m_intptrHwnd);
            if (sts != Codes.STS.SUCCESS)
            {
                // MessageBox.Show("OPENDSM failed...");
                return;
            }

            // Get the default driver...
            sts = _Twain.DatIdentity(DataGroups.CONTROL, Messages.GETDEFAULT, ref twidentity);
            if (sts == Codes.STS.SUCCESS)
            {
                szDefault = TWAINSDK.IdentityToCsv(twidentity);
            }

            // Enumerate the drivers...
            for (sts = _Twain.DatIdentity(DataGroups.CONTROL, Messages.GETFIRST, ref twidentity);
                 sts != Codes.STS.ENDOFLIST;
                 sts = _Twain.DatIdentity(DataGroups.CONTROL, Messages.GETNEXT, ref twidentity))
            {
                lszIdentity.Add(TWAINSDK.IdentityToCsv(twidentity));
            }

            // Ruh-roh...
            if (lszIdentity.Count == 0)
            {
                MessageBox.Show("There are no TWAIN drivers installed on this system...");
                return;
            }

            scannerList = new ObservableCollection<string>();
            foreach (string sz in lszIdentity)
            {
                string[] aszIdentity = CSV.Parse(sz);
                scannerList.Add(aszIdentity[11]);
            }
        }
        private Codes.STS DeviceEventCallback()
        {
            Codes.STS sts;
            TW_DEVICEEVENT twdeviceevent;

            // Drain the event queue...
            while (true)
            {
                // Try to get an event...
                twdeviceevent = default(TW_DEVICEEVENT);
                sts = _Twain.DatDeviceevent(DataGroups.CONTROL, Messages.GET, ref twdeviceevent);
                if (sts != Codes.STS.SUCCESS)
                {
                    break;
                }
            }

            // Return a status, in case we ever need it for anything...
            return Codes.STS.SUCCESS;
        }

    }
}
