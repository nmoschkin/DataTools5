'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: NativeShell
''         Wrappers for native and COM shell interfaces.
''
'' Some enum documentation copied from the MSDN (and in some cases, updated).
'' Some classes and interfaces were ported from the WindowsAPICodePack.
'' 
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''

Option Explicit On

Imports System
Imports System.IO
Imports System.Collections.Specialized
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.ComTypes
Imports System.Drawing
Imports System.Reflection
Imports System.Text
Imports System.Numerics
Imports System.Threading
Imports Microsoft.Win32

Imports CoreCT.Memory
Imports System.Linq.Expressions
Imports DataTools.Interop.My.Resources

Namespace Native

    ''<HideModuleName>
    ''Public Module NativeShell

#Region "The Windows Shell and Interfaces"

    '' This code is mostly translated from the Windows API Code Pack. I added some IIDs for Windows 8.1
#Region "Microsoft Windows API Code Pack Shell Extension Translation"

#Region "IIDS"

    Public NotInheritable Class ShellIIDGuid

        Public Const IModalWindow As String = "B4DB1657-70D7-485E-8E3E-6FCB5A5C1802"
        Public Const IFileDialog As String = "42F85136-DB7E-439C-85F1-E4075D135FC8"
        Public Const IFileOpenDialog As String = "D57C7288-D4AD-4768-BE02-9D969532D960"
        Public Const IFileSaveDialog As String = "84BCCD23-5FDE-4CDB-AEA4-AF64B83D78AB"
        Public Const IFileDialogEvents As String = "973510DB-7D7F-452B-8975-74A85828D354"
        Public Const IFileDialogControlEvents As String = "36116642-D713-4B97-9B83-7484A9D00433"
        Public Const IFileDialogCustomize As String = "E6FDD21A-163F-4975-9C8C-A69F1BA37034"

        Public Const IShellItem As String = "43826D1E-E718-42EE-BC55-A1E261C37BFE"
        Public Const IShellItem2 As String = "7E9FB0D3-919F-4307-AB2E-9B1860310C93"
        Public Const IShellItemArray As String = "B63EA76D-1F85-456F-A19C-48159EFA858B"
        Public Const IShellLibrary As String = "11A66EFA-382E-451A-9234-1E0E12EF3085"
        Public Const IThumbnailCache As String = "F676C15D-596A-4ce2-8234-33996F445DB1"
        Public Const ISharedBitmap As String = "091162a4-bc96-411f-aae8-c5122cd03363"
        Public Const IShellFolder As String = "000214E6-0000-0000-C000-000000000046"
        Public Const IShellFolder2 As String = "93F2F68C-1D1B-11D3-A30E-00C04F79ABD1"
        Public Const IEnumIDList As String = "000214F2-0000-0000-C000-000000000046"
        Public Const IShellLinkW As String = "000214F9-0000-0000-C000-000000000046"
        Public Const CShellLink As String = "00021401-0000-0000-C000-000000000046"

        Public Const IPropertyStore As String = "886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99"
        Public Const IPropertyStoreCache As String = "3017056d-9a91-4e90-937d-746c72abbf4f"
        Public Const IPropertyDescription As String = "6F79D558-3E96-4549-A1D1-7D75D2288814"
        Public Const IPropertyDescription2 As String = "57D2EDED-5062-400E-B107-5DAE79FE57A6"
        Public Const IPropertyDescriptionList As String = "1F9FC1D0-C39B-4B26-817F-011967D3440E"
        Public Const IPropertyEnumType As String = "11E1FBF9-2D56-4A6B-8DB3-7CD193A471F2"
        Public Const IPropertyEnumType2 As String = "9B6E051C-5DDD-4321-9070-FE2ACB55E794"
        Public Const IPropertyEnumTypeList As String = "A99400F4-3D84-4557-94BA-1242FB2CC9A6"
        Public Const IPropertyStoreCapabilities As String = "c8e2d566-186e-4d49-bf41-6909ead56acc"

        Public Const ICondition As String = "0FC988D4-C935-4b97-A973-46282EA175C8"
        Public Const ISearchFolderItemFactory As String = "a0ffbc28-5482-4366-be27-3e81e78e06c2"
        Public Const IConditionFactory As String = "A5EFE073-B16F-474f-9F3E-9F8B497A3E08"
        Public Const IRichChunk As String = "4FDEF69C-DBC9-454e-9910-B34F3C64B510"
        Public Const IPersistStream As String = "00000109-0000-0000-C000-000000000046"
        Public Const IPersist As String = "0000010c-0000-0000-C000-000000000046"
        Public Const IEnumUnknown As String = "00000100-0000-0000-C000-000000000046"
        Public Const IQuerySolution As String = "D6EBC66B-8921-4193-AFDD-A1789FB7FF57"
        Public Const IQueryParser As String = "2EBDEE67-3505-43f8-9946-EA44ABC8E5B0"
        Public Const IQueryParserManager As String = "A879E3C4-AF77-44fb-8F37-EBD1487CF920"

        Public Const INotinheritableBitmap = "091162a4-bc96-411f-aae8-c5122cd03363"
        Public Const IShellItemImageFactory = "bcc18b79-ba16-442f-80c4-8a59c30c463b"
        Public Const IContextMenu = "000214e4-0000-0000-c000-000000000046"
        Public Const IContextMenu2 = "000214f4-0000-0000-c000-000000000046"
        Public Const IContextMenu3 = "BCFCE0A0-EC17-11D0-8D10-00A0C90F2719"

        Public Const IImageList = "46EB5926-582E-4017-9FDF-E8998DAA0950"
    End Class

    Public NotInheritable Class ShellCLSIDGuid

        '' CLSID GUIDs for relevant coclasses.
        Public Const FileOpenDialog = "DC1C5A9C-E88A-4DDE-A5A1-60F82A20AEF7"
        Public Const FileSaveDialog = "C0B4E2F3-BA21-4773-8DBA-335EC946EB8B"
        Public Const KnownFolderManager = "4DF0C730-DF9D-4AE3-9153-AA6B82E9795A"
        Public Const ShellLibrary = "D9B3211D-E57F-4426-AAEF-30A806ADD397"
        Public Const SearchFolderItemFactory = "14010e02-bbbd-41f0-88e3-eda371216584"
        Public Const ConditionFactory = "E03E85B0-7BE3-4000-BA98-6C13DE9FA486"
        Public Const QueryParserManager = "5088B39A-29B4-4d9d-8245-4EE289222F66"
    End Class

    Public NotInheritable Class ShellKFIDGuid

        Public Const ComputerFolder = "0AC0837C-BBF8-452A-850D-79D08E667CA7"
        Public Const Favorites = "1777F761-68AD-4D8A-87BD-30B759FA33DD"
        Public Const Documents = "FDD39AD0-238F-46AF-ADB4-6C85480369C7"
        Public Const Profile = "5E6C858F-0E22-4760-9AFE-EA3317B67173"

        Public Const GenericLibrary = "5c4f28b5-f869-4e84-8e60-f11db97c5cc7"
        Public Const DocumentsLibrary = "7d49d726-3c21-4f05-99aa-fdc2c9474656"
        Public Const MusicLibrary = "94d6ddcc-4a68-4175-a374-bd584a510b78"
        Public Const PicturesLibrary = "b3690e58-e961-423b-b687-386ebfd83239"
        Public Const VideosLibrary = "5fa96407-7e77-483c-ac93-691d05850de8"

        Public Const Libraries = "1B3EA5DC-B587-4786-B4EF-BD1DC332AEAE"
    End Class

    Public NotInheritable Class ShellBHIDGuid

        Public Const ShellFolderObject = "3981e224-f559-11d3-8e3a-00c04f6837d5"
    End Class

    Public NotInheritable Class KnownFolderIds

        ''' <summary>
        ''' Computer
        ''' </summary>
        Public Shared ReadOnly Computer As Guid = New Guid(&HAC0837C, &HBBF8S, &H452AS, &H85, &HD, &H79, &HD0, &H8E, &H66, &H7C, &HA7)

        ''' <summary>
        ''' Conflicts
        ''' </summary>
        Public Shared ReadOnly Conflict As Guid = New Guid(&H4BFEFB45, &H347DS, &H4006S, &HA5, &HBE, &HAC, &HC, &HB0, &H56, &H71, &H92)

        ''' <summary>
        ''' Control Panel
        ''' </summary>
        Public Shared ReadOnly ControlPanel As Guid = New Guid(&H82A74AEB, &HAEB4S, &H465CS, &HA0, &H14, &HD0, &H97, &HEE, &H34, &H6D, &H63)

        ''' <summary>
        ''' Desktop
        ''' </summary>
        Public Shared ReadOnly Desktop As Guid = New Guid(&HB4BFCC3A, &HDB2CS, &H424CS, &HB0, &H29, &H7F, &HE9, &H9A, &H87, &HC6, &H41)

        ''' <summary>
        ''' Internet Explorer
        ''' </summary>
        Public Shared ReadOnly Internet As Guid = New Guid(&H4D9F7874, &H4E0CS, &H4904S, &H96, &H7B, &H40, &HB0, &HD2, &HC, &H3E, &H4B)

        ''' <summary>
        ''' Network
        ''' </summary>
        Public Shared ReadOnly Network As Guid = New Guid(&HD20BEEC4, &H5CA8S, &H4905S, &HAE, &H3B, &HBF, &H25, &H1E, &HA0, &H9B, &H53)

        ''' <summary>
        ''' Printers
        ''' </summary>
        Public Shared ReadOnly Printers As Guid = New Guid(&H76FC4E2D, &HD6ADS, &H4519S, &HA6, &H63, &H37, &HBD, &H56, &H6, &H81, &H85)

        ''' <summary>
        ''' Sync Center
        ''' </summary>
        Public Shared ReadOnly SyncManager As Guid = New Guid(&H43668BF8, &HC14ES, &H49B2S, &H97, &HC9, &H74, &H77, &H84, &HD7, &H84, &HB7)

        ''' <summary>
        ''' Network Connections
        ''' </summary>
        Public Shared ReadOnly Connections As Guid = New Guid(&H6F0CD92B, &H2E97S, &H45D1S, &H88, &HFF, &HB0, &HD1, &H86, &HB8, &HDE, &HDD)

        ''' <summary>
        ''' Sync Setup
        ''' </summary>
        Public Shared ReadOnly SyncSetup As Guid = New Guid(&HF214138, &HB1D3S, &H4A90S, &HBB, &HA9, &H27, &HCB, &HC0, &HC5, &H38, &H9A)

        ''' <summary>
        ''' Sync Results
        ''' </summary>
        Public Shared ReadOnly SyncResults As Guid = New Guid(&H289A9A43, &HBE44S, &H4057S, &HA4, &H1B, &H58, &H7A, &H76, &HD7, &HE7, &HF9)

        ''' <summary>
        ''' Recycle Bin
        ''' </summary>
        Public Shared ReadOnly RecycleBin As Guid = New Guid(&HB7534046, &H3ECBS, &H4C18S, &HBE, &H4E, &H64, &HCD, &H4C, &HB7, &HD6, &HAC)

        ''' <summary>
        ''' Fonts
        ''' </summary>
        Public Shared ReadOnly Fonts As Guid = New Guid(&HFD228CB7, &HAE11S, &H4AE3S, &H86, &H4C, &H16, &HF3, &H91, &HA, &HB8, &HFE)

        ''' <summary>
        ''' Startup
        ''' </summary>
        Public Shared ReadOnly Startup As Guid = New Guid(&HB97D20BB, &HF46AS, &H4C97S, &HBA, &H10, &H5E, &H36, &H8, &H43, &H8, &H54)

        ''' <summary>
        ''' Programs
        ''' </summary>
        Public Shared ReadOnly Programs As Guid = New Guid(&HA77F5D77, &H2E2BS, &H44C3S, &HA6, &HA2, &HAB, &HA6, &H1, &H5, &H4A, &H51)

        ''' <summary>
        ''' Start Menu
        ''' </summary>
        Public Shared ReadOnly StartMenu As Guid = New Guid(&H625B53C3, &HAB48S, &H4EC1S, &HBA, &H1F, &HA1, &HEF, &H41, &H46, &HFC, &H19)

        ''' <summary>
        ''' Recent Items
        ''' </summary>
        Public Shared ReadOnly Recent As Guid = New Guid(&HAE50C081, &HEBD2S, &H438AS, &H86, &H55, &H8A, &H9, &H2E, &H34, &H98, &H7A)

        ''' <summary>
        ''' SendTo
        ''' </summary>
        Public Shared ReadOnly SendTo As Guid = New Guid(&H8983036C, &H27C0S, &H404BS, &H8F, &H8, &H10, &H2D, &H10, &HDC, &HFD, &H74)

        ''' <summary>
        ''' Documents
        ''' </summary>
        Public Shared ReadOnly Documents As Guid = New Guid(&HFDD39AD0, &H238FS, &H46AFS, &HAD, &HB4, &H6C, &H85, &H48, &H3, &H69, &HC7)

        ''' <summary>
        ''' Favorites
        ''' </summary>
        Public Shared ReadOnly Favorites As Guid = New Guid(&H1777F761, &H68ADS, &H4D8AS, &H87, &HBD, &H30, &HB7, &H59, &HFA, &H33, &HDD)

        ''' <summary>
        ''' Network Shortcuts
        ''' </summary>
        Public Shared ReadOnly NetHood As Guid = New Guid(&HC5ABBF53, &HE17FS, &H4121S, &H89, &H0, &H86, &H62, &H6F, &HC2, &HC9, &H73)

        ''' <summary>
        ''' Printer Shortcuts
        ''' </summary>
        Public Shared ReadOnly PrintHood As Guid = New Guid(&H9274BD8D, &HCFD1S, &H41C3S, &HB3, &H5E, &HB1, &H3F, &H55, &HA7, &H58, &HF4)

        ''' <summary>
        ''' Templates
        ''' </summary>
        Public Shared ReadOnly Templates As Guid = New Guid(&HA63293E8, &H664ES, &H48DBS, &HA0, &H79, &HDF, &H75, &H9E, &H5, &H9, &HF7)

        ''' <summary>
        ''' Startup
        ''' </summary>
        Public Shared ReadOnly CommonStartup As Guid = New Guid(&H82A5EA35, &HD9CDS, &H47C5S, &H96, &H29, &HE1, &H5D, &H2F, &H71, &H4E, &H6E)

        ''' <summary>
        ''' Programs
        ''' </summary>
        Public Shared ReadOnly CommonPrograms As Guid = New Guid(&H139D44E, &H6AFES, &H49F2S, &H86, &H90, &H3D, &HAF, &HCA, &HE6, &HFF, &HB8)

        ''' <summary>
        ''' Start Menu
        ''' </summary>
        Public Shared ReadOnly CommonStartMenu As Guid = New Guid(&HA4115719, &HD62ES, &H491DS, &HAA, &H7C, &HE7, &H4B, &H8B, &HE3, &HB0, &H67)

        ''' <summary>
        ''' Public Desktop
        ''' </summary>
        Public Shared ReadOnly PublicDesktop As Guid = New Guid(&HC4AA340D, &HF20FS, &H4863S, &HAF, &HEF, &HF8, &H7E, &HF2, &HE6, &HBA, &H25)

        ''' <summary>
        ''' ProgramData
        ''' </summary>
        Public Shared ReadOnly ProgramData As Guid = New Guid(&H62AB5D82, &HFDC1S, &H4DC3S, &HA9, &HDD, &H7, &HD, &H1D, &H49, &H5D, &H97)

        ''' <summary>
        ''' Templates
        ''' </summary>
        Public Shared ReadOnly CommonTemplates As Guid = New Guid(&HB94237E7, &H57ACS, &H4347S, &H91, &H51, &HB0, &H8C, &H6C, &H32, &HD1, &HF7)

        ''' <summary>
        ''' Public Documents
        ''' </summary>
        Public Shared ReadOnly PublicDocuments As Guid = New Guid(&HED4824AF, &HDCE4S, &H45A8S, &H81, &HE2, &HFC, &H79, &H65, &H8, &H36, &H34)

        ''' <summary>
        ''' Roaming
        ''' </summary>
        Public Shared ReadOnly RoamingAppData As Guid = New Guid(&H3EB685DB, &H65F9S, &H4CF6S, &HA0, &H3A, &HE3, &HEF, &H65, &H72, &H9F, &H3D)

        ''' <summary>
        ''' Local
        ''' </summary>
        Public Shared ReadOnly LocalAppData As Guid = New Guid(&HF1B32785, &H6FBAS, &H4FCFS, &H9D, &H55, &H7B, &H8E, &H7F, &H15, &H70, &H91)

        ''' <summary>
        ''' LocalLow
        ''' </summary>
        Public Shared ReadOnly LocalAppDataLow As Guid = New Guid(&HA520A1A4, &H1780S, &H4FF6S, &HBD, &H18, &H16, &H73, &H43, &HC5, &HAF, &H16)

        ''' <summary>
        ''' Temporary Internet Files
        ''' </summary>
        Public Shared ReadOnly InternetCache As Guid = New Guid(&H352481E8, &H33BES, &H4251S, &HBA, &H85, &H60, &H7, &HCA, &HED, &HCF, &H9D)

        ''' <summary>
        ''' Cookies
        ''' </summary>
        Public Shared ReadOnly Cookies As Guid = New Guid(&H2B0F765D, &HC0E9S, &H4171S, &H90, &H8E, &H8, &HA6, &H11, &HB8, &H4F, &HF6)

        ''' <summary>
        ''' History
        ''' </summary>
        Public Shared ReadOnly History As Guid = New Guid(&HD9DC8A3B, &HB784S, &H432ES, &HA7, &H81, &H5A, &H11, &H30, &HA7, &H59, &H63)

        ''' <summary>
        ''' System32
        ''' </summary>
        Public Shared ReadOnly System As Guid = New Guid(&H1AC14E77, &H2E7, &H4E5DS, &HB7, &H44, &H2E, &HB1, &HAE, &H51, &H98, &HB7)

        ''' <summary>
        ''' System32
        ''' </summary>
        Public Shared ReadOnly SystemX86 As Guid = New Guid(&HD65231B0, &HB2F1S, &H4857S, &HA4, &HCE, &HA8, &HE7, &HC6, &HEA, &H7D, &H27)

        ''' <summary>
        ''' Windows
        ''' </summary>
        Public Shared ReadOnly Windows As Guid = New Guid(&HF38BF404, &H1D43S, &H42F2S, &H93, &H5, &H67, &HDE, &HB, &H28, &HFC, &H23)

        ''' <summary>
        ''' The user's username (%USERNAME%)
        ''' </summary>
        Public Shared ReadOnly Profile As Guid = New Guid(&H5E6C858F, &HE22, &H4760S, &H9A, &HFE, &HEA, &H33, &H17, &HB6, &H71, &H73)

        ''' <summary>
        ''' Pictures
        ''' </summary>
        Public Shared ReadOnly Pictures As Guid = New Guid(&H33E28130, &H4E1ES, &H4676S, &H83, &H5A, &H98, &H39, &H5C, &H3B, &HC3, &HBB)

        ''' <summary>
        ''' Program Files
        ''' </summary>
        Public Shared ReadOnly ProgramFilesX86 As Guid = New Guid(&H7C5A40EF, &HA0FBS, &H4BFCS, &H87, &H4A, &HC0, &HF2, &HE0, &HB9, &HFA, &H8E)

        ''' <summary>
        ''' Common Files
        ''' </summary>
        Public Shared ReadOnly ProgramFilesCommonX86 As Guid = New Guid(&HDE974D24, &HD9C6S, &H4D3ES, &HBF, &H91, &HF4, &H45, &H51, &H20, &HB9, &H17)

        ''' <summary>
        ''' Program Files
        ''' </summary>
        Public Shared ReadOnly ProgramFilesX64 As Guid = New Guid(&H6D809377, &H6AF0S, &H444BS, &H89, &H57, &HA3, &H77, &H3F, &H2, &H20, &HE)

        ''' <summary>
        ''' Common Files
        ''' </summary>
        Public Shared ReadOnly ProgramFilesCommonX64 As Guid = New Guid(&H6365D5A7, &HF0D, &H45E5S, &H87, &HF6, &HD, &HA5, &H6B, &H6A, &H4F, &H7D)

        ''' <summary>
        ''' Program Files
        ''' </summary>
        Public Shared ReadOnly ProgramFiles As Guid = New Guid(&H905E63B6, &HC1BFS, &H494ES, &HB2, &H9C, &H65, &HB7, &H32, &HD3, &HD2, &H1A)

        ''' <summary>
        ''' Common Files
        ''' </summary>
        Public Shared ReadOnly ProgramFilesCommon As Guid = New Guid(&HF7F1ED05, &H9F6DS, &H47A2S, &HAA, &HAE, &H29, &HD3, &H17, &HC6, &HF0, &H66)

        ''' <summary>
        ''' Administrative Tools
        ''' </summary>
        Public Shared ReadOnly AdminTools As Guid = New Guid(&H724EF170, &HA42DS, &H4FEFS, &H9F, &H26, &HB6, &HE, &H84, &H6F, &HBA, &H4F)

        ''' <summary>
        ''' Administrative Tools
        ''' </summary>
        Public Shared ReadOnly CommonAdminTools As Guid = New Guid(&HD0384E7D, &HBAC3S, &H4797S, &H8F, &H14, &HCB, &HA2, &H29, &HB3, &H92, &HB5)

        ''' <summary>
        ''' Music
        ''' </summary>
        Public Shared ReadOnly Music As Guid = New Guid(&H4BD8D571, &H6D19S, &H48D3S, &HBE, &H97, &H42, &H22, &H20, &H8, &HE, &H43)

        ''' <summary>
        ''' Videos
        ''' </summary>
        Public Shared ReadOnly Videos As Guid = New Guid(&H18989B1D, &H99B5S, &H455BS, &H84, &H1C, &HAB, &H7C, &H74, &HE4, &HDD, &HFC)

        ''' <summary>
        ''' Public Pictures
        ''' </summary>
        Public Shared ReadOnly PublicPictures As Guid = New Guid(&HB6EBFB86, &H6907S, &H413CS, &H9A, &HF7, &H4F, &HC2, &HAB, &HF0, &H7C, &HC5)

        ''' <summary>
        ''' Public Music
        ''' </summary>
        Public Shared ReadOnly PublicMusic As Guid = New Guid(&H3214FAB5, &H9757S, &H4298S, &HBB, &H61, &H92, &HA9, &HDE, &HAA, &H44, &HFF)

        ''' <summary>
        ''' Public Videos
        ''' </summary>
        Public Shared ReadOnly PublicVideos As Guid = New Guid(&H2400183A, &H6185S, &H49FBS, &HA2, &HD8, &H4A, &H39, &H2A, &H60, &H2B, &HA3)

        ''' <summary>
        ''' Resources
        ''' </summary>
        Public Shared ReadOnly ResourceDir As Guid = New Guid(&H8AD10C31, &H2ADBS, &H4296S, &HA8, &HF7, &HE4, &H70, &H12, &H32, &HC9, &H72)

        ''' <summary>
        ''' None
        ''' </summary>
        Public Shared ReadOnly LocalizedResourcesDir As Guid = New Guid(&H2A00375E, &H224CS, &H49DES, &HB8, &HD1, &H44, &HD, &HF7, &HEF, &H3D, &HDC)

        ''' <summary>
        ''' OEM Links
        ''' </summary>
        Public Shared ReadOnly CommonOEMLinks As Guid = New Guid(&HC1BAE2D0, &H10DFS, &H4334S, &HBE, &HDD, &H7A, &HA2, &HB, &H22, &H7A, &H9D)

        ''' <summary>
        ''' Temporary Burn Folder
        ''' </summary>
        Public Shared ReadOnly CDBurning As Guid = New Guid(&H9E52AB10, &HF80DS, &H49DFS, &HAC, &HB8, &H43, &H30, &HF5, &H68, &H78, &H55)

        ''' <summary>
        ''' Users
        ''' </summary>
        Public Shared ReadOnly UserProfiles As Guid = New Guid(&H762D272, &HC50AS, &H4BB0S, &HA3, &H82, &H69, &H7D, &HCD, &H72, &H9B, &H80)

        ''' <summary>
        ''' Playlists
        ''' </summary>
        Public Shared ReadOnly Playlists As Guid = New Guid(&HDE92C1C7, &H837FS, &H4F69S, &HA3, &HBB, &H86, &HE6, &H31, &H20, &H4A, &H23)

        ''' <summary>
        ''' Sample Playlists
        ''' </summary>
        Public Shared ReadOnly SamplePlaylists As Guid = New Guid(&H15CA69B3, &H30EES, &H49C1S, &HAC, &HE1, &H6B, &H5E, &HC3, &H72, &HAF, &HB5)

        ''' <summary>
        ''' Sample Music
        ''' </summary>
        Public Shared ReadOnly SampleMusic As Guid = New Guid(&HB250C668, &HF57DS, &H4EE1S, &HA6, &H3C, &H29, &HE, &HE7, &HD1, &HAA, &H1F)

        ''' <summary>
        ''' Sample Pictures
        ''' </summary>
        Public Shared ReadOnly SamplePictures As Guid = New Guid(&HC4900540, &H2379S, &H4C75S, &H84, &H4B, &H64, &HE6, &HFA, &HF8, &H71, &H6B)

        ''' <summary>
        ''' Sample Videos
        ''' </summary>
        Public Shared ReadOnly SampleVideos As Guid = New Guid(&H859EAD94, &H2E85S, &H48ADS, &HA7, &H1A, &H9, &H69, &HCB, &H56, &HA6, &HCD)

        ''' <summary>
        ''' Slide Shows
        ''' </summary>
        Public Shared ReadOnly PhotoAlbums As Guid = New Guid(&H69D2CF90, &HFC33S, &H4FB7S, &H9A, &HC, &HEB, &HB0, &HF0, &HFC, &HB4, &H3C)

        ''' <summary>
        ''' Public
        ''' </summary>
        Public Shared ReadOnly [Public] As Guid = New Guid(&HDFDF76A2, &HC82AS, &H4D63S, &H90, &H6A, &H56, &H44, &HAC, &H45, &H73, &H85)

        ''' <summary>
        ''' Programs and Features
        ''' </summary>
        Public Shared ReadOnly ChangeRemovePrograms As Guid = New Guid(&HDF7266AC, &H9274S, &H4867S, &H8D, &H55, &H3B, &HD6, &H61, &HDE, &H87, &H2D)

        ''' <summary>
        ''' Installed Updates
        ''' </summary>
        Public Shared ReadOnly AppUpdates As Guid = New Guid(&HA305CE99, &HF527S, &H492BS, &H8B, &H1A, &H7E, &H76, &HFA, &H98, &HD6, &HE4)

        ''' <summary>
        ''' Get Programs
        ''' </summary>
        Public Shared ReadOnly AddNewPrograms As Guid = New Guid(&HDE61D971, &H5EBCS, &H4F02S, &HA3, &HA9, &H6C, &H82, &H89, &H5E, &H5C, &H4)

        ''' <summary>
        ''' Downloads
        ''' </summary>
        Public Shared ReadOnly Downloads As Guid = New Guid(&H374DE290, &H123FS, &H4565S, &H91, &H64, &H39, &HC4, &H92, &H5E, &H46, &H7B)

        ''' <summary>
        ''' Public Downloads
        ''' </summary>
        Public Shared ReadOnly PublicDownloads As Guid = New Guid(&H3D644C9B, &H1FB8S, &H4F30S, &H9B, &H45, &HF6, &H70, &H23, &H5F, &H79, &HC0)

        ''' <summary>
        ''' Searches
        ''' </summary>
        Public Shared ReadOnly SavedSearches As Guid = New Guid(&H7D1D3A04, &HDEBBS, &H4115S, &H95, &HCF, &H2F, &H29, &HDA, &H29, &H20, &HDA)

        ''' <summary>
        ''' Quick Launch
        ''' </summary>
        Public Shared ReadOnly QuickLaunch As Guid = New Guid(&H52A4F021, &H7B75S, &H48A9S, &H9F, &H6B, &H4B, &H87, &HA2, &H10, &HBC, &H8F)

        ''' <summary>
        ''' Contacts
        ''' </summary>
        Public Shared ReadOnly Contacts As Guid = New Guid(&H56784854, &HC6CBS, &H462BS, &H81, &H69, &H88, &HE3, &H50, &HAC, &HB8, &H82)

        ''' <summary>
        ''' Gadgets
        ''' </summary>
        Public Shared ReadOnly SidebarParts As Guid = New Guid(&HA75D362E, &H50FCS, &H4FB7S, &HAC, &H2C, &HA8, &HBE, &HAA, &H31, &H44, &H93)

        ''' <summary>
        ''' Gadgets
        ''' </summary>
        Public Shared ReadOnly SidebarDefaultParts As Guid = New Guid(&H7B396E54, &H9EC5S, &H4300S, &HBE, &HA, &H24, &H82, &HEB, &HAE, &H1A, &H26)

        ''' <summary>
        ''' Tree property value folder
        ''' </summary>
        Public Shared ReadOnly TreeProperties As Guid = New Guid(&H5B3749AD, &HB49FS, &H49C1S, &H83, &HEB, &H15, &H37, &HF, &HBD, &H48, &H82)

        ''' <summary>
        ''' GameExplorer
        ''' </summary>
        Public Shared ReadOnly PublicGameTasks As Guid = New Guid(&HDEBF2536, &HE1A8S, &H4C59S, &HB6, &HA2, &H41, &H45, &H86, &H47, &H6A, &HEA)

        ''' <summary>
        ''' GameExplorer
        ''' </summary>
        Public Shared ReadOnly GameTasks As Guid = New Guid(&H54FAE61, &H4DD8S, &H4787S, &H80, &HB6, &H9, &H2, &H20, &HC4, &HB7, &H0)

        ''' <summary>
        ''' Saved Games
        ''' </summary>
        Public Shared ReadOnly SavedGames As Guid = New Guid(&H4C5C32FF, &HBB9DS, &H43B0S, &HB5, &HB4, &H2D, &H72, &HE5, &H4E, &HAA, &HA4)

        ''' <summary>
        ''' Games
        ''' </summary>
        Public Shared ReadOnly Games As Guid = New Guid(&HCAC52C1A, &HB53DS, &H4EDCS, &H92, &HD7, &H6B, &H2E, &H8A, &HC1, &H94, &H34)

        ''' <summary>
        ''' Recorded TV
        ''' </summary>
        Public Shared ReadOnly RecordedTV As Guid = New Guid(&HBD85E001, &H112ES, &H431ES, &H98, &H3B, &H7B, &H15, &HAC, &H9, &HFF, &HF1)

        ''' <summary>
        ''' Microsoft Office Outlook
        ''' </summary>
        Public Shared ReadOnly SearchMapi As Guid = New Guid(&H98EC0E18, &H2098S, &H4D44S, &H86, &H44, &H66, &H97, &H93, &H15, &HA2, &H81)

        ''' <summary>
        ''' Offline Files
        ''' </summary>
        Public Shared ReadOnly SearchCsc As Guid = New Guid(&HEE32E446, &H31CAS, &H4ABAS, &H81, &H4F, &HA5, &HEB, &HD2, &HFD, &H6D, &H5E)

        ''' <summary>
        ''' Links
        ''' </summary>
        Public Shared ReadOnly Links As Guid = New Guid(&HBFB9D5E0, &HC6A9S, &H404CS, &HB2, &HB2, &HAE, &H6D, &HB6, &HAF, &H49, &H68)

        ''' <summary>
        ''' The user's full name (for instance, Jean Philippe Bagel) entered when the user account was created.
        ''' </summary>
        Public Shared ReadOnly UsersFiles As Guid = New Guid(&HF3CE0F7C, &H4901S, &H4ACCS, &H86, &H48, &HD5, &HD4, &H4B, &H4, &HEF, &H8F)

        ''' <summary>
        ''' Search home
        ''' </summary>
        Public Shared ReadOnly SearchHome As Guid = New Guid(&H190337D1, &HB8CAS, &H4121S, &HA6, &H39, &H6D, &H47, &H2D, &H16, &H97, &H2A)

        ''' <summary>
        ''' Original Images
        ''' </summary>
        Public Shared ReadOnly OriginalImages As Guid = New Guid(&H2C36C0AA, &H5812S, &H4B87S, &HBF, &HD0, &H4C, &HD0, &HDF, &HB1, &H9B, &H39)

        ''' <summary>
        ''' SkyDrive; Windows 8.1 Folder
        ''' </summary>
        Public Shared ReadOnly SkyDrive As Guid = New Guid(&HA52BBA46, &HE9E1S, &H435FS, &HB3, &HD9, &H28, &HDA, &HA6, &H48, &HC0, &HF6)

        ''' <summary>
        ''' OneDrive; Windows 8.1/Windows 10 Folder
        ''' </summary>
        Public Shared ReadOnly OneDrive As Guid = New Guid(&HA52BBA46, &HE9E1S, &H435FS, &HB3, &HD9, &H28, &HDA, &HA6, &H48, &HC0, &HF6)

    End Class

