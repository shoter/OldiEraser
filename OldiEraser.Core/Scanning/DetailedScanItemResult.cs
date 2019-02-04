using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Core.Scanning
{
    public class DetailedScanItemResult
    {
        public string Path { get; private set; }
        public DateTime ModifyDate { get; private set; }
        public long DaysToRemove { get; private set; }
        public ItemType ItemType { get; private set; }
        public DetailedScanItemStatus Status { get; private set; }

        public DetailedScanItemResult(ScanResult result)
        {
            Path = result.Path;
            ModifyDate = result.ModifyDate;
            DaysToRemove = result.DaysToLive;
            ItemType = result.ItemType;
            Status = result.Status;

        }
    }
}
