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
        /// Get the active registry key.
        /// </summary>
        protected RegistryKey BaseKey
        {
            get => baseKey;
            private set
            {
                SetProperty(ref baseKey, value);
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

            BaseKey = RegistryKey.OpenBaseKey(hive, view);
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
        protected T[] BinaryToArray<T>(byte[] data) where T: struct
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
        protected byte[] ArrayToBinary<T>(T[] value) where T: struct
        {
            var mm = new SafePtr();
            mm.FromArray<T>(value);

            byte[] output = (byte[])mm;

            mm.Free();
            return output;
        }


        /// <summary>
        /// Write a parseable object to the registry.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueName"></param>
        /// <param name="value"></param>
        /// <param name="subKey"></param>
        protected void WriteParseable<T>(string valueName, T value, string subKey = null) where T : struct
        {
            var mtd = typeof(T).GetMethod("Parse");

            if (mtd == null || !mtd.IsStatic || !mtd.IsPublic) throw new InvalidCastException();

            RegistryKey k;

            if (subKey != null)
            {
                k = BaseKey.OpenSubKey(subKey);
            }
            else
            {
                k = BaseKey;
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
        protected T? ReadParseable<T>(string valueName, string subKey = null, string defaultValue = null) where T: struct
        {
            var mtd = typeof(T).GetMethod("Parse");

            if (mtd == null || !mtd.IsStatic || !mtd.IsPublic) throw new InvalidCastException();

            RegistryKey k;

            if (subKey != null)
            {
                k = BaseKey.OpenSubKey(subKey);
            }
            else
            {
                k = BaseKey;
            }
            
            T output;

            string s = (string)k.GetValue(valueName, defaultValue);

            if (s == null) return null;
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
        protected bool? ReadValue(string valueName, string subKey = null, bool? defaultValue = null) 
        {
            RegistryKey k;

            if (subKey != null)
            {
                k = BaseKey.OpenSubKey(subKey);
            }
            else
            {
                k = BaseKey;
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
        protected void WriteValue(string valueName, bool value, string subKey = null) 
        {
            RegistryKey k;

            if (subKey != null)
            {
                k = BaseKey.OpenSubKey(subKey);
            }
            else
            {
                k = BaseKey;
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
        protected T? ReadValue<T>(string valueName, string subKey = null, T? defaultValue = null) where T: struct
        {
            RegistryKey k;

            if (subKey != null)
            {
                k = BaseKey.OpenSubKey(subKey);
            }
            else
            {
                k = BaseKey;
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
        protected void WriteValue<T>(string valueName, T value, string subKey = null) where T : struct
        {
            RegistryKey k;

            if (subKey != null)
            {
                k = BaseKey.OpenSubKey(subKey);
            }
            else
            {
                k = BaseKey;
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
        protected string ReadValue(string valueName, string subKey = null, string defaultValue = null)
        {

            RegistryKey k;

            if (subKey != null)
            {
                k = BaseKey.OpenSubKey(subKey);
            }
            else
            {
                k = BaseKey;
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
        protected void WriteValue(string valueName, string value, string subKey = null, bool expandEnv = false)
        {

            RegistryKey k;

            if (subKey != null)
            {
                k = BaseKey.OpenSubKey(subKey);
            }
            else
            {
                k = BaseKey;
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
        protected string[] ReadValue(string valueName, string subKey = null, string[] defaultValue = null)
        {

            RegistryKey k;

            if (subKey != null)
            {
                k = BaseKey.OpenSubKey(subKey);
            }
            else
            {
                k = BaseKey;
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
        protected void WriteValue(string valueName, string[] value, string subKey = null)
        {
            RegistryKey k;

            if (subKey != null)
            {
                k = BaseKey.OpenSubKey(subKey);
            }
            else
            {
                k = BaseKey;
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
        protected List<T> ReadList<T>(string valueName, string subKey = null, string itemPrefix = "Item_") where T: struct
        {
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

            k = BaseKey.OpenSubKey(sk);

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
        protected void WriteList<T>(string valueName, IEnumerable<T> value, string subKey = null, string itemPrefix = "Item_") where T : struct
        {
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

            k = BaseKey.OpenSubKey(sk);

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
        protected List<string> ReadList(string valueName, string subKey = null, string itemPrefix = "Item_") 
        {
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

            k = BaseKey.OpenSubKey(sk);

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
        protected void WriteList(string valueName, IEnumerable<string> value, string subKey = null, string itemPrefix = "Item_") 
        {
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

            k = BaseKey.OpenSubKey(sk);

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
        protected T[] ReadArray<T>(string valueName, string subKey = null, T[] defaultValue = null) where T: struct
        {
            RegistryKey k;

            if (subKey != null)
            {
                k = BaseKey.OpenSubKey(subKey);
            }
            else
            {
                k = BaseKey;
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
        protected void WriteArray<T>(string valueName, IEnumerable<T> value, string subKey = null) where T: struct
        {
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
                k = BaseKey.OpenSubKey(subKey);
            }
            else
            {
                k = BaseKey;
            }

            var output = ArrayToBinary(work);
            k.SetValue(valueName, output, RegistryValueKind.Binary);

            if (subKey != null)
            {
                k.Close();
            }
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
