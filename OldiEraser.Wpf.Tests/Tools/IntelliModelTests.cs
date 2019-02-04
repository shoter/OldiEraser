using OldiEraser.Wpf.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OldiEraser.Wpf.Tests.Tools
{
    internal class SomeModel : IntelliModel
    {
        public ObservableProperty<string> Prop { get; } = new ObservableProperty<string>();
    }

    public class IntelliModelTests
    {
        [Fact]
        public void AfterChangeOfValue_ShouldFireNotifyEvent()
        {
            var model = new SomeModel();
            bool properFired = false;

            model.PropertyChanged += (object _, PropertyChangedEventArgs arg) =>
            {
                properFired = arg.PropertyName == nameof(SomeModel.Prop) + ".Value" ;
            };

            model.Prop.Value = "asdasd";
            Assert.True(properFired);
        }

    }
}
