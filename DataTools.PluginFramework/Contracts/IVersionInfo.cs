using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.PluginFramework
{
    public interface IVersionInfo
    {
        string Major { get; }

        string Minor { get; }

        string Revision { get; }

        string Notes { get; }

    }
}
