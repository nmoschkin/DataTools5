using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace AssetTool
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
    public static class Settings
    {

        public const string ConfigRootKey = @"Software\DataTools\AssetTool";

        public const int DefaultMaxRecentFiles = 40;

        public static bool OpenLastOnStartup
        {
            get
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(ConfigRootKey, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryOptions.None);

                bool res = GetBoolVal(key, "OpenLastOnStartup", true);
                key.Close();
                
                return res;
            }
            set
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(ConfigRootKey, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryOptions.None);
            
                SetBoolVal(key, "OpenLastOnStartup", value);
                
                key.Close();
            }
        }


        public static Guid LastActiveProfileId
        {
            get
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(ConfigRootKey, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryOptions.None);
                var res = (Guid)(key.GetValue("LastActiveProfileId", Guid.Empty));
                key.Close();
                return res;
            }
            set
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(ConfigRootKey, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryOptions.None);
                key.SetValue("LastActiveProfileId", value);
                key.Close();
            }
        }

        public static string LastBrowseFolder
        {
            get
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(ConfigRootKey, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryOptions.None);
                var res = (string)(key.GetValue("LastBrowseFolder", string.Empty));
                key.Close();
                return res;
            }
            set
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(ConfigRootKey, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryOptions.None);
                key.SetValue("LastBrowseFolder", value);
                key.Close();
            }
        }

        public static Size LastWindowSize
        {
            get
            {
                // all this just to make a default value
                var def = new Size(Application.Current.MainWindow.Width, Application.Current.MainWindow.Height);
                var bytes = new List<byte>();

                bytes.AddRange(BitConverter.GetBytes(def.Width));
                bytes.AddRange(BitConverter.GetBytes(def.Height));
                // end default value creation

                RegistryKey key = Registry.CurrentUser.CreateSubKey(ConfigRootKey, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryOptions.None);
                
                byte[] res = (byte[])(key.GetValue("LastWindowSize", bytes.ToArray()));

                // this data is not correct
                // overwrite the data with a correct entry
                if (res.Length != 16)
                {
                    key.SetValue("LastWindowSize", bytes.ToArray(), RegistryValueKind.Binary);
                    key.Close();
                    return def;
                }

                key.Close();

                return new Size(BitConverter.ToDouble(res, 0), BitConverter.ToDouble(res, 8));
            }
            set
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(ConfigRootKey, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryOptions.None);

                var bytes = new List<byte>();

                bytes.AddRange(BitConverter.GetBytes(value.Width));
                bytes.AddRange(BitConverter.GetBytes(value.Height));

                key.SetValue("LastWindowSize", bytes.ToArray(), RegistryValueKind.Binary);
                key.Close();
            }
        }

        public static Point LastWindowLocation
        {
            get
            {
                // all this just to make a default value
                var def = new Point(Application.Current.MainWindow.Left, Application.Current.MainWindow.Top);
                var bytes = new List<byte>();

                bytes.AddRange(BitConverter.GetBytes(def.X));
                bytes.AddRange(BitConverter.GetBytes(def.Y));
                // end default value creation

                RegistryKey key = Registry.CurrentUser.CreateSubKey(ConfigRootKey, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryOptions.None);

                byte[] res = (byte[])(key.GetValue("LastWindowLocation", bytes.ToArray()));

                // this data is not correct
                // overwrite the data with a correct entry
                if (res.Length != 16)
                {
                    key.SetValue("LastWindowLocation", bytes.ToArray(), RegistryValueKind.Binary);
                    key.Close();
                    return def;
                }
                
                key.Close();

                return new Point(BitConverter.ToDouble(res, 0), BitConverter.ToDouble(res, 8));
            }
            set
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(ConfigRootKey, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryOptions.None);

                var bytes = new List<byte>();

                bytes.AddRange(BitConverter.GetBytes(value.X));
                bytes.AddRange(BitConverter.GetBytes(value.Y));

                key.SetValue("LastWindowLocation", bytes.ToArray(), RegistryValueKind.Binary);
                key.Close();
            }
        }



        private static bool GetBoolVal(RegistryKey key, string valueName, bool? defaultValue)
        {
            object val;
            byte defVal;

            if (defaultValue == null)
            {
                defVal = 1;
            }
            else
            {
                if (defaultValue == true) defVal = 1; else defVal = 0;
            }
            
            
            if (defaultValue == null)
            {
                val = key.GetValue(valueName);
            }
            else
            {
                val = key.GetValue(valueName, new byte[1] { defVal });
            }
            bool res = ((byte[])val)[0] == 0 ? false : true;
            
            return res;
        }

        private static void SetBoolVal(RegistryKey key, string valueName, bool value)
        {
            byte[] b = new byte[1] { value ? (byte)1 : (byte)0 };

            key.SetValue(valueName, b, RegistryValueKind.Binary);
        }

        static Settings()
        {
            
        }
    }

}