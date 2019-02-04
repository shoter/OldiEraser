using OldiEraser.Core.Settings;
using OldiEraser.Wpf.Tools;

namespace OldiEraser.Wpf.Models
{
    public class FolderSettingItemViewModel : IntelliModel
    {
        public ObservableProperty<string> Path { get; } = new ObservableProperty<string>();
        public ObservableProperty<long> DaysAge { get; } = new ObservableProperty<long>();
        public TransformObservableProperty<int, string> DeleteBehaviour { get; }
        = new TransformObservableProperty<int, string>(i => ((DirectoriesDeleteBehaviour)i).ToString());
        public ObservableProperty<int> DeleteBehaviourID { get; } = new ObservableProperty<int>();

        public FolderSettingItemViewModel(SpecificFolderSetting setting)
        {
            Path.Value = setting.DirectoryPath;
            if (setting.Status == SpecificFolderSettingStatus.Initialized)
            {
                DaysAge.Value = setting.DayAgeToRemove;
                DeleteBehaviourID.Value = (int)setting.DirectoriesDeleteBehaviour;
                DeleteBehaviour.LinkWith(DeleteBehaviourID);
            }

            DaysAge.OnModify += SettingsModified;
            DeleteBehaviourID.OnModify += SettingsModified;
        }

        private void SettingsModified(object sender, System.EventArgs e)
        {
            if (DaysAge.Value > 0)
                App.Oldi.UpdateSetting(Path.Value, new FolderSettings((uint)DaysAge.Value, (DirectoriesDeleteBehaviour)DeleteBehaviourID.Value)); 
        }
    }
}