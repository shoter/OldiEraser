using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Core.Settings
{
    public class FolderSettings
    {
        public uint DayAgeToRemove { get; private set; }
        public DirectoriesDeleteBehaviour DirectoriesDeleteBehaviour { get; private set; }

        public FolderSettings(uint dayAgeToRemove, DirectoriesDeleteBehaviour directoriesDeleteBehaviour)
        {
            this.DayAgeToRemove = dayAgeToRemove;
            this.DirectoriesDeleteBehaviour = directoriesDeleteBehaviour;
        }

        public bool IsOldEnoughToRemove(DateTime now, DateTime date)
        {
            var difference = now - date;

            return difference.TotalDays >= DayAgeToRemove;
        }
    }
}
