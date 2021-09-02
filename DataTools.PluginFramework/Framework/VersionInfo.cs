using DataTools.PluginFramework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.PluginFramework.Framework
{
    public class VersionInfo : IVersionInfo
    {
        public VersionInfo()
        {
            var assy = Assembly.GetCallingAssembly();
            ReadAssembly(assy);
        }

        public VersionInfo(Assembly assy)
        {
            ReadAssembly(assy);
        }

        public VersionInfo(string file)
        {
            var assy = Assembly.LoadFrom(file);
            ReadAssembly(assy);
        }

        private void ReadAssembly(Assembly assy)
        {
            var attrs = assy.GetCustomAttributes();
            string vers = null;

            foreach (var attr in attrs)
            {
                if (attr is AssemblyInformationalVersionAttribute f)
                {
                    vers = f.InformationalVersion;
                    break;
                }
                else if (attr is AssemblyFileVersionAttribute af)
                {
                    vers = af.Version;
                    break;
                }
                else if (attr is AssemblyVersionAttribute va)
                {
                    vers = va.Version;
                    break;
                }

            }

            if (vers != null)
            {
                var s = vers.Split(".");

                Major = s[0];
                Minor = s[1];
                Revision = s[2];
            }

        }

        public virtual string Major { get; set; } = "";

        public virtual string Minor { get; set; } = "";

        public virtual string Revision { get; set; } = "";

        public virtual string Notes { get; set; } = "";

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Revision}";
        }
    }
}
