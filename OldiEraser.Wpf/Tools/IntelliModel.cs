using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Wpf.Tools
{
    public class IntelliModel : INotifyPropertyChanged
    {
        private Dictionary<IObservableProperty, string> observableProperties = new Dictionary<IObservableProperty, string>();

        public IntelliModel()
        {
            PropertyInfo[] properties = GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType.GetInterfaces().Any(i => i == typeof(IObservableProperty))&&
                    property.GetValue(this) is IObservableProperty obsProp)
                {
                    observableProperties.Add(obsProp, property.Name);
                    obsProp.OnModify += ObsProp_OnModify;
                }
            }
        }

        public ObservableProperty<T> Attach<T>(ObservableProperty<T> prop)
        {
            prop.OnModify += ObsProp_OnModify;
            return prop;
        }

        private void ObsProp_OnModify(object sender, EventArgs e)
        {
            string fieldName = observableProperties[sender as IObservableProperty] + ".Value";
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(fieldName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
