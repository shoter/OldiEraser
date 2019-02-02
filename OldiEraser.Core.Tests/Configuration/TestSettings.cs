using OldiEraser.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Core.Tests.Configuration
{
    public class TestSettings : ConfigBase
    {
        private static TestSettings instance;
        public static TestSettings Instance => instance ?? (instance = new TestSettings());

        public TestSettings()
            : base("testSettings.json") { }

        public string TempFileLocation => GetSettingValue("tempDirectoryLocation");
        public string OldFileEraserTestLocation => GetSettingValue("oldFileEraserTestLocation");
    }
}
