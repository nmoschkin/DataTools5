// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: NativeShell
//         Wrappers for native and COM shell interfaces.
//
// Some enum documentation copied from the MSDN (and in some cases, updated).
// Some classes and interfaces were ported from the WindowsAPICodePack.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the MIT License   
// ************************************************* ''

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
    public sealed class ShellBHIDGuid
    {
        public const string ShellFolderObject = "3981e224-f559-11d3-8e3a-00c04f6837d5";
        public const string ShellFolderUIObject = "3981e225-f559-11d3-8e3a-00c04f6837d5";
        public const string ShellFolderViewObject = "3981e226-f559-11d3-8e3a-00c04f6837d5";
        public const string Storage = "3981e227-f559-11d3-8e3a-00c04f6837d5";
        public const string Stream = "1cebb3ab-7c10-499a-a417-92ca16c4cb83";
        public const string RandomAccessStream = "f16fc93b-77ae-4cfe-bda7-a866eea6878d";
        public const string LinkTargetItem = "3981e228-f559-11d3-8e3a-00c04f6837d5";
        public const string StorageEnum = "4621a4e3-f0d6-4773-8a9c-46e77b174840";
        public const string Transfer = "d5e346a1-f753-4932-b403-4574800e2498";
        public const string PropertyStore = "0384e1a4-1523-439c-a4c8-ab911052f586";
        public const string ThumbnailHandler = "7b2e650a-8e20-4f4a-b09e-6597afc72fb0";
        public const string EnumItems = "94f60519-2850-4924-aa5a-d15e84868039";
        public const string DataObject = "b8c0bd9f-ed24-455c-83e6-d5390c4fe8c4";
        public const string AssociationArray = "bea9ef17-82f1-4f60-9284-4f8db75c3be9";
        public const string Filter = "38d08778-f557-4690-9ebf-ba54706ad8f7";
        public const string EnumAssocHandlers = "b8ab0b9c-c2ec-4f7a-918d-314900e6280a";
        public const string FilePlaceholder = "8677dceb-aae0-4005-8d3d-547fa852f825";

    }
}
