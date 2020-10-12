// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library - Interop
// '
// ' Module: NativeShell
// '         Wrappers for native and COM shell interfaces.
// '
// ' Some enum documentation copied from the MSDN (and in some cases, updated).
// ' Some classes and interfaces were ported from the WindowsAPICodePack.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

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
    // Summary:
    // Defines a unique key for a Shell Property
    public struct PropertyKey : IEquatable<PropertyKey>
    {
        // 
        // Summary:
        // PropertyKey Constructor
        // 
        // Parameters:
        // formatId:
        // A unique GUID for the property
        // 
        // propertyId:
        // Property identifier (PID)
        public PropertyKey(Guid formatId, int propertyId)
        {
            _FormatId = formatId;
            _PropertyId = propertyId;
        }
        // 
        // Summary:
        // PropertyKey Constructor
        // 
        // Parameters:
        // formatId:
        // A string represenstion of a GUID for the property
        // 
        // propertyId:
        // Property identifier (PID)
        public PropertyKey(string formatId, int propertyId)
        {
            _FormatId = new Guid(formatId);
            _PropertyId = propertyId;
        }

        // Summary:
        // Implements the != (inequality) operator.
        // 
        // Parameters:
        // propKey1:
        // First property key to compare
        // 
        // propKey2:
        // Second property key to compare.
        // 
        // Returns:
        // true if object a does not equal object b. false otherwise.
        public static bool operator !=(PropertyKey propKey1, PropertyKey propKey2)
        {
            return !propKey1.Equals(propKey2);
        }
        // 
        // Summary:
        // Implements the == (equality) operator.
        // 
        // Parameters:
        // propKey1:
        // First property key to compare.
        // 
        // propKey2:
        // Second property key to compare.
        // 
        // Returns:
        // true if object a equals object b. false otherwise.
        public static bool operator ==(PropertyKey propKey1, PropertyKey propKey2)
        {
            return propKey1.Equals(propKey2);
        }

        // Summary:
        // A unique GUID for the property
        private Guid _FormatId;

        public Guid FormatId
        {
            get
            {
                return _FormatId;
            }
        }
        // 
        // Summary:
        // Property identifier (PID)
        private int _PropertyId;

        public int PropertyId
        {
            get
            {
                return _PropertyId;
            }
        }

        // Summary:
        // Returns whether this object is equal to another. This is vital for performance
        // of value types.
        // 
        // Parameters:
        // obj:
        // The object to compare against.
        // 
        // Returns:
        // Equality result.
        public override bool Equals(object obj)
        {
            return Equals((PropertyKey)obj);
        }
        // 
        // Summary:
        // Returns whether this object is equal to another. This is vital for performance
        // of value types.
        // 
        // Parameters:
        // other:
        // The object to compare against.
        // 
        // Returns:
        // Equality result.
        public bool Equals(PropertyKey other)
        {
            if (other.FormatId != _FormatId)
                return false;
            if (other.PropertyId != _PropertyId)
                return false;
            return true;
        }
        // 
        // Summary:
        // Returns the hash code of the object. This is vital for performance of value
        // types.
        public override int GetHashCode()
        {
            var i = default(int);
            var b = _FormatId.ToByteArray();
            foreach (var by in b)
                i += by;
            i += _PropertyId;
            return i;
        }
        // 
        // Summary:
        // Override ToString() to provide a user friendly string representation
        // 
        // Returns:
        // String representing the property key
        public override string ToString()
        {
            return _FormatId.ToString("B").ToUpper() + "[" + _PropertyId + "]";
        }
    }
}
