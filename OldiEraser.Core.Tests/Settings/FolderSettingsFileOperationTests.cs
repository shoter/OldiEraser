using OldiEraser.Core.Settings;
using OldiEraser.Core.Tests.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OldiEraser.Core.Tests.Settings
{
    public class FolderSettingsFileOperationTests
    {
        private readonly string tempFilePath = Path.Combine(TestSettings.Instance.TempFileLocation, Global.settingFileName);
        private readonly string tempFileDirectory = TestSettings.Instance.TempFileLocation;
        private IFolderSettingsFileReader settingsReader = new FolderSettingsFileReader();
        private IFolderSettingsFileSaver settingsSaver = new FolderSettingsFileSaver();


        [Fact]
        public void Settings_AfterSaveShouldBeAbleToload()
        {
            CreateTempDirectory();

            foreach (DirectoriesDeleteBehaviour behaviour in Enum.GetValues(typeof(DirectoriesDeleteBehaviour)).Cast<DirectoriesDeleteBehaviour>())
            {
                DeleteTempFileIfExists();
                FolderSettings settings = new FolderSettings(dayAgeToRemove: 35, directoriesDeleteBehaviour: behaviour);

                settingsSaver.SaveAsync(settings, tempFileDirectory);
                FolderSettings loaded = settingsReader.ReadFileAsync(tempFileDirectory).Result;

                Assert.Equal(settings.DayAgeToRemove, loaded.DayAgeToRemove);
                Assert.Equal(settings.DirectoriesDeleteBehaviour, loaded.DirectoriesDeleteBehaviour);
            }
        }

        [Fact]
        public void Settings_NoFile_ShouldReturnNull()
        {
            CreateTempDirectory();
            DeleteTempFileIfExists();

            Assert.Null(settingsReader.ReadFileAsync(tempFileDirectory).Result);
        }


        private void CreateTempDirectory()
        {
            var directory = Path.GetDirectoryName(tempFilePath);
            Directory.CreateDirectory(directory);
        }


        private void DeleteTempFileIfExists()
        {
            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);
        }
    }
}
