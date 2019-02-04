using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldiEraser.Wpf.Tools
{
    public class TransformObservableProperty<TSource, TDestination> : ObservableProperty<TDestination>
    {
        private readonly Func<TSource, TDestination> transformFunc;
        private ObservableProperty<TSource> source;

        public override TDestination Value
        {
            get => transformFunc(source.Value);
            set { } //set does nothing
        }

        public TransformObservableProperty(Func<TSource, TDestination> transformFunc)
        {
            this.transformFunc = transformFunc;
        }

        public void LinkWith(ObservableProperty<TSource> newSource)
        {
            if(source != null)
            {
                source.OnModify -= Source_OnModify;
            }

            this.source = newSource;
            newSource.OnModify += Source_OnModify;
        }

        private void Source_OnModify(object sender, EventArgs e)
        {
            var args = e as ObservableProperty<TSource>.ObservablePropertyChangedArgs;
            FireEvent(this, transformFunc(args.PreviousValue), transformFunc(args.NewValue));
        }
    }
}
