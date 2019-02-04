using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace OldiEraser.Core.Settings
{
    public class SpecificFolderSetting : FolderSettings
    {
        public string DirectoryPath { get; set; }

        [JsonIgnore]
        public override uint DayAgeToRemove
        {
            get => base.DayAgeToRemove; internal set
            {
                base.DayAgeToRemove = value;
                Status = SpecificFolderSettingStatus.Initialized;

            }
        }

        [JsonIgnore]
        public override DirectoriesDeleteBehaviour DirectoriesDeleteBehaviour { get => base.DirectoriesDeleteBehaviour; internal set => base.DirectoriesDeleteBehaviour = value; }

        [JsonIgnore]
        public SpecificFolderSettingStatus Status { get; private set; } = SpecificFolderSettingStatus.NotInitialized;

        public SpecificFolderSetting(string directory, uint daysAge, DirectoriesDeleteBehaviour deleteBehaviour)
            : base(daysAge, deleteBehaviour)
        {
            this.DirectoryPath = directory;
            Status = SpecificFolderSettingStatus.Initialized;
        }

        public SpecificFolderSetting(string directory, FolderSettings fs)
            : this(directory, fs.DayAgeToRemove, fs.DirectoriesDeleteBehaviour) { }

        internal SpecificFolderSetting() : base(30, DirectoriesDeleteBehaviour.DoNothing)
        {
            Status = SpecificFolderSettingStatus.NotInitialized;
        }

        public async Task LoadSettingsFromFile(IFolderSettingsFileReader reader)
        {
            try
            {
                if (DirectoryPath == null)
                    throw new IOException("DirectoryPath is null");

                var settings = await reader.ReadFileAsync(DirectoryPath).ConfigureAwait(false);

                if (settings == null)
                {
                    throw new IOException("File could not be read!");
                }

                this.DayAgeToRemove = settings.DayAgeToRemove;
                this.DirectoriesDeleteBehaviour = settings.DirectoriesDeleteBehaviour;
                Status = SpecificFolderSettingStatus.Initialized;
            }
            catch (IOException)
            {
                Status = SpecificFolderSettingStatus.Broken;
            }
        }
    }
}
