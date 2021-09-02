using DataTools.Observable;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.PluginFramework
{
    /// <summary>
    /// Default implementation of <see cref="IInitializationBundle"/>.
    /// </summary>
    public class InitializationBundle : ObservableBase, IInitializationBundle
    {

        public virtual string LicenseString { get; set; } = "";

    }
}
