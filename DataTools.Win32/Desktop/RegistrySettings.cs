using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using Microsoft.Win32;

using DataTools.Observable;
using DataTools.Win32.Memory;

namespace DataTools.Desktop
{
    /// <summary>
    /// Abstract base class for a registry serialization/deserialization class.
    /// </summary>
    public abstract class RegistrySettings : ObservableBase, IDisposable
    {
        private RegistryKey baseKey;
        private RegistryKey parentKey;

        protected bool primAsStrings;
        protected bool asLists;

        protected string rootKey;
        protected RegistryHive hive;
        protected RegistryView view;

        protected static readonly Type[] primitiveTypes = new Type[] { typeof(bool), typeof(sbyte), typeof(byte), typeof(char), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) };
        protected static readonly Type[] primitiveArrayTypes = new Type[] { typeof(bool[]), typeof(sbyte[]), typeof(byte[]), typeof(char[]), typeof(short[]), typeof(ushort[]), typeof(int[]), typeof(uint[]), typeof(long[]), typeof(ulong[]), typeof(float[]), typeof(double[]), typeof(decimal[]) };


        /// <summary>
        /// Gets or sets a value indicating whether primitives are always written to and read from the registry as string values.
        /// </summary>
        protected virtual bool PrimitivesAsStrings
        {
            get => primAsStrings;
            set
            {
                SetProperty(ref primAsStrings, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether arrays of primitives are always written to and read from the registry as individual registry entries.
        /// </summary>
        protected virtual bool ArraysAsLists
        {
            get => asLists;
            set
            {
                SetProperty(ref asLists, value);
            }
        }

        /// <summary>
        /// Get the active parent registry key that is opened.
        /// </summary>
        protected RegistryKey ParentKey
        {
            get => parentKey;
            private set
            {
                SetProperty(ref parentKey, value);
            }
        }

        /// <summary>
        /// Get the name of the root key.
        /// </summary>
        public virtual string RootKey
        {
            get => rootKey;
            protected set
            {
                SetProperty(ref rootKey, value);
            }
        }

        /// <summary>
        /// Get the registry hive.
        /// </summary>
        public virtual RegistryHive Hive
        {
            get => hive;
            set
            {
                SetProperty(ref hive, value);
            }
        }

        /// <summary>
        /// Get the registry view.
        /// </summary>
        public virtual RegistryView View
        {
            get => view;
            protected set
            {
                SetProperty(ref view, value);
            }
        }

        /// <summary>
        /// Create a new RegistrySettings-based object at the specified location.
        /// </summary>
        /// <param name="hive">The registry hive.</param>
        /// <param name="rootKey">The root key.</param>
        /// <param name="view">The registry view.</param>
        public RegistrySettings(RegistryHive hive, string rootKey, RegistryView view = RegistryView.Default)
        {
            this.hive = hive;
            this.rootKey = rootKey;
            this.view = view;

            baseKey = RegistryKey.OpenBaseKey(hive, view);
            parentKey = baseKey.OpenSubKey(rootKey, true);
        }

        /// <summary>
        /// Returns true if the specified type is a primitive type or primitive array type.
        /// </summary>
        /// <param name="t">The type to check.</param>
        /// <param name="isArray">Receives a value indicating that the type is an array.</param>
        /// <returns>True or false.</returns>
        protected static bool IsPrimitive(Type t, out bool isArray)
        {
            foreach (var tck in primitiveTypes)
            {
                if (tck.Equals(t))
                {
                    isArray = false;
                    return true;
                }
            }

            foreach (var tck in primitiveArrayTypes)
            {
                if (tck.Equals(t))
                {
                    isArray = true;
                    return true;
                }
            }

            isArray = false;
            return false;
        }

        /// <summary>
        /// Returns true if the specified type is a primitive type.
        /// </summary>
        /// <param name="t">The type to check.</param>
        /// <returns>True or false.</returns>
        protected static bool IsPrimitive(Type t)
        {
            foreach (var tck in primitiveTypes)
            {
                if (tck.Equals(t)) return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the specified type is a primitive array type.
        /// </summary>
        /// <param name="t">The type to check.</param>
        /// <returns>True or false.</returns>
        protected static bool IsPrimitiveArray(Type t)
        {
            foreach (var tck in primitiveArrayTypes)
            {
                if (tck.Equals(t)) return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the Parse and TryParse methods of a Parseable type.
        /// </summary>
        /// <param name="t">The type to check.</param>
        /// <param name="parseFunc">The Parse function <see cref="MethodInfo"/> object.</param>
        /// <param name="tryParseFunc">The TryParse function <see cref="MethodInfo"/> object.</param>
        /// <returns>True if the type is parseable.</returns>
        protected static bool GetParseableInfo(Type t, out MethodInfo parseFunc, out MethodInfo tryParseFunc)
        {
            var mtd = t.GetMethod("Parse");

            if (mtd == null || !mtd.IsStatic || !mtd.IsPublic)
            {
                parseFunc = null;
                tryParseFunc = null;

                return false;
            }

            parseFunc = mtd;

            mtd = t.GetMethod("TryParse");

            if (mtd != null && mtd.IsStatic && mtd.IsPublic)
            {
                tryParseFunc = mtd;
            }
            else
            {
                tryParseFunc = null;
            }

            return true;
        }

        /// <summary>
        /// Gets the suggested <see cref="RegistryValueKind"/> for a given type.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        protected static RegistryValueKind GetValueKind(Type t)
        {

            if (t.Equals(typeof(string)))
            {
                return RegistryValueKind.String;
            }
            else if (t.Equals(typeof(string[])))
            {
                return RegistryValueKind.MultiString;
            }
            else
            {
                var dwt = new Type[] { typeof(bool), typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint) };
                var qwt = new Type[] { typeof(long), typeof(ulong) };

                foreach (var dw in dwt)
                {
                    if (dw.Equals(t)) return RegistryValueKind.DWord;
                }

                foreach (var qw in qwt)
                {
                    if (qw.Equals(t)) return RegistryValueKind.QWord;
                }

                if (IsPrimitiveArray(t))
                {
                    return RegistryValueKind.Binary;
                }
                else
                {
                    return RegistryValueKind.Unknown;
                }

            }
        }

        /// <summary>
        /// Convert from bytes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        protected static T[] BinaryToArray<T>(byte[] data) where T: struct
        {
            var mm = (SafePtr)data;
            var output = mm.ToArray<T>();
            mm.Free();
            return output;
        }

        /// <summary>
        /// Convert to bytes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        protected static byte[] ArrayToBinary<T>(T[] value) where T: struct
        {
            var mm = new SafePtr();
            mm.FromArray<T>(value);

            byte[] output = (byte[])mm;

            mm.Free();
            return output;
        }


        /// <summary>
        /// Try to create a new instance of the specified type.
        /// </summary>
        /// <param name="t">The type to instantiate.</param>
        /// <returns>A new object or null if unsuccessful.</returns>
        protected virtual object TryCreateInstance(Type t)
        {
            var c = t.GetConstructor(Type.EmptyTypes);

            if (c == null) return null;
            return c.Invoke(new object[0]);
        }



        /// <summary>
        /// Write a parseable object to the registry.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueName"></param>
        /// <param name="value"></param>
        /// <param name="subKey"></param>
        protected virtual void WriteParseable<T>(string valueName, T value, string subKey = null) 
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));

            MethodInfo mtd;
            if (!GetParseableInfo(typeof(T), out mtd, out _) &&) throw new InvalidCastException();
          
            RegistryKey k;

            if (subKey != null)
            {
                k = ParentKey.OpenSubKey(subKey);
            }
            else
            {
                k = ParentKey;
            }

            string s = value.ToString();

            k.SetValue(valueName, s, RegistryValueKind.String);

            if (subKey != null)
            {
                k.Close();
            }
           
        }

        /// <summary>
        /// Read a parseable object from the registry.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueName"></param>
        /// <param name="subKey"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected virtual T ReadParseable<T>(string valueName, string subKey = null, string defaultValue = null) 
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));

            MethodInfo mtd;
            if (!GetParseableInfo(typeof(T), out mtd, out _)) throw new InvalidCastException();

            RegistryKey k;

            if (subKey != null)
            {
                k = ParentKey.OpenSubKey(subKey);
            }
            else
            {
                k = ParentKey;
            }
            
            T output;

            string s = (string)k.GetValue(valueName, defaultValue);

            if (s == null) return default;
            output = (T)mtd.Invoke(null, new object[] { s });

            if (subKey != null)
            {
                k.Close();
            }

            return output;
        }

