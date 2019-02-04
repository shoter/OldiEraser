using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OldiEraser.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OldiEraser.Core.Tests.Settings
{
    public class SpecificFolderSettingTests
    {
        [Fact]
        public void DaysToRemoveAndBehaviour_ShouldBeIgnoredDuringSerialization()
        {
            var settings = new SpecificFolderSetting(
                "C:\\temp",
                30,
                DirectoriesDeleteBehaviour.DeleteFilesInside);

            var json = JsonConvert.SerializeObject(settings);
            var dyn = JObject.Parse(json);

            Assert.False(dyn.ContainsKey("DirectoriesDeleteBehaviour"));
            Assert.False(dyn.ContainsKey("DayAgeToRemove"));
        }

        [Fact]
        public void DaysToRemoveAndBehaviour_Path_ShouldBeSerialized()
        {
            var settings = new SpecificFolderSetting(
                "C:\\temp",
                30,
                DirectoriesDeleteBehaviour.DeleteFilesInside);

            var json = JsonConvert.SerializeObject(settings);
            var dyn = JObject.Parse(json);

            Assert.True(dyn.ContainsKey("DirectoryPath"));
        }

    }
}
