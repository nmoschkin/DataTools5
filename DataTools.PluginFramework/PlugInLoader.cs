
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.PluginFramework
{
    public static class PlugInLoader
    {
        private static readonly List<PlugInInfo> pluginCache = new List<PlugInInfo>();

        public static IReadOnlyList<PlugInInfo> PlugInCache
        {
            get => pluginCache;
        }

        /// <summary>
        /// Loads a Plug-In DLL
        /// </summary>
        /// <param name="modulePath"></param>
        /// <param name="createDefaultInstances"></param>
        /// <returns></returns>
        public static PlugInInfo[] RegisterPlugInModule(string modulePath, bool createDefaultInstances = false)
        {
            Assembly asy = Assembly.LoadFrom(modulePath);
            var newlist = new List<PlugInInfo>();

            if (asy != null)
            {
                var alltypes = asy.GetTypes();

                var l = new List<Type>();

                foreach (var type in alltypes)
                {
                    ConstructorInfo? defcons = null;

                    if (type.IsClass && type.IsPublic)
                    {
                        var ifaces = type.GetInterfaces();
                        if (ifaces != null)
                        {
                            foreach (var iface in ifaces)
                            {
                                if (typeof(IPlugIn).IsAssignableFrom(iface))
                                {
                                    var newoverlay = new PlugInInfo(type);

                                    defcons = type.GetConstructor(new Type[0]);
                                    if (defcons != null && defcons.IsPublic)
                                    {
                                        newoverlay.DefaultConstructor = defcons;
                                        if (createDefaultInstances)
                                            newoverlay.CreateDefaultInstance();
                                    }

                                    newoverlay.FullName = type.FullName;
                                    newoverlay.LoadedAssembly = asy;

                                    pluginCache.Add(newoverlay);
                                    newlist.Add(newoverlay);

                                    break;
                                }
                            }
                        }
                    }
                }
                return newlist.ToArray();

            }

            return null;

        }


    }

    public class PlugInInfo
    {
        public PlugInInfo(Type type)
        {
            PlugInType = type;
        }

        public bool CreateDefaultInstance()
        {
            if (!HasDefaultConstructor) return false;

            var newobj = DefaultConstructor.Invoke(new object[0]);

            if (newobj != null)
            {
                DefaultInstance = (IPlugIn)newobj;
            }

            return HasDefaultInstance;
        }

        public IPlugIn CreateInstance(object[] parameters = null)
        {
            Type[] types;

            if (parameters == null || parameters.Length == 0)
            {
                parameters = new object[0];
                types = new Type[0];
            }
            else
            {
                int c = parameters.Length;
                types = new Type[c];

                for (int i = 0; i < c; i++)
                {
                    object obj = parameters[i];

                    if (obj != null)
                    {
                        types[i] = obj.GetType();
                    }
                    else
                    {
                        throw new ArgumentNullException();
                    }
                }
            }

            try
            {
                var c = PlugInType.GetConstructor(types);

                if (c != null)
                {
                    return (IPlugIn)c.Invoke(parameters);
                }

            }
            catch
            {
            }

            return null;
        }


        public bool HasDefaultConstructor { get => DefaultConstructor != null; }

        public bool HasDefaultInstance { get => DefaultInstance != null; }

        public IPlugIn DefaultInstance { get; internal set; }


        public Assembly LoadedAssembly { get; internal set; }


        public ConstructorInfo DefaultConstructor { get; internal set; }


        public Type PlugInType { get; internal set; }


        public string FullName { get; internal set; }


        public Guid RuntimeToken { get; internal set; } = Guid.NewGuid();


        public DateTime DateActivated { get; internal set; } = DateTime.Now;

    }


}
