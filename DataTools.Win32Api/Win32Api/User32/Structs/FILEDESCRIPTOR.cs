// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: Native
//         Myriad Windows API Declares
//
// Started in 2000 on Windows 98/ME (and then later 2000).
//
// Still kicking in 2014 on Windows 8.1!
// A whole bunch of pInvoke/Const/Declare/Struct and associated utility functions that have been collected over the years.

// Some enum documentation copied from the MSDN (and in some cases, updated).
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using DataTools.Memory;
using DataTools.Win32Api;

namespace DataTools.Win32Api
{
    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct FILEDESCRIPTOR
    {
        public uint dwFlags;
        public Guid clsid;

        public W32SIZE sizel;
        public W32POINT pointl;

        public uint dwFileAttributes;

        public FILETIME ftCreationTime;
        public FILETIME ftLastAccessTime;
        public FILETIME ftLastWriteTime;

        public uint nFileSizeHigh;
        public uint nFileSizeLow;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string cFileName;

        public FILEDESCRIPTOR(string name, ulong size, DateTime createDate, DateTime writeDate)
        {
            cFileName = Path.GetFileName(name);
            nFileSizeLow = (uint)(size & 0xFFFFFFFFUL);
            nFileSizeHigh = (uint)(size >> 32);

            ftLastAccessTime = ftLastWriteTime = ftCreationTime = new FILETIME();

            sizel = new W32SIZE();
            pointl = new W32POINT();
            dwFlags = 0;
            clsid = Guid.Empty;
            dwFileAttributes = 0;

            FileTools.LocalToFileTime(writeDate, ref ftLastWriteTime);

            FileTools.LocalToFileTime(writeDate, ref ftLastAccessTime);

            FileTools.LocalToFileTime(createDate, ref ftCreationTime);
        }

        public override string ToString()
        {
            return cFileName;
        }

        public static implicit operator string(FILEDESCRIPTOR operand)
        {
            return operand.cFileName;
        }

        public static explicit operator FILEDESCRIPTOR(string operand)
        {
            var wf = new WIN32_FIND_DATA();
            IntPtr i;
            i = IO.FindFirstFile(operand, ref wf);
            if ((long)i > -1)
            {
                IO.FindClose(i);
                var fd = new FILEDESCRIPTOR();
                fd.cFileName = wf.cFilename;
                fd.dwFileAttributes = (uint)wf.dwFileAttributes;
                fd.ftCreationTime = wf.ftCreationTime;
                fd.ftLastAccessTime = wf.ftLastAccessTime;
                fd.ftLastWriteTime = wf.ftLastWriteTime;
                fd.nFileSizeHigh = (uint)wf.nFileSizeHigh;
                fd.nFileSizeLow = wf.nFileSizeLow;
                return fd;
            }

            return default;
        }
    }
}
