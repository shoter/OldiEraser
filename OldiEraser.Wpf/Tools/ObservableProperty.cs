using OldiEraser.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Wpf.Tools
{
    public class ObservableProperty<T> : Disposable, IObservableProperty, INotifyPropertyChanged
    {
        T value;
        public virtual T Value
        {
            get => value;
            set
            {
                T prev = this.value;
                this.value = value;
                FireEvent(this, value, prev);
            }
        }

        protected void FireEvent(object sender, T value, T prev)
        {
            OnModify?.Invoke(sender, new ObservablePropertyChangedArgs(prev, value));
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs("Value"));
        }

        public event EventHandler<EventArgs> OnModify;
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableProperty()
        {
        }



        public class ObservablePropertyChangedArgs : EventArgs
        {
            public T PreviousValue { get; private set; }
            public T NewValue { get; private set; }

            public ObservablePropertyChangedArgs(T previousValue, T newValue)
            {
                this.PreviousValue = previousValue;
                this.NewValue = newValue;
            }
        }
    }
}
