using OldiEraser.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OldiEraser.Common.Tests.Configuration
{
    public class ConfigSettingTests
    {
        [Fact]
        public void BasicInitialization_ShouldHaveProperPropertiesValues()
        {
            string key = "saa";
            string value = "abra";
            var config = new ConfigSetting(key: key, value: value);

            Assert.Equal(key, config.Key);
            Assert.Equal(value, config.Value);
            Assert.True(config.HasValue);
        }

        [Fact]
        public void NullValue_HasValueShouldBeFalse()
        {
            string key = "saa";
            string value = null;

            var config = new ConfigSetting(key, value);

            Assert.False(config.HasValue);
        }

        [Fact]
        public void EmptyValue_HasValueShouldBeFalse()
        {
            string key = "saa";
            string value = "";

            var config = new ConfigSetting(key, value);

            Assert.False(config.HasValue);
        }

        [Fact]
        public void Number_ToIntShouldConvertProperly()
        {
            string key = "saa";
            int number = 12345;

            var config = new ConfigSetting(key, number.ToString());

            Assert.Equal(number, config.ToInt());
        }
    }
}
