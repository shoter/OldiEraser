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
        public async Task SaveAsync(FolderSettings folderSettings, string directoryPath)
        {
            string filePath = Path.Combine(directoryPath, Global.settingFileName);
            var json = JsonConvert.SerializeObject(folderSettings, Formatting.Indented);
            if(File.Exists(filePath))
                File.SetAttributes(filePath, (~File.GetAttributes(filePath)) & FileAttributes.Hidden);

            using (var writer = new StreamWriter(filePath, append: false))
            {
                await writer.WriteAsync(json);
            }
            File.SetAttributes(filePath, File.GetAttributes(filePath) | FileAttributes.Hidden);
        }
    }
}
