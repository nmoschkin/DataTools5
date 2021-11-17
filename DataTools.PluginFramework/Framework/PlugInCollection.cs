
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("DataTools.Chart")]

namespace DataTools.PluginFramework.Framework
{
    /// <summary>
    /// Represents a collection of <see cref="IPlugIn"/> implementations.
    /// </summary>
    public class PlugInCollection : ObservableCollection<IPlugIn>
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="PlugInCollection"/> class.
        /// </summary>
        internal PlugInCollection() : base()
        {
        }

    }
}
