using OldiEraser.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Core
{
    public class AppData
    {
        public static AppData Default = new AppData();

        public List<SpecificFolderSetting> FolderSettings { get; set; } = new List<SpecificFolderSetting>();

        public bool EnableAutosave { get; set; } = true;

        public DateTime LastEraseTime { get; set; } = DateTime.Now;

        public AppData() { }

    }
}
