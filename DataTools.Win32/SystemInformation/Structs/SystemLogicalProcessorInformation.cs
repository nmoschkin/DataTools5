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
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Navigation;
using DataTools.Memory;
using DataTools.Text;

namespace DataTools.SystemInformation
{
    /// <summary>
    /// Logical processor information stucture.  Contains information about a logical processor on the local machine.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct SystemLogicalProcessorInformation
    {
        public override string ToString()
        {
            string s;

            if (Relationship == LogicalProcessorRelationship.RelationCache)
            {
                s = $"L{CacheDescriptor.Level} Cache, {TextTools.PrintFriendlySize(CacheDescriptor.Size)} ({GetProcMaskInfoString()})";
                return s;
            }
            else if (Relationship == LogicalProcessorRelationship.RelationProcessorCore)
            {
                return $"Processor Core ({GetProcMaskInfoString()})";
            }
            else if (Relationship == LogicalProcessorRelationship.RelationProcessorPackage)
            {
                return "Processor Package";
            }
            else if (Relationship == LogicalProcessorRelationship.RelationNumaNode)
            {
                return $"Numa Node {NodeNumber} ({GetProcMaskInfoString()})";
            }

            return base.ToString();
        }

        private string GetProcMaskInfoString()
        {
            long m = ProcessorMask;
            string s = "";

            int c = 0;

            while (m != 0)
            {
                if ((m & 1) == 1)
                {
                    if (s != "") s += ", ";
                    s += (c + 1).ToString();
                }

                m >>= 1;
                c++;
            }

            s = "Processors " + s;
            return s;
        }

        /// <summary>
        /// Processor mask
        /// </summary>
        [FieldOffset(0)]
        public readonly long ProcessorMask;

        /// <summary>
        /// Processor relationship (entity kind)
        /// </summary>

        [FieldOffset(8)]
        public readonly LogicalProcessorRelationship Relationship;

        /// <summary>
        /// Flags
        /// </summary>
        [FieldOffset(16)]
        public readonly byte Flags;

        /// <summary>
        /// Node number
        /// </summary>
        [FieldOffset(16)]
        public readonly int NodeNumber;


        /// <summary>
        /// Cache descriptor
        /// </summary>
        [FieldOffset(16)]
        public readonly ProcessorCacheDescriptor CacheDescriptor;

        /// <summary>
        /// Reserved 1
        /// </summary>
        [FieldOffset(16)]
        internal readonly long Reserved1;

        /// <summary>
        /// Reserved 2
        /// </summary>
        [FieldOffset(24)]
        internal readonly long Reserved2;

    }
}
