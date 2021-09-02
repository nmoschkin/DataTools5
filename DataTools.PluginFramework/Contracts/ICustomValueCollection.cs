using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.Specialized;

namespace DataTools.PluginFramework
{
    public interface ICustomValueCollection : IList, ICollection, INotifyPropertyChanged, INotifyCollectionChanged
    {        
        string Key { get; }
    }

    public interface ICustomValueCollection<T> : ICustomValueCollection, IList<T>, ICollection<T>, INotifyPropertyChanged, INotifyCollectionChanged
    {
    }


}
