using OldiEraser.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OldiEraser.Wpf
{
    /// <summary>
    /// Interaction logic for AddDirectoryDialog.xaml
    /// </summary>
    public partial class AddDirectoryDialog : Window
    {
        private readonly AddDirectoryDialogViewModel vm;
        private event EventHandler<RoutedEventArgs> OnSave;

        private AddDirectoryDialog(string directoryPath)
        {
            InitializeComponent();

            vm = new AddDirectoryDialogViewModel(directoryPath);
            this.DataContext = vm;
        }

        public static AddDirectoryDialogViewModel Show(string directoryPath)
        {
            var dir = new AddDirectoryDialog(directoryPath);
            bool saved = false;
            dir.OnSave += (_a, _b) => { saved = true; };
            dir.ShowDialog();

            if (saved)
                return dir.vm;
            else
                return null;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (vm.DaysBeforeRemoval > 0)
                OnSave(this, e);
            Close();
        }
    }
}