        /// <summary>
        /// Read a boolean value from the registry.
        /// </summary>
        /// <param name="valueName"></param>
        /// <param name="subKey"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected virtual bool? ReadValue(string valueName, string subKey = null, bool? defaultValue = null) 
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));

            RegistryKey k;

            if (subKey != null)
            {
                k = ParentKey.OpenSubKey(subKey);
            }
            else
            {
                k = ParentKey;
            }

            string defVal;

            if (defaultValue == true)
            {
                defVal = "True";
            }
            else if (defaultValue == false)
            {
                defVal = "False";
            }
            else
            {
                defVal = null;
            }

            bool output;

            string s = (string)k.GetValue(valueName, defVal);

            if (s == null) return null;
            output = bool.Parse(s);

            if (subKey != null)
            {
                k.Close();
            }

            return output;
        }


        /// <summary>
        /// Write a boolean value to the registry.
        /// </summary>
        /// <param name="valueName"></param>
        /// <param name="value"></param>
        /// <param name="subKey"></param>
        protected virtual void WriteValue(string valueName, bool value, string subKey = null) 
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));

            RegistryKey k;

            if (subKey != null)
            {
                k = ParentKey.OpenSubKey(subKey);
            }
            else
            {
                k = ParentKey;
            }

            if (value == true)
            {
                k.SetValue(valueName, "True");
            }
            else
            {
                k.SetValue(valueName, "False");
            }

            if (subKey != null)
            {
                k.Close();
            }

        }

        /// <summary>
        /// Read a blittable value from the registry.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueName"></param>
        /// <param name="subKey"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected virtual T? ReadValue<T>(string valueName, string subKey = null, T? defaultValue = null) where T: struct
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));

            RegistryKey k;

            if (subKey != null)
            {
                k = ParentKey.OpenSubKey(subKey);
            }
            else
            {
                k = ParentKey;
            }

            T output;
            var obj = k.GetValue(valueName, defaultValue);

            if (obj == null) return null;
            output = (T)obj;

            if (subKey != null)
            {
                k.Close();
            }

            return output;
        }


        /// <summary>
        /// Write a blittable value to the registry.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueName"></param>
        /// <param name="value"></param>
        /// <param name="subKey"></param>
        protected virtual void WriteValue<T>(string valueName, T value, string subKey = null) where T : struct
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));

            RegistryKey k;

            if (subKey != null)
            {
                k = ParentKey.OpenSubKey(subKey);
            }
            else
            {
                k = ParentKey;
            }

            var vk = GetValueKind(typeof(T));

            k.SetValue(valueName, value, vk);

            if (subKey != null)
            {
                k.Close();
            }

        }

        /// <summary>
        /// Read a string from the registry.
        /// </summary>
        /// <param name="valueName"></param>
        /// <param name="subKey"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected virtual string ReadValue(string valueName, string subKey = null, string defaultValue = null)
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));

            RegistryKey k;

            if (subKey != null)
            {
                k = ParentKey.OpenSubKey(subKey);
            }
            else
            {
                k = ParentKey;
            }

            var output = (string)k.GetValue(valueName, defaultValue);

            if (subKey != null)
            {
                k.Close();
            }

            return output;
        }

        /// <summary>
        /// Write a string to the registry.
        /// </summary>
        /// <param name="valueName"></param>
        /// <param name="value"></param>
        /// <param name="subKey"></param>
        /// <param name="expandEnv"></param>
        protected virtual void WriteValue(string valueName, string value, string subKey = null, bool expandEnv = false)
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));

            RegistryKey k;

            if (subKey != null)
            {
                k = ParentKey.OpenSubKey(subKey);
            }
            else
            {
                k = ParentKey;
            }

            if (expandEnv)
            {
                k.SetValue(valueName, value, RegistryValueKind.ExpandString);
            }
            else
            {
                k.SetValue(valueName, value, RegistryValueKind.String);
            }

            if (subKey != null)
            {
                k.Close();
            }

        }

        /// <summary>
        /// Read a string array from the registry.
        /// </summary>
        /// <param name="valueName">The name of the value.</param>
        /// <param name="subKey">Optional sub key.</param>
        /// <param name="defaultValue">Optional default value.</param>
        protected virtual string[] ReadValue(string valueName, string subKey = null, string[] defaultValue = null)
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));

            RegistryKey k;

            if (subKey != null)
            {
                k = ParentKey.OpenSubKey(subKey);
            }
            else
            {
                k = ParentKey;
            }

            var output = (string[])k.GetValue(valueName, defaultValue);

            if (subKey != null)
            {
                k.Close();
            }

            return output;
        }

        /// <summary>
        /// Write a string array to the registry.
        /// </summary>
        /// <param name="valueName">The name of the value.</param>
        /// <param name="value">The value.</param>
        /// <param name="subKey">Optional sub key.</param>
        protected virtual void WriteValue(string valueName, string[] value, string subKey = null)
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));

            RegistryKey k;

            if (subKey != null)
            {
                k = ParentKey.OpenSubKey(subKey);
            }
            else
            {
                k = ParentKey;
            }

            k.SetValue(valueName, value, RegistryValueKind.MultiString);

            if (subKey != null)
            {
                k.Close();
            }

        }

        /// <summary>
        /// Reads a list of blittable items from the registry as individual entries.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="valueName">The name of the list key.</param>
        /// <param name="subKey">Optional name of the sub key beneath the root but above the list key.</param>
        /// <param name="itemPrefix">Optional item name prefix.</param>
        protected virtual IList<T> ReadList<T>(string valueName, string subKey = null, string itemPrefix = "Item_") where T: struct
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));

            string sk;
            RegistryKey k;
            var output = new List<T>();

            if (subKey != null)
            {
                sk = Path.Join(subKey, valueName);
            }
            else
            {
                sk = valueName;
            }

            k = ParentKey.OpenSubKey(sk);

            int i = 0;
            int c = k.GetValueNames().Length;

            for (i = 1; i <= c; i++) 
            {
                sk = $"{itemPrefix}{i}";

                var ret = k.GetValue(sk);

                if (ret == null) break;

                output.Add((T)ret);
            }

            k.Close();
            return output;
        }

        /// <summary>
        /// Writes a list of blittable items to the registry as individual entries.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="valueName">The name of the list key.</param>
        /// <param name="value">The values.</param>
        /// <param name="subKey">Optional name of the sub key beneath the root but above the list key.</param>
        /// <param name="itemPrefix">Optional item name prefix.</param>
        protected virtual void WriteList<T>(string valueName, IEnumerable<T> value, string subKey = null, string itemPrefix = "Item_") where T : struct
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));

            string sk;
            RegistryKey k;

            if (subKey != null)
            {
                sk = Path.Join(subKey, valueName);
            }
            else
            {
                sk = valueName;
            }

            k = ParentKey.OpenSubKey(sk);

            int i = 1;

            foreach (var val in value)
            {
                sk = $"{itemPrefix}{i}";
                WriteValue(sk, val);
                i++;
            }

            k.Close();
        }


        /// <summary>
        /// Reads a list of strings from individual registry entries.
        /// </summary>
        /// <param name="valueName">The name of the list key.</param>
        /// <param name="subKey">Optional name of the sub key beneath the root but above the list key.</param>
        /// <param name="itemPrefix">Optional item name prefix.</param>
        /// <returns></returns>
        protected virtual IList<string> ReadList(string valueName, string subKey = null, string itemPrefix = "Item_") 
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));

            string sk;
            RegistryKey k;
            var output = new List<string>();

            if (subKey != null)
            {
                sk = Path.Join(subKey, valueName);
            }
            else
            {
                sk = valueName;
            }

            k = ParentKey.OpenSubKey(sk);

            int i = 0;
            int c = k.GetValueNames().Length;

            for (i = 1; i <= c; i++)
            {
                sk = $"{itemPrefix}{i}";

                var ret = k.GetValue(sk);

                if (ret == null) break;

                output.Add((string)ret);
            }

            k.Close();
            return output;
        }

        /// <summary>
        /// Writes a list of strings to individual registry entries.
        /// </summary>
        /// <param name="valueName">The name of the list key.</param>
        /// <param name="value">The values.</param>
        /// <param name="subKey">Optional name of the sub key beneath the root but above the list key.</param>
        /// <param name="itemPrefix">Optional item name prefix.</param>
        protected virtual void WriteList(string valueName, IEnumerable<string> value, string subKey = null, string itemPrefix = "Item_") 
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));

            string sk;
            RegistryKey k;

            if (subKey != null)
            {
                sk = Path.Join(subKey, valueName);
            }
            else
            {
                sk = valueName;
            }

            k = ParentKey.OpenSubKey(sk);

            int i = 1;

            foreach (var val in value)
            {
                sk = $"{itemPrefix}{i}";
                WriteValue(sk, val);
                i++;
            }

            k.Close();
        }

        /// <summary>
        /// Reads an array of blittable types from the registry.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueName">The value name.</param>
        /// <param name="subKey">Optional sub key</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns></returns>
        protected virtual T[] ReadArray<T>(string valueName, string subKey = null, T[] defaultValue = null) where T: struct
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));

            RegistryKey k;

            if (subKey != null)
            {
                k = ParentKey.OpenSubKey(subKey);
            }
            else
            {
                k = ParentKey;
            }

            byte[] defVal;
            
            if (defaultValue != null)
            {
                defVal = ArrayToBinary(defaultValue);
            }
            else
            {
                defVal = null;
            }
            
            byte[] b = (byte[])k.GetValue(valueName, defVal);
            var output = BinaryToArray<T>(b);

            if (subKey != null)
            {
                k.Close();
            }

            return output;
        }

        /// <summary>
        /// Writes an array of blittable types to a binary value in the registry.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="valueName">The value name.</param>
        /// <param name="value">The values to write.</param>
        /// <param name="subKey">Optional sub key</param>
        protected virtual void WriteArray<T>(string valueName, IEnumerable<T> value, string subKey = null) where T: struct
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));

            RegistryKey k;

            var l = new List<T>();
            foreach (var obj in value)
            {
                l.Add(obj);
            }

            T[] work = l.ToArray();
            l.Clear();

            if (subKey != null)
            {
                k = ParentKey.OpenSubKey(subKey);
            }
            else
            {
                k = ParentKey;
            }

            var output = ArrayToBinary(work);
            k.SetValue(valueName, output, RegistryValueKind.Binary);

            if (subKey != null)
            {
                k.Close();
            }
        }

        /// <summary>
        /// Write a <see cref="IDictionary{TKey, TValue}"/> object to a sub key.
        /// </summary>
        /// <typeparam name="T">The type parameter.</typeparam>
        /// <param name="valueName">The name of the key.</param>
        /// <param name="value">The values to write.</param>
        /// <param name="subKey">The optional sub key that is above the valueName but below the root key.</param>
        protected virtual void WriteDictionary<T>(string valueName, IDictionary<string, T> value, string subKey = null) where T: struct
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));

            string sk;
            RegistryKey k;

            if (subKey != null)
            {
                sk = Path.Join(subKey, valueName);
            }
            else
            {
                sk = valueName;
            }

            k = ParentKey.OpenSubKey(sk);

            int i = 1;

            foreach (var val in value)
            {
                WriteValue(val.Key, val.Value);
                i++;
            }

            k.Close();
        }

        /// <summary>
        /// Read the values of a sub key into a <see cref="IDictionary{TKey, TValue}"/> object.
        /// </summary>
        /// <typeparam name="T">The type parameter.</typeparam>
        /// <param name="valueName">The name of the key.</param>
        /// <param name="subKey">The optional sub key that is above the valueName but below the root key.</param>
        protected virtual IDictionary<string, T> ReadDictionary<T>(string valueName, string subKey = null) where T : struct
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));

            string sk;
            RegistryKey k;

            if (subKey != null)
            {
                sk = Path.Join(subKey, valueName);
            }
            else
            {
                sk = valueName;
            }

            k = ParentKey.OpenSubKey(sk);

            string[] vn = k.GetValueNames();

            var dict = new Dictionary<string, T>();

            foreach(var name in vn)
            {
                var obj = k.GetValue(name);
                
                if (obj == null) continue;

                var value = (T)obj;

                dict.Add(name, value);
            }

            
            k.Close();

            return dict;
        }


        /// <summary>
        /// Write a <see cref="IDictionary{TKey, TValue}"/> object to a sub key.
        /// </summary>
        /// <param name="valueName">The name of the key.</param>
        /// <param name="value">The values to write.</param>
        /// <param name="subKey">The optional sub key that is above the valueName but below the root key.</param>
        protected virtual void WriteDictionary(string valueName, IDictionary<string, string> value, string subKey = null)
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));

            string sk;
            RegistryKey k;

            if (subKey != null)
            {
                sk = Path.Join(subKey, valueName);
            }
            else
            {
                sk = valueName;
            }

            k = ParentKey.OpenSubKey(sk);

            int i = 1;

            foreach (var val in value)
            {
                k.SetValue(val.Key, val.Value);
                i++;
            }

            k.Close();
        }

        /// <summary>
        /// Read the values of a sub key into a <see cref="IDictionary{TKey, TValue}"/> object.
        /// </summary>
        /// <param name="valueName">The name of the key.</param>
        /// <param name="subKey">The optional sub key that is above the valueName but below the root key.</param>
        protected virtual IDictionary<string, string> ReadDictionary(string valueName, string subKey = null) 
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));

            string sk;
            RegistryKey k;

            if (subKey != null)
            {
                sk = Path.Join(subKey, valueName);
            }
            else
            {
                sk = valueName;
            }

            k = ParentKey.OpenSubKey(sk);

            string[] vn = k.GetValueNames();

            var dict = new Dictionary<string, string>();

            foreach (var name in vn)
            {
                var obj = k.GetValue(name);

                if (obj == null) continue;

                var value = (string)obj;

                dict.Add(name, value);
            }


            k.Close();

            return dict;
        }


        /// <summary>
        /// Read an object graph from the registry.
        /// </summary>
        /// <param name="valueName">The value name of the key that will continue the graph.</param>
        /// <param name="t">The type of object to read.</param>
        /// <param name="subKey">The key path between the parent key and the valueName.</param>
        /// <returns></returns>
        protected virtual object ReadObject(string valueName, Type t, string subKey = null) 
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));
            if (t == null || !t.IsClass) throw new ArgumentException($"Type '{t.FullName}' is not a class");

            var ret = TryCreateInstance(t);

            if (ret == null) throw new ArgumentException($"Cannot create instance of type '{t.FullName}'.");

            string sk;
            RegistryKey k;

            if (subKey != null)
            {
                sk = Path.Join(subKey, valueName);
            }
            else
            {
                sk = valueName;
            }

            k = ParentKey.OpenSubKey(sk);

            var pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var p in pi)
            {
                if (!p.CanWrite) continue;
                MethodInfo mtd;

                var pb = GetParseableInfo(p.PropertyType, out mtd, out _);

                if (typeof(string).IsAssignableTo(p.PropertyType) || pb)
                {
                    string sp = (string)k.GetValue(p.Name);

                    if (sp == null) continue;

                    if (mtd != null)
                    {
                        try
                        {
                            p.SetValue(ret, mtd.Invoke(null, new object[] { sp }));
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            p.SetValue(ret, sp);
                        }
                        catch { }
                    }
                }
                else if (p.PropertyType.IsClass)
                {
                    p.SetValue(ret, ReadObject(p.Name, p.PropertyType, sk));
                }
            }

            k.Close();

            return ret;
        }

        /// <summary>
        /// Write an object graph to a registry key.
        /// </summary>
        /// <param name="valueName">The value name of the key that will continue the graph.</param>
        /// <param name="value">The object to write.</param>
        /// <param name="subKey">The key path between the parent key and the valueName.</param>
        protected virtual void WriteObject(string valueName, object value, string subKey = null) 
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));
            Type t = value.GetType();
            if (t == null || !t.IsClass) throw new ArgumentException();

            string sk;
            RegistryKey k;

            if (subKey != null)
            {
                sk = Path.Join(subKey, valueName);
            }
            else
            {
                sk = valueName;
            }

            k = ParentKey.OpenSubKey(sk);

            var pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var p in pi)
            {
                if (!p.CanWrite) continue;

                var pb = GetParseableInfo(p.PropertyType, out _, out _);
                
                if (p.PropertyType.IsAssignableFrom(typeof(string)) || pb)
                {
                    string sp = p.GetValue(value)?.ToString() ?? null;
                    if (sp == null) continue;

                    k.SetValue(p.Name, sp);
                }
                else if (p.PropertyType.IsClass)
                {
                    WriteObject(p.Name, p.GetValue(value), sk);
                }
            }

            k.Close();
        }



        #region IDisposable

        protected bool disposed = false;

        ~RegistrySettings()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            if (disposed) throw new ObjectDisposedException(nameof(RegistrySettings));
            this.Dispose(true);
        }

        protected void Dispose(bool isDisposing)
        {
            parentKey?.Close();
            parentKey = null;

            baseKey?.Close();
            baseKey = null;

            if (isDisposing) disposed = true;
        }

        #endregion

    }

    /// <summary>
    /// Abstract base class for a strongly-typed Registry serialization/deserialization class.
    /// </summary>
    /// <typeparam name="U">A class object parameter type.</typeparam>
    public abstract class RegistrySettings<U> : RegistrySettings where U: class
    {

        /// <summary>
        /// Create a new RegistrySettings-based object at the specified location.
        /// </summary>
        /// <param name="hive">The registry hive.</param>
        /// <param name="rootKey">The root key.</param>
        /// <param name="view">The registry view.</param>
        public RegistrySettings(RegistryHive hive, string rootKey, RegistryView view = RegistryView.Default) : base(hive, rootKey, view)
        {
        }

        /// <summary>
        /// Deserialize an object from the registry.
        /// </summary>
        /// <param name="valueName">The value name to deserialize.</param>
        /// <param name="subKey">The optional sub key.</param>
        /// <param name="defaultValue">The optional default value.</param>
        /// <returns></returns>
        public abstract U DeserializeObject(string valueName, string subKey = null, U defaultValue = null);

        /// <summary>
        /// Serialize an object to the registry.
        /// </summary>
        /// <param name="valueName">The value name to serialize to.</param>
        /// <param name="value">The value.</param>
        /// <param name="subKey">The optional sub key.</param>
        public abstract void SerializeObject(string valueName, U value, string subKey = null);


    }
}
