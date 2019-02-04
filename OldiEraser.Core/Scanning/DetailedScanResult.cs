using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Core.Scanning
{
    public class DetailedScanResult : ShortScanResult
    {
        public List<DetailedScanItemResult> Items { get; internal set; } = new List<DetailedScanItemResult>();

        public DetailedScanResult(string directoryPath, DateTime startTime)
            :base(directoryPath, startTime)
        {

        }
    }
}
