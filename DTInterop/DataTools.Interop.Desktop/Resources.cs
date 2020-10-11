// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library - Interop
// '
// ' Module: Resources.
// '         Tools for grabbing resources from EXE and DLL modules.
// '
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License
// ' ************************************************* ''

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using CoreCT.Memory;
using CoreCT.Text;
using DataTools.ExtendedMath.ColorMath;
using DataTools.Interop.Native;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.Interop.Desktop
{

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Flags to be used with LoadLibraryEx
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum LoadLibraryExFlags
    {

        /// <summary>
        /// If this value is used, and the executable module is a DLL, the system does not call DllMain for process and thread initialization and termination. Also, the system does not load additional executable modules that are referenced by the specified module.
        /// </summary>
        DONT_RESOLVE_DLL_REFERENCES = 0x1,

        /// <summary>
        /// If this value is used, the system does not check OnlineAppLocker rules or apply OnlineSoftware Restriction Policies for the DLL. This action applies only to the DLL being loaded and not to its dependencies. This value is recommended for use in setup programs that must run extracted DLLs during installation.
        /// </summary>
        LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x10,

        /// <summary>
        /// If this value is used, the system maps the file into the calling process's virtual address space as if it were a data file. Nothing is done to execute or prepare to execute the mapped file. Therefore, you cannot call functions like GetModuleFileName, GetModuleHandle or GetProcAddress with this DLL. Using this value causes writes to read-only memory to raise an access violation. Use this flag when you want to load a DLL only to extract messages or resources from it.
        /// </summary>
        LOAD_LIBRARY_AS_DATAFILE = 0x2,

        /// <summary>
        /// Similar to LOAD_LIBRARY_AS_DATAFILE, except that the DLL file is opened with exclusive write access for the calling process. Other processes cannot open the DLL file for write access while it is in use. However, the DLL can still be opened by other processes.
        /// </summary>
        LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 0x40,

        /// <summary>
        /// If this value is used, the system maps the file into the process's virtual address space as an image file. However, the loader does not load the static imports or perform the other usual initialization steps. Use this flag when you want to load a DLL only to extract messages or resources from it. If forced integrity checking is desired for the loaded file then LOAD_LIBRARY_AS_IMAGE is recommended instead.
        /// </summary>
        LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x20,

        /// <summary>
        /// If this value is used, the application's installation directory is searched for the DLL and its dependencies. Directories in the standard search path are not searched. This value cannot be combined with LOAD_WITH_ALTERED_SEARCH_PATH.
        /// </summary>
        LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 0x200,

        /// <summary>
        /// This value is a combination of LOAD_LIBRARY_SEARCH_APPLICATION_DIR, LOAD_LIBRARY_SEARCH_SYSTEM32, and LOAD_LIBRARY_SEARCH_USER_DIRS. Directories in the standard search path are not searched. This value cannot be combined with LOAD_WITH_ALTERED_SEARCH_PATH.
        /// </summary>
        LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x1000,

        /// <summary>
        /// If this value is used, the directory that contains the DLL is temporarily added to the beginning of the list of directories that are searched for the DLL's dependencies. Directories in the standard search path are not searched.
        /// </summary>
        LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 0x100,

        /// <summary>
        /// If this value is used, %windows%\system32 is searched for the DLL and its dependencies. Directories in the standard search path are not searched. This value cannot be combined with LOAD_WITH_ALTERED_SEARCH_PATH.
        /// </summary>
        LOAD_LIBRARY_SEARCH_SYSTEM32 = 0x800,

        /// <summary>
        /// If this value is used, directories added using the AddDllDirectory or the SetDllDirectory function are searched for the DLL and its dependencies. If more than one directory has been added, the order in which the directories are searched is unspecified. Directories in the standard search path are not searched. This value cannot be combined with LOAD_WITH_ALTERED_SEARCH_PATH.
        /// </summary>
        LOAD_LIBRARY_SEARCH_USER_DIRS = 0x400,

        /// <summary>
        /// If this value is used and lpFileName specifies an absolute path, the system uses the alternate file search strategy discussed in the Remarks section to find associated executable modules that the specified module causes to be loaded. If this value is used and lpFileName specifies a relative path, the behavior is undefined.
        /// </summary>
        LOAD_WITH_ALTERED_SEARCH_PATH = 0x8
    }

    /// <summary>
    /// Tools for retrieving system and individual EXE/DLL resources, and converting resources to WPF format.
    /// </summary>
    public static class Resources
    {

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public const int RT_CURSOR = 1;
        public const int RT_BITMAP = 2;
        public const int RT_ICON = 3;
        public const int RT_MENU = 4;
        public const int RT_DIALOG = 5;
        public const int RT_STRING = 6;
        public const int RT_FONTDIR = 7;
        public const int RT_FONT = 8;
        public const int RT_ACCELERATOR = 9;
        public const int RT_RCDATA = 10;
        public const int RT_MESSAGETABLE = 11;
        public const int DIFFERENCE = 11;
        public const int RT_GROUP_CURSOR = RT_CURSOR + DIFFERENCE;
        public const int RT_GROUP_ICON = RT_ICON + DIFFERENCE;
        public const int RT_VERSION = 16;
        public const int RT_DLGINCLUDE = 17;
        public const int RT_PLUGPLAY = 19;
        public const int RT_VXD = 20;
        public const int RT_ANICURSOR = 21;
        public const int RT_ANIICON = 22;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        [DllImport("shell32.dll", EntryPoint = "ExtractAssociatedIconExW")]
        static extern IntPtr ExtractAssociatedIconEx(IntPtr hInst, [MarshalAs(UnmanagedType.LPTStr)] string lpIconPath, ref int lpiIconIndex, ref int lpiIconId);
        [DllImport("user32", EntryPoint = "LoadStringW", CharSet = CharSet.Unicode)]
        static extern int LoadString(IntPtr hInstance, int uID, MemPtr lpBuffer, int nBufferMax);
        [DllImport("kernel32")]
        static extern IntPtr LockResource(IntPtr hResData);
        [DllImport("kernel32")]
        static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);
        [DllImport("kernel32")]
        static extern int SizeofResource(IntPtr hModule, IntPtr hResInfo);
        [DllImport("kernel32", EntryPoint = "LoadLibraryExW", CharSet = CharSet.Unicode)]

        static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, LoadLibraryExFlags flags);

        public delegate bool EnumResNameProc(IntPtr hModule, string lpszType, string lpszName, IntPtr lParam);

        public delegate bool EnumResNameProcPtr(IntPtr hModule, MemPtr lpszType, MemPtr lpszName, IntPtr lParam);

        [DllImport("kernel32", EntryPoint = "EnumResourceNamesExW", CharSet = CharSet.Unicode)]

        static extern bool EnumResourceNamesEx(IntPtr hModule, string lpszType, EnumResNameProc lpEnumFunc, IntPtr lParam, int dwFlags, int langId);
        [DllImport("kernel32", EntryPoint = "EnumResourceNamesExW", CharSet = CharSet.Unicode)]

        static extern bool EnumResourceNamesEx(IntPtr hModule, IntPtr lpszType, EnumResNameProcPtr lpEnumFunc, IntPtr lParam, int dwFlags, int langId);
        [DllImport("user32")]
        static extern int LookupIconIdFromDirectoryEx(IntPtr presbits, [MarshalAs(UnmanagedType.Bool)] bool fIcon, int cxDesired, int cyDesired, int Flags);
        [DllImport("kernel32", EntryPoint = "FindResourceExW", CharSet = CharSet.Unicode)]

        static extern IntPtr FindResourceEx(IntPtr hModule, [MarshalAs(UnmanagedType.LPWStr)] string lpType, [MarshalAs(UnmanagedType.LPWStr)] string lpName, ushort wLanguage);
        [DllImport("kernel32", EntryPoint = "FindResourceExW", CharSet = CharSet.Unicode)]

        static extern IntPtr FindResourceEx(IntPtr hModule, IntPtr lpType, IntPtr lpName, ushort wLanguage);
        [DllImport("user32")]
        static extern IntPtr CreateIconFromResourceEx(IntPtr phIconBits, int cbIconBits, [MarshalAs(UnmanagedType.Bool)] bool fIcon, int dwVersion, int cxDesired, int cyDesired, int uFlags);

        public enum SystemIconSizes
        {
            Small = PInvoke.SHIL_SMALL,
            Large = PInvoke.SHIL_LARGE,
            ExtraLarge = PInvoke.SHIL_EXTRALARGE,
            Jumbo = PInvoke.SHIL_JUMBO
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NEWHEADER
        {
            public ushort Reserved;
            public ushort ResType;
            public ushort ResCount;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ICONRESDIR
        {
            public byte Width;
            public byte Height;
            public byte ColorCount;
            public byte reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RESOURCEHEADER
        {
            public int DataSize;
            public int HeaderSize;
            public int Type;
            public int Name;
            public int DataVersion;
            public short MemoryFlags;
            public short LanguageId;
            public int Version;
            public int Characteristics;
        }

        /// <summary>
        /// Holds a collection of MemPtr.
        /// </summary>
        /// <remarks></remarks>
        public class ResCol : List<MemPtr>
        {
        }

        /// <summary>
        /// Private icon WPFLibrary cache.
        /// </summary>
        /// <remarks></remarks>
        private static Dictionary<string, System.Windows.Media.Imaging.BitmapSource> WPFLibCache = new Dictionary<string, System.Windows.Media.Imaging.BitmapSource>();

        /// <summary>
        /// Add an icon to the WPFLibrary cache.
        /// </summary>
        /// <param name="resId">The entire icon resource identifier, including filename and resource number.</param>
        /// <param name="icn">The Icon object to add.</param>
        /// <remarks></remarks>
        public static void AddToWPFLibCache(string resId, System.Windows.Media.Imaging.BitmapSource icn)
        {
            if (resId is null)
                return;
            WPFLibCache.Add(resId, icn);
        }

        /// <summary>
        /// Lookup a WPFLibrary Icon object from the private cache.
        /// </summary>
        /// <param name="resId">The entire icon resource identifier, including filename and resource number.</param>
        /// <param name="icn">Variable that receives the Icon object.</param>
        /// <returns>True if the resource was found.</returns>
        /// <remarks></remarks>
        public static bool LookupWPFLibIcon(string resId, ref System.Windows.Media.Imaging.BitmapSource icn)
        {
            if (resId is null)
                return false;
            return WPFLibCache.TryGetValue(resId, out icn);
        }


        /// <summary>
        /// Private icon library cache.
        /// </summary>
        /// <remarks></remarks>
        private static Dictionary<string, Icon> LibCache = new Dictionary<string, Icon>();

        /// <summary>
        /// Add an icon to the library cache.
        /// </summary>
        /// <param name="resId">The entire icon resource identifier, including filename and resource number.</param>
        /// <param name="icn">The Icon object to add.</param>
        /// <remarks></remarks>
        public static void AddToLibCache(string resId, Icon icn)
        {
            if (resId is null)
                return;
            LibCache.Add(resId, icn);
        }

        /// <summary>
        /// Lookup a library Icon object from the private cache.
        /// </summary>
        /// <param name="resId">The entire icon resource identifier, including filename and resource number.</param>
        /// <param name="icn">Variable that receives the Icon object.</param>
        /// <returns>True if the resource was found.</returns>
        /// <remarks></remarks>
        public static bool LookupLibIcon(string resId, ref Icon icn)
        {
            if (resId is null)
                return false;
            return LibCache.TryGetValue(resId, out icn);
        }

        /// <summary>
        /// Clear the private library cache.
        /// </summary>
        /// <remarks></remarks>
        public static void ClearLibCache()
        {
            LibCache.Clear();
            GC.Collect(0);
        }

        /// <summary>
        /// Parses a resource filename and attempts to distill a valid absolute file path and optional resource identifier from the input string.
        /// </summary>
        /// <param name="fileName">Filename to parse.</param>
        /// <param name="resId">Optional variable to receive the resource identifier.</param>
        /// <returns>A cleaned filename.</returns>
        /// <remarks></remarks>
        public static string ParseResourceFilename(string fileName, ref int resId)
        {
            if (fileName is null)
            {
                return null;
            }

            // ' let's do some jiggery to figure out what the fileName string actually points to:
            // ' strip the double quotes sometimes passed in (especially from Registry variables)
            if (fileName.Substring(0, 1) == "\"")
            {
                fileName = fileName.Replace("\"", "");
            }

            // ' if we have an @ sign (as is common of resource identifiers) we strip that out.
            if (fileName.Substring(0, 1) == "@")
                fileName = fileName.Substring(1);

            // ' does this file have an attached resource identifier?
            int i = fileName.LastIndexOf(",");
            if (i != -1)
            {
                int.TryParse(fileName.Substring(i + 1), out resId);
                fileName = fileName.Substring(0, i);
            }

            // ' finally, we expand any embedded environment variables.
            fileName = Environment.ExpandEnvironmentVariables(fileName);

            // ' after all that, we still don't have a path?  We'll assume system32 is the path.
            if (fileName.IndexOf(@"\") == -1)
            {
                fileName = Environment.ExpandEnvironmentVariables(@"%systemroot%\system32\" + fileName);
            }

            if (fileName.IndexOf("%") >= 0)
                return null;
            try
            {
                if (File.Exists(fileName) == false)
                    return null;
                else
                    return fileName;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Enumerate the resource directory of all of the resources of the given type in the specified module.
        /// </summary>
        /// <param name="hmod">Handle to a valid, open module from which to retrieve the resource directory.</param>
        /// <param name="restype">The resource type.</param>
        /// <returns>An array of MemPtr structures.</returns>
        /// <remarks></remarks>
        public static ResCol EnumResources(IntPtr hmod, IntPtr restype)
        {
            var _ptrs = new ResCol();
            int c = 0;
            EnumResourceNamesEx(hmod, restype, new EnumResNameProcPtr((hModule, lpszType, lpszName, lParam) =>
            {
                _ptrs.Add(lpszName);
                c += 1;
                return true;
            }), IntPtr.Zero, 0, 0);
            return _ptrs;
        }

        /// <summary>
        /// Returns true if the given pointer is an INTRESOURCE identifier.
        /// </summary>
        /// <param name="ptr"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsIntResource(IntPtr ptr)
        {
            return (ptr.ToInt64() & 0xFFFFL) == ptr.ToInt64();
        }

        /// <summary>
        /// Loads a string resource from the given library at the specified resource identifier location.
        /// </summary>
        /// <param name="fileName">Resource string containing filename and resource id.</param>
        /// <param name="uFlags">Optional flags.</param>
        /// <param name="parseResName">Specify whether to parse the resource id from the filename.</param>
        /// <param name="maxBuffer">Specifies the maximum size of the return value, in bytes.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string LoadStringResource(string fileName, LoadLibraryExFlags uFlags = LoadLibraryExFlags.LOAD_LIBRARY_AS_DATAFILE | LoadLibraryExFlags.LOAD_LIBRARY_AS_IMAGE_RESOURCE, bool parseResName = false, int maxBuffer = 4096)
        {
            string LoadStringResourceRet = default;
            LoadStringResourceRet = LoadStringResource(fileName, 0, uFlags, true, maxBuffer);
            return LoadStringResourceRet;
        }

        /// <summary>
        /// Loads a string resource from the given library at the specified resource identifier location.
        /// </summary>
        /// <param name="fileName">Library from which to load the resource.</param>
        /// <param name="resId">Resource identifier.</param>
        /// <param name="uFlags">Optional flags.</param>
        /// <param name="parseResName">Specify whether to parse the resource id from the filename.</param>
        /// <param name="maxBuffer">Specifies the maximum size of the return value, in bytes.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string LoadStringResource(string fileName, int resId, LoadLibraryExFlags uFlags = LoadLibraryExFlags.LOAD_LIBRARY_AS_DATAFILE | LoadLibraryExFlags.LOAD_LIBRARY_AS_IMAGE_RESOURCE, bool parseResName = false, int maxBuffer = 4096)
        {
            string LoadStringResourceRet = default;
            IntPtr hmod;
            if (parseResName)
            {
                fileName = ParseResourceFilename(fileName, ref resId);
            }
            else
            {
                int argresId = 0;
                fileName = ParseResourceFilename(fileName, resId: ref argresId);
            }

            try
            {
                hmod = Resources.LoadLibraryEx(fileName, IntPtr.Zero, uFlags);
            }
            catch (Exception ex)
            {
                return null;
            }

            if (hmod == IntPtr.Zero)
            {
                throw new NativeException(PInvoke.GetLastError());
            }

            var mm = new MemPtr();

            mm.AllocZero(maxBuffer);
            LoadString(hmod, resId, mm, maxBuffer);
            LoadStringResourceRet = Conversions.ToString(mm);
            mm.Free();

            PInvoke.FreeLibrary(hmod);
            return LoadStringResourceRet;
        }

        /// <summary>
        /// A simple class to hold WPF-compatible icon resources.
        /// </summary>
        /// <remarks></remarks>
        public class LibraryIcon
        {

            /// <summary>
            /// The name of the library icon (or the resource identifier).
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Name { get; set; }

            /// <summary>
            /// The BitmapSource of the icon.
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public System.Windows.Media.Imaging.BitmapSource Image { get; set; }

            /// <summary>
            /// Instantiate a new LibraryIcon object.
            /// </summary>
            /// <remarks></remarks>
            public LibraryIcon()
            {
            }

            /// <summary>
            /// Instantiate a new LibraryIcon object.
            /// </summary>
            /// <param name="name">Name or resource id of the icon.</param>
            /// <param name="image">BitmapSource object of the icon.</param>
            /// <remarks></remarks>
            public LibraryIcon(string name, System.Windows.Media.Imaging.BitmapSource image)
            {
                Name = name;
                Image = image;
            }
        }

        /// <summary>
        /// Loads all icons from a specified file.  Returns an observable collection of LibraryIcon.
        /// </summary>
        /// <param name="fileName">The filename from which to load the icons.</param>
        /// <param name="desiredSize">The desired size of the icon.</param>
        /// <param name="uFlags">Optional flags.</param>
        /// <returns>An ObservableCollection(Of LibraryIcon)</returns>
        /// <remarks></remarks>
        public static System.Collections.ObjectModel.ObservableCollection<LibraryIcon> LoadAllLibraryIcons(string fileName, StandardIcons desiredSize, LoadLibraryExFlags uFlags = LoadLibraryExFlags.LOAD_LIBRARY_AS_DATAFILE | LoadLibraryExFlags.LOAD_LIBRARY_AS_IMAGE_RESOURCE)
        {
            fileName = Environment.ExpandEnvironmentVariables(fileName);
            var l = new System.Collections.ObjectModel.ObservableCollection<LibraryIcon>();
            IntPtr hmod;
            try
            {
                hmod = Resources.LoadLibraryEx(fileName, IntPtr.Zero, uFlags);
            }
            catch (Exception ex)
            {
                return null;
            }

            if (hmod == IntPtr.Zero)
            {
                throw new NativeException();
            }

            var enumres = EnumResources(hmod, (IntPtr)RT_GROUP_ICON);
            int i;
            int c = enumres.Count - 1;
            string s;
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                s = "#" + (i + 1);
                l.Add(new LibraryIcon(s, MakeWPFImage(_internalLoadLibraryIcon(fileName, i, null, desiredSize, uFlags, enumres, false, hmod))));
            }

            PInvoke.FreeLibrary(hmod);
            return l;
        }

        /// <summary>
        /// Loads an icon resource from a file.
        /// </summary>
        /// <param name="fileName">Icon resource string containing filename and resource id.</param>
        /// <param name="desiredSize">The desired size of the icon in one of the standard icon sizes.</param>
        /// <param name="uFlags">Optional flags.</param>
        /// <param name="enumRes">Optional pre-enumerated resource directory.</param>
        /// <returns>A managed-code .NET Framework Icon object.</returns>
        /// <remarks></remarks>
        public static Icon LoadLibraryIcon(string fileName, StandardIcons desiredSize, LoadLibraryExFlags uFlags = LoadLibraryExFlags.LOAD_LIBRARY_AS_DATAFILE | LoadLibraryExFlags.LOAD_LIBRARY_AS_IMAGE_RESOURCE, ResCol enumRes = null)
        {
            Icon LoadLibraryIconRet = default;
            LoadLibraryIconRet = _internalLoadLibraryIcon(fileName, 0x80000, null, desiredSize, uFlags, enumRes, true, IntPtr.Zero);
            return LoadLibraryIconRet;
        }

        /// <summary>
        /// Loads an icon resource from a file.
        /// </summary>
        /// <param name="fileName">Library, executable or icon file.</param>
        /// <param name="iIcon">The icon index.  A negative value means the index is the name of the icon, a positive value indicates the absolute position in the resource table.</param>
        /// <param name="desiredSize">The desired size of the icon in one of the standard icon sizes.</param>
        /// <param name="uFlags">Optional flags.</param>
        /// <param name="enumRes">Optional pre-enumerated resource directory.</param>
        /// <returns>A managed-code .NET Framework Icon object.</returns>
        /// <remarks></remarks>
        public static Icon LoadLibraryIcon(string fileName, int iIcon, StandardIcons desiredSize, LoadLibraryExFlags uFlags = LoadLibraryExFlags.LOAD_LIBRARY_AS_DATAFILE | LoadLibraryExFlags.LOAD_LIBRARY_AS_IMAGE_RESOURCE, ResCol enumRes = null)
        {
            Icon LoadLibraryIconRet = default;
            LoadLibraryIconRet = _internalLoadLibraryIcon(fileName, iIcon, null, desiredSize, uFlags, enumRes, false, IntPtr.Zero);
            return LoadLibraryIconRet;
        }

        /// <summary>
        /// Loads an icon resource from a file.
        /// </summary>
        /// <param name="fileName">Library, executable or icon file.</param>
        /// <param name="resIcon">The icon index string identifier.</param>
        /// <param name="desiredSize">The desired size of the icon in one of the standard icon sizes.</param>
        /// <param name="uFlags">Optional flags.</param>
        /// <param name="enumRes">Optional pre-enumerated resource directory.</param>
        /// <returns>A managed-code .NET Framework Icon object.</returns>
        /// <remarks></remarks>
        public static Icon LoadLibraryIcon(string fileName, string resIcon, StandardIcons desiredSize, LoadLibraryExFlags uFlags = LoadLibraryExFlags.LOAD_LIBRARY_AS_DATAFILE | LoadLibraryExFlags.LOAD_LIBRARY_AS_IMAGE_RESOURCE, ResCol enumRes = null)
        {
            Icon LoadLibraryIconRet = default;
            LoadLibraryIconRet = _internalLoadLibraryIcon(fileName, 0x80000, resIcon, desiredSize, uFlags, enumRes, false, IntPtr.Zero);
            return LoadLibraryIconRet;
        }

        /// <summary>
        /// Internal function to load an icon resource from a file, somehow.
        /// </summary>
        /// <param name="fileName">Library, executable or icon file.</param>
        /// <param name="iIcon">The icon index to open. This parameter is ignored if resIcon is not null.</param>
        /// <param name="resIcon">The icon index string identifier.</param>
        /// <param name="desiredSize">The desired size of the icon in one of the standard icon sizes.</param>
        /// <param name="uFlags">Flags.</param>
        /// <param name="enumRes">pre-enumerated resource directory.</param>
        /// <param name="parseIconIndex">Whether to parse the icon index directly from the filename.  The resIcon parameter is ignored if this is true.</param>
        /// <param name="hMod">The handle to an open module or 0.</param>
        /// <returns>A managed-code .NET Framework Icon object.</returns>
        /// <remarks></remarks>
        private static Icon _internalLoadLibraryIcon(string fileName, int iIcon, string resIcon, StandardIcons desiredSize, LoadLibraryExFlags uFlags, ResCol enumRes, bool parseIconIndex, IntPtr hMod)
        {
            Icon _internalLoadLibraryIconRet = default;
            string lk = null;
            Icon icn = null;
            bool noh = hMod == IntPtr.Zero;
            PInvoke.BITMAPINFOHEADER idata;
            if (parseIconIndex)
            {
                fileName = ParseResourceFilename(fileName, ref iIcon);
            }
            else
            {
                int argresId = 0;
                fileName = ParseResourceFilename(fileName, resId: ref argresId);
            }

            if (resIcon is object)
            {
                int argresId1 = 0;
                fileName = ParseResourceFilename(fileName, resId: ref argresId1);
                if (resIcon[0] == '#' && TextTools.IsNumber(resIcon.Substring(1)))
                {
                    iIcon = -int.Parse(resIcon.Substring(1));
                }
            }
            else
            {
                int argresId2 = 0;
                fileName = ParseResourceFilename(fileName, resId: ref argresId2);
            }

            if (fileName is null)
                return null;
            lk = fileName + "," + -iIcon;
            if (LookupLibIcon(lk, ref icn))
            {
                return icn;
            }

            int cx;
            int cy;

            // a shortcut.  Every value in the StandardIcons enum begins with 'Icon' and ends with a number.
            cx = (int)Conversion.Val(desiredSize.ToString().Substring(4));
            cy = cx;
            IntPtr hicon;
            int idx = iIcon;
            IntPtr hres;
            IntPtr hdata;
            IntPtr hglob;

            // ' if it's an actual icon file, we use our own IconImage class to read it.
            if (Path.GetExtension(fileName) == ".ico")
            {
                var fi = new IconImage(fileName);
                IconImageEntry fa = null;
                foreach (var fe in fi.Entries)
                {

                    // ' find the closest size greater than the requested size image.
                    if (fe.EntryInfo.wBitsPixel == 32)
                    {
                        // ' store the image if it's bigger.
                        if (fa is null || fa.Width <= fe.Width)
                            fa = fe;
                        if (fe.StandardIconType == desiredSize)
                            return fe.ToIcon();
                    }
                }
                // ' we're done, no need to open a resource.
                if (fa is object)
                    return fa.ToIcon();
                else
                    return null;
            }

            // ' open the library.
            if (noh)
            {
                try
                {
                    hMod = Resources.LoadLibraryEx(fileName, IntPtr.Zero, uFlags);
                }
                catch (Exception ex)
                {
                    return null;
                }

                if (hMod == IntPtr.Zero)
                {
                    throw new NativeException();
                }
            }

            // ' pre-initialize the return value with null.
            _internalLoadLibraryIconRet = null;

            // ' if enumRes was already passed in we don't need to do this step...
            // ' also, if enumRes was passed in, it was by a particular function enumerating only RT_GROUP_ICON,
            // ' so a distinction does not have to be made in that case.

            // ' we want RT_GROUP_ICON resources.
            if (enumRes is null)
                enumRes = EnumResources(hMod, (IntPtr)RT_GROUP_ICON);

            // ' no RT_GROUP_ICONs, they must want an actual ICON.
            if (enumRes is null)
            {
                enumRes = EnumResources(hMod, (IntPtr)RT_ICON);

                // ' are we looking for the integer name of the icon resource?
                if (iIcon < 0)
                {
                    idx = 0;
                    foreach (var n in enumRes)
                    {
                        // ' looking for resources with this INTRESOURCE id
                        if (n.Handle.ToInt64() == -iIcon)
                        {
                            break;
                        }
                        else if (!IsIntResource(n))
                        {
                            if (n.CharAt(0L) == '#')
                            {
                                // ' a resource with an string integer name?
                                string s = n.GetString(1L);
                                int i;
                                if (int.TryParse(s, out i))
                                {
                                    if (i == -iIcon)
                                        break;
                                }
                            }
                            else if ((n.ToString() ?? "") == (resIcon ?? ""))
                            {
                                // ' a plain string resource name?
                                break;
                            }
                        }
                        // ' iterate the index to find the absolute position of the named resource.
                        idx += 1;
                    }
                }

                if (enumRes is null)
                {
                    if (noh)
                        PInvoke.FreeLibrary(hMod);
                    return _internalLoadLibraryIconRet;
                }

                if (idx > enumRes.Count)
                {
                    if (noh)
                        PInvoke.FreeLibrary(hMod);
                    return _internalLoadLibraryIconRet;
                }

                // ' find the icon.
                hres = FindResourceEx(hMod, new IntPtr(RT_ICON), enumRes[idx].Handle, 0);

                // ' load the resource.
                hglob = LoadResource(hMod, hres);

                // ' grab the memory handle.
                hdata = LockResource(hglob);

                // ' Grab the raw bitmap structure from the icon resource, so we
                // ' can use the actual width and height as opposed to the
                // ' system-stretched return result.
                idata = (PInvoke.BITMAPINFOHEADER)Marshal.PtrToStructure(hdata, typeof(PInvoke.BITMAPINFOHEADER));

                // ' create the icon from the data in the resource.
                // ' I read a great many articles before I finally figured out that &H30000 MUST be passed, just because.
                // ' There is no specified reason.  That's simply the magic number and it won't work without it.
                hicon = CreateIconFromResourceEx(hdata, SizeofResource(hMod, hres), true, 0x30000, idata.biWidth, idata.biWidth, 0);

                // ' clone the unmanaged icon.
                icn = (Icon)Icon.FromHandle(hicon).Clone();
                // ' add to the library cache
                AddToLibCache(lk, icn);

                // ' set return value
                _internalLoadLibraryIconRet = icn;

                // ' free our unmanaged resources.
                PInvoke.DestroyIcon(hicon);
                if (noh)
                    PInvoke.FreeLibrary(hMod);
                return _internalLoadLibraryIconRet;
            }

            // ' are we looking for the integer name of an icon resource?
            if (iIcon < 0)
            {
                idx = 0;
                foreach (var n in enumRes)
                {
                    // ' looking for resources with this INTRESOURCE id
                    if (n.Handle.ToInt64() == -iIcon)
                    {
                        break;
                    }
                    else if (!IsIntResource(n))
                    {
                        if (n.CharAt(0L) == '#')
                        {
                            // ' a resource with an string integer name?
                            string s = n.GetString(1L);
                            int i;
                            if (int.TryParse(s, out i))
                            {
                                if (i == -iIcon)
                                    break;
                            }
                        }
                        else if ((n.ToString() ?? "") == (resIcon ?? ""))
                        {
                            // ' a plain string resource name?
                            break;
                        }
                    }
                    // ' iterate the index to find the absolute position of the named resource.
                    idx += 1;
                }
            }

            // ' we have resources, but the index is too high.
            if (idx >= enumRes.Count)
            {
                if (noh)
                    PInvoke.FreeLibrary(hMod);
                return _internalLoadLibraryIconRet;
            }

            // ' find the group icon resource.
            hres = FindResourceEx(hMod, new IntPtr(RT_GROUP_ICON), enumRes[idx].Handle, 0);
            if (hres != IntPtr.Zero)
            {
                // ' load the resource.
                hglob = LoadResource(hMod, hres);

                // ' grab the handle
                hdata = LockResource(hglob);
                int i;

                // ' lookup the icon by size.
                i = LookupIconIdFromDirectoryEx(hdata, true, cx, cy, 0);
                if (i == 0)
                {
                    if (noh)
                        PInvoke.FreeLibrary(hMod);
                    return _internalLoadLibraryIconRet;
                }

                // ' find THAT icon resource.
                hres = FindResourceEx(hMod, new IntPtr(RT_ICON), new IntPtr(i), 0);

                // 'load that resource.
                hglob = LoadResource(hMod, hres);

                // 'grab that handle.
                hdata = LockResource(hglob);

                // ' Grab the raw bitmap structure from the icon resource, so we
                // ' can use the actual width and height as opposed to the
                // ' system-stretched return result.
                idata = (PInvoke.BITMAPINFOHEADER)Marshal.PtrToStructure(hdata, typeof(PInvoke.BITMAPINFOHEADER));

                // ' create the icon.
                hicon = CreateIconFromResourceEx(hdata, SizeofResource(hMod, hres), true, 0x30000, idata.biWidth, idata.biWidth, 0);

                // ' clone the unmanaged icon.

                if (hicon != IntPtr.Zero)
                {
                    icn = (Icon)Icon.FromHandle(hicon).Clone();

                    // ' add to the library cache
                    AddToLibCache(lk, icn);

                    // ' set return value
                    _internalLoadLibraryIconRet = icn;

                    // ' destroy the unmanaged icon.
                    PInvoke.DestroyIcon(hicon);
                }
            }

            // ' free the library.
            if (noh)
                PInvoke.FreeLibrary(hMod);
            return _internalLoadLibraryIconRet;
        }

        /// <summary>
        /// Gets the number of icon resources in a specified file or library.
        /// </summary>
        /// <param name="library"></param>
        /// <returns>Total number of icon resources.</returns>
        /// <remarks></remarks>
        public static int GetLibraryIconCount(string library = @"%systemroot%\system32\shell32.dll")
        {
            var a = new IntPtr();
            var b = new IntPtr();

            return PInvoke.ExtractIconEx(Environment.ExpandEnvironmentVariables(library), -1, ref a , ref b, 0U);
        }

        /// <summary>
        /// Retrieves a Bitmap resource from a library.
        /// </summary>
        /// <param name="iBmp"></param>
        /// <param name="iSize"></param>
        /// <param name="uFlags"></param>
        /// <param name="library"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Bitmap GetLibraryBitmap(int iBmp, Size iSize, int uFlags = PInvoke.LR_DEFAULTCOLOR + PInvoke.LR_CREATEDIBSECTION, string library = @"%systemroot%\system32\shell32.dll")
        {
            Bitmap GetLibraryBitmapRet = default;
            var hInst = PInvoke.LoadLibrary(Environment.ExpandEnvironmentVariables(library));
            IntPtr hBmp;
            if (hInst == IntPtr.Zero)
                return null;
            hBmp = PInvoke.LoadImage(hInst, (IntPtr)iBmp, PInvoke.IMAGE_BITMAP, iSize.Width, iSize.Height, uFlags);
            if (hBmp == IntPtr.Zero)
                return null;
            PInvoke.FreeLibrary(hInst);
            GetLibraryBitmapRet = Image.FromHbitmap(hBmp);
            NativeShell.DeleteObject(hBmp);
            return GetLibraryBitmapRet;
        }

        private static string MakeKey(int iIcon, SystemIconSizes shil)
        {
            return $"{iIcon},{shil}";
        }

        private static int GetIconIndex(string filename)
        {
            var sgfin = default(ShellFileGetAttributesOptions);
            var sgfout = default(ShellFileGetAttributesOptions);
            var lpInfo = new PInvoke.SHFILEINFO();
            IntPtr x;
            int iFlags = 0;
            iFlags = iFlags | PInvoke.SHGFI_SYSICONINDEX | PInvoke.SHGFI_PIDL;
            SafePtr mm = (SafePtr)filename;
            mm.Length += 2L;
            NativeShell.SHParseDisplayName(mm.handle, IntPtr.Zero, out x, sgfin, ref sgfout);
            mm.Free();
            PInvoke.SHGetItemInfo(x, 0, ref lpInfo, Marshal.SizeOf(lpInfo), iFlags);
            Marshal.FreeCoTaskMem(x);
            return lpInfo.iIcon;
        }

        /// <summary>
        /// Retrieves the icon for the file from the shell system image list.
        /// </summary>
        /// <param name="lpFilename">Filename for which to retrieve the file icon.</param>
        /// <param name="shil">The desired size of the icon.</param>
        /// <param name="iIndex">Receives the index of the image in the system image list.</param>
        /// <returns>A System.Drawing.Icon image.</returns>
        /// <remarks></remarks>
        public static Icon GetFileIcon(string lpFilename, SystemIconSizes shil, ref int? iIndex)
        {
            // ' The shell system image list
            var riid = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");
            Icon icn = null;
            IntPtr i = new IntPtr();
            int iIcon;
            iIcon = (iIndex is object && iIndex > 0) == true ? (int)iIndex : GetIconIndex(lpFilename);
            if (iIcon == 0)
                return null;
            string key = MakeKey(iIcon, shil);
            if (LookupLibIcon(key, ref icn))
            {
                return icn;
            }

            PInvoke.SHGetImageList((int)shil, ref riid, ref i);
            i = (IntPtr)PInvoke.ImageList_GetIcon(i, iIcon, 0U);
            if (i != IntPtr.Zero)
            {
                icn = (Icon)Icon.FromHandle(i).Clone();
                PInvoke.DestroyIcon(i);
                if (iIndex is object)
                {
                    iIndex = iIcon;
                }

                AddToLibCache(key, icn);
                return icn;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves the icon for the file from the shell system image list as WPF <see cref="System.Windows.Media.Imaging.BitmapSource"/>.
        /// </summary>
        /// <param name="fileName">Filename for which to retrieve the file icon.</param>
        /// <param name="shil">The desired size of the icon.</param>
        /// <returns>A <see cref="System.Windows.Media.Imaging.BitmapSource"/> object.</returns>
        /// <remarks></remarks>
        public static System.Windows.Media.Imaging.BitmapSource GetFileIconWPF(string fileName, SystemIconSizes shil = SystemIconSizes.Large)
        {
            int iIcon;
            iIcon = GetIconIndex(fileName);
            System.Windows.Media.Imaging.BitmapSource bs = null;
            string key = MakeKey(iIcon, shil);
            if (iIcon != 0 && !LookupWPFLibIcon(key, ref bs))
            {
                int? argiIndex = null;
                bs = MakeWPFImage(GetFileIcon(fileName, shil, iIndex: ref argiIndex));
                AddToWPFLibCache(key, bs);
            }

            return bs;
        }


        /// <summary>
        /// Gets the system imagelist cache index for the icon for the shell item pointed to by lpItemID
        /// </summary>
        /// <param name="lpItemID">A pointer to a SHITEMID structure.</param>
        /// <param name="fSmall">Retrieve a small icon.</param>
        /// <param name="hIml">Unused</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int GetItemIconIndex(IntPtr lpItemID, bool fSmall)
        {
            var lpInfo = new PInvoke.SHFILEINFO();
            int i;
            int fFlags;
            if (fSmall == false)
            {
                fFlags = PInvoke.SHGFI_SYSICONINDEX + PInvoke.SHGFI_ICON;
            }
            else
            {
                fFlags = PInvoke.SHGFI_SYSICONINDEX + PInvoke.SHGFI_SMALLICON;
            }

            i = (int)PInvoke.SHGetItemInfo(lpItemID, 0, ref lpInfo, Strings.Len(lpInfo), fFlags | PInvoke.SHGFI_PIDL);
            if (i != 0L)
            {
                return lpInfo.iIcon;
            }

            return 0;
        }

        /// <summary>
        /// Gets the system imagelist cache index for the icon for the shell item pointed to by lpFilename
        /// </summary>
        /// <param name="lpFilename">Filename for which to retrieve the icon index.</param>
        /// <param name="shil">Size of the icon for which to retrieve the index.</param>
        /// <returns>The shell image list index for the file icon.</returns>
        /// <remarks></remarks>
        public static int GetFileIconIndex(string lpFilename, SystemIconSizes shil = SystemIconSizes.ExtraLarge)
        {
            var lpInfo = new PInvoke.SHFILEINFO();
            int iFlags = 0;
            iFlags = iFlags | PInvoke.SHGFI_SYSICONINDEX;
            SafePtr mm = (SafePtr)lpFilename;
            mm.Length += Strings.Len('A');
            PInvoke.SHGetItemInfo(mm.handle, 0, ref lpInfo, Marshal.SizeOf(lpInfo), iFlags);
            return lpInfo.iIcon;
        }

        /// <summary>
        /// Retrieves a pointer to a system image list.
        /// </summary>
        /// <param name="shil">The image size for the image list to retrieve.</param>
        /// <returns>A pointer to a system image list for images of the specified size.</returns>
        /// <remarks></remarks>
        public static IntPtr GetSystemImageList(SystemIconSizes shil)
        {
            IntPtr i = new IntPtr();
            var riid = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");

            PInvoke.SHGetImageList((int)shil, ref riid, ref i);
            return i;
        }

        /// <summary>
        /// Retrieves a file icon from its index in the system image list.
        /// </summary>
        /// <param name="index">Index of icon to retrieve.</param>
        /// <param name="shil">Size of icon to retrieve.</param>
        /// <returns>A System.Drawing.Icon object.</returns>
        /// <remarks></remarks>
        public static Icon GetFileIconFromIndex(int index, SystemIconSizes shil = SystemIconSizes.ExtraLarge)
        {
            IntPtr i = new IntPtr();
            Icon icn;
            var riid = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");

            int iFlags = 0;
            iFlags = iFlags | PInvoke.SHGFI_SYSICONINDEX;

            PInvoke.SHGetImageList((int)shil, ref riid, ref i);

            i = (IntPtr)PInvoke.ImageList_GetIcon(i, index, 0U);

            if (i != IntPtr.Zero)
            {
                icn = (Icon)Icon.FromHandle(i).Clone();
                PInvoke.DestroyIcon(i);
                return icn;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves the icon for the specified shell item handle.
        /// </summary>
        /// <param name="lpItemID">A pointer to a SHITEMID structure.</param>
        /// <param name="shil">The size of the icon to retrieve.</param>
        /// <returns>A System.Drawing.Icon object.</returns>
        /// <remarks></remarks>
        public static Icon GetItemIcon(IntPtr lpItemID, SystemIconSizes shil = SystemIconSizes.ExtraLarge)
        {
            var lpInfo = new PInvoke.SHFILEINFO();
            IntPtr i = new IntPtr();
            Icon icn;
            var riid = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");
            // iFlags = SHGFI_ICON
            int iFlags = 0;
            iFlags = iFlags | PInvoke.SHGFI_SYSICONINDEX | PInvoke.SHGFI_PIDL;
            i = PInvoke.SHGetItemInfo(lpItemID, 0, ref lpInfo, Marshal.SizeOf(lpInfo), iFlags);
            if (lpInfo.iIcon == 0)
            {
                return null;
            }

            PInvoke.SHGetImageList((int)shil, ref riid, ref i);
            i = (IntPtr)PInvoke.ImageList_GetIcon(i, lpInfo.iIcon, 0U);
            if (i != IntPtr.Zero)
            {
                icn = (Icon)Icon.FromHandle(i).Clone();
                PInvoke.DestroyIcon(i);
                return icn;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gray out an icon.
        /// </summary>
        /// <param name="icn">The input icon.</param>
        /// <returns>The grayed out icon.</returns>
        /// <remarks></remarks>
        public static Image GrayIcon(Icon icn)
        {
            var n = new Bitmap(icn.Width, icn.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var g = Graphics.FromImage(n);
            g.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, n.Width, n.Height));
            g.DrawIcon(icn, 0, 0);
            g.Dispose();
            var bm = new System.Drawing.Imaging.BitmapData();
            var mm = new MemPtr(n.Width * n.Height * 4);
            bm.Stride = n.Width * 4;
            bm.Scan0 = mm;
            bm.PixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            bm.Width = n.Width;
            bm.Height = n.Height;
            bm = n.LockBits(new Rectangle(0, 0, n.Width, n.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite | System.Drawing.Imaging.ImageLockMode.UserInputBuffer, System.Drawing.Imaging.PixelFormat.Format32bppArgb, bm);
            int i;
            int c;

            // Dim b() As Byte

            // ReDim b((bm.Stride * bm.Height) - 1)
            // MemCpy(b, bm.Scan0, bm.Stride * bm.Height)
            c = bm.Stride * bm.Height - 1;
            int stp = (int)(bm.Stride / (double)bm.Width);

            // For i = 3 To c Step stp
            // If b(i) > &H7F Then b(i) = &H7F
            // Next

            var loopTo = c;
            for (i = 3; stp >= 0 ? i <= loopTo : i >= loopTo; i += stp)
            {
                if (mm.ByteAt(i) > 0x7F)
                    mm.ByteAt(i) = 0x7F;
            }

            // MemCpy(bm.Scan0, b, bm.Stride * bm.Height)
            n.UnlockBits(bm);
            mm.Free();
            return n;
        }

        /// <summary>
        /// Create a Device Independent Bitmap from an icon.
        /// </summary>
        /// <param name="icn"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IntPtr DIBFromIcon(Icon icn)
        {
            var bmp = IconToTransparentBitmap(icn);
            var _d = new IntPtr();

            return MakeDIBSection(bmp, ref _d);
        }

        public static System.Windows.Media.Imaging.BitmapSource MakeWPFImage(Icon img)
        {
            var _d = new IntPtr();
            return MakeWPFImage(img, ref _d);
        }

        public static System.Windows.Media.Imaging.BitmapSource MakeWPFImage(Bitmap img)
        {
            var _d = new IntPtr();
            return MakeWPFImage(img, ref _d);
        }

        /// <summary>
        /// Creates a WPF BitmapSource from a Bitmap.
        /// </summary>
        /// <param name="img">The <see cref="System.Drawing.Icon"/> object to convert.</param>
        /// <param name="bitPtr">Set this to zero.</param>
        /// <param name="dpiX">The X DPI to use to create the new image (default is 96.0)</param>
        /// <param name="dpiY">The Y DPI to use to create the new image (default is 96.0)</param>
        /// <param name="createOnApplicationThread"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static System.Windows.Media.Imaging.BitmapSource MakeWPFImage(Icon img, ref IntPtr bitPtr, double dpiX = 96.0d, double dpiY = 96.0d, bool createOnApplicationThread = true)
        {
            return MakeWPFImage(IconToTransparentBitmap(img), ref bitPtr, dpiX, dpiY, createOnApplicationThread);
        }

        /// <summary>
        /// Creates a WPF BitmapSource from a Bitmap.
        /// </summary>
        /// <param name="img">The <see cref="System.Drawing.Image"/> object to convert.</param>
        /// <param name="bitPtr">Set this to zero.</param>
        /// <param name="dpiX">The X DPI to use to create the new image (default is 96.0)</param>
        /// <param name="dpiY">The Y DPI to use to create the new image (default is 96.0)</param>
        /// <param name="createOnApplicationThread"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static System.Windows.Media.Imaging.BitmapSource MakeWPFImage(Bitmap img, ref IntPtr bitPtr, double dpiX = 96.0d, double dpiY = 96.0d, bool createOnApplicationThread = true)
        {
            if (img is null)
                return null;
            if (img.Width == 0)
                return null;
            int BytesPerRow = (int)((double)(img.Width * 32 + 31 & ~31) / 8d);
            int size = img.Height * BytesPerRow;
            var bm = new System.Drawing.Imaging.BitmapData();
            System.Windows.Media.Imaging.BitmapSource bmp = null;
            try
            {
                bm = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppPArgb, bm);
                if (createOnApplicationThread && System.Windows.Application.Current is object)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() => bmp = System.Windows.Media.Imaging.BitmapSource.Create(img.Width, img.Height, dpiX, dpiY, System.Windows.Media.PixelFormats.Bgra32, null, bm.Scan0, size, BytesPerRow));
                }
                else
                {
                    bmp = System.Windows.Media.Imaging.BitmapSource.Create(img.Width, img.Height, dpiX, dpiY, System.Windows.Media.PixelFormats.Bgra32, null, bm.Scan0, size, BytesPerRow);
                }
            }
            catch (Exception ex)
            {
                if (bm is object)
                    img.UnlockBits(bm);
                Interaction.MsgBox("Error: " + ex.Message);
                return null;
            }

            img.UnlockBits(bm);
            return bmp;
        }

        /// <summary>
        /// Creates a System.Drawing.Bitmap image from a WPF source.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Bitmap MakeBitmapFromWPF(System.Windows.Media.Imaging.BitmapSource source)
        {
            var mm = new MemPtr();
            Bitmap bmp = null;
            if (System.Windows.Application.Current is object)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    bmp = new Bitmap(source.PixelWidth, source.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    mm.Alloc(bmp.Width * bmp.Height * 4);
                    var bm = new System.Drawing.Imaging.BitmapData();
                    bm.Scan0 = mm.Handle;
                    bm.Stride = bmp.Width * 4;
                    bm = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite | System.Drawing.Imaging.ImageLockMode.UserInputBuffer, System.Drawing.Imaging.PixelFormat.Format32bppArgb, bm);
                    source.CopyPixels(System.Windows.Int32Rect.Empty, mm.Handle, (int)mm.Length, bmp.Width * 4);
                    bmp.UnlockBits(bm);
                    mm.Free();
                });
            }
            else
            {
                bmp = new Bitmap(source.PixelWidth, source.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                mm.Alloc(bmp.Width * bmp.Height * 4);
                var bm = new System.Drawing.Imaging.BitmapData();
                bm.Scan0 = mm.Handle;
                bm.Stride = bmp.Width * 4;
                bm = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite | System.Drawing.Imaging.ImageLockMode.UserInputBuffer, System.Drawing.Imaging.PixelFormat.Format32bppArgb, bm);
                source.CopyPixels(System.Windows.Int32Rect.Empty, mm.Handle, (int)mm.Length, bmp.Width * 4);
                bmp.UnlockBits(bm);
                mm.Free();
            }

            return bmp;
        }

        /// <summary>
        /// Create a writable device-independent bitmap from the specified image.
        /// </summary>
        /// <param name="img">Image to copy.</param>
        /// <param name="bitPtr">Optional variable to receive a pointer to the bitmap bits.</param>
        /// <returns>A new DIB handle that must be destroyed with DeleteObject().</returns>
        /// <remarks></remarks>
        public static IntPtr MakeDIBSection(Bitmap img, ref IntPtr bitPtr)
        {
            // Build header.
            // adapted from C++ code examples.

            short wBitsPerPixel = 32;
            int BytesPerRow = (int)((double)(img.Width * wBitsPerPixel + 31 & ~31L) / 8d);
            int size = img.Height * BytesPerRow;
            var bmpInfo = default(PInvoke.BITMAPINFO);
            var mm = new MemPtr();
            int bmpSizeOf = Marshal.SizeOf(bmpInfo);
            mm.ReAlloc(bmpSizeOf + size);
            var pbmih = default(PInvoke.BITMAPINFOHEADER);
            pbmih.biSize = Marshal.SizeOf(pbmih);
            pbmih.biWidth = img.Width;
            pbmih.biHeight = img.Height; // ' positive indicates bottom-up DIB
            pbmih.biPlanes = 1;
            pbmih.biBitCount = wBitsPerPixel;
            pbmih.biCompression = (int)PInvoke.BI_RGB;
            pbmih.biSizeImage = size;
            pbmih.biXPelsPerMeter = (int)(24.5d * 1000d); // pixels per meter! And these values MUST be correct if you want to pass a DIB to a native menu.
            pbmih.biYPelsPerMeter = (int)(24.5d * 1000d); // pixels per meter!
            pbmih.biClrUsed = 0;
            pbmih.biClrImportant = 0;
            var pPixels = IntPtr.Zero;
            int DIB_RGB_COLORS = 0;
            Marshal.StructureToPtr(pbmih, mm.Handle, false);
            var hPreviewBitmap = PInvoke.CreateDIBSection(IntPtr.Zero, mm.Handle, (uint)DIB_RGB_COLORS, ref pPixels, IntPtr.Zero, 0);
            bitPtr = pPixels;
            var bm = new System.Drawing.Imaging.BitmapData();
            bm = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppPArgb, bm);
            var pCurrSource = bm.Scan0;

            // ' Our DIBsection is bottom-up...start at the bottom row...
            var pCurrDest = pPixels + (img.Width - 1) * BytesPerRow;
            // ' ... and work our way up
            int DestinationStride = -BytesPerRow;
            for (int curY = 0, loopTo = img.Height - 1; curY <= loopTo; curY++)
            {
                CoreCT.Memory.NativeLib.Native.MemCpy(pCurrSource, pCurrDest, BytesPerRow);
                pCurrSource = pCurrSource + bm.Stride;
                pCurrDest = pCurrDest + DestinationStride;
            }

            // Free up locked buffer.
            img.UnlockBits(bm);
            return hPreviewBitmap;
        }

        /// <summary>
        /// Highlight the specified icon with the specified color.
        /// </summary>
        /// <param name="icn">Input icon.</param>
        /// <param name="liteColor">Highlight base color.</param>
        /// <returns>A new highlighted Image object.</returns>
        /// <remarks></remarks>
        public static Image HiliteIcon(Icon icn, Color liteColor)
        {
            var n = new Bitmap(icn.Width, icn.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var g = Graphics.FromImage(n);
            int lc = liteColor.ToArgb();
            g.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, n.Width, n.Height));
            g.DrawIcon(icn, 0, 0);
            g.Dispose();
            var bm = new System.Drawing.Imaging.BitmapData();
            n.LockBits(new Rectangle(0, 0, n.Width, n.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb, bm);
            int[] b;
            int i;
            int c;
            b = new int[(bm.Width * bm.Height)];

            // ' take the unmanaged memory and make it something manageable and VB-like.

            Marshal.Copy(bm.Scan0, b, 0, bm.Stride * bm.Height);
            // NativeLib.Native.MemCpy(bm.Scan0, b, bm.Stride * bm.Height)

            c = b.Length - 1;
            int stp = (int)(bm.Stride / (double)bm.Width);
            var hsv = new ColorStructs.HSVDATA();
            var hsv2 = new ColorStructs.HSVDATA();

            // convert the color to HSV.
            ColorMath.ColorToHSV(liteColor, ref hsv);
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                if (b[i] == 0)
                    continue;
                ColorMath.ColorToHSV(Color.FromArgb(b[i]), ref hsv2);
                hsv2.Hue = hsv.Hue;
                hsv2.Saturation = hsv.Saturation;
                hsv2.Value *= 1.1d;
                b[i] = ColorMath.HSVToColor(hsv2);
            }

            Marshal.Copy(b, 0, bm.Scan0, bm.Stride * bm.Height);
            n.UnlockBits(bm);
            return n;
        }

        /// <summary>
        /// Converts a 32 bit icon into a 32 bit Argb transparent bitmap.
        /// </summary>
        /// <param name="icn">Input icon.</param>
        /// <returns>A 32-bit Argb Bitmap object.</returns>
        /// <remarks></remarks>
        public static Bitmap IconToTransparentBitmap(Icon icn)
        {
            if (icn is null)
                return null;
            var n = new Bitmap(icn.Width, icn.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var g = Graphics.FromImage(n);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            g.Clear(Color.Transparent);
            g.DrawIcon(icn, 0, 0);
            g.Dispose();
            return n;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }
}