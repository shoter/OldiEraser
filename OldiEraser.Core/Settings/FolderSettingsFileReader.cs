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
        public virtual async Task<FolderSettings> ReadFileAsync(string directoryPath)
        {
            string filePath = Path.Combine(directoryPath, Global.settingFileName);

            if (File.Exists(filePath) == false)
                return null;

            using (var reader = new StreamReader(filePath))
            {
                string json = await reader.ReadToEndAsync().ConfigureAwait(continueOnCapturedContext: false);
                return JsonConvert.DeserializeObject<FolderSettings>(json);
            }
        }
    }
}
