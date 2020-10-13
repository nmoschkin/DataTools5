// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: Very low level disk partition information
//         Utility library.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DataTools.Text;
using DataTools.Hardware.Native;
using DataTools.Hardware.Disk.Partitions.Gpt;

namespace DataTools.Hardware.Disk.Partitions
{

    
    /// <summary>
    /// Sorter for PartitionCodeInfo.  Sorts Windows NT specific implementations
    /// of MBR partition codes first.
    /// </summary>
    /// <remarks></remarks>
    internal class PartCodeSorter : IComparer<PartitionCodeInfo>
    {
        private List<string> los1 = new List<string>();
        private List<string> los2 = new List<string>();

        /// <summary>
        /// Compare two PartitionCodeInfo classes by Partition ID and Operating System.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Compare(PartitionCodeInfo x, PartitionCodeInfo y)
        {
            los1.Clear();
            los2.Clear();
            if (x.SupporedOSes is object)
                los1.AddRange(x.SupporedOSes);
            if (y.SupporedOSes is object)
                los2.AddRange(y.SupporedOSes);
            if (x.PartitionID.Value == y.PartitionID.Value)
            {
                if (los1.Count == 1 && los1[0] == "Windows NT" && los2.Count == 1 && los2[0] == "Windows NT")
                {
                    return string.Compare(x.Description, y.Description);
                }
                else if (los1.Count == 1 && los1[0] == "Windows NT")
                {
                    return -1;
                }
                else if (los2.Count == 1 && los2[0] == "Windows NT")
                {
                    return 1;
                }
                else if (los1.Contains("Windows NT") && los2.Contains("Windows NT"))
                {
                    return string.Compare(x.Description, y.Description);
                }
                else if (los1.Contains("Windows NT"))
                {
                    return -1;
                }
                else if (los2.Contains("Windows NT"))
                {
                    return 1;
                }
                else
                {
                    return string.Compare(x.Description, y.Description);
                }
            }
            else
            {
                return x.PartitionID.Value - y.PartitionID.Value;
            }
        }
    }

}