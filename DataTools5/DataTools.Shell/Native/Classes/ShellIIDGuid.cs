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
// Licensed Under the Microsoft Public License   
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
    //<HideModuleName>
    //Public Module NativeShell

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    // This code is mostly translated from the Windows API Code Pack. I added some IIDs for Windows 8.1
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    public sealed class ShellIIDGuid
    {
        public const string IModalWindow = "B4DB1657-70D7-485E-8E3E-6FCB5A5C1802";
        public const string IFileDialog = "42F85136-DB7E-439C-85F1-E4075D135FC8";
        public const string IFileOpenDialog = "D57C7288-D4AD-4768-BE02-9D969532D960";
        public const string IFileSaveDialog = "84BCCD23-5FDE-4CDB-AEA4-AF64B83D78AB";
        public const string IFileDialogEvents = "973510DB-7D7F-452B-8975-74A85828D354";
        public const string IFileDialogControlEvents = "36116642-D713-4B97-9B83-7484A9D00433";
        public const string IFileDialogCustomize = "E6FDD21A-163F-4975-9C8C-A69F1BA37034";
        public const string IShellItem = "43826D1E-E718-42EE-BC55-A1E261C37BFE";
        public const string IShellItem2 = "7E9FB0D3-919F-4307-AB2E-9B1860310C93";
        public const string IShellItemArray = "B63EA76D-1F85-456F-A19C-48159EFA858B";
        public const string IShellLibrary = "11A66EFA-382E-451A-9234-1E0E12EF3085";
        public const string IThumbnailCache = "F676C15D-596A-4ce2-8234-33996F445DB1";
        public const string ISharedBitmap = "091162a4-bc96-411f-aae8-c5122cd03363";
        public const string IShellFolder = "000214E6-0000-0000-C000-000000000046";
        public const string IShellFolder2 = "93F2F68C-1D1B-11D3-A30E-00C04F79ABD1";
        public const string IEnumIDList = "000214F2-0000-0000-C000-000000000046";
        public const string IShellLinkW = "000214F9-0000-0000-C000-000000000046";
        public const string CShellLink = "00021401-0000-0000-C000-000000000046";
        public const string IPropertyStore = "886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99";
        public const string IPropertyStoreCache = "3017056d-9a91-4e90-937d-746c72abbf4f";
        public const string IPropertyDescription = "6F79D558-3E96-4549-A1D1-7D75D2288814";
        public const string IPropertyDescription2 = "57D2EDED-5062-400E-B107-5DAE79FE57A6";
        public const string IPropertyDescriptionList = "1F9FC1D0-C39B-4B26-817F-011967D3440E";
        public const string IPropertyEnumType = "11E1FBF9-2D56-4A6B-8DB3-7CD193A471F2";
        public const string IPropertyEnumType2 = "9B6E051C-5DDD-4321-9070-FE2ACB55E794";
        public const string IPropertyEnumTypeList = "A99400F4-3D84-4557-94BA-1242FB2CC9A6";
        public const string IPropertyStoreCapabilities = "c8e2d566-186e-4d49-bf41-6909ead56acc";
        public const string ICondition = "0FC988D4-C935-4b97-A973-46282EA175C8";
        public const string ISearchFolderItemFactory = "a0ffbc28-5482-4366-be27-3e81e78e06c2";
        public const string IConditionFactory = "A5EFE073-B16F-474f-9F3E-9F8B497A3E08";
        public const string IRichChunk = "4FDEF69C-DBC9-454e-9910-B34F3C64B510";
        public const string IPersistStream = "00000109-0000-0000-C000-000000000046";
        public const string IPersist = "0000010c-0000-0000-C000-000000000046";
        public const string IEnumUnknown = "00000100-0000-0000-C000-000000000046";
        public const string IQuerySolution = "D6EBC66B-8921-4193-AFDD-A1789FB7FF57";
        public const string IQueryParser = "2EBDEE67-3505-43f8-9946-EA44ABC8E5B0";
        public const string IQueryParserManager = "A879E3C4-AF77-44fb-8F37-EBD1487CF920";
        public const string INotinheritableBitmap = "091162a4-bc96-411f-aae8-c5122cd03363";
        public const string IShellItemImageFactory = "bcc18b79-ba16-442f-80c4-8a59c30c463b";
        public const string IContextMenu = "000214e4-0000-0000-c000-000000000046";
        public const string IContextMenu2 = "000214f4-0000-0000-c000-000000000046";
        public const string IContextMenu3 = "BCFCE0A0-EC17-11D0-8D10-00A0C90F2719";
        public const string IImageList = "46EB5926-582E-4017-9FDF-E8998DAA0950";
    }
}
