using OldiEraser.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Wpf.Models
{
    public class MainWindowViewModel
    {
        private readonly AppData appData;

        public ObservableCollection<FolderSettingItemViewModel> FolderSettingItems { get; } = new ObservableCollection<FolderSettingItemViewModel>();

        public MainWindowViewModel(AppData appData)
        {
            this.appData = appData;

            appData.FolderSettings
                .ForEach(f => FolderSettingItems.Add(new FolderSettingItemViewModel(f)));
        }
    }
}
