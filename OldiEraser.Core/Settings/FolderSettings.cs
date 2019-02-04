using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Core.Settings
{
    public class FolderSettings
    {
        // Those settings will be saved in the file in given directory. We do not want them.

        public static FolderSettings Default => new FolderSettings(30, DirectoriesDeleteBehaviour.DoNothing);

        
        public virtual uint DayAgeToRemove { get; internal set; }

        public virtual DirectoriesDeleteBehaviour DirectoriesDeleteBehaviour { get; internal set; }

        public FolderSettings(uint dayAgeToRemove, DirectoriesDeleteBehaviour directoriesDeleteBehaviour)
        {
            this.DayAgeToRemove = dayAgeToRemove;
            this.DirectoriesDeleteBehaviour = directoriesDeleteBehaviour;
        }
    }
}
