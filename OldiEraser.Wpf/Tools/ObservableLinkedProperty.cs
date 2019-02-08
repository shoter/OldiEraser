using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Wpf.Tools
{
    public class ObservableLinkedProperty<T> : ObservableProperty<T>
    {
        private ObservableProperty<T> link;

        public override T Value
        {
            get => link.Value;
            set
            {
                var prev = link.Value;
                link.Value = value;
                FireEvent(this, value, prev);
            }
        }


        public ObservableLinkedProperty()
        {

        }

        public void LinkWith(ObservableProperty<T> other)
        {
            if(link != null)
            {
                link.OnModify -= Link_OnModify;
            }

            link = other;
            link.OnModify += Link_OnModify;
        }

        private void Link_OnModify(object sender, EventArgs e)
        {
            var args = e as ObservablePropertyChangedArgs;
            FireEvent(this, args.NewValue, args.PreviousValue);
        }
    }
}
