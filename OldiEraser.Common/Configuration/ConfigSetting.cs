using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Common.Configuration
{
    public class ConfigSetting
    {
        private readonly string key;
        private readonly string value;
        private readonly bool hasValue;

        public string Key => key;
        public string Value => value;
        public bool HasValue => hasValue;

        public ConfigSetting(string key, string value)
        {
            this.key = key;
            this.value = value;
            this.hasValue = string.IsNullOrEmpty(value) == false;
        }

        public int ToInt()
        {
            return int.Parse(Value);
        }
    }
}
