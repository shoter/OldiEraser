using Moq;
using OldiEraser.Common;
using OldiEraser.Common.IO;
using OldiEraser.Core.Settings;
using OldiEraser.Core.Tests.Configuration;
using System;
using System.IO;
using Xunit;

namespace OldiEraser.Core.Tests
{
    public class OldFileEraserTests
    {
        private readonly string testDirectory = TestSettings.Instance.OldFileEraserTestLocation;
        private readonly Mock<IDateTimeProvider> dateTimeProvider = new Mock<IDateTimeProvider>();
        private readonly Mock<FolderSettingsFileReader> folderSettingsReader = new Mock<FolderSettingsFileReader>();
        private readonly IOldFileEraser oldFileEraser;
        private readonly IFolderSettingsFileSaver folderSettingsSaver = new FolderSettingsFileSaver();

        public OldFileEraserTests()
        {
            dateTimeProvider.SetupGet(x => x.Now).Returns(DateTime.Now);
            oldFileEraser = new OldFileEraser(folderSettingsReader.Object, dateTimeProvider.Object);
            RemoveOldFilesInDirectory();
        }

        [Fact]
        public void OldFileEraser_CreateNewFiles_ShouldNotRemoveThem()
        {
            var files = CreateFiles(10, testDirectory);
            folderSettingsReader.Setup(x => x.ReadFile(It.IsAny<string>()))
                .Returns(new FolderSettings(30, DirectoriesDeleteBehaviour.DoNothing));

            oldFileEraser.EraseOldFiles(TestSettings.Instance.OldFileEraserTestLocation);

            foreach(var f in files)
            {
                Assert.True(File.Exists(f));
            }
        }

        [Fact]
        public void OldFileEraser_CreateNewFiles_30daysInFutureShouldRemoveThem()
        {
            var files = CreateFiles(10, testDirectory);
            folderSettingsReader.Setup(x => x.ReadFile(It.IsAny<string>()))
                .Returns(new FolderSettings(30, DirectoriesDeleteBehaviour.DoNothing));
            dateTimeProvider.SetupGet(x => x.Now)
                .Returns(DateTime.Now.AddDays(30));

            oldFileEraser.EraseOldFiles(TestSettings.Instance.OldFileEraserTestLocation);

            foreach (var f in files)
            {
                Assert.False(File.Exists(f));
            }
        }

        [Fact]
        public void OldFileEraser_CreateNewFiles_shouldNotRemove1DayBefore()
        {
            var files = CreateFiles(10, testDirectory);
            folderSettingsReader.Setup(x => x.ReadFile(It.IsAny<string>()))
                .Returns(new FolderSettings(30, DirectoriesDeleteBehaviour.DoNothing));
            dateTimeProvider.SetupGet(x => x.Now)
                .Returns(DateTime.Now.AddDays(29));

            oldFileEraser.EraseOldFiles(TestSettings.Instance.OldFileEraserTestLocation);

            foreach (var f in files)
            {
                Assert.True(File.Exists(f));
            }
        }

        [Fact]
        public void OldFileEraser_DoNothingBehaviour_shouldNotRemoveDirectoryAndFileInside()
        {
            var files = CreateFiles(5, testDirectory);
            string directory = Path.Combine(testDirectory, "asd\\");
            var filesInside = CreateFiles(5, directory);


            folderSettingsReader.Setup(x => x.ReadFile(It.IsAny<string>()))
                .Returns(new FolderSettings(30, DirectoriesDeleteBehaviour.DoNothing));
            dateTimeProvider.SetupGet(x => x.Now)
                .Returns(DateTime.Now.AddYears(10));

            oldFileEraser.EraseOldFiles(TestSettings.Instance.OldFileEraserTestLocation);

            Assert.True(Directory.Exists(directory));
            foreach (var f in filesInside)
                Assert.True(File.Exists(f));
        }

