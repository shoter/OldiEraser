using OldiEraser.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Wpf.Models
{
    public class AddDirectoryDialogViewModel
    {
        public string Path { get; set; }
        public int DaysBeforeRemoval { get; set; }
        public int BehaviourID { get; set; }

        public List<BehaviourItemViewModel> Behaviours { get; set; }

        public AddDirectoryDialogViewModel(string dirPath)
        {
            Path = dirPath;
            DaysBeforeRemoval = 30;
            BehaviourID = (int)DirectoriesDeleteBehaviour.DoNothing;

            Behaviours = Enum.GetValues(typeof(DirectoriesDeleteBehaviour)).Cast<DirectoriesDeleteBehaviour>()
               .Select(b => new BehaviourItemViewModel()
               {
                   Id = (int)b,
                   Name = b.ToString()
               }).ToList();
        }
    }
}
