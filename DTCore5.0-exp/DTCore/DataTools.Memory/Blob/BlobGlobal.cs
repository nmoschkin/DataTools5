// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// '
// ' Module: Blob
// '         Global Utility Methods and Enums
// '         
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.ComponentModel;
using DataTools.Memory.Internal;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.Memory
{

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    static class BlobUtil
    {
        public static object BytesToEnum(byte[] b, Type t)
        {
            SafePtr sp = b;
            int x = Blob.BlobTypeSize(Blob.TypeToBlobType(t.GetEnumUnderlyingType()));
            switch (x)
            {
                case 1:
                    {
                        if (Native.Unsigned(t))
                        {
                            return Enum.ToObject(t, sp[0L]);
                        }
                        else
                        {
                            return Enum.ToObject(t, sp.get_SByteAt(0L));
                        }

                        break;
                    }

                case 2:
                    {
                        if (Native.Unsigned(t))
                        {
                            return Enum.ToObject(t, sp.get_UShortAt(0L));
                        }
                        else
                        {
                            return Enum.ToObject(t, sp.get_ShortAt(0L));
                        }

                        break;
                    }

                case 4:
                    {
                        if (Native.Unsigned(t))
                        {
                            return Enum.ToObject(t, sp.get_UIntegerAt(0L));
                        }
                        else
                        {
                            return Enum.ToObject(t, sp.get_IntegerAt(0L));
                        }

                        break;
                    }

                case 8:
                    {
                        if (Native.Unsigned(t))
                        {
                            return Enum.ToObject(t, sp.get_ULongAt(0L));
                        }
                        else
                        {
                            return Enum.ToObject(t, sp.get_LongAt(0L));
                        }

                        break;
                    }
            }

            return null;
        }

        public static byte[] EnumToBytes(object val)
        {
            SafePtr sp = null;
            var t = val.GetType();
            int x = Blob.BlobTypeSize(Blob.TypeToBlobType(t.GetEnumUnderlyingType()));
            switch (x)
            {
                case 1:
                    {
                        if (Native.Unsigned(t))
                        {
                            sp = Conversions.ToByte(val);
                        }
                        else
                        {
                            sp = Conversions.ToSByte(val);
                        }

                        break;
                    }

                case 2:
                    {
                        if (Native.Unsigned(t))
                        {
                            sp = Conversions.ToUShort(val);
                        }
                        else
                        {
                            sp = Conversions.ToShort(val);
                        }

                        break;
                    }

                case 4:
                    {
                        if (Native.Unsigned(t))
                        {
                            sp = Conversions.ToUInteger(val);
                        }
                        else
                        {
                            sp = Conversions.ToInteger(val);
                        }

                        break;
                    }

                case 8:
                    {
                        if (Native.Unsigned(t))
                        {
                            sp = Conversions.ToULong(val);
                        }
                        else
                        {
                            sp = Conversions.ToLong(val);
                        }

                        break;
                    }
            }

            return sp;
        }

        // Friend BlobMasterHeap As BlobHeap

        static BlobUtil()
        {
            // BlobMasterHeap = New BlobHeap(SystemInfo.SystemInfo.dwPageSize * 76800)
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Specifies the type of memory allocated for the given blob.
    /// </summary>
    /// <remarks></remarks>
    [Flags]
    public enum MemAllocType
    {

        /// <summary>
        /// Invalid memory.
        /// </summary>
        /// <remarks></remarks>
        Invalid = 0,

        /// <summary>
        /// Normal heap memory.
        /// </summary>
        /// <remarks></remarks>
        Heap = 1,

        /// <summary>
        /// NetApi memory.
        /// </summary>
        /// <remarks></remarks>
        Network = 2,

        /// <summary>
        /// Virtual memory.
        /// </summary>
        /// <remarks></remarks>
        Virtual = 4,

        /// <summary>
        /// COM memory.
        /// </summary>
        /// <remarks></remarks>
        Com = 8,

        /// <summary>
        /// The memory is aligned.
        /// </summary>
        /// <remarks></remarks>
        Aligned = 32,

        /// <summary>
        /// Other/Unknown (or unowned) memory.
        /// </summary>
        /// <remarks></remarks>
        Other = 64
    }

    /// <summary>
    /// Specifies the blob parsing method to use for string content.
    /// </summary>
    /// <remarks></remarks>
    [DefaultValue(0)]
    public enum BlobParsingMethod
    {

        /// <summary>
        /// Unicode UTF-16 (default)
        /// </summary>
        /// <remarks></remarks>
        Unicode = 0,

        /// <summary>
        /// ASCII or UTF-8.
        /// </summary>
        /// <remarks></remarks>
        Ascii = 1,

        /// <summary>
        /// Use the byte-order-mark to determine.
        /// </summary>
        /// <remarks></remarks>
        BOM = 2
    }

    /// <summary>
    /// Indicates the type of content within the blob (loosely represented).
    /// </summary>
    /// <remarks></remarks>
    [DefaultValue(-1)]
    public enum BlobTypes : int
    {

        /// <summary>
        /// Content is invalid.
        /// </summary>
        /// <remarks></remarks>
        Invalid = 0xFFFFFF20,

        /// <summary>
        /// SByte
        /// </summary>
        /// <remarks></remarks>
        SByte = 0,

        /// <summary>
        /// Byte
        /// </summary>
        /// <remarks></remarks>
        Byte = 1,

        /// <summary>
        /// Short
        /// </summary>
        /// <remarks></remarks>
        Short = 2,

        /// <summary>
        /// UShort
        /// </summary>
        /// <remarks></remarks>
        UShort = 3,

        /// <summary>
        /// Integer
        /// </summary>
        /// <remarks></remarks>
        Integer = 4,

        /// <summary>
        /// UInteger
        /// </summary>
        /// <remarks></remarks>
        UInteger = 5,

        /// <summary>
        /// Long
        /// </summary>
        /// <remarks></remarks>
        Long = 6,

        /// <summary>
        /// ULong
        /// </summary>
        /// <remarks></remarks>
        ULong = 7,

        /// <summary>
        /// System.Numerics.BigInteger
        /// </summary>
        /// <remarks></remarks>
        BigInteger = 8,

        /// <summary>
        /// Single-precision floating-point number.
        /// </summary>
        /// <remarks></remarks>
        Single = 9,

        /// <summary>
        /// Double-precision floating-point number.
        /// </summary>
        /// <remarks></remarks>
        Double = 0xA,

        /// <summary>
        /// Quadruple-precision floating-point number.
        /// </summary>
        /// <remarks></remarks>
        Decimal = 0xB,

        /// <summary>
        /// DateTime object.
        /// </summary>
        /// <remarks></remarks>
        Date = 0xC,

        /// <summary>
        /// Char
        /// </summary>
        /// <remarks></remarks>
        Char = 0xD,

        /// <summary>
        /// String
        /// </summary>
        /// <remarks></remarks>
        String = 0xE,

        /// <summary>
        /// System.Guid
        /// </summary>
        /// <remarks></remarks>
        Guid = 0xF,

        /// <summary>
        /// PNG Image
        /// </summary>
        /// <remarks></remarks>
        Image = 0x10,

        /// <summary>
        /// Color
        /// </summary>
        /// <remarks></remarks>
        Color = 0x11,

        /// <summary>
        /// Worm Record
        /// </summary>
        /// <remarks></remarks>
        WormRecord = 0x12,

        /// <summary>
        /// Null-terminated string(s)
        /// </summary>
        /// <remarks></remarks>
        NtString = 0x13,

        /// <summary>
        /// Boolean
        /// </summary>
        /// <remarks></remarks>
        Boolean = 0x14,

        // ' For each of the following in serialization, the
        // ' stored data shall be: array-type moniker, type moniker, number of elements, data.
        // ' and where data is a length of parcel moniker at a 32 bit integer (and no more... for now), and then the data.

        // ' some of these may require the calling program to fulfill the requirement
        // ' of creating an instance of the required object via the InstanceHelper event.

        /// <summary>
        /// Denotes the base type to be an array.
        /// </summary>
        Array = 0x15,

        /// <summary>
        /// Denotes the base type to be an array.
        /// </summary>
        List = 0x16,

        /// <summary>
        /// Denotes the base type to be an array.
        /// </summary>
        ListOf = 0x17,

        /// <summary>
        /// Denotes the base type to be an array.
        /// </summary>
        Collection = 0x18,

        /// <summary>
        /// Denotes the base type to be an array.
        /// </summary>
        CollectionOf = 0x19,

        /// <summary>
        /// Denotes the base type to be an array.
        /// </summary>
        ObservableCollection = 0x1A,

        /// <summary>
        /// Denotes the base type to be an array.
        /// </summary>
        ObservableCollectionOf = 0x1B,

        /// <summary>
        /// Indicates numeric array, generally only used by SerializerAdapter.
        /// </summary>
        NumericArray = 0x40000000,

        /// <summary>
        /// Generic byte[] based blob of data, generally only used by SerializerAdapter.
        /// </summary>
        Blob = 0x2000001C
    }

    /// <summary>
    /// Indicates a pure ordinal integer blob type.
    /// </summary>
    /// <remarks></remarks>
    [DefaultValue(0xFFFFFFF8)]
    public enum BlobOrdinalTypes : int
    {
        /// <summary>
        /// Invalid value
        /// </summary>
        /// <remarks></remarks>
        Invalid = 0xFFFFFFF8,

        /// <summary>
        /// SByte
        /// </summary>
        /// <remarks></remarks>
        SByte = 0,

        /// <summary>
        /// Byte
        /// </summary>
        /// <remarks></remarks>
        Byte = 1,

        /// <summary>
        /// Short
        /// </summary>
        /// <remarks></remarks>
        Short = 2,

        /// <summary>
        /// UShort
        /// </summary>
        /// <remarks></remarks>
        UShort = 3,

        /// <summary>
        /// Integer
        /// </summary>
        /// <remarks></remarks>
        Integer = 4,

        /// <summary>
        /// UInteger
        /// </summary>
        /// <remarks></remarks>
        UInteger = 5,

        /// <summary>
        /// Long
        /// </summary>
        /// <remarks></remarks>
        Long = 6,

        /// <summary>
        /// ULong
        /// </summary>
        /// <remarks></remarks>
        ULong = 7
    }

    [DefaultValue(-1)]
    internal enum SystemBlobTypes : int
    {

        /// <summary>
        /// Content is invalid.
        /// </summary>
        /// <remarks></remarks>
        Invalid = 0xFFFFFF20,

        /// <summary>
        /// SByte
        /// </summary>
        /// <remarks></remarks>
        SByte = 0,
        Int8 = 0,

        /// <summary>
        /// Byte
        /// </summary>
        /// <remarks></remarks>
        Byte = 1,
        UInt8 = 1,

        /// <summary>
        /// Short
        /// </summary>
        /// <remarks></remarks>
        Short = 2,
        Int16 = 2,

        /// <summary>
        /// UShort
        /// </summary>
        /// <remarks></remarks>
        UShort = 3,
        UInt16 = 3,

        /// <summary>
        /// Integer
        /// </summary>
        /// <remarks></remarks>
        Integer = 4,
        Int32 = 4,

        /// <summary>
        /// UInteger
        /// </summary>
        /// <remarks></remarks>
        UInteger = 5,
        UInt32 = 5,

        /// <summary>
        /// Long
        /// </summary>
        /// <remarks></remarks>
        Long = 6,
        Int64 = 6,

        /// <summary>
        /// ULong
        /// </summary>
        /// <remarks></remarks>
        ULong = 7,
        UInt64 = 7,

        /// <summary>
        /// Single-precision floating-point number.
        /// </summary>
        /// <remarks></remarks>
        Single = 9,
        Float = 9,

        /// <summary>
        /// Double-precision floating-point number.
        /// </summary>
        /// <remarks></remarks>
        Double = 0xA,

        /// <summary>
        /// Quadruple-precision floating-point number.
        /// </summary>
        /// <remarks></remarks>
        Decimal = 0xB,

        /// <summary>
        /// DateTime object.
        /// </summary>
        /// <remarks></remarks>
        Date = 0xC,

        /// <summary>
        /// Char
        /// </summary>
        /// <remarks></remarks>
        Char = 0xD,

        /// <summary>
        /// String
        /// </summary>
        /// <remarks></remarks>
        String = 0xE,

        /// <summary>
        /// System.Guid
        /// </summary>
        /// <remarks></remarks>
        Guid = 0xF,

        /// <summary>
        /// PNG Image
        /// </summary>
        /// <remarks></remarks>
        Image = 0x10,

        /// <summary>
        /// Color
        /// </summary>
        /// <remarks></remarks>
        Color = 0x11,

        /// <summary>
        /// Boolean
        /// </summary>
        /// <remarks></remarks>
        Boolean = 0x14
    }

    /// <summary>
    /// Blob constants
    /// </summary>
    /// <remarks></remarks>
    internal enum BlobConst
    {
        /// <summary>
        /// Maximum blob-type integer value for mathematical operations.
        /// </summary>
        /// <remarks></remarks>
        MaxMath = 11,

        /// <summary>
        /// Maximum blob-type integer value for bitwi
        /// </summary>
        /// <remarks></remarks>
        MaxBit = 7,
        UBound = 19
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */

}