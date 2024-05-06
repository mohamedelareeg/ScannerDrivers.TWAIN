using System.Diagnostics.CodeAnalysis;
using static ScannerDrivers.TWAIN.TWAINSDK;

namespace ScannerDrivers.TWAIN.Contants.Structure
{
    public struct TW_ENTRYPOINT_DELEGATES
    {
        public uint Size;
        [SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public nint DSM_Entry;
        public DSM_MEMALLOC DSM_MemAllocate;
        public DSM_MEMFREE DSM_MemFree;
        public DSM_MEMLOCK DSM_MemLock;
        public DSM_MEMUNLOCK DSM_MemUnlock;
    }
}
