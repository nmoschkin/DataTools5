using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

using DataTools.Shell.Native;

using stdole;

namespace DataTools.Win32.Media
{
    public static class Media32
    {

        [DllImport("mf.dll")]
        static extern HResult MFGetService(
          object punkObject,
          ref Guid guidService,
          ref Guid riid,
          out object ppvObject
        );


        [DllImport("mf.dll")]
        static extern HResult MFCreateSourceResolver(
            out IMFSourceResolver ppISourceResolver
        );

    }

    internal enum MF_OBJECT_TYPE
    {
        MF_OBJECT_MEDIASOURCE = 0,
        MF_OBJECT_BYTESTREAM = (MF_OBJECT_MEDIASOURCE + 1),
        MF_OBJECT_INVALID = (MF_OBJECT_BYTESTREAM + 1)
    }

    internal enum MFBYTESTREAM_SEEK_ORIGIN
    {
        msoBegin = 0,
        msoCurrent = (msoBegin + 1)
    }


    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("ac6b7889-0740-4d51-8619-905994a55cc6")]
    internal interface IMFAsyncResult : IUnknown
    {
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public HResult GetState(
            /* [out] */ out IUnknown ppunkState);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public HResult GetStatus();

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public HResult SetStatus(
            /* [in] */ HResult hrStatus);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public HResult GetObject(
            /* [out] */ out IUnknown ppObject);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public /* [local] */ IUnknown GetStateNoAddRef();
        
    }


    [Guid("a27003cf-2354-4f2a-8d6a-ab7cff15437e")]
    internal interface IMFAsyncCallback : IUnknown
    {
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public HResult GetParameters(
            /* [out] */ out int pdwFlags,
            /* [out] */ out int pdwQueue);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public HResult Invoke(
            /* [in] */ IMFAsyncResult pAsyncResult);
        
    };



    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("ad4c1b00-4bf7-422f-9175-756693d9130d")]
    internal interface IMFByteStream : IUnknown
    {

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public HResult GetCapabilities(
            /* [out] */ out int pdwCapabilities);
        
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public HResult GetLength(
            /* [out] */ out long pqwLength);
        
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public HResult SetLength(
            /* [in] */ long qwLength);
        
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public HResult GetCurrentPosition(
            /* [out] */ out long pqwPosition);
        
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public HResult SetCurrentPosition(
            /* [in] */ long qwPosition);
        
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public HResult IsEndOfStream(
            /* [out] */ out bool pfEndOfStream);
        
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public HResult Read(
            /* [size_is][out] */ byte[] pb,
            /* [in] */ int cb,
            /* [out] */ out int pcbRead);
        
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public /* [local] */ HResult BeginRead(
            /* [annotation][out] */
            byte[] pb,
            /* [in] */ int cb,
            /* [in] */ IMFAsyncCallback pCallback,
            /* [in] */ IUnknown punkState);
        
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public /* [local] */ HResult EndRead(
            /* [in] */ IMFAsyncResult pResult,
            /* [annotation][out] */
            out int pcbRead);
        
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public HResult Write(
            /* [size_is][in] */ byte[] pb,
            /* [in] */ int cb,
            /* [out] */ out int pcbWritten);
        
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public /* [local] */ HResult BeginWrite(
            /* [annotation][in] */
            byte[] pb,
            /* [in] */ int cb,
            /* [in] */ IMFAsyncCallback pCallback,
            /* [in] */ IUnknown punkState);
        
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public /* [local] */ HResult EndWrite(
            /* [in] */ IMFAsyncResult pResult,
            /* [annotation][out] */
            out int pcbWritten);
        
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public HResult Seek(
            /* [in] */ MFBYTESTREAM_SEEK_ORIGIN SeekOrigin,
            /* [in] */ long llSeekOffset,
            /* [in] */ int dwSeekFlags,
            /* [out] */ out long pqwCurrentPosition);
        
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public HResult Flush();
        
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public HResult Close();
        
    };


    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("FBE5A32D-A497-4b61-BB85-97B1A848A6E3")]

    internal interface IMFSourceResolver : IUnknown
    {
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public /* [local] */ HResult CreateObjectFromURL(
            /* [in] */ string pwszURL,
            /* [in] */ int dwFlags,
            /* [in] */ IPropertyStore pProps,
            /* [annotation][out] */
            out MF_OBJECT_TYPE pObjectType,
            /* [annotation][out] */
            out IUnknown ppObject);


        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public /* [local] */ HResult CreateObjectFromByteStream(
            /* [in] */ IMFByteStream pByteStream,
            /* [in] */ string pwszURL,
            /* [in] */ int dwFlags,
            /* [in] */ IPropertyStore pProps,
            /* [annotation][out] */
            out MF_OBJECT_TYPE pObjectType,
            /* [annotation][out] */
            out IUnknown ppObject);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public /* [local] */ HResult BeginCreateObjectFromURL(
            /* [in] */ string pwszURL,
            /* [in] */ int dwFlags,
            /* [in] */ IPropertyStore pProps,
            /* [annotation][out] */
            out IUnknown ppIUnknownCancelCookie,
            /* [in] */ IMFAsyncCallback pCallback,
            /* [in] */ IUnknown punkState);
        
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public /* [local] */ HResult EndCreateObjectFromURL(
            /* [in] */ IMFAsyncResult pResult,
            /* [annotation][out] */
            out MF_OBJECT_TYPE pObjectType,
            /* [annotation][out] */
            out IUnknown ppObject);
        
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public /* [local] */ HResult BeginCreateObjectFromByteStream(
            /* [in] */ IMFByteStream pByteStream,
            /* [in] */ string pwszURL,
            /* [in] */ int dwFlags,
            /* [in] */ IPropertyStore pProps,
            /* [annotation][out] */
            out IUnknown ppIUnknownCancelCookie,
            /* [in] */ IMFAsyncCallback pCallback,
            /* [in] */ IUnknown punkState);
        
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public /* [local] */ HResult EndCreateObjectFromByteStream(
            /* [in] */ IMFAsyncResult pResult,
            /* [annotation][out] */
            out MF_OBJECT_TYPE pObjectType,
            /* [annotation][out] */
            out IUnknown ppObject);
        
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        public /* [local] */ HResult CancelObjectCreation(
            /* [in] */ IUnknown pIUnknownCancelCookie);
        
    };
    

}
