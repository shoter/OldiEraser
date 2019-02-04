using OldiEraser.Common;
using OldiEraser.Core.Configuration;
using OldiEraser.Core.Scanning;
using OldiEraser.Core.Settings;
using OldiEraser.Wpf.Dialogs;
using OldiEraser.Wpf.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OldiEraser.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel vm;
        private DetailsViewModel detailsVm;
        private readonly List<BehaviourItemViewModel> behaviours;
        public MainWindow()
        {
            InitializeComponent();
            vm = new MainWindowViewModel(App.Oldi.Data);
            behaviours = Enum.GetValues(typeof(DirectoriesDeleteBehaviour)).Cast<DirectoriesDeleteBehaviour>()
                .Select(b => new BehaviourItemViewModel()
                {
                    Id = (int)b,
                    Name = b.ToString()
                }).ToList();

            this.BehaviourList.ItemsSource = behaviours;
            this.DataContext = vm;
        }

        private void OpenAbout(object sender, RoutedEventArgs e)
        {
            vm.FolderSettingItems[0].DaysAge.Value--;
            //System.Windows.MessageBox.Show("Created by Shoter");
        }

        private void ExitApplication(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void FolderList_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var item = e.AddedItems[0] as FolderSettingItemViewModel;
                var sett = App.Oldi.Data.FolderSettings.First(s => s.DirectoryPath == item.Path.Value);

                var scanner = new OldiScanner(new FolderSettingsFileReader(), new DateTimeProvider());
                var result = scanner.DetailedScan(sett.DirectoryPath);

                detailsVm = new DetailsViewModel(result, item);
                DetailsPanel.DataContext = detailsVm;
                //BehaviourList.DataContext = detailsVm;
                //DaysAge.DataContext = detailsVm;
                //DetailFiles.DataContext = detailsVm;
                //DetailDirectories.DataContext = detailsVm;
            }

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dir = new OpenDirectoryDialog().OpenDirectory();

            if (string.IsNullOrWhiteSpace(dir) == false)
            {
                var reader = new FolderSettingsFileReader();

                var settings = reader.ReadFileAsync(dir).Result;
                if (settings == null)
                {
                    var vm = AddDirectoryDialog.Show(dir);
                    if (vm == null)
                        return;
                    settings = new FolderSettings((uint)vm.DaysBeforeRemoval, (DirectoriesDeleteBehaviour)vm.BehaviourID);
                    var writer = new FolderSettingsFileSaver();
                    writer.SaveAsync(settings, dir).Wait();
                }

                var specificSettings = new SpecificFolderSetting(dir, settings);

                App.Oldi.AddSettings(specificSettings);
                vm.FolderSettingItems.Add(new FolderSettingItemViewModel(specificSettings));
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (FolderList.SelectedItem is FolderSettingItemViewModel selected)
            {
                App.Oldi.RemoveSetting(selected.Path.Value);
                vm.FolderSettingItems.Remove(selected);
            }
        }
    }
}
