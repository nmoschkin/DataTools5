// ************************************************* ''
// DataTools C# Native Utility Library For Windows 
//
// Module: SystemInfo
//         Provides basic information about the
//         current computer.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the MIT License   
// ************************************************* ''


namespace DataTools.SystemInformation
{
    /// <summary>
    /// Processor architecture type.
    /// </summary>
    public enum ArchitectureType : short
    {

        /// <summary>
        /// 32-bit system.
        /// </summary>
        /// <remarks></remarks>
        x86 = 0,

        /// <summary>
        /// Iatium-based system.
        /// </summary>
        /// <remarks></remarks>
        IA64 = 6,

        /// <summary>
        /// 64-bit system.
        /// </summary>
        /// <remarks></remarks>
        x64 = 9
    }
}
