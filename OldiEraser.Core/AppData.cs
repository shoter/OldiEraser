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
        public AppData Default => new AppData()
        {
            FolderSettings = new List<FolderSettings>()
        };

        public List<FolderSettings> FolderSettings { get; set;}


    }
}
