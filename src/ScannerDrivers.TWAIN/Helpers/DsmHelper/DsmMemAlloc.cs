using ScannerDrivers.TWAIN.Contants.Enum;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Alloc memory used with the data source.
        /// </summary>
        /// <param name="a_u32Size">Number of bytes to allocate</param>
        /// <returns>Point to memory</returns>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust", Unrestricted = false)]
        public IntPtr DsmMemAlloc(uint a_u32Size, bool a_blForcePointer = false)
        {
            IntPtr intptr;

            // Use the DSM...
            if ((m_twentrypointdelegates.DSM_MemAllocate != null) && !a_blForcePointer)
            {
                intptr = m_twentrypointdelegates.DSM_MemAllocate(a_u32Size);
                if (intptr == IntPtr.Zero)
                {
                    Log.Error("DSM_MemAllocate failed...");
                }
                return (intptr);
            }

            // Do it ourselves, Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                intptr = (IntPtr)NativeMethods.GlobalAlloc((uint)(a_blForcePointer ? 0x0040 /* GPTR */ : 0x0042 /* GHND */), (UIntPtr)a_u32Size);
                if (intptr == IntPtr.Zero)
                {
                    Log.Error("GlobalAlloc failed...");
                }
                return (intptr);
            }

            // Do it ourselves, Linux...
            if (ms_platform == Platform.LINUX)
            {
                intptr = Marshal.AllocHGlobal((int)a_u32Size);
                if (intptr == IntPtr.Zero)
                {
                    Log.Error("AllocHGlobal failed...");
                }
                return (intptr);
            }

            // Do it ourselves, Mac OS X...
            if (ms_platform == Platform.MACOSX)
            {
                IntPtr intptrIndirect = Marshal.AllocHGlobal((int)a_u32Size);
                if (intptrIndirect == IntPtr.Zero)
                {
                    Log.Error("AllocHGlobal(indirect) failed...");
                    return (intptrIndirect);
                }
                IntPtr intptrDirect = Marshal.AllocHGlobal(Marshal.SizeOf(intptrIndirect));
                if (intptrDirect == IntPtr.Zero)
                {
                    Log.Error("AllocHGlobal(direct) failed...");
                    return (intptrDirect);
                }
                Marshal.StructureToPtr(intptrIndirect, intptrDirect, true);
                return (intptrDirect);
            }

            // Trouble, Log will throw an exception for us...
            Log.Assert("Unsupported platform..." + ms_platform);
            return (IntPtr.Zero);
        }
    }
}
