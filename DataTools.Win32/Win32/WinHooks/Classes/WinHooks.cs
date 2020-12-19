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
    public delegate IntPtr WinHookCallback(int nCode, IntPtr wParam, IntPtr lParam);

    public delegate IntPtr ShellCallback(ShellHookCodes nCode, IntPtr wParam, IntPtr lParam);

    internal static class WinHooks
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(WindowHookTypes idHook, WinHookCallback lpfn, IntPtr hmod, int dwThreadId);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(WindowHookTypes idHook, ShellCallback lpfn, IntPtr hmod, int dwThreadId);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hHook, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hHook);



        // HHOOK SetWindowsHookExA(
        // Int       idHook,
        // HOOKPROC  lpfn,
        // HINSTANCE hmod,
        // DWORD     dwThreadId
        // );

    }
}
