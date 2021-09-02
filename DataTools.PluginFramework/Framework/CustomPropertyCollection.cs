using DataTools.Observable;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.PluginFramework.Framework
{
  
    public class CustomPropertyCollection : ObservableDictionary<string, ICustomProperty>, ICustomPropertyCollection
    {
        public CustomPropertyCollection() : base("Name")
        {
        }

        public CustomPropertyCollection(IEnumerable<ICustomProperty> items) : base("Name", items)
        {
        }

        public CustomPropertyCollection(IList<ICustomProperty> items) : base("Name", items)
        {
        }

    }
}
