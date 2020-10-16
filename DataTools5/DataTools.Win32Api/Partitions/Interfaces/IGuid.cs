using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Win32Api.PartitionInfo
{
    /// <summary>
    /// Represents something that is identifiable by its GUID.
    /// </summary>
    /// <remarks></remarks>
    public interface IGuid
    {
        Guid Id { get; }
    }

}
