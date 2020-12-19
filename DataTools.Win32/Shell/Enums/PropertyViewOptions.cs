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
}
