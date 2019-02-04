using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OldiEraser.Core.Scanning
{
    public interface IOldiScanner
    {
        ShortScanResult ShortScan(string directoryPath);
        DetailedScanResult DetailedScan(string directoryPath);
        void Scan(string directoryPath, Action<ScanResult> scanResultAction);
        void Scan(string directoryPath, Action<ScanResult> scanResultAction, CancellationTokenSource token);
    }
}
