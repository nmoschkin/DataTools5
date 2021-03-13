using System;
using System.Collections.Generic;
using System.Text;

namespace DataTools.Text.Csv
{

    /// <summary>
    /// Describes a column type for sorting.
    /// </summary>
    /// <remarks></remarks>
    public enum ColumnType
    {

        /// <summary>
        /// No type, auto, or default.
        /// </summary>
        /// <remarks></remarks>
        None,

        /// <summary>
        /// Explicity textual column type.
        /// </summary>
        /// <remarks></remarks>
        Text,

        /// <summary>
        /// Explicitly numeric column type.
        /// </summary>
        /// <remarks></remarks>
        Number
    }

}
