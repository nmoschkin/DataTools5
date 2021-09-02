
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

namespace DataTools.PluginFramework
{
    /// <summary>
    /// Contains information used to initialize an overlay.
    /// </summary>
    public interface IInitializationBundle
    {
        /// <summary>
        /// A parameter that may be used to carry licensing information.
        /// </summary>
        string LicenseString { get; set; }
    }
}
