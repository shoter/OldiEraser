using OldiEraser.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OldiEraser.Common.Tests.Configuration
{
    public class ConfigBaseTests
    {
        private readonly TestConfig config = new TestConfig();

        [Fact]
        public void GetSetting_HasCorrectValues()
        {
            Assert.Equal("good", config.AppleSetting.Value);
            Assert.True(config.AppleSetting.HasValue);
        }

        [Fact]
        public void GetSettingValue_HasCorrectValue()
        {
            Assert.Equal("good", config.Pea);
        }

        [Fact]
        public void GetSetting_EmptySetting_HasEmptyValueAndHasValueIsFalse()
        {
            Assert.Equal(string.Empty, config.FooSetting.Value);
            Assert.False(config.FooSetting.HasValue);
        }

        [Fact]
        public void GetSettingValue_EmptySetting_HasEmptyValue()
        {
            Assert.Equal(string.Empty, config.Foo);
        }

        [Fact]
        public void GetSetting_Null_HasCorrectValues()
        {
            Assert.Null(config.NotHereSetting.Value);
            Assert.False(config.NotHereSetting.HasValue);
            Assert.NotEmpty(config.NotHereSetting.Key);
        }

        [Fact]
        public void GetSettingValue_Null_IsNull()
        {
            Assert.Null(config.NotHere);
        }
    }
}
