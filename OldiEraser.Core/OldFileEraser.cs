using OldiEraser.Common;
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
        private readonly IFolderSettingsFileReader settingsReader;
        private readonly IDateTimeProvider dateTimeProvider;

        public OldFileEraser(IFolderSettingsFileReader settingsReader, IDateTimeProvider dateTimeProvider)
        {
            this.settingsReader = settingsReader;
            this.dateTimeProvider = dateTimeProvider;
        }

        public void EraseOldFiles(string directoryPath)
        {
            FolderSettings settings = settingsReader.ReadFile(Path.Combine(directoryPath, Global.settingFileName));
            if (settings == null)
                return;

            EraseOldFiles(directoryPath, settings);
        }

        protected void EraseOldFiles(string directoryPath, FolderSettings settings)
        {
            FolderSettings newSettings = settingsReader.ReadFile(Path.Combine(directoryPath, Global.settingFileName));

            if (newSettings != null)
                settings = newSettings;


            string[] files = Directory.GetFiles(directoryPath, "*", SearchOption.TopDirectoryOnly);
            foreach(var file in files)
            {
                DateTime modificationTime = File.GetLastWriteTime(file);

                if(settings.IsOldEnoughToRemove(dateTimeProvider.Now, modificationTime))
                {
                    File.Delete(file);
                }
            }

            if(settings.DirectoriesDeleteBehaviour != DirectoriesDeleteBehaviour.DoNothing)
            {
                string[] directories = Directory.GetDirectories(directoryPath);
                foreach(var directory in directories)
                {
                    if(settings.DirectoriesDeleteBehaviour == DirectoriesDeleteBehaviour.DeleteFilesInside)
                    {
                        EraseOldFiles(directory, settings);
                    }
                    else
                    {
                        DateTime modificationTime = Directory.GetLastWriteTime(directory);
                        if (settings.IsOldEnoughToRemove(dateTimeProvider.Now, modificationTime))
                            Directory.Delete(directory, recursive: true);
                    }
                }
            }
        }
    }
}
