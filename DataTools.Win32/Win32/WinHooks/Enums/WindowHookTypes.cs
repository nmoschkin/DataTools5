// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: WinHooks
//         Windows Hook Functions
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
using System.Runtime.InteropServices;

namespace DataTools.Win32
{
    public enum WindowHookTypes
    {

        /// <summary>
        /// Installs a hook procedure that monitors messages before the system sends them To the destination window procedure. For more information, see the CallWndProc hook procedure.
        /// </summary>
        CallWndProc = 4,

        /// <summary>
        /// Installs a hook procedure that monitors messages after they have been processed by the destination window procedure. For more information, see the CallWndRetProc hook procedure.
        /// </summary>
        CallWndProcReturning = 12,

        /// <summary>
        /// Installs a hook procedure that receives notifications useful To a CBT application. For more information, see the CBTProc hook procedure.
        /// </summary>
        Cbt = 5,

        /// <summary>
        /// Installs a hook procedure useful For debugging other hook procedures. For more information, see the DebugProc hook procedure.
        /// </summary>
        Debug = 9,

        /// <summary>
        /// Installs a hook procedure that will be called When the application's foreground thread is about to become idle. This hook is useful for performing low priority tasks during idle time. For more information, see the ForegroundIdleProc hook procedure.
        /// </summary>
        ForegroundIdle = 11,

        /// <summary>
        /// Installs a hook procedure that monitors messages posted To a message queue. For more information, see the GetMsgProc hook procedure.
        /// </summary>
        GetMessage = 3,

        /// <summary>
        /// Installs a hook procedure that posts messages previously recorded by a WH_JOURNALRECORD hook procedure. For more information, see the JournalPlaybackProc hook procedure.
        /// </summary>
        JournalPlayback = 1,

        /// <summary>
        /// Installs a hook procedure that records input messages posted To the system message queue. This hook Is useful For recording macros. For more information, see the JournalRecordProc hook procedure.
        /// </summary>
        JournalRecord = 0,

        /// <summary>
        /// Installs a hook procedure that monitors keystroke messages. For more information, see the KeyboardProc hook procedure.
        /// </summary>
        Keyboard = 2,

        /// <summary>
        /// Installs a hook procedure that monitors low-level keyboard input events. For more information, see the LowLevelKeyboardProc hook procedure.
        /// </summary>
        KeyboardLowLevel = 13,

        /// <summary>
        /// Installs a hook procedure that monitors mouse messages. For more information, see the MouseProc hook procedure.
        /// </summary>
        Mouse = 7,

        /// <summary>
        /// Installs a hook procedure that monitors low-level mouse input events. For more information, see the LowLevelMouseProc hook procedure.
        /// </summary>
        MouseLowLevel = 14,

        /// <summary>
        /// Installs a hook procedure that monitors messages generated As a result Of an input Event In a dialog box, message box, menu, Or scroll bar. For more information, see the MessageProc hook procedure.
        /// </summary>
        MessageFilter = -1,

        /// <summary>
        /// Installs a hook procedure that receives notifications useful To shell applications. For more information, see the ShellProc hook procedure.
        /// </summary>
        Shell = 10,

        /// <summary>
        /// Installs a hook procedure that monitors messages generated As a result Of an input Event In a dialog box, message box, menu, Or scroll bar. The hook procedure monitors these messages For all applications In the same desktop As the calling thread. For more information, see the SysMsgProc hook procedure.
        /// </summary>
        SystemMessageFilter = 6
    }
}
