using System;
using System.Collections.Generic;
using System.Text;

namespace DataTools.Text.Csv
{    
    
    /// <summary>
    /// Common interface for CSV records.
    /// </summary>
    /// <remarks></remarks>
    public interface ICsvRow
    {

        /// <summary>
        /// Get the values for a CSV record.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        string[] GetValues();

        /// <summary>
        /// Set the values for a CSV record.
        /// </summary>
        /// <param name="vals"></param>
        /// <remarks></remarks>
        void SetValues(string[] vals);

        /// <summary>
        /// Get the raw text for a CSV record.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        string Text { get; set; }
    }



}
