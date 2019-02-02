using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Common.Configuration
{
    public class ConfigBase
    {
        private readonly IConfiguration configuration;

        protected IConfiguration Configuration => configuration;


        public ConfigBase(string configFileName)
        {
            this.configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configFileName, optional: false, reloadOnChange: true)
                .Build();

        }

        protected ConfigSetting GetSetting(string key)
        {
            return new ConfigSetting(key, this.configuration[key]);
        }

        protected string GetSettingValue(string key)
        {
            return this.configuration[key];
        }
    }
}