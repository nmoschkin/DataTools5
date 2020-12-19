// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: DevProp
//         Native Device Properites.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using DataTools.Text;

namespace DataTools.Win32
{
    /// <summary>
    /// Device property types.
    /// </summary>
    /// <remarks></remarks>
    public enum DevPropTypes
    {
        /// <summary>
        /// nothing, no property data
        /// </summary>
        [Description("nothing, no property data")]
        Empty = DevProp.DEVPROP_TYPE_EMPTY,

        /// <summary>
        /// null property data
        /// </summary>
        [Description("null property data")]
        Null = DevProp.DEVPROP_TYPE_NULL,

        /// <summary>
        /// 8-bit signed int (SBYTE)
        /// </summary>
        [Description("8-bit signed int (SBYTE)")]
        SByte = DevProp.DEVPROP_TYPE_SBYTE,

        /// <summary>
        /// 8-bit unsigned int (BYTE)
        /// </summary>
        [Description("8-bit unsigned int (BYTE)")]
        Byte = DevProp.DEVPROP_TYPE_BYTE,

        /// <summary>
        /// 16-bit signed int (SHORT)
        /// </summary>
        [Description("16-bit signed int (SHORT)")]
        Int16 = DevProp.DEVPROP_TYPE_INT16,

        /// <summary>
        /// 16-bit unsigned int (USHORT)
        /// </summary>
        [Description("16-bit unsigned int (USHORT)")]
        UInt16 = DevProp.DEVPROP_TYPE_UINT16,

        /// <summary>
        /// 32-bit signed int (LONG)
        /// </summary>
        [Description("32-bit signed int (LONG)")]
        Int32 = DevProp.DEVPROP_TYPE_INT32,

        /// <summary>
        /// 32-bit unsigned int (ULONG)
        /// </summary>
        [Description("32-bit unsigned int (ULONG)")]
        UInt32 = DevProp.DEVPROP_TYPE_UINT32,

        /// <summary>
        /// 64-bit signed int (LONG64)
        /// </summary>
        [Description("64-bit signed int (LONG64)")]
        Int64 = DevProp.DEVPROP_TYPE_INT64,

        /// <summary>
        /// 64-bit unsigned int (ULONG64)
        /// </summary>
        [Description("64-bit unsigned int (ULONG64)")]
        UInt64 = DevProp.DEVPROP_TYPE_UINT64,

        /// <summary>
        /// 32-bit floating-point (FLOAT)
        /// </summary>
        [Description("32-bit floating-point (FLOAT)")]
        Float = DevProp.DEVPROP_TYPE_FLOAT,

        /// <summary>
        /// 64-bit floating-point (DOUBLE)
        /// </summary>
        [Description("64-bit floating-point (DOUBLE)")]
        Double = DevProp.DEVPROP_TYPE_DOUBLE,

        /// <summary>
        /// 128-bit data (DECIMAL)
        /// </summary>
        [Description("128-bit data (DECIMAL)")]
        Decimal = DevProp.DEVPROP_TYPE_DECIMAL,

        /// <summary>
        /// 128-bit unique identifier (GUID)
        /// </summary>
        [Description("128-bit unique identifier (GUID)")]
        Guid = DevProp.DEVPROP_TYPE_GUID,

        /// <summary>
        /// 64 bit signed int currency value (CURRENCY)
        /// </summary>
        [Description("64 bit signed int currency value (CURRENCY)")]
        Currency = DevProp.DEVPROP_TYPE_CURRENCY,

        /// <summary>
        /// date (DATE)
        /// </summary>
        [Description("date (DATE)")]
        Date = DevProp.DEVPROP_TYPE_DATE,

        /// <summary>
        /// file time (FILETIME)
        /// </summary>
        [Description("file time (FILETIME)")]
        FileTime = DevProp.DEVPROP_TYPE_FILETIME,

        /// <summary>
        /// 8-bit boolean = (DEVPROP_BOOLEAN)
        /// </summary>
        [Description("8-bit boolean = (DEVPROP_BOOLEAN)")]
        Boolean = DevProp.DEVPROP_TYPE_BOOLEAN,

        /// <summary>
        /// null-terminated string
        /// </summary>
        [Description("null-terminated string")]
        String = DevProp.DEVPROP_TYPE_STRING,

        /// <summary>
        /// multi-sz string list
        /// </summary>
        [Description("multi-sz string list")]
        StringList = DevProp.DEVPROP_TYPE_STRING | DevProp.DEVPROP_TYPEMOD_LIST,

        /// <summary>
        /// self-relative binary SECURITY_DESCRIPTOR
        /// </summary>
        [Description("self-relative binary SECURITY_DESCRIPTOR")]
        SecurityDescriptor = DevProp.DEVPROP_TYPE_SECURITY_DESCRIPTOR,

        /// <summary>
        /// security descriptor string (SDDL format)
        /// </summary>
        [Description("security descriptor string (SDDL format)")]
        SecurityDescriptorString = DevProp.DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING,

        /// <summary>
        /// device property key = (DEVPROPKEY)
        /// </summary>
        [Description("device property key = (DEVPROPKEY)")]
        DevPropKey = DevProp.DEVPROP_TYPE_DEVPROPKEY,

        /// <summary>
        /// device property type = (DEVPROPTYPE)
        /// </summary>
        [Description("device property type = (DEVPROPTYPE)")]
        DevPropType = DevProp.DEVPROP_TYPE_DEVPROPTYPE,

        /// <summary>
        /// custom binary data
        /// </summary>
        [Description("custom binary data")]
        Binary = DevProp.DEVPROP_TYPE_BYTE | DevProp.DEVPROP_TYPEMOD_ARRAY,

        /// <summary>
        /// 32-bit Win32 system error code
        /// </summary>
        [Description("32-bit Win32 system error code")]
        Error = DevProp.DEVPROP_TYPE_ERROR,

        /// <summary>
        /// 32-bit NTSTATUS code
        /// </summary>
        [Description("32-bit NTSTATUS code")]
        NTStatus = DevProp.DEVPROP_TYPE_NTSTATUS,

        /// <summary>
        /// string resource (@[path\]&lt;dllname&gt;,-&lt;strId&gt;)
        /// </summary>
        [Description(@"string resource (@[path\]<dllname>,-<strId>)")]
        StringIndirect = DevProp.DEVPROP_TYPE_STRING_INDIRECT,

        /// <summary>
        /// Array property key type modifier.
        /// </summary>
        /// <remarks></remarks>
        Array = DevProp.DEVPROP_TYPEMOD_ARRAY,

        /// <summary>
        /// List property key type modifier.
        /// </summary>
        /// <remarks></remarks>
        List = DevProp.DEVPROP_TYPEMOD_LIST
    }
}
