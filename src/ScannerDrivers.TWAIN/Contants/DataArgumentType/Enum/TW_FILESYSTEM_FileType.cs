using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerDrivers.TWAIN.Contants.DataArgumentType.Enum
{
    /// <summary>
    /// TW_FILESYSTEM.FileType values
    /// </summary>
    public enum TW_FILESYSTEM_FileType : ushort
    {
        CAMERA = 0,
        CAMERATOP = 1,
        CAMERABOTTOM = 2,
        CAMERAPREVIEW = 3,
        DOMAIN = 4,
        HOST = 5,
        DIRECTORY = 6,
        IMAGE = 7,
        UNKNOWN = 8
    }
}
