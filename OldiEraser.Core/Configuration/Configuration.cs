using OldiEraser.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Core.Configuration
{
    public class Configuration : ConfigBase, IOldieConfiguration
    {
        public Configuration()
            : base("settings.json")
        {
        }

        public string DataFilePath => GetSettingValue("dataFile");
    }
}
