using OldiEraser.Common;
using OldiEraser.Core.Scanning;
using OldiEraser.Core.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Core
{
    public class OldFileEraser : IOldFileEraser
    {
        private readonly IOldiScanner scanner;

        public OldFileEraser(IOldiScanner scanner)
        {
            this.scanner = scanner;
        }

        public void EraseOldFiles(string directoryPath)
        {
            scanner.Scan(directoryPath, r =>
            {
                if (r.Status != DetailedScanItemStatus.Deletion)
                    return;

                switch(r.ItemType)
                {
                    case ItemType.Directory:
                        Directory.Delete(r.Path, recursive: true);
                        return;
                    case ItemType.File:
                        File.Delete(r.Path);
                        return;
                }
            });
        }
    }
}
