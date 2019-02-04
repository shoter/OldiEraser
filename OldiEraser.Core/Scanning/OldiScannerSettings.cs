using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Core.Scanning
{
    public class OldiScannerSettings
    {
        public static OldiScannerSettings Default = new OldiScannerSettings()
        {
            WarningDayLiveThreshold = 7
        };

        public uint WarningDayLiveThreshold { get; set; }
    }
}
