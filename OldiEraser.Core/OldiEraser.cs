using Newtonsoft.Json;
using OldiEraser.Common;
using OldiEraser.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OldiEraser.Common.Threading;
using OldiEraser.Core.Settings;
using OldiEraser.Common.IO;

namespace OldiEraser.Core
{
    public class OldiEraser : Disposable 
    {
        private readonly IOldieConfiguration configuration;
        private readonly IDateTimeProvider timeProvider;
        public AppData Data { get; private set;}

        public OldiEraser(IOldieConfiguration configuration, IDateTimeProvider timeProvider)
        {
            this.configuration = configuration;
            this.timeProvider = timeProvider;
            Data = LoadAppData();
            Remove();
        }

        public void Remove()
        {
            var eraser = new OldFileEraser(new Scanning.OldiScanner(new FolderSettingsFileReader(), timeProvider));
            foreach(var s in Data.FolderSettings)
            {
                eraser.EraseOldFiles(s.DirectoryPath);
            }
            Data.LastEraseTime = timeProvider.Now;
        }

        public void AddSettings(SpecificFolderSetting sett)
        {
            Data.FolderSettings.Add(sett);
        }

        public void RemoveSetting(string directoryPath)
        {
            SpecificFolderSetting sett = GetSetting(directoryPath);
            Data.FolderSettings.Remove(sett);
        }

        private SpecificFolderSetting GetSetting(string directoryPath)
        {
            directoryPath = PathUtil.NormalizePath(directoryPath);
            return Data.FolderSettings.FirstOrDefault(s => PathUtil.NormalizePath(s.DirectoryPath) == directoryPath);
        }

        public void UpdateSetting(string directoryPath, FolderSettings newSett)
        {
            var sett = GetSetting(directoryPath);
            if(sett != null)
            {
                sett.DayAgeToRemove = newSett.DayAgeToRemove;
                sett.DirectoriesDeleteBehaviour = newSett.DirectoriesDeleteBehaviour;

                var saver = new FolderSettingsFileSaver();
                saver.SaveAsync(newSett, directoryPath).Wait();
            }
        }


        private AppData LoadAppData()
        {
            string dataFile = configuration.DataFilePath;
            var settingsReader = new FolderSettingsFileReader();

            if (!File.Exists(dataFile))
                return AppData.Default;
            try
            {
                AppData data;
                using (var reader = new StreamReader(dataFile))
                {
                    var json = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject<AppData>(json);
                }

                data.FolderSettings = data.FolderSettings ?? new List<SpecificFolderSetting>();

                data.FolderSettings
                    .WaitAll(x => x.LoadSettingsFromFile(settingsReader));

                data.FolderSettings = data.FolderSettings
                    .Where(f => f.Status != SpecificFolderSettingStatus.Broken)
                    .ToList();

                return data;
            }
            catch(IOException)
            {
                // if something does not load right then file is broken. 
                // We will use default file in that situation
                return AppData.Default;
            }
        }

        protected override void FreeManagedObjects()
        {
            if(Data.EnableAutosave)
                SaveAppData();
        }

        private void SaveAppData()
        {
            try
            {
                string dataFile = configuration.DataFilePath;
                using (var writer = new StreamWriter(dataFile))
                {
                    var json = JsonConvert.SerializeObject(Data, Formatting.Indented);
                    writer.Write(json);
                }
            }
            catch (IOException e)
            {
                throw new Exception("Could not save data!", e);
            }
        }
    }
}
