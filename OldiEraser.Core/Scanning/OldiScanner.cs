using OldiEraser.Common;
using OldiEraser.Core.Scanning;
using OldiEraser.Core.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OldiEraser.Core.Scanning
{
    public class OldiScanner : IOldiScanner
    {
        private readonly IFolderSettingsFileReader settingsReader;
        private readonly OldiScannerSettings scannerSettings;
        private readonly IDateTimeProvider timeProvider;

        public OldiScanner(IFolderSettingsFileReader reader, IDateTimeProvider timeProvider, OldiScannerSettings settings)
        {
            this.settingsReader = reader;
            this.scannerSettings = settings;
            this.timeProvider = timeProvider;
        }

        public OldiScanner(IFolderSettingsFileReader reader, IDateTimeProvider timeProvider)
            : this(reader, timeProvider, OldiScannerSettings.Default)
        { }


        public DetailedScanResult DetailedScan(string directoryPath)
        {
            DetailedScanResult result = new DetailedScanResult(directoryPath, timeProvider.Now);

            Scan(directoryPath, r =>
            {
                ShortScanUpdateResult(r, result);
                result.Items.Add(new DetailedScanItemResult(r));
            });

            result.EndTime = timeProvider.Now;
            return result;
        }

        public void Scan(string directoryPath, Action<ScanResult> scanResultAction)
            => Scan(directoryPath, scanResultAction, null);

        public void Scan(string directoryPath, Action<ScanResult> scanResultAction, CancellationTokenSource token)
        {
            FolderSettings settings = settingsReader.ReadFileAsync(directoryPath).Result;
            if (settings == null)
                return;

            Scan(directoryPath, settings, scanResultAction, null);
        }

        public ShortScanResult ShortScan(string directoryPath)
        {
            ShortScanResult result = new ShortScanResult(directoryPath, timeProvider.Now);

            Scan(directoryPath, r =>
            {
                ShortScanUpdateResult(r, result);
            });

            result.EndTime = timeProvider.Now;
            return result;
        }

        private static void ShortScanUpdateResult(ScanResult r, ShortScanResult result)
        {
            switch (r.Status)
            {
                case DetailedScanItemStatus.Deletion when r.ItemType == ItemType.File:
                    result.NumberOfFilesToDelete++;
                    break;
                case DetailedScanItemStatus.Warning when r.ItemType == ItemType.File:
                    result.NumberOfFilesThatWillSoonBeDeleted++;
                    break;

                case DetailedScanItemStatus.Deletion when r.ItemType == ItemType.Directory:
                    result.NumberOfDirectoriesToDelete++;
                    break;
                case DetailedScanItemStatus.Warning when r.ItemType == ItemType.Directory:
                    result.NumberOfDirectoriesThatWillSoonBeDeleted++;
                    break;
            }
        }

        protected void Scan(string directoryPath, FolderSettings settings, Action<ScanResult> scanAction, CancellationTokenSource token)
        {
            FolderSettings newSettings = settingsReader.ReadFileAsync(Path.Combine(directoryPath, Global.settingFileName)).Result;

            if (newSettings != null)
                settings = newSettings;


            string[] files = Directory.GetFiles(directoryPath);
            foreach (var file in files)
            {
                if (file.EndsWith(Global.settingFileName))
                    continue;

                if (token?.IsCancellationRequested ?? false)
                    return;

                DateTime modificationTime = File.GetLastWriteTime(file);
                DateTime creationTime = File.GetCreationTime(file);
                DateTime lastTime = getLastModificationTime(modificationTime, creationTime);

                scanAction(new ScanResult()
                {
                    ModifyDate = modificationTime,
                    Path = file,
                    DaysToLive = settings.DayAgeToRemove - GetAge(lastTime, timeProvider.Now),
                    ItemType = ItemType.File,
                    Status = DetermineScanItemStatus(settings, lastTime)
                });

            }


            if (settings.DirectoriesDeleteBehaviour != DirectoriesDeleteBehaviour.DoNothing)
            {
                string[] directories = Directory.GetDirectories(directoryPath);
                foreach (var directory in directories)
                {
                    if (token?.IsCancellationRequested ?? false)
                        return;

                    if (settings.DirectoriesDeleteBehaviour == DirectoriesDeleteBehaviour.DeleteFilesInside)
                    {
                        Scan(directory, settings, scanAction, token);
                    }
                    else
                    {
                        DateTime modificationTime = Directory.GetLastWriteTime(directory);
                        DateTime creationTime = Directory.GetCreationTime(directory);
                        DateTime lastTime = getLastModificationTime(modificationTime, creationTime);
                        scanAction(new ScanResult()
                        {
                            ModifyDate = modificationTime,
                            Path = directory,
                            DaysToLive = settings.DayAgeToRemove - GetAge(lastTime, timeProvider.Now),
                            ItemType = ItemType.Directory,
                            Status = DetermineScanItemStatus(settings, lastTime)
                        });
                    }
                }
            }
        }

        private DateTime getLastModificationTime(DateTime modificationTime, DateTime creationTime)
        {
            if (creationTime > modificationTime)
                return creationTime;
            return modificationTime;
        }

        private DetailedScanItemStatus DetermineScanItemStatus(FolderSettings settings, DateTime lastModificationTime)
        {
            if (IsOldEnough(lastModificationTime, timeProvider.Now, settings))
            {
                return DetailedScanItemStatus.Deletion;
            }
            else if (IsOldEnough(lastModificationTime, timeProvider.Now, Math.Max(0, settings.DayAgeToRemove - scannerSettings.WarningDayLiveThreshold)))
            {
                return DetailedScanItemStatus.Warning;
            }
            else
            {
                return DetailedScanItemStatus.DoNotDelete;
            }
        }

        private static bool IsOldEnough(DateTime lastModificationTime, DateTime currentTime, FolderSettings settings)
            => IsOldEnough(lastModificationTime, currentTime, settings.DayAgeToRemove);

        private static bool IsOldEnough(DateTime lastModificationTime, DateTime currentTime, uint dayToLive)
            => GetAge(lastModificationTime, currentTime) >= dayToLive;

        private static int GetAge(DateTime lastModificationTime, DateTime currentTime)
            => (currentTime - lastModificationTime).Days;
    }
}
