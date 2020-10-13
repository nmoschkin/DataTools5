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
// Licensed Under the Microsoft Public License   
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
    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Stores information about how to sort a column that is displayed in the folder view.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SortColumn
    {

        /// <summary>
        /// Creates a sort column with the specified direction for the given property.
        /// </summary>
        /// <param name="propertyKey">Property key for the property that the user will sort.</param>
        /// <param name="direction">The direction in which the items are sorted.</param>
        public SortColumn(PropertyKey propertyKey, SortDirection direction)
        {
            m_propertyKey = propertyKey;
            m_direction = direction;
        }

        /// <summary>
        /// The ID of the column by which the user will sort. A PropertyKey structure.
        /// For example, for the "Name" column, the property key is PKEY_ItemNameDisplay or
        /// PropertySystem.SystemProperties.System.ItemName.
        /// </summary>
        public PropertyKey PropertyKey
        {
            get
            {
                return m_propertyKey;
            }

            set
            {
                m_propertyKey = value;
            }
        }

        private PropertyKey m_propertyKey;

        /// <summary>
        /// The direction in which the items are sorted.
        /// </summary>
        public SortDirection Direction
        {
            get
            {
                return m_direction;
            }

            set
            {
                m_direction = value;
            }
        }

        private SortDirection m_direction;


        /// <summary>
        /// Implements the == (equality) operator.
        /// </summary>
        /// <param name="col1">First object to compare.</param>
        /// <param name="col2">Second object to compare.</param>
        /// <returns>True if col1 equals col2; false otherwise.</returns>
        public static bool operator ==(SortColumn col1, SortColumn col2)
        {
            return col1.Direction == col2.Direction && col1.PropertyKey == col2.PropertyKey;
        }

        /// <summary>
        /// Implements the != (unequality) operator.
        /// </summary>
        /// <param name="col1">First object to compare.</param>
        /// <param name="col2">Second object to compare.</param>
        /// <returns>True if col1 does not equals col1; false otherwise.</returns>
        public static bool operator !=(SortColumn col1, SortColumn col2)
        {
            return !(col1 == col2);
        }

        /// <summary>
        /// Determines if this object is equal to another.
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns>Returns true if the objects are equal; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is null || !ReferenceEquals(obj.GetType(), typeof(SortColumn)))
            {
                return false;
            }

            return this == (SortColumn)obj;
        }

        /// <summary>
        /// Generates a nearly unique hashcode for this structure.
        /// </summary>
        /// <returns>A hash code.</returns>
        public override int GetHashCode()
        {
            int hash = m_direction.GetHashCode();
            hash = hash * 31 + m_propertyKey.GetHashCode();
            return hash;
        }
    }
}