#End Region

#Region "Shell Enums"

    ''' <summary>
    ''' CommonFileDialog AddPlace locations
    ''' </summary>
    Public Enum FileDialogAddPlaceLocation
        ''' <summary>
        ''' At the bottom of the Favorites or Places list.
        ''' </summary>
        Bottom = &H0

        ''' <summary>
        ''' At the top of the Favorites or Places list.
        ''' </summary>
        Top = &H1
    End Enum

    ''' <summary>
    ''' One of the values that indicates how the ShellObject DisplayName should look.
    ''' </summary>
    Public Enum DisplayNameType
        ''' <summary>
        ''' Returns the display name relative to the desktop.
        ''' </summary>
        [Default] = &H0

        ''' <summary>
        ''' Returns the parsing name relative to the parent folder.
        ''' </summary>
        RelativeToParent = CInt(&H80018001I)

        ''' <summary>
        ''' Returns the path relative to the parent folder in a 
        ''' friendly format as displayed in an address bar.
        ''' </summary>
        RelativeToParentAddressBar = CInt(&H8007C001I)

        ''' <summary>
        ''' Returns the parsing name relative to the desktop.
        ''' </summary>
        RelativeToDesktop = CInt(&H80028000I)

        ''' <summary>
        ''' Returns the editing name relative to the parent folder.
        ''' </summary>
        RelativeToParentEditing = CInt(&H80031001I)

        ''' <summary>
        ''' Returns the editing name relative to the desktop.
        ''' </summary>
        RelativeToDesktopEditing = CInt(&H8004C000I)

        ''' <summary>
        ''' Returns the display name relative to the file system path.
        ''' </summary>
        FileSystemPath = CInt(&H80058000I)

        ''' <summary>
        ''' Returns the display name relative to a URL.
        ''' </summary>
        Url = CInt(&H80068000I)
    End Enum
    ''' <summary>
    ''' Available Library folder types
    ''' </summary>
    Public Enum LibraryFolderType
        ''' <summary>
        ''' General Items
        ''' </summary>
        Generic = 0

        ''' <summary>
        ''' Documents
        ''' </summary>
        Documents

        ''' <summary>
        ''' Music
        ''' </summary>
        Music

        ''' <summary>
        ''' Pictures
        ''' </summary>
        Pictures

        ''' <summary>
        ''' Videos
        ''' </summary>
        Videos

    End Enum

    ''' <summary>
    ''' Flags controlling the appearance of a window
    ''' </summary>
    Public Enum WindowShowCommand
        ''' <summary>
        ''' Hides the window and activates another window.
        ''' </summary>
        Hide = 0

        ''' <summary>
        ''' Activates and displays the window (including restoring
        ''' it to its original size and position).
        ''' </summary>
        Normal = 1

        ''' <summary>
        ''' Minimizes the window.
        ''' </summary>
        Minimized = 2

        ''' <summary>
        ''' Maximizes the window.
        ''' </summary>
        Maximized = 3

        ''' <summary>
        ''' Similar to Normal, except that the window
        ''' is not activated.
        ''' </summary>
        ShowNoActivate = 4

        ''' <summary>
        ''' Activates the window and displays it in its current size
        ''' and position.
        ''' </summary>
        Show = 5

        ''' <summary>
        ''' Minimizes the window and activates the next top-level window.
        ''' </summary>
        Minimize = 6

        ''' <summary>
        ''' Minimizes the window and does not activate it.
        ''' </summary>
        ShowMinimizedNoActivate = 7

        ''' <summary>
        ''' Similar to Normal, except that the window is not
        ''' activated.
        ''' </summary>
        ShowNA = 8

        ''' <summary>
        ''' Activates and displays the window, restoring it to its original
        ''' size and position.
        ''' </summary>
        Restore = 9

        ''' <summary>
        ''' Sets the show state based on the initial value specified when
        ''' the process was created.
        ''' </summary>
        [Default] = 10

        ''' <summary>
        ''' Minimizes a window, even if the thread owning the window is not
        ''' responding.  Use this only to minimize windows from a different
        ''' thread.
        ''' </summary>
        ForceMinimize = 11
    End Enum

    ''' <summary>
    ''' Provides a set of flags to be used with SearchCondition
    ''' to indicate the operation in SearchConditionFactory's methods.
    ''' </summary>
    Public Enum SearchConditionOperation
        ''' <summary>
        ''' An implicit comparison between the value of the property and the value of the constant.
        ''' </summary>
        Implicit = 0

        ''' <summary>
        ''' The value of the property and the value of the constant must be equal.
        ''' </summary>
        Equal = 1

        ''' <summary>
        ''' The value of the property and the value of the constant must not be equal.
        ''' </summary>
        NotEqual = 2

        ''' <summary>
        ''' The value of the property must be less than the value of the constant.
        ''' </summary>
        LessThan = 3

        ''' <summary>
        ''' The value of the property must be greater than the value of the constant.
        ''' </summary>
        GreaterThan = 4

        ''' <summary>
        ''' The value of the property must be less than or equal to the value of the constant.
        ''' </summary>
        LessThanOrEqual = 5

        ''' <summary>
        ''' The value of the property must be greater than or equal to the value of the constant.
        ''' </summary>
        GreaterThanOrEqual = 6

        ''' <summary>
        ''' The value of the property must begin with the value of the constant.
        ''' </summary>
        ValueStartsWith = 7

        ''' <summary>
        ''' The value of the property must end with the value of the constant.
        ''' </summary>
        ValueEndsWith = 8

        ''' <summary>
        ''' The value of the property must contain the value of the constant.
        ''' </summary>
        ValueContains = 9

        ''' <summary>
        ''' The value of the property must not contain the value of the constant.
        ''' </summary>
        ValueNotContains = 10

        ''' <summary>
        ''' The value of the property must match the value of the constant, where '?' 
        ''' matches any single character and '*' matches any sequence of characters.
        ''' </summary>
        DosWildcards = 11

        ''' <summary>
        ''' The value of the property must contain a word that is the value of the constant.
        ''' </summary>
        WordEqual = 12

        ''' <summary>
        ''' The value of the property must contain a word that begins with the value of the constant.
        ''' </summary>
        WordStartsWith = 13

        ''' <summary>
        ''' The application is free to interpret this in any suitable way.
        ''' </summary>
        ApplicationSpecific = 14
    End Enum

    ''' <summary>
    ''' Set of flags to be used with SearchConditionFactory.
    ''' </summary>
    Public Enum SearchConditionType
        ''' <summary>
        ''' Indicates that the values of the subterms are combined by "AND".
        ''' </summary>
        [And] = 0

        ''' <summary>
        ''' Indicates that the values of the subterms are combined by "OR".
        ''' </summary>
        [Or] = 1

        ''' <summary>
        ''' Indicates a "NOT" comparison of subterms.
        ''' </summary>
        [Not] = 2

        ''' <summary>
        ''' Indicates that the node is a comparison between a property and a 
        ''' constant value using a SearchConditionOperation.
        ''' </summary>
        Leaf = 3
    End Enum

    ''' <summary>
    ''' Used to describe the view mode.
    ''' </summary>
    Public Enum FolderLogicalViewMode
        ''' <summary>
        ''' The view is not specified.
        ''' </summary>
        Unspecified = -1

        ''' <summary>
        ''' This should have the same affect as Unspecified.
        ''' </summary>
        None = 0

        ''' <summary>
        ''' The minimum valid enumeration value. Used for validation purposes only.
        ''' </summary>
        First = 1

        ''' <summary>
        ''' Details view.
        ''' </summary>
        Details = 1

        ''' <summary>
        ''' Tiles view.
        ''' </summary>
        Tiles = 2

        ''' <summary>
        ''' Icons view.
        ''' </summary>
        Icons = 3

        ''' <summary>
        ''' Windows 7 and later. List view.
        ''' </summary>
        List = 4

        ''' <summary>
        ''' Windows 7 and later. Content view.
        ''' </summary>
        Content = 5

        ''' <summary>
        ''' The maximum valid enumeration value. Used for validation purposes only.
        ''' </summary>
        Last = 5
    End Enum

    ''' <summary>
    ''' The direction in which the items are sorted.
    ''' </summary>
    Public Enum SortDirection
        ''' <summary>
        ''' A default value for sort direction, this value should not be used;
        ''' instead use Descending or Ascending.
        ''' </summary>
        [Default] = 0

        ''' <summary>
        ''' The items are sorted in descending order. Whether the sort is alphabetical, numerical, 
        ''' and so on, is determined by the data type of the column indicated in propkey.
        ''' </summary>
        Descending = -1

        ''' <summary>
        ''' The items are sorted in ascending order. Whether the sort is alphabetical, numerical, 
        ''' and so on, is determined by the data type of the column indicated in propkey.
        ''' </summary>
        Ascending = 1
    End Enum

    ''' <summary>
    ''' Provides a set of flags to be used with IQueryParser::SetOption and 
    ''' IQueryParser::GetOption to indicate individual options.
    ''' </summary>
    Public Enum StructuredQuerySingleOption
        ''' <summary>
        ''' The value should be VT_LPWSTR and the path to a file containing a schema binary.
        ''' </summary>
        Schema

        ''' <summary>
        ''' The value must be VT_EMPTY (the default) or a VT_UI4 that is an LCID. It is used
        ''' as the locale of contents (not keywords) in the query to be searched for, when no
        ''' other information is available. The default value is the current keyboard locale.
        ''' Retrieving the value always returns a VT_UI4.
        ''' </summary>
        Locale

        ''' <summary>
        ''' This option is used to override the default word breaker used when identifying keywords
        ''' in queries. The default word breaker is chosen according to the language of the keywords
        ''' (cf. SQSO_LANGUAGE_KEYWORDS below). When setting this option, the value should be VT_EMPTY
        ''' for using the default word breaker, or a VT_UNKNOWN with an object supporting
        ''' the IWordBreaker interface. Retrieving the option always returns a VT_UNKNOWN with an object
        ''' supporting the IWordBreaker interface.
        ''' </summary>
        WordBreaker

        ''' <summary>
        ''' The value should be VT_EMPTY or VT_BOOL with VARIANT_TRUE to allow natural query
        ''' syntax (the default) or VT_BOOL with VARIANT_FALSE to allow only advanced query syntax.
        ''' Retrieving the option always returns a VT_BOOL.
        ''' This option is now deprecated, use SQSO_SYNTAX.
        ''' </summary>
        NaturalSyntax

        ''' <summary>
        ''' The value should be VT_BOOL with VARIANT_TRUE to generate query expressions
        ''' as if each word in the query had a star appended to it (unless followed by punctuation
        ''' other than a parenthesis), or VT_EMPTY or VT_BOOL with VARIANT_FALSE to
        ''' use the words as they are (the default). A word-wheeling application
        ''' will generally want to set this option to true.
        ''' Retrieving the option always returns a VT_BOOL.
        ''' </summary>
        AutomaticWildcard

        ''' <summary>
        ''' Reserved. The value should be VT_EMPTY (the default) or VT_I4.
        ''' Retrieving the option always returns a VT_I4.
        ''' </summary>
        TraceLevel

        ''' <summary>
        ''' The value must be a VT_UI4 that is a LANGID. It defaults to the default user UI language.
        ''' </summary>
        LanguageKeywords

        ''' <summary>
        ''' The value must be a VT_UI4 that is a STRUCTURED_QUERY_SYNTAX value.
        ''' It defaults to SQS_NATURAL_QUERY_SYNTAX.
        ''' </summary>
        Syntax

        ''' <summary>
        ''' The value must be a VT_BLOB that is a copy of a TIME_ZONE_INFORMATION structure.
        ''' It defaults to the current time zone.
        ''' </summary>
        TimeZone

        ''' <summary>
        ''' This setting decides what connector should be assumed between conditions when none is specified.
        ''' The value must be a VT_UI4 that is a CONDITION_TYPE. Only CT_AND_CONDITION and CT_OR_CONDITION
        ''' are valid. It defaults to CT_AND_CONDITION.
        ''' </summary>
        ImplicitConnector

        ''' <summary>
        ''' This setting decides whether there are special requirements on the case of connector keywords (such
        ''' as AND or OR). The value must be a VT_UI4 that is a CASE_REQUIREMENT value.
        ''' It defaults to CASE_REQUIREMENT_UPPER_IF_AQS.
        ''' </summary>
        ConnectorCase

    End Enum

    ''' <summary>
    ''' Provides a set of flags to be used with IQueryParser::SetMultiOption 
    ''' to indicate individual options.
    ''' </summary>
    Public Enum StructuredQueryMultipleOption
        ''' <summary>
        ''' The key should be property name P. The value should be a
        ''' VT_UNKNOWN with an IEnumVARIANT which has two values: a VT_BSTR that is another
        ''' property name Q and a VT_I4 that is a CONDITION_OPERATION cop. A predicate with
        ''' property name P, some operation and a value V will then be replaced by a predicate
        ''' with property name Q, operation cop and value V before further processing happens.
        ''' </summary>
        VirtualProperty

        ''' <summary>
        ''' The key should be a value type name V. The value should be a
        ''' VT_LPWSTR with a property name P. A predicate with no property name and a value of type
        ''' V (or any subtype of V) will then use property P.
        ''' </summary>
        DefaultProperty

        ''' <summary>
        ''' The key should be a value type name V. The value should be a
        ''' VT_UNKNOWN with a IConditionGenerator G. The GenerateForLeaf method of
        ''' G will then be applied to any predicate with value type V and if it returns a query
        ''' expression, that will be used. If it returns NULL, normal processing will be used
        ''' instead.
        ''' </summary>
        GeneratorForType

        ''' <summary>
        ''' The key should be a property name P. The value should be a VT_VECTOR|VT_LPWSTR,
        ''' where each string is a property name. The count must be at least one. This "map" will be
        ''' added to those of the loaded schema and used during resolution. A second call with the
        ''' same key will replace the current map. If the value is VT_NULL, the map will be removed.
        ''' </summary>
        MapProperty
    End Enum

    ''' <summary>
    ''' Used by IQueryParserManager::SetOption to set parsing options. 
    ''' This can be used to specify schemas and localization options.
    ''' </summary>
    Public Enum QueryParserManagerOption
        ''' <summary>
        ''' A VT_LPWSTR containing the name of the file that contains the schema binary. 
        ''' The default value is StructuredQuerySchema.bin for the SystemIndex catalog 
        ''' and StructuredQuerySchemaTrivial.bin for the trivial catalog.
        ''' </summary>
        SchemaBinaryName = 0

        ''' <summary>
        ''' Either a VT_BOOL or a VT_LPWSTR. If the value is a VT_BOOL and is FALSE, 
        ''' a pre-localized schema will not be used. If the value is a VT_BOOL and is TRUE, 
        ''' IQueryParserManager will use the pre-localized schema binary in 
        ''' "%ALLUSERSPROFILE%\Microsoft\Windows". If the value is a VT_LPWSTR, the value should 
        ''' contain the full path of the folder in which the pre-localized schema binary can be found. 
        ''' The default value is VT_BOOL with TRUE.
        ''' </summary>
        PreLocalizedSchemaBinaryPath = 1

        ''' <summary>
        ''' A VT_LPWSTR containing the full path to the folder that contains the 
        ''' unlocalized schema binary. The default value is "%SYSTEMROOT%\System32".
        ''' </summary>
        UnlocalizedSchemaBinaryPath = 2

        ''' <summary>
        ''' A VT_LPWSTR containing the full path to the folder that contains the 
        ''' localized schema binary that can be read and written to as needed. 
        ''' The default value is "%LOCALAPPDATA%\Microsoft\Windows".
        ''' </summary>
        LocalizedSchemaBinaryPath = 3

        ''' <summary>
        ''' A VT_BOOL. If TRUE, then the paths for pre-localized and localized binaries 
        ''' have "\(LCID)" appended to them, where language code identifier (LCID) is 
        ''' the decimal locale ID for the localized language. The default is TRUE.
        ''' </summary>
        AppendLCIDToLocalizedPath = 4

        ''' <summary>
        ''' A VT_UNKNOWN with an object supporting ISchemaLocalizerSupport. 
        ''' This object will be used instead of the default localizer support object.
        ''' </summary>
        LocalizerSupport = 5
    End Enum

    <Flags>
    Public Enum FileOpenOptions
        OverwritePrompt = &H2
        StrictFileTypes = &H4
        NoChangeDirectory = &H8
        PickFolders = &H20
        ' Ensure that items returned are filesystem items.
        ForceFilesystem = &H40
        ' Allow choosing items that have no storage.
        AllNonStorageItems = &H80
        NoValidate = &H100
        AllowMultiSelect = &H200
        PathMustExist = &H800
        FileMustExist = &H1000
        CreatePrompt = &H2000
        ShareAware = &H4000
        NoReadOnlyReturn = &H8000
        NoTestFileCreate = &H10000
        HideMruPlaces = &H20000
        HidePinnedPlaces = &H40000
        NoDereferenceLinks = &H100000
        DontAddToRecent = &H2000000
        ForceShowHidden = &H10000000
        DefaultNoMiniMode = &H20000000
    End Enum
    Public Enum ControlState
        Inactive = &H0
        Enable = &H1
        Visible = &H2
    End Enum
    Public Enum ShellItemDesignNameOptions
        Normal = &H0
        ' SIGDN_NORMAL
        ParentRelativeParsing = CInt(&H80018001I)
        ' SIGDN_INFOLDER | SIGDN_FORPARSING
        DesktopAbsoluteParsing = CInt(&H80028000I)
        ' SIGDN_FORPARSING
        ParentRelativeEditing = CInt(&H80031001I)
        ' SIGDN_INFOLDER | SIGDN_FOREDITING
        DesktopAbsoluteEditing = CInt(&H8004C000I)
        ' SIGDN_FORPARSING | SIGDN_FORADDRESSBAR
        FileSystemPath = CInt(&H80058000I)
        ' SIGDN_FORPARSING
        Url = CInt(&H80068000I)
        ' SIGDN_FORPARSING
        ParentRelativeForAddressBar = CInt(&H8007C001I)
        ' SIGDN_INFOLDER | SIGDN_FORPARSING | SIGDN_FORADDRESSBAR
        ParentRelative = CInt(&H80080001I)
        ' SIGDN_INFOLDER
    End Enum

    ''' <summary>
    ''' Indicate flags that modify the property store object retrieved by methods 
    ''' that create a property store, such as IShellItem2::GetPropertyStore or 
    ''' IPropertyStoreFactory::GetPropertyStore.
    ''' </summary>
    <Flags>
    Public Enum GetPropertyStoreOptions
        ''' <summary>
        ''' Meaning to a calling process: Return a read-only property store that contains all 
        ''' properties. Slow items (offline files) are not opened. 
        ''' Combination with other flags: Can be overridden by other flags.
        ''' </summary>
        [Default] = 0

        ''' <summary>
        ''' Meaning to a calling process: Include only properties directly from the property
        ''' handler, which opens the file on the disk, network, or device. Meaning to a file 
        ''' folder: Only include properties directly from the handler.
        ''' 
        ''' Meaning to other folders: When delegating to a file folder, pass this flag on 
        ''' to the file folder; do not do any multiplexing (MUX). When not delegating to a 
        ''' file folder, ignore this flag instead of returning a failure code.
        ''' 
        ''' Combination with other flags: Cannot be combined with GPS_TEMPORARY, 
        ''' GPS_FASTPROPERTIESONLY, or GPS_BESTEFFORT.
        ''' </summary>
        HandlePropertiesOnly = &H1

        ''' <summary>
        ''' Meaning to a calling process: Can write properties to the item. 
        ''' Note: The store may contain fewer properties than a read-only store. 
        ''' 
        ''' Meaning to a file folder: ReadWrite.
        ''' 
        ''' Meaning to other folders: ReadWrite. Note: When using default MUX, 
        ''' return a single unmultiplexed store because the default MUX does not support ReadWrite.
        ''' 
        ''' Combination with other flags: Cannot be combined with GPS_TEMPORARY, GPS_FASTPROPERTIESONLY, 
        ''' GPS_BESTEFFORT, or GPS_DELAYCREATION. Implies GPS_HANDLERPROPERTIESONLY.
        ''' </summary>
        ReadWrite = &H2

        ''' <summary>
        ''' Meaning to a calling process: Provides a writable store, with no initial properties, 
        ''' that exists for the lifetime of the Shell item instance; basically, a property bag 
        ''' attached to the item instance. 
        ''' 
        ''' Meaning to a file folder: Not applicable. Handled by the Shell item.
        ''' 
        ''' Meaning to other folders: Not applicable. Handled by the Shell item.
        ''' 
        ''' Combination with other flags: Cannot be combined with any other flag. Implies GPS_READWRITE
        ''' </summary>
        Temporary = &H4

        ''' <summary>
        ''' Meaning to a calling process: Provides a store that does not involve reading from the 
        ''' disk or network. Note: Some values may be different, or missing, compared to a store 
        ''' without this flag. 
        ''' 
        ''' Meaning to a file folder: Include the "innate" and "fallback" stores only. Do not load the handler.
        ''' 
        ''' Meaning to other folders: Include only properties that are available in memory or can 
        ''' be computed very quickly (no properties from disk, network, or peripheral IO devices). 
        ''' This is normally only data sources from the IDLIST. When delegating to other folders, pass this flag on to them.
        ''' 
        ''' Combination with other flags: Cannot be combined with GPS_TEMPORARY, GPS_READWRITE, 
        ''' GPS_HANDLERPROPERTIESONLY, or GPS_DELAYCREATION.
        ''' </summary>
        FastPropertiesOnly = &H8

        ''' <summary>
        ''' Meaning to a calling process: Open a slow item (offline file) if necessary. 
        ''' Meaning to a file folder: Retrieve a file from offline storage, if necessary. 
        ''' Note: Without this flag, the handler is not created for offline files.
        ''' 
        ''' Meaning to other folders: Do not return any properties that are very slow.
        ''' 
        ''' Combination with other flags: Cannot be combined with GPS_TEMPORARY or GPS_FASTPROPERTIESONLY.
        ''' </summary>
        OpensLowItem = &H10

        ''' <summary>
        ''' Meaning to a calling process: Delay memory-intensive operations, such as file access, until 
        ''' a property is requested that requires such access. 
        ''' 
        ''' Meaning to a file folder: Do not create the handler until needed; for example, either 
        ''' GetCount/GetAt or GetValue, where the innate store does not satisfy the request. 
        ''' Note: GetValue might fail due to file access problems.
        ''' 
        ''' Meaning to other folders: If the folder has memory-intensive properties, such as 
        ''' delegating to a file folder or network access, it can optimize performance by 
        ''' supporting IDelayedPropertyStoreFactory and splitting up its properties into a 
        ''' fast and a slow store. It can then use delayed MUX to recombine them.
        ''' 
        ''' Combination with other flags: Cannot be combined with GPS_TEMPORARY or 
        ''' GPS_READWRITE
        ''' </summary>
        DelayCreation = &H20

        ''' <summary>
        ''' Meaning to a calling process: Succeed at getting the store, even if some 
        ''' properties are not returned. Note: Some values may be different, or missing,
        ''' compared to a store without this flag. 
        ''' 
        ''' Meaning to a file folder: Succeed and return a store, even if the handler or 
        ''' innate store has an error during creation. Only fail if substores fail.
        ''' 
        ''' Meaning to other folders: Succeed on getting the store, even if some properties 
        ''' are not returned.
        ''' 
        ''' Combination with other flags: Cannot be combined with GPS_TEMPORARY, 
        ''' GPS_READWRITE, or GPS_HANDLERPROPERTIESONLY.
        ''' </summary>
        BestEffort = &H40

        ''' <summary>
        ''' Mask for valid GETPROPERTYSTOREFLAGS values.
        ''' </summary>
        MaskValid = &HFF
    End Enum


    Public Enum SHOPType

        SHOP_PRINTERNAME = &H1 '' lpObject points To a printer friendly name

        SHOP_FILEPATH = &H2 '' lpObject points To a fully qualified path+file name

        SHOP_VOLUMEGUID = &H4 '' lpObject points To a Volume GUID

    End Enum

    Public Enum ShellItemAttributeOptions
        ' if multiple items and the attirbutes together.
        [And] = &H1
        ' if multiple items or the attributes together.
        [Or] = &H2
        ' Call GetAttributes directly on the 
        ' ShellFolder for multiple attributes.
        AppCompat = &H3

        ' A mask for SIATTRIBFLAGS_AND, SIATTRIBFLAGS_OR, and SIATTRIBFLAGS_APPCOMPAT. Callers normally do not use this value.
        Mask = &H3

        ' Windows 7 and later. Examine all items in the array to compute the attributes. 
        ' Note that this can result in poor performance over large arrays and therefore it 
        ' should be used only when needed. Cases in which you pass this flag should be extremely rare.
        AllItems = &H4000
    End Enum

    Public Enum FileDialogEventShareViolationResponse
        [Default] = &H0
        Accept = &H1
        Refuse = &H2
    End Enum
    Public Enum FileDialogEventOverwriteResponse
        [Default] = &H0
        Accept = &H1
        Refuse = &H2
    End Enum
    Public Enum FileDialogAddPlacement
        Bottom = &H0
        Top = &H1
    End Enum

    <Flags>
    Public Enum SIIGBF
        ResizeToFit = &H0
        BiggerSizeOk = &H1
        MemoryOnly = &H2
        IconOnly = &H4
        ThumbnailOnly = &H8
        InCacheOnly = &H10
    End Enum

    <Flags>
    Public Enum ThumbnailOptions
        Extract = &H0
        InCacheOnly = &H1
        FastExtract = &H2
        ForceExtraction = &H4
        SlowReclaim = &H8
        ExtractDoNotCache = &H20
    End Enum

    <Flags>
    Public Enum ThumbnailCacheOptions
        [Default] = &H0
        LowQuality = &H1
        Cached = &H2
    End Enum

    <Flags>
    Public Enum ShellFileGetAttributesOptions
        ''' <summary>
        ''' The specified items can be copied.
        ''' </summary>
        CanCopy = &H1

        ''' <summary>
        ''' The specified items can be moved.
        ''' </summary>
        CanMove = &H2

        ''' <summary>
        ''' Shortcuts can be created for the specified items. This flag has the same value as DROPEFFECT. 
        ''' The normal use of this flag is to add a Create Shortcut item to the shortcut menu that is displayed 
        ''' during drag-and-drop operations. However, SFGAO_CANLINK also adds a Create Shortcut item to the Microsoft 
        ''' Windows Explorer's File menu and to normal shortcut menus. 
        ''' If this item is selected, your application's IContextMenu::InvokeCommand is invoked with the lpVerb 
        ''' member of the CMINVOKECOMMANDINFO structure set to "link." Your application is responsible for creating the link.
        ''' </summary>
        CanLink = &H4

        ''' <summary>
        ''' The specified items can be bound to an IStorage interface through IShellFolder::BindToObject.
        ''' </summary>
        Storage = &H8

        ''' <summary>
        ''' The specified items can be renamed.
        ''' </summary>
        CanRename = &H10

        ''' <summary>
        ''' The specified items can be deleted.
        ''' </summary>
        CanDelete = &H20

        ''' <summary>
        ''' The specified items have property sheets.
        ''' </summary>
        HasPropertySheet = &H40

        ''' <summary>
        ''' The specified items are drop targets.
        ''' </summary>
        DropTarget = &H100

        ''' <summary>
        ''' This flag is a mask for the capability flags.
        ''' </summary>
        CapabilityMask = &H177

        ''' <summary>
        ''' Windows 7 and later. The specified items are system items.
        ''' </summary>
        System = &H1000

        ''' <summary>
        ''' The specified items are encrypted.
        ''' </summary>
        Encrypted = &H2000

        ''' <summary>
        ''' Indicates that accessing the object = through IStream or other storage interfaces, 
        ''' is a slow operation. 
        ''' Applications should avoid accessing items flagged with SFGAO_ISSLOW.
        ''' </summary>
        IsSlow = &H4000

        ''' <summary>
        ''' The specified items are ghosted icons.
        ''' </summary>
        Ghosted = &H8000

        ''' <summary>
        ''' The specified items are shortcuts.
        ''' </summary>
        Link = &H10000

        ''' <summary>
        ''' The specified folder objects are shared.
        ''' </summary>    
        Share = &H20000

        ''' <summary>
        ''' The specified items are read-only. In the case of folders, this means 
        ''' that new items cannot be created in those folders.
        ''' </summary>
        [ReadOnly] = &H40000

        ''' <summary>
        ''' The item is hidden and should not be displayed unless the 
        ''' Show hidden files and folders option is enabled in Folder Settings.
        ''' </summary>
        Hidden = &H80000

        ''' <summary>
        ''' This flag is a mask for the display attributes.
        ''' </summary>
        DisplayAttributeMask = &HFC000

        ''' <summary>
        ''' The specified folders contain one or more file system folders.
        ''' </summary>
        FileSystemAncestor = &H10000000

        ''' <summary>
        ''' The specified items are folders.
        ''' </summary>
        Folder = &H20000000

        ''' <summary>
        ''' The specified folders or file objects are part of the file system 
        ''' that is, they are files, directories, or root directories).
        ''' </summary>
        FileSystem = &H40000000

        ''' <summary>
        ''' The specified folders have subfolders = and are, therefore, 
        ''' expandable in the left pane of Windows Explorer).
        ''' </summary>
        HasSubFolder = CInt(&H80000000I)

        ''' <summary>
        ''' This flag is a mask for the contents attributes.
        ''' </summary>
        ContentsMask = CInt(&H80000000I)

        ''' <summary>
        ''' When specified as input, SFGAO_VALIDATE instructs the folder to validate that the items 
        ''' pointed to by the contents of apidl exist. If one or more of those items do not exist, 
        ''' IShellFolder::GetAttributesOf returns a failure code. 
        ''' When used with the file system folder, SFGAO_VALIDATE instructs the folder to discard cached 
        ''' properties retrieved by clients of IShellFolder2::GetDetailsEx that may 
        ''' have accumulated for the specified items.
        ''' </summary>
        Validate = &H1000000

        ''' <summary>
        ''' The specified items are on removable media or are themselves removable devices.
        ''' </summary>
        Removable = &H2000000

        ''' <summary>
        ''' The specified items are compressed.
        ''' </summary>
        Compressed = &H4000000

        ''' <summary>
        ''' The specified items can be browsed in place.
        ''' </summary>
        Browsable = &H8000000

        ''' <summary>
        ''' The items are nonenumerated items.
        ''' </summary>
        Nonenumerated = &H100000

        ''' <summary>
        ''' The objects contain new content.
        ''' </summary>
        NewContent = &H200000

        ''' <summary>
        ''' It is possible to create monikers for the specified file objects or folders.
        ''' </summary>
        CanMoniker = &H400000

        ''' <summary>
        ''' Not supported.
        ''' </summary>
        HasStorage = &H400000

        ''' <summary>
        ''' Indicates that the item has a stream associated with it that can be accessed 
        ''' by a call to IShellFolder::BindToObject with IID_IStream in the riid parameter.
        ''' </summary>
        Stream = &H400000

        ''' <summary>
        ''' Children of this item are accessible through IStream or IStorage. 
        ''' Those children are flagged with SFGAO_STORAGE or SFGAO_STREAM.
        ''' </summary>
        StorageAncestor = &H800000

        ''' <summary>
        ''' This flag is a mask for the storage capability attributes.
        ''' </summary>
        StorageCapabilityMask = &H70C50008

        ''' <summary>
        ''' Mask used by PKEY_SFGAOFlags to remove certain values that are considered 
        ''' to cause slow calculations or lack context. 
        ''' Equal to SFGAO_VALIDATE | SFGAO_ISSLOW | SFGAO_HASSUBFOLDER.
        ''' </summary>
        PkeyMask = &H81044000I
    End Enum

    <Flags>
    Public Enum ShellFolderEnumerationOptions As UShort
        CheckingForChildren = &H10
        Folders = &H20
        NonFolders = &H40
        IncludeHidden = &H80
        InitializeOnFirstNext = &H100
        NetPrinterSearch = &H200
        Shareable = &H400
        Storage = &H800
        NavigationEnum = &H1000
        FastItems = &H2000
        FlatList = &H4000
        EnableAsync = &H8000
    End Enum

    Public Enum SICHINTF

        SICHINT_DISPLAY = &H0
        SICHINT_CANONICAL = &H10000000
        SICHINT_TEST_FILESYSPATH_IF_NOT_EQUAL = &H20000000
        SICHINT_ALLFIELDS = (&H80000000)
    End Enum


    ''' <summary>
    ''' Thumbnail Alpha Types
    ''' </summary>
    Public Enum ThumbnailAlphaType
        ''' <summary>
        ''' Let the system decide.
        ''' </summary>
        Unknown = 0

        ''' <summary>
        ''' No transparency
        ''' </summary>
        NoAlphaChannel = 1

        ''' <summary>
        ''' Has transparency
        ''' </summary>
        HasAlphaChannel = 2
    End Enum

    <Flags>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification:="Follows native api.")>
    Public Enum AccessModes
        ''' <summary>
        ''' Indicates that, in direct mode, each change to a storage 
        ''' or stream element is written as it occurs.
        ''' </summary>
        Direct = &H0

        ''' <summary>
        ''' Indicates that, in transacted mode, changes are buffered 
        ''' and written only if an explicit commit operation is called. 
        ''' </summary>
        Transacted = &H10000

        ''' <summary>
        ''' Provides a faster implementation of a compound file 
        ''' in a limited, but frequently used, case. 
        ''' </summary>
        Simple = &H8000000

        ''' <summary>
        ''' Indicates that the object is read-only, 
        ''' meaning that modifications cannot be made.
        ''' </summary>
        Read = &H0

        ''' <summary>
        ''' Enables you to save changes to the object, 
        ''' but does not permit access to its data. 
        ''' </summary>
        Write = &H1

        ''' <summary>
        ''' Enables access and modification of object data.
        ''' </summary>
        ReadWrite = &H2

        ''' <summary>
        ''' Specifies that subsequent openings of the object are 
        ''' not denied read or write access. 
        ''' </summary>
        ShareDenyNone = &H40

        ''' <summary>
        ''' Prevents others from subsequently opening the object in Read mode. 
        ''' </summary>
        ShareDenyRead = &H30

        ''' <summary>
        ''' Prevents others from subsequently opening the object 
        ''' for Write or ReadWrite access.
        ''' </summary>
        ShareDenyWrite = &H20

        ''' <summary>
        ''' Prevents others from subsequently opening the object in any mode. 
        ''' </summary>
        ShareExclusive = &H10

        ''' <summary>
        ''' Opens the storage object with exclusive access to the most 
        ''' recently committed version.
        ''' </summary>
        Priority = &H40000

        ''' <summary>
        ''' Indicates that the underlying file is to be automatically destroyed when the root 
        ''' storage object is released. This feature is most useful for creating temporary files. 
        ''' </summary>
        DeleteOnRelease = &H4000000

        ''' <summary>
        ''' Indicates that, in transacted mode, a temporary scratch file is usually used 
        ''' to save modifications until the Commit method is called. 
        ''' Specifying NoScratch permits the unused portion of the original file 
        ''' to be used as work space instead of creating a new file for that purpose. 
        ''' </summary>
        NoScratch = &H100000

        ''' <summary>
        ''' Indicates that an existing storage object 
        ''' or stream should be removed before the new object replaces it. 
        ''' </summary>
        Create = &H1000

        ''' <summary>
        ''' Creates the new object while preserving existing data in a stream named "Contents". 
        ''' </summary>
        Convert = &H20000

        ''' <summary>
        ''' Causes the create operation to fail if an existing object with the specified name exists.
        ''' </summary>
        FailIfThere = &H0

        ''' <summary>
        ''' This flag is used when opening a storage object with Transacted 
        ''' and without ShareExclusive or ShareDenyWrite. 
        ''' In this case, specifying NoSnapshot prevents the system-provided 
        ''' implementation from creating a snapshot copy of the file. 
        ''' Instead, changes to the file are written to the end of the file. 
        ''' </summary>
        NoSnapshot = &H200000

        ''' <summary>
        ''' Supports direct mode for single-writer, multireader file operations. 
        ''' </summary>
        DirectSingleWriterMultipleReader = &H400000
    End Enum

    ''' <summary>
    ''' Describes the event that has occurred. 
    ''' Typically, only one event is specified at a time. 
    ''' If more than one event is specified, 
    ''' the values contained in the dwItem1 and dwItem2 parameters must be the same, 
    ''' respectively, for all specified events. 
    ''' This parameter can be one or more of the following values:
    ''' </summary>
    <Flags>
    Public Enum ShellObjectChangeTypes
        ''' <summary>
        ''' None
        ''' </summary>
        None = 0

        ''' <summary>
        ''' The name of a nonfolder item has changed. 
        ''' SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. 
        ''' dwItem1 contains the previous PIDL or name of the item. 
        ''' dwItem2 contains the new PIDL or name of the item.
        ''' </summary>
        ItemRename = &H1

        ''' <summary>
        ''' A nonfolder item has been created. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. 
        ''' dwItem1 contains the item that was created.
        ''' dwItem2 is not used and should be NULL.
        ''' </summary>
        ItemCreate = &H2

        ''' <summary>
        ''' A nonfolder item has been deleted. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. 
        ''' dwItem1 contains the item that was deleted. 
        ''' dwItem2 is not used and should be NULL.
        ''' </summary>
        ItemDelete = &H4

        ''' <summary>
        ''' A folder has been created. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. 
        ''' dwItem1 contains the folder that was created. 
        ''' dwItem2 is not used and should be NULL.
        ''' </summary>
        DirectoryCreate = &H8

        ''' <summary>
        ''' A folder has been removed. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. 
        ''' dwItem1 contains the folder that was removed. 
        ''' dwItem2 is not used and should be NULL.
        ''' </summary>
        DirectoryDelete = &H10

        ''' <summary>
        ''' Storage media has been inserted into a drive. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. 
        ''' dwItem1 contains the root of the drive that contains the new media. 
        ''' dwItem2 is not used and should be NULL.
        ''' </summary>
        MediaInsert = &H20

        ''' <summary>
        ''' Storage media has been removed from a drive. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. 
        ''' dwItem1 contains the root of the drive from which the media was removed. 
        ''' dwItem2 is not used and should be NULL.
        ''' </summary>
        MediaRemove = &H40

        ''' <summary>
        ''' A drive has been removed. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. 
        ''' dwItem1 contains the root of the drive that was removed. 
        ''' dwItem2 is not used and should be NULL.
        ''' </summary>
        DriveRemove = &H80

        ''' <summary>
        ''' A drive has been added. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. 
        ''' dwItem1 contains the root of the drive that was added. 
        ''' dwItem2 is not used and should be NULL.
        ''' </summary>
        DriveAdd = &H100

        ''' <summary>
        ''' A folder on the local computer is being shared via the network. 
        ''' SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. 
        ''' dwItem1 contains the folder that is being shared. 
        ''' dwItem2 is not used and should be NULL.
        ''' </summary>
        NetShare = &H200

        ''' <summary>
        ''' A folder on the local computer is no longer being shared via the network. 
        ''' SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. 
        ''' dwItem1 contains the folder that is no longer being shared. 
        ''' dwItem2 is not used and should be NULL.
        ''' </summary>
        NetUnshare = &H400

        ''' <summary>
        ''' The attributes of an item or folder have changed. 
        ''' SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. 
        ''' dwItem1 contains the item or folder that has changed.
        ''' dwItem2 is not used and should be NULL.
        ''' </summary>
        AttributesChange = &H800

        ''' <summary>
        ''' The contents of an existing folder have changed, but the folder still exists and has not been renamed. 
        ''' SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. 
        ''' dwItem1 contains the folder that has changed. 
        ''' dwItem2 is not used and should be NULL. 
        ''' If a folder has been created, deleted, or renamed, use SHCNE_MKDIR, SHCNE_RMDIR, or SHCNE_RENAMEFOLDER, respectively.
        ''' </summary>
        DirectoryContentsUpdate = &H1000

        ''' <summary>
        ''' An existing item (a folder or a nonfolder) has changed, but the item still exists and has not been renamed. 
        ''' SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. 
        ''' dwItem1 contains the item that has changed. 
        ''' dwItem2 is not used and should be NULL. 
        ''' If a nonfolder item has been created, deleted, or renamed, 
        ''' use SHCNE_CREATE, SHCNE_DELETE, or SHCNE_RENAMEITEM, respectively, instead.
        ''' </summary>
        Update = &H2000

        ''' <summary>
        ''' The computer has disconnected from a server. 
        ''' SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. 
        ''' dwItem1 contains the server from which the computer was disconnected. 
        ''' dwItem2 is not used and should be NULL.
        ''' </summary>
        ServerDisconnect = &H4000

        ''' <summary>
        ''' An image in the system image list has changed. 
        ''' SHCNF_DWORD must be specified in uFlags.
        ''' dwItem1 is not used and should be NULL.
        ''' dwItem2 contains the index in the system image list that has changed.         
        ''' </summary> //verify this is not opposite?
        SystemImageUpdate = &H8000

        ''' <summary>
        ''' The name of a folder has changed. SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. 
        ''' dwItem1 contains the previous PIDL or name of the folder. 
        ''' dwItem2 contains the new PIDL or name of the folder.
        ''' </summary>
        DirectoryRename = &H20000

        ''' <summary>
        ''' The amount of free space on a drive has changed. 
        ''' SHCNF_IDLIST or SHCNF_PATH must be specified in uFlags. 
        ''' dwItem1 contains the root of the drive on which the free space changed. 
        ''' dwItem2 is not used and should be NULL.
        ''' </summary>
        FreeSpace = &H40000

        ''' <summary>
        ''' A file type association has changed. 
        ''' SHCNF_IDLIST must be specified in the uFlags parameter. 
        ''' dwItem1 and dwItem2 are not used and must be NULL.
        ''' </summary>
        AssociationChange = &H8000000

        ''' <summary>
        ''' Specifies a combination of all of the disk event identifiers.
        ''' </summary>
        DiskEventsMask = &H2381F

        ''' <summary>
        ''' Specifies a combination of all of the global event identifiers.
        ''' </summary>
        GlobalEventsMask = &HC0581E0

        ''' <summary>
        ''' All events have occurred.
        ''' </summary>
        AllEventsMask = &H7FFFFFFF

        ''' <summary>
        ''' The specified event occurred as a result of a system interrupt. 
        ''' As this value modifies other event values, it cannot be used alone.
        ''' </summary>
        FromInterrupt = CInt(&H80000000I)
    End Enum

#End Region

#Region "Shell Structures"

    ''' <summary>
    ''' Stores information about how to sort a column that is displayed in the folder view.
    ''' </summary>    
    <StructLayout(LayoutKind.Sequential)>
    Public Structure SortColumn

        ''' <summary>
        ''' Creates a sort column with the specified direction for the given property.
        ''' </summary>
        ''' <param name="propertyKey">Property key for the property that the user will sort.</param>
        ''' <param name="direction">The direction in which the items are sorted.</param>
        Public Sub New(propertyKey As PropertyKey, direction As SortDirection)
            Me.m_propertyKey = propertyKey
            Me.m_direction = direction
        End Sub

        ''' <summary>
        ''' The ID of the column by which the user will sort. A PropertyKey structure. 
        ''' For example, for the "Name" column, the property key is PKEY_ItemNameDisplay or
        ''' PropertySystem.SystemProperties.System.ItemName.
        ''' </summary>                
        Public Property PropertyKey() As PropertyKey
            Get
                Return m_propertyKey
            End Get
            Set(value As PropertyKey)
                m_propertyKey = value
            End Set
        End Property
        Private m_propertyKey As PropertyKey

        ''' <summary>
        ''' The direction in which the items are sorted.
        ''' </summary>                        
        Public Property Direction() As SortDirection
            Get
                Return m_direction
            End Get
            Set(value As SortDirection)
                m_direction = value
            End Set
        End Property
        Private m_direction As SortDirection


        ''' <summary>
        ''' Implements the == (equality) operator.
        ''' </summary>
        ''' <param name="col1">First object to compare.</param>
        ''' <param name="col2">Second object to compare.</param>
        ''' <returns>True if col1 equals col2; false otherwise.</returns>
        Public Shared Operator =(col1 As SortColumn, col2 As SortColumn) As Boolean
            Return (col1.Direction = col2.Direction) AndAlso (col1.PropertyKey = col2.PropertyKey)
        End Operator

        ''' <summary>
        ''' Implements the != (unequality) operator.
        ''' </summary>
        ''' <param name="col1">First object to compare.</param>
        ''' <param name="col2">Second object to compare.</param>
        ''' <returns>True if col1 does not equals col1; false otherwise.</returns>
        Public Shared Operator <>(col1 As SortColumn, col2 As SortColumn) As Boolean
            Return Not (col1 = col2)
        End Operator

        ''' <summary>
        ''' Determines if this object is equal to another.
        ''' </summary>
        ''' <param name="obj">The object to compare</param>
        ''' <returns>Returns true if the objects are equal; false otherwise.</returns>
        Public Overrides Function Equals(obj As Object) As Boolean
            If obj Is Nothing OrElse obj.[GetType]() IsNot GetType(SortColumn) Then
                Return False
            End If
            Return (Me = CType(obj, SortColumn))
        End Function

        ''' <summary>
        ''' Generates a nearly unique hashcode for this structure.
        ''' </summary>
        ''' <returns>A hash code.</returns>
        Public Overrides Function GetHashCode() As Integer
            Dim hash As Integer = Me.m_direction.GetHashCode()
            hash = hash * 31 + Me.m_propertyKey.GetHashCode()
            Return hash
        End Function

    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
    Public Structure ThumbnailId
        <MarshalAs(UnmanagedType.LPArray, SizeParamIndex:=16)>
        Private rgbKey As Byte
    End Structure

    ' Summary:
    '     Defines a unique key for a Shell Property
    Public Structure PropertyKey
        Implements IEquatable(Of PropertyKey)
        '
        ' Summary:
        '     PropertyKey Constructor
        '
        ' Parameters:
        '   formatId:
        '     A unique GUID for the property
        '
        '   propertyId:
        '     Property identifier (PID)
        Public Sub New(formatId As Guid, propertyId As Integer)
            _FormatId = formatId
            _PropertyId = propertyId
        End Sub
        '
        ' Summary:
        '     PropertyKey Constructor
        '
        ' Parameters:
        '   formatId:
        '     A string represenstion of a GUID for the property
        '
        '   propertyId:
        '     Property identifier (PID)
        Public Sub New(formatId As String, propertyId As Integer)
            _FormatId = New Guid(formatId)
            _PropertyId = propertyId
        End Sub

        ' Summary:
        '     Implements the != (inequality) operator.
        '
        ' Parameters:
        '   propKey1:
        '     First property key to compare
        '
        '   propKey2:
        '     Second property key to compare.
        '
        ' Returns:
        '     true if object a does not equal object b. false otherwise.
        Public Shared Operator <>(propKey1 As PropertyKey, propKey2 As PropertyKey) As Boolean
            Return Not propKey1.Equals(propKey2)
        End Operator
        '
        ' Summary:
        '     Implements the == (equality) operator.
        '
        ' Parameters:
        '   propKey1:
        '     First property key to compare.
        '
        '   propKey2:
        '     Second property key to compare.
        '
        ' Returns:
        '     true if object a equals object b. false otherwise.
        Public Shared Operator =(propKey1 As PropertyKey, propKey2 As PropertyKey) As Boolean
            Return propKey1.Equals(propKey2)
        End Operator

        ' Summary:
        '     A unique GUID for the property
        Private _FormatId As Guid
        Public ReadOnly Property FormatId() As Guid
            Get
                Return _FormatId
            End Get
        End Property
        '
        ' Summary:
        '     Property identifier (PID)
        Private _PropertyId As Integer
        Public ReadOnly Property PropertyId() As Integer
            Get
                Return _PropertyId
            End Get
        End Property

        ' Summary:
        '     Returns whether this object is equal to another. This is vital for performance
        '     of value types.
        '
        ' Parameters:
        '   obj:
        '     The object to compare against.
        '
        ' Returns:
        '     Equality result.
        Public Overloads Overrides Function Equals(obj As Object) As Boolean
            Return Equals(CType(obj, PropertyKey))
        End Function
        '
        ' Summary:
        '     Returns whether this object is equal to another. This is vital for performance
        '     of value types.
        '
        ' Parameters:
        '   other:
        '     The object to compare against.
        '
        ' Returns:
        '     Equality result.
        Public Overloads Function Equals(other As PropertyKey) As Boolean Implements IEquatable(Of PropertyKey).Equals
            With other
                If .FormatId <> _FormatId Then Return False
                If .PropertyId <> _PropertyId Then Return False
            End With

            Return True
        End Function
        '
        ' Summary:
        '     Returns the hash code of the object. This is vital for performance of value
        '     types.
        Public Overrides Function GetHashCode() As Integer
            Dim i As Integer
            Dim b() As Byte = _FormatId.ToByteArray

            For Each by In b
                i += by
            Next

            i += _PropertyId
            Return i
        End Function
        '
        ' Summary:
        '     Override ToString() to provide a user friendly string representation
        '
        ' Returns:
        '     String representing the property key
        Public Overrides Function ToString() As String
            Return _FormatId.ToString("B").ToUpper & "[" & _PropertyId & "]"
        End Function
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure SHITEMID
        Public cb As UShort
        Public ItemId As Guid
    End Structure