        [Fact]
        public void OldFileEraser_DeleteOldFolderBehaviour_SholdRemoveOldFolders()
        {
            string directory = Path.Combine(testDirectory, "asd\\");
            var filesInside = CreateFiles(5, directory);


            folderSettingsReader.Setup(x => x.ReadFile(It.IsAny<string>()))
                .Returns(new FolderSettings(30, DirectoriesDeleteBehaviour.DeleteOldDirectories));
            dateTimeProvider.SetupGet(x => x.Now)
                .Returns(DateTime.Now.AddYears(10));

            oldFileEraser.EraseOldFiles(TestSettings.Instance.OldFileEraserTestLocation);

            Assert.False(Directory.Exists(directory));
        }

        [Fact]
        public void OldFileEraser_DeleteOldFolderBehaviour_SholdNotRemoveYoungFiles()
        {
            string directory = Path.Combine(testDirectory, "asd\\");
            var filesInside = CreateFiles(5, directory);


            folderSettingsReader.Setup(x => x.ReadFile(It.IsAny<string>()))
                .Returns(new FolderSettings(30, DirectoriesDeleteBehaviour.DeleteOldDirectories));
            dateTimeProvider.SetupGet(x => x.Now)
                .Returns(DateTime.Now.AddDays(29));

            oldFileEraser.EraseOldFiles(TestSettings.Instance.OldFileEraserTestLocation);

            Assert.True(Directory.Exists(directory));
        }

        [Fact]
        public void OldFileEraser_DeleteRecursive_SholdNotRemoveDirectoryButFiles()
        {
            string directory = Path.Combine(testDirectory, "asd\\");
            var filesInside = CreateFiles(5, directory);


            folderSettingsReader.Setup(x => x.ReadFile(It.IsAny<string>()))
                .Returns(new FolderSettings(30, DirectoriesDeleteBehaviour.DeleteFilesInside));
            dateTimeProvider.SetupGet(x => x.Now)
                .Returns(DateTime.Now.AddDays(31));

            oldFileEraser.EraseOldFiles(TestSettings.Instance.OldFileEraserTestLocation);

            Assert.True(Directory.Exists(directory));
            foreach (var f in filesInside)
                Assert.False(File.Exists(f));
        }

        [Fact]
        public void OldFileEraser_DeleteRecursive_ShouldNotRemoveFilesAndDirectoryIfYoung()
        {
            string directory = Path.Combine(testDirectory, "asd\\");
            var filesInside = CreateFiles(5, directory);


            folderSettingsReader.Setup(x => x.ReadFile(It.IsAny<string>()))
                .Returns(new FolderSettings(30, DirectoriesDeleteBehaviour.DeleteFilesInside));
            dateTimeProvider.SetupGet(x => x.Now)
                .Returns(DateTime.Now.AddDays(29));

            oldFileEraser.EraseOldFiles(TestSettings.Instance.OldFileEraserTestLocation);

            Assert.True(Directory.Exists(directory));
            foreach (var f in filesInside)
                Assert.True(File.Exists(f));
        }

        [Fact]
        public void OldFileEraser_DeleteRecursive_ShouldRemoveFilesInNestedFolderButNotTheNestedFolder()
        {
            string directory = Path.Combine(testDirectory, "asd\\subfolder\\");
            var filesInside = CreateFiles(5, directory);


            folderSettingsReader.Setup(x => x.ReadFile(It.IsAny<string>()))
                .Returns(new FolderSettings(30, DirectoriesDeleteBehaviour.DeleteFilesInside));
            dateTimeProvider.SetupGet(x => x.Now)
                .Returns(DateTime.Now.AddDays(31));

            oldFileEraser.EraseOldFiles(TestSettings.Instance.OldFileEraserTestLocation);

            Assert.True(Directory.Exists(directory));
            foreach (var f in filesInside)
                Assert.False(File.Exists(f));
        }

