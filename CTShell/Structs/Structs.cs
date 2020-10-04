using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CoreCT.Shell32.Structs
{
    public enum SortDirection
    {
        Ascending,
        Descending
    }

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
            this.m_propertyKey = propertyKey;
            this.m_direction = direction;
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
            return (col1.Direction == col2.Direction) && (col1.PropertyKey == col2.PropertyKey);
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
            if (obj == null || obj.GetType() != typeof(SortColumn))
                return false;
            return (this == (SortColumn)obj);
        }

        /// <summary>
        /// Generates a nearly unique hashcode for this structure.
        /// </summary>
        /// <returns>A hash code.</returns>
        public override int GetHashCode()
        {
            int hash = this.m_direction.GetHashCode();
            hash = hash * 31 + this.m_propertyKey.GetHashCode();
            return hash;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct ThumbnailId
    {
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 16)]
        private byte rgbKey;
    }

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
            {
                var withBlock = other;
                if (withBlock.FormatId != _FormatId)
                    return false;
                if (withBlock.PropertyId != _PropertyId)
                    return false;
            }

            return true;
        }
        // 
        // Summary:
        // Returns the hash code of the object. This is vital for performance of value
        // types.
        public override int GetHashCode()
        {
            int i = 0;
            byte[] b = _FormatId.ToByteArray();

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
