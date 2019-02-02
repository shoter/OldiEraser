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
            createTempDirectory();

            foreach (DirectoriesDeleteBehaviour behaviour in Enum.GetValues(typeof(DirectoriesDeleteBehaviour)).Cast<DirectoriesDeleteBehaviour>())
            {
                deleteTempFileIfExists();
                FolderSettings settings = new FolderSettings(dayAgeToRemove: 35, directoriesDeleteBehaviour: behaviour);

                settingsSaver.Save(settings, tempFileDirectory);
                FolderSettings loaded = settingsReader.ReadFile(tempFileDirectory);

                Assert.Equal(settings.DayAgeToRemove, loaded.DayAgeToRemove);
                Assert.Equal(settings.DirectoriesDeleteBehaviour, loaded.DirectoriesDeleteBehaviour);
            }
        }

        [Fact]
        public void Settings_NoFile_ShouldReturnNull()
        {
            createTempDirectory();
            deleteTempFileIfExists();

            Assert.Null(settingsReader.ReadFile(tempFileDirectory));
        }


        private void createTempDirectory()
        {
            var directory = Path.GetDirectoryName(tempFilePath);
            Directory.CreateDirectory(directory);
        }


        private void deleteTempFileIfExists()
        {
            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);
        }
    }
}