        [Fact]
        public void OldFileEraser_DeleteRecursive_ShouldCheckSettingsInNestedFolderAndNotDeleteFilesBecauseTheyAreYoung()
        {
            string settingsDirectory = Path.Combine(testDirectory, "asd");
            string directory = Path.Combine(testDirectory, "asd\\subfolder\\");
            var filesInside = CreateFiles(5, directory);


            folderSettingsReader.Setup(x => x.ReadFile(testDirectory))
                .Returns(new FolderSettings(30, DirectoriesDeleteBehaviour.DeleteFilesInside));
            folderSettingsReader.Setup(x => x.ReadFile(settingsDirectory))
                .Returns(new FolderSettings(45, DirectoriesDeleteBehaviour.DeleteFilesInside));
            dateTimeProvider.SetupGet(x => x.Now)
                .Returns(DateTime.Now.AddDays(44));

            oldFileEraser.EraseOldFiles(TestSettings.Instance.OldFileEraserTestLocation);

            Assert.True(Directory.Exists(directory));
            foreach (var f in filesInside)
                Assert.True(File.Exists(f));
        }

        [Fact]
        public void OldFileEraser_DeleteRecursive_ShouldCheckSettingsInNestedFolderAndNotDeleteFilesBecauseDifferentBehaviour()
        {
            string settingsDirectory = Path.Combine(testDirectory, "asd");
            string directory = Path.Combine(testDirectory, "asd\\subfolder\\");
            var filesInside = CreateFiles(5, directory);


            folderSettingsReader.Setup(x => x.ReadFile(testDirectory))
                .Returns(new FolderSettings(30, DirectoriesDeleteBehaviour.DeleteFilesInside));
            folderSettingsReader.Setup(x => x.ReadFile(settingsDirectory))
                .Returns(new FolderSettings(45, DirectoriesDeleteBehaviour.DoNothing));
            dateTimeProvider.SetupGet(x => x.Now)
                .Returns(DateTime.Now.AddDays(44));

            oldFileEraser.EraseOldFiles(TestSettings.Instance.OldFileEraserTestLocation);

            Assert.True(Directory.Exists(directory));
            foreach (var f in filesInside)
                Assert.True(File.Exists(f));
        }

        [Fact]
        public void OldFileEraser_DeleteRecursive_ShouldDeleteFilesBecauseBehaviourWillChange()
        {
            string settingsDirectory = Path.Combine(testDirectory, "asd");
            string directory = Path.Combine(testDirectory, "asd\\subfolder\\");
            var filesInside = CreateFiles(5, directory);


            folderSettingsReader.Setup(x => x.ReadFile(It.Is<string>(s => s.Contains(testDirectory))))
                .Returns(new FolderSettings(45, DirectoriesDeleteBehaviour.DeleteFilesInside));
            folderSettingsReader.Setup(x => x.ReadFile(It.Is<string>(s => s.Contains(settingsDirectory))))
                .Returns(new FolderSettings(35, DirectoriesDeleteBehaviour.DeleteFilesInside));
            dateTimeProvider.SetupGet(x => x.Now)
                .Returns(DateTime.Now.AddDays(44));

            oldFileEraser.EraseOldFiles(TestSettings.Instance.OldFileEraserTestLocation);

            Assert.True(Directory.Exists(directory));
            foreach (var f in filesInside)
                Assert.False(File.Exists(f));
        }

        private string[] CreateFiles(int count, string directory)
        {
            string[] filePaths = new string[count];
            var randomFileFactory = new RandomFileFactory(directory);

            for(int i = 0;i < count;++i)
            {
                filePaths[i] = randomFileFactory.CreateRandomFile();
            }

            return filePaths;
        }

        private void RemoveOldFilesInDirectory()
        {
            Directory.CreateDirectory(TestSettings.Instance.OldFileEraserTestLocation);
            string[] files = Directory.GetFiles(TestSettings.Instance.OldFileEraserTestLocation);
            
            foreach(var f in files)
            {
                File.Delete(f);
            }

            string[] dirs = Directory.GetDirectories(testDirectory);
            foreach(var d in dirs)
            {
                Directory.Delete(d, recursive: true);
            }
        }
    }
}
