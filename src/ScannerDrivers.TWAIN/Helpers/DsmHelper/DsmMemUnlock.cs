using ScannerDrivers.TWAIN.Contants.Enum;
using System.Security.Permissions;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Unlock memory used with the data source...
        /// </summary>
        /// <param name="a_intptrHandle">Handle to unlock</param>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust", Unrestricted = false)]
        public void DsmMemUnlock(IntPtr a_intptrHandle)
        {
            // Validate...
            if (a_intptrHandle == IntPtr.Zero)
            {
                return;
            }

            // Use the DSM...
            if (m_twentrypointdelegates.DSM_MemUnlock != null)
            {
                m_twentrypointdelegates.DSM_MemUnlock(a_intptrHandle);
                return;
            }

            // Do it ourselves, Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                NativeMethods.GlobalUnlock(a_intptrHandle);
                return;
            }

            // Do it ourselves, Linux...
            if (ms_platform == Platform.LINUX)
            {
                return;
            }

            // Do it ourselves, Mac OS X...
            if (ms_platform == Platform.MACOSX)
            {
                return;
            }

            // Trouble, Log will throw an exception for us...
            Log.Assert("Unsupported platform..." + ms_platform);
        }
    }
}
