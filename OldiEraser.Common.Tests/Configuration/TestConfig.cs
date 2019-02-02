using OldiEraser.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Common.Tests.Configuration
{
    public class TestConfig : ConfigBase
    {
        public TestConfig()
            : base("settings.json")
        { }

        public ConfigSetting AppleSetting => GetSetting("section:fruits:apple");
        public string Pea => GetSettingValue("section:fruits:pea");
        public ConfigSetting FooSetting => GetSetting("section:foo");
        public string Foo => GetSettingValue("section:foo");
        public string NotHere => GetSettingValue("section:nothere");
        public ConfigSetting NotHereSetting => GetSetting("section:nothere");
    }
}
