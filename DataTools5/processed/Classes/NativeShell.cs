// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library - Interop
// '
// ' Module: NativeShell
// '         Wrappers for native and COM shell interfaces.
// '
// ' Some enum documentation copied from the MSDN (and in some cases, updated).
// ' Some classes and interfaces were ported from the WindowsAPICodePack.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

//using DataTools.Hardware.MessageResources;
//using DataTools.Hardware;
//using DataTools.Hardware.Native;

namespace DataTools.Shell.Native
{
    #endregion
    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    internal static class NativeShell
    {

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int SHCreateShellItemArrayFromDataObject(IDataObject pdo, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] ref IShellItemArray iShellItemArray);

        // The following parameter is not used - binding context.
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern HResult SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string path, IntPtr pbc, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] ref IShellItem2 shellItem);

        // The following parameter is not used - binding context.
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern HResult SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string path, IntPtr pbc, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] ref IShellItem shellItem);
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern HResult PathParseIconLocation([MarshalAs(UnmanagedType.LPWStr)] ref string pszIconFile);
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern HResult SHCreateItemWithParent(IntPtr pidlParaent, IShellFolder psfParent, IntPtr pidl, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] ref object ppvItem);

        // PCIDLIST_ABSOLUTE
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int SHCreateItemFromIDList(IntPtr pidl, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] ref IShellItem2 ppv);
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = true)]
        internal static extern HResult SHParseDisplayName([In][MarshalAs(UnmanagedType.LPWStr)] string pszName, [In] IntPtr pbc, out IntPtr ppidl, [In] ShellFileGetAttributesOptions sfgaoIn, out ShellFileGetAttributesOptions psfgaoOut);
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern HResult SHParseDisplayName(IntPtr pszName, IntPtr pbc, out IntPtr ppidl, ShellFileGetAttributesOptions sfgaoIn, ref ShellFileGetAttributesOptions psfgaoOut);
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool SHObjectProperties(IntPtr hwnd, SHOPType shopObjectType, string pszObjectName, string pszPropertyPage);
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern HResult SHBindToObject(IShellFolder psf, IntPtr pidl, IntPtr pbc, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] ref object ppv);
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int SHGetIDListFromObject(IntPtr iUnknown, ref IntPtr ppidl);
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int SHGetDesktopFolder([MarshalAs(UnmanagedType.Interface)] ref IShellFolder ppshf);
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int SHCreateShellItem(IntPtr pidlParent, [In][MarshalAs(UnmanagedType.Interface)] IShellFolder psfParent, IntPtr pidl, [MarshalAs(UnmanagedType.Interface)] ref IShellItem ppsi);
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern uint ILGetSize(IntPtr pidl);
        [DllImport("shell32.dll", CharSet = CharSet.None)]
        internal static extern void ILFree(IntPtr pidl);
        [DllImport("gdi32.dll")]
        internal static extern bool DeleteObject(IntPtr hObject);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        public const int InPlaceStringTruncated = 0x401A0;

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("Shell32", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        internal static extern int SHShowManageLibraryUI([In][MarshalAs(UnmanagedType.Interface)] IShellItem library, [In] IntPtr hwndOwner, [In] string title, [In] string instruction, [In] LibraryManageDialogOptions lmdOptions);

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public const int CommandLink = 0xE;
        public const uint SetNote = 0x1609U;
        public const uint GetNote = 0x160AU;
        public const uint GetNoteLength = 0x160BU;
        public const uint SetShield = 0x160CU;
        internal const int MAX_PATH = 260;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public const int MaxPath = 260;

        [DllImport("shell32.dll")]
        public static extern bool SHGetPathFromIDListW(IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszPath);

        public enum STRRET_TYPE : uint
        {
            STRRET_WSTR = 0U,
            STRRET_OFFSET = 1U,
            STRRET_CSTR = 2U
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct STRRET
        {

            // <FieldOffset(0)>
            public STRRET_TYPE uType;

            // Public pOleStr As IntPtr

            // <FieldOffset(4), MarshalAs(UnmanagedType.LPWStr)>
            // Public uOffset As UInteger

            // <FieldOffset(4)>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_PATH)]
            public char[] _cStr;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ShellNotifyStruct
        {
            public IntPtr item1;
            public IntPtr item2;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SHChangeNotifyEntry
        {
            public IntPtr pIdl;
            [MarshalAs(UnmanagedType.Bool)]
            public bool recursively;
        }

        [DllImport("shell32.dll")]
        public static extern uint SHChangeNotifyRegister(IntPtr windowHandle, ShellChangeNotifyEventSource sources, ShellObjectChangeTypes events, uint message, int entries, ref SHChangeNotifyEntry changeNotifyEntry);
        [DllImport("shell32.dll")]
        public static extern IntPtr SHChangeNotification_Lock(IntPtr windowHandle, int processId, ref IntPtr pidl, ref uint lEvent);
        [DllImport("shell32.dll")]
        public static extern bool SHChangeNotification_Unlock(IntPtr hLock);
        [DllImport("shell32.dll")]
        public static extern bool SHChangeNotifyDeregister(uint hNotify);

        [Flags]
        public enum ShellChangeNotifyEventSource
        {
            InterruptLevel = 0x1,
            ShellLevel = 0x2,
            RecursiveInterrupt = 0x1000,
            NewDelivery = 0x8000
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        public enum ASSOC_FILTER
        {
            NONE = 0,
            RECOMMENDED = 1
        }

        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        public static extern HResult SHAssocEnumHandlers([MarshalAs(UnmanagedType.LPWStr)][In] string pszExtra, [In] ASSOC_FILTER afFilter, out IEnumAssocHandlers ppEnumHandler);
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr Shell_GetCachedImageIndex([MarshalAs(UnmanagedType.LPWStr)] string pwszIconPath, int iIconIndex, uint uIconFlags);

        public static List<KeyValuePair<string, IAssocHandler[]>> HandlerCache = new List<KeyValuePair<string, IAssocHandler[]>>();

        public static void ClearHandlerCache()
        {
            HandlerCache.Clear();
        }

        public static IAssocHandler[] FindInCache(string fileExtension)
        {
            foreach (var kv in HandlerCache)
            {
                if ((kv.Key ?? "") == (fileExtension ?? ""))
                    return kv.Value;
            }

            return null;
        }

        public static void AddToCache(string fileExtension, IAssocHandler[] assoc)
        {
            var kv = new KeyValuePair<string, IAssocHandler[]>(fileExtension, assoc);
            HandlerCache.Add(kv);
        }

        private static List<IAssocHandler> lAssoc = new List<IAssocHandler>();

        public static IAssocHandler[] EnumFileHandlers(string fileExtension, bool flush = false)
        {
            var assoc = flush ? null : FindInCache(fileExtension);

            if (assoc is object)
                return assoc;

            if (lAssoc.Count > 0)
                lAssoc.Clear();
        
            IAssocHandler h = null;
            IEnumAssocHandlers handlers = null;

            uint cret;

            try
            {
                NativeShell.SHAssocEnumHandlers(fileExtension, ASSOC_FILTER.RECOMMENDED, out handlers);
            }
            catch 
            {
                return null;
            }

            do
            {
                handlers.Next(1U, out h, out cret);
                if (h is object)
                {
                    lAssoc.Add(h);
                }
            }
            while (cret > 0L);
            
            assoc = lAssoc.ToArray();
            AddToCache(fileExtension, assoc);

            return assoc;
        }
    }
}
