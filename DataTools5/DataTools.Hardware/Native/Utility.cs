// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: Utility
//         Miscellaneous Utility Functions
//
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using DataTools.Memory;

namespace DataTools.Hardware.Native
{


    /// <summary>
    /// Public utility methods
    /// </summary>
    public static class Utility
    {


        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Convert an array of bytes to a structure
        /// </summary>
        /// <param name="input">An arrayou of bytes</param>
        /// <param name="output">A valid structure object (cannot be null)</param>
        /// <param name="startingIndex">the starting index in the array of bytes.</param>
        /// <param name="length">Length in the array of bytes.</param>
        /// <returns>True if successful</returns>
        public static bool BytesToStruct(byte[] input, ref object output, int startingIndex = 0, int length = -1)
        {
            try
            {
                var ptr = Marshal.AllocHGlobal(input.Length);
                if (length == -1)
                    length = input.Length - startingIndex;
                Marshal.Copy(input, startingIndex, ptr, length);
                output = Marshal.PtrToStructure(ptr, output.GetType());
                Marshal.FreeHGlobal(ptr);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Writes a structure to a byte array
        /// </summary>
        /// <param name="input">Structure</param>
        /// <param name="output">Byte array</param>
        /// <returns>True if successful</returns>
        public static bool StructToBytes(object input, ref byte[] output)
        {
            try
            {
                int a = Marshal.SizeOf(input);
                var ptr = Marshal.AllocHGlobal(a);
                output = new byte[a];
                Marshal.StructureToPtr(input, ptr, false);
                Marshal.Copy(ptr, output, 0, a);
                Marshal.FreeHGlobal(ptr);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Write a structure to a byte array, and return the byte array.
        /// </summary>
        /// <param name="input"></param>
        /// <returns><see cref="Byte()"/></returns>
        public static byte[] StructToBytes(object input)
        {
            try
            {
                int a = Marshal.SizeOf(input);
                var ptr = Marshal.AllocHGlobal(a);
                byte[] output = null;
                output = new byte[a];
                Marshal.StructureToPtr(input, ptr, false);
                Marshal.Copy(ptr, output, 0, a);
                Marshal.FreeHGlobal(ptr);
                return output;
            }
            catch
            {
                return null;
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Read a structure from a stream
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object</param>
        /// <param name="offset">Offset inside the stream to begin reading the struct.</param>
        /// <param name="struct">The output struct.</param>
        /// <returns>True if successful</returns>
        public static bool ReadStruct(Stream stream, int offset, ref object @struct)
        {
            try
            {
                int a = Marshal.SizeOf(@struct);
                byte[] b;
                b = new byte[a];
                stream.Read(b, offset, a);
                return BytesToStruct(b, ref @struct);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Read a structure of type T from a stream
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object</param>
        /// <param name="struct">The output struct of type T</param>
        /// <returns>True if successful</returns>
        public static bool ReadStruct<T>(Stream stream, ref T @struct) where T : struct
        {
            try
            {
                int a = Marshal.SizeOf<T>();
                byte[] b;
                b = new byte[a];
                stream.Read(b, 0, a);
                var gch = GCHandle.Alloc(b, GCHandleType.Pinned);
                MemPtr mm = gch.AddrOfPinnedObject();
                @struct = mm.ToStruct<T>();
                gch.Free();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Gets a <see cref="DescriptionAttribute" /> value
        /// </summary>
        /// <param name="value">An object</param>
        /// <returns>A <see cref="DescriptionAttribute" /> value</returns>
        public static string GetEnumDescription(object value)
        {
            if (!(value != null && value.GetType().BaseType == typeof(Enum))) return null;

            var fi = value.GetType().GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var fe in fi)
            {

                object fobj = fe.GetValue(value);

                if (fobj.ToString() == value.ToString())
                {
                    return GetDescription(fe);
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a <see cref="DescriptionAttribute" /> value
        /// </summary>
        /// <param name="mi">A <see cref="MemberInfo"/> object</param>
        /// <returns>A <see cref="DescriptionAttribute" /> value</returns>
        public static string GetDescription(MemberInfo mi)
        {
            DescriptionAttribute attr = (DescriptionAttribute)mi.GetCustomAttribute(typeof(DescriptionAttribute));
            if (attr is null)
                return null;
            return attr.Description;
        }

        /// <summary>
        /// Gets a <see cref="DescriptionAttribute" /> value
        /// </summary>
        /// <param name="t">A <see cref="System.Type"/></param>
        /// <returns>A <see cref="DescriptionAttribute" /> value</returns>
        public static string GetDescription(Type t)
        {
            DescriptionAttribute attr = (DescriptionAttribute)t.GetCustomAttribute(typeof(DescriptionAttribute));
            if (attr is null)
                return null;
            return attr.Description;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public static bool MoveFile(string oldPath, string newPath)
        {
            return FileApi.MoveFile(oldPath, newPath) == 0 ? false : true;
        }




        /// <summary>
        /// Completely destroy a directory
        /// </summary>
        /// <param name="directory"></param>
        /// <remarks></remarks>
        public static bool DeleteWithExtremePrejudice(string directory, bool preserveRoot = true, bool warnConfirm = true)
        {
            var sh = new PInvoke.SHFILEOPSTRUCT();
            directory = Path.GetFullPath(directory);
            if (warnConfirm)
            {
                var res = MessageBox.Show("WARNING!  You are about to entirely destroy:" + "\r\n" + directory + "!" + "\r\n" + "\r\n" + "This action cannot be undone!  Proceed?", "Delete Entire Directory", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                
                if (res == MessageBoxResult.No)                   return false;
            }

            sh.hWnd = IntPtr.Zero;
            sh.pFrom = directory + '\x0';

            sh.wFunc = PInvoke.FO_DELETE;
            sh.fFlags = PInvoke.FOF_SILENT | PInvoke.FOF_NOCONFIRMATION | PInvoke.FOF_NOERRORUI | PInvoke.FOF_NOCONFIRMMKDIR;
            return PInvoke.SHFileOperation(sh) == 0;
        }



        /// <summary>
        /// Retrieves the combined total file size for every file match string in an array.
        /// </summary>
        /// <param name="FileNames">Array of filenames or filename search patterns (wildcards).</param>
        /// <returns>Total combined size of all discovered files.</returns>
        /// <remarks></remarks>
        public static long SizeOfAll(string[] FileNames)
        {
            long gt = 0L;
            IntPtr h;
            var fd = new PInvoke.WIN32_FIND_DATA();
            foreach (string s in FileNames)
            {
                h = FileApi.FindFirstFile(s, ref fd);
                if ((long)h == -1)
                    continue;
                do
                {
                    gt += fd.nFileSize;
                    fd = new PInvoke.WIN32_FIND_DATA();
                }
                while (FileApi.FindNextFile(h, ref fd));
                FileApi.FindClose(h);
            }

            return gt;
        }

        /// <summary>
        /// Starts a new process.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool Execute(string fileName, string args = null)
        {
            var proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Arguments = args;
            proc.Start();
            return true;
        }

        /// <summary>
        /// Returns true if the file is an exe.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsExe(string fileName)
        {
            return fileName.Substring(fileName.Length - 4, 4).ToLower() == ".exe";
        }

        /// <summary>
        /// Returns whether or not a file consists of the given extension.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsExt(string fileName, string ext)
        {
            if ((fileName.Substring(fileName.Length - (ext.Length + 1), ext.Length + 1).ToLower() ?? "") == ("." + ext ?? ""))
                return true;
            else
                return false;
        }

        // SHBrowseForFolder callback function

        /// <summary>
        /// SHBrowseForFolder callback function.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="uMsg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        /// <remarks>
        /// Since we need the absolute path, we take the path of the most recently
        /// selected item.  The only way to do that is through processing of the
        /// BFFM_SELCHANGED event generated by SHBrowseForFolder, and retrieve the
        /// full path using the SHGetPathFromIDList and wParam
        /// </remarks>
        public static int BrowseCallback(IntPtr hWnd, short uMsg, int wParam, int lParam)
        {
            if (uMsg == PInvoke.BFFM_SELCHANGED)
            {
                PInvoke.mBrowseCurrent = new string('\0', 260);
                PInvoke.SHGetPathFromIDList(wParam, PInvoke.mBrowseCurrent);
                PInvoke.BrowserLastFolder = wParam;
            }

            return 0;
        }




        /// <summary>
        /// Gets the file's attributes.
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static FileAttributes GetFileAttributes(string FileName)
        {
            FileAttributes GetFileAttributesRet = default;
            var wf = new PInvoke.WIN32_FIND_DATA();
            IntPtr i;
            i = FileApi.FindFirstFile(FileName, ref wf);
            if ((long)i != -1L)
            {
                FileApi.FindClose(i);
                GetFileAttributesRet = wf.dwFileAttributes;
            }
            else
            {
                GetFileAttributesRet = 0;
            }

            return GetFileAttributesRet;
        }

        /// <summary>
        /// Set the file's attributes.
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="Attributes"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool SetFileAttributes(string FileName, FileAttributes Attributes)
        {
            bool SetFileAttributesRet = default;
            if (!FileExists(FileName))
                return false;
            SetFileAttributesRet = FileApi.pSetFileAttributes(FileName, (int)Attributes) == 0 ? false : true;
            return SetFileAttributesRet;
        }

        // Return the three dates of the given file

        /// <summary>
        /// Gets the file time stamps.
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="CreationTime"></param>
        /// <param name="LastAccessTime"></param>
        /// <param name="LastWriteTime"></param>
        /// <remarks></remarks>
        public static void GetFileTime(string FileName, ref DateTime CreationTime, ref DateTime LastAccessTime, ref DateTime LastWriteTime)
        {
            var wf = new PInvoke.WIN32_FIND_DATA();
            IntPtr i;
            FileName = FileName.Trim('\\');
            i = FileApi.FindFirstFile(FileName, ref wf);
            if ((long)i != -1)
            {
                FileApi.FindClose(i);
                CreationTime = FileToLocal(wf.ftCreationTime);
                LastAccessTime = FileToLocal(wf.ftLastAccessTime);
                LastWriteTime = FileToLocal(wf.ftLastWriteTime);
            }
        }

        /// <summary>
        /// Set the file time stamps to the specified times.
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="CreationTime"></param>
        /// <param name="LastAccessTime"></param>
        /// <param name="LastWriteTime"></param>
        /// <remarks></remarks>
        public static void SetFileTime(string FileName, PInvoke.FILETIME CreationTime, PInvoke.FILETIME LastAccessTime, PInvoke.FILETIME LastWriteTime)
        {
            IntPtr hFile;
            if (Directory.Exists(FileName))
            {
                hFile = FileApi.CreateFile(FileName, FileApi.FILE_WRITE_ATTRIBUTES, 0, IntPtr.Zero, FileApi.OPEN_EXISTING, FileApi.FILE_FLAG_BACKUP_SEMANTICS, IntPtr.Zero);
            }
            else if (File.Exists(FileName))
            {
                hFile = FileApi.CreateFile(FileName, FileApi.FILE_WRITE_ATTRIBUTES, 0, IntPtr.Zero, FileApi.OPEN_EXISTING, 0, IntPtr.Zero);
            }
            else
            {
                return;
            }

            if ((long)hFile == -1L)
            {
                int g = PInvoke.GetLastError();
                return;
            }

            try
            {
                FileApi.pSetFileTime2(hFile, ref CreationTime, ref LastAccessTime, ref LastWriteTime);
            }
            catch
            {
                // MsgBox(ex.Message)
            }

            PInvoke.CloseHandle(hFile);
        }


        /// <summary>
        /// Set the file time stamps to the specified times.
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="CreationTime"></param>
        /// <param name="LastAccessTime"></param>
        /// <param name="LastWriteTime"></param>
        /// <remarks></remarks>
        public static void SetFileTime(string FileName, DateTime CreationTime, DateTime LastAccessTime, DateTime LastWriteTime)
        {
            var wf = default(PInvoke.WIN32_FIND_DATA);
            LocalToFileTime(CreationTime, ref wf.ftCreationTime);
            LocalToFileTime(LastAccessTime, ref wf.ftLastAccessTime);
            LocalToFileTime(LastWriteTime, ref wf.ftLastWriteTime);
            SetFileTime(FileName, wf.ftCreationTime, wf.ftLastAccessTime, wf.ftLastWriteTime);
        }

        // Get the size of a file

        /// <summary>
        /// Returns the file size in the style of the native function, with a high DWORD and a low DWORD.
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="lpSizeHigh"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static uint GetFileSize(string FileName, ref uint lpSizeHigh)
        {
            IntPtr hFile;
            uint SL;
            var lpSh = default(uint);
            if (!FileExists(FileName))
                return 0U;
            hFile = FileApi.CreateFile(FileName, FileApi.GENERIC_READ, 0, IntPtr.Zero, FileApi.OPEN_EXISTING, 0, IntPtr.Zero);
            if ((long)hFile == -1L)
                return 0U;
            SL = FileApi.pGetFileSize(hFile, ref lpSh);
            lpSizeHigh = lpSh;
            PInvoke.CloseHandle(hFile);
            return SL;
        }

        /// <summary>
        /// Returns the size of the file using the native function.
        /// </summary>
        /// <param name="Filename"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static long GetFileSize(string Filename)
        {
            var h = default(uint);
            uint l;
            l = GetFileSize(Filename, ref h);
            return MakeLong(l, (int)h);
        }

        // Returns a boolean indicating weather the file exists or not

        /// <summary>
        /// Returns true if the file exists.  Users native function.
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool FileExists(string FileName)
        {
            var wf = new PInvoke.WIN32_FIND_DATA();
            IntPtr i;
            i = FileApi.FindFirstFile(FileName, ref wf);
            if ((long)i != -1L)
            {
                FileApi.FindClose(i);
                return true;
            }

            return false;
        }

        // Returns a boolean indicating weather the folder exists or not

        /// <summary>
        /// Returns true if the path exists.  Uses native function.
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool PathExists(string Path)
        {
            IntPtr h;
            
            int i;
            string s;
            
            var wf = new PInvoke.WIN32_FIND_DATA();
            var df = default(bool);

            s = Path;
            i = s.Length;

            if (i == 0)
                return false;

            if (s.LastIndexOf(@"\") == s.Length - 1 & i != 3)
            {
                s = s.Substring(0, i - 1);
            }
            else if (i == 3 & s.Substring(i - 2, 2) == @":\")
            {
                s = s + "*.*";
                df = true;
            }

            h = FileApi.FindFirstFile(s, ref wf);
            if ((int)h != -1L)
            {
                if (((int)wf.dwFileAttributes & FileApi.FILE_ATTRIBUTE_DIRECTORY) == FileApi.FILE_ATTRIBUTE_DIRECTORY | df == true)
                {
                    FileApi.FindClose(h);
                    return true;
                }

                FileApi.FindClose(h);
            }

            return false;
        }

        /// <summary>
        /// Retrieves the location of a special folder.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="fSpcPath"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetSpecialFolder(IntPtr hWnd, PInvoke.SpecialFolderConstants fSpcPath)
        {
            var lpStr = new string('\0', 512);

            PInvoke.SHGetSpecialFolderPath(hWnd, lpStr, (int)fSpcPath, false);
            
            return lpStr.Trim('\0');
        }


        /// <summary>
        /// Converts a FILETIME structure into a DateTime object in the local time zone.
        /// </summary>
        /// <param name="lpTime"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime FileToLocal(PInvoke.FILETIME lpTime)
        {
            DateTime pDate;
            TimeSpan pTime;
            var lpSystem = new PInvoke.SYSTEMTIME();
            var ftLocal = new PInvoke.FILETIME();

            // convert the filetime from UTC to local time
            FileApi.FileTimeToLocalFileTime(ref lpTime, ref ftLocal);

            // convert the FILETIME structure to a system time structure
            FileApi.FileTimeToSystemTime(ref ftLocal, ref lpSystem);

            // construct a Date variable from the system time structure.
            pTime = new TimeSpan(0, lpSystem.wHour, lpSystem.wMinute, lpSystem.wSecond, lpSystem.wMilliseconds);

            try
            {
                pDate = new DateTime(lpSystem.wYear, lpSystem.wMonth, lpSystem.wDay);
                return pDate.Add(pTime);
            }
            catch
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// Converts a FILETIME structure into a DateTime object in the system time zone (generally UTC).
        /// </summary>
        /// <param name="lpTime"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime FileToSystem(PInvoke.FILETIME lpTime)
        {
            DateTime pDate;
            TimeSpan pTime;

            var lpSystem = new PInvoke.SYSTEMTIME();

            FileApi.FileTimeToSystemTime(ref lpTime, ref lpSystem);

            pTime = new TimeSpan(lpSystem.wHour, lpSystem.wMinute, lpSystem.wSecond);
            pDate = new DateTime(lpSystem.wYear, lpSystem.wMonth, lpSystem.wDay);

            return pDate.Add(pTime);
        }

        /// <summary>
        /// Returns a date-time string appropriate for injecting into a SQL record.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string TimeToSQL(DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Converts a DateTime object containing a UTC time value into a FILETIME structure
        /// </summary>
        /// <param name="time"></param>
        /// <param name="lpTime"></param>
        /// <remarks></remarks>
        public static void SystemToFileTime(DateTime time, ref PInvoke.FILETIME lpTime)
        {
            var st = new PInvoke.SYSTEMTIME()
            {
                wYear = (ushort)time.Year,
                wMonth = (ushort)time.Month,
                wDay = (ushort)time.Day,
                wHour = (ushort)time.Hour,
                wMinute = (ushort)time.Minute,
                wSecond = (ushort)time.Second
            };

            FileApi.SystemTimeToFileTime(ref st, ref lpTime);
        }

        /// <summary>
        /// Converts a DateTime object containing a local time value into a FILETIME structure
        /// </summary>
        /// <param name="time"></param>
        /// <param name="lpTime"></param>
        /// <remarks></remarks>
        public static void LocalToFileTime(DateTime time, ref PInvoke.FILETIME lpTime)
        {
            var st = new PInvoke.SYSTEMTIME()
            {
                wYear = (ushort)time.Year,
                wMonth = (ushort)time.Month,
                wDay = (ushort)time.Day,
                wHour = (ushort)time.Hour,
                wMinute = (ushort)time.Minute,
                wSecond = (ushort)time.Second,
                wMilliseconds = (ushort)time.Millisecond
            };


            var lft = new PInvoke.FILETIME();

            FileApi.SystemTimeToFileTime(ref st, ref lft);
            FileApi.LocalFileTimeToFileTime(ref lft, ref lpTime);
        }

        /// <summary>
        /// Combines two integers into a long.
        /// </summary>
        /// <param name="dwLow"></param>
        /// <param name="dwHigh"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static long MakeLong(uint dwLow, int dwHigh)
        {
            return (long)dwHigh << 32 | dwLow;
        }

        /// <summary>
        /// Combines two shorts into an int.
        /// </summary>
        /// <param name="dwLow"></param>
        /// <param name="dwHigh"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int MakeInt(ushort dwLow, short dwHigh)
        {
            return dwHigh << 16 | dwLow;
        }

        /// <summary>
        /// Makes an int resource (basically a new IntPtr with a value of i).
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IntPtr MAKEINTRESOURCE(int i)
        {
            return new IntPtr(i);
        }
    }
}