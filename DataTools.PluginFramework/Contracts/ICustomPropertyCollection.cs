using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.PluginFramework
{
    public interface ICustomPropertyCollection : IList<ICustomProperty>, INotifyPropertyChanged, INotifyCollectionChanged
    {
    }

}
