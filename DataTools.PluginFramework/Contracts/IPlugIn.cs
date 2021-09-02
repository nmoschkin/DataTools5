
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
    public interface IPlugIn : INotifyPropertyChanged, INotifyPropertyChanging, ISerializable
    {

        /// <summary>
        /// Gets the company that created the plugin.
        /// </summary>
        string Company { get; }

        /// <summary>
        /// Gets the company that created the plugin.
        /// </summary>
        string Copyright { get; }

        /// <summary>
        /// Gets version information.
        /// </summary>
        IVersionInfo VersionInfo { get; }

        /// <summary>
        /// Gets the description of the plugin.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// True if this plugin requires a license.
        /// </summary>
        bool RequiresLicense { get; }

        /// <summary>
        /// True if only one instance of this plugin should be instantiated.
        /// </summary>
        bool IsSingleton { get; }

        /// <summary>
        /// True if the plugin furnishes its own configuration panel.
        /// </summary>
        bool HasConfigPanel { get; }

        /// <summary>
        /// Open the configuration panel on the specified site.
        /// </summary>
        /// <param name="site">The site to place the configuration panel.</param>
        void OpenConfigPanel(object site);
                
        /// <summary>
        /// Gets additional properties that are not intrinsically known to the plugin engine.
        /// </summary>
        /// <remarks>
        /// Each group contains a list of properties.  Properties can only be accessed and manipulated via property groups.
        /// This pattern enforces property grouping, as it may be required by a visual layout.
        /// </remarks>
        IList<ICustomPropertyGroup> PropertyGroups { get; }

        /// <summary>
        /// Initialize the plugin with the specified initialization bundle.
        /// </summary>
        /// <param name="config"></param>
        void Initialize(IInitializationBundle bundle);

        /// <summary>
        /// Gets an initialization bundle that can be used to initialize an object.
        /// </summary>
        /// <returns></returns>
        IInitializationBundle GetInitializationBundle();

        /// <summary>
        /// True if this plugin currently has dependencies.
        /// </summary>
        bool HasDependencies { get; }

        /// <summary>
        /// True if this plugin supports dependencies.
        /// </summary>
        /// <remarks>
        /// If this property returns true, then <see cref="GetValidDependencyTypes"/> must return at least one <see cref="System.Type"/> derived from <see cref="IPlugIn"/>.
        /// </remarks>
        bool CanHaveDependencies { get; }

        /// <summary>
        /// Add a dependency to this plugin.
        /// </summary>
        /// <param name="plugin"></param>
        void AddDependency(IPlugIn plugin);

        /// <summary>
        /// Remove a dependency for this plugin.
        /// </summary>
        /// <param name="plugin"></param>
        void RemoveDependency(IPlugIn plugin);

        /// <summary>
        /// Gets a list of all dependencies that this plugin is currently using.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This is a list of runtime dependency instances. For a list of valid dependency types, use <see cref="GetValidDependencyTypes"/>.
        /// </remarks>
        ICollection<IPlugIn> GetDependencies();

        /// <summary>
        /// Gets a list of all valid dependency types, or null if this plugin does not support dependencies.
        /// </summary>
        /// <returns></returns>
        ICollection<Type> GetValidDependencyTypes();

        /// <summary>
        /// Arbitrary data that may be stored.
        /// </summary>
        object Tag { get; set; }

    }
}
