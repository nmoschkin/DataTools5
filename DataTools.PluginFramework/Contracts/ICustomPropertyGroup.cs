using DataTools.PluginFramework.Framework;

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
    /// Group property bag interface.
    /// </summary>
    public interface ICustomPropertyGroup
    {
        IChoicePool ChoicePool { get; }

        /// <summary>
        /// Gets the overlay associated with this property group.
        /// </summary>
        IPlugIn PlugIn { get; }

        /// <summary>
        /// Gets the position the group should be rendered in.
        /// </summary>
        /// <remarks>
        /// All groups with identical group positions will be rendered in alphabetical order by group name.
        /// </remarks>
        int GroupPosition { get; }

        /// <summary>
        /// Group name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Group description
        /// </summary>
        string Description { get; }

        /// <summary>
        /// True if the implementation allows for adding and removing properties.
        /// </summary>
        bool AllowAddRemoveProperties { get; }

        /// <summary>
        /// Custom property bag.
        /// </summary>
        CustomPropertyCollection Properties { get; }

    }
}
