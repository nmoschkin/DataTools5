using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;

using CoreCT.Shell32.Structs;
using CoreCT.Shell32.Enums;
using CoreCT.Shell32.ShellID;
using CoreCT.Shell32.PropertyStore;
using System.Drawing;
using System.Text;

namespace CoreCT.Shell32.Interfaces
{

    [ComImport]
    [Guid(ShellIIDGuid.IModalWindow)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IModalWindow
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        extern int Show([In] IntPtr parent);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IShellItem)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellItem
    {
        // Not supported: IBindCtx.
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult BindToHandler([In] IntPtr pbc, [In] ref Guid bhid, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetParent([MarshalAs(UnmanagedType.Interface)] ref IShellItem ppsi);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetDisplayName([In] ShellItemDesignNameOptions sigdnName, ref IntPtr ppszName);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetAttributes([In] ShellFileGetAttributesOptions sfgaoMask, ref ShellFileGetAttributesOptions psfgaoAttribs);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult Compare([In][MarshalAs(UnmanagedType.Interface)] IShellItem psi, [In] SICHINTF hint, ref int piOrder);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IShellItem2)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IShellItem2 : IShellItem
    {
        // Not supported: IBindCtx.
        // <PreserveSig> _
        // <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        // Function BindToHandler(<[In]> pbc As IntPtr, <[In]> ByRef bhid As Guid, <[In]> ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IShellFolder) As HResult

        // <PreserveSig> _
        // <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        // Function GetParent(<MarshalAs(UnmanagedType.[Interface])> ByRef ppsi As IShellItem) As HResult

        // <PreserveSig> _
        // <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        // Function GetDisplayName(<[In]> sigdnName As ShellItemDesignNameOptions, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppszName As String) As HResult

        // <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        // Sub GetAttributes(<[In]> sfgaoMask As ShellFileGetAttributesOptions, ByRef psfgaoAttribs As ShellFileGetAttributesOptions)

        // <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        // Sub Compare(<[In], MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, <[In]> hint As UInteger, ByRef piOrder As Integer)

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        extern int GetPropertyStore([In] GetPropertyStoreOptions Flags, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IPropertyStore ppv);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetPropertyStoreWithCreateObject([In] GetPropertyStoreOptions Flags, [In][MarshalAs(UnmanagedType.IUnknown)] object punkCreateObject, [In] ref Guid riid, ref IntPtr ppv);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetPropertyStoreForKeys([In] ref PropertyKey rgKeys, [In] uint cKeys, [In] GetPropertyStoreOptions Flags, [In] ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out IPropertyStore ppv);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetPropertyDescriptionList([In] ref PropertyKey keyType, [In] ref Guid riid, ref IntPtr ppv);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult Update([In][MarshalAs(UnmanagedType.Interface)] IBindCtx pbc);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetProperty([In] ref PropertyKey key, out PropVariant ppropvar);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetCLSID([In] ref PropertyKey key, ref Guid pclsid);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetFileTime([In] ref PropertyKey key, ref FILETIME pft);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetInt32([In] ref PropertyKey key, ref int pi);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetString([In] ref PropertyKey key, [MarshalAs(UnmanagedType.LPWStr)] ref string ppsz);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetUInt32([In] ref PropertyKey key, ref uint pui);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetUInt64([In] ref PropertyKey key, ref ulong pull);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetBool([In] ref PropertyKey key, ref int pf);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IShellItemArray)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellItemArray
    {
        // Not supported: IBindCtx.
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult BindToHandler([In][MarshalAs(UnmanagedType.Interface)] IntPtr pbc, [In] ref Guid rbhid, [In] ref Guid riid, ref IntPtr ppvOut);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetPropertyStore([In] int Flags, [In] ref Guid riid, ref IntPtr ppv);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetPropertyDescriptionList([In] ref PropertyKey keyType, [In] ref Guid riid, ref IntPtr ppv);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetAttributes([In] ShellItemAttributeOptions dwAttribFlags, [In] ShellFileGetAttributesOptions sfgaoMask, ref ShellFileGetAttributesOptions psfgaoAttribs);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetCount(ref uint pdwNumItems);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetItemAt([In] uint dwIndex, [MarshalAs(UnmanagedType.Interface)] ref IShellItem ppsi);

        // Not supported: IEnumShellItems (will use GetCount and GetItemAt instead).
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult EnumItems([MarshalAs(UnmanagedType.Interface)] ref IntPtr ppenumShellItems);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IShellLibrary)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IShellLibrary
    {
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult LoadLibraryFromItem([In][MarshalAs(UnmanagedType.Interface)] IShellItem library, [In] AccessModes grfMode);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void LoadLibraryFromKnownFolder([In] ref Guid knownfidLibrary, [In] AccessModes grfMode);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void AddFolder([In][MarshalAs(UnmanagedType.Interface)] IShellItem location);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void RemoveFolder([In][MarshalAs(UnmanagedType.Interface)] IShellItem location);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetFolders([In] LibraryFolderFilter lff, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] ref IShellItemArray ppv);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void ResolveFolder([In][MarshalAs(UnmanagedType.Interface)] IShellItem folderToResolve, [In] uint timeout, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] ref IShellItem ppv);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetDefaultSaveFolder([In] DefaultSaveFolderType dsft, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] ref IShellItem ppv);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void SetDefaultSaveFolder([In] DefaultSaveFolderType dsft, [In][MarshalAs(UnmanagedType.Interface)] IShellItem si);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetOptions(ref LibraryOptions lofOptions);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void SetOptions([In] LibraryOptions lofMask, [In] LibraryOptions lofOptions);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetFolderType(ref Guid ftid);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void SetFolderType([In] ref Guid ftid);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetIcon([MarshalAs(UnmanagedType.LPWStr)] ref string icon);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void SetIcon([In][MarshalAs(UnmanagedType.LPWStr)] string icon);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void Commit();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void Save([In][MarshalAs(UnmanagedType.Interface)] IShellItem folderToSaveIn, [In][MarshalAs(UnmanagedType.LPWStr)] string libraryName, [In] LibrarySaveOptions lsf, [MarshalAs(UnmanagedType.Interface)] ref IShellItem2 savedTo);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void SaveInKnownFolder([In] ref Guid kfidToSaveIn, [In][MarshalAs(UnmanagedType.LPWStr)] string libraryName, [In] LibrarySaveOptions lsf, [MarshalAs(UnmanagedType.Interface)] ref IShellItem2 savedTo);
    }

    [ComImport]
    [Guid("bcc18b79-ba16-442f-80c4-8a59c30c463b")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IShellItemImageFactory
    {
        [PreserveSig]
        HResult GetImage([In][MarshalAs(UnmanagedType.Struct)] Size size, [In] SIIGBF flags, out IntPtr phbm);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IThumbnailCache)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IThumbnailCache
    {
        void GetThumbnail([In] IShellItem pShellItem, [In] uint cxyRequestedThumbSize, [In] ThumbnailOptions flags, out ISharedBitmap ppvThumb, out ThumbnailCacheOptions pOutFlags, out ThumbnailId pThumbnailID);
        void GetThumbnailByID([In] ThumbnailId thumbnailID, [In] uint cxyRequestedThumbSize, out ISharedBitmap ppvThumb, out ThumbnailCacheOptions pOutFlags);
    }

    [ComImport]
    [Guid(ShellIIDGuid.ISharedBitmap)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface ISharedBitmap
    {
        void GetSharedBitmap(out IntPtr phbm);
        void GetSize(out Size pSize);
        void GetFormat(out ThumbnailAlphaType pat);
        void InitializeBitmap([In] IntPtr hbm, [In] ThumbnailAlphaType wtsAT);
        void Detach(out IntPtr phbm);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IShellFolder)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComConversionLoss]
    public interface IShellFolder
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void ParseDisplayName(IntPtr hwnd, [In][MarshalAs(UnmanagedType.Interface)] ref IBindCtx pbc, [In][MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, [In] ref uint pchEaten, out IntPtr ppidl, [In] ref uint pdwAttributes);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult EnumObjects([In] IntPtr hwnd, [In] ShellFolderEnumerationOptions grfFlags, [MarshalAs(UnmanagedType.Interface)] ref IEnumIDList ppenumIDList);

        // [In, MarshalAs(UnmanagedType.Interface)] IBindCtx
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult BindToObject([In] IntPtr pidl, IntPtr pbc, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IShellFolder ppv);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void BindToStorage([In] ref IntPtr pidl, [In][MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, [In] ref Guid riid, ref IntPtr ppv);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void CompareIDs([In] IntPtr lParam, [In] ref IntPtr pidl1, [In] ref IntPtr pidl2);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void CreateViewObject([In] IntPtr hwndOwner, [In] ref Guid riid, ref IntPtr ppv);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetAttributesOf([In] uint cidl, [In] IntPtr apidl, [In] ref uint rgfInOut);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetUIObjectOf([In] IntPtr hwndOwner, [In] uint cidl, [In] IntPtr apidl, [In] ref Guid riid, [In] ref uint rgfReserved, ref IntPtr ppv);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetDisplayNameOf([In] IntPtr pidl, [In] ShellItemDesignNameOptions uFlags, IntPtr pName);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void SetNameOf([In] IntPtr hwnd, [In] ref IntPtr pidl, [In][MarshalAs(UnmanagedType.LPWStr)] string pszName, [In] uint uFlags, out IntPtr ppidlOut);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IShellFolder2)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComConversionLoss]
    public interface IShellFolder2 : IShellFolder
    {
        // <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        // Sub ParseDisplayName(<[In]> hwnd As IntPtr, <[In], MarshalAs(UnmanagedType.[Interface])> pbc As IBindCtx, <[In], MarshalAs(UnmanagedType.LPWStr)> pszDisplayName As String, <[In], Out> ByRef pchEaten As UInteger, <Out> ppidl As IntPtr, <[In], Out> ByRef pdwAttributes As UInteger)

        // <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        // Sub EnumObjects(<[In]> hwnd As IntPtr, <[In]> grfFlags As ShellFolderEnumerationOptions, <MarshalAs(UnmanagedType.[Interface])> ByRef ppenumIDList As IEnumIDList)

        // '[In, MarshalAs(UnmanagedType.Interface)] IBindCtx
        // <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        // Sub BindToObject(<[In]> pidl As IntPtr, pbc As IntPtr, <[In]> ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IShellFolder)

        // <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        // Sub BindToStorage(<[In]> ByRef pidl As IntPtr, <[In], MarshalAs(UnmanagedType.[Interface])> pbc As IBindCtx, <[In]> ByRef riid As Guid, ByRef ppv As IntPtr)

        // <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        // Sub CompareIDs(<[In]> lParam As IntPtr, <[In]> ByRef pidl1 As IntPtr, <[In]> ByRef pidl2 As IntPtr)

        // <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        // Sub CreateViewObject(<[In]> hwndOwner As IntPtr, <[In]> ByRef riid As Guid, ByRef ppv As IntPtr)

        // <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        // Sub GetAttributesOf(<[In]> cidl As UInteger, <[In]> apidl As IntPtr, <[In], Out> ByRef rgfInOut As UInteger)

        // <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        // Sub GetUIObjectOf(<[In]> hwndOwner As IntPtr, <[In]> cidl As UInteger, <[In]> apidl As IntPtr, <[In]> ByRef riid As Guid, <[In], Out> ByRef rgfReserved As UInteger, ByRef ppv As IntPtr)

        // <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        // Sub GetDisplayNameOf(<[In]> ByRef pidl As IntPtr, <[In]> uFlags As UInteger, ByRef pName As IntPtr)

        // <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        // Sub SetNameOf(<[In]> hwnd As IntPtr, <[In]> ByRef pidl As IntPtr, <[In], MarshalAs(UnmanagedType.LPWStr)> pszName As String, <[In]> uFlags As UInteger, <Out> ppidlOut As IntPtr)

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetDefaultSearchGUID(ref Guid pguid);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void EnumSearches(out IntPtr ppenum);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetDefaultColumn([In] uint dwRes, ref uint pSort, ref uint pDisplay);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetDefaultColumnState([In] uint iColumn, ref uint pcsFlags);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetDetailsEx([In] ref IntPtr pidl, [In] ref PropertyKey pscid, [MarshalAs(UnmanagedType.Struct)] ref object pv);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetDetailsOf([In] ref IntPtr pidl, [In] uint iColumn, ref IntPtr psd);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void MapColumnToSCID([In] uint iColumn, ref PropertyKey pscid);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IEnumIDList)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumIDList
    {
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult Next(uint celt, ref IntPtr rgelt, ref uint pceltFetched);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult Skip([In] uint celt);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult Reset();
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult Clone([MarshalAs(UnmanagedType.Interface)] ref IEnumIDList ppenum);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IShellLinkW)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellLinkW
    {
        // ref _WIN32_FIND_DATAW pfd,
        void GetPath([MarshalAs(UnmanagedType.LPWStr)] out StringBuilder pszFile, int cchMaxPath, IntPtr pfd, uint fFlags);
        void GetIDList(ref IntPtr ppidl);
        void SetIDList(IntPtr pidl);
        void GetDescription([MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxName);
        void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
        void GetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
        void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
        void GetArguments([MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
        void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
        void GetHotKey(ref short wHotKey);
        void SetHotKey(short wHotKey);
        void GetShowCmd(ref uint iShowCmd);
        void SetShowCmd(uint iShowCmd);
        void GetIconLocation([MarshalAs(UnmanagedType.LPWStr)] out StringBuilder pszIconPath, int cchIconPath, ref int iIcon);
        void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
        void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, uint dwReserved);
        void Resolve(IntPtr hwnd, uint fFlags);
        void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
    }

    [ComImport]
    [Guid(ShellIIDGuid.CShellLink)]
    [ClassInterface(ClassInterfaceType.None)]
    public class CShellLink
    {
    }

    // Summary:
    // Provides the managed definition of the IPersistStream interface, with functionality
    // from IPersist.
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00000109-0000-0000-C000-000000000046")]
    public interface IPersistStream
    {
        // Summary:
        // Retrieves the class identifier (CLSID) of an object.
        // 
        // Parameters:
        // pClassID:
        // When this method returns, contains a reference to the CLSID. This parameter
        // is passed uninitialized.
        [PreserveSig]
        void GetClassID(ref Guid pClassID);
        // 
        // Summary:
        // Checks an object for changes since it was last saved to its current file.
        // 
        // Returns:
        // S_OK if the file has changed since it was last saved; S_FALSE if the file
        // has not changed since it was last saved.
        [PreserveSig]
        HResult IsDirty();
        [PreserveSig]
        HResult Load([In][MarshalAs(UnmanagedType.Interface)] IStream stm);
        [PreserveSig]
        HResult Save([In][MarshalAs(UnmanagedType.Interface)] IStream stm, bool fRemember);
        [PreserveSig]
        HResult GetSizeMax(ref ulong cbSize);
    }

    [ComImport]
    [Guid(ShellIIDGuid.ICondition)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICondition : IPersistStream
    {
        // Summary:
        // Retrieves the class identifier (CLSID) of an object.
        // 
        // Parameters:
        // pClassID:
        // When this method returns, contains a reference to the CLSID. This parameter
        // is passed uninitialized.
        // 
        // Summary:
        // Checks an object for changes since it was last saved to its current file.
        // 
        // Returns:
        // S_OK if the file has changed since it was last saved; S_FALSE if the file
        // has not changed since it was last saved.

        // For any node, return what kind of node it is.
        [PreserveSig]
        HResult GetConditionType(out SearchConditionType pNodeType);

        // riid must be IID_IEnumUnknown, IID_IEnumVARIANT or IID_IObjectArray, or in the case of a negation node IID_ICondition.
        // If this is a leaf node, E_FAIL will be returned.
        // If this is a negation node, then if riid is IID_ICondition, *ppv will be set to a single ICondition, otherwise an enumeration of one.
        // If this is a conjunction or a disjunction, *ppv will be set to an enumeration of the subconditions.
        [PreserveSig]
        HResult GetSubConditions([In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

        // If this is not a leaf node, E_FAIL will be returned.
        // Retrieve the property name, operation and value from the leaf node.
        // Any one of ppszPropertyName, pcop and ppropvar may be NULL.
        [PreserveSig]
        HResult GetComparisonInfo([MarshalAs(UnmanagedType.LPWStr)] out string ppszPropertyName, out SearchConditionOperation pcop, out PropVariant ppropvar);

        // If this is not a leaf node, E_FAIL will be returned.
        // *ppszValueTypeName will be set to the semantic type of the value, or to NULL if this is not meaningful.
        [PreserveSig]
        HResult GetValueType([MarshalAs(UnmanagedType.LPWStr)] out string ppszValueTypeName);

        // If this is not a leaf node, E_FAIL will be returned.
        // If the value of the leaf node is VT_EMPTY, *ppszNormalization will be set to an empty string.
        // If the value is a string (VT_LPWSTR, VT_BSTR or VT_LPSTR), then *ppszNormalization will be set to a
        // character-normalized form of the value.
        // Otherwise, *ppszNormalization will be set to some (character-normalized) string representation of the value.
        [PreserveSig]
        HResult GetValueNormalization([MarshalAs(UnmanagedType.LPWStr)] out string ppszNormalization);

        // Return information about what parts of the input produced the property, the operation and the value.
        // Any one of ppPropertyTerm, ppOperationTerm and ppValueTerm may be NULL.
        // For a leaf node returned by the parser, the position information of each IRichChunk identifies the tokens that
        // contributed the property/operation/value, the string value is the corresponding part of the input string, and
        // the PROPVARIANT is VT_EMPTY.
        [PreserveSig]
        HResult GetInputTerms(out IRichChunk ppPropertyTerm, out IRichChunk ppOperationTerm, out IRichChunk ppValueTerm);

        // Make a deep copy of this ICondition.
        [PreserveSig]
        HResult Clone(out ICondition ppc);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IRichChunk)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IRichChunk
    {
        // The position *pFirstPos is zero-based.
        // Any one of pFirstPos, pLength, ppsz and pValue may be NULL.
        // [out, annotation("__out_opt")] ULONG* pFirstPos, [out, annotation("__out_opt")] ULONG* pLength, [out, annotation("__deref_opt_out_opt")] LPWSTR* ppsz, [out, annotation("__out_opt")] PROPVARIANT* pValue
        [PreserveSig]
        HResult GetData();
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(ShellIIDGuid.IEnumUnknown)]
    public interface IEnumUnknown
    {
        [PreserveSig]
        HResult Next(uint requestedNumber, ref IntPtr buffer, ref uint fetchedNumber);
        [PreserveSig]
        HResult Skip(uint number);
        [PreserveSig]
        HResult Reset();
        [PreserveSig]
        HResult Clone(ref IEnumUnknown result);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IConditionFactory)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IConditionFactory
    {
        [PreserveSig]
        HResult MakeNot([In] ICondition pcSub, [In] bool fSimplify, out ICondition ppcResult);
        [PreserveSig]
        HResult MakeAndOr([In] SearchConditionType ct, [In] IEnumUnknown peuSubs, [In] bool fSimplify, out ICondition ppcResult);
        [PreserveSig]
        HResult MakeLeaf([In][MarshalAs(UnmanagedType.LPWStr)] string pszPropertyName, [In] SearchConditionOperation cop, [In][MarshalAs(UnmanagedType.LPWStr)] string pszValueType, [In] PropVariant ppropvar, IRichChunk richChunk1, IRichChunk richChunk2, IRichChunk richChunk3, [In] bool fExpand, out ICondition ppcResult);

        // [In] ICondition pc, [In] STRUCTURED_QUERY_RESOLVE_OPTION sqro, [In] ref SYSTEMTIME pstReferenceTime, [Out] out ICondition ppcResolved
        [PreserveSig]
        HResult Resolve();
    }

    [ComImport]
    [Guid(ShellIIDGuid.IConditionFactory)]
    [CoClass(typeof(ConditionFactoryCoClass))]
    public interface INativeConditionFactory : IConditionFactory
    {
    }

    [ComImport]
    [ClassInterface(ClassInterfaceType.None)]
    [TypeLibType(TypeLibTypeFlags.FCanCreate)]
    [Guid(ShellCLSIDGuid.ConditionFactory)]
    public class ConditionFactoryCoClass
    {
    }

    [ComImport]
    [Guid(ShellIIDGuid.ISearchFolderItemFactory)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ISearchFolderItemFactory
    {
        [PreserveSig]
        HResult SetDisplayName([In][MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName);
        [PreserveSig]
        HResult SetFolderTypeID([In] Guid ftid);
        [PreserveSig]
        HResult SetFolderLogicalViewMode([In] FolderLogicalViewMode flvm);
        [PreserveSig]
        HResult SetIconSize([In] int iIconSize);
        [PreserveSig]
        HResult SetVisibleColumns([In] uint cVisibleColumns, [In][MarshalAs(UnmanagedType.LPArray)] PropertyKey[] rgKey);
        [PreserveSig]
        HResult SetSortColumns([In] uint cSortColumns, [In][MarshalAs(UnmanagedType.LPArray)] SortColumn[] rgSortColumns);
        [PreserveSig]
        HResult SetGroupColumn([In] ref PropertyKey keyGroup);
        [PreserveSig]
        HResult SetStacks([In] uint cStackKeys, [In][MarshalAs(UnmanagedType.LPArray)] PropertyKey[] rgStackKeys);
        [PreserveSig]
        HResult SetScope([In][MarshalAs(UnmanagedType.Interface)] IShellItemArray ppv);
        [PreserveSig]
        HResult SetCondition([In] ICondition pCondition);
        [PreserveSig]
        int GetShellItem(ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IShellItem ppv);
        [PreserveSig]
        HResult GetIDList(out IntPtr ppidl);
    }

    [ComImport]
    [Guid(ShellIIDGuid.ISearchFolderItemFactory)]
    [CoClass(typeof(SearchFolderItemFactoryCoClass))]
    public interface INativeSearchFolderItemFactory : ISearchFolderItemFactory
    {
    }

    [ComImport]
    [ClassInterface(ClassInterfaceType.None)]
    [TypeLibType(TypeLibTypeFlags.FCanCreate)]
    [Guid(ShellCLSIDGuid.SearchFolderItemFactory)]
    public class SearchFolderItemFactoryCoClass
    {
    }

    [ComImport]
    [Guid(ShellIIDGuid.IQuerySolution)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IQuerySolution : IConditionFactory
    {

        // Retrieve the condition tree and the "main type" of the solution.
        // ppQueryNode and ppMainType may be NULL.
        [PreserveSig]
        HResult GetQuery([MarshalAs(UnmanagedType.Interface)] out ICondition ppQueryNode, [MarshalAs(UnmanagedType.Interface)] out IEntity ppMainType);

        // Identify parts of the input string not accounted for.
        // Each parse error is represented by an IRichChunk where the position information
        // reflect token counts, the string is NULL and the value is a VT_I4
        // where lVal is from the ParseErrorType enumeration. The valid
        // values for riid are IID_IEnumUnknown and IID_IEnumVARIANT.
        // void** 
        [PreserveSig]
        HResult GetErrors([In] ref Guid riid, out IntPtr ppParseErrors);

        // Report the query string, how it was tokenized and what LCID and word breaker were used (for recognizing keywords).
        // ppszInputString, ppTokens, pLocale and ppWordBreaker may be NULL.
        // ITokenCollection** 
        // IUnknown** 
        [PreserveSig]
        HResult GetLexicalData([MarshalAs(UnmanagedType.LPWStr)] ref string ppszInputString, out IntPtr ppTokens, out uint plcid, out IntPtr ppWordBreaker);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IQueryParser)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IQueryParser
    {
        // Parse parses an input string, producing a query solution.
        // pCustomProperties should be an enumeration of IRichChunk objects, one for each custom property
        // the application has recognized. pCustomProperties may be NULL, equivalent to an empty enumeration.
        // For each IRichChunk, the position information identifies the character span of the custom property,
        // the string value should be the name of an actual property, and the PROPVARIANT is completely ignored.
        [PreserveSig]
        HResult Parse([In][MarshalAs(UnmanagedType.LPWStr)] string pszInputString, [In] IEnumUnknown pCustomProperties, out IQuerySolution ppSolution);

        // Set a single option. See STRUCTURED_QUERY_SINGLE_OPTION above.
        [PreserveSig]
        HResult SetOption([In] StructuredQuerySingleOption option, [In] PropVariant pOptionValue);
        [PreserveSig]
        HResult GetOption([In] StructuredQuerySingleOption option, out PropVariant pOptionValue);

        // Set a multi option. See STRUCTURED_QUERY_MULTIOPTION above.
        [PreserveSig]
        HResult SetMultiOption([In] StructuredQueryMultipleOption option, [In][MarshalAs(UnmanagedType.LPWStr)] string pszOptionKey, [In] PropVariant pOptionValue);

        // Get a schema provider for browsing the currently loaded schema.
        // ISchemaProvider
        [PreserveSig]
        HResult GetSchemaProvider(out IntPtr ppSchemaProvider);

        // Restate a condition as a query string according to the currently selected syntax.
        // The parameter fUseEnglish is reserved for future use; must be FALSE.
        [PreserveSig]
        HResult RestateToString([In] ICondition pCondition, [In] bool fUseEnglish, [MarshalAs(UnmanagedType.LPWStr)] out string ppszQueryString);

        // Parse a condition for a given property. It can be anything that would go after 'PROPERTY:' in an AQS expession.
        [PreserveSig]
        HResult ParsePropertyValue([In][MarshalAs(UnmanagedType.LPWStr)] string pszPropertyName, [In][MarshalAs(UnmanagedType.LPWStr)] string pszInputString, out IQuerySolution ppSolution);

        // Restate a condition for a given property. If the condition contains a leaf with any other property name, or no property name at all,
        // E_INVALIDARG will be returned.
        [PreserveSig]
        HResult RestatePropertyValueToString([In] ICondition pCondition, [In] bool fUseEnglish, [MarshalAs(UnmanagedType.LPWStr)] out string ppszPropertyName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszQueryString);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IQueryParserManager)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IQueryParserManager
    {
        // Create a query parser loaded with the schema for a certain catalog localize to a certain language, and initialized with
        // standard defaults. One valid value for riid is IID_IQueryParser.
        [PreserveSig]
        HResult CreateLoadedParser([In][MarshalAs(UnmanagedType.LPWStr)] string pszCatalog, [In] ushort langidForKeywords, [In] ref Guid riid, out IQueryParser ppQueryParser);

        // In addition to setting AQS/NQS and automatic wildcard for the given query parser, this sets up standard named entity handlers and
        // sets the keyboard locale as locale for word breaking.
        [PreserveSig]
        HResult InitializeOptions([In] bool fUnderstandNQS, [In] bool fAutoWildCard, [In] IQueryParser pQueryParser);

        // Change one of the settings for the query parser manager, such as the name of the schema binary, or the location of the localized and unlocalized
        // schema binaries. By default, the settings point to the schema binaries used by Windows Shell.
        [PreserveSig]
        HResult SetOption([In] QueryParserManagerOption option, [In] PropVariant pOptionValue);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IQueryParserManager)]
    [CoClass(typeof(QueryParserManagerCoClass))]
    internal interface INativeQueryParserManager : IQueryParserManager
    {
    }

    [ComImport]
    [ClassInterface(ClassInterfaceType.None)]
    [TypeLibType(TypeLibTypeFlags.FCanCreate)]
    [Guid(ShellCLSIDGuid.QueryParserManager)]
    internal class QueryParserManagerCoClass
    {
    }

    [ComImport]
    [Guid("24264891-E80B-4fd3-B7CE-4FF2FAE8931F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IEntity
    {
        // 
    }

}