#End Region

#Region "Shell COM Interfaces"

    <ComImport, Guid(ShellIIDGuid.IModalWindow), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IModalWindow
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), PreserveSig>
        Function Show(<[In]> parent As IntPtr) As Integer
    End Interface

    <ComImport, Guid(ShellIIDGuid.IShellItem), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IShellItem
        ' Not supported: IBindCtx.
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function BindToHandler(<[In]> pbc As IntPtr, <[In]> ByRef bhid As Guid, <[In]> ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppv As Object) As HResult

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetParent(<MarshalAs(UnmanagedType.[Interface])> ByRef ppsi As IShellItem)

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetDisplayName(<[In]> sigdnName As ShellItemDesignNameOptions, ByRef ppszName As IntPtr) As HResult

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetAttributes(<[In]> sfgaoMask As ShellFileGetAttributesOptions, ByRef psfgaoAttribs As ShellFileGetAttributesOptions)

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function Compare(<[In], MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, <[In]> hint As SICHINTF, ByRef piOrder As Integer) As HResult
    End Interface

    <ComImport, Guid(ShellIIDGuid.IShellItem2), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IShellItem2
        Inherits IShellItem
        ' Not supported: IBindCtx.
        '<PreserveSig> _
        '<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        'Function BindToHandler(<[In]> pbc As IntPtr, <[In]> ByRef bhid As Guid, <[In]> ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IShellFolder) As HResult

        '<PreserveSig> _
        '<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        'Function GetParent(<MarshalAs(UnmanagedType.[Interface])> ByRef ppsi As IShellItem) As HResult

        '<PreserveSig> _
        '<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        'Function GetDisplayName(<[In]> sigdnName As ShellItemDesignNameOptions, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppszName As String) As HResult

        '<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        'Sub GetAttributes(<[In]> sfgaoMask As ShellFileGetAttributesOptions, ByRef psfgaoAttribs As ShellFileGetAttributesOptions)

        '<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        'Sub Compare(<[In], MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, <[In]> hint As UInteger, ByRef piOrder As Integer)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), PreserveSig>
        Function GetPropertyStore(<[In]> Flags As GetPropertyStoreOptions, <[In]> ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IPropertyStore) As Integer

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetPropertyStoreWithCreateObject(<[In]> Flags As GetPropertyStoreOptions, <[In], MarshalAs(UnmanagedType.IUnknown)> punkCreateObject As Object, <[In]> ByRef riid As Guid, ByRef ppv As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetPropertyStoreForKeys(<[In]> ByRef rgKeys As PropertyKey, <[In]> cKeys As UInteger, <[In]> Flags As GetPropertyStoreOptions, <[In]> ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.IUnknown)> ByRef ppv As IPropertyStore)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetPropertyDescriptionList(<[In]> ByRef keyType As PropertyKey, <[In]> ByRef riid As Guid, ByRef ppv As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function Update(<[In], MarshalAs(UnmanagedType.[Interface])> pbc As IBindCtx) As HResult

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetProperty(<[In]> ByRef key As PropertyKey, <Out> ppropvar As PropVariant)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetCLSID(<[In]> ByRef key As PropertyKey, ByRef pclsid As Guid)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetFileTime(<[In]> ByRef key As PropertyKey, ByRef pft As System.Runtime.InteropServices.ComTypes.FILETIME)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetInt32(<[In]> ByRef key As PropertyKey, ByRef pi As Integer)

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetString(<[In]> ByRef key As PropertyKey, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppsz As String) As HResult

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetUInt32(<[In]> ByRef key As PropertyKey, ByRef pui As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetUInt64(<[In]> ByRef key As PropertyKey, ByRef pull As ULong)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetBool(<[In]> ByRef key As PropertyKey, ByRef pf As Integer)
    End Interface

    <ComImport, Guid(ShellIIDGuid.IShellItemArray), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IShellItemArray
        ' Not supported: IBindCtx.
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function BindToHandler(<[In], MarshalAs(UnmanagedType.[Interface])> pbc As IntPtr, <[In]> ByRef rbhid As Guid, <[In]> ByRef riid As Guid, ByRef ppvOut As IntPtr) As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetPropertyStore(<[In]> Flags As Integer, <[In]> ByRef riid As Guid, ByRef ppv As IntPtr) As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetPropertyDescriptionList(<[In]> ByRef keyType As PropertyKey, <[In]> ByRef riid As Guid, ByRef ppv As IntPtr) As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetAttributes(<[In]> dwAttribFlags As ShellItemAttributeOptions, <[In]> sfgaoMask As ShellFileGetAttributesOptions, ByRef psfgaoAttribs As ShellFileGetAttributesOptions) As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetCount(ByRef pdwNumItems As UInteger) As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetItemAt(<[In]> dwIndex As UInteger, <MarshalAs(UnmanagedType.[Interface])> ByRef ppsi As IShellItem) As HResult

        ' Not supported: IEnumShellItems (will use GetCount and GetItemAt instead).
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function EnumItems(<MarshalAs(UnmanagedType.[Interface])> ByRef ppenumShellItems As IntPtr) As HResult
    End Interface

    <ComImport, Guid(ShellIIDGuid.IShellLibrary), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IShellLibrary
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function LoadLibraryFromItem(<[In], MarshalAs(UnmanagedType.[Interface])> library As IShellItem, <[In]> grfMode As AccessModes) As HResult

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub LoadLibraryFromKnownFolder(<[In]> ByRef knownfidLibrary As Guid, <[In]> grfMode As AccessModes)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub AddFolder(<[In], MarshalAs(UnmanagedType.[Interface])> location As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub RemoveFolder(<[In], MarshalAs(UnmanagedType.[Interface])> location As IShellItem)

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetFolders(<[In]> lff As LibraryFolderFilter, <[In]> ByRef riid As Guid, <MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IShellItemArray) As HResult

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub ResolveFolder(<[In], MarshalAs(UnmanagedType.[Interface])> folderToResolve As IShellItem, <[In]> timeout As UInteger, <[In]> ByRef riid As Guid, <MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetDefaultSaveFolder(<[In]> dsft As DefaultSaveFolderType, <[In]> ByRef riid As Guid, <MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub SetDefaultSaveFolder(<[In]> dsft As DefaultSaveFolderType, <[In], MarshalAs(UnmanagedType.[Interface])> si As IShellItem)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetOptions(ByRef lofOptions As LibraryOptions)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub SetOptions(<[In]> lofMask As LibraryOptions, <[In]> lofOptions As LibraryOptions)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetFolderType(ByRef ftid As Guid)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub SetFolderType(<[In]> ByRef ftid As Guid)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetIcon(<MarshalAs(UnmanagedType.LPWStr)> ByRef icon As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub SetIcon(<[In], MarshalAs(UnmanagedType.LPWStr)> icon As String)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub Commit()

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub Save(<[In], MarshalAs(UnmanagedType.[Interface])> folderToSaveIn As IShellItem, <[In], MarshalAs(UnmanagedType.LPWStr)> libraryName As String, <[In]> lsf As LibrarySaveOptions, <MarshalAs(UnmanagedType.[Interface])> ByRef savedTo As IShellItem2)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub SaveInKnownFolder(<[In]> ByRef kfidToSaveIn As Guid, <[In], MarshalAs(UnmanagedType.LPWStr)> libraryName As String, <[In]> lsf As LibrarySaveOptions, <MarshalAs(UnmanagedType.[Interface])> ByRef savedTo As IShellItem2)
    End Interface

    <ComImportAttribute>
    <GuidAttribute("bcc18b79-ba16-442f-80c4-8a59c30c463b")>
    <InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)>
    Interface IShellItemImageFactory
        <PreserveSig>
        Function GetImage(<[In], MarshalAs(UnmanagedType.Struct)> size As System.Drawing.Size, <[In]> flags As SIIGBF, <Out> ByRef phbm As IntPtr) As HResult
    End Interface

    <ComImport, Guid(ShellIIDGuid.IThumbnailCache), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Interface IThumbnailCache
        Sub GetThumbnail(<[In]> pShellItem As IShellItem, <[In]> cxyRequestedThumbSize As UInteger, <[In]> flags As ThumbnailOptions, <Out> ByRef ppvThumb As ISharedBitmap, <Out> ByRef pOutFlags As ThumbnailCacheOptions, <Out> pThumbnailID As ThumbnailId)

        Sub GetThumbnailByID(<[In]> thumbnailID As ThumbnailId, <[In]> cxyRequestedThumbSize As UInteger, <Out> ByRef ppvThumb As ISharedBitmap, <Out> ByRef pOutFlags As ThumbnailCacheOptions)
    End Interface

    <ComImport, Guid(ShellIIDGuid.ISharedBitmap), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Interface ISharedBitmap
        Sub GetSharedBitmap(<Out> ByRef phbm As IntPtr)
        Sub GetSize(<Out> ByRef pSize As System.Drawing.Size)
        Sub GetFormat(<Out> ByRef pat As ThumbnailAlphaType)
        Sub InitializeBitmap(<[In]> hbm As IntPtr, <[In]> wtsAT As ThumbnailAlphaType)
        Sub Detach(<Out> ByRef phbm As IntPtr)
    End Interface


    <ComImport, Guid(ShellIIDGuid.IShellFolder), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComConversionLoss>
    Public Interface IShellFolder
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub ParseDisplayName(hwnd As IntPtr, <[In], MarshalAs(UnmanagedType.[Interface])> ByRef pbc As IBindCtx, <[In], MarshalAs(UnmanagedType.LPWStr)> pszDisplayName As String, <[In], Out> ByRef pchEaten As UInteger, <Out> ByRef ppidl As IntPtr, <[In], Out> ByRef pdwAttributes As UInteger)
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function EnumObjects(<[In]> hwnd As IntPtr, <[In]> grfFlags As ShellFolderEnumerationOptions, <MarshalAs(UnmanagedType.[Interface])> ByRef ppenumIDList As IEnumIDList) As HResult

        '[In, MarshalAs(UnmanagedType.Interface)] IBindCtx
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function BindToObject(<[In]> pidl As IntPtr, pbc As IntPtr, <[In]> ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IShellFolder) As HResult

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub BindToStorage(<[In]> ByRef pidl As IntPtr, <[In], MarshalAs(UnmanagedType.[Interface])> pbc As IBindCtx, <[In]> ByRef riid As Guid, ByRef ppv As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub CompareIDs(<[In]> lParam As IntPtr, <[In]> ByRef pidl1 As IntPtr, <[In]> ByRef pidl2 As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub CreateViewObject(<[In]> hwndOwner As IntPtr, <[In]> ByRef riid As Guid, ByRef ppv As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetAttributesOf(<[In]> cidl As UInteger, <[In]> ByRef apidl As IntPtr, <[In], Out> ByRef rgfInOut As Integer)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetUIObjectOf(<[In]> hwndOwner As IntPtr, <[In]> cidl As UInteger, <[In]> apidl As IntPtr, <[In]> ByRef riid As Guid, <[In], Out> ByRef rgfReserved As UInteger, ByRef ppv As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetDisplayNameOf(<[In]> pidl As IntPtr, <[In]> uFlags As ShellItemDesignNameOptions, pName As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub SetNameOf(<[In]> hwnd As IntPtr, <[In]> ByRef pidl As IntPtr, <[In], MarshalAs(UnmanagedType.LPWStr)> pszName As String, <[In]> uFlags As UInteger, <Out> ppidlOut As IntPtr)
    End Interface

    <ComImport, Guid(ShellIIDGuid.IShellFolder2), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComConversionLoss>
    Public Interface IShellFolder2
        Inherits IShellFolder
        '<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        'Sub ParseDisplayName(<[In]> hwnd As IntPtr, <[In], MarshalAs(UnmanagedType.[Interface])> pbc As IBindCtx, <[In], MarshalAs(UnmanagedType.LPWStr)> pszDisplayName As String, <[In], Out> ByRef pchEaten As UInteger, <Out> ppidl As IntPtr, <[In], Out> ByRef pdwAttributes As UInteger)

        '<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        'Sub EnumObjects(<[In]> hwnd As IntPtr, <[In]> grfFlags As ShellFolderEnumerationOptions, <MarshalAs(UnmanagedType.[Interface])> ByRef ppenumIDList As IEnumIDList)

        ''[In, MarshalAs(UnmanagedType.Interface)] IBindCtx
        '<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        'Sub BindToObject(<[In]> pidl As IntPtr, pbc As IntPtr, <[In]> ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IShellFolder)

        '<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        'Sub BindToStorage(<[In]> ByRef pidl As IntPtr, <[In], MarshalAs(UnmanagedType.[Interface])> pbc As IBindCtx, <[In]> ByRef riid As Guid, ByRef ppv As IntPtr)

        '<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        'Sub CompareIDs(<[In]> lParam As IntPtr, <[In]> ByRef pidl1 As IntPtr, <[In]> ByRef pidl2 As IntPtr)

        '<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        'Sub CreateViewObject(<[In]> hwndOwner As IntPtr, <[In]> ByRef riid As Guid, ByRef ppv As IntPtr)

        '<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        'Sub GetAttributesOf(<[In]> cidl As UInteger, <[In]> apidl As IntPtr, <[In], Out> ByRef rgfInOut As UInteger)

        '<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        'Sub GetUIObjectOf(<[In]> hwndOwner As IntPtr, <[In]> cidl As UInteger, <[In]> apidl As IntPtr, <[In]> ByRef riid As Guid, <[In], Out> ByRef rgfReserved As UInteger, ByRef ppv As IntPtr)

        '<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        'Sub GetDisplayNameOf(<[In]> ByRef pidl As IntPtr, <[In]> uFlags As UInteger, ByRef pName As IntPtr)

        '<MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)> _
        'Sub SetNameOf(<[In]> hwnd As IntPtr, <[In]> ByRef pidl As IntPtr, <[In], MarshalAs(UnmanagedType.LPWStr)> pszName As String, <[In]> uFlags As UInteger, <Out> ppidlOut As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetDefaultSearchGUID(ByRef pguid As Guid)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub EnumSearches(<Out> ByRef ppenum As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetDefaultColumn(<[In]> dwRes As UInteger, ByRef pSort As UInteger, ByRef pDisplay As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetDefaultColumnState(<[In]> iColumn As UInteger, ByRef pcsFlags As UInteger)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetDetailsEx(<[In]> ByRef pidl As IntPtr, <[In]> ByRef pscid As PropertyKey, <MarshalAs(UnmanagedType.Struct)> ByRef pv As Object)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetDetailsOf(<[In]> ByRef pidl As IntPtr, <[In]> iColumn As UInteger, ByRef psd As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub MapColumnToSCID(<[In]> iColumn As UInteger, ByRef pscid As PropertyKey)
    End Interface

    <ComImport, Guid(ShellIIDGuid.IEnumIDList), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IEnumIDList
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function [Next](celt As UInteger, ByRef rgelt As IntPtr, ByRef pceltFetched As UInteger) As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function Skip(<[In]> celt As UInteger) As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function Reset() As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function Clone(<MarshalAs(UnmanagedType.[Interface])> ByRef ppenum As IEnumIDList) As HResult
    End Interface

    <ComImport, Guid(ShellIIDGuid.IShellLinkW), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IShellLinkW
        'ref _WIN32_FIND_DATAW pfd,
        Sub GetPath(<Out, MarshalAs(UnmanagedType.LPWStr)> pszFile As StringBuilder, cchMaxPath As Integer, pfd As IntPtr, fFlags As UInteger)
        Sub GetIDList(ByRef ppidl As IntPtr)
        Sub SetIDList(pidl As IntPtr)
        Sub GetDescription(<Out, MarshalAs(UnmanagedType.LPWStr)> pszFile As StringBuilder, cchMaxName As Integer)
        Sub SetDescription(<MarshalAs(UnmanagedType.LPWStr)> pszName As String)
        Sub GetWorkingDirectory(<Out, MarshalAs(UnmanagedType.LPWStr)> pszDir As StringBuilder, cchMaxPath As Integer)
        Sub SetWorkingDirectory(<MarshalAs(UnmanagedType.LPWStr)> pszDir As String)
        Sub GetArguments(<Out, MarshalAs(UnmanagedType.LPWStr)> pszArgs As StringBuilder, cchMaxPath As Integer)
        Sub SetArguments(<MarshalAs(UnmanagedType.LPWStr)> pszArgs As String)
        Sub GetHotKey(ByRef wHotKey As Short)
        Sub SetHotKey(wHotKey As Short)
        Sub GetShowCmd(ByRef iShowCmd As UInteger)
        Sub SetShowCmd(iShowCmd As UInteger)
        Sub GetIconLocation(<Out, MarshalAs(UnmanagedType.LPWStr)> ByRef pszIconPath As StringBuilder, cchIconPath As Integer, ByRef iIcon As Integer)
        Sub SetIconLocation(<MarshalAs(UnmanagedType.LPWStr)> pszIconPath As String, iIcon As Integer)
        Sub SetRelativePath(<MarshalAs(UnmanagedType.LPWStr)> pszPathRel As String, dwReserved As UInteger)
        Sub Resolve(hwnd As IntPtr, fFlags As UInteger)
        Sub SetPath(<MarshalAs(UnmanagedType.LPWStr)> pszFile As String)
    End Interface

    <ComImport, Guid(ShellIIDGuid.CShellLink), ClassInterface(ClassInterfaceType.None)>
    Public Class CShellLink
    End Class

    ' Summary:
    '     Provides the managed definition of the IPersistStream interface, with functionality
    '     from IPersist.
    <ComImport>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid("00000109-0000-0000-C000-000000000046")>
    Public Interface IPersistStream
        ' Summary:
        '     Retrieves the class identifier (CLSID) of an object.
        '
        ' Parameters:
        '   pClassID:
        '     When this method returns, contains a reference to the CLSID. This parameter
        '     is passed uninitialized.
        <PreserveSig>
        Sub GetClassID(ByRef pClassID As Guid)
        '
        ' Summary:
        '     Checks an object for changes since it was last saved to its current file.
        '
        ' Returns:
        '     S_OK if the file has changed since it was last saved; S_FALSE if the file
        '     has not changed since it was last saved.
        <PreserveSig>
        Function IsDirty() As HResult

        <PreserveSig>
        Function Load(<[In], MarshalAs(UnmanagedType.[Interface])> stm As IStream) As HResult

        <PreserveSig>
        Function Save(<[In], MarshalAs(UnmanagedType.[Interface])> stm As IStream, fRemember As Boolean) As HResult

        <PreserveSig>
        Function GetSizeMax(ByRef cbSize As ULong) As HResult
    End Interface

    <ComImport, Guid(ShellIIDGuid.ICondition), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface ICondition
        Inherits IPersistStream
        ' Summary:
        '     Retrieves the class identifier (CLSID) of an object.
        '
        ' Parameters:
        '   pClassID:
        '     When this method returns, contains a reference to the CLSID. This parameter
        '     is passed uninitialized.
        '
        ' Summary:
        '     Checks an object for changes since it was last saved to its current file.
        '
        ' Returns:
        '     S_OK if the file has changed since it was last saved; S_FALSE if the file
        '     has not changed since it was last saved.

        ' For any node, return what kind of node it is.
        <PreserveSig>
        Function GetConditionType(<Out> ByRef pNodeType As SearchConditionType) As HResult

        ' riid must be IID_IEnumUnknown, IID_IEnumVARIANT or IID_IObjectArray, or in the case of a negation node IID_ICondition.
        ' If this is a leaf node, E_FAIL will be returned.
        ' If this is a negation node, then if riid is IID_ICondition, *ppv will be set to a single ICondition, otherwise an enumeration of one.
        ' If this is a conjunction or a disjunction, *ppv will be set to an enumeration of the subconditions.
        <PreserveSig>
        Function GetSubConditions(<[In]> ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppv As Object) As HResult

        ' If this is not a leaf node, E_FAIL will be returned.
        ' Retrieve the property name, operation and value from the leaf node.
        ' Any one of ppszPropertyName, pcop and ppropvar may be NULL.
        <PreserveSig>
        Function GetComparisonInfo(<Out, MarshalAs(UnmanagedType.LPWStr)> ByRef ppszPropertyName As String, <Out> ByRef pcop As SearchConditionOperation, <Out> ppropvar As PropVariant) As HResult

        ' If this is not a leaf node, E_FAIL will be returned.
        ' *ppszValueTypeName will be set to the semantic type of the value, or to NULL if this is not meaningful.
        <PreserveSig>
        Function GetValueType(<Out, MarshalAs(UnmanagedType.LPWStr)> ByRef ppszValueTypeName As String) As HResult

        ' If this is not a leaf node, E_FAIL will be returned.
        ' If the value of the leaf node is VT_EMPTY, *ppszNormalization will be set to an empty string.
        ' If the value is a string (VT_LPWSTR, VT_BSTR or VT_LPSTR), then *ppszNormalization will be set to a
        ' character-normalized form of the value.
        ' Otherwise, *ppszNormalization will be set to some (character-normalized) string representation of the value.
        <PreserveSig>
        Function GetValueNormalization(<Out, MarshalAs(UnmanagedType.LPWStr)> ByRef ppszNormalization As String) As HResult

        ' Return information about what parts of the input produced the property, the operation and the value.
        ' Any one of ppPropertyTerm, ppOperationTerm and ppValueTerm may be NULL.
        ' For a leaf node returned by the parser, the position information of each IRichChunk identifies the tokens that
        ' contributed the property/operation/value, the string value is the corresponding part of the input string, and
        ' the PROPVARIANT is VT_EMPTY.
        <PreserveSig>
        Function GetInputTerms(<Out> ByRef ppPropertyTerm As IRichChunk, <Out> ByRef ppOperationTerm As IRichChunk, <Out> ByRef ppValueTerm As IRichChunk) As HResult

        ' Make a deep copy of this ICondition.
        <PreserveSig>
        Function Clone(<Out> ByRef ppc As ICondition) As HResult
    End Interface

    <ComImport, Guid(ShellIIDGuid.IRichChunk), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IRichChunk
        ' The position *pFirstPos is zero-based.
        ' Any one of pFirstPos, pLength, ppsz and pValue may be NULL.
        '[out, annotation("__out_opt")] ULONG* pFirstPos, [out, annotation("__out_opt")] ULONG* pLength, [out, annotation("__deref_opt_out_opt")] LPWSTR* ppsz, [out, annotation("__out_opt")] PROPVARIANT* pValue
        <PreserveSig>
        Function GetData() As HResult
    End Interface

    <ComImport>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid(ShellIIDGuid.IEnumUnknown)>
    Public Interface IEnumUnknown
        <PreserveSig>
        Function [Next](requestedNumber As UInt32, ByRef buffer As IntPtr, ByRef fetchedNumber As UInt32) As HResult
        <PreserveSig>
        Function Skip(number As UInt32) As HResult
        <PreserveSig>
        Function Reset() As HResult
        <PreserveSig>
        Function Clone(ByRef result As IEnumUnknown) As HResult
    End Interface


    <ComImport, Guid(ShellIIDGuid.IConditionFactory), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IConditionFactory
        <PreserveSig>
        Function MakeNot(<[In]> pcSub As ICondition, <[In]> fSimplify As Boolean, <Out> ByRef ppcResult As ICondition) As HResult

        <PreserveSig>
        Function MakeAndOr(<[In]> ct As SearchConditionType, <[In]> peuSubs As IEnumUnknown, <[In]> fSimplify As Boolean, <Out> ByRef ppcResult As ICondition) As HResult

        <PreserveSig>
        Function MakeLeaf(<[In], MarshalAs(UnmanagedType.LPWStr)> pszPropertyName As String, <[In]> cop As SearchConditionOperation, <[In], MarshalAs(UnmanagedType.LPWStr)> pszValueType As String, <[In]> ppropvar As PropVariant, richChunk1 As IRichChunk, richChunk2 As IRichChunk,
    richChunk3 As IRichChunk, <[In]> fExpand As Boolean, <Out> ByRef ppcResult As ICondition) As HResult

        '[In] ICondition pc, [In] STRUCTURED_QUERY_RESOLVE_OPTION sqro, [In] ref SYSTEMTIME pstReferenceTime, [Out] out ICondition ppcResolved
        <PreserveSig>
        Function Resolve() As HResult

    End Interface

    <ComImport, Guid(ShellIIDGuid.IConditionFactory), CoClass(GetType(ConditionFactoryCoClass))>
    Public Interface INativeConditionFactory
        Inherits IConditionFactory
    End Interface

    <ComImport, ClassInterface(ClassInterfaceType.None), TypeLibType(TypeLibTypeFlags.FCanCreate), Guid(ShellCLSIDGuid.ConditionFactory)>
    Public Class ConditionFactoryCoClass
    End Class

    <ComImport, Guid(ShellIIDGuid.ISearchFolderItemFactory), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface ISearchFolderItemFactory
        <PreserveSig>
        Function SetDisplayName(<[In], MarshalAs(UnmanagedType.LPWStr)> pszDisplayName As String) As HResult

        <PreserveSig>
        Function SetFolderTypeID(<[In]> ftid As Guid) As HResult

        <PreserveSig>
        Function SetFolderLogicalViewMode(<[In]> flvm As FolderLogicalViewMode) As HResult

        <PreserveSig>
        Function SetIconSize(<[In]> iIconSize As Integer) As HResult

        <PreserveSig>
        Function SetVisibleColumns(<[In]> cVisibleColumns As UInteger, <[In], MarshalAs(UnmanagedType.LPArray)> rgKey As PropertyKey()) As HResult

        <PreserveSig>
        Function SetSortColumns(<[In]> cSortColumns As UInteger, <[In], MarshalAs(UnmanagedType.LPArray)> rgSortColumns As SortColumn()) As HResult

        <PreserveSig>
        Function SetGroupColumn(<[In]> ByRef keyGroup As PropertyKey) As HResult

        <PreserveSig>
        Function SetStacks(<[In]> cStackKeys As UInteger, <[In], MarshalAs(UnmanagedType.LPArray)> rgStackKeys As PropertyKey()) As HResult

        <PreserveSig>
        Function SetScope(<[In], MarshalAs(UnmanagedType.[Interface])> ppv As IShellItemArray) As HResult

        <PreserveSig>
        Function SetCondition(<[In]> pCondition As ICondition) As HResult

        <PreserveSig>
        Function GetShellItem(ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IShellItem) As Integer

        <PreserveSig>
        Function GetIDList(<Out> ppidl As IntPtr) As HResult
    End Interface

    <ComImport, Guid(ShellIIDGuid.ISearchFolderItemFactory), CoClass(GetType(SearchFolderItemFactoryCoClass))>
    Public Interface INativeSearchFolderItemFactory
        Inherits ISearchFolderItemFactory
    End Interface

    <ComImport, ClassInterface(ClassInterfaceType.None), TypeLibType(TypeLibTypeFlags.FCanCreate), Guid(ShellCLSIDGuid.SearchFolderItemFactory)>
    Public Class SearchFolderItemFactoryCoClass
    End Class

    <ComImport, Guid(ShellIIDGuid.IQuerySolution), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Interface IQuerySolution
        Inherits IConditionFactory

        ' Retrieve the condition tree and the "main type" of the solution.
        ' ppQueryNode and ppMainType may be NULL.
        <PreserveSig>
        Function GetQuery(<Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppQueryNode As ICondition, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppMainType As IEntity) As HResult

        ' Identify parts of the input string not accounted for.
        ' Each parse error is represented by an IRichChunk where the position information
        ' reflect token counts, the string is NULL and the value is a VT_I4
        ' where lVal is from the ParseErrorType enumeration. The valid
        ' values for riid are IID_IEnumUnknown and IID_IEnumVARIANT.
        ' void** 
        <PreserveSig>
        Function GetErrors(<[In]> ByRef riid As Guid, <Out> ByRef ppParseErrors As IntPtr) As HResult

        ' Report the query string, how it was tokenized and what LCID and word breaker were used (for recognizing keywords).
        ' ppszInputString, ppTokens, pLocale and ppWordBreaker may be NULL.
        ' ITokenCollection** 
        ' IUnknown** 
        <PreserveSig>
        Function GetLexicalData(<MarshalAs(UnmanagedType.LPWStr)> ByRef ppszInputString As String, <Out> ByRef ppTokens As IntPtr, <Out> ByRef plcid As UInteger, <Out> ByRef ppWordBreaker As IntPtr) As HResult
    End Interface

    <ComImport, Guid(ShellIIDGuid.IQueryParser), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IQueryParser
        ' Parse parses an input string, producing a query solution.
        ' pCustomProperties should be an enumeration of IRichChunk objects, one for each custom property
        ' the application has recognized. pCustomProperties may be NULL, equivalent to an empty enumeration.
        ' For each IRichChunk, the position information identifies the character span of the custom property,
        ' the string value should be the name of an actual property, and the PROPVARIANT is completely ignored.
        <PreserveSig>
        Function Parse(<[In], MarshalAs(UnmanagedType.LPWStr)> pszInputString As String, <[In]> pCustomProperties As IEnumUnknown, <Out> ByRef ppSolution As IQuerySolution) As HResult

        ' Set a single option. See STRUCTURED_QUERY_SINGLE_OPTION above.
        <PreserveSig>
        Function SetOption(<[In]> [option] As StructuredQuerySingleOption, <[In]> pOptionValue As PropVariant) As HResult

        <PreserveSig>
        Function GetOption(<[In]> [option] As StructuredQuerySingleOption, <Out> pOptionValue As PropVariant) As HResult

        ' Set a multi option. See STRUCTURED_QUERY_MULTIOPTION above.
        <PreserveSig>
        Function SetMultiOption(<[In]> [option] As StructuredQueryMultipleOption, <[In], MarshalAs(UnmanagedType.LPWStr)> pszOptionKey As String, <[In]> pOptionValue As PropVariant) As HResult

        ' Get a schema provider for browsing the currently loaded schema.
        'ISchemaProvider
        <PreserveSig>
        Function GetSchemaProvider(<Out> ByRef ppSchemaProvider As IntPtr) As HResult

        ' Restate a condition as a query string according to the currently selected syntax.
        ' The parameter fUseEnglish is reserved for future use; must be FALSE.
        <PreserveSig>
        Function RestateToString(<[In]> pCondition As ICondition, <[In]> fUseEnglish As Boolean, <Out, MarshalAs(UnmanagedType.LPWStr)> ByRef ppszQueryString As String) As HResult

        ' Parse a condition for a given property. It can be anything that would go after 'PROPERTY:' in an AQS expession.
        <PreserveSig>
        Function ParsePropertyValue(<[In], MarshalAs(UnmanagedType.LPWStr)> pszPropertyName As String, <[In], MarshalAs(UnmanagedType.LPWStr)> pszInputString As String, <Out> ByRef ppSolution As IQuerySolution) As HResult

        ' Restate a condition for a given property. If the condition contains a leaf with any other property name, or no property name at all,
        ' E_INVALIDARG will be returned.
        <PreserveSig>
        Function RestatePropertyValueToString(<[In]> pCondition As ICondition, <[In]> fUseEnglish As Boolean, <Out, MarshalAs(UnmanagedType.LPWStr)> ByRef ppszPropertyName As String, <Out, MarshalAs(UnmanagedType.LPWStr)> ByRef ppszQueryString As String) As HResult
    End Interface

    <ComImport, Guid(ShellIIDGuid.IQueryParserManager), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IQueryParserManager
        ' Create a query parser loaded with the schema for a certain catalog localize to a certain language, and initialized with
        ' standard defaults. One valid value for riid is IID_IQueryParser.
        <PreserveSig>
        Function CreateLoadedParser(<[In], MarshalAs(UnmanagedType.LPWStr)> pszCatalog As String, <[In]> langidForKeywords As UShort, <[In]> ByRef riid As Guid, <Out> ByRef ppQueryParser As IQueryParser) As HResult

        ' In addition to setting AQS/NQS and automatic wildcard for the given query parser, this sets up standard named entity handlers and
        ' sets the keyboard locale as locale for word breaking.
        <PreserveSig>
        Function InitializeOptions(<[In]> fUnderstandNQS As Boolean, <[In]> fAutoWildCard As Boolean, <[In]> pQueryParser As IQueryParser) As HResult

        ' Change one of the settings for the query parser manager, such as the name of the schema binary, or the location of the localized and unlocalized
        ' schema binaries. By default, the settings point to the schema binaries used by Windows Shell.
        <PreserveSig>
        Function SetOption(<[In]> [option] As QueryParserManagerOption, <[In]> pOptionValue As PropVariant) As HResult

    End Interface

    <ComImport, Guid(ShellIIDGuid.IQueryParserManager), CoClass(GetType(QueryParserManagerCoClass))>
    Friend Interface INativeQueryParserManager
        Inherits IQueryParserManager
    End Interface

    <ComImport, ClassInterface(ClassInterfaceType.None), TypeLibType(TypeLibTypeFlags.FCanCreate), Guid(ShellCLSIDGuid.QueryParserManager)>
    Friend Class QueryParserManagerCoClass
    End Class

    <ComImport, Guid("24264891-E80B-4fd3-B7CE-4FF2FAE8931F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IEntity
        '
    End Interface

#End Region

    Public Module NativeShell

#Region "Shell Helper Methods"

        <DllImport("shell32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
        Friend Function SHCreateShellItemArrayFromDataObject(pdo As System.Runtime.InteropServices.ComTypes.IDataObject, ByRef riid As Guid, <MarshalAs(UnmanagedType.[Interface])> ByRef iShellItemArray As IShellItemArray) As Integer
        End Function

        ' The following parameter is not used - binding context.
        <DllImport("shell32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
        Friend Function SHCreateItemFromParsingName(<MarshalAs(UnmanagedType.LPWStr)> path As String, pbc As IntPtr, ByRef riid As Guid, <MarshalAs(UnmanagedType.[Interface])> ByRef shellItem As IShellItem2) As HResult
        End Function

        ' The following parameter is not used - binding context.
        <DllImport("shell32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
        Friend Function SHCreateItemFromParsingName(<MarshalAs(UnmanagedType.LPWStr)> path As String, pbc As IntPtr, ByRef riid As Guid, <MarshalAs(UnmanagedType.[Interface])> ByRef shellItem As IShellItem) As HResult
        End Function

        <DllImport("shlwapi.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
        Friend Function PathParseIconLocation(<MarshalAs(UnmanagedType.LPWStr)> ByRef pszIconFile As String) As HResult
        End Function

        <DllImport("shell32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
        Public Function SHCreateItemWithParent(pidlParaent As IntPtr, psfParent As IShellFolder, pidl As IntPtr, ByRef riid As Guid, <MarshalAs(UnmanagedType.Interface)> ByRef ppvItem As Object) As HResult
        End Function

        'PCIDLIST_ABSOLUTE
        <DllImport("shell32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
        Friend Function SHCreateItemFromIDList(pidl As IntPtr, ByRef riid As Guid, <MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IShellItem2) As Integer
        End Function

        <DllImport("shell32.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=True)>
        Public Function SHParseDisplayName(<[In], MarshalAs(UnmanagedType.LPWStr)> pszName As String, <[In]> pbc As IntPtr, <Out> ByRef ppidl As IntPtr, <[In]> sfgaoIn As ShellFileGetAttributesOptions, <[In], Out> ByRef psfgaoOut As ShellFileGetAttributesOptions) As HResult
        End Function

        <DllImport("shell32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
        Public Function SHParseDisplayName(pszName As IntPtr, pbc As IntPtr, <Out> ByRef ppidl As IntPtr, sfgaoIn As ShellFileGetAttributesOptions, ByRef psfgaoOut As ShellFileGetAttributesOptions) As HResult
        End Function


        <DllImport("shell32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
        Public Function SHObjectProperties(hwnd As IntPtr, shopObjectType As SHOPType, pszObjectName As String, pszPropertyPage As String) As Boolean
        End Function


        <DllImport("shell32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
        Public Function SHBindToObject(psf As IShellFolder, pidl As IntPtr, pbc As IntPtr, ByRef riid As Guid, <MarshalAs(UnmanagedType.Interface)> ByRef ppv As Object) As HResult
        End Function


        <DllImport("shell32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
        Public Function SHGetIDListFromObject(iUnknown As IntPtr, ByRef ppidl As IntPtr) As Integer
        End Function

        <DllImport("shell32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
        Public Function SHGetDesktopFolder(<MarshalAs(UnmanagedType.[Interface])> ByRef ppshf As IShellFolder) As Integer
        End Function

        <DllImport("shell32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
        Public Function SHCreateShellItem(pidlParent As IntPtr, <[In], MarshalAs(UnmanagedType.[Interface])> psfParent As IShellFolder, pidl As IntPtr, <MarshalAs(UnmanagedType.[Interface])> ByRef ppsi As IShellItem) As Integer
        End Function

        <DllImport("shell32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
        Public Function ILGetSize(pidl As IntPtr) As UInteger
        End Function

        <DllImport("shell32.dll", CharSet:=CharSet.None)>
        Public Sub ILFree(pidl As IntPtr)
        End Sub

        <DllImport("gdi32.dll")>
        Public Function DeleteObject(hObject As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

#End Region

        Public Const InPlaceStringTruncated As Integer = &H401A0

#Region "Shell Library Helper Methods"

        <DllImport("Shell32", CharSet:=CharSet.Unicode, CallingConvention:=CallingConvention.Winapi, SetLastError:=True)>
        Public Function SHShowManageLibraryUI(<[In], MarshalAs(UnmanagedType.[Interface])> library As IShellItem, <[In]> hwndOwner As IntPtr, <[In]> title As String, <[In]> instruction As String, <[In]> lmdOptions As LibraryManageDialogOptions) As Integer
        End Function

#End Region

#Region "Command Link Definitions"

        Public Const CommandLink As Integer = &HE
        Public Const SetNote As UInteger = &H1609
        Public Const GetNote As UInteger = &H160A
        Public Const GetNoteLength As UInteger = &H160B
        Public Const SetShield As UInteger = &H160C

#End Region

#Region "Shell notification definitions"

        Public Const MaxPath As Integer = 260

        <DllImport("shell32.dll")>
        Public Function SHGetPathFromIDListW(pidl As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> pszPath As StringBuilder) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        Public Enum STRRET_TYPE As UInteger
            STRRET_WSTR = 0
            STRRET_OFFSET = 1
            STRRET_CSTR = 2
        End Enum

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure STRRET

            '<FieldOffset(0)>
            Public uType As STRRET_TYPE

            'Public pOleStr As IntPtr

            '<FieldOffset(4), MarshalAs(UnmanagedType.LPWStr)>
            'Public uOffset As UInteger

            '<FieldOffset(4)>
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=MAX_PATH)>
            Public _cStr As Char()

        End Structure
        <StructLayout(LayoutKind.Sequential)>
        Public Structure ShellNotifyStruct
            Public item1 As IntPtr
            Public item2 As IntPtr
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SHChangeNotifyEntry
            Public pIdl As IntPtr

            <MarshalAs(UnmanagedType.Bool)>
            Public recursively As Boolean
        End Structure

        <DllImport("shell32.dll")>
        Public Function SHChangeNotifyRegister(windowHandle As IntPtr, sources As ShellChangeNotifyEventSource, events As ShellObjectChangeTypes, message As UInteger, entries As Integer, ByRef changeNotifyEntry As SHChangeNotifyEntry) As UInteger
        End Function

        <DllImport("shell32.dll")>
        Public Function SHChangeNotification_Lock(windowHandle As IntPtr, processId As Integer, ByRef pidl As IntPtr, ByRef lEvent As UInteger) As IntPtr
        End Function

        <DllImport("shell32.dll")>
        Public Function SHChangeNotification_Unlock(hLock As IntPtr) As <MarshalAs(UnmanagedType.Bool)> [Boolean]
        End Function

        <DllImport("shell32.dll")>
        Public Function SHChangeNotifyDeregister(hNotify As UInteger) As <MarshalAs(UnmanagedType.Bool)> [Boolean]
        End Function

        <Flags>
        Public Enum ShellChangeNotifyEventSource
            InterruptLevel = &H1
            ShellLevel = &H2
            RecursiveInterrupt = &H1000
            NewDelivery = &H8000
        End Enum

#End Region

        Public Enum ASSOC_FILTER
            NONE = 0
            RECOMMENDED = 1
        End Enum

        Public Declare Unicode Function SHAssocEnumHandlers Lib "Shell32.dll" (<MarshalAs(UnmanagedType.LPWStr), [In]> pszExtra As String, <[In]> afFilter As ASSOC_FILTER, <Out> ByRef ppEnumHandler As IEnumAssocHandlers) As HResult

        Public Declare Unicode Function Shell_GetCachedImageIndex Lib "shell32.dll" (<MarshalAs(UnmanagedType.LPWStr)> pwszIconPath As String, iIconIndex As Integer, uIconFlags As UInteger) As IntPtr

        Public HandlerCache As New List(Of KeyValuePair(Of String, IAssocHandler()))

        Public Sub ClearHandlerCache()
            HandlerCache.Clear()
        End Sub

        Public Function FindInCache(fileExtension As String) As IAssocHandler()
            For Each kv In HandlerCache
                If kv.Key = fileExtension Then Return kv.Value
            Next

            Return Nothing
        End Function

        Public Sub AddToCache(fileExtension As String, assoc() As IAssocHandler)
            Dim kv As New KeyValuePair(Of String, IAssocHandler())(fileExtension, assoc)
            HandlerCache.Add(kv)
        End Sub

        Public Function EnumFileHandlers(fileExtension As String, Optional flush As Boolean = False) As IAssocHandler()

            Dim assoc() As IAssocHandler = If(flush, Nothing, FindInCache(fileExtension))
            If assoc IsNot Nothing Then Return assoc

            Static lAssoc As New List(Of IAssocHandler)

            If lAssoc.Count > 0 Then lAssoc.Clear()

            Dim h As IAssocHandler = Nothing,
                handlers As IEnumAssocHandlers = Nothing

            Dim cret As UInteger = 0

            Try
                SHAssocEnumHandlers(fileExtension, ASSOC_FILTER.RECOMMENDED, handlers)
            Catch ex As Exception
                Return Nothing
            End Try

            Do
                h = Nothing
                cret = 0
                handlers.Next(1, h, cret)

                If h IsNot Nothing Then
                    lAssoc.Add(h)
                End If
            Loop While cret > 0

            assoc = lAssoc.ToArray
            AddToCache(fileExtension, assoc)

            Return assoc

        End Function

    End Module


#Region "Shell Library Enums"

    Public Enum LibraryFolderFilter
        ForceFileSystem = 1
        StorageItems = 2
        AllItems = 3
    End Enum

    <Flags>
    Public Enum LibraryOptions
        [Default] = 0
        PinnedToNavigationPane = &H1
        MaskAll = &H1
    End Enum

    Public Enum DefaultSaveFolderType
        Detect = 1
        [Private] = 2
        [Public] = 3
    End Enum

    Public Enum LibrarySaveOptions
        FailIfThere = 0
        OverrideExisting = 1
        MakeUniqueName = 2
    End Enum

    Public Enum LibraryManageDialogOptions
        [Default] = 0
        NonIndexableLocationWarning = 1
    End Enum


#End Region


#Region "Shell Property Store"

#Region "Native Methods"

    Public NotInheritable Class PropertySystemNativeMethods
        Private Sub New()
        End Sub
#Region "Property Definitions"

        Public Enum RelativeDescriptionType
            General
            [Date]
            Size
            Count
            Revision
            Length
            Duration
            Speed
            Rate
            Rating
            Priority
        End Enum

#End Region

#Region "Property System Helpers"

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
        Public Shared Function PSGetNameFromPropertyKey(ByRef propkey As PropertyKey, <Out, MarshalAs(UnmanagedType.LPWStr)> ByRef ppszCanonicalName As String) As Integer
        End Function

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
        Public Shared Function PSGetPropertyDescription(ByRef propkey As PropertyKey, ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IPropertyDescription) As HResult
        End Function

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
        Public Shared Function PSGetPropertyKeyFromName(<[In], MarshalAs(UnmanagedType.LPWStr)> pszCanonicalName As String, ByRef propkey As PropertyKey) As Integer
        End Function

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
        Public Shared Function PSGetPropertyDescriptionListFromString(<[In], MarshalAs(UnmanagedType.LPWStr)> pszPropList As String, <[In]> ByRef riid As Guid, ByRef ppv As IPropertyDescriptionList) As Integer
        End Function



#End Region
    End Class

#End Region

#Region "Property System Enumerations"

    ''' <summary>
    ''' Property store cache state
    ''' </summary>
    Public Enum PropertyStoreCacheState
        ''' <summary>
        ''' Contained in file, not updated.
        ''' </summary>
        Normal = 0

        ''' <summary>
        ''' Not contained in file.
        ''' </summary>
        NotInSource = 1

        ''' <summary>
        ''' Contained in file, has been updated since file was consumed.
        ''' </summary>
        Dirty = 2
    End Enum

    ''' <summary>
    ''' Delineates the format of a property string.
    ''' </summary>
    ''' <remarks>
    ''' Typically use one, or a bitwise combination of 
    ''' these flags, to specify the format. Some flags are mutually exclusive, 
    ''' so combinations like <c>ShortTime | LongTime | HideTime</c> are not allowed.
    ''' </remarks>
    <Flags>
    Public Enum PropertyDescriptionFormatOptions
        ''' <summary>
        ''' The format settings specified in the property's .propdesc file.
        ''' </summary>
        None = 0

        ''' <summary>
        ''' The value preceded with the property's display name.
        ''' </summary>
        ''' <remarks>
        ''' This flag is ignored when the <c>hideLabelPrefix</c> attribute of the <c>labelInfo</c> element 
        ''' in the property's .propinfo file is set to true.
        ''' </remarks>
        PrefixName = &H1

        ''' <summary>
        ''' The string treated as a file name.
        ''' </summary>
        FileName = &H2

        ''' <summary>
        ''' The sizes displayed in kilobytes (KB), regardless of size. 
        ''' </summary>
        ''' <remarks>
        ''' This flag applies to properties of <c>Integer</c> types and aligns the values in the column. 
        ''' </remarks>
        AlwaysKB = &H4

        ''' <summary>
        ''' Reserved.
        ''' </summary>
        RightToLeft = &H8

        ''' <summary>
        ''' The time displayed as 'hh:mm am/pm'.
        ''' </summary>
        ShortTime = &H10

        ''' <summary>
        ''' The time displayed as 'hh:mm:ss am/pm'.
        ''' </summary>
        LongTime = &H20

        ''' <summary>
        ''' The time portion of date/time hidden.
        ''' </summary>
        HideTime = 64

        ''' <summary>
        ''' The date displayed as 'MM/DD/YY'. For example, '3/21/04'.
        ''' </summary>
        ShortDate = &H80

        ''' <summary>
        ''' The date displayed as 'DayOfWeek Month day, year'. 
        ''' For example, 'Monday, March 21, 2004'.
        ''' </summary>
        LongDate = &H100

        ''' <summary>
        ''' The date portion of date/time hidden.
        ''' </summary>
        HideDate = &H200

        ''' <summary>
        ''' The friendly date descriptions, such as "Yesterday".
        ''' </summary>
        RelativeDate = &H400

        ''' <summary>
        ''' The text displayed in a text box as a cue for the user, such as 'Enter your name'.
        ''' </summary>
        ''' <remarks>
        ''' The invitation text is returned if formatting failed or the value was empty. 
        ''' Invitation text is text displayed in a text box as a cue for the user, 
        ''' Formatting can fail if the data entered 
        ''' is not of an expected type, such as putting alpha characters in 
        ''' a phone number field.
        ''' </remarks>
        UseEditInvitation = &H800

        ''' <summary>
        ''' This flag requires UseEditInvitation to also be specified. When the 
        ''' formatting flags are ReadOnly | UseEditInvitation and the algorithm 
        ''' would have shown invitation text, a string is returned that indicates 
        ''' the value is "Unknown" instead of the invitation text.
        ''' </summary>
        [ReadOnly] = &H1000

        ''' <summary>
        ''' The detection of the reading order is not automatic. Useful when converting 
        ''' to ANSI to omit the Unicode reading order characters.
        ''' </summary>
        NoAutoReadingOrder = &H2000

        ''' <summary>
        ''' Smart display of DateTime values
        ''' </summary>
        SmartDateTime = &H4000
    End Enum

    ''' <summary>
    ''' Specifies the display types for a property.
    ''' </summary>
    Public Enum PropertyDisplayType
        ''' <summary>
        ''' The String Display. This is the default if the property doesn't specify a display type.
        ''' </summary>
        [String] = 0

        ''' <summary>
        ''' The Number Display.
        ''' </summary>
        Number = 1

        ''' <summary>
        ''' The Boolean Display.
        ''' </summary>
        [Boolean] = 2

        ''' <summary>
        ''' The DateTime Display.
        ''' </summary>
        DateTime = 3

        ''' <summary>
        ''' The Enumerated Display.
        ''' </summary>
        Enumerated = 4
    End Enum

    ''' <summary>
    ''' Property Aggregation Type
    ''' </summary>
    Public Enum PropertyAggregationType
        ''' <summary>
        ''' The string "Multiple Values" is displayed.
        ''' </summary>
        [Default] = 0

        ''' <summary>
        ''' The first value in the selection is displayed.
        ''' </summary>
        First = 1

        ''' <summary>
        ''' The sum of the selected values is displayed. This flag is never returned 
        ''' for data types VT_LPWSTR, VT_BOOL, and VT_FILETIME.
        ''' </summary>
        Sum = 2

        ''' <summary>
        ''' The numerical average of the selected values is displayed. This flag 
        ''' is never returned for data types VT_LPWSTR, VT_BOOL, and VT_FILETIME.
        ''' </summary>
        Average = 3

        ''' <summary>
        ''' The date range of the selected values is displayed. This flag is only 
        ''' returned for values of the VT_FILETIME data type.
        ''' </summary>
        DateRange = 4

        ''' <summary>
        ''' A concatenated string of all the values is displayed. The order of 
        ''' individual values in the string is undefined. The concatenated 
        ''' string omits duplicate values; if a value occurs more than once, 
        ''' it only appears a single time in the concatenated string.
        ''' </summary>
        Union = 5

        ''' <summary>
        ''' The highest of the selected values is displayed.
        ''' </summary>
        Max = 6

        ''' <summary>
        ''' The lowest of the selected values is displayed.
        ''' </summary>
        Min = 7
    End Enum

    ''' <summary>
    ''' Property Enumeration Types
    ''' </summary>
    Public Enum PropEnumType
        ''' <summary>
        ''' Use DisplayText and either RangeMinValue or RangeSetValue.
        ''' </summary>
        DiscreteValue = 0

        ''' <summary>
        ''' Use DisplayText and either RangeMinValue or RangeSetValue
        ''' </summary>
        RangedValue = 1

        ''' <summary>
        ''' Use DisplayText
        ''' </summary>
        DefaultValue = 2

        ''' <summary>
        ''' Use Value or RangeMinValue
        ''' </summary>
        EndRange = 3
    End Enum

    ''' <summary>
    ''' Describes how a property should be treated for display purposes.
    ''' </summary>
    <Flags>
    Public Enum PropertyColumnStateOptions
        ''' <summary>
        ''' Default value
        ''' </summary>
        None = &H0

        ''' <summary>
        ''' The value is displayed as a string.
        ''' </summary>
        StringType = &H1

        ''' <summary>
        ''' The value is displayed as an integer.
        ''' </summary>
        IntegerType = &H2

        ''' <summary>
        ''' The value is displayed as a date/time.
        ''' </summary>
        DateType = &H3

        ''' <summary>
        ''' A mask for display type values StringType, IntegerType, and DateType.
        ''' </summary>
        TypeMask = &HF

        ''' <summary>
        ''' The column should be on by default in Details view.
        ''' </summary>
        OnByDefault = &H10

        ''' <summary>
        ''' Will be slow to compute. Perform on a background thread.
        ''' </summary>
        Slow = &H20

        ''' <summary>
        ''' Provided by a handler, not the folder.
        ''' </summary>
        Extended = &H40

        ''' <summary>
        ''' Not displayed in the context menu, but is listed in the More... dialog.
        ''' </summary>
        SecondaryUI = &H80

        ''' <summary>
        ''' Not displayed in the user interface (I).
        ''' </summary>
        Hidden = &H100

        ''' <summary>
        ''' VarCmp produces same result as IShellFolder::CompareIDs.
        ''' </summary>
        PreferVariantCompare = &H200

        ''' <summary>
        ''' PSFormatForDisplay produces same result as IShellFolder::CompareIDs.
        ''' </summary>
        PreferFormatForDisplay = &H400

        ''' <summary>
        ''' Do not sort folders separately.
        ''' </summary>
        NoSortByFolders = &H800

        ''' <summary>
        ''' Only displayed in the UI.
        ''' </summary>
        ViewOnly = &H10000

        ''' <summary>
        ''' Marks columns with values that should be read in a batch.
        ''' </summary>
        BatchRead = &H20000

        ''' <summary>
        ''' Grouping is disabled for this column.
        ''' </summary>
        NoGroupBy = &H40000

        ''' <summary>
        ''' Can't resize the column.
        ''' </summary>
        FixedWidth = &H1000

        ''' <summary>
        ''' The width is the same in all dots per inch (dpi)s.
        ''' </summary>
        NoDpiScale = &H2000

        ''' <summary>
        ''' Fixed width and height ratio.
        ''' </summary>
        FixedRatio = &H4000

        ''' <summary>
        ''' Filters out new display flags.
        ''' </summary>
        DisplayMask = &HF000
    End Enum

    ''' <summary>
    ''' Specifies the condition type to use when displaying the property in the query builder user interface (I).
    ''' </summary>
    Public Enum PropertyConditionType
        ''' <summary>
        ''' The default condition type.
        ''' </summary>
        None = 0

        ''' <summary>
        ''' The string type.
        ''' </summary>
        [String] = 1

        ''' <summary>
        ''' The size type.
        ''' </summary>
        Size = 2

        ''' <summary>
        ''' The date/time type.
        ''' </summary>
        DateTime = 3

        ''' <summary>
        ''' The Boolean type.
        ''' </summary>
        [Boolean] = 4

        ''' <summary>
        ''' The number type.
        ''' </summary>
        Number = 5
    End Enum

    ''' <summary>
    ''' Provides a set of flags to be used with IConditionFactory, 
    ''' ICondition, and IConditionGenerator to indicate the operation.
    ''' </summary>
    Public Enum PropertyConditionOperation
        ''' <summary>
        ''' The implicit comparison between the value of the property and the value of the constant.
        ''' </summary>
        Implicit

        ''' <summary>
        ''' The value of the property and the value of the constant must be equal.
        ''' </summary>
        Equal

        ''' <summary>
        ''' The value of the property and the value of the constant must not be equal.
        ''' </summary>
        NotEqual

        ''' <summary>
        ''' The value of the property must be less than the value of the constant.
        ''' </summary>
        LessThan

        ''' <summary>
        ''' The value of the property must be greater than the value of the constant.
        ''' </summary>
        GreaterThan

        ''' <summary>
        ''' The value of the property must be less than or equal to the value of the constant.
        ''' </summary>
        LessThanOrEqual

        ''' <summary>
        ''' The value of the property must be greater than or equal to the value of the constant.
        ''' </summary>
        GreaterThanOrEqual

        ''' <summary>
        ''' The value of the property must begin with the value of the constant.
        ''' </summary>
        ValueStartsWith

        ''' <summary>
        ''' The value of the property must end with the value of the constant.
        ''' </summary>
        ValueEndsWith

        ''' <summary>
        ''' The value of the property must contain the value of the constant.
        ''' </summary>
        ValueContains

        ''' <summary>
        ''' The value of the property must not contain the value of the constant.
        ''' </summary>
        ValueNotContains

        ''' <summary>
        ''' The value of the property must match the value of the constant, where '?' matches any single character and '*' matches any sequence of characters.
        ''' </summary>
        DOSWildCards

        ''' <summary>
        ''' The value of the property must contain a word that is the value of the constant.
        ''' </summary>
        WordEqual

        ''' <summary>
        ''' The value of the property must contain a word that begins with the value of the constant.
        ''' </summary>
        WordStartsWith

        ''' <summary>
        ''' The application is free to interpret this in any suitable way.
        ''' </summary>
        ApplicationSpecific
    End Enum

    ''' <summary>
    ''' Specifies the property description grouping ranges.
    ''' </summary>
    Public Enum PropertyGroupingRange
        ''' <summary>
        ''' The individual values.
        ''' </summary>
        Discrete = 0

        ''' <summary>
        ''' The static alphanumeric ranges.
        ''' </summary>
        Alphanumeric = 1

        ''' <summary>
        ''' The static size ranges.
        ''' </summary>
        Size = 2

        ''' <summary>
        ''' The dynamically-created ranges.
        ''' </summary>
        Dynamic = 3

        ''' <summary>
        ''' The month and year groups.
        ''' </summary>
        [Date] = 4

        ''' <summary>
        ''' The percent groups.
        ''' </summary>
        Percent = 5

        ''' <summary>
        ''' The enumerated groups.
        ''' </summary>
        Enumerated = 6
    End Enum

    ''' <summary>
    ''' Describes the particular wordings of sort offerings.
    ''' </summary>
    ''' <remarks>
    ''' Note that the strings shown are English versions only; 
    ''' localized strings are used for other locales.
    ''' </remarks>
    Public Enum PropertySortDescription
        ''' <summary>
        ''' The default ascending or descending property sort, "Sort going up", "Sort going down".
        ''' </summary>
        General

        ''' <summary>
        ''' The alphabetical sort, "A on top", "Z on top".
        ''' </summary>
        AToZ

        ''' <summary>
        ''' The numerical sort, "Lowest on top", "Highest on top".
        ''' </summary>
        LowestToHighest

        ''' <summary>
        ''' The size sort, "Smallest on top", "Largest on top".
        ''' </summary>
        SmallestToBiggest

        ''' <summary>
        ''' The chronological sort, "Oldest on top", "Newest on top".
        ''' </summary>
        OldestToNewest
    End Enum

    ''' <summary>
    ''' Describes the attributes of the <c>typeInfo</c> element in the property's <c>.propdesc</c> file.
    ''' </summary>
    <Flags>
    Public Enum PropertyTypeOptions
        ''' <summary>
        ''' The property uses the default values for all attributes.
        ''' </summary>
        None = &H0

        ''' <summary>
        ''' The property can have multiple values.   
        ''' </summary>
        ''' <remarks>
        ''' These values are stored as a VT_VECTOR in the PROPVARIANT structure.
        ''' This value is set by the multipleValues attribute of the typeInfo element in the property's .propdesc file.
        ''' </remarks>
        MultipleValues = &H1

        ''' <summary>
        ''' This property cannot be written to. 
        ''' </summary>
        ''' <remarks>
        ''' This value is set by the isInnate attribute of the typeInfo element in the property's .propdesc file.
        ''' </remarks>
        IsInnate = &H2

        ''' <summary>
        ''' The property is a group heading. 
        ''' </summary>
        ''' <remarks>
        ''' This value is set by the isGroup attribute of the typeInfo element in the property's .propdesc file.
        ''' </remarks>
        IsGroup = &H4

        ''' <summary>
        ''' The user can group by this property. 
        ''' </summary>
        ''' <remarks>
        ''' This value is set by the canGroupBy attribute of the typeInfo element in the property's .propdesc file.
        ''' </remarks>
        CanGroupBy = &H8

        ''' <summary>
        ''' The user can stack by this property. 
        ''' </summary>
        ''' <remarks>
        ''' This value is set by the canStackBy attribute of the typeInfo element in the property's .propdesc file.
        ''' </remarks>
        CanStackBy = &H10

        ''' <summary>
        ''' This property contains a hierarchy. 
        ''' </summary>
        ''' <remarks>
        ''' This value is set by the isTreeProperty attribute of the typeInfo element in the property's .propdesc file.
        ''' </remarks>
        IsTreeProperty = &H20

        ''' <summary>
        ''' Include this property in any full text query that is performed. 
        ''' </summary>
        ''' <remarks>
        ''' This value is set by the includeInFullTextQuery attribute of the typeInfo element in the property's .propdesc file.
        ''' </remarks>
        IncludeInFullTextQuery = &H40

        ''' <summary>
        ''' This property is meant to be viewed by the user.  
        ''' </summary>
        ''' <remarks>
        ''' This influences whether the property shows up in the "Choose Columns" dialog, for example.
        ''' This value is set by the isViewable attribute of the typeInfo element in the property's .propdesc file.
        ''' </remarks>
        IsViewable = &H80

        ''' <summary>
        ''' This property is included in the list of properties that can be queried.   
        ''' </summary>
        ''' <remarks>
        ''' A queryable property must also be viewable.
        ''' This influences whether the property shows up in the query builder UI.
        ''' This value is set by the isQueryable attribute of the typeInfo element in the property's .propdesc file.
        ''' </remarks>
        IsQueryable = &H100

        ''' <summary>
        ''' Used with an innate property (that is, a value calculated from other property values) to indicate that it can be deleted.  
        ''' </summary>
        ''' <remarks>
        ''' Windows Vista with Service Pack 1 (SP1) and later.
        ''' This value is used by the Remove Properties user interface (I) to determine whether to display a check box next to an property that allows that property to be selected for removal.
        ''' Note that a property that is not innate can always be purged regardless of the presence or absence of this flag.
        ''' </remarks>
        CanBePurged = &H200

        ''' <summary>
        ''' This property is owned by the system.
        ''' </summary>
        IsSystemProperty = CInt(&H80000000I)

        ''' <summary>
        ''' A mask used to retrieve all flags.
        ''' </summary>
        MaskAll = CInt(&H800001FFI)
    End Enum

    ''' <summary>
    ''' Associates property names with property description list strings.
    ''' </summary>
    <Flags>
    Public Enum PropertyViewOptions
        ''' <summary>
        ''' The property is shown by default.
        ''' </summary>
        None = &H0

        ''' <summary>
        ''' The property is centered.
        ''' </summary>
        CenterAlign = &H1

        ''' <summary>
        ''' The property is right aligned.
        ''' </summary>
        RightAlign = &H2

        ''' <summary>
        ''' The property is shown as the beginning of the next collection of properties in the view.
        ''' </summary>
        BeginNewGroup = &H4

        ''' <summary>
        ''' The remainder of the view area is filled with the content of this property.
        ''' </summary>
        FillArea = &H8

        ''' <summary>
        ''' The property is reverse sorted if it is a property in a list of sorted properties.
        ''' </summary>
        SortDescending = &H10

        ''' <summary>
        ''' The property is only shown if it is present.
        ''' </summary>
        ShowOnlyIfPresent = &H20

        ''' <summary>
        ''' The property is shown by default in a view (where applicable).
        ''' </summary>
        ShowByDefault = &H40

        ''' <summary>
        ''' The property is shown by default in primary column selection user interface (I).
        ''' </summary>
        ShowInPrimaryList = &H80

        ''' <summary>
        ''' The property is shown by default in secondary column selection UI.
        ''' </summary>
        ShowInSecondaryList = &H100

        ''' <summary>
        ''' The label is hidden if the view is normally inclined to show the label.
        ''' </summary>
        HideLabel = &H200

        ''' <summary>
        ''' The property is not displayed as a column in the UI.
        ''' </summary>
        Hidden = &H800

        ''' <summary>
        ''' The property is wrapped to the next row.
        ''' </summary>
        CanWrap = &H1000

        ''' <summary>
        ''' A mask used to retrieve all flags.
        ''' </summary>
        MaskAll = &H3FF
    End Enum

#End Region

#Region "PropVariant"

    '' Borrowed from the WindowsAPICodePack (v1.1), Translated.


    ''' <summary>
    ''' Represents the OLE struct PROPVARIANT.
    ''' This class is intended for internal use only.
    ''' </summary>
    ''' <remarks>
    ''' Originally sourced from http://blogs.msdn.com/adamroot/pages/interop-with-propvariants-in-net.aspx
    ''' and modified to support additional types including vectors and ability to set values
    ''' </remarks>
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable", MessageId:="_ptr2")>
    <StructLayout(LayoutKind.Explicit)>
    Public NotInheritable Class PropVariant
        Implements IDisposable
#Region "Vector Action Cache"

        ' A static dictionary of delegates to get data from array's contained within PropVariants
        Private Shared _vectorActions As Dictionary(Of Type, Action(Of PropVariant, Array, UInteger)) = Nothing
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")>
        Private Shared Function GenerateVectorActions() As Dictionary(Of Type, Action(Of PropVariant, Array, UInteger))
            Dim cache As New Dictionary(Of Type, Action(Of PropVariant, Array, UInteger))()

            cache.Add(GetType(Int16), Sub(pv, array, i)
                                          Dim val As Short
                                          PropVariantNativeMethods.PropVariantGetInt16Elem(pv, i, val)
                                          array.SetValue(val, i)

                                      End Sub)

            cache.Add(GetType(UInt16), Sub(pv, array, i)
                                           Dim val As UShort
                                           PropVariantNativeMethods.PropVariantGetUInt16Elem(pv, i, val)
                                           array.SetValue(val, i)

                                       End Sub)

            cache.Add(GetType(Int32), Sub(pv, array, i)
                                          Dim val As Integer
                                          PropVariantNativeMethods.PropVariantGetInt32Elem(pv, i, val)
                                          array.SetValue(val, i)

                                      End Sub)

            cache.Add(GetType(UInt32), Sub(pv, array, i)
                                           Dim val As UInteger
                                           PropVariantNativeMethods.PropVariantGetUInt32Elem(pv, i, val)
                                           array.SetValue(val, i)

                                       End Sub)

            cache.Add(GetType(Int64), Sub(pv, array, i)
                                          Dim val As Long
                                          PropVariantNativeMethods.PropVariantGetInt64Elem(pv, i, val)
                                          array.SetValue(val, i)

                                      End Sub)

            cache.Add(GetType(UInt64), Sub(pv, array, i)
                                           Dim val As ULong
                                           PropVariantNativeMethods.PropVariantGetUInt64Elem(pv, i, val)
                                           array.SetValue(val, i)

                                       End Sub)

            cache.Add(GetType(DateTime), Sub(pv, array, i)
                                             Dim val As System.Runtime.InteropServices.ComTypes.FILETIME
                                             PropVariantNativeMethods.PropVariantGetFileTimeElem(pv, i, val)

                                             Dim fileTime As Long = GetFileTimeAsLong(val)

                                             array.SetValue(DateTime.FromFileTime(fileTime), i)

                                         End Sub)

            cache.Add(GetType([Boolean]), Sub(pv, array, i)
                                              Dim val As Boolean
                                              PropVariantNativeMethods.PropVariantGetBooleanElem(pv, i, val)
                                              array.SetValue(val, i)

                                          End Sub)

            cache.Add(GetType([Double]), Sub(pv, array, i)
                                             Dim val As Double
                                             PropVariantNativeMethods.PropVariantGetDoubleElem(pv, i, val)
                                             array.SetValue(val, i)

                                         End Sub)

            cache.Add(GetType([Single]), Sub(pv, array, i)
                                             ' float
                                             Dim val As Single() = New Single(0) {}
                                             Marshal.Copy(pv._ptr2, val, CInt(i), 1)
                                             array.SetValue(val(0), CInt(i))

                                         End Sub)

            cache.Add(GetType([Decimal]), Sub(pv, array, i)
                                              Dim val As Integer() = New Integer(3) {}
                                              For a As Integer = 0 To val.Length - 1
                                                  'index * size + offset quarter
                                                  val(a) = Marshal.ReadInt32(pv._ptr2, CInt(i) * Len(CDec(0)) + a * 4)
                                              Next
                                              array.SetValue(New Decimal(val), i)

                                          End Sub)

            cache.Add(GetType([String]), Sub(pv, array, i)
                                             Dim val As String = String.Empty
                                             PropVariantNativeMethods.PropVariantGetStringElem(pv, i, val)
                                             array.SetValue(val, i)

                                         End Sub)

            Return cache
        End Function
#End Region

#Region "Dynamic Construction / Factory (Expressions)"

        ''' <summary>
        ''' Attempts to create a PropVariant by finding an appropriate constructor.
        ''' </summary>
        ''' <param name="value">Object from which PropVariant should be created.</param>
        Public Shared Function FromObject(value As Object) As PropVariant
            If value Is Nothing Then
                Return New PropVariant()
            Else
                Dim func = GetDynamicConstructor(value.[GetType]())
                Return func(value)
            End If
        End Function

        ' A dictionary and lock to contain compiled expression trees for constructors
        Private Shared _cache As New Dictionary(Of Type, Func(Of Object, PropVariant))()
        Private Shared _padlock As New Object()

        ' Retrieves a cached constructor expression.
        ' If no constructor has been cached, it attempts to find/add it.  If it cannot be found
        ' an exception is thrown.
        ' This method looks for a public constructor with the same parameter type as the object.
        Private Shared Function GetDynamicConstructor(type As Type) As Func(Of Object, PropVariant)
            SyncLock _padlock
                ' initial check, if action is found, return it
                Dim action As Func(Of Object, PropVariant) = Nothing
                If Not _cache.TryGetValue(type, action) Then
                    ' iterates through all constructors
                    Dim constructor As ConstructorInfo = GetType(PropVariant).GetConstructor(New Type() {type})

                    If constructor Is Nothing Then
                        ' if the method was not found, throw.
                        Throw New ArgumentException(LocalizedMessages.PropVariantTypeNotSupported)
                    Else
                        ' if the method was found, create an expression to call it.
                        ' create parameters to action                    
                        Dim arg = Expression.Parameter(GetType(Object), "arg")

                        ' create an expression to invoke the constructor with an argument cast to the correct type
                        Dim create = Expression.[New](constructor, Expression.Convert(arg, type))

                        ' compiles expression into an action delegate
                        action = Expression.Lambda(Of Func(Of Object, PropVariant))(create, arg).Compile()
                        _cache.Add(type, action)
                    End If
                End If
                Return action
            End SyncLock
        End Function

#End Region

#Region "Fields"

        <FieldOffset(0)>
        Private _decimal As Decimal

        ' This is actually a VarEnum value, but the VarEnum type
        ' requires 4 bytes instead of the expected 2.
        <FieldOffset(0)>
        Private _valueType As UShort

        ' Reserved Fields
        '[FieldOffset(2)]
        'ushort _wReserved1;
        '[FieldOffset(4)]
        'ushort _wReserved2;
        '[FieldOffset(6)]
        'ushort _wReserved3;

        ' In order to allow x64 compat, we need to allow for
        ' expansion of the IntPtr. However, the BLOB struct
        ' uses a 4-byte int, followed by an IntPtr, so
        ' although the valueData field catches most pointer values,
        ' we need an additional 4-bytes to get the BLOB
        ' pointer. The valueDataExt field provides this, as well as
        ' the last 4-bytes of an 8-byte value on 32-bit
        ' architectures.
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")>
        <FieldOffset(12)>
        Private _ptr2 As IntPtr
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")>
        <FieldOffset(8)>
        Private _ptr As IntPtr
        <FieldOffset(8)>
        Private _int32 As Int32
        <FieldOffset(8)>
        Private _uint32 As UInt32
        <FieldOffset(8)>
        Private _byte As Byte
        <FieldOffset(8)>
        Private _sbyte As SByte
        <FieldOffset(8)>
        Private _short As Short
        <FieldOffset(8)>
        Private _ushort As UShort
        <FieldOffset(8)>
        Private _long As Long
        <FieldOffset(8)>
        Private _ulong As ULong
        <FieldOffset(8)>
        Private _double As Double
        <FieldOffset(8)>
        Private _float As Single

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Default constrcutor
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Set a string value
        ''' </summary>
        Public Sub New(value As String)
            If value Is Nothing Then
                Throw New ArgumentException(LocalizedMessages.PropVariantNullString, "value")
            End If

            _valueType = CUShort(VarEnum.VT_LPWSTR)
            _ptr = Marshal.StringToCoTaskMemUni(value)
        End Sub

        ''' <summary>
        ''' Set a string vector
        ''' </summary>
        Public Sub New(value As String())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If

            PropVariantNativeMethods.InitPropVariantFromStringVector(value, CUInt(value.Length), Me)
        End Sub

        ''' <summary>
        ''' Set a bool vector
        ''' </summary>
        Public Sub New(value As Boolean())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If

            PropVariantNativeMethods.InitPropVariantFromBooleanVector(value, CUInt(value.Length), Me)
        End Sub

        ''' <summary>
        ''' Set a short vector
        ''' </summary>
        Public Sub New(value As Short())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If

            PropVariantNativeMethods.InitPropVariantFromInt16Vector(value, CUInt(value.Length), Me)
        End Sub

        ''' <summary>
        ''' Set a short vector
        ''' </summary>
        Public Sub New(value As UShort())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If


            PropVariantNativeMethods.InitPropVariantFromUInt16Vector(value, CUInt(value.Length), Me)
        End Sub

        ''' <summary>
        ''' Set an int vector
        ''' </summary>
        Public Sub New(value As Integer())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If

            PropVariantNativeMethods.InitPropVariantFromInt32Vector(value, CUInt(value.Length), Me)
        End Sub

        ''' <summary>
        ''' Set an uint vector
        ''' </summary>
        Public Sub New(value As UInteger())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If

            PropVariantNativeMethods.InitPropVariantFromUInt32Vector(value, CUInt(value.Length), Me)
        End Sub

        ''' <summary>
        ''' Set a long vector
        ''' </summary>
        Public Sub New(value As Long())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If

            PropVariantNativeMethods.InitPropVariantFromInt64Vector(value, CUInt(value.Length), Me)
        End Sub

        ''' <summary>
        ''' Set a ulong vector
        ''' </summary>
        Public Sub New(value As ULong())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If

            PropVariantNativeMethods.InitPropVariantFromUInt64Vector(value, CUInt(value.Length), Me)
        End Sub

        ''' <summary>>
        ''' Set a double vector
        ''' </summary>
        Public Sub New(value As Double())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If

            PropVariantNativeMethods.InitPropVariantFromDoubleVector(value, CUInt(value.Length), Me)
        End Sub


        ''' <summary>
        ''' Set a DateTime vector
        ''' </summary>
        Public Sub New(value As DateTime())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If
            Dim fileTimeArr As System.Runtime.InteropServices.ComTypes.FILETIME() = New System.Runtime.InteropServices.ComTypes.FILETIME(value.Length - 1) {}

            For i As Integer = 0 To value.Length - 1
                fileTimeArr(i) = DateTimeToFileTime(value(i))
            Next

            PropVariantNativeMethods.InitPropVariantFromFileTimeVector(fileTimeArr, CUInt(fileTimeArr.Length), Me)
        End Sub

        ''' <summary>
        ''' Set a bool value
        ''' </summary>
        Public Sub New(value As Boolean)
            _valueType = CUShort(VarEnum.VT_BOOL)
            _int32 = If((value = True), -1, 0)
        End Sub

        ''' <summary>
        ''' Set a DateTime value
        ''' </summary>
        Public Sub New(value As DateTime)
            _valueType = CUShort(VarEnum.VT_FILETIME)

            Dim ft As System.Runtime.InteropServices.ComTypes.FILETIME = DateTimeToFileTime(value)
            PropVariantNativeMethods.InitPropVariantFromFileTime(ft, Me)
        End Sub


        ''' <summary>
        ''' Set a byte value
        ''' </summary>
        Public Sub New(value As Byte)
            _valueType = CUShort(VarEnum.VT_UI1)
            _byte = value
        End Sub

        ''' <summary>
        ''' Set a sbyte value
        ''' </summary>
        Public Sub New(value As SByte)
            _valueType = CUShort(VarEnum.VT_I1)
            _sbyte = value
        End Sub

        ''' <summary>
        ''' Set a short value
        ''' </summary>
        Public Sub New(value As Short)
            _valueType = CUShort(VarEnum.VT_I2)
            _short = value
        End Sub

        ''' <summary>
        ''' Set an unsigned short value
        ''' </summary>
        Public Sub New(value As UShort)
            _valueType = CUShort(VarEnum.VT_UI2)
            _ushort = value
        End Sub

        ''' <summary>
        ''' Set an int value
        ''' </summary>
        Public Sub New(value As Integer)
            _valueType = CUShort(VarEnum.VT_I4)
            _int32 = value
        End Sub

        ''' <summary>
        ''' Set an unsigned int value
        ''' </summary>
        Public Sub New(value As UInteger)
            _valueType = CUShort(VarEnum.VT_UI4)
            _uint32 = value
        End Sub

        ''' <summary>
        ''' Set a decimal  value
        ''' </summary>
        Public Sub New(value As Decimal)
            _decimal = value

            ' It is critical that the value type be set after the decimal value, because they overlap.
            ' If valuetype is written first, its value will be lost when _decimal is written.
            _valueType = CUShort(VarEnum.VT_DECIMAL)
        End Sub

        ''' <summary>
        ''' Create a PropVariant with a contained decimal array.
        ''' </summary>
        ''' <param name="value">Decimal array to wrap.</param>
        Public Sub New(value As Decimal())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If

            _valueType = CUShort(VarEnum.VT_DECIMAL Or VarEnum.VT_VECTOR)
            _int32 = value.Length

            ' allocate required memory for array with 128bit elements
            _ptr2 = Marshal.AllocCoTaskMem(value.Length * Len(CDec(0)))
            For i As Integer = 0 To value.Length - 1
                Dim bits As Integer() = Decimal.GetBits(value(i))
                Marshal.Copy(bits, 0, _ptr2, bits.Length)
            Next
        End Sub

        ''' <summary>
        ''' Create a PropVariant containing a float type.
        ''' </summary>        
        Public Sub New(value As Single)
            _valueType = CUShort(VarEnum.VT_R4)

            _float = value
        End Sub

        ''' <summary>
        ''' Creates a PropVariant containing a float[] array.
        ''' </summary>        
        Public Sub New(value As Single())
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If

            _valueType = CUShort(VarEnum.VT_R4 Or VarEnum.VT_VECTOR)
            _int32 = value.Length

            _ptr2 = Marshal.AllocCoTaskMem(value.Length * 4)

            Marshal.Copy(value, 0, _ptr2, value.Length)
        End Sub

        ''' <summary>
        ''' Set a long
        ''' </summary>
        Public Sub New(value As Long)
            _long = value
            _valueType = CUShort(VarEnum.VT_I8)
        End Sub

        ''' <summary>
        ''' Set a ulong
        ''' </summary>
        Public Sub New(value As ULong)
            _valueType = CUShort(VarEnum.VT_UI8)
            _ulong = value
        End Sub

        ''' <summary>
        ''' Set a double
        ''' </summary>
        Public Sub New(value As Double)
            _valueType = CUShort(VarEnum.VT_R8)
            _double = value
        End Sub

#End Region

#Region "Uncalled methods - These are currently not called, but I think may be valid in the future."

        ''' <summary>
        ''' Set an IUnknown value
        ''' </summary>
        ''' <param name="value">The new value to set.</param>
        Friend Sub SetIUnknown(value As Object)
            _valueType = CUShort(VarEnum.VT_UNKNOWN)
            _ptr = Marshal.GetIUnknownForObject(value)
        End Sub


        ''' <summary>
        ''' Set a safe array value
        ''' </summary>
        ''' <param name="array">The new value to set.</param>
        Friend Sub SetSafeArray(array As Array)
            If array Is Nothing Then
                Throw New ArgumentNullException("array")
            End If
            Const vtUnknown As UShort = 13
            Dim psa As IntPtr = PropVariantNativeMethods.SafeArrayCreateVector(vtUnknown, 0, CUInt(array.Length))

            Dim pvData As IntPtr = PropVariantNativeMethods.SafeArrayAccessData(psa)
            Try
                ' to remember to release lock on data
                For i As Integer = 0 To array.Length - 1
                    Dim obj As Object = array.GetValue(i)
                    Dim punk As IntPtr = If((obj IsNot Nothing), Marshal.GetIUnknownForObject(obj), IntPtr.Zero)
                    Marshal.WriteIntPtr(pvData, i * IntPtr.Size, punk)
                Next
            Finally
                PropVariantNativeMethods.SafeArrayUnaccessData(psa)
            End Try

            _valueType = CUShort(VarEnum.VT_ARRAY) Or CUShort(VarEnum.VT_UNKNOWN)
            _ptr = psa
        End Sub

#End Region

#Region "public Properties"

        ''' <summary>
        ''' Gets or sets the variant type.
        ''' </summary>
        Public Property VarType() As VarEnum
            Get
                Return CType(_valueType, VarEnum)
            End Get
            Set
                _valueType = CUShort(Value)
            End Set
        End Property

        ''' <summary>
        ''' Checks if this has an empty or null value
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IsNullOrEmpty() As Boolean
            Get
                Return (_valueType = CUShort(VarEnum.VT_EMPTY) OrElse _valueType = CUShort(VarEnum.VT_NULL))
            End Get
        End Property

        ''' <summary>
        ''' Gets the variant value.
        ''' </summary>
        Public ReadOnly Property Value() As Object
            Get
                Select Case CType(_valueType, VarEnum)
                    Case VarEnum.VT_I1
                        Return _sbyte
                    Case VarEnum.VT_UI1
                        Return _byte
                    Case VarEnum.VT_I2
                        Return _short
                    Case VarEnum.VT_UI2
                        Return _ushort
                    Case VarEnum.VT_I4, VarEnum.VT_INT
                        Return _int32
                    Case VarEnum.VT_UI4, VarEnum.VT_UINT
                        Return _uint32
                    Case VarEnum.VT_I8
                        Return _long
                    Case VarEnum.VT_UI8
                        Return _ulong
                    Case VarEnum.VT_R4
                        Return _float
                    Case VarEnum.VT_R8
                        Return _double
                    Case VarEnum.VT_BOOL
                        Return _int32 = -1
                    Case VarEnum.VT_ERROR
                        Return _long
                    Case VarEnum.VT_CY
                        Return _decimal
                    Case VarEnum.VT_DATE
                        Return DateTime.FromOADate(_double)
                    Case VarEnum.VT_FILETIME
                        Return DateTime.FromFileTime(_long)
                    Case VarEnum.VT_BSTR
                        Return Marshal.PtrToStringBSTR(_ptr)
                    Case VarEnum.VT_BLOB
                        Return GetBlobData()
                    Case VarEnum.VT_LPSTR
                        Return Marshal.PtrToStringAnsi(_ptr)
                    Case VarEnum.VT_LPWSTR
                        Return Marshal.PtrToStringUni(_ptr)
                    Case VarEnum.VT_UNKNOWN
                        Return Marshal.GetObjectForIUnknown(_ptr)
                    Case VarEnum.VT_DISPATCH
                        Return Marshal.GetObjectForIUnknown(_ptr)
                    Case VarEnum.VT_DECIMAL
                        Return _decimal
                    Case VarEnum.VT_ARRAY Or VarEnum.VT_UNKNOWN
                        Return CrackSingleDimSafeArray(_ptr)
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_LPWSTR)
                        Return GetVector(Of String)()
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_I2)
                        Return GetVector(Of Int16)()
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_UI2)
                        Return GetVector(Of UInt16)()
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_I4)
                        Return GetVector(Of Int32)()
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_UI4)
                        Return GetVector(Of UInt32)()
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_I8)
                        Return GetVector(Of Int64)()
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_UI8)
                        Return GetVector(Of UInt64)()
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_R4)
                        Return GetVector(Of Single)()
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_R8)
                        Return GetVector(Of [Double])()
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_BOOL)
                        Return GetVector(Of [Boolean])()
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_FILETIME)
                        Return GetVector(Of DateTime)()
                    Case (VarEnum.VT_VECTOR Or VarEnum.VT_DECIMAL)
                        Return GetVector(Of [Decimal])()
                    Case Else
                        ' if the value cannot be marshaled
                        Return Nothing
                End Select
            End Get
        End Property

#End Region

#Region "Private Methods"

        Private Shared Function GetFileTimeAsLong(ByRef val As System.Runtime.InteropServices.ComTypes.FILETIME) As Long
            Return (CLng(val.dwHighDateTime) << 32) + val.dwLowDateTime
        End Function

        Private Shared Function DateTimeToFileTime(value As DateTime) As System.Runtime.InteropServices.ComTypes.FILETIME
            Dim hFT As Long = value.ToFileTime()
            Dim ft As New System.Runtime.InteropServices.ComTypes.FILETIME()
            ft.dwLowDateTime = CInt(hFT And &HFFFFFFFFUI)
            ft.dwHighDateTime = CInt(hFT >> 32)
            Return ft
        End Function

        Private Function GetBlobData() As Object
            Dim blobData As Byte() = New Byte(_int32 - 1) {}

            Dim pBlobData As IntPtr = _ptr2
            Marshal.Copy(pBlobData, blobData, 0, _int32)

            Return blobData
        End Function

        Private Function GetVector(Of T)() As Array
            Dim count As Integer = PropVariantNativeMethods.PropVariantGetElementCount(Me)
            If count <= 0 Then
                Return Nothing
            End If

            SyncLock _padlock
                If _vectorActions Is Nothing Then
                    _vectorActions = GenerateVectorActions()
                End If
            End SyncLock

            Dim action As Action(Of PropVariant, Array, UInteger) = Nothing
            If Not _vectorActions.TryGetValue(GetType(T), action) Then
                Throw New InvalidCastException(LocalizedMessages.PropVariantUnsupportedType)
            End If

            Dim array As Array = New T(count - 1) {}
            For i As Integer = 0 To count - 1
                action(Me, array, CUInt(i))
            Next

            Return array
        End Function

        Private Shared Function CrackSingleDimSafeArray(psa As IntPtr) As Array
            Dim cDims As UInteger = PropVariantNativeMethods.SafeArrayGetDim(psa)
            If cDims <> 1 Then
                Throw New ArgumentException(LocalizedMessages.PropVariantMultiDimArray, "psa")
            End If

            Dim lBound As Integer = PropVariantNativeMethods.SafeArrayGetLBound(psa, 1UI)
            Dim uBound As Integer = PropVariantNativeMethods.SafeArrayGetUBound(psa, 1UI)

            Dim n As Integer = uBound - lBound + 1
            ' uBound is inclusive
            Dim array As Object() = New Object(n - 1) {}
            For i As Integer = lBound To uBound
                array(i) = PropVariantNativeMethods.SafeArrayGetElement(psa, i)
            Next

            Return array
        End Function

#End Region

#Region "IDisposable Members"

        ''' <summary>
        ''' Disposes the object, calls the clear function.
        ''' </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            PropVariantNativeMethods.PropVariantClear(Me)

            GC.SuppressFinalize(Me)
        End Sub

        ''' <summary>
        ''' Finalizer
        ''' </summary>
        Protected Overrides Sub Finalize()
            Try
                Dispose()
            Finally
                MyBase.Finalize()
            End Try
        End Sub

#End Region

        ''' <summary>
        ''' Provides an simple string representation of the contained data and type.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Return String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}: {1}", Value, VarType.ToString())
        End Function

    End Class

    Friend NotInheritable Class PropVariantNativeMethods
        Private Sub New()
        End Sub
        ' returns hresult
        <DllImport("Ole32.dll", PreserveSig:=False)>
        Friend Shared Sub PropVariantClear(<[In], Out> pvar As PropVariant)
        End Sub

        ' psa is actually returned, not hresult
        <DllImport("OleAut32.dll", PreserveSig:=True)>
        Friend Shared Function SafeArrayCreateVector(vt As UShort, lowerBound As Integer, cElems As UInteger) As IntPtr
        End Function

        ' returns hresult
        <DllImport("OleAut32.dll", PreserveSig:=False)>
        Friend Shared Function SafeArrayAccessData(psa As IntPtr) As IntPtr
        End Function

        ' returns hresult
        <DllImport("OleAut32.dll", PreserveSig:=False)>
        Friend Shared Sub SafeArrayUnaccessData(psa As IntPtr)
        End Sub

        ' retuns uint32
        <DllImport("OleAut32.dll", PreserveSig:=True)>
        Friend Shared Function SafeArrayGetDim(psa As IntPtr) As UInteger
        End Function

        ' returns hresult
        <DllImport("OleAut32.dll", PreserveSig:=False)>
        Friend Shared Function SafeArrayGetLBound(psa As IntPtr, nDim As UInteger) As Integer
        End Function

        ' returns hresult
        <DllImport("OleAut32.dll", PreserveSig:=False)>
        Friend Shared Function SafeArrayGetUBound(psa As IntPtr, nDim As UInteger) As Integer
        End Function

        ' This decl for SafeArrayGetElement is only valid for cDims==1!
        ' returns hresult
        <DllImport("OleAut32.dll", PreserveSig:=False)>
        Friend Shared Function SafeArrayGetElement(psa As IntPtr, ByRef rgIndices As Integer) As <MarshalAs(UnmanagedType.IUnknown)> Object
        End Function

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromPropVariantVectorElem(<[In]> propvarIn As PropVariant, iElem As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromFileTime(<[In]> ByRef pftIn As System.Runtime.InteropServices.ComTypes.FILETIME, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
        Friend Shared Function PropVariantGetElementCount(<[In]> propVar As PropVariant) As <MarshalAs(UnmanagedType.I4)> Integer
        End Function

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub PropVariantGetBooleanElem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out, MarshalAs(UnmanagedType.Bool)> ByRef pfVal As Boolean)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub PropVariantGetInt16Elem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out> ByRef pnVal As Short)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub PropVariantGetUInt16Elem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out> ByRef pnVal As UShort)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub PropVariantGetInt32Elem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out> ByRef pnVal As Integer)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub PropVariantGetUInt32Elem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out> ByRef pnVal As UInteger)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub PropVariantGetInt64Elem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out> ByRef pnVal As Int64)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub PropVariantGetUInt64Elem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out> ByRef pnVal As UInt64)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub PropVariantGetDoubleElem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out> ByRef pnVal As Double)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub PropVariantGetFileTimeElem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <Out, MarshalAs(UnmanagedType.Struct)> ByRef pftVal As System.Runtime.InteropServices.ComTypes.FILETIME)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub PropVariantGetStringElem(<[In]> propVar As PropVariant, <[In]> iElem As UInteger, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppszVal As String)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromBooleanVector(<[In], MarshalAs(UnmanagedType.LPArray)> prgf As Boolean(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromInt16Vector(<[In], Out> prgn As Int16(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromUInt16Vector(<[In], Out> prgn As UInt16(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromInt32Vector(<[In], Out> prgn As Int32(), cElems As UInteger, <Out> propVar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromUInt32Vector(<[In], Out> prgn As UInt32(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromInt64Vector(<[In], Out> prgn As Int64(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromUInt64Vector(<[In], Out> prgn As UInt64(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromDoubleVector(<[In], Out> prgn As Double(), cElems As UInteger, <Out> propvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromFileTimeVector(<[In], Out> prgft As System.Runtime.InteropServices.ComTypes.FILETIME(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub

        <DllImport("propsys.dll", CharSet:=CharSet.Unicode, SetLastError:=True, PreserveSig:=False)>
        Friend Shared Sub InitPropVariantFromStringVector(<[In], Out> prgsz As String(), cElems As UInteger, <Out> ppropvar As PropVariant)
        End Sub
    End Class


#End Region

    <ComImport>
    <Guid(ShellIIDGuid.IPropertyStoreCapabilities)>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Interface IPropertyStoreCapabilities
        Function IsPropertyWritable(<[In]> ByRef propertyKey As PropertyKey) As HResult
    End Interface

    ''' <summary>
    ''' An in-memory property store cache
    ''' </summary>
    <ComImport>
    <Guid(ShellIIDGuid.IPropertyStoreCache)>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Interface IPropertyStoreCache
        ''' <summary>
        ''' Gets the state of a property stored in the cache
        ''' </summary>
        ''' <param name="key"></param>
        ''' <param name="state"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetState(ByRef key As PropertyKey, <Out> ByRef state As PropertyStoreCacheState) As HResult

        ''' <summary>
        ''' Gets the valeu and state of a property in the cache
        ''' </summary>
        ''' <param name="propKey"></param>
        ''' <param name="pv"></param>
        ''' <param name="state"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetValueAndState(ByRef propKey As PropertyKey, <Out> pv As PropVariant, <Out> ByRef state As PropertyStoreCacheState) As HResult

        ''' <summary>
        ''' Sets the state of a property in the cache.
        ''' </summary>
        ''' <param name="propKey"></param>
        ''' <param name="state"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function SetState(ByRef propKey As PropertyKey, state As PropertyStoreCacheState) As HResult

        ''' <summary>
        ''' Sets the value and state in the cache.
        ''' </summary>
        ''' <param name="propKey"></param>
        ''' <param name="pv"></param>
        ''' <param name="state"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function SetValueAndState(ByRef propKey As PropertyKey, <[In]> pv As PropVariant, state As PropertyStoreCacheState) As HResult
    End Interface

    ''' <summary>
    ''' A property store
    ''' </summary>
    <ComImport>
    <Guid(ShellIIDGuid.IPropertyStore)>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Interface IPropertyStore
        ''' <summary>
        ''' Gets the number of properties contained in the property store.
        ''' </summary>
        ''' <param name="propertyCount"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetCount(<Out> ByRef propertyCount As UInteger) As HResult

        ''' <summary>
        ''' Get a property key located at a specific index.
        ''' </summary>
        ''' <param name="propertyIndex"></param>
        ''' <param name="key"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetAt(<[In]> propertyIndex As UInteger, ByRef key As PropertyKey) As HResult

        ''' <summary>
        ''' Gets the value of a property from the store
        ''' </summary>
        ''' <param name="key"></param>
        ''' <param name="pv"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetValue(<[In]> ByRef key As PropertyKey, <Out> pv As PropVariant) As HResult

        ''' <summary>
        ''' Sets the value of a property in the store
        ''' </summary>
        ''' <param name="key"></param>
        ''' <param name="pv"></param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), PreserveSig>
        Function SetValue(<[In]> ByRef key As PropertyKey, <[In]> pv As PropVariant) As HResult

        ''' <summary>
        ''' Commits the changes.
        ''' </summary>
        ''' <returns></returns>
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function Commit() As HResult
    End Interface

    <ComImport, Guid(ShellIIDGuid.IPropertyDescriptionList), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IPropertyDescriptionList
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetCount(ByRef pcElem As UInteger)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetAt(<[In]> iElem As UInteger, <[In]> ByRef riid As Guid, <MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IPropertyDescription)
    End Interface

    <ComImport, Guid(ShellIIDGuid.IPropertyDescription), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IPropertyDescription
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetPropertyKey(ByRef pkey As PropertyKey)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetCanonicalName(<MarshalAs(UnmanagedType.LPWStr)> ByRef ppszName As String)
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetPropertyType(ByRef pvartype As VarEnum) As HResult
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), PreserveSig>
        Function GetDisplayName(ByRef ppszName As IntPtr) As HResult
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetEditInvitation(ByRef ppszInvite As IntPtr) As HResult
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetTypeFlags(<[In]> mask As PropertyTypeOptions, ByRef ppdtFlags As PropertyTypeOptions) As HResult
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetViewFlags(ByRef ppdvFlags As PropertyViewOptions) As HResult
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetDefaultColumnWidth(ByRef pcxChars As UInteger) As HResult
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetDisplayType(ByRef pdisplaytype As PropertyDisplayType) As HResult
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetColumnState(ByRef pcsFlags As PropertyColumnStateOptions) As HResult
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetGroupingRange(ByRef pgr As PropertyGroupingRange) As HResult
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetRelativeDescriptionType(ByRef prdt As PropertySystemNativeMethods.RelativeDescriptionType)
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetRelativeDescription(<[In]> propvar1 As PropVariant, <[In]> propvar2 As PropVariant, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppszDesc1 As String, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppszDesc2 As String)
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetSortDescription(ByRef psd As PropertySortDescription) As HResult
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetSortDescriptionLabel(<[In]> fDescending As Boolean, ByRef ppszDescription As IntPtr) As HResult
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetAggregationType(ByRef paggtype As PropertyAggregationType) As HResult
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetConditionType(ByRef pcontype As PropertyConditionType, ByRef popDefault As PropertyConditionOperation) As HResult
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetEnumTypeList(<[In]> ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IPropertyEnumTypeList) As HResult
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub CoerceToCanonicalValue(<[In], Out> propvar As PropVariant)
        ' Note: this method signature may be wrong, but it is not used.
        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function FormatForDisplay(<[In]> propvar As PropVariant, <[In]> ByRef pdfFlags As PropertyDescriptionFormatOptions, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppszDisplay As String) As HResult
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function IsValueCanonical(<[In]> propvar As PropVariant) As HResult
    End Interface

    <ComImport, Guid(ShellIIDGuid.IPropertyDescription2), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IPropertyDescription2
        Inherits IPropertyDescription

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetImageReferenceForValue(<[In]> propvar As PropVariant, <Out, MarshalAs(UnmanagedType.LPWStr)> ByRef ppszImageRes As String)
    End Interface

    <ComImport, Guid(ShellIIDGuid.IPropertyEnumType), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IPropertyEnumType
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetEnumType(<Out> ByRef penumtype As PropEnumType)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetValue(<Out> ppropvar As PropVariant)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetRangeMinValue(<Out> ppropvar As PropVariant)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetRangeSetValue(<Out> ppropvar As PropVariant)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetDisplayText(<Out, MarshalAs(UnmanagedType.LPWStr)> ByRef ppszDisplay As String)
    End Interface

    <ComImport, Guid(ShellIIDGuid.IPropertyEnumType2), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IPropertyEnumType2
        Inherits IPropertyEnumType

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetImageReference(<Out, MarshalAs(UnmanagedType.LPWStr)> ByRef ppszImageRes As String)
    End Interface


    <ComImport, Guid(ShellIIDGuid.IPropertyEnumTypeList), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IPropertyEnumTypeList
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetCount(<Out> ByRef pctypes As UInteger)

        ' riid may be IID_IPropertyEnumType
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetAt(<[In]> itype As UInteger, <[In]> ByRef riid As Guid, <Out, MarshalAs(UnmanagedType.[Interface])> ByRef ppv As IPropertyEnumType)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub GetConditionAt(<[In]> index As UInteger, <[In]> ByRef riid As Guid, ByRef ppv As IntPtr)

        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Sub FindMatchingIndex(<[In]> propvarCmp As PropVariant, <Out> ByRef pnIndex As UInteger)
    End Interface

#End Region

#End Region    '' End of Windows API Code Pack translation.


#Region "HResult"

    Public Enum HResult
        ''' <summary>
        ''' S_OK
        ''' </summary>
        Ok = &H0I

        ''' <summary>
        ''' S_FALSE
        ''' </summary>
        [False] = &H1I

        ''' <summary>
        ''' E_INVALIDARG
        ''' </summary>
        InvalidArguments = CInt(&H80070057I)

        ''' <summary>
        ''' E_OUTOFMEMORY
        ''' </summary>
        OutOfMemory = CInt(&H8007000EI)

        ''' <summary>
        ''' E_NOINTERFACE
        ''' </summary>
        NoInterface = CInt(&H80004002I)

        ''' <summary>
        ''' E_FAIL
        ''' </summary>
        Fail = CInt(&H80004005I)

        ''' <summary>
        ''' E_ELEMENTNOTFOUND
        ''' </summary>
        ElementNotFound = CInt(&H80070490I)

        ''' <summary>
        ''' TYPE_E_ELEMENTNOTFOUND
        ''' </summary>
        TypeElementNotFound = CInt(&H8002802BI)

        ''' <summary>
        ''' NO_OBJECT
        ''' </summary>
        NoObject = CInt(&H800401E5I)

        ''' <summary>
        ''' Win32 Error code: ERROR_CANCELLED
        ''' </summary>
        Win32ErrorCanceled = 1223I

        ''' <summary>
        ''' ERROR_CANCELLED
        ''' </summary>
        Canceled = CInt(&H800704C7I)

        ''' <summary>
        ''' The requested resource is in use
        ''' </summary>
        ResourceInUse = CInt(&H800700AAI)

        ''' <summary>
        ''' The requested resource is read-only.
        ''' </summary>
        AccessDenied = CInt(&H80030005I)
    End Enum

#End Region    '' This is HResult.  It is everyone's friend.


    '' This code was written, from the ground, up, by me (Nathan Moschkin)
#Region "File Association Handlers"

    <ComImport, Guid("92218CAB-ECAA-4335-8133-807FD234C2EE"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComConversionLoss>
    Public Interface IAssocHandlerInvoker

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function SupportsSelection() As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function Invoke() As HResult

    End Interface

    <ComImport, Guid("F04061AC-1659-4a3f-A954-775AA57FC083"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComConversionLoss>
    Public Interface IAssocHandler

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetName(<MarshalAs(UnmanagedType.LPWStr), Out> ByRef ppsz As String) As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetUIName(<MarshalAs(UnmanagedType.LPWStr), Out> ByRef ppsz As String) As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function GetIconLocation(<MarshalAs(UnmanagedType.LPWStr), Out> ByRef ppszPath As String, <Out> ByRef pIndex As Integer) As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function IsRecommended() As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function MakeDefault(<MarshalAs(UnmanagedType.LPWStr), [In]> pszDescription As String) As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function Invoke(<[In]> pdo As IDataObject) As HResult

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function CreateInvoker(<[In]> pdo As IDataObject, <Out> ByRef ppInvoker As IAssocHandlerInvoker) As HResult

    End Interface

    <ComImport, Guid("973810ae-9599-4b88-9e4d-6ee98c9552da"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IEnumAssocHandlers

        <PreserveSig>
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime)>
        Function [Next](<[In]> celt As UInteger, <Out> ByRef rgelt As IAssocHandler, <Out> ByRef pceltFetched As UInteger) As HResult

    End Interface


#End Region

#End Region


    ''    End Module

End Namespace