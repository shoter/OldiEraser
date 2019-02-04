using OldiEraser.Core.Scanning;
using OldiEraser.Wpf.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Wpf.Models
{
    public class DetailsViewModel : IntelliModel
    {
        public FolderSettingItemViewModel FolderSettingsVm { get; set; }

        public string Path { get; internal set; }
        public string WarningNumberOfFiles { get; internal set; }
        public string WarningNumberOfDirectories { get; internal set; }

        public ObservableCollection<DetailItemViewModel> Files { get; set; } = new ObservableCollection<DetailItemViewModel>();
        public ObservableCollection<DetailItemViewModel> Directories { get; set; } = new ObservableCollection<DetailItemViewModel>();


        private ObservableLinkedProperty<long> daysAge = new ObservableLinkedProperty<long>();
        public long  DaysAge
        {
            get => daysAge.Value;
            set
            {
                daysAge.Value = Math.Max(0, value);
            }
        }

        public ObservableLinkedProperty<int> BehaviourID { get; } = new ObservableLinkedProperty<int>();


        public ObservableProperty<string> DirectoriesWarnings { get; } = new ObservableProperty<string>();

        public DetailsViewModel(DetailedScanResult result, FolderSettingItemViewModel fsVM)
        {
            daysAge.LinkWith(fsVM.DaysAge);
            BehaviourID.LinkWith(fsVM.DeleteBehaviourID);
            FolderSettingsVm = fsVM;
            Path = result.ScannedPath;
            WarningNumberOfDirectories = (result.NumberOfDirectoriesThatWillSoonBeDeleted + result.NumberOfDirectoriesToDelete).ToString();
            WarningNumberOfFiles = (result.NumberOfFilesThatWillSoonBeDeleted + result.NumberOfFilesToDelete).ToString();

            result.Items
                .OrderBy(i => i.DaysToRemove)
                .ToList()
                .ForEach(i =>
                {
                    switch (i.ItemType)
                    {
                        case Core.ItemType.File:
                            Files.Add(new DetailItemViewModel(i.Path, i.DaysToRemove));
                            break;
                        case Core.ItemType.Directory:
                            Directories.Add(new DetailItemViewModel(i.Path, i.DaysToRemove));
                            break;
                    }
                });
        }

        private string GenerateWarning(DetailedScanItemResult r) =>
            $"{r.Path} - {r.DaysToRemove} day(s) to remove\n";





    }
}
