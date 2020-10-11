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
using DataTools.Interop.Resources;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.Interop.Native
{

    // '<HideModuleName>
    // 'Public Module NativeShell

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    // ' This code is mostly translated from the Windows API Code Pack. I added some IIDs for Windows 8.1
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

    public sealed class ShellCLSIDGuid
    {

        // ' CLSID GUIDs for relevant coclasses.
        public const string FileOpenDialog = "DC1C5A9C-E88A-4DDE-A5A1-60F82A20AEF7";
        public const string FileSaveDialog = "C0B4E2F3-BA21-4773-8DBA-335EC946EB8B";
        public const string KnownFolderManager = "4DF0C730-DF9D-4AE3-9153-AA6B82E9795A";
        public const string ShellLibrary = "D9B3211D-E57F-4426-AAEF-30A806ADD397";
        public const string SearchFolderItemFactory = "14010e02-bbbd-41f0-88e3-eda371216584";
        public const string ConditionFactory = "E03E85B0-7BE3-4000-BA98-6C13DE9FA486";
        public const string QueryParserManager = "5088B39A-29B4-4d9d-8245-4EE289222F66";
    }

    public sealed class ShellKFIDGuid
    {
        public const string ComputerFolder = "0AC0837C-BBF8-452A-850D-79D08E667CA7";
        public const string Favorites = "1777F761-68AD-4D8A-87BD-30B759FA33DD";
        public const string Documents = "FDD39AD0-238F-46AF-ADB4-6C85480369C7";
        public const string Profile = "5E6C858F-0E22-4760-9AFE-EA3317B67173";
        public const string GenericLibrary = "5c4f28b5-f869-4e84-8e60-f11db97c5cc7";
        public const string DocumentsLibrary = "7d49d726-3c21-4f05-99aa-fdc2c9474656";
        public const string MusicLibrary = "94d6ddcc-4a68-4175-a374-bd584a510b78";
        public const string PicturesLibrary = "b3690e58-e961-423b-b687-386ebfd83239";
        public const string VideosLibrary = "5fa96407-7e77-483c-ac93-691d05850de8";
        public const string Libraries = "1B3EA5DC-B587-4786-B4EF-BD1DC332AEAE";
    }

    public sealed class ShellBHIDGuid
    {
        public const string ShellFolderObject = "3981e224-f559-11d3-8e3a-00c04f6837d5";
    }

    public sealed class KnownFolderIds
    {

        /// <summary>
        /// Computer
        /// </summary>
        public static readonly Guid Computer = new Guid(0xAC0837C, 0xBBF8, 0x452A, 0x85, 0xD, 0x79, 0xD0, 0x8E, 0x66, 0x7C, 0xA7);

        /// <summary>
        /// Conflicts
        /// </summary>
        public static readonly Guid Conflict = new Guid(0x4BFEFB45, 0x347D, 0x4006, 0xA5, 0xBE, 0xAC, 0xC, 0xB0, 0x56, 0x71, 0x92);

        /// <summary>
        /// Control Panel
        /// </summary>
        public static readonly Guid ControlPanel = new Guid(0x82A74AEB, 0xAEB4, 0x465C, 0xA0, 0x14, 0xD0, 0x97, 0xEE, 0x34, 0x6D, 0x63);

        /// <summary>
        /// Desktop
        /// </summary>
        public static readonly Guid Desktop = new Guid(0xB4BFCC3A, 0xDB2C, 0x424C, 0xB0, 0x29, 0x7F, 0xE9, 0x9A, 0x87, 0xC6, 0x41);

        /// <summary>
        /// Internet Explorer
        /// </summary>
        public static readonly Guid Internet = new Guid(0x4D9F7874, 0x4E0C, 0x4904, 0x96, 0x7B, 0x40, 0xB0, 0xD2, 0xC, 0x3E, 0x4B);

        /// <summary>
        /// Network
        /// </summary>
        public static readonly Guid Network = new Guid(0xD20BEEC4, 0x5CA8, 0x4905, 0xAE, 0x3B, 0xBF, 0x25, 0x1E, 0xA0, 0x9B, 0x53);

        /// <summary>
        /// Printers
        /// </summary>
        public static readonly Guid Printers = new Guid(0x76FC4E2D, 0xD6AD, 0x4519, 0xA6, 0x63, 0x37, 0xBD, 0x56, 0x6, 0x81, 0x85);

        /// <summary>
        /// Sync Center
        /// </summary>
        public static readonly Guid SyncManager = new Guid(0x43668BF8, 0xC14E, 0x49B2, 0x97, 0xC9, 0x74, 0x77, 0x84, 0xD7, 0x84, 0xB7);

        /// <summary>
        /// Network Connections
        /// </summary>
        public static readonly Guid Connections = new Guid(0x6F0CD92B, 0x2E97, 0x45D1, 0x88, 0xFF, 0xB0, 0xD1, 0x86, 0xB8, 0xDE, 0xDD);

        /// <summary>
        /// Sync Setup
        /// </summary>
        public static readonly Guid SyncSetup = new Guid(0xF214138, 0xB1D3, 0x4A90, 0xBB, 0xA9, 0x27, 0xCB, 0xC0, 0xC5, 0x38, 0x9A);

        /// <summary>
        /// Sync Results
        /// </summary>
        public static readonly Guid SyncResults = new Guid(0x289A9A43, 0xBE44, 0x4057, 0xA4, 0x1B, 0x58, 0x7A, 0x76, 0xD7, 0xE7, 0xF9);

        /// <summary>
        /// Recycle Bin
        /// </summary>
        public static readonly Guid RecycleBin = new Guid(0xB7534046, 0x3ECB, 0x4C18, 0xBE, 0x4E, 0x64, 0xCD, 0x4C, 0xB7, 0xD6, 0xAC);

        /// <summary>
        /// Fonts
        /// </summary>
        public static readonly Guid Fonts = new Guid(0xFD228CB7, 0xAE11, 0x4AE3, 0x86, 0x4C, 0x16, 0xF3, 0x91, 0xA, 0xB8, 0xFE);

        /// <summary>
        /// Startup
        /// </summary>
        public static readonly Guid Startup = new Guid(0xB97D20BB, 0xF46A, 0x4C97, 0xBA, 0x10, 0x5E, 0x36, 0x8, 0x43, 0x8, 0x54);

        /// <summary>
        /// Programs
        /// </summary>
        public static readonly Guid Programs = new Guid(0xA77F5D77, 0x2E2B, 0x44C3, 0xA6, 0xA2, 0xAB, 0xA6, 0x1, 0x5, 0x4A, 0x51);

        /// <summary>
        /// Start Menu
        /// </summary>
        public static readonly Guid StartMenu = new Guid(0x625B53C3, 0xAB48, 0x4EC1, 0xBA, 0x1F, 0xA1, 0xEF, 0x41, 0x46, 0xFC, 0x19);

        /// <summary>
        /// Recent Items
        /// </summary>
        public static readonly Guid Recent = new Guid(0xAE50C081, 0xEBD2, 0x438A, 0x86, 0x55, 0x8A, 0x9, 0x2E, 0x34, 0x98, 0x7A);

        /// <summary>
        /// SendTo
        /// </summary>
        public static readonly Guid SendTo = new Guid(0x8983036C, 0x27C0, 0x404B, 0x8F, 0x8, 0x10, 0x2D, 0x10, 0xDC, 0xFD, 0x74);

        /// <summary>
        /// Documents
        /// </summary>
        public static readonly Guid Documents = new Guid(0xFDD39AD0, 0x238F, 0x46AF, 0xAD, 0xB4, 0x6C, 0x85, 0x48, 0x3, 0x69, 0xC7);

        /// <summary>
        /// Favorites
        /// </summary>
        public static readonly Guid Favorites = new Guid(0x1777F761, 0x68AD, 0x4D8A, 0x87, 0xBD, 0x30, 0xB7, 0x59, 0xFA, 0x33, 0xDD);

        /// <summary>
        /// Network Shortcuts
        /// </summary>
        public static readonly Guid NetHood = new Guid(0xC5ABBF53, 0xE17F, 0x4121, 0x89, 0x0, 0x86, 0x62, 0x6F, 0xC2, 0xC9, 0x73);

        /// <summary>
        /// Printer Shortcuts
        /// </summary>
        public static readonly Guid PrintHood = new Guid(0x9274BD8D, 0xCFD1, 0x41C3, 0xB3, 0x5E, 0xB1, 0x3F, 0x55, 0xA7, 0x58, 0xF4);

        /// <summary>
        /// Templates
        /// </summary>
        public static readonly Guid Templates = new Guid(0xA63293E8, 0x664E, 0x48DB, 0xA0, 0x79, 0xDF, 0x75, 0x9E, 0x5, 0x9, 0xF7);

        /// <summary>
        /// Startup
        /// </summary>
        public static readonly Guid CommonStartup = new Guid(0x82A5EA35, 0xD9CD, 0x47C5, 0x96, 0x29, 0xE1, 0x5D, 0x2F, 0x71, 0x4E, 0x6E);

        /// <summary>
        /// Programs
        /// </summary>
        public static readonly Guid CommonPrograms = new Guid(0x139D44E, 0x6AFE, 0x49F2, 0x86, 0x90, 0x3D, 0xAF, 0xCA, 0xE6, 0xFF, 0xB8);

        /// <summary>
        /// Start Menu
        /// </summary>
        public static readonly Guid CommonStartMenu = new Guid(0xA4115719, 0xD62E, 0x491D, 0xAA, 0x7C, 0xE7, 0x4B, 0x8B, 0xE3, 0xB0, 0x67);

        /// <summary>
        /// Public Desktop
        /// </summary>
        public static readonly Guid PublicDesktop = new Guid(0xC4AA340D, 0xF20F, 0x4863, 0xAF, 0xEF, 0xF8, 0x7E, 0xF2, 0xE6, 0xBA, 0x25);

        /// <summary>
        /// ProgramData
        /// </summary>
        public static readonly Guid ProgramData = new Guid(0x62AB5D82, 0xFDC1, 0x4DC3, 0xA9, 0xDD, 0x7, 0xD, 0x1D, 0x49, 0x5D, 0x97);

        /// <summary>
        /// Templates
        /// </summary>
        public static readonly Guid CommonTemplates = new Guid(0xB94237E7, 0x57AC, 0x4347, 0x91, 0x51, 0xB0, 0x8C, 0x6C, 0x32, 0xD1, 0xF7);

        /// <summary>
        /// Public Documents
        /// </summary>
        public static readonly Guid PublicDocuments = new Guid(0xED4824AF, 0xDCE4, 0x45A8, 0x81, 0xE2, 0xFC, 0x79, 0x65, 0x8, 0x36, 0x34);

        /// <summary>
        /// Roaming
        /// </summary>
        public static readonly Guid RoamingAppData = new Guid(0x3EB685DB, 0x65F9, 0x4CF6, 0xA0, 0x3A, 0xE3, 0xEF, 0x65, 0x72, 0x9F, 0x3D);

        /// <summary>
        /// Local
        /// </summary>
        public static readonly Guid LocalAppData = new Guid(0xF1B32785, 0x6FBA, 0x4FCF, 0x9D, 0x55, 0x7B, 0x8E, 0x7F, 0x15, 0x70, 0x91);

        /// <summary>
        /// LocalLow
        /// </summary>
        public static readonly Guid LocalAppDataLow = new Guid(0xA520A1A4, 0x1780, 0x4FF6, 0xBD, 0x18, 0x16, 0x73, 0x43, 0xC5, 0xAF, 0x16);

        /// <summary>
        /// Temporary Internet Files
        /// </summary>
        public static readonly Guid InternetCache = new Guid(0x352481E8, 0x33BE, 0x4251, 0xBA, 0x85, 0x60, 0x7, 0xCA, 0xED, 0xCF, 0x9D);

        /// <summary>
        /// Cookies
        /// </summary>
        public static readonly Guid Cookies = new Guid(0x2B0F765D, 0xC0E9, 0x4171, 0x90, 0x8E, 0x8, 0xA6, 0x11, 0xB8, 0x4F, 0xF6);

        /// <summary>
        /// History
        /// </summary>
        public static readonly Guid History = new Guid(0xD9DC8A3B, 0xB784, 0x432E, 0xA7, 0x81, 0x5A, 0x11, 0x30, 0xA7, 0x59, 0x63);

        /// <summary>
        /// System32
        /// </summary>
        public static readonly Guid System = new Guid(0x1AC14E77, 0x2E7, 0x4E5D, 0xB7, 0x44, 0x2E, 0xB1, 0xAE, 0x51, 0x98, 0xB7);

        /// <summary>
        /// System32
        /// </summary>
        public static readonly Guid SystemX86 = new Guid(0xD65231B0, 0xB2F1, 0x4857, 0xA4, 0xCE, 0xA8, 0xE7, 0xC6, 0xEA, 0x7D, 0x27);

        /// <summary>
        /// Windows
        /// </summary>
        public static readonly Guid Windows = new Guid(0xF38BF404, 0x1D43, 0x42F2, 0x93, 0x5, 0x67, 0xDE, 0xB, 0x28, 0xFC, 0x23);

        /// <summary>
        /// The user's username (%USERNAME%)
        /// </summary>
        public static readonly Guid Profile = new Guid(0x5E6C858F, 0xE22, 0x4760, 0x9A, 0xFE, 0xEA, 0x33, 0x17, 0xB6, 0x71, 0x73);

        /// <summary>
        /// Pictures
        /// </summary>
        public static readonly Guid Pictures = new Guid(0x33E28130, 0x4E1E, 0x4676, 0x83, 0x5A, 0x98, 0x39, 0x5C, 0x3B, 0xC3, 0xBB);

        /// <summary>
        /// Program Files
        /// </summary>
        public static readonly Guid ProgramFilesX86 = new Guid(0x7C5A40EF, 0xA0FB, 0x4BFC, 0x87, 0x4A, 0xC0, 0xF2, 0xE0, 0xB9, 0xFA, 0x8E);

        /// <summary>
        /// Common Files
        /// </summary>
        public static readonly Guid ProgramFilesCommonX86 = new Guid(0xDE974D24, 0xD9C6, 0x4D3E, 0xBF, 0x91, 0xF4, 0x45, 0x51, 0x20, 0xB9, 0x17);

        /// <summary>
        /// Program Files
        /// </summary>
        public static readonly Guid ProgramFilesX64 = new Guid(0x6D809377, 0x6AF0, 0x444B, 0x89, 0x57, 0xA3, 0x77, 0x3F, 0x2, 0x20, 0xE);

        /// <summary>
        /// Common Files
        /// </summary>
        public static readonly Guid ProgramFilesCommonX64 = new Guid(0x6365D5A7, 0xF0D, 0x45E5, 0x87, 0xF6, 0xD, 0xA5, 0x6B, 0x6A, 0x4F, 0x7D);

        /// <summary>
        /// Program Files
        /// </summary>
        public static readonly Guid ProgramFiles = new Guid(0x905E63B6, 0xC1BF, 0x494E, 0xB2, 0x9C, 0x65, 0xB7, 0x32, 0xD3, 0xD2, 0x1A);

        /// <summary>
        /// Common Files
        /// </summary>
        public static readonly Guid ProgramFilesCommon = new Guid(0xF7F1ED05, 0x9F6D, 0x47A2, 0xAA, 0xAE, 0x29, 0xD3, 0x17, 0xC6, 0xF0, 0x66);

        /// <summary>
        /// Administrative Tools
        /// </summary>
        public static readonly Guid AdminTools = new Guid(0x724EF170, 0xA42D, 0x4FEF, 0x9F, 0x26, 0xB6, 0xE, 0x84, 0x6F, 0xBA, 0x4F);

        /// <summary>
        /// Administrative Tools
        /// </summary>
        public static readonly Guid CommonAdminTools = new Guid(0xD0384E7D, 0xBAC3, 0x4797, 0x8F, 0x14, 0xCB, 0xA2, 0x29, 0xB3, 0x92, 0xB5);

        /// <summary>
        /// Music
        /// </summary>
        public static readonly Guid Music = new Guid(0x4BD8D571, 0x6D19, 0x48D3, 0xBE, 0x97, 0x42, 0x22, 0x20, 0x8, 0xE, 0x43);

        /// <summary>
        /// Videos
        /// </summary>
        public static readonly Guid Videos = new Guid(0x18989B1D, 0x99B5, 0x455B, 0x84, 0x1C, 0xAB, 0x7C, 0x74, 0xE4, 0xDD, 0xFC);

        /// <summary>
        /// Public Pictures
        /// </summary>
        public static readonly Guid PublicPictures = new Guid(0xB6EBFB86, 0x6907, 0x413C, 0x9A, 0xF7, 0x4F, 0xC2, 0xAB, 0xF0, 0x7C, 0xC5);

        /// <summary>
        /// Public Music
        /// </summary>
        public static readonly Guid PublicMusic = new Guid(0x3214FAB5, 0x9757, 0x4298, 0xBB, 0x61, 0x92, 0xA9, 0xDE, 0xAA, 0x44, 0xFF);

        /// <summary>
        /// Public Videos
        /// </summary>
        public static readonly Guid PublicVideos = new Guid(0x2400183A, 0x6185, 0x49FB, 0xA2, 0xD8, 0x4A, 0x39, 0x2A, 0x60, 0x2B, 0xA3);

        /// <summary>
        /// Resources
        /// </summary>
        public static readonly Guid ResourceDir = new Guid(0x8AD10C31, 0x2ADB, 0x4296, 0xA8, 0xF7, 0xE4, 0x70, 0x12, 0x32, 0xC9, 0x72);

        /// <summary>
        /// None
        /// </summary>
        public static readonly Guid LocalizedResourcesDir = new Guid(0x2A00375E, 0x224C, 0x49DE, 0xB8, 0xD1, 0x44, 0xD, 0xF7, 0xEF, 0x3D, 0xDC);

        /// <summary>
        /// OEM Links
        /// </summary>
        public static readonly Guid CommonOEMLinks = new Guid(0xC1BAE2D0, 0x10DF, 0x4334, 0xBE, 0xDD, 0x7A, 0xA2, 0xB, 0x22, 0x7A, 0x9D);

        /// <summary>
        /// Temporary Burn Folder
        /// </summary>
        public static readonly Guid CDBurning = new Guid(0x9E52AB10, 0xF80D, 0x49DF, 0xAC, 0xB8, 0x43, 0x30, 0xF5, 0x68, 0x78, 0x55);

        /// <summary>
        /// Users
        /// </summary>
        public static readonly Guid UserProfiles = new Guid(0x762D272, 0xC50A, 0x4BB0, 0xA3, 0x82, 0x69, 0x7D, 0xCD, 0x72, 0x9B, 0x80);

        /// <summary>
        /// Playlists
        /// </summary>
        public static readonly Guid Playlists = new Guid(0xDE92C1C7, 0x837F, 0x4F69, 0xA3, 0xBB, 0x86, 0xE6, 0x31, 0x20, 0x4A, 0x23);

        /// <summary>
        /// Sample Playlists
        /// </summary>
        public static readonly Guid SamplePlaylists = new Guid(0x15CA69B3, 0x30EE, 0x49C1, 0xAC, 0xE1, 0x6B, 0x5E, 0xC3, 0x72, 0xAF, 0xB5);

        /// <summary>
        /// Sample Music
        /// </summary>
        public static readonly Guid SampleMusic = new Guid(0xB250C668, 0xF57D, 0x4EE1, 0xA6, 0x3C, 0x29, 0xE, 0xE7, 0xD1, 0xAA, 0x1F);

        /// <summary>
        /// Sample Pictures
        /// </summary>
        public static readonly Guid SamplePictures = new Guid(0xC4900540, 0x2379, 0x4C75, 0x84, 0x4B, 0x64, 0xE6, 0xFA, 0xF8, 0x71, 0x6B);

        /// <summary>
        /// Sample Videos
        /// </summary>
        public static readonly Guid SampleVideos = new Guid(0x859EAD94, 0x2E85, 0x48AD, 0xA7, 0x1A, 0x9, 0x69, 0xCB, 0x56, 0xA6, 0xCD);

        /// <summary>
        /// Slide Shows
        /// </summary>
        public static readonly Guid PhotoAlbums = new Guid(0x69D2CF90, 0xFC33, 0x4FB7, 0x9A, 0xC, 0xEB, 0xB0, 0xF0, 0xFC, 0xB4, 0x3C);

        /// <summary>
        /// Public
        /// </summary>
        public static readonly Guid Public = new Guid(0xDFDF76A2, 0xC82A, 0x4D63, 0x90, 0x6A, 0x56, 0x44, 0xAC, 0x45, 0x73, 0x85);

        /// <summary>
        /// Programs and Features
        /// </summary>
        public static readonly Guid ChangeRemovePrograms = new Guid(0xDF7266AC, 0x9274, 0x4867, 0x8D, 0x55, 0x3B, 0xD6, 0x61, 0xDE, 0x87, 0x2D);

        /// <summary>
        /// Installed Updates
        /// </summary>
        public static readonly Guid AppUpdates = new Guid(0xA305CE99, 0xF527, 0x492B, 0x8B, 0x1A, 0x7E, 0x76, 0xFA, 0x98, 0xD6, 0xE4);

        /// <summary>
        /// Get Programs
        /// </summary>
        public static readonly Guid AddNewPrograms = new Guid(0xDE61D971, 0x5EBC, 0x4F02, 0xA3, 0xA9, 0x6C, 0x82, 0x89, 0x5E, 0x5C, 0x4);

        /// <summary>
        /// Downloads
        /// </summary>
        public static readonly Guid Downloads = new Guid(0x374DE290, 0x123F, 0x4565, 0x91, 0x64, 0x39, 0xC4, 0x92, 0x5E, 0x46, 0x7B);

        /// <summary>
        /// Public Downloads
        /// </summary>
        public static readonly Guid PublicDownloads = new Guid(0x3D644C9B, 0x1FB8, 0x4F30, 0x9B, 0x45, 0xF6, 0x70, 0x23, 0x5F, 0x79, 0xC0);

        /// <summary>
        /// Searches
        /// </summary>
        public static readonly Guid SavedSearches = new Guid(0x7D1D3A04, 0xDEBB, 0x4115, 0x95, 0xCF, 0x2F, 0x29, 0xDA, 0x29, 0x20, 0xDA);

        /// <summary>
        /// Quick Launch
        /// </summary>
        public static readonly Guid QuickLaunch = new Guid(0x52A4F021, 0x7B75, 0x48A9, 0x9F, 0x6B, 0x4B, 0x87, 0xA2, 0x10, 0xBC, 0x8F);

        /// <summary>
        /// Contacts
        /// </summary>
        public static readonly Guid Contacts = new Guid(0x56784854, 0xC6CB, 0x462B, 0x81, 0x69, 0x88, 0xE3, 0x50, 0xAC, 0xB8, 0x82);

        /// <summary>
        /// Gadgets
        /// </summary>
        public static readonly Guid SidebarParts = new Guid(0xA75D362E, 0x50FC, 0x4FB7, 0xAC, 0x2C, 0xA8, 0xBE, 0xAA, 0x31, 0x44, 0x93);

        /// <summary>
        /// Gadgets
        /// </summary>
        public static readonly Guid SidebarDefaultParts = new Guid(0x7B396E54, 0x9EC5, 0x4300, 0xBE, 0xA, 0x24, 0x82, 0xEB, 0xAE, 0x1A, 0x26);

        /// <summary>
        /// Tree property value folder
        /// </summary>
        public static readonly Guid TreeProperties = new Guid(0x5B3749AD, 0xB49F, 0x49C1, 0x83, 0xEB, 0x15, 0x37, 0xF, 0xBD, 0x48, 0x82);

        /// <summary>
        /// GameExplorer
        /// </summary>
        public static readonly Guid PublicGameTasks = new Guid(0xDEBF2536, 0xE1A8, 0x4C59, 0xB6, 0xA2, 0x41, 0x45, 0x86, 0x47, 0x6A, 0xEA);

        /// <summary>
        /// GameExplorer
        /// </summary>
        public static readonly Guid GameTasks = new Guid(0x54FAE61, 0x4DD8, 0x4787, 0x80, 0xB6, 0x9, 0x2, 0x20, 0xC4, 0xB7, 0x0);

        /// <summary>
        /// Saved Games
        /// </summary>
        public static readonly Guid SavedGames = new Guid(0x4C5C32FF, 0xBB9D, 0x43B0, 0xB5, 0xB4, 0x2D, 0x72, 0xE5, 0x4E, 0xAA, 0xA4);

        /// <summary>
        /// Games
        /// </summary>
        public static readonly Guid Games = new Guid(0xCAC52C1A, 0xB53D, 0x4EDC, 0x92, 0xD7, 0x6B, 0x2E, 0x8A, 0xC1, 0x94, 0x34);

        /// <summary>
        /// Recorded TV
        /// </summary>
        public static readonly Guid RecordedTV = new Guid(0xBD85E001, 0x112E, 0x431E, 0x98, 0x3B, 0x7B, 0x15, 0xAC, 0x9, 0xFF, 0xF1);

        /// <summary>
        /// Microsoft Office Outlook
        /// </summary>
        public static readonly Guid SearchMapi = new Guid(0x98EC0E18, 0x2098, 0x4D44, 0x86, 0x44, 0x66, 0x97, 0x93, 0x15, 0xA2, 0x81);

        /// <summary>
        /// Offline Files
        /// </summary>
        public static readonly Guid SearchCsc = new Guid(0xEE32E446, 0x31CA, 0x4ABA, 0x81, 0x4F, 0xA5, 0xEB, 0xD2, 0xFD, 0x6D, 0x5E);

        /// <summary>
        /// Links
        /// </summary>
        public static readonly Guid Links = new Guid(0xBFB9D5E0, 0xC6A9, 0x404C, 0xB2, 0xB2, 0xAE, 0x6D, 0xB6, 0xAF, 0x49, 0x68);

        /// <summary>
        /// The user's full name (for instance, Jean Philippe Bagel) entered when the user account was created.
        /// </summary>
        public static readonly Guid UsersFiles = new Guid(0xF3CE0F7C, 0x4901, 0x4ACC, 0x86, 0x48, 0xD5, 0xD4, 0x4B, 0x4, 0xEF, 0x8F);

        /// <summary>
        /// Search home
        /// </summary>
        public static readonly Guid SearchHome = new Guid(0x190337D1, 0xB8CA, 0x4121, 0xA6, 0x39, 0x6D, 0x47, 0x2D, 0x16, 0x97, 0x2A);

        /// <summary>
        /// Original Images
        /// </summary>
        public static readonly Guid OriginalImages = new Guid(0x2C36C0AA, 0x5812, 0x4B87, 0xBF, 0xD0, 0x4C, 0xD0, 0xDF, 0xB1, 0x9B, 0x39);

        /// <summary>
        /// SkyDrive; Windows 8.1 Folder
        /// </summary>
        public static readonly Guid SkyDrive = new Guid(0xA52BBA46, 0xE9E1, 0x435F, 0xB3, 0xD9, 0x28, 0xDA, 0xA6, 0x48, 0xC0, 0xF6);

        /// <summary>
        /// OneDrive; Windows 8.1/Windows 10 Folder
        /// </summary>
        public static readonly Guid OneDrive = new Guid(0xA52BBA46, 0xE9E1, 0x435F, 0xB3, 0xD9, 0x28, 0xDA, 0xA6, 0x48, 0xC0, 0xF6);
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// CommonFileDialog AddPlace locations
    /// </summary>
    public enum FileDialogAddPlaceLocation
    {
        /// <summary>
        /// At the bottom of the Favorites or Places list.
        /// </summary>
        Bottom = 0x0,

        /// <summary>
        /// At the top of the Favorites or Places list.
        /// </summary>
        Top = 0x1
    }

    /// <summary>
    /// One of the values that indicates how the ShellObject DisplayName should look.
    /// </summary>
    public enum DisplayNameType : uint
    {
        /// <summary>
        /// Returns the display name relative to the desktop.
        /// </summary>
        Default = 0x0,

        /// <summary>
        /// Returns the parsing name relative to the parent folder.
        /// </summary>
        RelativeToParent = 0x80018001,

        /// <summary>
        /// Returns the path relative to the parent folder in a
        /// friendly format as displayed in an address bar.
        /// </summary>
        RelativeToParentAddressBar = 0x8007C001,

        /// <summary>
        /// Returns the parsing name relative to the desktop.
        /// </summary>
        RelativeToDesktop = 0x80028000,

        /// <summary>
        /// Returns the editing name relative to the parent folder.
        /// </summary>
        RelativeToParentEditing = 0x80031001,

        /// <summary>
        /// Returns the editing name relative to the desktop.
        /// </summary>
        RelativeToDesktopEditing = 0x8004C000,

        /// <summary>
        /// Returns the display name relative to the file system path.
        /// </summary>
        FileSystemPath = 0x80058000,

        /// <summary>
        /// Returns the display name relative to a URL.
        /// </summary>
        Url = 0x80068000
    }
    /// <summary>
    /// Available Library folder types
    /// </summary>
    public enum LibraryFolderType
    {
        /// <summary>
        /// General Items
        /// </summary>
        Generic = 0,

        /// <summary>
        /// Documents
        /// </summary>
        Documents,

        /// <summary>
        /// Music
        /// </summary>
        Music,

        /// <summary>
        /// Pictures
        /// </summary>
        Pictures,

        /// <summary>
        /// Videos
        /// </summary>
        Videos
    }

    /// <summary>
    /// Flags controlling the appearance of a window
    /// </summary>
    public enum WindowShowCommand
    {
        /// <summary>
        /// Hides the window and activates another window.
        /// </summary>
        Hide = 0,

        /// <summary>
        /// Activates and displays the window (including restoring
        /// it to its original size and position).
        /// </summary>
        Normal = 1,

        /// <summary>
        /// Minimizes the window.
        /// </summary>
        Minimized = 2,

        /// <summary>
        /// Maximizes the window.
        /// </summary>
        Maximized = 3,

        /// <summary>
        /// Similar to Normal, except that the window
        /// is not activated.
        /// </summary>
        ShowNoActivate = 4,

        /// <summary>
        /// Activates the window and displays it in its current size
        /// and position.
        /// </summary>
        Show = 5,

        /// <summary>
        /// Minimizes the window and activates the next top-level window.
        /// </summary>
        Minimize = 6,

        /// <summary>
        /// Minimizes the window and does not activate it.
        /// </summary>
        ShowMinimizedNoActivate = 7,

        /// <summary>
        /// Similar to Normal, except that the window is not
        /// activated.
        /// </summary>
        ShowNA = 8,

        /// <summary>
        /// Activates and displays the window, restoring it to its original
        /// size and position.
        /// </summary>
        Restore = 9,

        /// <summary>
        /// Sets the show state based on the initial value specified when
        /// the process was created.
        /// </summary>
        Default = 10,

        /// <summary>
        /// Minimizes a window, even if the thread owning the window is not
        /// responding.  Use this only to minimize windows from a different
        /// thread.
        /// </summary>
        ForceMinimize = 11
    }

    /// <summary>
    /// Provides a set of flags to be used with SearchCondition
    /// to indicate the operation in SearchConditionFactory's methods.
    /// </summary>
    public enum SearchConditionOperation
    {
        /// <summary>
        /// An implicit comparison between the value of the property and the value of the constant.
        /// </summary>
        Implicit = 0,

        /// <summary>
        /// The value of the property and the value of the constant must be equal.
        /// </summary>
        Equal = 1,

        /// <summary>
        /// The value of the property and the value of the constant must not be equal.
        /// </summary>
        NotEqual = 2,

        /// <summary>
        /// The value of the property must be less than the value of the constant.
        /// </summary>
        LessThan = 3,

        /// <summary>
        /// The value of the property must be greater than the value of the constant.
        /// </summary>
        GreaterThan = 4,

        /// <summary>
        /// The value of the property must be less than or equal to the value of the constant.
        /// </summary>
        LessThanOrEqual = 5,

        /// <summary>
        /// The value of the property must be greater than or equal to the value of the constant.
        /// </summary>
        GreaterThanOrEqual = 6,

        /// <summary>
        /// The value of the property must begin with the value of the constant.
        /// </summary>
        ValueStartsWith = 7,

        /// <summary>
        /// The value of the property must end with the value of the constant.
        /// </summary>
        ValueEndsWith = 8,

        /// <summary>
        /// The value of the property must contain the value of the constant.
        /// </summary>
        ValueContains = 9,

        /// <summary>
        /// The value of the property must not contain the value of the constant.
        /// </summary>
        ValueNotContains = 10,

        /// <summary>
        /// The value of the property must match the value of the constant, where '?'
        /// matches any single character and '*' matches any sequence of characters.
        /// </summary>
        DosWildcards = 11,

        /// <summary>
        /// The value of the property must contain a word that is the value of the constant.
        /// </summary>
        WordEqual = 12,

        /// <summary>
        /// The value of the property must contain a word that begins with the value of the constant.
        /// </summary>
        WordStartsWith = 13,

        /// <summary>
        /// The application is free to interpret this in any suitable way.
        /// </summary>
        ApplicationSpecific = 14
    }

    /// <summary>
    /// Set of flags to be used with SearchConditionFactory.
    /// </summary>
    public enum SearchConditionType
    {
        /// <summary>
        /// Indicates that the values of the subterms are combined by "AND".
        /// </summary>
        And = 0,

        /// <summary>
        /// Indicates that the values of the subterms are combined by "OR".
        /// </summary>
        Or = 1,

        /// <summary>
        /// Indicates a "NOT" comparison of subterms.
        /// </summary>
        Not = 2,

        /// <summary>
        /// Indicates that the node is a comparison between a property and a
        /// constant value using a SearchConditionOperation.
        /// </summary>
        Leaf = 3
    }

    /// <summary>
    /// Used to describe the view mode.
    /// </summary>
    public enum FolderLogicalViewMode
    {
        /// <summary>
        /// The view is not specified.
        /// </summary>
        Unspecified = -1,

        /// <summary>
        /// This should have the same affect as Unspecified.
        /// </summary>
        None = 0,

        /// <summary>
        /// The minimum valid enumeration value. Used for validation purposes only.
        /// </summary>
        First = 1,

        /// <summary>
        /// Details view.
        /// </summary>
        Details = 1,

        /// <summary>
        /// Tiles view.
        /// </summary>
        Tiles = 2,

        /// <summary>
        /// Icons view.
        /// </summary>
        Icons = 3,

        /// <summary>
        /// Windows 7 and later. List view.
        /// </summary>
        List = 4,

        /// <summary>
        /// Windows 7 and later. Content view.
        /// </summary>
        Content = 5,

        /// <summary>
        /// The maximum valid enumeration value. Used for validation purposes only.
        /// </summary>
        Last = 5
    }

    /// <summary>
    /// The direction in which the items are sorted.
    /// </summary>
    public enum SortDirection
    {
        /// <summary>
        /// A default value for sort direction, this value should not be used;
        /// instead use Descending or Ascending.
        /// </summary>
        Default = 0,

        /// <summary>
        /// The items are sorted in descending order. Whether the sort is alphabetical, numerical,
        /// and so on, is determined by the data type of the column indicated in propkey.
        /// </summary>
        Descending = -1,

        /// <summary>
        /// The items are sorted in ascending order. Whether the sort is alphabetical, numerical,
        /// and so on, is determined by the data type of the column indicated in propkey.
        /// </summary>
        Ascending = 1
    }

    /// <summary>
    /// Provides a set of flags to be used with IQueryParser::SetOption and
    /// IQueryParser::GetOption to indicate individual options.
    /// </summary>
    public enum StructuredQuerySingleOption
    {
        /// <summary>
        /// The value should be VT_LPWSTR and the path to a file containing a schema binary.
        /// </summary>
        Schema,

        /// <summary>
        /// The value must be VT_EMPTY (the default) or a VT_UI4 that is an LCID. It is used
        /// as the locale of contents (not keywords) in the query to be searched for, when no
        /// other information is available. The default value is the current keyboard locale.
        /// Retrieving the value always returns a VT_UI4.
        /// </summary>
        Locale,

        /// <summary>
        /// This option is used to override the default word breaker used when identifying keywords
        /// in queries. The default word breaker is chosen according to the language of the keywords
        /// (cf. SQSO_LANGUAGE_KEYWORDS below). When setting this option, the value should be VT_EMPTY
        /// for using the default word breaker, or a VT_UNKNOWN with an object supporting
        /// the IWordBreaker interface. Retrieving the option always returns a VT_UNKNOWN with an object
        /// supporting the IWordBreaker interface.
        /// </summary>
        WordBreaker,

        /// <summary>
        /// The value should be VT_EMPTY or VT_BOOL with VARIANT_TRUE to allow natural query
        /// syntax (the default) or VT_BOOL with VARIANT_FALSE to allow only advanced query syntax.
        /// Retrieving the option always returns a VT_BOOL.
        /// This option is now deprecated, use SQSO_SYNTAX.
        /// </summary>
        NaturalSyntax,

        /// <summary>
        /// The value should be VT_BOOL with VARIANT_TRUE to generate query expressions
        /// as if each word in the query had a star appended to it (unless followed by punctuation
        /// other than a parenthesis), or VT_EMPTY or VT_BOOL with VARIANT_FALSE to
        /// use the words as they are (the default). A word-wheeling application
        /// will generally want to set this option to true.
        /// Retrieving the option always returns a VT_BOOL.
        /// </summary>
        AutomaticWildcard,

        /// <summary>
        /// Reserved. The value should be VT_EMPTY (the default) or VT_I4.
        /// Retrieving the option always returns a VT_I4.
        /// </summary>
        TraceLevel,

        /// <summary>
        /// The value must be a VT_UI4 that is a LANGID. It defaults to the default user UI language.
        /// </summary>
        LanguageKeywords,

        /// <summary>
        /// The value must be a VT_UI4 that is a STRUCTURED_QUERY_SYNTAX value.
        /// It defaults to SQS_NATURAL_QUERY_SYNTAX.
        /// </summary>
        Syntax,

        /// <summary>
        /// The value must be a VT_BLOB that is a copy of a TIME_ZONE_INFORMATION structure.
        /// It defaults to the current time zone.
        /// </summary>
        TimeZone,

        /// <summary>
        /// This setting decides what connector should be assumed between conditions when none is specified.
        /// The value must be a VT_UI4 that is a CONDITION_TYPE. Only CT_AND_CONDITION and CT_OR_CONDITION
        /// are valid. It defaults to CT_AND_CONDITION.
        /// </summary>
        ImplicitConnector,

        /// <summary>
        /// This setting decides whether there are special requirements on the case of connector keywords (such
        /// as AND or OR). The value must be a VT_UI4 that is a CASE_REQUIREMENT value.
        /// It defaults to CASE_REQUIREMENT_UPPER_IF_AQS.
        /// </summary>
        ConnectorCase
    }

    /// <summary>
    /// Provides a set of flags to be used with IQueryParser::SetMultiOption
    /// to indicate individual options.
    /// </summary>
    public enum StructuredQueryMultipleOption
    {
        /// <summary>
        /// The key should be property name P. The value should be a
        /// VT_UNKNOWN with an IEnumVARIANT which has two values: a VT_BSTR that is another
        /// property name Q and a VT_I4 that is a CONDITION_OPERATION cop. A predicate with
        /// property name P, some operation and a value V will then be replaced by a predicate
        /// with property name Q, operation cop and value V before further processing happens.
        /// </summary>
        VirtualProperty,

        /// <summary>
        /// The key should be a value type name V. The value should be a
        /// VT_LPWSTR with a property name P. A predicate with no property name and a value of type
        /// V (or any subtype of V) will then use property P.
        /// </summary>
        DefaultProperty,

        /// <summary>
        /// The key should be a value type name V. The value should be a
        /// VT_UNKNOWN with a IConditionGenerator G. The GenerateForLeaf method of
        /// G will then be applied to any predicate with value type V and if it returns a query
        /// expression, that will be used. If it returns NULL, normal processing will be used
        /// instead.
        /// </summary>
        GeneratorForType,

        /// <summary>
        /// The key should be a property name P. The value should be a VT_VECTOR|VT_LPWSTR,
        /// where each string is a property name. The count must be at least one. This "map" will be
        /// added to those of the loaded schema and used during resolution. A second call with the
        /// same key will replace the current map. If the value is VT_NULL, the map will be removed.
        /// </summary>
        MapProperty
    }

    /// <summary>
    /// Used by IQueryParserManager::SetOption to set parsing options.
    /// This can be used to specify schemas and localization options.
    /// </summary>
    public enum QueryParserManagerOption
    {
        /// <summary>
        /// A VT_LPWSTR containing the name of the file that contains the schema binary.
        /// The default value is StructuredQuerySchema.bin for the SystemIndex catalog
        /// and StructuredQuerySchemaTrivial.bin for the trivial catalog.
        /// </summary>
        SchemaBinaryName = 0,

        /// <summary>
        /// Either a VT_BOOL or a VT_LPWSTR. If the value is a VT_BOOL and is FALSE,
        /// a pre-localized schema will not be used. If the value is a VT_BOOL and is TRUE,
        /// IQueryParserManager will use the pre-localized schema binary in
        /// "%ALLUSERSPROFILE%\Microsoft\Windows". If the value is a VT_LPWSTR, the value should
        /// contain the full path of the folder in which the pre-localized schema binary can be found.
        /// The default value is VT_BOOL with TRUE.
        /// </summary>
        PreLocalizedSchemaBinaryPath = 1,

        /// <summary>
        /// A VT_LPWSTR containing the full path to the folder that contains the
        /// unlocalized schema binary. The default value is "%SYSTEMROOT%\System32".
        /// </summary>
        UnlocalizedSchemaBinaryPath = 2,

        /// <summary>
        /// A VT_LPWSTR containing the full path to the folder that contains the
        /// localized schema binary that can be read and written to as needed.
        /// The default value is "%LOCALAPPDATA%\Microsoft\Windows".
        /// </summary>
        LocalizedSchemaBinaryPath = 3,

        /// <summary>
        /// A VT_BOOL. If TRUE, then the paths for pre-localized and localized binaries
        /// have "\(LCID)" appended to them, where language code identifier (LCID) is
        /// the decimal locale ID for the localized language. The default is TRUE.
        /// </summary>
        AppendLCIDToLocalizedPath = 4,

        /// <summary>
        /// A VT_UNKNOWN with an object supporting ISchemaLocalizerSupport.
        /// This object will be used instead of the default localizer support object.
        /// </summary>
        LocalizerSupport = 5
    }

    [Flags]
    public enum FileOpenOptions
    {
        OverwritePrompt = 0x2,
        StrictFileTypes = 0x4,
        NoChangeDirectory = 0x8,
        PickFolders = 0x20,
        // Ensure that items returned are filesystem items.
        ForceFilesystem = 0x40,
        // Allow choosing items that have no storage.
        AllNonStorageItems = 0x80,
        NoValidate = 0x100,
        AllowMultiSelect = 0x200,
        PathMustExist = 0x800,
        FileMustExist = 0x1000,
        CreatePrompt = 0x2000,
        ShareAware = 0x4000,
        NoReadOnlyReturn = 0x8000,
        NoTestFileCreate = 0x10000,
        HideMruPlaces = 0x20000,
        HidePinnedPlaces = 0x40000,
        NoDereferenceLinks = 0x100000,
        DontAddToRecent = 0x2000000,
        ForceShowHidden = 0x10000000,
        DefaultNoMiniMode = 0x20000000
    }

    public enum ControlState
    {
        Inactive = 0x0,
        Enable = 0x1,
        Visible = 0x2
    }

    public enum ShellItemDesignNameOptions : uint
    {
        Normal = 0x0,
        // SIGDN_NORMAL
        ParentRelativeParsing = 0x80018001,
        // SIGDN_INFOLDER | SIGDN_FORPARSING
        DesktopAbsoluteParsing = 0x80028000,
        // SIGDN_FORPARSING
        ParentRelativeEditing = 0x80031001,
        // SIGDN_INFOLDER | SIGDN_FOREDITING
        DesktopAbsoluteEditing = 0x8004C000,
        // SIGDN_FORPARSING | SIGDN_FORADDRESSBAR
        FileSystemPath = 0x80058000,
        // SIGDN_FORPARSING
        Url = 0x80068000,
        // SIGDN_FORPARSING
        ParentRelativeForAddressBar = 0x8007C001,
        // SIGDN_INFOLDER | SIGDN_FORPARSING | SIGDN_FORADDRESSBAR
        ParentRelative = 0x80080001
        // SIGDN_INFOLDER
    }

    /// <summary>
    /// Indicate flags that modify the property store object retrieved by methods
    /// that create a property store, such as IShellItem2::GetPropertyStore or
    /// IPropertyStoreFactory::GetPropertyStore.
    /// </summary>
    [Flags]
    public enum GetPropertyStoreOptions
    {
        /// <summary>
        /// Meaning to a calling process: Return a read-only property store that contains all
        /// properties. Slow items (offline files) are not opened.
        /// Combination with other flags: Can be overridden by other flags.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Meaning to a calling process: Include only properties directly from the property
        /// handler, which opens the file on the disk, network, or device. Meaning to a file
        /// folder: Only include properties directly from the handler.
        /// 
        /// Meaning to other folders: When delegating to a file folder, pass this flag on
        /// to the file folder; do not do any multiplexing (MUX). When not delegating to a
        /// file folder, ignore this flag instead of returning a failure code.
        /// 
        /// Combination with other flags: Cannot be combined with GPS_TEMPORARY,
        /// GPS_FASTPROPERTIESONLY, or GPS_BESTEFFORT.
        /// </summary>
        HandlePropertiesOnly = 0x1,

        /// <summary>
        /// Meaning to a calling process: Can write properties to the item.
        /// Note: The store may contain fewer properties than a read-only store.
        /// 
        /// Meaning to a file folder: ReadWrite.
        /// 
        /// Meaning to other folders: ReadWrite. Note: When using default MUX,
        /// return a single unmultiplexed store because the default MUX does not support ReadWrite.
        /// 
        /// Combination with other flags: Cannot be combined with GPS_TEMPORARY, GPS_FASTPROPERTIESONLY,
        /// GPS_BESTEFFORT, or GPS_DELAYCREATION. Implies GPS_HANDLERPROPERTIESONLY.
        /// </summary>
        ReadWrite = 0x2,

        /// <summary>
        /// Meaning to a calling process: Provides a writable store, with no initial properties,
        /// that exists for the lifetime of the Shell item instance; basically, a property bag
        /// attached to the item instance.
        /// 
        /// Meaning to a file folder: Not applicable. Handled by the Shell item.
        /// 
        /// Meaning to other folders: Not applicable. Handled by the Shell item.
        /// 
        /// Combination with other flags: Cannot be combined with any other flag. Implies GPS_READWRITE
        /// </summary>
        Temporary = 0x4,

        /// <summary>
        /// Meaning to a calling process: Provides a store that does not involve reading from the
        /// disk or network. Note: Some values may be different, or missing, compared to a store
        /// without this flag.
        /// 
        /// Meaning to a file folder: Include the "innate" and "fallback" stores only. Do not load the handler.
        /// 
        /// Meaning to other folders: Include only properties that are available in memory or can
        /// be computed very quickly (no properties from disk, network, or peripheral IO devices).
        /// This is normally only data sources from the IDLIST. When delegating to other folders, pass this flag on to them.
        /// 
        /// Combination with other flags: Cannot be combined with GPS_TEMPORARY, GPS_READWRITE,
        /// GPS_HANDLERPROPERTIESONLY, or GPS_DELAYCREATION.
        /// </summary>
        FastPropertiesOnly = 0x8,

        /// <summary>
        /// Meaning to a calling process: Open a slow item (offline file) if necessary.
        /// Meaning to a file folder: Retrieve a file from offline storage, if necessary.
        /// Note: Without this flag, the handler is not created for offline files.
        /// 
        /// Meaning to other folders: Do not return any properties that are very slow.
        /// 
        /// Combination with other flags: Cannot be combined with GPS_TEMPORARY or GPS_FASTPROPERTIESONLY.
        /// </summary>
        OpensLowItem = 0x10,

        /// <summary>
        /// Meaning to a calling process: Delay memory-intensive operations, such as file access, until
        /// a property is requested that requires such access.
        /// 
        /// Meaning to a file folder: Do not create the handler until needed; for example, either
        /// GetCount/GetAt or GetValue, where the innate store does not satisfy the request.
        /// Note: GetValue might fail due to file access problems.
        /// 
        /// Meaning to other folders: If the folder has memory-intensive properties, such as
        /// delegating to a file folder or network access, it can optimize performance by
        /// supporting IDelayedPropertyStoreFactory and splitting up its properties into a
        /// fast and a slow store. It can then use delayed MUX to recombine them.
        /// 
        /// Combination with other flags: Cannot be combined with GPS_TEMPORARY or
        /// GPS_READWRITE
        /// </summary>
        DelayCreation = 0x20,

        /// <summary>
        /// Meaning to a calling process: Succeed at getting the store, even if some
        /// properties are not returned. Note: Some values may be different, or missing,
        /// compared to a store without this flag.
        /// 
        /// Meaning to a file folder: Succeed and return a store, even if the handler or
        /// innate store has an error during creation. Only fail if substores fail.
        /// 
        /// Meaning to other folders: Succeed on getting the store, even if some properties
        /// are not returned.
        /// 
        /// Combination with other flags: Cannot be combined with GPS_TEMPORARY,
        /// GPS_READWRITE, or GPS_HANDLERPROPERTIESONLY.
        /// </summary>
        BestEffort = 0x40,

        /// <summary>
        /// Mask for valid GETPROPERTYSTOREFLAGS values.
        /// </summary>
        MaskValid = 0xFF
    }

    public enum SHOPType
    {
        SHOP_PRINTERNAME = 0x1, // ' lpObject points To a printer friendly name
        SHOP_FILEPATH = 0x2, // ' lpObject points To a fully qualified path+file name
        SHOP_VOLUMEGUID = 0x4 // ' lpObject points To a Volume GUID
    }

    public enum ShellItemAttributeOptions
    {
        // if multiple items and the attirbutes together.
        And = 0x1,
        // if multiple items or the attributes together.
        Or = 0x2,
        // Call GetAttributes directly on the 
        // ShellFolder for multiple attributes.
        AppCompat = 0x3,

        // A mask for SIATTRIBFLAGS_AND, SIATTRIBFLAGS_OR, and SIATTRIBFLAGS_APPCOMPAT. Callers normally do not use this value.
        Mask = 0x3,

        // Windows 7 and later. Examine all items in the array to compute the attributes. 
        // Note that this can result in poor performance over large arrays and therefore it 
        // should be used only when needed. Cases in which you pass this flag should be extremely rare.
        AllItems = 0x4000
    }

    public enum FileDialogEventShareViolationResponse
    {
        Default = 0x0,
        Accept = 0x1,
        Refuse = 0x2
    }

    public enum FileDialogEventOverwriteResponse
    {
        Default = 0x0,
        Accept = 0x1,
        Refuse = 0x2
    }

    public enum FileDialogAddPlacement
    {
        Bottom = 0x0,
        Top = 0x1
    }

    [Flags]
    public enum SIIGBF
    {
        ResizeToFit = 0x0,
        BiggerSizeOk = 0x1,
        MemoryOnly = 0x2,
        IconOnly = 0x4,
        ThumbnailOnly = 0x8,
        InCacheOnly = 0x10
    }

    [Flags]
    public enum ThumbnailOptions
    {
        Extract = 0x0,
        InCacheOnly = 0x1,
        FastExtract = 0x2,
        ForceExtraction = 0x4,
        SlowReclaim = 0x8,
        ExtractDoNotCache = 0x20
    }

    [Flags]
    public enum ThumbnailCacheOptions
    {
        Default = 0x0,
        LowQuality = 0x1,
        Cached = 0x2
    }

    [Flags]
    public enum ShellFileGetAttributesOptions : uint
    {
        /// <summary>
        /// The specified items can be copied.
        /// </summary>
        CanCopy = 0x1,

        /// <summary>
        /// The specified items can be moved.
        /// </summary>
        CanMove = 0x2,

        /// <summary>
        /// Shortcuts can be created for the specified items. This flag has the same value as DROPEFFECT.
        /// The normal use of this flag is to add a Create Shortcut item to the shortcut menu that is displayed
        /// during drag-and-drop operations. However, SFGAO_CANLINK also adds a Create Shortcut item to the Microsoft
        /// Windows Explorer's File menu and to normal shortcut menus.
        /// If this item is selected, your application's IContextMenu::InvokeCommand is invoked with the lpVerb
        /// member of the CMINVOKECOMMANDINFO structure set to "link." Your application is responsible for creating the link.
        /// </summary>
        CanLink = 0x4,

        /// <summary>
        /// The specified items can be bound to an IStorage interface through IShellFolder::BindToObject.
        /// </summary>
        Storage = 0x8,

        /// <summary>
        /// The specified items can be renamed.
        /// </summary>
        CanRename = 0x10,

        /// <summary>
        /// The specified items can be deleted.
        /// </summary>
        CanDelete = 0x20,

        /// <summary>
        /// The specified items have property sheets.
        /// </summary>
        HasPropertySheet = 0x40,

        /// <summary>
        /// The specified items are drop targets.
        /// </summary>
        DropTarget = 0x100,

        /// <summary>
        /// This flag is a mask for the capability flags.
        /// </summary>
        CapabilityMask = 0x177,

        /// <summary>
        /// Windows 7 and later. The specified items are system items.
        /// </summary>
        System = 0x1000,

        /// <summary>
        /// The specified items are encrypted.
        /// </summary>
        Encrypted = 0x2000,

        /// <summary>
        /// Indicates that accessing the object = through IStream or other storage interfaces,
        /// is a slow operation.
        /// Applications should avoid accessing items flagged with SFGAO_ISSLOW.
        /// </summary>
        IsSlow = 0x4000,

        /// <summary>
        /// The specified items are ghosted icons.
        /// </summary>
        Ghosted = 0x8000,

        /// <summary>
        /// The specified items are shortcuts.
        /// </summary>
        Link = 0x10000,

        /// <summary>
        /// The specified folder objects are shared.
        /// </summary>
        Share = 0x20000,

        /// <summary>
        /// The specified items are read-only. In the case of folders, this means
        /// that new items cannot be created in those folders.
        /// </summary>
        ReadOnly = 0x40000,

        /// <summary>
        /// The item is hidden and should not be displayed unless the
        /// Show hidden files and folders option is enabled in Folder Settings.
        /// </summary>
        Hidden = 0x80000,

        /// <summary>
        /// This flag is a mask for the display attributes.
        /// </summary>
        DisplayAttributeMask = 0xFC000,

        /// <summary>
        /// The specified folders contain one or more file system folders.
        /// </summary>
        FileSystemAncestor = 0x10000000,

        /// <summary>
        /// The specified items are folders.
        /// </summary>
        Folder = 0x20000000,

        /// <summary>
        /// The specified folders or file objects are part of the file system
        /// that is, they are files, directories, or root directories).
        /// </summary>
        FileSystem = 0x40000000,

        /// <summary>
        /// The specified folders have subfolders = and are, therefore,
        /// expandable in the left pane of Windows Explorer).
        /// </summary>
        HasSubFolder = 0x80000000,

        /// <summary>
        /// This flag is a mask for the contents attributes.
        /// </summary>
        ContentsMask = 0x80000000,

        /// <summary>
        /// When specified as input, SFGAO_VALIDATE instructs the folder to validate that the items
        /// pointed to by the contents of apidl exist. If one or more of those items do not exist,
        /// IShellFolder::GetAttributesOf returns a failure code.
        /// When used with the file system folder, SFGAO_VALIDATE instructs the folder to discard cached
        /// properties retrieved by clients of IShellFolder2::GetDetailsEx that may
        /// have accumulated for the specified items.
        /// </summary>
        Validate = 0x1000000,

        /// <summary>
        /// The specified items are on removable media or are themselves removable devices.
        /// </summary>
        Removable = 0x2000000,

        /// <summary>
        /// The specified items are compressed.
        /// </summary>
        Compressed = 0x4000000,

        /// <summary>
        /// The specified items can be browsed in place.
        /// </summary>
        Browsable = 0x8000000,

        /// <summary>
        /// The items are nonenumerated items.
        /// </summary>
        Nonenumerated = 0x100000,

        /// <summary>
        /// The objects contain new content.
        /// </summary>
        NewContent = 0x200000,

        /// <summary>
        /// It is possible to create monikers for the specified file objects or folders.
        /// </summary>
        CanMoniker = 0x400000,

        /// <summary>
        /// Not supported.
        /// </summary>
        HasStorage = 0x400000,

        /// <summary>
        /// Indicates that the item has a stream associated with it that can be accessed
        /// by a call to IShellFolder::BindToObject with IID_IStream in the riid parameter.
        /// </summary>
        Stream = 0x400000,

        /// <summary>
        /// Children of this item are accessible through IStream or IStorage.
        /// Those children are flagged with SFGAO_STORAGE or SFGAO_STREAM.
        /// </summary>
        StorageAncestor = 0x800000,

        /// <summary>
        /// This flag is a mask for the storage capability attributes.
        /// </summary>
        StorageCapabilityMask = 0x70C50008,

        /// <summary>
        /// Mask used by PKEY_SFGAOFlags to remove certain values that are considered
        /// to cause slow calculations or lack context.
        /// Equal to SFGAO_VALIDATE | SFGAO_ISSLOW | SFGAO_HASSUBFOLDER.
        /// </summary>
        PkeyMask = 0x81044000
    }

    [Flags]
    public enum ShellFolderEnumerationOptions : ushort
    {
        CheckingForChildren = 0x10,
        Folders = 0x20,
        NonFolders = 0x40,
        IncludeHidden = 0x80,
        InitializeOnFirstNext = 0x100,
        NetPrinterSearch = 0x200,
        Shareable = 0x400,
        Storage = 0x800,
        NavigationEnum = 0x1000,
        FastItems = 0x2000,
        FlatList = 0x4000,
        EnableAsync = 0x8000
    }

    public enum SICHINTF : uint
    {
        SICHINT_DISPLAY = 0x0,
        SICHINT_CANONICAL = 0x10000000,
        SICHINT_TEST_FILESYSPATH_IF_NOT_EQUAL = 0x20000000,
        SICHINT_ALLFIELDS = 0x80000000
    }


    /// <summary>
    /// Thumbnail Alpha Types
    /// </summary>
    public enum ThumbnailAlphaType
    {
        /// <summary>
        /// Let the system decide.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// No transparency
        /// </summary>
        NoAlphaChannel = 1,

        /// <summary>
        /// Has transparency
        /// </summary>
        HasAlphaChannel = 2
    }

    [Flags]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "Follows native api.")]
    public enum AccessModes
    {
        /// <summary>
        /// Indicates that, in direct mode, each change to a storage
        /// or stream element is written as it occurs.
        /// </summary>
        Direct = 0x0,

        /// <summary>
        /// Indicates that, in transacted mode, changes are buffered
        /// and written only if an explicit commit operation is called.
        /// </summary>
        Transacted = 0x10000,

        /// <summary>
        /// Provides a faster implementation of a compound file
        /// in a limited, but frequently used, case.
        /// </summary>
        Simple = 0x8000000,

        /// <summary>
        /// Indicates that the object is read-only,
        /// meaning that modifications cannot be made.
        /// </summary>
        Read = 0x0,

        /// <summary>
        /// Enables you to save changes to the object,
        /// but does not permit access to its data.
        /// </summary>
        Write = 0x1,

        /// <summary>
        /// Enables access and modification of object data.
        /// </summary>
        ReadWrite = 0x2,

        /// <summary>
        /// Specifies that subsequent openings of the object are
        /// not denied read or write access.
        /// </summary>
        ShareDenyNone = 0x40,

        /// <summary>
        /// Prevents others from subsequently opening the object in Read mode.
        /// </summary>
        ShareDenyRead = 0x30,

        /// <summary>
        /// Prevents others from subsequently opening the object
        /// for Write or ReadWrite access.
        /// </summary>
        ShareDenyWrite = 0x20,

        /// <summary>
        /// Prevents others from subsequently opening the object in any mode.
        /// </summary>
        ShareExclusive = 0x10,

        /// <summary>
        /// Opens the storage object with exclusive access to the most
        /// recently committed version.
        /// </summary>
        Priority = 0x40000,

        /// <summary>
        /// Indicates that the underlying file is to be automatically destroyed when the root
        /// storage object is released. This feature is most useful for creating temporary files.
        /// </summary>
        DeleteOnRelease = 0x4000000,

        /// <summary>
        /// Indicates that, in transacted mode, a temporary scratch file is usually used
        /// to save modifications until the Commit method is called.
        /// Specifying NoScratch permits the unused portion of the original file
        /// to be used as work space instead of creating a new file for that purpose.
        /// </summary>
        NoScratch = 0x100000,

        /// <summary>
        /// Indicates that an existing storage object
        /// or stream should be removed before the new object replaces it.
        /// </summary>
        Create = 0x1000,

        /// <summary>
        /// Creates the new object while preserving existing data in a stream named "Contents".
        /// </summary>
        Convert = 0x20000,

        /// <summary>
        /// Causes the create operation to fail if an existing object with the specified name exists.
        /// </summary>
        FailIfThere = 0x0,

        /// <summary>
        /// This flag is used when opening a storage object with Transacted
        /// and without ShareExclusive or ShareDenyWrite.
        /// In this case, specifying NoSnapshot prevents the system-provided
        /// implementation from creating a snapshot copy of the file.
        /// Instead, changes to the file are written to the end of the file.
        /// </summary>
        NoSnapshot = 0x200000,

        /// <summary>
        /// Supports direct mode for single-writer, multireader file operations.
        /// </summary>
        DirectSingleWriterMultipleReader = 0x400000
    }

    /// <summary>
    /// Describes the event that has occurred.
    /// Typically, only one event is specified at a time.
    /// If more than one event is specified,
    /// the values contained in the dwItem1 and dwItem2 parameters must be the same,
    /// respectively, for all specified events.
    /// This parameter can be one or more of the following values:
    /// </summary>
    [Flags]
    public enum ShellObjectChangeTypes : uint
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// The name of a nonfolder item has changed.
        /// SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags.
        /// dwItem1 contains the previous PIDL or name of the item.
        /// dwItem2 contains the new PIDL or name of the item.
        /// </summary>
        ItemRename = 0x1,

        /// <summary>
        /// A nonfolder item has been created. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags.
        /// dwItem1 contains the item that was created.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        ItemCreate = 0x2,

        /// <summary>
        /// A nonfolder item has been deleted. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags.
        /// dwItem1 contains the item that was deleted.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        ItemDelete = 0x4,

        /// <summary>
        /// A folder has been created. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags.
        /// dwItem1 contains the folder that was created.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        DirectoryCreate = 0x8,

        /// <summary>
        /// A folder has been removed. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags.
        /// dwItem1 contains the folder that was removed.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        DirectoryDelete = 0x10,

        /// <summary>
        /// Storage media has been inserted into a drive. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags.
        /// dwItem1 contains the root of the drive that contains the new media.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        MediaInsert = 0x20,

        /// <summary>
        /// Storage media has been removed from a drive. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags.
        /// dwItem1 contains the root of the drive from which the media was removed.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        MediaRemove = 0x40,

        /// <summary>
        /// A drive has been removed. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags.
        /// dwItem1 contains the root of the drive that was removed.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        DriveRemove = 0x80,

        /// <summary>
        /// A drive has been added. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags.
        /// dwItem1 contains the root of the drive that was added.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        DriveAdd = 0x100,

        /// <summary>
        /// A folder on the local computer is being shared via the network.
        /// SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags.
        /// dwItem1 contains the folder that is being shared.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        NetShare = 0x200,

        /// <summary>
        /// A folder on the local computer is no longer being shared via the network.
        /// SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags.
        /// dwItem1 contains the folder that is no longer being shared.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        NetUnshare = 0x400,

        /// <summary>
        /// The attributes of an item or folder have changed.
        /// SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags.
        /// dwItem1 contains the item or folder that has changed.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        AttributesChange = 0x800,

        /// <summary>
        /// The contents of an existing folder have changed, but the folder still exists and has not been renamed.
        /// SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags.
        /// dwItem1 contains the folder that has changed.
        /// dwItem2 is not used and should be NULL.
        /// If a folder has been created, deleted, or renamed, use SHCNE_MKDIR, SHCNE_RMDIR, or SHCNE_RENAMEFOLDER, respectively.
        /// </summary>
        DirectoryContentsUpdate = 0x1000,

        /// <summary>
        /// An existing item (a folder or a nonfolder) has changed, but the item still exists and has not been renamed.
        /// SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags.
        /// dwItem1 contains the item that has changed.
        /// dwItem2 is not used and should be NULL.
        /// If a nonfolder item has been created, deleted, or renamed,
        /// use SHCNE_CREATE, SHCNE_DELETE, or SHCNE_RENAMEITEM, respectively, instead.
        /// </summary>
        Update = 0x2000,

        /// <summary>
        /// The computer has disconnected from a server.
        /// SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags.
        /// dwItem1 contains the server from which the computer was disconnected.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        ServerDisconnect = 0x4000,

        /// <summary>
        /// An image in the system image list has changed.
        /// SHCNF_DWORD must be specified in uFlags.
        /// dwItem1 is not used and should be NULL.
        /// dwItem2 contains the index in the system image list that has changed.
        /// </summary> //verify this is not opposite?
        SystemImageUpdate = 0x8000,

        /// <summary>
        /// The name of a folder has changed. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags.
        /// dwItem1 contains the previous PIDL or name of the folder.
        /// dwItem2 contains the new PIDL or name of the folder.
        /// </summary>
        DirectoryRename = 0x20000,

        /// <summary>
        /// The amount of free space on a drive has changed.
        /// SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags.
        /// dwItem1 contains the root of the drive on which the free space changed.
        /// dwItem2 is not used and should be NULL.
        /// </summary>
        FreeSpace = 0x40000,

        /// <summary>
        /// A file type association has changed.
        /// SHCNF_IDLIST must be specified in the uFlags parameter.
        /// dwItem1 and dwItem2 are not used and must be NULL.
        /// </summary>
        AssociationChange = 0x8000000,

        /// <summary>
        /// Specifies a combination of all of the disk event identifiers.
        /// </summary>
        DiskEventsMask = 0x2381F,

        /// <summary>
        /// Specifies a combination of all of the global event identifiers.
        /// </summary>
        GlobalEventsMask = 0xC0581E0,

        /// <summary>
        /// All events have occurred.
        /// </summary>
        AllEventsMask = 0x7FFFFFFF,

        /// <summary>
        /// The specified event occurred as a result of a system interrupt.
        /// As this value modifies other event values, it cannot be used alone.
        /// </summary>
        FromInterrupt = 0x80000000
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Stores information about how to sort a column that is displayed in the folder view.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SortColumn
    {

        /// <summary>
        /// Creates a sort column with the specified direction for the given property.
        /// </summary>
        /// <param name="propertyKey">Property key for the property that the user will sort.</param>
        /// <param name="direction">The direction in which the items are sorted.</param>
        public SortColumn(PropertyKey propertyKey, SortDirection direction)
        {
            m_propertyKey = propertyKey;
            m_direction = direction;
        }

        /// <summary>
        /// The ID of the column by which the user will sort. A PropertyKey structure.
        /// For example, for the "Name" column, the property key is PKEY_ItemNameDisplay or
        /// PropertySystem.SystemProperties.System.ItemName.
        /// </summary>
        public PropertyKey PropertyKey
        {
            get
            {
                return m_propertyKey;
            }

            set
            {
                m_propertyKey = value;
            }
        }

        private PropertyKey m_propertyKey;

        /// <summary>
        /// The direction in which the items are sorted.
        /// </summary>
        public SortDirection Direction
        {
            get
            {
                return m_direction;
            }

            set
            {
                m_direction = value;
            }
        }

        private SortDirection m_direction;


        /// <summary>
        /// Implements the == (equality) operator.
        /// </summary>
        /// <param name="col1">First object to compare.</param>
        /// <param name="col2">Second object to compare.</param>
        /// <returns>True if col1 equals col2; false otherwise.</returns>
        public static bool operator ==(SortColumn col1, SortColumn col2)
        {
            return col1.Direction == col2.Direction && col1.PropertyKey == col2.PropertyKey;
        }

        /// <summary>
        /// Implements the != (unequality) operator.
        /// </summary>
        /// <param name="col1">First object to compare.</param>
        /// <param name="col2">Second object to compare.</param>
        /// <returns>True if col1 does not equals col1; false otherwise.</returns>
        public static bool operator !=(SortColumn col1, SortColumn col2)
        {
            return !(col1 == col2);
        }

        /// <summary>
        /// Determines if this object is equal to another.
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns>Returns true if the objects are equal; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is null || !ReferenceEquals(obj.GetType(), typeof(SortColumn)))
            {
                return false;
            }

            return this == (SortColumn)obj;
        }

        /// <summary>
        /// Generates a nearly unique hashcode for this structure.
        /// </summary>
        /// <returns>A hash code.</returns>
        public override int GetHashCode()
        {
            int hash = m_direction.GetHashCode();
            hash = hash * 31 + m_propertyKey.GetHashCode();
            return hash;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct ThumbnailId
    {
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 16)]
        private byte rgbKey;
    }

    // Summary:
    // Defines a unique key for a Shell Property
    public struct PropertyKey : IEquatable<PropertyKey>
    {
        // 
        // Summary:
        // PropertyKey Constructor
        // 
        // Parameters:
        // formatId:
        // A unique GUID for the property
        // 
        // propertyId:
        // Property identifier (PID)
        public PropertyKey(Guid formatId, int propertyId)
        {
            _FormatId = formatId;
            _PropertyId = propertyId;
        }
        // 
        // Summary:
        // PropertyKey Constructor
        // 
        // Parameters:
        // formatId:
        // A string represenstion of a GUID for the property
        // 
        // propertyId:
        // Property identifier (PID)
        public PropertyKey(string formatId, int propertyId)
        {
            _FormatId = new Guid(formatId);
            _PropertyId = propertyId;
        }

        // Summary:
        // Implements the != (inequality) operator.
        // 
        // Parameters:
        // propKey1:
        // First property key to compare
        // 
        // propKey2:
        // Second property key to compare.
        // 
        // Returns:
        // true if object a does not equal object b. false otherwise.
        public static bool operator !=(PropertyKey propKey1, PropertyKey propKey2)
        {
            return !propKey1.Equals(propKey2);
        }
        // 
        // Summary:
        // Implements the == (equality) operator.
        // 
        // Parameters:
        // propKey1:
        // First property key to compare.
        // 
        // propKey2:
        // Second property key to compare.
        // 
        // Returns:
        // true if object a equals object b. false otherwise.
        public static bool operator ==(PropertyKey propKey1, PropertyKey propKey2)
        {
            return propKey1.Equals(propKey2);
        }

        // Summary:
        // A unique GUID for the property
        private Guid _FormatId;

        public Guid FormatId
        {
            get
            {
                return _FormatId;
            }
        }
        // 
        // Summary:
        // Property identifier (PID)
        private int _PropertyId;

        public int PropertyId
        {
            get
            {
                return _PropertyId;
            }
        }

        // Summary:
        // Returns whether this object is equal to another. This is vital for performance
        // of value types.
        // 
        // Parameters:
        // obj:
        // The object to compare against.
        // 
        // Returns:
        // Equality result.
        public override bool Equals(object obj)
        {
            return Equals((PropertyKey)obj);
        }
        // 
        // Summary:
        // Returns whether this object is equal to another. This is vital for performance
        // of value types.
        // 
        // Parameters:
        // other:
        // The object to compare against.
        // 
        // Returns:
        // Equality result.
        public bool Equals(PropertyKey other)
        {
            if (other.FormatId != _FormatId)
                return false;
            if (other.PropertyId != _PropertyId)
                return false;
            return true;
        }
        // 
        // Summary:
        // Returns the hash code of the object. This is vital for performance of value
        // types.
        public override int GetHashCode()
        {
            var i = default(int);
            var b = _FormatId.ToByteArray();
            foreach (var by in b)
                i += by;
            i += _PropertyId;
            return i;
        }
        // 
        // Summary:
        // Override ToString() to provide a user friendly string representation
        // 
        // Returns:
        // String representing the property key
        public override string ToString()
        {
            return _FormatId.ToString("B").ToUpper() + "[" + _PropertyId + "]";
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SHITEMID
    {
        public ushort cb;
        public Guid ItemId;
    }


    #region COM Interfaces

    [ComImport(),
    Guid(ShellIIDGuid.IModalWindow),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IModalWindow
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
        PreserveSig]
        int Show([In] IntPtr parent);
    }

    [ComImport,
    Guid(ShellIIDGuid.IShellItem),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IShellItem
    {
        // Not supported: IBindCtx.
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult BindToHandler(
            [In] IntPtr pbc,
            [In] ref Guid bhid,
            [In] ref Guid riid,
            [Out, MarshalAs(UnmanagedType.Interface)] out IShellFolder ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetParent([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetDisplayName(
            [In] ShellItemDesignNameOptions sigdnName,
            out IntPtr ppszName);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetAttributes([In] ShellFileGetAttributesOptions sfgaoMask, out ShellFileGetAttributesOptions psfgaoAttribs);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult Compare(
            [In, MarshalAs(UnmanagedType.Interface)] IShellItem psi,
            [In] SICHINTF hint,
            out int piOrder);
    }

    [ComImport,
    Guid(ShellIIDGuid.IShellItem2),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IShellItem2 : IShellItem
    {
        // Not supported: IBindCtx.
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult BindToHandler(
            [In] IntPtr pbc,
            [In] ref Guid bhid,
            [In] ref Guid riid,
            [Out, MarshalAs(UnmanagedType.Interface)] out IShellFolder ppv);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetParent([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetDisplayName(
            [In] ShellItemDesignNameOptions sigdnName,
            [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetAttributes([In] ShellFileGetAttributesOptions sfgaoMask, out ShellFileGetAttributesOptions psfgaoAttribs);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Compare(
            [In, MarshalAs(UnmanagedType.Interface)] IShellItem psi,
            [In] uint hint,
            out int piOrder);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), PreserveSig]
        int GetPropertyStore(
            [In] GetPropertyStoreOptions Flags,
            [In] ref Guid riid,
            [Out, MarshalAs(UnmanagedType.Interface)] out IPropertyStore ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetPropertyStoreWithCreateObject([In] GetPropertyStoreOptions Flags, [In, MarshalAs(UnmanagedType.IUnknown)] object punkCreateObject, [In] ref Guid riid, out IntPtr ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetPropertyStoreForKeys([In] ref PropertyKey rgKeys, [In] uint cKeys, [In] GetPropertyStoreOptions Flags, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.IUnknown)] out IPropertyStore ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetPropertyDescriptionList([In] ref PropertyKey keyType, [In] ref Guid riid, out IntPtr ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult Update([In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetProperty([In] ref PropertyKey key, [Out] PropVariant ppropvar);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetCLSID([In] ref PropertyKey key, out Guid pclsid);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetFileTime([In] ref PropertyKey key, out System.Runtime.InteropServices.ComTypes.FILETIME pft);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetInt32([In] ref PropertyKey key, out int pi);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetString([In] ref PropertyKey key, [MarshalAs(UnmanagedType.LPWStr)] out string ppsz);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetUInt32([In] ref PropertyKey key, out uint pui);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetUInt64([In] ref PropertyKey key, out ulong pull);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetBool([In] ref PropertyKey key, out int pf);
    }

    [ComImport,
    Guid(ShellIIDGuid.IShellItemArray),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IShellItemArray
    {
        // Not supported: IBindCtx.
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult BindToHandler(
            [In, MarshalAs(UnmanagedType.Interface)] IntPtr pbc,
            [In] ref Guid rbhid,
            [In] ref Guid riid,
            out IntPtr ppvOut);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetPropertyStore(
            [In] int Flags,
            [In] ref Guid riid,
            out IntPtr ppv);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetPropertyDescriptionList(
            [In] ref PropertyKey keyType,
            [In] ref Guid riid,
            out IntPtr ppv);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetAttributes(
            [In] ShellItemAttributeOptions dwAttribFlags,
            [In] ShellFileGetAttributesOptions sfgaoMask,
            out ShellFileGetAttributesOptions psfgaoAttribs);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetCount(out uint pdwNumItems);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetItemAt(
            [In] uint dwIndex,
            [MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

        // Not supported: IEnumShellItems (will use GetCount and GetItemAt instead).
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult EnumItems([MarshalAs(UnmanagedType.Interface)] out IntPtr ppenumShellItems);
    }

    [ComImport,
    Guid(ShellIIDGuid.IShellLibrary),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IShellLibrary
    {
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult LoadLibraryFromItem(
            [In, MarshalAs(UnmanagedType.Interface)] IShellItem library,
            [In] AccessModes grfMode);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void LoadLibraryFromKnownFolder(
            [In] ref Guid knownfidLibrary,
            [In] AccessModes grfMode);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void AddFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem location);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void RemoveFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem location);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult GetFolders(
            [In] LibraryFolderFilter lff,
            [In] ref Guid riid,
            [MarshalAs(UnmanagedType.Interface)] out IShellItemArray ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void ResolveFolder(
            [In, MarshalAs(UnmanagedType.Interface)] IShellItem folderToResolve,
            [In] uint timeout,
            [In] ref Guid riid,
            [MarshalAs(UnmanagedType.Interface)] out IShellItem ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetDefaultSaveFolder(
            [In] DefaultSaveFolderType dsft,
            [In] ref Guid riid,
            [MarshalAs(UnmanagedType.Interface)] out IShellItem ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetDefaultSaveFolder(
            [In] DefaultSaveFolderType dsft,
            [In, MarshalAs(UnmanagedType.Interface)] IShellItem si);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetOptions(
            out LibraryOptions lofOptions);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetOptions(
            [In] LibraryOptions lofMask,
            [In] LibraryOptions lofOptions);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetFolderType(out Guid ftid);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetFolderType([In] ref Guid ftid);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetIcon([MarshalAs(UnmanagedType.LPWStr)] out string icon);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetIcon([In, MarshalAs(UnmanagedType.LPWStr)] string icon);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Commit();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Save(
            [In, MarshalAs(UnmanagedType.Interface)] IShellItem folderToSaveIn,
            [In, MarshalAs(UnmanagedType.LPWStr)] string libraryName,
            [In] LibrarySaveOptions lsf,
            [MarshalAs(UnmanagedType.Interface)] out IShellItem2 savedTo);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SaveInKnownFolder(
            [In] ref Guid kfidToSaveIn,
            [In, MarshalAs(UnmanagedType.LPWStr)] string libraryName,
            [In] LibrarySaveOptions lsf,
            [MarshalAs(UnmanagedType.Interface)] out IShellItem2 savedTo);
    };

    [ComImportAttribute()]
    [GuidAttribute("bcc18b79-ba16-442f-80c4-8a59c30c463b")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    interface IShellItemImageFactory
    {
        [PreserveSig]
        HResult GetImage(
        [In, MarshalAs(UnmanagedType.Struct)] Size size,
        [In] SIIGBF flags,
        [Out] out IntPtr phbm);
    }

    [ComImport,
    Guid(ShellIIDGuid.IThumbnailCache),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IThumbnailCache
    {
        void GetThumbnail([In] IShellItem pShellItem,
        [In] uint cxyRequestedThumbSize,
        [In] ThumbnailOptions flags,
        [Out] out ISharedBitmap ppvThumb,
        [Out] out ThumbnailCacheOptions pOutFlags,
        [Out] ThumbnailId pThumbnailID);

        void GetThumbnailByID([In] ThumbnailId thumbnailID,
        [In] uint cxyRequestedThumbSize,
        [Out] out ISharedBitmap ppvThumb,
        [Out] out ThumbnailCacheOptions pOutFlags);
    }

    [ComImport,
    Guid(ShellIIDGuid.ISharedBitmap),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface ISharedBitmap
    {
        void GetSharedBitmap([Out] out IntPtr phbm);
        void GetSize([Out] out Size pSize);
        void GetFormat([Out] out ThumbnailAlphaType pat);
        void InitializeBitmap([In] IntPtr hbm, [In] ThumbnailAlphaType wtsAT);
        void Detach([Out] out IntPtr phbm);
    }
    [ComImport,
    Guid(ShellIIDGuid.IShellFolder),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    ComConversionLoss]
    internal interface IShellFolder
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void ParseDisplayName(IntPtr hwnd, [In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, [In, MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, [In, Out] ref uint pchEaten, [Out] IntPtr ppidl, [In, Out] ref uint pdwAttributes);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult EnumObjects([In] IntPtr hwnd, [In] ShellFolderEnumerationOptions grfFlags, [MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenumIDList);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult BindToObject([In] IntPtr pidl, /*[In, MarshalAs(UnmanagedType.Interface)] IBindCtx*/ IntPtr pbc, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out IShellFolder ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void BindToStorage([In] ref IntPtr pidl, [In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, [In] ref Guid riid, out IntPtr ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void CompareIDs([In] IntPtr lParam, [In] ref IntPtr pidl1, [In] ref IntPtr pidl2);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void CreateViewObject([In] IntPtr hwndOwner, [In] ref Guid riid, out IntPtr ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetAttributesOf([In] uint cidl, [In] IntPtr apidl, [In, Out] ref uint rgfInOut);


        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetUIObjectOf([In] IntPtr hwndOwner, [In] uint cidl, [In] IntPtr apidl, [In] ref Guid riid, [In, Out] ref uint rgfReserved, out IntPtr ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetDisplayNameOf([In] IntPtr pidl, [In] uint uFlags, IntPtr pName);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetNameOf([In] IntPtr hwnd, [In] ref IntPtr pidl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszName, [In] uint uFlags, [Out] IntPtr ppidlOut);
    }

    [ComImport,
    Guid(ShellIIDGuid.IShellFolder2),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    ComConversionLoss]
    internal interface IShellFolder2 : IShellFolder
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void ParseDisplayName([In] IntPtr hwnd, [In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, [In, MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, [In, Out] ref uint pchEaten, [Out] IntPtr ppidl, [In, Out] ref uint pdwAttributes);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void EnumObjects([In] IntPtr hwnd, [In] ShellFolderEnumerationOptions grfFlags, [MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenumIDList);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void BindToObject([In] IntPtr pidl, /*[In, MarshalAs(UnmanagedType.Interface)] IBindCtx*/ IntPtr pbc, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out IShellFolder ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void BindToStorage([In] ref IntPtr pidl, [In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, [In] ref Guid riid, out IntPtr ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void CompareIDs([In] IntPtr lParam, [In] ref IntPtr pidl1, [In] ref IntPtr pidl2);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void CreateViewObject([In] IntPtr hwndOwner, [In] ref Guid riid, out IntPtr ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetAttributesOf([In] uint cidl, [In] IntPtr apidl, [In, Out] ref uint rgfInOut);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetUIObjectOf([In] IntPtr hwndOwner, [In] uint cidl, [In] IntPtr apidl, [In] ref Guid riid, [In, Out] ref uint rgfReserved, out IntPtr ppv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetDisplayNameOf([In] ref IntPtr pidl, [In] uint uFlags, out IntPtr pName);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetNameOf([In] IntPtr hwnd, [In] ref IntPtr pidl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszName, [In] uint uFlags, [Out] IntPtr ppidlOut);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetDefaultSearchGUID(out Guid pguid);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void EnumSearches([Out] out IntPtr ppenum);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetDefaultColumn([In] uint dwRes, out uint pSort, out uint pDisplay);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetDefaultColumnState([In] uint iColumn, out uint pcsFlags);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetDetailsEx([In] ref IntPtr pidl, [In] ref PropertyKey pscid, [MarshalAs(UnmanagedType.Struct)] out object pv);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetDetailsOf([In] ref IntPtr pidl, [In] uint iColumn, out IntPtr psd);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void MapColumnToSCID([In] uint iColumn, out PropertyKey pscid);
    }

    [ComImport,
    Guid(ShellIIDGuid.IEnumIDList),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IEnumIDList
    {
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult Next(uint celt, out IntPtr rgelt, out uint pceltFetched);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult Skip([In] uint celt);

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult Reset();

        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        HResult Clone([MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenum);
    }

    [ComImport,
    Guid(ShellIIDGuid.IShellLinkW),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IShellLinkW
    {
        void GetPath(
            [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile,
            int cchMaxPath,
            //ref _WIN32_FIND_DATAW pfd,
            IntPtr pfd,
            uint fFlags);
        void GetIDList(out IntPtr ppidl);
        void SetIDList(IntPtr pidl);
        void GetDescription(
            [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile,
            int cchMaxName);
        void SetDescription(
            [MarshalAs(UnmanagedType.LPWStr)] string pszName);
        void GetWorkingDirectory(
            [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir,
            int cchMaxPath
            );
        void SetWorkingDirectory(
            [MarshalAs(UnmanagedType.LPWStr)] string pszDir);
        void GetArguments(
            [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs,
            int cchMaxPath);
        void SetArguments(
            [MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
        void GetHotKey(out short wHotKey);
        void SetHotKey(short wHotKey);
        void GetShowCmd(out uint iShowCmd);
        void SetShowCmd(uint iShowCmd);
        void GetIconLocation(
            [Out(), MarshalAs(UnmanagedType.LPWStr)] out StringBuilder pszIconPath,
            int cchIconPath,
            out int iIcon);
        void SetIconLocation(
            [MarshalAs(UnmanagedType.LPWStr)] string pszIconPath,
            int iIcon);
        void SetRelativePath(
            [MarshalAs(UnmanagedType.LPWStr)] string pszPathRel,
            uint dwReserved);
        void Resolve(IntPtr hwnd, uint fFlags);
        void SetPath(
            [MarshalAs(UnmanagedType.LPWStr)] string pszFile);
    }

    [ComImport,
    Guid(ShellIIDGuid.CShellLink),
    ClassInterface(ClassInterfaceType.None)]
    internal class CShellLink { }

    // Summary:
    //     Provides the managed definition of the IPersistStream interface, with functionality
    //     from IPersist.
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00000109-0000-0000-C000-000000000046")]
    internal interface IPersistStream
    {
        // Summary:
        //     Retrieves the class identifier (CLSID) of an object.
        //
        // Parameters:
        //   pClassID:
        //     When this method returns, contains a reference to the CLSID. This parameter
        //     is passed uninitialized.
        [PreserveSig]
        void GetClassID(out Guid pClassID);
        //
        // Summary:
        //     Checks an object for changes since it was last saved to its current file.
        //
        // Returns:
        //     S_OK if the file has changed since it was last saved; S_FALSE if the file
        //     has not changed since it was last saved.
        [PreserveSig]
        HResult IsDirty();

        [PreserveSig]
        HResult Load([In, MarshalAs(UnmanagedType.Interface)] IStream stm);

        [PreserveSig]
        HResult Save([In, MarshalAs(UnmanagedType.Interface)] IStream stm, bool fRemember);

        [PreserveSig]
        HResult GetSizeMax(out ulong cbSize);
    }

    [ComImport(),
    Guid(ShellIIDGuid.ICondition),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ICondition : IPersistStream
    {
        // Summary:
        //     Retrieves the class identifier (CLSID) of an object.
        //
        // Parameters:
        //   pClassID:
        //     When this method returns, contains a reference to the CLSID. This parameter
        //     is passed uninitialized.
        [PreserveSig]
        void GetClassID(out Guid pClassID);
        //
        // Summary:
        //     Checks an object for changes since it was last saved to its current file.
        //
        // Returns:
        //     S_OK if the file has changed since it was last saved; S_FALSE if the file
        //     has not changed since it was last saved.
        [PreserveSig]
        HResult IsDirty();

        [PreserveSig]
        HResult Load([In, MarshalAs(UnmanagedType.Interface)] IStream stm);

        [PreserveSig]
        HResult Save([In, MarshalAs(UnmanagedType.Interface)] IStream stm, bool fRemember);

        [PreserveSig]
        HResult GetSizeMax(out ulong cbSize);

        // For any node, return what kind of node it is.
        [PreserveSig]
        HResult GetConditionType([Out()] out SearchConditionType pNodeType);

        // riid must be IID_IEnumUnknown, IID_IEnumVARIANT or IID_IObjectArray, or in the case of a negation node IID_ICondition.
        // If this is a leaf node, E_FAIL will be returned.
        // If this is a negation node, then if riid is IID_ICondition, *ppv will be set to a single ICondition, otherwise an enumeration of one.
        // If this is a conjunction or a disjunction, *ppv will be set to an enumeration of the subconditions.
        [PreserveSig]
        HResult GetSubConditions([In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out object ppv);

        // If this is not a leaf node, E_FAIL will be returned.
        // Retrieve the property name, operation and value from the leaf node.
        // Any one of ppszPropertyName, pcop and ppropvar may be NULL.
        [PreserveSig]
        HResult GetComparisonInfo(
            [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszPropertyName,
            [Out] out SearchConditionOperation pcop,
            [Out] PropVariant ppropvar);

        // If this is not a leaf node, E_FAIL will be returned.
        // *ppszValueTypeName will be set to the semantic type of the value, or to NULL if this is not meaningful.
        [PreserveSig]
        HResult GetValueType([Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszValueTypeName);

        // If this is not a leaf node, E_FAIL will be returned.
        // If the value of the leaf node is VT_EMPTY, *ppszNormalization will be set to an empty string.
        // If the value is a string (VT_LPWSTR, VT_BSTR or VT_LPSTR), then *ppszNormalization will be set to a
        // character-normalized form of the value.
        // Otherwise, *ppszNormalization will be set to some (character-normalized) string representation of the value.
        [PreserveSig]
        HResult GetValueNormalization([Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszNormalization);

        // Return information about what parts of the input produced the property, the operation and the value.
        // Any one of ppPropertyTerm, ppOperationTerm and ppValueTerm may be NULL.
        // For a leaf node returned by the parser, the position information of each IRichChunk identifies the tokens that
        // contributed the property/operation/value, the string value is the corresponding part of the input string, and
        // the PROPVARIANT is VT_EMPTY.
        [PreserveSig]
        HResult GetInputTerms([Out] out IRichChunk ppPropertyTerm, [Out] out IRichChunk ppOperationTerm, [Out] out IRichChunk ppValueTerm);

        // Make a deep copy of this ICondition.
        [PreserveSig]
        HResult Clone([Out()] out ICondition ppc);
    };

    [ComImport,
    Guid(ShellIIDGuid.IRichChunk),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IRichChunk
    {
        // The position *pFirstPos is zero-based.
        // Any one of pFirstPos, pLength, ppsz and pValue may be NULL.
        [PreserveSig]
        HResult GetData(/*[out, annotation("__out_opt")] ULONG* pFirstPos, [out, annotation("__out_opt")] ULONG* pLength, [out, annotation("__deref_opt_out_opt")] LPWSTR* ppsz, [out, annotation("__out_opt")] PROPVARIANT* pValue*/);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(ShellIIDGuid.IEnumUnknown)]
    internal interface IEnumUnknown
    {
        [PreserveSig]
        HResult Next(UInt32 requestedNumber, ref IntPtr buffer, ref UInt32 fetchedNumber);
        [PreserveSig]
        HResult Skip(UInt32 number);
        [PreserveSig]
        HResult Reset();
        [PreserveSig]
        HResult Clone(out IEnumUnknown result);
    }


    [ComImport,
    Guid(ShellIIDGuid.IConditionFactory),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IConditionFactory
    {
        [PreserveSig]
        HResult MakeNot([In] ICondition pcSub, [In] bool fSimplify, [Out] out ICondition ppcResult);

        [PreserveSig]
        HResult MakeAndOr([In] SearchConditionType ct, [In] IEnumUnknown peuSubs, [In] bool fSimplify, [Out] out ICondition ppcResult);

        [PreserveSig]
        HResult MakeLeaf(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pszPropertyName,
            [In] SearchConditionOperation cop,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pszValueType,
            [In] PropVariant ppropvar,
            IRichChunk richChunk1,
            IRichChunk richChunk2,
            IRichChunk richChunk3,
            [In] bool fExpand,
            [Out] out ICondition ppcResult);

        [PreserveSig]
        HResult Resolve(/*[In] ICondition pc, [In] STRUCTURED_QUERY_RESOLVE_OPTION sqro, [In] ref SYSTEMTIME pstReferenceTime, [Out] out ICondition ppcResolved*/);

    };

    [ComImport,
    Guid(ShellIIDGuid.IConditionFactory),
    CoClass(typeof(ConditionFactoryCoClass))]
    internal interface INativeConditionFactory : IConditionFactory
    {
    }

    [ComImport,
    ClassInterface(ClassInterfaceType.None),
    TypeLibType(TypeLibTypeFlags.FCanCreate),
    Guid(ShellCLSIDGuid.ConditionFactory)]
    internal class ConditionFactoryCoClass
    {
    }



    [ComImport,
    Guid(ShellIIDGuid.ISearchFolderItemFactory),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ISearchFolderItemFactory
    {
        [PreserveSig]
        HResult SetDisplayName([In, MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName);

        [PreserveSig]
        HResult SetFolderTypeID([In] Guid ftid);

        [PreserveSig]
        HResult SetFolderLogicalViewMode([In] FolderLogicalViewMode flvm);

        [PreserveSig]
        HResult SetIconSize([In] int iIconSize);

        [PreserveSig]
        HResult SetVisibleColumns([In] uint cVisibleColumns, [In, MarshalAs(UnmanagedType.LPArray)] PropertyKey[] rgKey);

        [PreserveSig]
        HResult SetSortColumns([In] uint cSortColumns, [In, MarshalAs(UnmanagedType.LPArray)] SortColumn[] rgSortColumns);

        [PreserveSig]
        HResult SetGroupColumn([In] ref PropertyKey keyGroup);

        [PreserveSig]
        HResult SetStacks([In] uint cStackKeys, [In, MarshalAs(UnmanagedType.LPArray)] PropertyKey[] rgStackKeys);

        [PreserveSig]
        HResult SetScope([In, MarshalAs(UnmanagedType.Interface)] IShellItemArray ppv);

        [PreserveSig]
        HResult SetCondition([In] ICondition pCondition);

        [PreserveSig]
        int GetShellItem(ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out IShellItem ppv);

        [PreserveSig]
        HResult GetIDList([Out] IntPtr ppidl);
    };

    [ComImport,
    Guid(ShellIIDGuid.ISearchFolderItemFactory),
    CoClass(typeof(SearchFolderItemFactoryCoClass))]
    internal interface INativeSearchFolderItemFactory : ISearchFolderItemFactory
    {
    }

    [ComImport,
    ClassInterface(ClassInterfaceType.None),
    TypeLibType(TypeLibTypeFlags.FCanCreate),
    Guid(ShellCLSIDGuid.SearchFolderItemFactory)]
    internal class SearchFolderItemFactoryCoClass
    {
    }

    [ComImport,
    Guid(ShellIIDGuid.IQuerySolution),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IQuerySolution : IConditionFactory
    {
        [PreserveSig]
        HResult MakeNot([In] ICondition pcSub, [In] bool fSimplify, [Out] out ICondition ppcResult);

        [PreserveSig]
        HResult MakeAndOr([In] SearchConditionType ct, [In] IEnumUnknown peuSubs, [In] bool fSimplify, [Out] out ICondition ppcResult);

        [PreserveSig]
        HResult MakeLeaf(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pszPropertyName,
            [In] SearchConditionOperation cop,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pszValueType,
            [In] PropVariant ppropvar,
            IRichChunk richChunk1,
            IRichChunk richChunk2,
            IRichChunk richChunk3,
            [In] bool fExpand,
            [Out] out ICondition ppcResult);

        [PreserveSig]
        HResult Resolve(/*[In] ICondition pc, [In] int sqro, [In] ref SYSTEMTIME pstReferenceTime, [Out] out ICondition ppcResolved*/);

        // Retrieve the condition tree and the "main type" of the solution.
        // ppQueryNode and ppMainType may be NULL.
        [PreserveSig]
        HResult GetQuery([Out, MarshalAs(UnmanagedType.Interface)] out ICondition ppQueryNode, [Out, MarshalAs(UnmanagedType.Interface)] out IEntity ppMainType);

        // Identify parts of the input string not accounted for.
        // Each parse error is represented by an IRichChunk where the position information
        // reflect token counts, the string is NULL and the value is a VT_I4
        // where lVal is from the ParseErrorType enumeration. The valid
        // values for riid are IID_IEnumUnknown and IID_IEnumVARIANT.
        [PreserveSig]
        HResult GetErrors([In] ref Guid riid, [Out] out /* void** */ IntPtr ppParseErrors);

        // Report the query string, how it was tokenized and what LCID and word breaker were used (for recognizing keywords).
        // ppszInputString, ppTokens, pLocale and ppWordBreaker may be NULL.
        [PreserveSig]
        HResult GetLexicalData([MarshalAs(UnmanagedType.LPWStr)] out string ppszInputString, [Out] /* ITokenCollection** */ out IntPtr ppTokens, [Out] out uint plcid, [Out] /* IUnknown** */ out IntPtr ppWordBreaker);
    }

    [ComImport,
    Guid(ShellIIDGuid.IQueryParser),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IQueryParser
    {
        // Parse parses an input string, producing a query solution.
        // pCustomProperties should be an enumeration of IRichChunk objects, one for each custom property
        // the application has recognized. pCustomProperties may be NULL, equivalent to an empty enumeration.
        // For each IRichChunk, the position information identifies the character span of the custom property,
        // the string value should be the name of an actual property, and the PROPVARIANT is completely ignored.
        [PreserveSig]
        HResult Parse([In, MarshalAs(UnmanagedType.LPWStr)] string pszInputString, [In] IEnumUnknown pCustomProperties, [Out] out IQuerySolution ppSolution);

        // Set a single option. See STRUCTURED_QUERY_SINGLE_OPTION above.
        [PreserveSig]
        HResult SetOption([In] StructuredQuerySingleOption option, [In] PropVariant pOptionValue);

        [PreserveSig]
        HResult GetOption([In] StructuredQuerySingleOption option, [Out] PropVariant pOptionValue);

        // Set a multi option. See STRUCTURED_QUERY_MULTIOPTION above.
        [PreserveSig]
        HResult SetMultiOption([In] StructuredQueryMultipleOption option, [In, MarshalAs(UnmanagedType.LPWStr)] string pszOptionKey, [In] PropVariant pOptionValue);

        // Get a schema provider for browsing the currently loaded schema.
        [PreserveSig]
        HResult GetSchemaProvider([Out] out /*ISchemaProvider*/ IntPtr ppSchemaProvider);

        // Restate a condition as a query string according to the currently selected syntax.
        // The parameter fUseEnglish is reserved for future use; must be FALSE.
        [PreserveSig]
        HResult RestateToString([In] ICondition pCondition, [In] bool fUseEnglish, [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszQueryString);

        // Parse a condition for a given property. It can be anything that would go after 'PROPERTY:' in an AQS expession.
        [PreserveSig]
        HResult ParsePropertyValue([In, MarshalAs(UnmanagedType.LPWStr)] string pszPropertyName, [In, MarshalAs(UnmanagedType.LPWStr)] string pszInputString, [Out] out IQuerySolution ppSolution);

        // Restate a condition for a given property. If the condition contains a leaf with any other property name, or no property name at all,
        // E_INVALIDARG will be returned.
        [PreserveSig]
        HResult RestatePropertyValueToString([In] ICondition pCondition, [In] bool fUseEnglish, [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszPropertyName, [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszQueryString);
    }

    [ComImport,
    Guid(ShellIIDGuid.IQueryParserManager),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IQueryParserManager
    {
        // Create a query parser loaded with the schema for a certain catalog localize to a certain language, and initialized with
        // standard defaults. One valid value for riid is IID_IQueryParser.
        [PreserveSig]
        HResult CreateLoadedParser([In, MarshalAs(UnmanagedType.LPWStr)] string pszCatalog, [In] ushort langidForKeywords, [In] ref Guid riid, [Out] out IQueryParser ppQueryParser);

        // In addition to setting AQS/NQS and automatic wildcard for the given query parser, this sets up standard named entity handlers and
        // sets the keyboard locale as locale for word breaking.
        [PreserveSig]
        HResult InitializeOptions([In] bool fUnderstandNQS, [In] bool fAutoWildCard, [In] IQueryParser pQueryParser);

        // Change one of the settings for the query parser manager, such as the name of the schema binary, or the location of the localized and unlocalized
        // schema binaries. By default, the settings point to the schema binaries used by Windows Shell.
        [PreserveSig]
        HResult SetOption([In] QueryParserManagerOption option, [In] PropVariant pOptionValue);

    };

    [ComImport,
    Guid(ShellIIDGuid.IQueryParserManager),
    CoClass(typeof(QueryParserManagerCoClass))]
    internal interface INativeQueryParserManager : IQueryParserManager
    {
    }

    [ComImport,
    ClassInterface(ClassInterfaceType.None),
    TypeLibType(TypeLibTypeFlags.FCanCreate),
    Guid(ShellCLSIDGuid.QueryParserManager)]
    internal class QueryParserManagerCoClass
    {
    }

    [ComImport,
    Guid("24264891-E80B-4fd3-B7CE-4FF2FAE8931F"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IEntity
    {
        // TODO
    }
    #endregion
    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    public static class NativeShell
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
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = FileApi.MAX_PATH)]
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


    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    public enum LibraryFolderFilter
    {
        ForceFileSystem = 1,
        StorageItems = 2,
        AllItems = 3
    }

    [Flags]
    public enum LibraryOptions
    {
        Default = 0,
        PinnedToNavigationPane = 0x1,
        MaskAll = 0x1
    }

    public enum DefaultSaveFolderType
    {
        Detect = 1,
        Private = 2,
        Public = 3
    }

    public enum LibrarySaveOptions
    {
        FailIfThere = 0,
        OverrideExisting = 1,
        MakeUniqueName = 2
    }

    public enum LibraryManageDialogOptions
    {
        Default = 0,
        NonIndexableLocationWarning = 1
    }


    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    public sealed class PropertySystemNativeMethods
    {
        private PropertySystemNativeMethods()
        {
        }
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public enum RelativeDescriptionType
        {
            General,
            Date,
            Size,
            Count,
            Revision,
            Length,
            Duration,
            Speed,
            Rate,
            Rating,
            Priority
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int PSGetNameFromPropertyKey(ref PropertyKey propkey, [MarshalAs(UnmanagedType.LPWStr)] out string ppszCanonicalName);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern HResult PSGetPropertyDescription(ref PropertyKey propkey, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IPropertyDescription ppv);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int PSGetPropertyKeyFromName([In][MarshalAs(UnmanagedType.LPWStr)] string pszCanonicalName, ref PropertyKey propkey);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int PSGetPropertyDescriptionListFromString([In][MarshalAs(UnmanagedType.LPWStr)] string pszPropList, [In] ref Guid riid, ref IPropertyDescriptionList ppv);



        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Property store cache state
    /// </summary>
    public enum PropertyStoreCacheState
    {
        /// <summary>
        /// Contained in file, not updated.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Not contained in file.
        /// </summary>
        NotInSource = 1,

        /// <summary>
        /// Contained in file, has been updated since file was consumed.
        /// </summary>
        Dirty = 2
    }

    /// <summary>
    /// Delineates the format of a property string.
    /// </summary>
    /// <remarks>
    /// Typically use one, or a bitwise combination of
    /// these flags, to specify the format. Some flags are mutually exclusive,
    /// so combinations like <c>ShortTime | LongTime | HideTime</c> are not allowed.
    /// </remarks>
    [Flags]
    public enum PropertyDescriptionFormatOptions
    {
        /// <summary>
        /// The format settings specified in the property's .propdesc file.
        /// </summary>
        None = 0,

        /// <summary>
        /// The value preceded with the property's display name.
        /// </summary>
        /// <remarks>
        /// This flag is ignored when the <c>hideLabelPrefix</c> attribute of the <c>labelInfo</c> element
        /// in the property's .propinfo file is set to true.
        /// </remarks>
        PrefixName = 0x1,

        /// <summary>
        /// The string treated as a file name.
        /// </summary>
        FileName = 0x2,

        /// <summary>
        /// The sizes displayed in kilobytes (KB), regardless of size.
        /// </summary>
        /// <remarks>
        /// This flag applies to properties of <c>Integer</c> types and aligns the values in the column.
        /// </remarks>
        AlwaysKB = 0x4,

        /// <summary>
        /// Reserved.
        /// </summary>
        RightToLeft = 0x8,

        /// <summary>
        /// The time displayed as 'hh:mm am/pm'.
        /// </summary>
        ShortTime = 0x10,

        /// <summary>
        /// The time displayed as 'hh:mm:ss am/pm'.
        /// </summary>
        LongTime = 0x20,

        /// <summary>
        /// The time portion of date/time hidden.
        /// </summary>
        HideTime = 64,

        /// <summary>
        /// The date displayed as 'MM/DD/YY'. For example, '3/21/04'.
        /// </summary>
        ShortDate = 0x80,

        /// <summary>
        /// The date displayed as 'DayOfWeek Month day, year'.
        /// For example, 'Monday, March 21, 2004'.
        /// </summary>
        LongDate = 0x100,

        /// <summary>
        /// The date portion of date/time hidden.
        /// </summary>
        HideDate = 0x200,

        /// <summary>
        /// The friendly date descriptions, such as "Yesterday".
        /// </summary>
        RelativeDate = 0x400,

        /// <summary>
        /// The text displayed in a text box as a cue for the user, such as 'Enter your name'.
        /// </summary>
        /// <remarks>
        /// The invitation text is returned if formatting failed or the value was empty.
        /// Invitation text is text displayed in a text box as a cue for the user,
        /// Formatting can fail if the data entered
        /// is not of an expected type, such as putting alpha characters in
        /// a phone number field.
        /// </remarks>
        UseEditInvitation = 0x800,

        /// <summary>
        /// This flag requires UseEditInvitation to also be specified. When the
        /// formatting flags are ReadOnly | UseEditInvitation and the algorithm
        /// would have shown invitation text, a string is returned that indicates
        /// the value is "Unknown" instead of the invitation text.
        /// </summary>
        ReadOnly = 0x1000,

        /// <summary>
        /// The detection of the reading order is not automatic. Useful when converting
        /// to ANSI to omit the Unicode reading order characters.
        /// </summary>
        NoAutoReadingOrder = 0x2000,

        /// <summary>
        /// Smart display of DateTime values
        /// </summary>
        SmartDateTime = 0x4000
    }

    /// <summary>
    /// Specifies the display types for a property.
    /// </summary>
    public enum PropertyDisplayType
    {
        /// <summary>
        /// The String Display. This is the default if the property doesn't specify a display type.
        /// </summary>
        String = 0,

        /// <summary>
        /// The Number Display.
        /// </summary>
        Number = 1,

        /// <summary>
        /// The Boolean Display.
        /// </summary>
        Boolean = 2,

        /// <summary>
        /// The DateTime Display.
        /// </summary>
        DateTime = 3,

        /// <summary>
        /// The Enumerated Display.
        /// </summary>
        Enumerated = 4
    }

    /// <summary>
    /// Property Aggregation Type
    /// </summary>
    public enum PropertyAggregationType
    {
        /// <summary>
        /// The string "Multiple Values" is displayed.
        /// </summary>
        Default = 0,

        /// <summary>
        /// The first value in the selection is displayed.
        /// </summary>
        First = 1,

        /// <summary>
        /// The sum of the selected values is displayed. This flag is never returned
        /// for data types VT_LPWSTR, VT_BOOL, and VT_FILETIME.
        /// </summary>
        Sum = 2,

        /// <summary>
        /// The numerical average of the selected values is displayed. This flag
        /// is never returned for data types VT_LPWSTR, VT_BOOL, and VT_FILETIME.
        /// </summary>
        Average = 3,

        /// <summary>
        /// The date range of the selected values is displayed. This flag is only
        /// returned for values of the VT_FILETIME data type.
        /// </summary>
        DateRange = 4,

        /// <summary>
        /// A concatenated string of all the values is displayed. The order of
        /// individual values in the string is undefined. The concatenated
        /// string omits duplicate values; if a value occurs more than once,
        /// it only appears a single time in the concatenated string.
        /// </summary>
        Union = 5,

        /// <summary>
        /// The highest of the selected values is displayed.
        /// </summary>
        Max = 6,

        /// <summary>
        /// The lowest of the selected values is displayed.
        /// </summary>
        Min = 7
    }

    /// <summary>
    /// Property Enumeration Types
    /// </summary>
    public enum PropEnumType
    {
        /// <summary>
        /// Use DisplayText and either RangeMinValue or RangeSetValue.
        /// </summary>
        DiscreteValue = 0,

        /// <summary>
        /// Use DisplayText and either RangeMinValue or RangeSetValue
        /// </summary>
        RangedValue = 1,

        /// <summary>
        /// Use DisplayText
        /// </summary>
        DefaultValue = 2,

        /// <summary>
        /// Use Value or RangeMinValue
        /// </summary>
        EndRange = 3
    }

    /// <summary>
    /// Describes how a property should be treated for display purposes.
    /// </summary>
    [Flags]
    public enum PropertyColumnStateOptions
    {
        /// <summary>
        /// Default value
        /// </summary>
        None = 0x0,

        /// <summary>
        /// The value is displayed as a string.
        /// </summary>
        StringType = 0x1,

        /// <summary>
        /// The value is displayed as an integer.
        /// </summary>
        IntegerType = 0x2,

        /// <summary>
        /// The value is displayed as a date/time.
        /// </summary>
        DateType = 0x3,

        /// <summary>
        /// A mask for display type values StringType, IntegerType, and DateType.
        /// </summary>
        TypeMask = 0xF,

        /// <summary>
        /// The column should be on by default in Details view.
        /// </summary>
        OnByDefault = 0x10,

        /// <summary>
        /// Will be slow to compute. Perform on a background thread.
        /// </summary>
        Slow = 0x20,

        /// <summary>
        /// Provided by a handler, not the folder.
        /// </summary>
        Extended = 0x40,

        /// <summary>
        /// Not displayed in the context menu, but is listed in the More... dialog.
        /// </summary>
        SecondaryUI = 0x80,

        /// <summary>
        /// Not displayed in the user interface (I).
        /// </summary>
        Hidden = 0x100,

        /// <summary>
        /// VarCmp produces same result as IShellFolder::CompareIDs.
        /// </summary>
        PreferVariantCompare = 0x200,

        /// <summary>
        /// PSFormatForDisplay produces same result as IShellFolder::CompareIDs.
        /// </summary>
        PreferFormatForDisplay = 0x400,

        /// <summary>
        /// Do not sort folders separately.
        /// </summary>
        NoSortByFolders = 0x800,

        /// <summary>
        /// Only displayed in the UI.
        /// </summary>
        ViewOnly = 0x10000,

        /// <summary>
        /// Marks columns with values that should be read in a batch.
        /// </summary>
        BatchRead = 0x20000,

        /// <summary>
        /// Grouping is disabled for this column.
        /// </summary>
        NoGroupBy = 0x40000,

        /// <summary>
        /// Can't resize the column.
        /// </summary>
        FixedWidth = 0x1000,

        /// <summary>
        /// The width is the same in all dots per inch (dpi)s.
        /// </summary>
        NoDpiScale = 0x2000,

        /// <summary>
        /// Fixed width and height ratio.
        /// </summary>
        FixedRatio = 0x4000,

        /// <summary>
        /// Filters out new display flags.
        /// </summary>
        DisplayMask = 0xF000
    }

    /// <summary>
    /// Specifies the condition type to use when displaying the property in the query builder user interface (I).
    /// </summary>
    public enum PropertyConditionType
    {
        /// <summary>
        /// The default condition type.
        /// </summary>
        None = 0,

        /// <summary>
        /// The string type.
        /// </summary>
        String = 1,

        /// <summary>
        /// The size type.
        /// </summary>
        Size = 2,

        /// <summary>
        /// The date/time type.
        /// </summary>
        DateTime = 3,

        /// <summary>
        /// The Boolean type.
        /// </summary>
        Boolean = 4,

        /// <summary>
        /// The number type.
        /// </summary>
        Number = 5
    }

    /// <summary>
    /// Provides a set of flags to be used with IConditionFactory,
    /// ICondition, and IConditionGenerator to indicate the operation.
    /// </summary>
    public enum PropertyConditionOperation
    {
        /// <summary>
        /// The implicit comparison between the value of the property and the value of the constant.
        /// </summary>
        Implicit,

        /// <summary>
        /// The value of the property and the value of the constant must be equal.
        /// </summary>
        Equal,

        /// <summary>
        /// The value of the property and the value of the constant must not be equal.
        /// </summary>
        NotEqual,

        /// <summary>
        /// The value of the property must be less than the value of the constant.
        /// </summary>
        LessThan,

        /// <summary>
        /// The value of the property must be greater than the value of the constant.
        /// </summary>
        GreaterThan,

        /// <summary>
        /// The value of the property must be less than or equal to the value of the constant.
        /// </summary>
        LessThanOrEqual,

        /// <summary>
        /// The value of the property must be greater than or equal to the value of the constant.
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// The value of the property must begin with the value of the constant.
        /// </summary>
        ValueStartsWith,

        /// <summary>
        /// The value of the property must end with the value of the constant.
        /// </summary>
        ValueEndsWith,

        /// <summary>
        /// The value of the property must contain the value of the constant.
        /// </summary>
        ValueContains,

        /// <summary>
        /// The value of the property must not contain the value of the constant.
        /// </summary>
        ValueNotContains,

        /// <summary>
        /// The value of the property must match the value of the constant, where '?' matches any single character and '*' matches any sequence of characters.
        /// </summary>
        DOSWildCards,

        /// <summary>
        /// The value of the property must contain a word that is the value of the constant.
        /// </summary>
        WordEqual,

        /// <summary>
        /// The value of the property must contain a word that begins with the value of the constant.
        /// </summary>
        WordStartsWith,

        /// <summary>
        /// The application is free to interpret this in any suitable way.
        /// </summary>
        ApplicationSpecific
    }

    /// <summary>
    /// Specifies the property description grouping ranges.
    /// </summary>
    public enum PropertyGroupingRange
    {
        /// <summary>
        /// The individual values.
        /// </summary>
        Discrete = 0,

        /// <summary>
        /// The static alphanumeric ranges.
        /// </summary>
        Alphanumeric = 1,

        /// <summary>
        /// The static size ranges.
        /// </summary>
        Size = 2,

        /// <summary>
        /// The dynamically-created ranges.
        /// </summary>
        Dynamic = 3,

        /// <summary>
        /// The month and year groups.
        /// </summary>
        Date = 4,

        /// <summary>
        /// The percent groups.
        /// </summary>
        Percent = 5,

        /// <summary>
        /// The enumerated groups.
        /// </summary>
        Enumerated = 6
    }

    /// <summary>
    /// Describes the particular wordings of sort offerings.
    /// </summary>
    /// <remarks>
    /// Note that the strings shown are English versions only;
    /// localized strings are used for other locales.
    /// </remarks>
    public enum PropertySortDescription
    {
        /// <summary>
        /// The default ascending or descending property sort, "Sort going up", "Sort going down".
        /// </summary>
        General,

        /// <summary>
        /// The alphabetical sort, "A on top", "Z on top".
        /// </summary>
        AToZ,

        /// <summary>
        /// The numerical sort, "Lowest on top", "Highest on top".
        /// </summary>
        LowestToHighest,

        /// <summary>
        /// The size sort, "Smallest on top", "Largest on top".
        /// </summary>
        SmallestToBiggest,

        /// <summary>
        /// The chronological sort, "Oldest on top", "Newest on top".
        /// </summary>
        OldestToNewest
    }

    /// <summary>
    /// Describes the attributes of the <c>typeInfo</c> element in the property's <c>.propdesc</c> file.
    /// </summary>
    [Flags]
    public enum PropertyTypeOptions : uint
    {
        /// <summary>
        /// The property uses the default values for all attributes.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// The property can have multiple values.
        /// </summary>
        /// <remarks>
        /// These values are stored as a VT_VECTOR in the PROPVARIANT structure.
        /// This value is set by the multipleValues attribute of the typeInfo element in the property's .propdesc file.
        /// </remarks>
        MultipleValues = 0x1,

        /// <summary>
        /// This property cannot be written to.
        /// </summary>
        /// <remarks>
        /// This value is set by the isInnate attribute of the typeInfo element in the property's .propdesc file.
        /// </remarks>
        IsInnate = 0x2,

        /// <summary>
        /// The property is a group heading.
        /// </summary>
        /// <remarks>
        /// This value is set by the isGroup attribute of the typeInfo element in the property's .propdesc file.
        /// </remarks>
        IsGroup = 0x4,

        /// <summary>
        /// The user can group by this property.
        /// </summary>
        /// <remarks>
        /// This value is set by the canGroupBy attribute of the typeInfo element in the property's .propdesc file.
        /// </remarks>
        CanGroupBy = 0x8,

        /// <summary>
        /// The user can stack by this property.
        /// </summary>
        /// <remarks>
        /// This value is set by the canStackBy attribute of the typeInfo element in the property's .propdesc file.
        /// </remarks>
        CanStackBy = 0x10,

        /// <summary>
        /// This property contains a hierarchy.
        /// </summary>
        /// <remarks>
        /// This value is set by the isTreeProperty attribute of the typeInfo element in the property's .propdesc file.
        /// </remarks>
        IsTreeProperty = 0x20,

        /// <summary>
        /// Include this property in any full text query that is performed.
        /// </summary>
        /// <remarks>
        /// This value is set by the includeInFullTextQuery attribute of the typeInfo element in the property's .propdesc file.
        /// </remarks>
        IncludeInFullTextQuery = 0x40,

        /// <summary>
        /// This property is meant to be viewed by the user.
        /// </summary>
        /// <remarks>
        /// This influences whether the property shows up in the "Choose Columns" dialog, for example.
        /// This value is set by the isViewable attribute of the typeInfo element in the property's .propdesc file.
        /// </remarks>
        IsViewable = 0x80,

        /// <summary>
        /// This property is included in the list of properties that can be queried.
        /// </summary>
        /// <remarks>
        /// A queryable property must also be viewable.
        /// This influences whether the property shows up in the query builder UI.
        /// This value is set by the isQueryable attribute of the typeInfo element in the property's .propdesc file.
        /// </remarks>
        IsQueryable = 0x100,

        /// <summary>
        /// Used with an innate property (that is, a value calculated from other property values) to indicate that it can be deleted.
        /// </summary>
        /// <remarks>
        /// Windows Vista with Service Pack 1 (SP1) and later.
        /// This value is used by the Remove Properties user interface (I) to determine whether to display a check box next to an property that allows that property to be selected for removal.
        /// Note that a property that is not innate can always be purged regardless of the presence or absence of this flag.
        /// </remarks>
        CanBePurged = 0x200,

        /// <summary>
        /// This property is owned by the system.
        /// </summary>
        IsSystemProperty = 0x80000000,

        /// <summary>
        /// A mask used to retrieve all flags.
        /// </summary>
        MaskAll = 0x800001FF
    }

    /// <summary>
    /// Associates property names with property description list strings.
    /// </summary>
    [Flags]
    public enum PropertyViewOptions
    {
        /// <summary>
        /// The property is shown by default.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// The property is centered.
        /// </summary>
        CenterAlign = 0x1,

        /// <summary>
        /// The property is right aligned.
        /// </summary>
        RightAlign = 0x2,

        /// <summary>
        /// The property is shown as the beginning of the next collection of properties in the view.
        /// </summary>
        BeginNewGroup = 0x4,

        /// <summary>
        /// The remainder of the view area is filled with the content of this property.
        /// </summary>
        FillArea = 0x8,

        /// <summary>
        /// The property is reverse sorted if it is a property in a list of sorted properties.
        /// </summary>
        SortDescending = 0x10,

        /// <summary>
        /// The property is only shown if it is present.
        /// </summary>
        ShowOnlyIfPresent = 0x20,

        /// <summary>
        /// The property is shown by default in a view (where applicable).
        /// </summary>
        ShowByDefault = 0x40,

        /// <summary>
        /// The property is shown by default in primary column selection user interface (I).
        /// </summary>
        ShowInPrimaryList = 0x80,

        /// <summary>
        /// The property is shown by default in secondary column selection UI.
        /// </summary>
        ShowInSecondaryList = 0x100,

        /// <summary>
        /// The label is hidden if the view is normally inclined to show the label.
        /// </summary>
        HideLabel = 0x200,

        /// <summary>
        /// The property is not displayed as a column in the UI.
        /// </summary>
        Hidden = 0x800,

        /// <summary>
        /// The property is wrapped to the next row.
        /// </summary>
        CanWrap = 0x1000,

        /// <summary>
        /// A mask used to retrieve all flags.
        /// </summary>
        MaskAll = 0x3FF
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    // ' Borrowed from the WindowsAPICodePack (v1.1), Translated.


    /// <summary>
    /// Represents the OLE struct PROPVARIANT.
    /// This class is intended for internal use only.
    /// </summary>
    /// <remarks>
    /// Originally sourced from http://blogs.msdn.com/adamroot/pages/interop-with-propvariants-in-net.aspx
    /// and modified to support additional types including vectors and ability to set values
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable", MessageId = "_ptr2")]
    [StructLayout(LayoutKind.Explicit)]
    public sealed class PropVariant : IDisposable
    {
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // A static dictionary of delegates to get data from array's contained within PropVariants
        private static Dictionary<Type, Action<PropVariant, Array, uint>> _vectorActions = null;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private static Dictionary<Type, Action<PropVariant, Array, uint>> GenerateVectorActions()
        {
            var cache = new Dictionary<Type, Action<PropVariant, Array, uint>>();
            cache.Add(typeof(short), (pv, array, i) =>
            {
                short val;
                PropVariantNativeMethods.PropVariantGetInt16Elem(pv, i, out val);
                array.SetValue(val, i);
            });
            cache.Add(typeof(ushort), (pv, array, i) =>
            {
                ushort val;
                PropVariantNativeMethods.PropVariantGetUInt16Elem(pv, i, out val);
                array.SetValue(val, i);
            });
            cache.Add(typeof(int), (pv, array, i) =>
            {
                int val;
                PropVariantNativeMethods.PropVariantGetInt32Elem(pv, i, out val);
                array.SetValue(val, i);
            });
            cache.Add(typeof(uint), (pv, array, i) =>
            {
                uint val;
                PropVariantNativeMethods.PropVariantGetUInt32Elem(pv, i, out val);
                array.SetValue(val, i);
            });
            cache.Add(typeof(long), (pv, array, i) =>
            {
                long val;
                PropVariantNativeMethods.PropVariantGetInt64Elem(pv, i, out val);
                array.SetValue(val, i);
            });
            cache.Add(typeof(ulong), (pv, array, i) =>
            {
                ulong val;
                PropVariantNativeMethods.PropVariantGetUInt64Elem(pv, i, out val);
                array.SetValue(val, i);
            });
            cache.Add(typeof(DateTime), (pv, array, i) =>
            {
                FILETIME val;
                PropVariantNativeMethods.PropVariantGetFileTimeElem(pv, i, out val);
                long fileTime = GetFileTimeAsLong(ref val);
                array.SetValue(DateTime.FromFileTime(fileTime), i);
            });
            cache.Add(typeof(bool), (pv, array, i) =>
            {
                bool val;
                PropVariantNativeMethods.PropVariantGetBooleanElem(pv, i, out val);
                array.SetValue(val, i);
            });
            cache.Add(typeof(double), (pv, array, i) =>
            {
                double val;
                PropVariantNativeMethods.PropVariantGetDoubleElem(pv, i, out val);
                array.SetValue(val, i);
            });
            cache.Add(typeof(float), (pv, array, i) =>
            {
                // float
                var val = new float[1];
                Marshal.Copy(pv._ptr2, val, (int)i, 1);
                array.SetValue(val[0], (int)i);
            });
            cache.Add(typeof(decimal), (pv, array, i) =>
            {
                var val = new int[4];
                for (int a = 0, loopTo = val.Length - 1; a <= loopTo; a++)
                    // index * size + offset quarter
                    val[a] = Marshal.ReadInt32(pv._ptr2, (int)i * Strings.Len(0) + a * 4);
                array.SetValue(new decimal(val), i);
            });
            cache.Add(typeof(string), (pv, array, i) =>
            {
                string val = string.Empty;
                PropVariantNativeMethods.PropVariantGetStringElem(pv, i, ref val);
                array.SetValue(val, i);
            });
            return cache;
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Attempts to create a PropVariant by finding an appropriate constructor.
        /// </summary>
        /// <param name="value">Object from which PropVariant should be created.</param>
        public static PropVariant FromObject(object value)
        {
            if (value is null)
            {
                return new PropVariant();
            }
            else
            {
                var func = GetDynamicConstructor(value.GetType());
                return func(value);
            }
        }

        // A dictionary and lock to contain compiled expression trees for constructors
        private static Dictionary<Type, Func<object, PropVariant>> _cache = new Dictionary<Type, Func<object, PropVariant>>();
        private static object _padlock = new object();

        // Retrieves a cached constructor expression.
        // If no constructor has been cached, it attempts to find/add it.  If it cannot be found
        // an exception is thrown.
        // This method looks for a public constructor with the same parameter type as the object.
        private static Func<object, PropVariant> GetDynamicConstructor(Type type)
        {
            lock (_padlock)
            {
                // initial check, if action is found, return it
                Func<object, PropVariant> action = null;
                if (!_cache.TryGetValue(type, out action))
                {
                    // iterates through all constructors
                    var constructor = typeof(PropVariant).GetConstructor(new Type[] { type });
                    if (constructor is null)
                    {
                        // if the method was not found, throw.
                        throw new ArgumentException(LocalizedMessages.PropVariantTypeNotSupported);
                    }
                    else
                    {
                        // if the method was found, create an expression to call it.
                        // create parameters to action                    
                        var arg = Expression.Parameter(typeof(object), "arg");

                        // create an expression to invoke the constructor with an argument cast to the correct type
                        var create = Expression.New(constructor, Expression.Convert(arg, type));

                        // compiles expression into an action delegate
                        action = Expression.Lambda<Func<object, PropVariant>>(create, arg).Compile();
                        _cache.Add(type, action);
                    }
                }

                return action;
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [FieldOffset(0)]
        private decimal _decimal;

        // This is actually a VarEnum value, but the VarEnum type
        // requires 4 bytes instead of the expected 2.
        [FieldOffset(0)]
        private ushort _valueType;

        // Reserved Fields
        // [FieldOffset(2)]
        // ushort _wReserved1;
        // [FieldOffset(4)]
        // ushort _wReserved2;
        // [FieldOffset(6)]
        // ushort _wReserved3;

        // In order to allow x64 compat, we need to allow for
        // expansion of the IntPtr. However, the BLOB struct
        // uses a 4-byte int, followed by an IntPtr, so
        // although the valueData field catches most pointer values,
        // we need an additional 4-bytes to get the BLOB
        // pointer. The valueDataExt field provides this, as well as
        // the last 4-bytes of an 8-byte value on 32-bit
        // architectures.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
        [FieldOffset(12)]
        private IntPtr _ptr2;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
        [FieldOffset(8)]
        private IntPtr _ptr;
        [FieldOffset(8)]
        private int _int32;
        [FieldOffset(8)]
        private uint _uint32;
        [FieldOffset(8)]
        private byte _byte;
        [FieldOffset(8)]
        private sbyte _sbyte;
        [FieldOffset(8)]
        private short _short;
        [FieldOffset(8)]
        private ushort _ushort;
        [FieldOffset(8)]
        private long _long;
        [FieldOffset(8)]
        private ulong _ulong;
        [FieldOffset(8)]
        private double _double;
        [FieldOffset(8)]
        private float _float;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Default constrcutor
        /// </summary>
        public PropVariant()
        {
        }

        /// <summary>
        /// Set a string value
        /// </summary>
        public PropVariant(string value)
        {
            if (value is null)
            {
                throw new ArgumentException(LocalizedMessages.PropVariantNullString, "value");
            }

            _valueType = (ushort)VarEnum.VT_LPWSTR;
            _ptr = Marshal.StringToCoTaskMemUni(value);
        }

        /// <summary>
        /// Set a string vector
        /// </summary>
        public PropVariant(string[] value)
        {
            if (value is null)
            {
                throw new ArgumentNullException("value");
            }

            PropVariantNativeMethods.InitPropVariantFromStringVector(value, (uint)value.Length, this);
        }

        /// <summary>
        /// Set a bool vector
        /// </summary>
        public PropVariant(bool[] value)
        {
            if (value is null)
            {
                throw new ArgumentNullException("value");
            }

            PropVariantNativeMethods.InitPropVariantFromBooleanVector(value, (uint)value.Length, this);
        }

        /// <summary>
        /// Set a short vector
        /// </summary>
        public PropVariant(short[] value)
        {
            if (value is null)
            {
                throw new ArgumentNullException("value");
            }

            PropVariantNativeMethods.InitPropVariantFromInt16Vector(value, (uint)value.Length, this);
        }

        /// <summary>
        /// Set a short vector
        /// </summary>
        public PropVariant(ushort[] value)
        {
            if (value is null)
            {
                throw new ArgumentNullException("value");
            }

            PropVariantNativeMethods.InitPropVariantFromUInt16Vector(value, (uint)value.Length, this);
        }

        /// <summary>
        /// Set an int vector
        /// </summary>
        public PropVariant(int[] value)
        {
            if (value is null)
            {
                throw new ArgumentNullException("value");
            }

            PropVariantNativeMethods.InitPropVariantFromInt32Vector(value, (uint)value.Length, this);
        }

        /// <summary>
        /// Set an uint vector
        /// </summary>
        public PropVariant(uint[] value)
        {
            if (value is null)
            {
                throw new ArgumentNullException("value");
            }

            PropVariantNativeMethods.InitPropVariantFromUInt32Vector(value, (uint)value.Length, this);
        }

        /// <summary>
        /// Set a long vector
        /// </summary>
        public PropVariant(long[] value)
        {
            if (value is null)
            {
                throw new ArgumentNullException("value");
            }

            PropVariantNativeMethods.InitPropVariantFromInt64Vector(value, (uint)value.Length, this);
        }

        /// <summary>
        /// Set a ulong vector
        /// </summary>
        public PropVariant(ulong[] value)
        {
            if (value is null)
            {
                throw new ArgumentNullException("value");
            }

            PropVariantNativeMethods.InitPropVariantFromUInt64Vector(value, (uint)value.Length, this);
        }

        /// <summary>>
        /// Set a double vector
        /// </summary>
        public PropVariant(double[] value)
        {
            if (value is null)
            {
                throw new ArgumentNullException("value");
            }

            PropVariantNativeMethods.InitPropVariantFromDoubleVector(value, (uint)value.Length, this);
        }


        /// <summary>
        /// Set a DateTime vector
        /// </summary>
        public PropVariant(DateTime[] value)
        {
            if (value is null)
            {
                throw new ArgumentNullException("value");
            }

            var fileTimeArr = new FILETIME[value.Length];
            for (int i = 0, loopTo = value.Length - 1; i <= loopTo; i++)
                fileTimeArr[i] = DateTimeToFileTime(value[i]);
            PropVariantNativeMethods.InitPropVariantFromFileTimeVector(fileTimeArr, (uint)fileTimeArr.Length, this);
        }

        /// <summary>
        /// Set a bool value
        /// </summary>
        public PropVariant(bool value)
        {
            _valueType = (ushort)VarEnum.VT_BOOL;
            _int32 = value == true ? -1 : 0;
        }

        /// <summary>
        /// Set a DateTime value
        /// </summary>
        public PropVariant(DateTime value)
        {
            _valueType = (ushort)VarEnum.VT_FILETIME;
            var ft = DateTimeToFileTime(value);
            PropVariantNativeMethods.InitPropVariantFromFileTime(ref ft, this);
        }


        /// <summary>
        /// Set a byte value
        /// </summary>
        public PropVariant(byte value)
        {
            _valueType = (ushort)VarEnum.VT_UI1;
            _byte = value;
        }

        /// <summary>
        /// Set a sbyte value
        /// </summary>
        public PropVariant(sbyte value)
        {
            _valueType = (ushort)VarEnum.VT_I1;
            _sbyte = value;
        }

        /// <summary>
        /// Set a short value
        /// </summary>
        public PropVariant(short value)
        {
            _valueType = (ushort)VarEnum.VT_I2;
            _short = value;
        }

        /// <summary>
        /// Set an unsigned short value
        /// </summary>
        public PropVariant(ushort value)
        {
            _valueType = (ushort)VarEnum.VT_UI2;
            _ushort = value;
        }

        /// <summary>
        /// Set an int value
        /// </summary>
        public PropVariant(int value)
        {
            _valueType = (ushort)VarEnum.VT_I4;
            _int32 = value;
        }

        /// <summary>
        /// Set an unsigned int value
        /// </summary>
        public PropVariant(uint value)
        {
            _valueType = (ushort)VarEnum.VT_UI4;
            _uint32 = value;
        }

        /// <summary>
        /// Set a decimal  value
        /// </summary>
        public PropVariant(decimal value)
        {
            _decimal = value;

            // It is critical that the value type be set after the decimal value, because they overlap.
            // If valuetype is written first, its value will be lost when _decimal is written.
            _valueType = (ushort)VarEnum.VT_DECIMAL;
        }

        /// <summary>
        /// Create a PropVariant with a contained decimal array.
        /// </summary>
        /// <param name="value">Decimal array to wrap.</param>
        public PropVariant(decimal[] value)
        {
            if (value is null)
            {
                throw new ArgumentNullException("value");
            }

            _valueType = (ushort)(VarEnum.VT_DECIMAL | VarEnum.VT_VECTOR);
            _int32 = value.Length;

            // allocate required memory for array with 128bit elements
            _ptr2 = Marshal.AllocCoTaskMem(value.Length * Strings.Len(0));
            for (int i = 0, loopTo = value.Length - 1; i <= loopTo; i++)
            {
                var bits = decimal.GetBits(value[i]);
                Marshal.Copy(bits, 0, _ptr2, bits.Length);
            }
        }

        /// <summary>
        /// Create a PropVariant containing a float type.
        /// </summary>
        public PropVariant(float value)
        {
            _valueType = (ushort)VarEnum.VT_R4;
            _float = value;
        }

        /// <summary>
        /// Creates a PropVariant containing a float[] array.
        /// </summary>
        public PropVariant(float[] value)
        {
            if (value is null)
            {
                throw new ArgumentNullException("value");
            }

            _valueType = (ushort)(VarEnum.VT_R4 | VarEnum.VT_VECTOR);
            _int32 = value.Length;
            _ptr2 = Marshal.AllocCoTaskMem(value.Length * 4);
            Marshal.Copy(value, 0, _ptr2, value.Length);
        }

        /// <summary>
        /// Set a long
        /// </summary>
        public PropVariant(long value)
        {
            _long = value;
            _valueType = (ushort)VarEnum.VT_I8;
        }

        /// <summary>
        /// Set a ulong
        /// </summary>
        public PropVariant(ulong value)
        {
            _valueType = (ushort)VarEnum.VT_UI8;
            _ulong = value;
        }

        /// <summary>
        /// Set a double
        /// </summary>
        public PropVariant(double value)
        {
            _valueType = (ushort)VarEnum.VT_R8;
            _double = value;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Set an IUnknown value
        /// </summary>
        /// <param name="value">The new value to set.</param>
        internal void SetIUnknown(object value)
        {
            _valueType = (ushort)VarEnum.VT_UNKNOWN;
            _ptr = Marshal.GetIUnknownForObject(value);
        }


        /// <summary>
        /// Set a safe array value
        /// </summary>
        /// <param name="array">The new value to set.</param>
        internal void SetSafeArray(Array array)
        {
            if (array is null)
            {
                throw new ArgumentNullException("array");
            }

            const ushort vtUnknown = 13;
            var psa = PropVariantNativeMethods.SafeArrayCreateVector(vtUnknown, 0, (uint)array.Length);
            var pvData = PropVariantNativeMethods.SafeArrayAccessData(psa);
            try
            {
                // to remember to release lock on data
                for (int i = 0, loopTo = array.Length - 1; i <= loopTo; i++)
                {
                    var obj = array.GetValue(i);
                    var punk = obj is object ? Marshal.GetIUnknownForObject(obj) : IntPtr.Zero;
                    Marshal.WriteIntPtr(pvData, i * IntPtr.Size, punk);
                }
            }
            finally
            {
                PropVariantNativeMethods.SafeArrayUnaccessData(psa);
            }

            _valueType = (ushort)VarEnum.VT_ARRAY | (ushort)VarEnum.VT_UNKNOWN;
            _ptr = psa;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Gets or sets the variant type.
        /// </summary>
        public VarEnum VarType
        {
            get
            {
                return (VarEnum)Conversions.ToInteger(_valueType);
            }

            set
            {
                _valueType = (ushort)value;
            }
        }

        /// <summary>
        /// Checks if this has an empty or null value
        /// </summary>
        /// <returns></returns>
        public bool IsNullOrEmpty
        {
            get
            {
                return _valueType == (ushort)VarEnum.VT_EMPTY || _valueType == (ushort)VarEnum.VT_NULL;
            }
        }

        /// <summary>
        /// Gets the variant value.
        /// </summary>
        public object Value
        {
            get
            {
                switch ((VarEnum)Conversions.ToInteger(_valueType))
                {
                    case VarEnum.VT_I1:
                        {
                            return _sbyte;
                        }

                    case VarEnum.VT_UI1:
                        {
                            return _byte;
                        }

                    case VarEnum.VT_I2:
                        {
                            return _short;
                        }

                    case VarEnum.VT_UI2:
                        {
                            return _ushort;
                        }

                    case VarEnum.VT_I4:
                    case VarEnum.VT_INT:
                        {
                            return _int32;
                        }

                    case VarEnum.VT_UI4:
                    case VarEnum.VT_UINT:
                        {
                            return _uint32;
                        }

                    case VarEnum.VT_I8:
                        {
                            return _long;
                        }

                    case VarEnum.VT_UI8:
                        {
                            return _ulong;
                        }

                    case VarEnum.VT_R4:
                        {
                            return _float;
                        }

                    case VarEnum.VT_R8:
                        {
                            return _double;
                        }

                    case VarEnum.VT_BOOL:
                        {
                            return _int32 == -1;
                        }

                    case VarEnum.VT_ERROR:
                        {
                            return _long;
                        }

                    case VarEnum.VT_CY:
                        {
                            return _decimal;
                        }

                    case VarEnum.VT_DATE:
                        {
                            return DateTime.FromOADate(_double);
                        }

                    case VarEnum.VT_FILETIME:
                        {
                            return DateTime.FromFileTime(_long);
                        }

                    case VarEnum.VT_BSTR:
                        {
                            return Marshal.PtrToStringBSTR(_ptr);
                        }

                    case VarEnum.VT_BLOB:
                        {
                            return GetBlobData();
                        }

                    case VarEnum.VT_LPSTR:
                        {
                            return Marshal.PtrToStringAnsi(_ptr);
                        }

                    case VarEnum.VT_LPWSTR:
                        {
                            return Marshal.PtrToStringUni(_ptr);
                        }

                    case VarEnum.VT_UNKNOWN:
                        {
                            return Marshal.GetObjectForIUnknown(_ptr);
                        }

                    case VarEnum.VT_DISPATCH:
                        {
                            return Marshal.GetObjectForIUnknown(_ptr);
                        }

                    case VarEnum.VT_DECIMAL:
                        {
                            return _decimal;
                        }

                    case VarEnum.VT_ARRAY | VarEnum.VT_UNKNOWN:
                        {
                            return CrackSingleDimSafeArray(_ptr);
                        }

                    case VarEnum.VT_VECTOR | VarEnum.VT_LPWSTR:
                        {
                            return GetVector<string>();
                        }

                    case VarEnum.VT_VECTOR | VarEnum.VT_I2:
                        {
                            return GetVector<short>();
                        }

                    case VarEnum.VT_VECTOR | VarEnum.VT_UI2:
                        {
                            return GetVector<ushort>();
                        }

                    case VarEnum.VT_VECTOR | VarEnum.VT_I4:
                        {
                            return GetVector<int>();
                        }

                    case VarEnum.VT_VECTOR | VarEnum.VT_UI4:
                        {
                            return GetVector<uint>();
                        }

                    case VarEnum.VT_VECTOR | VarEnum.VT_I8:
                        {
                            return GetVector<long>();
                        }

                    case VarEnum.VT_VECTOR | VarEnum.VT_UI8:
                        {
                            return GetVector<ulong>();
                        }

                    case VarEnum.VT_VECTOR | VarEnum.VT_R4:
                        {
                            return GetVector<float>();
                        }

                    case VarEnum.VT_VECTOR | VarEnum.VT_R8:
                        {
                            return GetVector<double>();
                        }

                    case VarEnum.VT_VECTOR | VarEnum.VT_BOOL:
                        {
                            return GetVector<bool>();
                        }

                    case VarEnum.VT_VECTOR | VarEnum.VT_FILETIME:
                        {
                            return GetVector<DateTime>();
                        }

                    case VarEnum.VT_VECTOR | VarEnum.VT_DECIMAL:
                        {
                            return GetVector<decimal>();
                        }

                    default:
                        {
                            // if the value cannot be marshaled
                            return null;
                        }
                }
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private static long GetFileTimeAsLong(ref FILETIME val)
        {
            return ((long)val.dwHighDateTime << 32) + val.dwLowDateTime;
        }

        private static FILETIME DateTimeToFileTime(DateTime value)
        {
            long hFT = value.ToFileTime();
            var ft = new FILETIME();
            ft.dwLowDateTime = (int)(hFT & 0xFFFFFFFFU);
            ft.dwHighDateTime = (int)(hFT >> 32);
            return ft;
        }

        private object GetBlobData()
        {
            var blobData = new byte[_int32];
            var pBlobData = _ptr2;
            Marshal.Copy(pBlobData, blobData, 0, _int32);
            return blobData;
        }

        private Array GetVector<T>()
        {
            int count = PropVariantNativeMethods.PropVariantGetElementCount(this);
            if (count <= 0)
            {
                return null;
            }

            lock (_padlock)
            {
                if (_vectorActions is null)
                {
                    _vectorActions = GenerateVectorActions();
                }
            }

            Action<PropVariant, Array, uint> action = null;
            if (!_vectorActions.TryGetValue(typeof(T), out action))
            {
                throw new InvalidCastException(LocalizedMessages.PropVariantUnsupportedType);
            }

            Array array = new T[count];
            for (int i = 0, loopTo = count - 1; i <= loopTo; i++)
                action(this, array, (uint)i);
            return array;
        }

        private static Array CrackSingleDimSafeArray(IntPtr psa)
        {
            uint cDims = PropVariantNativeMethods.SafeArrayGetDim(psa);
            if (cDims != 1L)
            {
                throw new ArgumentException(LocalizedMessages.PropVariantMultiDimArray, "psa");
            }

            int lBound = PropVariantNativeMethods.SafeArrayGetLBound(psa, 1U);
            int uBound = PropVariantNativeMethods.SafeArrayGetUBound(psa, 1U);
            int n = uBound - lBound + 1;
            // uBound is inclusive
            var array = new object[n];
            for (int i = lBound, loopTo = uBound; i <= loopTo; i++)
                array[i] = PropVariantNativeMethods.SafeArrayGetElement(psa, ref i);
            return array;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Disposes the object, calls the clear function.
        /// </summary>
        public void Dispose()
        {
            PropVariantNativeMethods.PropVariantClear(this);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~PropVariant()
        {
            try
            {
                Dispose();
            }
            finally
            {
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /// <summary>
        /// Provides an simple string representation of the contained data and type.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}: {1}", Value, VarType.ToString());
        }
    }

    internal sealed class PropVariantNativeMethods
    {
        private PropVariantNativeMethods()
        {
        }
        // returns hresult
        [DllImport("Ole32.dll", PreserveSig = false)]
        internal static extern void PropVariantClear(PropVariant pvar);

        // psa is actually returned, not hresult
        [DllImport("OleAut32.dll", PreserveSig = true)]
        internal static extern IntPtr SafeArrayCreateVector(ushort vt, int lowerBound, uint cElems);

        // returns hresult
        [DllImport("OleAut32.dll", PreserveSig = false)]
        internal static extern IntPtr SafeArrayAccessData(IntPtr psa);

        // returns hresult
        [DllImport("OleAut32.dll", PreserveSig = false)]
        internal static extern void SafeArrayUnaccessData(IntPtr psa);

        // retuns uint32
        [DllImport("OleAut32.dll", PreserveSig = true)]
        internal static extern uint SafeArrayGetDim(IntPtr psa);

        // returns hresult
        [DllImport("OleAut32.dll", PreserveSig = false)]
        internal static extern int SafeArrayGetLBound(IntPtr psa, uint nDim);

        // returns hresult
        [DllImport("OleAut32.dll", PreserveSig = false)]
        internal static extern int SafeArrayGetUBound(IntPtr psa, uint nDim);

        // This decl for SafeArrayGetElement is only valid for cDims==1!
        // returns hresult
        [DllImport("OleAut32.dll", PreserveSig = false)]
        internal static extern object SafeArrayGetElement(IntPtr psa, ref int rgIndices);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromPropVariantVectorElem([In] PropVariant propvarIn, uint iElem, PropVariant ppropvar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromFileTime([In] ref FILETIME pftIn, PropVariant ppropvar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int PropVariantGetElementCount([In] PropVariant propVar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetBooleanElem([In] PropVariant propVar, [In] uint iElem, [MarshalAs(UnmanagedType.Bool)] out bool pfVal);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetInt16Elem([In] PropVariant propVar, [In] uint iElem, out short pnVal);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetUInt16Elem([In] PropVariant propVar, [In] uint iElem, out ushort pnVal);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetInt32Elem([In] PropVariant propVar, [In] uint iElem, out int pnVal);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetUInt32Elem([In] PropVariant propVar, [In] uint iElem, out uint pnVal);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetInt64Elem([In] PropVariant propVar, [In] uint iElem, out long pnVal);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetUInt64Elem([In] PropVariant propVar, [In] uint iElem, out ulong pnVal);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetDoubleElem([In] PropVariant propVar, [In] uint iElem, out double pnVal);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetFileTimeElem([In] PropVariant propVar, [In] uint iElem, [MarshalAs(UnmanagedType.Struct)] out FILETIME pftVal);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void PropVariantGetStringElem([In] PropVariant propVar, [In] uint iElem, [MarshalAs(UnmanagedType.LPWStr)] ref string ppszVal);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromBooleanVector([In][MarshalAs(UnmanagedType.LPArray)] bool[] prgf, uint cElems, PropVariant ppropvar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromInt16Vector([In] short[] prgn, uint cElems, PropVariant ppropvar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromUInt16Vector([In] ushort[] prgn, uint cElems, PropVariant ppropvar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromInt32Vector([In] int[] prgn, uint cElems, PropVariant propVar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromUInt32Vector([In] uint[] prgn, uint cElems, PropVariant ppropvar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromInt64Vector([In] long[] prgn, uint cElems, PropVariant ppropvar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromUInt64Vector([In] ulong[] prgn, uint cElems, PropVariant ppropvar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromDoubleVector([In] double[] prgn, uint cElems, PropVariant propvar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromFileTimeVector([In] FILETIME[] prgft, uint cElems, PropVariant ppropvar);
        [DllImport("propsys.dll", CharSet = CharSet.Unicode, SetLastError = true, PreserveSig = false)]
        internal static extern void InitPropVariantFromStringVector([In] string[] prgsz, uint cElems, PropVariant ppropvar);
    }


    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    [ComImport]
    [Guid(ShellIIDGuid.IPropertyStoreCapabilities)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IPropertyStoreCapabilities
    {
        HResult IsPropertyWritable([In] ref PropertyKey propertyKey);
    }

    /// <summary>
    /// An in-memory property store cache
    /// </summary>
    [ComImport]
    [Guid(ShellIIDGuid.IPropertyStoreCache)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IPropertyStoreCache
    {
        /// <summary>
        /// Gets the state of a property stored in the cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetState(ref PropertyKey key, out PropertyStoreCacheState state);

        /// <summary>
        /// Gets the valeu and state of a property in the cache
        /// </summary>
        /// <param name="propKey"></param>
        /// <param name="pv"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetValueAndState(ref PropertyKey propKey, out PropVariant pv, out PropertyStoreCacheState state);

        /// <summary>
        /// Sets the state of a property in the cache.
        /// </summary>
        /// <param name="propKey"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult SetState(ref PropertyKey propKey, PropertyStoreCacheState state);

        /// <summary>
        /// Sets the value and state in the cache.
        /// </summary>
        /// <param name="propKey"></param>
        /// <param name="pv"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult SetValueAndState(ref PropertyKey propKey, [In] PropVariant pv, PropertyStoreCacheState state);
    }

    /// <summary>
    /// A property store
    /// </summary>
    [ComImport]
    [Guid(ShellIIDGuid.IPropertyStore)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IPropertyStore
    {
        /// <summary>
        /// Gets the number of properties contained in the property store.
        /// </summary>
        /// <param name="propertyCount"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetCount(out uint propertyCount);

        /// <summary>
        /// Get a property key located at a specific index.
        /// </summary>
        /// <param name="propertyIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetAt([In] uint propertyIndex, ref PropertyKey key);

        /// <summary>
        /// Gets the value of a property from the store
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pv"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetValue([In] ref PropertyKey key, out PropVariant pv);

        /// <summary>
        /// Sets the value of a property in the store
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pv"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        extern HResult SetValue([In] ref PropertyKey key, [In] PropVariant pv);

        /// <summary>
        /// Commits the changes.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult Commit();
    }

    [ComImport]
    [Guid(ShellIIDGuid.IPropertyDescriptionList)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPropertyDescriptionList
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetCount(ref uint pcElem);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetAt([In] uint iElem, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] ref IPropertyDescription ppv);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IPropertyDescription)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPropertyDescription
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetPropertyKey(ref PropertyKey pkey);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetCanonicalName([MarshalAs(UnmanagedType.LPWStr)] ref string ppszName);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetPropertyType(ref VarEnum pvartype);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        extern HResult GetDisplayName(ref IntPtr ppszName);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetEditInvitation(ref IntPtr ppszInvite);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetTypeFlags([In] PropertyTypeOptions mask, ref PropertyTypeOptions ppdtFlags);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetViewFlags(ref PropertyViewOptions ppdvFlags);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetDefaultColumnWidth(ref uint pcxChars);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetDisplayType(ref PropertyDisplayType pdisplaytype);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetColumnState(ref PropertyColumnStateOptions pcsFlags);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetGroupingRange(ref PropertyGroupingRange pgr);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetRelativeDescriptionType(ref PropertySystemNativeMethods.RelativeDescriptionType prdt);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetRelativeDescription([In] PropVariant propvar1, [In] PropVariant propvar2, [MarshalAs(UnmanagedType.LPWStr)] ref string ppszDesc1, [MarshalAs(UnmanagedType.LPWStr)] ref string ppszDesc2);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetSortDescription(ref PropertySortDescription psd);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetSortDescriptionLabel([In] bool fDescending, ref IntPtr ppszDescription);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetAggregationType(ref PropertyAggregationType paggtype);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetConditionType(ref PropertyConditionType pcontype, ref PropertyConditionOperation popDefault);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetEnumTypeList([In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IPropertyEnumTypeList ppv);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void CoerceToCanonicalValue([In] PropVariant propvar);
        // Note: this method signature may be wrong, but it is not used.
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult FormatForDisplay([In] PropVariant propvar, [In] ref PropertyDescriptionFormatOptions pdfFlags, [MarshalAs(UnmanagedType.LPWStr)] ref string ppszDisplay);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult IsValueCanonical([In] PropVariant propvar);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IPropertyDescription2)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPropertyDescription2 : IPropertyDescription
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetImageReferenceForValue([In] PropVariant propvar, [MarshalAs(UnmanagedType.LPWStr)] out string ppszImageRes);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IPropertyEnumType)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPropertyEnumType
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetEnumType(out PropEnumType penumtype);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetValue(out PropVariant ppropvar);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetRangeMinValue(out PropVariant ppropvar);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetRangeSetValue(out PropVariant ppropvar);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetDisplayText([MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplay);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IPropertyEnumType2)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPropertyEnumType2 : IPropertyEnumType
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetImageReference([MarshalAs(UnmanagedType.LPWStr)] out string ppszImageRes);
    }

    [ComImport]
    [Guid(ShellIIDGuid.IPropertyEnumTypeList)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPropertyEnumTypeList
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetCount(out uint pctypes);

        // riid may be IID_IPropertyEnumType
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetAt([In] uint itype, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IPropertyEnumType ppv);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void GetConditionAt([In] uint index, [In] ref Guid riid, ref IntPtr ppv);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern void FindMatchingIndex([In] PropVariant propvarCmp, out uint pnIndex);
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    public enum HResult : uint
    {
        /// <summary>
        /// S_OK
        /// </summary>
        Ok = 0x0,

        /// <summary>
        /// S_FALSE
        /// </summary>
        False = 0x1,

        /// <summary>
        /// E_INVALIDARG
        /// </summary>
        InvalidArguments = 0x80070057,

        /// <summary>
        /// E_OUTOFMEMORY
        /// </summary>
        OutOfMemory = 0x8007000E,

        /// <summary>
        /// E_NOINTERFACE
        /// </summary>
        NoInterface = 0x80004002,

        /// <summary>
        /// E_FAIL
        /// </summary>
        Fail = 0x80004005,

        /// <summary>
        /// E_ELEMENTNOTFOUND
        /// </summary>
        ElementNotFound = 0x80070490,

        /// <summary>
        /// TYPE_E_ELEMENTNOTFOUND
        /// </summary>
        TypeElementNotFound = 0x8002802B,

        /// <summary>
        /// NO_OBJECT
        /// </summary>
        NoObject = 0x800401E5,

        /// <summary>
        /// Win32 Error code: ERROR_CANCELLED
        /// </summary>
        Win32ErrorCanceled = 1223,

        /// <summary>
        /// ERROR_CANCELLED
        /// </summary>
        Canceled = 0x800704C7,

        /// <summary>
        /// The requested resource is in use
        /// </summary>
        ResourceInUse = 0x800700AA,

        /// <summary>
        /// The requested resource is read-only.
        /// </summary>
        AccessDenied = 0x80030005
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */

    // ' This code was written, from the ground, up, by me (Nathan Moschkin)
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    [ComImport]
    [Guid("92218CAB-ECAA-4335-8133-807FD234C2EE")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComConversionLoss]
    public interface IAssocHandlerInvoker
    {
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult SupportsSelection();
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult Invoke();
    }

    [ComImport]
    [Guid("F04061AC-1659-4a3f-A954-775AA57FC083")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComConversionLoss]
    public interface IAssocHandler
    {
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetName([MarshalAs(UnmanagedType.LPWStr)] out string ppsz);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetUIName([MarshalAs(UnmanagedType.LPWStr)] out string ppsz);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult GetIconLocation([MarshalAs(UnmanagedType.LPWStr)] out string ppszPath, out int pIndex);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult IsRecommended();
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult MakeDefault([MarshalAs(UnmanagedType.LPWStr)][In] string pszDescription);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult Invoke([In] IDataObject pdo);
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult CreateInvoker([In] IDataObject pdo, out IAssocHandlerInvoker ppInvoker);
    }

    [ComImport]
    [Guid("973810ae-9599-4b88-9e4d-6ee98c9552da")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumAssocHandlers
    {
        [PreserveSig]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        extern HResult Next([In] uint celt, out IAssocHandler rgelt, out uint pceltFetched);
    }


    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */

    // '    End Module

}