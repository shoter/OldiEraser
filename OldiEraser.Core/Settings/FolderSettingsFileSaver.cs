using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Core.Settings
{
    public class FolderSettingsFileSaver : IFolderSettingsFileSaver
    {
        public void Save(in FolderSettings folderSettings, string directoryPath)
        {
            string filePath = Path.Combine(directoryPath, Global.settingFileName);
            var json = JsonConvert.SerializeObject(folderSettings, Formatting.Indented);
            File.WriteAllText(filePath, json);
            File.SetAttributes(filePath, File.GetAttributes(filePath) | FileAttributes.Hidden);
        }
    }
}
