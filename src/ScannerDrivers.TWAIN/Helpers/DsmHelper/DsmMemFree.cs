using ScannerDrivers.TWAIN.Contants.Enum;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Free memory used with the data source...
        /// </summary>
        /// <param name="a_intptrHandle">Pointer to free</param>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust", Unrestricted = false)]
        public void DsmMemFree(ref IntPtr a_intptrHandle, bool a_blForcePointer = false)
        {
            // Validate...
            if (a_intptrHandle == IntPtr.Zero)
            {
                return;
            }

            // Use the DSM...
            if ((m_twentrypointdelegates.DSM_MemAllocate != null) && !a_blForcePointer)
            {
                m_twentrypointdelegates.DSM_MemFree(a_intptrHandle);
            }

            // Do it ourselves, Windows...
            else if (ms_platform == Platform.WINDOWS)
            {
                NativeMethods.GlobalFree(a_intptrHandle);
            }

            // Do it ourselves, Linux...
            else if (ms_platform == Platform.LINUX)
            {
                Marshal.FreeHGlobal(a_intptrHandle);
            }

            // Do it ourselves, Mac OS X...
            else if (ms_platform == Platform.MACOSX)
            {
                // Free the indirect pointer...
                IntPtr intptr = (IntPtr)Marshal.PtrToStructure(a_intptrHandle, typeof(IntPtr));
                if (intptr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(intptr);
                }

                // If we free the direct pointer the CLR tells us that we're
                // freeing something that was never allocated.  We're going
                // to believe it and not do a free.  But I'm also leaving this
                // here as a record of the decision...
                //Marshal.FreeHGlobal(a_twcapability.hContainer);
            }

            // Make sure the variable is cleared...
            a_intptrHandle = IntPtr.Zero;
        }
    }
}
