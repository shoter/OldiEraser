using OldiEraser.Wpf.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OldiEraser.Wpf.Tests.Tools
{
    public class ObservablePropertyTests
    {
        [Fact]
        public void OnChange_ShouldFireProperEvent()
        {
            var prop = new ObservableProperty<string>();
            object who = null;
            ObservableProperty<string>.ObservablePropertyChangedArgs args = null;

            prop.OnModify += (w, a) =>
            {
                who = w;
                args = a as ObservableProperty<string>.ObservablePropertyChangedArgs;
            };

            string newVal = "asd";
            prop.Value = "asd";

            Assert.Equal(who, prop);
            Assert.Equal(newVal, args.NewValue);
        }
    }
}
