using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DataTools.Text.Csv
{

    /// <summary>
    /// Specifies which kind of members of a class in a collection to import.
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    [DefaultValue(1)]
    public enum ImportFlags
    {

        /// <summary>
        /// Any public property.
        /// </summary>
        /// <remarks></remarks>
        Any = 0,

        /// <summary>
        /// Only browsable public properties.
        /// </summary>
        /// <remarks></remarks>
        Browsable = 1,

        /// <summary>
        /// Only DataMember public properties.
        /// </summary>
        /// <remarks></remarks>
        DataMember = 2,

        /// <summary>
        /// Include non-public properties.
        /// </summary>
        /// <remarks></remarks>
        NonPublic = 4,

        /// <summary>
        /// Use descriptions, where available.
        /// </summary>
        /// <remarks></remarks>
        Descriptions = 0x80
    }



}
