using System;
using System.Reflection;

namespace DataTools.Strings
{
    public static class SharedProp
    {

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // ' These functions are useful for objects whose valid values come from shared members of specific classes, such as Color and FontWeight.

        /// <summary>
        /// Returns the string name of an object that is set to one of its own (or another type's) shared members.
        /// </summary>
        /// <param name="value">The object to query.</param>
        /// <param name="altReference">Alternate reference type.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string SharedPropToName<T>(T value, Type altReference = null)
        {
            PropertyInfo[] p;
            if (altReference is null)
            {
                p = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Static);
            }
            else
            {
                p = altReference.GetProperties(BindingFlags.Public | BindingFlags.Static);
            }

            object vl;
            foreach (var pe in p)
            {
                vl = pe.GetValue(value);
                if (vl.Equals(value))
                {
                    return pe.Name;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns an object whose value corresponds to a named value of its own (or another type's) shared members.
        /// </summary>
        /// <typeparam name="T">A type that can be instantiated.</typeparam>
        /// <param name="value">The name of the value to seek.</param>
        /// <param name="altReference">Alternate reference type.</param>
        /// <param name="caseSensitive">Specifies whether the search is case-sensitive.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static T NameToSharedProp<T>(string value, Type altReference = null, bool caseSensitive = true) where T : new()
        {
            var x = new T();
            bool b;
            b = NameToSharedProp(value, ref x, altReference, caseSensitive);
            if (b)
                return x;
            else
                return default;
        }

        /// <summary>
        /// Returns an object whose value corresponds to a named value of its own (or another type's) shared members.
        /// </summary>
        /// <param name="value">The name of the value to seek.</param>
        /// <param name="instance">An active instance of the object whose value to seek.</param>
        /// <param name="altReference">Alternate reference type.</param>
        /// <param name="caseSensitive">Specifies whether the search is case-sensitive.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool NameToSharedProp<T>(string value, ref T instance, Type altReference = null, bool caseSensitive = true)
        {
            PropertyInfo[] p;
            if (altReference is object)
            {
                p = altReference.GetProperties(BindingFlags.Public | BindingFlags.Static);
            }
            else
            {
                p = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Static);
            }

            if (instance is null)
            {
                // let's just try to create it, it may work.
                instance = (T)(instance is object ? instance : System.ComponentModel.TypeDescriptor.CreateInstance(null, typeof(T), null, null));
                if (instance is null)
                    return default;
            }

            string c1 = value;
            if (caseSensitive)
            {
                foreach (var pe in p)
                {
                    if ((pe.Name ?? "") == (c1 ?? ""))
                    {
                        instance = (T)pe.GetValue(instance);
                        return true;
                    }
                }
            }
            else
            {
                c1 = c1.ToLower();
                foreach (var pe in p)
                {
                    if ((pe.Name.ToLower() ?? "") == (c1 ?? ""))
                    {
                        instance = (T)pe.GetValue(instance);
                        return true;
                    }
                }
            }

            return false;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }
}