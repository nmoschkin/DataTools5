using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCT.Shell32.Enums
{

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
        NoInterface = (0x80004002),

        /// <summary>
        /// E_FAIL
        /// </summary>
        Fail = (0x80004005),

        /// <summary>
        /// E_ELEMENTNOTFOUND
        /// </summary>
        ElementNotFound = (0x80070490),

        /// <summary>
        /// TYPE_E_ELEMENTNOTFOUND
        /// </summary>
        TypeElementNotFound = (0x8002802B),

        /// <summary>
        /// NO_OBJECT
        /// </summary>
        NoObject = (0x800401E5),

        /// <summary>
        /// Win32 Error code: ERROR_CANCELLED
        /// </summary>
        Win32ErrorCanceled = 1223,

        /// <summary>
        /// ERROR_CANCELLED
        /// </summary>
        Canceled = (0x800704C7),

        /// <summary>
        /// The requested resource is in use
        /// </summary>
        ResourceInUse = (0x800700AA),

        /// <summary>
        /// The requested resource is read-only.
        /// </summary>
        AccessDenied = (0x80030005)
    }



    /// <summary>
    ///     ''' CommonFileDialog AddPlace locations
    ///     ''' </summary>    
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
    ///     ''' One of the values that indicates how the ShellObject DisplayName should look.
    ///     ''' </summary>
    public enum DisplayNameType : uint
    {
        /// <summary>
        /// Returns the display name relative to the desktop.
        /// </summary>
        Default = 0x0,

        /// <summary>
        /// Returns the parsing name relative to the parent folder.
        /// </summary>
        RelativeToParent = (0x80018001),

        /// <summary>
        /// Returns the path relative to the parent folder in a 
        /// friendly format as displayed in an address bar.
        /// </summary>
        RelativeToParentAddressBar = (0x8007C001),

        /// <summary>
        /// Returns the parsing name relative to the desktop.
        /// </summary>
        RelativeToDesktop = (0x80028000),

        /// <summary>
        /// Returns the editing name relative to the parent folder.
        /// </summary>
        RelativeToParentEditing = (0x80031001),

        /// <summary>
        /// Returns the editing name relative to the desktop.
        /// </summary>
        RelativeToDesktopEditing = (0x8004C000),

        /// <summary>
        /// Returns the display name relative to the file system path.
        /// </summary>
        FileSystemPath = (0x80058000),

        /// <summary>
        /// Returns the display name relative to a URL.
        /// </summary>
        Url = (0x80068000)
    }
    /// <summary>
    ///     ''' Available Library folder types
    ///     ''' </summary>
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
    ///     ''' Flags controlling the appearance of a window
    ///     ''' </summary>
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
    ///     ''' Provides a set of flags to be used with SearchCondition
    ///     ''' to indicate the operation in SearchConditionFactory's methods.
    ///     ''' </summary>
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
    ///     ''' Set of flags to be used with SearchConditionFactory.
    ///     ''' </summary>
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
    ///     ''' Used to describe the view mode.
    ///     ''' </summary>
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
    ///     ''' The direction in which the items are sorted.
    ///     ''' </summary>
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
    ///     ''' Provides a set of flags to be used with IQueryParser::SetOption and 
    ///     ''' IQueryParser::GetOption to indicate individual options.
    ///     ''' </summary>
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
    ///     ''' Provides a set of flags to be used with IQueryParser::SetMultiOption 
    ///     ''' to indicate individual options.
    ///     ''' </summary>
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
    ///     ''' Used by IQueryParserManager::SetOption to set parsing options. 
    ///     ''' This can be used to specify schemas and localization options.
    ///     ''' </summary>
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
        ParentRelativeParsing = (0x80018001),
        // SIGDN_INFOLDER | SIGDN_FORPARSING
        DesktopAbsoluteParsing = (0x80028000),
        // SIGDN_FORPARSING
        ParentRelativeEditing = (0x80031001),
        // SIGDN_INFOLDER | SIGDN_FOREDITING
        DesktopAbsoluteEditing = (0x8004C000),
        // SIGDN_FORPARSING | SIGDN_FORADDRESSBAR
        FileSystemPath = (0x80058000),
        // SIGDN_FORPARSING
        Url = (0x80068000),
        // SIGDN_FORPARSING
        ParentRelativeForAddressBar = (0x8007C001),
        // SIGDN_INFOLDER | SIGDN_FORPARSING | SIGDN_FORADDRESSBAR
        ParentRelative = (0x80080001)
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
        SHOP_PRINTERNAME = 0x1 // ' lpObject points To a printer friendly name
    ,
        SHOP_FILEPATH = 0x2 // ' lpObject points To a fully qualified path+file name
    ,
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
        HasSubFolder = (0x80000000),

        /// <summary>
        /// This flag is a mask for the contents attributes.
        /// </summary>
        ContentsMask = (0x80000000),

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
        SICHINT_ALLFIELDS = (0x80000000)
    }


    /// <summary>
    ///     ''' Thumbnail Alpha Types
    ///     ''' </summary>
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
    ///     ''' Describes the event that has occurred. 
    ///     ''' Typically, only one event is specified at a time. 
    ///     ''' If more than one event is specified, 
    ///     ''' the values contained in the dwItem1 and dwItem2 parameters must be the same, 
    ///     ''' respectively, for all specified events. 
    ///     ''' This parameter can be one or more of the following values:
    ///     ''' </summary>
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
        FromInterrupt = (0x80000000)
    }


}