using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Core.Settings
{
    public interface IFolderSettingsFileSaver
    {
        void Save(in FolderSettings folderSettings, string filePath);
    }
}
