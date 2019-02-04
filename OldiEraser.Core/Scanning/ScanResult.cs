using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Core.Scanning
{
    public class ScanResult
    {
        public ItemType ItemType { get; internal set; }
        public DetailedScanItemStatus Status { get; internal set; }
        public long DaysToLive { get; internal set; }
        public string Path { get; internal set; }
        public DateTime ModifyDate { get; internal set; }

    }
}
