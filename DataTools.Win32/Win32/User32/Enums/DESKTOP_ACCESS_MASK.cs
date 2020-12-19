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
using DataTools.Win32;

namespace DataTools.Win32
{
    public enum DESKTOP_ACCESS_MASK
    {

        /// <summary>
        /// Required to delete the object.
        /// </summary>
        [Description("Required to delete the object.")]
        DELETE = 0x10000,
        /// <summary>
        /// Required to read information in the security descriptor for the object, not including the information in the SACL. To read or write the SACL, you must request the ACCESS_SYSTEM_SECURITY access right. For more information, see SACL Access Right.
        /// </summary>
        [Description("Required to read information in the security descriptor for the object, not including the information in the SACL. To read or write the SACL, you must request the ACCESS_SYSTEM_SECURITY access right. For more information, see SACL Access Right.")]
        READ_CONTROL = 0x20000,
        /// <summary>
        /// Not supported for desktop objects.
        /// </summary>
        [Description("Not supported for desktop objects.")]
        SYNCHRONIZE = 0x100000,
        /// <summary>
        /// Required to modify the DACL in the security descriptor for the object.
        /// </summary>
        [Description("Required to modify the DACL in the security descriptor for the object.")]
        WRITE_DAC = 0x40000,

        /// <summary>
        /// Required to change the owner in the security descriptor for the object.
        /// </summary>
        [Description("Required to change the owner in the security descriptor for the object.")]
        WRITE_OWNER = 0x80000,

        // The following table lists the object-specific access rights.
        // Access right	Description

        /// <summary>
        /// Required to create a menu on the desktop.
        /// </summary>
        [Description("Required to create a menu on the desktop.")]
        DESKTOP_CREATEMENU = 0x4,

        /// <summary>
        /// Required to create a window on the desktop.
        /// </summary>
        [Description("Required to create a window on the desktop.")]
        DESKTOP_CREATEWINDOW = 0x2,

        /// <summary>
        /// Required for the desktop to be enumerated.
        /// </summary>
        [Description("Required for the desktop to be enumerated.")]
        DESKTOP_ENUMERATE = 0x40,

        /// <summary>
        /// Required to establish any of the window hooks.
        /// </summary>
        [Description("Required to establish any of the window hooks.")]
        DESKTOP_HOOKCONTROL = 0x8,

        /// <summary>
        /// Required to perform journal playback on a desktop.
        /// </summary>
        [Description("Required to perform journal playback on a desktop.")]
        DESKTOP_JOURNALPLAYBACK = 0x20,

        /// <summary>
        /// Required to perform journal recording on a desktop.
        /// </summary>
        [Description("Required to perform journal recording on a desktop.")]
        DESKTOP_JOURNALRECORD = 0x10,

        /// <summary>
        /// Required to read objects on the desktop.
        /// </summary>
        [Description("Required to read objects on the desktop.")]
        DESKTOP_READOBJECTS = 0x1,

        /// <summary>
        /// Required to activate the desktop using the SwitchDesktop function.
        /// </summary>
        [Description("Required to activate the desktop using the SwitchDesktop function.")]
        DESKTOP_SWITCHDESKTOP = 0x100,

        /// <summary>
        /// Required to write objects on the desktop.
        /// </summary>
        [Description("Required to write objects on the desktop.")]
        DESKTOP_WRITEOBJECTS = 0x80,

        // The following are the generic access rights for a desktop object contained in the interactive window station of the user's logon session.
        // Access right	Description

        GENERIC_READ = DESKTOP_ENUMERATE | DESKTOP_READOBJECTS | IO.STANDARD_RIGHTS_READ,
        GENERIC_WRITE = DESKTOP_CREATEMENU | DESKTOP_CREATEWINDOW | DESKTOP_HOOKCONTROL | DESKTOP_JOURNALPLAYBACK | DESKTOP_JOURNALRECORD | DESKTOP_WRITEOBJECTS | IO.STANDARD_RIGHTS_WRITE,
        GENERIC_EXECUTE = DESKTOP_SWITCHDESKTOP | IO.STANDARD_RIGHTS_EXECUTE,
        GENERIC_ALL = DESKTOP_CREATEMENU | DESKTOP_CREATEWINDOW | DESKTOP_ENUMERATE | DESKTOP_HOOKCONTROL | DESKTOP_JOURNALPLAYBACK | DESKTOP_JOURNALRECORD | DESKTOP_READOBJECTS | DESKTOP_SWITCHDESKTOP | DESKTOP_WRITEOBJECTS | IO.STANDARD_RIGHTS_REQUIRED
    }
}
