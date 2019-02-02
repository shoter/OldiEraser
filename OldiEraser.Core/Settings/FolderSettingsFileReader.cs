using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Core.Settings
{
    public class FolderSettingsFileReader : IFolderSettingsFileReader
    {
        public virtual FolderSettings ReadFile(string directoryPath)
        {
            string filePath = Path.Combine(directoryPath, Global.settingFileName);

            if (File.Exists(filePath) == false)
                return null;

            return JsonConvert.DeserializeObject<FolderSettings>(File.ReadAllText(filePath));
        }
    }
}
