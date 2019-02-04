using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Core.Scanning
{
    public class ShortScanResult
    {
        public string ScannedPath { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int NumberOfFilesToDelete { get; set; } = 0;
        public int NumberOfDirectoriesToDelete { get; set; } = 0;
        public int NumberOfDirectoriesThatWillSoonBeDeleted { get; set; } = 0;
        public int NumberOfFilesThatWillSoonBeDeleted { get; set; } = 0;

        public ShortScanResult(string directoryPath, DateTime startTime)
        {
            this.StartTime = startTime;
            this.ScannedPath = directoryPath;
        }
            
    }
}
