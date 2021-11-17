using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DataTools.PluginFramework.Framework
{
    public class CustomValueCollection : ObservableCollection<object>, ICustomValueCollection
    {
        protected string key;

        public virtual string Key
        {
            get => key;
            set
            {
                if (key != value)
                {
                    key = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Key)));
                }
            }
        }
    }

    public class CustomValueCollection<T> : ObservableCollection<T>, ICustomValueCollection<T>
    {
        protected string key;

        public virtual string Key
        {
            get => key;
            set
            {
                if (key != value)
                {
                    key = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Key)));
                }
            }
        }
    }


}
