// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// '
// ' Module: Blob
// '         Exhaustive Memory Manipulation Object
// '         With adaptive buffering.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using DataTools.BitStream;
using DataTools.Memory.Internal;
using DataTools.Strings;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.Memory
{

    // ' *********************************
    // ' *********************************
    // ' Blob
    // ' *********************************
    // ' *********************************

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Exhaustive multipurpose memory manipulation class.
    /// </summary>
    /// <remarks></remarks>
    [TypeConverter(typeof(BlobConverter))]
    public class Blob : SafeHandle, IEquatable<SafeHandle>, IEquatable<UIntPtr>, IEquatable<MemPtr>, IEquatable<IntPtr>, IEquatable<SafePtr>, IEquatable<Blob>
    {

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        internal static readonly BlobConverter Converter = new BlobConverter();
        internal static IntPtr defaultHeap = Native.GetProcessHeap();
        internal static Type[] Types;
        internal static Type[] ArrayTypes;
        internal static object[] DumbInstance;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Gets the system page size.
        /// </summary>
        public static readonly object SystemPageSize = SystemInformation.SysInfo.SystemInfo.dwPageSize;

        /// <summary>
        /// Sets the global behavior for attempting to parse string input for content.
        /// </summary>
        /// <returns></returns>
        public static bool ParseStrings { get; set; } = false;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Represents the length of the buffer as presented to the outside world, regardless
        /// of allocation mode.
        /// </summary>
        private long virtLen = 0L;

        /// <summary>
        /// Represents the actual length of the allocated buffer, in memory.
        /// </summary>
        private long actualLen = 0L;

        /// <summary>
        /// represents the default amount by which to extend the buffer in buffering mode.
        /// </summary>
        private long _bufferExtend = SystemInformation.SysInfo.SystemInfo.dwPageSize;

        /// <summary>
        /// Represents a value indicating that buffering mode is active.
        /// </summary>
        private bool _inBuffer = false;

        /// <summary>
        /// Represents a value indicating that the BufferExtend property will be doubled
        /// Whenever a buffer reallocation occurs (not usually recommended.)
        /// </summary>
        private bool _AutoDouble = false;

        /// <summary>
        /// Represents the type of data this blob represents.
        /// </summary>
        private BlobTypes _BlobType;

        /// <summary>
        /// Indicates that string concatenations will not append a ChrW(0) to the end.
        /// </summary>
        private bool _StringCatNoNull = false;

        /// <summary>
        /// Maximum number of byte values to print in the ToString method before suspending that behavior.
        /// </summary>
        private int _MaxBlobPrintNum = 128;

        /// <summary>
        /// Indicates that the blob is locked.
        /// </summary>
        private bool _Locked = false;

        /// <summary>
        /// The handle allocation type.
        /// </summary>
        private MemAllocType _MemType = MemAllocType.Invalid;

        /// <summary>
        /// Basically the seek/position of the buffer in the Clip stream.
        /// </summary>
        private int _ClipNext = 0;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// contains the length of the primitive type (if applicable)
        /// </summary>
        internal int TypeLen;

        /// <summary>
        /// Contains the system type representation of the blob's contents.
        /// </summary>
        internal Type Type;

        /// <summary>
        /// Indicates that we own the handle.
        /// </summary>
        internal bool fOwn = true;

        /// <summary>
        /// The heap object of the heap that the blob is currently allocated on.
        /// </summary>
        internal BlobHeap activeHeap = null;

        /// <summary>
        /// The pointer to the active heap, either a private heap or the process heap.
        /// Default is process heap.
        /// </summary>
        internal IntPtr hHeap = defaultHeap;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Gets the heap that the current blob is created on.
        /// </summary>
        /// <returns></returns>
        internal BlobHeap Heap
        {
            get
            {
                if (activeHeap is null)
                    return BlobHeap.DefaultHeap;
                return activeHeap;
            }
        }

        /// <summary>
        /// Moves the specified blob to the specified heap, copying and freeing memory, if necessary.
        /// </summary>
        /// <param name="bl">The blob</param>
        /// <param name="heap">The heap</param>
        /// <returns>True if successful.</returns>
        public static bool MoveToHeap(Blob bl, BlobHeap heap)
        {
            if (System.Threading.Monitor.TryEnter(bl))
            {
                if (System.Threading.Monitor.TryEnter(heap))
                {
                    if (bl.Length == 0L)
                    {
                        if (bl.activeHeap is object)
                        {
                            bl.activeHeap.RemoveSelf(bl);
                        }

                        bl.virtLen = 0L;
                        bl.actualLen = 0L;
                        bl.hHeap = heap.DangerousGetHandle();
                        bl.activeHeap = heap;
                        bl.activeHeap.AddSelf(bl);
                    }
                    else
                    {
                        var ptr = Native.HeapAlloc(heap.DangerousGetHandle(), 0U, (IntPtr)bl.actualLen);
                        if (Conversions.ToBoolean(ptr))
                        {
                            if (bl.Length > uint.MaxValue)
                            {
                                Native.n_memcpy(ptr, bl.handle, (UIntPtr)bl.virtLen);
                            }
                            else
                            {
                                Native.MemCpy(ptr, bl.handle, (uint)bl.virtLen);
                            }

                            if (Native.HeapFree(bl.hHeap, 0U, bl.handle))
                            {
                                if (bl.activeHeap is object)
                                {
                                    bl.activeHeap.RemoveSelf(bl);
                                }

                                bl.handle = ptr;
                                bl.hHeap = heap.DangerousGetHandle();
                                bl.activeHeap = heap;
                                bl.activeHeap.AddSelf(bl);
                            }
                            else
                            {
                                System.Threading.Monitor.Exit(heap);
                                System.Threading.Monitor.Exit(bl);
                                return false;
                            }
                        }
                        else
                        {
                            System.Threading.Monitor.Exit(heap);
                            System.Threading.Monitor.Exit(bl);
                            return false;
                        }
                    }

                    System.Threading.Monitor.Exit(heap);
                    System.Threading.Monitor.Exit(bl);
                    return true;
                }
                else
                {
                    System.Threading.Monitor.Exit(bl);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public override bool IsInvalid
        {
            get
            {
                return handle == (IntPtr)0;
            }
        }

        protected override bool ReleaseHandle()
        {
            bool ReleaseHandleRet = default;
            if (activeHeap is object)
            {
                activeHeap.RemoveSelf(this);
            }

            ReleaseHandleRet = Free();
            return ReleaseHandleRet;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public Blob() : base((IntPtr)0, true)
        {
        }

        /// <summary>
        /// Initialize a new blob from a string.
        /// </summary>
        /// <param name="value"></param>
        /// <remarks></remarks>
        public Blob(string value) : base((IntPtr)0, true)
        {
            MemPtr mm = value;
            handle = mm.Handle;
        }

        /// <summary>
        /// Initialize a new blob with a pre-existing memory handle of the specified kind.
        /// </summary>
        /// <param name="value">Memory handle.</param>
        /// <param name="kind">Kind of memory handle.</param>
        /// <param name="fOwn">Whether or not to own the handle and release on object destruction.</param>
        /// <remarks></remarks>
        public Blob(IntPtr value, MemAllocType kind, bool fOwn = true) : base(value, fOwn)
        {
            this.fOwn = fOwn;
            _MemType = kind;
        }

        public Blob(byte[] bytes) : base((IntPtr)0, true)
        {
            Alloc(bytes.Length);
            Native.MemCpy(handle, bytes, (uint)bytes.Length);
        }

        /// <summary>
        /// Initialize a new blob from the specified structure.
        /// </summary>
        /// <param name="struct"></param>
        /// <remarks></remarks>
        public Blob(object @struct) : base((IntPtr)0, true)
        {
            if (@struct.GetType().IsValueType == false)
                throw new ArgumentException("Must be a structure");
            int l = Marshal.SizeOf(@struct);
            if (Alloc(l))
            {
                Marshal.StructureToPtr(@struct, handle, false);
            }
            else
            {
                throw new OutOfMemoryException("Could not initialize memory buffer.");
            }
        }


        /// <summary>
        /// Initialize a new blob by copying an existing blob
        /// </summary>
        /// <param name="blob"></param>
        /// <remarks></remarks>
        public Blob(Blob blob) : base((IntPtr)0, true)
        {
            Alloc(blob.Length);
            if (blob.Length > uint.MaxValue)
            {
                Native.n_memcpy(handle, blob.handle, (UIntPtr)blob.Length);
            }
            else
            {
                Native.MemCpy(handle, blob.handle, (uint)blob.Length);
            }

            TypeLen = blob.TypeLen;
            _BlobType = blob._BlobType;
            Align();
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private static void InitTypeSize()
        {
            Types = new Type[21];
            ArrayTypes = new Type[21];
            DumbInstance = new object[21];
            Types[0] = typeof(sbyte);
            Types[1] = typeof(byte);
            Types[2] = typeof(short);
            Types[3] = typeof(ushort);
            Types[4] = typeof(int);
            Types[5] = typeof(uint);
            Types[6] = typeof(long);
            Types[7] = typeof(ulong);
            Types[8] = typeof(BigInteger);
            Types[9] = typeof(float);
            Types[10] = typeof(double);
            Types[11] = typeof(decimal);
            Types[12] = typeof(DateTime);
            Types[13] = typeof(char);
            Types[14] = typeof(string);
            Types[15] = typeof(Guid);
            Types[16] = typeof(Image);
            Types[17] = typeof(Color);
            Types[18] = typeof(WormRecord);
            Types[19] = typeof(string);
            Types[20] = typeof(bool);
            BlobTypes i;
            object o;
            for (i = 0; i <= (BlobTypes)BlobConst.UBound; i++)
            {
                switch (i)
                {
                    case BlobTypes.BigInteger:
                    case BlobTypes.Image:
                    case BlobTypes.String:
                    case BlobTypes.NtString:
                    case BlobTypes.WormRecord:
                        {
                            continue;
                            break;
                        }

                    default:
                        {
                            o = Array.CreateInstance(Types[(int)i], 1);
                            ArrayTypes[(int)i] = o.GetType();
                            DumbInstance[(int)i] = o(0);
                            o = null;
                            break;
                        }
                }
            }
        }

        static Blob()
        {
            InitTypeSize();
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Converts a System.Type to a BlobTypes value
        /// </summary>
        [Description("Converts a System.Type to a BlobTypes value")]
        public static BlobTypes TypeToBlobType(Type type)
        {
            var t = type.IsArray ? type.GetElementType() : type;
            BlobTypes i;
            if (ReferenceEquals(type, typeof(bool)))
                return BlobTypes.Boolean;
            if (t.IsEnum == true)
            {
                t = t.GetEnumUnderlyingType();
            }
            else if (t.IsClass && (t.BaseType == typeof(Image) || t == typeof(Image)))
            {
                t = typeof(Image);
            }

            i = BlobTypes.Invalid;
            for (i = 0; i <= (BlobTypes)BlobConst.UBound; i++)
            {
                if (Types[(int)i] == t || (Types[(int)i].ToString() ?? "") == (t.ToString().Replace("&", "") ?? ""))
                {
                    break;
                }
            }

            if ((int)i > (int)BlobConst.UBound)
                return BlobTypes.Invalid;
            if (type.IsArray)
            {
                switch (i)
                {
                    // ' actual arrays of strings and BigIntegers are not supported because their size is not intrinsically knowable.
                    case BlobTypes.BigInteger:
                    case BlobTypes.String:
                    case BlobTypes.NtString:
                    case BlobTypes.Image:
                        {
                            return BlobTypes.Invalid;
                        }
                }
            }

            return i;
        }

        /// <summary>
        /// Converts a BlobTypes value to a System.Type
        /// </summary>
        [Description("Converts a BlobTypes value to a System.Type")]
        public static Type BlobTypeToType(BlobTypes type)
        {
            if (type == BlobTypes.Boolean)
                return typeof(bool);
            if (type == BlobTypes.Invalid)
                return typeof(byte[]);
            if (type == BlobTypes.NtString)
                return Types[(int)BlobTypes.String];
            return Types[(int)type];
        }

        /// <summary>
        /// Returns the element size of the specified BlobType
        /// </summary>
        [Description("Returns the element size of the specified BlobType")]
        public static int BlobTypeSize(BlobTypes type)
        {
            // ' returns 0 if the size is indeterminate

            switch (type)
            {
                case BlobTypes.Byte:
                case BlobTypes.SByte:
                case BlobTypes.BigInteger:
                case BlobTypes.Image:
                case BlobTypes.WormRecord:
                case BlobTypes.Boolean:
                    {
                        return 1;
                    }

                case BlobTypes.Short:
                case BlobTypes.UShort:
                    {
                        return 2;
                    }

                case BlobTypes.Char:
                case BlobTypes.String:
                case BlobTypes.NtString:
                    {
                        var ch = default(char);
                        return Microsoft.VisualBasic.Strings.Len(ch);
                    }

                case BlobTypes.Integer:
                case BlobTypes.UInteger:
                case BlobTypes.Single:
                case BlobTypes.Color:
                    {
                        return 4;
                    }

                case BlobTypes.Long:
                case BlobTypes.ULong:
                case BlobTypes.Double:
                case BlobTypes.Date:
                    {
                        return 8;
                    }

                case BlobTypes.Decimal:
                case BlobTypes.Guid:
                    {
                        return 16;
                    }

                default:
                    {
                        return 1;
                    }
            }
        }

        /// <summary>
        /// Rerank one blob to match another blob, using the maximum type length of the two.
        /// </summary>
        [Description("Rerank one blob to match another blob, using the maximum type length of the two.")]
        public static void ReRankMax(ref Blob blob1, Blob blob2)
        {
            int rs1 = BlobTypeSize(blob1.BlobType);
            int rs2 = BlobTypeSize(blob2.BlobType);
            if (rs1 == rs2)
            {
                // ' they are the same size, we'll make them the same type.
                blob2.Type = blob1.Type;
                return;
            }

            if (rs1 > rs2)
            {
                ReRank(ref blob2, blob1.BlobType);
                blob2.BlobType = blob1.BlobType;
            }
            else
            {
                ReRank(ref blob1, blob1.BlobType);
                blob1.BlobType = blob2.BlobType;
            }
        }

        /// <summary>
        /// Attempts to re-rank the specified blob to the maximum of two types, increasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.
        /// </summary>
        [Description("Attempts to re-rank the specified blob to the maximum of two types, increasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.")]
        public static void ReRankMax(ref Blob blob, Type type1, Type type2)
        {
            ReRankMax(ref blob, TypeToBlobType(type1), TypeToBlobType(type2));
        }

        /// <summary>
        /// Attempts to re-rank the specified blob to the maximum of two types, increasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.
        /// </summary>
        [Description("Attempts to re-rank the specified blob to the maximum of two types, increasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.")]
        public static void ReRankMax(ref Blob blob, BlobTypes type1, BlobTypes type2)
        {
            ReRank(ref blob, (BlobTypes)Math.Max((int)type1, (int)type2));
        }

        /// <summary>
        /// Rerank one blob to match another blob, using the minimum type length of the two.
        /// </summary>
        [Description("Rerank one blob to match another blob, using the minimum type length of the two.")]
        public static void ReRankMin(ref Blob blob1, Blob blob2)
        {
            int rs1 = BlobTypeSize(blob1.BlobType);
            int rs2 = BlobTypeSize(blob2.BlobType);
            if (rs1 == rs2)
                return;
            if (rs1 == rs2)
            {
                // ' they are the same size, we'll make them the same type.
                blob2.Type = blob1.Type;
                return;
            }

            if (rs1 > rs2)
            {
                ReRank(ref blob2, blob1.BlobType);
                blob2.Type = blob1.Type;
            }
            else
            {
                ReRank(ref blob1, blob1.BlobType);
                blob1.Type = blob2.Type;
            }
        }

        /// <summary>
        /// Attempts to re-rank the specified blob to the minimum of two types, decreasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.
        /// </summary>
        [Description("Attempts to re-rank the specified blob to the minimum of two types, decreasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.")]
        public static void ReRankMin(ref Blob blob, Type type1, Type type2)
        {
            ReRankMin(ref blob, TypeToBlobType(type1), TypeToBlobType(type2));
        }

        /// <summary>
        /// Attempts to re-rank the specified blob to the minimum of two types, decreasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.
        /// </summary>
        [Description("Attempts to re-rank the specified blob to the minimum of two types, decreasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.")]
        public static void ReRankMin(ref Blob blob, BlobTypes type1, BlobTypes type2)
        {
            ReRank(ref blob, (BlobTypes)Math.Min((int)type1, (int)type2));
        }

        /// <summary>
        /// Attempts to re-rank the specified blob, increasing or decreasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.
        /// </summary>
        [Description("Attempts to re-rank the specified blob, increasing or decreasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.")]
        public static void ReRank(ref Blob blob, Type type)
        {
            ReRank(ref blob, TypeToBlobType(type));
        }

        /// <summary>
        /// Attempts to re-rank the specified blob, increasing or decreasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.
        /// </summary>
        [Description("Attempts to re-rank the specified blob, increasing or decreasing the type length as needed.  This is not a coercion; the array, itself, is redimmed.")]
        public static void ReRank(ref Blob blob, BlobTypes type)
        {
            switch (blob.BlobType)
            {
                case BlobTypes.BigInteger:
                case BlobTypes.Guid:
                case BlobTypes.Invalid:
                case BlobTypes.String:
                case BlobTypes.NtString:
                case BlobTypes.Char:
                    {
                        throw new ArgumentException("Blobs that do not contain purely numeric values cannot be reranked.  Call TypeAlign, first, to coerce the blob into a numeric type.");
                        return;
                    }
            }

            switch (type)
            {
                case BlobTypes.BigInteger:
                case BlobTypes.Guid:
                case BlobTypes.Invalid:
                    {
                        throw new ArgumentException("Cannot rerank to non numeric types.  Use TypeAlign, instead.");
                        return;
                    }

                case BlobTypes.String:
                case BlobTypes.NtString:
                case BlobTypes.Char:
                    {
                        if (blob.BlobType == BlobTypes.Byte)
                            break;
                        throw new ArgumentException("Cannot rerank to non numeric types.  Use TypeAlign, instead.");
                        return;
                    }
            }

            int rs1 = BlobTypeSize(blob.BlobType);
            int rs2 = BlobTypeSize(type);
            if (rs1 == rs2)
            {
                blob.TypeAlign(BlobTypeToType(type));
                return;
            }

            int c = blob.Count;
            int e;
            int d = 0;
            int f = 0;
            int i;
            var cb = new Blob(blob);
            e = Math.Min(rs1, rs2);
            cb.Length = c * rs2;
            cb.BlobType = type;
            c -= 1;
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                Native.MemCpy(cb.handle + f, blob.handle + d, (uint)e);
                d += rs1;
                f += rs2;
            }

            blob.handle = cb.handle;
            cb.handle = (IntPtr)0;
            blob.TypeAlign(BlobTypeToType(type));
        }

        public static MemoryStream CreateMemoryStream(Blob blob)
        {
            return new MemoryStream((byte[])blob);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Returns the start position of the next clip
        /// </summary>
        [Description("Returns the start position of the next clip")]
        public int ClipNext
        {
            get
            {
                return _ClipNext;
            }
        }

        /// <summary>
        /// Seeks to the specified clip position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public long ClipSeek(long position)
        {
            long ClipSeekRet = default;
            if (position >= Length)
            {
                _ClipNext = (int)Length;
            }
            else
            {
                _ClipNext = (int)position;
            }

            ClipSeekRet = _ClipNext;
            return ClipSeekRet;
        }

        /// <summary>
        /// Returns a clip of the buffer as the given type
        /// </summary>
        [Description("Returns a clip of the buffer as the given type")]
        public object Clip(int length, Type type)
        {
            return Clip(_ClipNext, length, type);
        }

        /// <summary>
        /// Returns a clip of the buffer as the given type
        /// </summary>
        [Description("Returns a clip of the buffer as the given type")]
        public object Clip(int length, BlobTypes type)
        {
            return Clip(_ClipNext, length, type);
        }

        /// <summary>
        /// Returns a clip of the buffer as the given type
        /// </summary>
        [Description("Returns a clip of the buffer as the given type")]
        public object Clip(int startIndex, int length, BlobTypes type)
        {
            return Clip(startIndex, length, BlobTypeToType(type));
        }

        /// <summary>
        /// Returns a clip of the buffer as the given type
        /// </summary>
        [Description("Returns a clip of the buffer as the given type")]
        public object Clip(int startIndex, int length, Type type)
        {
            object ClipRet = default;
            if (length == 0 && type == typeof(string))
            {
                ClipRet = GrabString((IntPtr)startIndex);
                _ClipNext = startIndex + Conversions.ToString(ClipRet).Length * 2 + 2;
                return ClipRet;
            }

            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException("startIndex", "Index cannot be a negative number. If you need to substract from a pointer, use MemPtr, instead");
            }
            else if (startIndex + length > Length)
            {
                throw new ArgumentOutOfRangeException("length", "Parameter exceeds buffer size.");
            }

            _ClipNext = startIndex + length;
            if (type == typeof(WormRecord))
            {
                ClipRet = (WormRecord)new Blob(GrabBytes((IntPtr)0, length));
            }
            else
            {
                switch (type)
                {
                    case var @case when @case == typeof(string):
                        {
                            ClipRet = GrabString((IntPtr)startIndex, length >> 1);
                            break;
                        }

                    case var case1 when case1 == typeof(bool):
                        {
                            if (length > 1)
                            {
                                ClipRet = GrabBytes((IntPtr)startIndex, length);
                            }
                            else
                            {
                                ClipRet = Conversions.ToBoolean(get_ByteAt(startIndex));
                            }

                            break;
                        }

                    case var case2 when case2 == typeof(byte):
                        {
                            if (length > 1)
                            {
                                ClipRet = GrabBytes((IntPtr)startIndex, length);
                            }
                            else
                            {
                                ClipRet = get_ByteAt(startIndex);
                            }

                            break;
                        }

                    case var case3 when case3 == typeof(short):
                        {
                            if (length > 2)
                            {
                                ClipRet = GetShortArray((IntPtr)startIndex, length >> 1);
                            }
                            else
                            {
                                ClipRet = get_ShortAtAbsolute(startIndex);
                            }

                            break;
                        }

                    case var case4 when case4 == typeof(ushort):
                        {
                            if (length > 2)
                            {
                                ClipRet = GetUShortArray((IntPtr)startIndex, length >> 1);
                            }
                            else
                            {
                                ClipRet = get_UShortAtAbsolute(startIndex);
                            }

                            break;
                        }

                    case var case5 when case5 == typeof(int):
                        {
                            if (length > 4)
                            {
                                ClipRet = GetIntegerArray((IntPtr)startIndex, length >> 2);
                            }
                            else
                            {
                                ClipRet = get_IntegerAtAbsolute(startIndex);
                            }

                            break;
                        }

                    case var case6 when case6 == typeof(uint):
                        {
                            if (length > 4)
                            {
                                ClipRet = GetUIntegerArray((IntPtr)startIndex, length >> 2);
                            }
                            else
                            {
                                ClipRet = get_UIntegerAtAbsolute(startIndex);
                            }

                            break;
                        }

                    case var case7 when case7 == typeof(float):
                        {
                            if (length > 4)
                            {
                                ClipRet = GetSingleArray((IntPtr)startIndex, length >> 2);
                            }
                            else
                            {
                                ClipRet = get_SingleAtAbsolute(startIndex);
                            }

                            break;
                        }

                    case var case8 when case8 == typeof(long):
                        {
                            if (length > 8)
                            {
                                ClipRet = GetLongArray((IntPtr)startIndex, length >> 3);
                            }
                            else
                            {
                                ClipRet = get_LongAtAbsolute(startIndex);
                            }

                            break;
                        }

                    case var case9 when case9 == typeof(ulong):
                        {
                            if (length > 8)
                            {
                                ClipRet = GetULongArray((IntPtr)startIndex, length >> 3);
                            }
                            else
                            {
                                ClipRet = get_ULongAtAbsolute(startIndex);
                            }

                            break;
                        }

                    case var case10 when case10 == typeof(double):
                        {
                            if (length > 8)
                            {
                                ClipRet = GetDoubleArray((IntPtr)startIndex, length >> 3);
                            }
                            else
                            {
                                ClipRet = get_DoubleAtAbsolute(startIndex);
                            }

                            break;
                        }

                    case var case11 when case11 == typeof(decimal):
                        {
                            if (length > 16)
                            {
                                ClipRet = GetDecimalArray((IntPtr)startIndex, length >> 4);
                            }
                            else
                            {
                                ClipRet = get_DecimalAtAbsolute(startIndex);
                            }

                            break;
                        }

                    case var case12 when case12 == typeof(Guid):
                        {
                            if (length > 16)
                            {
                                ClipRet = GetGuidArray((IntPtr)startIndex, length >> 4);
                            }
                            else
                            {
                                ClipRet = get_GuidAtAbsolute(startIndex);
                            }

                            break;
                        }

                    default:
                        {
                            ClipRet = Converter.ConvertTo(new Blob(handle + startIndex, MemAllocType.Heap, false) { hHeap = hHeap, virtLen = length, actualLen = length }, type);
                            break;
                        }
                }
            }

            return ClipRet;
        }

        /// <summary>
        /// Sets the bytes of the specified object to the blob.
        /// </summary>
        /// <param name="index">Start index, in bytes to set the data</param>
        /// <param name="value">Object to set</param>
        /// <remarks></remarks>
        public void SetAt(long index, object value)
        {
            if (TypeToBlobType(value.GetType()) == BlobTypes.Invalid)
                return;
            Blob bl = (Blob)Converter.ConvertFrom(value);
            Native.MemCpy(handle + (int)index, bl.handle, (uint)bl.Length);
            bl.Dispose();
        }

        /// <summary>
        /// Sets the specified character array into the blob at the specified index
        /// </summary>
        /// <param name="index">Start index, in characters to set the data</param>
        /// <param name="value">Object to set</param>
        /// <remarks></remarks>
        public void SetAt(long index, char[] value)
        {
            if (index + 2 * value.Length > Length)
                throw new ArgumentOutOfRangeException();
            Native.MemCpy(handle + (int)(index * 2L), value, (uint)(value.Length * 2));
        }

        /// <summary>
        /// Sets the specified character array into the blob at the specified index
        /// </summary>
        /// <param name="index">Start index, in characters to set the data</param>
        /// <param name="value">Object to set</param>
        /// <remarks></remarks>
        public void SetAt(long index, string value)
        {
            if (index + 2 * value.Length > Length)
                throw new ArgumentOutOfRangeException();
            SetString((IntPtr)index, value);
        }

        /// <summary>
        /// Skips a clip.
        /// </summary>
        [Description("Skips a clip.")]
        public void Skip(int length)
        {
            _ClipNext += length;
            if (_ClipNext > Length)
            {
                _ClipNext = 0;
            }
        }

        /// <summary>
        /// Resets the clip count
        /// </summary>
        [Description("Resets the clip count")]
        public void ClipReset()
        {
            _ClipNext = 0;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        // ' if you're going to have an unmanaged buffer then there is
        // ' no reason for a managed byte array as a middle man.
        // ' it wastes many resources for large files.
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("kernel32.dll", EntryPoint = "CreateFileW", CharSet = CharSet.Unicode)]

        static extern IntPtr CreateFile([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, int dwDesiredAccess, int dwShareMode, IntPtr lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern bool WriteFile(IntPtr hFile, IntPtr lpBuffer, uint nNumberOfBytesToWrite, ref uint lpNumberOfBytesWritten, IntPtr lpOverlapped);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern bool ReadFile(IntPtr hFile, IntPtr lpBuffer, uint nNumberOfBytesToRead, ref uint lpNumberOfBytesRead, IntPtr lpOverlapped);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern bool FlushFileBuffers(IntPtr hFile);
        [DllImport("kernel32", EntryPoint = "GetFileSize")]
        static extern uint GetFileSize(IntPtr hFile, ref uint lpFileSizeHigh);
        [DllImport("kernel32")]
        static extern bool CloseHandle(IntPtr handle);

        private const int GENERIC_READ = 0x80000000;
        private const int GENERIC_WRITE = 0x40000000;
        private const int GENERIC_EXECUTE = 0x20000000;
        private const int GENERIC_ALL = 0x10000000;
        private const int FILE_ATTRIBUTE_NORMAL = 0x80;
        private const int CREATE_NEW = 1;
        private const int CREATE_ALWAYS = 2;
        private const int OPEN_EXISTING = 3;
        private const int OPEN_ALWAYS = 4;
        private const int TRUNCATE_EXISTING = 5;
        private const int FILE_SHARE_READ = 0x1;
        private const int FILE_SHARE_WRITE = 0x2;
        private const int FILE_SHARE_DELETE = 0x4;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        public long Read(string fileName, bool append = false)
        {
            var h = Blob.CreateFile(ref fileName, GENERIC_READ, 0, (IntPtr)0, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, (IntPtr)FILE_SHARE_READ);
            if (Conversions.ToBoolean(h))
            {
                IntPtr cptr;
                uint bytesread = 0U;
                var hi = default(uint);
                uint lo = GetFileSize(h, ref hi);
                long len = (long)hi << 32 | lo;
                if (append)
                {
                    long oldVirt = virtLen;
                    if (!Alloc(virtLen + len))
                    {
                        CloseHandle(h);
                        return 0L;
                    }

                    cptr = handle + (int)oldVirt;
                }
                else
                {
                    if (!Alloc(len))
                    {
                        CloseHandle(h);
                        return 0L;
                    }

                    cptr = handle;
                }

                ClipReset();
                long blen = len;
                long ofs = 0L;
                uint inpLen = (uint)Math.Min(uint.MaxValue, len);
                while (ofs < blen)
                {
                    ReadFile(h, cptr, inpLen, ref bytesread, (IntPtr)0);
                    if (bytesread < inpLen)
                    {
                        ofs += bytesread;
                        break;
                    }

                    ofs += inpLen;
                    cptr = cptr + (int)inpLen;
                    inpLen = (uint)Math.Min(blen - ofs, inpLen);
                }

                FlushFileBuffers(h);
                CloseHandle(h);
                return ofs;
            }
            else
            {
                return 0L;
            }
        }

        public long Read(string fileName, long length, bool append = false)
        {
            var h = Blob.CreateFile(ref fileName, GENERIC_READ, 0, (IntPtr)0, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, (IntPtr)FILE_SHARE_READ);
            if (Conversions.ToBoolean(h))
            {
                IntPtr cptr;
                uint bytesread = 0U;
                var hi = default(uint);
                uint lo = GetFileSize(h, ref hi);
                long len = Math.Min((long)hi << 32 | lo, length);
                if (append)
                {
                    long oldVirt = virtLen;
                    if (!Alloc(virtLen + len))
                    {
                        CloseHandle(h);
                        return 0L;
                    }

                    cptr = handle + (int)oldVirt;
                }
                else
                {
                    if (!Alloc(len))
                    {
                        CloseHandle(h);
                        return 0L;
                    }

                    cptr = handle;
                }

                ClipReset();
                long blen = len;
                long ofs = 0L;
                uint inpLen = (uint)Math.Min(uint.MaxValue, len);
                while (ofs < blen)
                {
                    ReadFile(h, cptr, inpLen, ref bytesread, (IntPtr)0);
                    if (bytesread < inpLen)
                    {
                        ofs += bytesread;
                        break;
                    }

                    ofs += inpLen;
                    cptr = cptr + (int)inpLen;
                    inpLen = (uint)Math.Min(blen - ofs, inpLen);
                }

                FlushFileBuffers(h);
                CloseHandle(h);
                return ofs;
            }
            else
            {
                return 0L;
            }
        }

        public long Write(string fileName, bool fOverwrite = true)
        {
            if (!fOverwrite & File.Exists(fileName))
                return Conversions.ToLong(false);
            var h = Blob.CreateFile(ref fileName, GENERIC_WRITE, 0, (IntPtr)0, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, (IntPtr)FILE_SHARE_READ);
            if (Conversions.ToBoolean(h))
            {
                uint byteswritten = 0U;
                long blen = virtLen;
                long ofs = 0L;
                uint inpLen = (uint)Math.Min(uint.MaxValue, virtLen);
                var cptr = handle;
                while (ofs < blen)
                {
                    WriteFile(h, cptr, inpLen, ref byteswritten, (IntPtr)0);
                    if (byteswritten < inpLen)
                    {
                        ofs += byteswritten;
                        break;
                    }

                    ofs += inpLen;
                    cptr = cptr + (int)inpLen;
                    inpLen = (uint)Math.Min(blen - ofs, inpLen);
                }

                FlushFileBuffers(h);
                CloseHandle(h);
                return ofs;
            }
            else
            {
                return Conversions.ToLong(false);
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public void ToStructure(ref object dest)
        {
            Marshal.PtrToStructure(handle, dest);
        }

        public void FromStructure(object src)
        {
            int cb = Marshal.SizeOf(src);
            if (cb <= 0)
                return;
            if (Length < cb)
                Length = cb;
            Marshal.StructureToPtr(src, handle, false);
        }

        public void AppendStructure(object src)
        {
            int cb = Marshal.SizeOf(src);
            var myptr = handle + (int)Length;
            if (cb <= 0)
                return;
            Length += cb;
            Marshal.StructureToPtr(src, myptr, false);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Parses a string into the byte array.
        /// </summary>
        /// <param name="input">String to parse.</param>
        /// <returns>Resulting Blob structure or Nothing.</returns>
        /// <remarks></remarks>
        [Description("Parses a string into the byte array.")]
        public static Blob Parse(char[] input)
        {
            return Parse(input.ToString());
        }

        /// <summary>
        /// Parses a string into the byte array.
        /// </summary>
        /// <param name="input">String to parse.</param>
        /// <returns>Resulting Blob structure or Nothing.</returns>
        /// <remarks></remarks>
        [Description("Parses a string into the byte array.")]
        public static Blob Parse(string input)
        {
            ;
#error Cannot convert OnErrorResumeNextStatementSyntax - see comment for details
            /* Cannot convert OnErrorResumeNextStatementSyntax, CONVERSION ERROR: Conversion for OnErrorResumeNextStatement not implemented, please report this issue in 'On Error Resume Next' at character 49918


            Input:
                        On Error Resume Next

             */
            var b = new Blob();
            char ch = 'A';
            Guid gu;
            object d = null;
            if (ParseStrings)
            {
                if (Conversions.ToBoolean(DateTime.TryParse(input, d)))
                {
                    b = BlobConverter.DateToBytes(Conversions.ToDate(d));
                    b.Type = typeof(DateTime);
                    b.TypeLen = Microsoft.VisualBasic.Strings.Len(d);
                    return b;
                }

                if (Guid.TryParse(input, out gu))
                {
                    b.Type = typeof(Guid);
                    b.TypeLen = 16;
                    b = gu.ToByteArray();
                    b.Align();
                    return b;
                }

                if (TryParseObject(input, ref b))
                    return b;
            }

            b = input;
            b.BlobType = BlobTypes.Char;
            return b;
        }

        /// <summary>
        /// Attempts to parse a supported object represented by the string into the Blob.
        /// </summary>
        /// <param name="input">String to attempt to parse.</param>
        /// <param name="blob">Resulting Blob structure or Nothing.</param>
        /// <returns>A value of True for success, otherwise False.</returns>
        /// <remarks></remarks>
        public static bool TryParseObject(string input, ref Blob blob)
        {
            var ft = SystemBlobTypes.Invalid;
            // ' look for numbers
            string[] argvalues1 = null;
            if (input[0] == '{' || Information.IsNumeric(TextTools.JustNumbers(input, maxSkip: 1, values: ref argvalues1)))
            {
                string s = input;
                string[] p;
                if (input[0] == '{')
                {
                    int argstartIndex = -1;
                    s = TextTools.TextBetween(input, "{", "}", startIndex: ref argstartIndex);
                }

                p = TextTools.BatchParse(s, ",");
                int i;
                int c = p.Length - 1;
                object[] objN;
                byte maxB = 4;
                object arrOut;

                // Temporary variables for determining floating-point and unsigned characteristics.
                byte flo = 0;
                bool uns = true;
                Type t;
                BlobTypes bt;

                // Temporary value containers.
                decimal dec;
                double db;
                float sn;
                Guid gu;
                BigInteger bi = default;
                // Temporary value containers.

                objN = new object[c + 1];
                var loopTo = c;
                for (i = 0; i <= loopTo; i++)
                {
                    if (ft == SystemBlobTypes.Guid)
                    {
                        if (!Guid.TryParse(Microsoft.VisualBasic.Strings.Trim(p[i]), out gu))
                        {
                            ft = SystemBlobTypes.Invalid;
                        }
                        else
                        {
                            objN[i] = gu;
                            continue;
                        }
                    }

                    string[] argvalues = null;
                    s = TextTools.JustNumbers(p[i], maxSkip: 1, values: ref argvalues);
                    if (string.IsNullOrEmpty(s))
                        return false;
                    if (flo == 0 && (s.IndexOf(".") >= 0 || s.IndexOf("e") >= 0 || s.IndexOf("E") >= 0))
                        flo = 1;
                    if (float.TryParse(s, out sn))
                    {
                        double.TryParse(s, out db);
                        if (db != sn)
                            maxB = 8;
                    }

                    if (maxB > 4 || !float.TryParse(s, out sn))
                    {
                        if (maxB > 8 || !double.TryParse(s, out db))
                        {
                            if (!decimal.TryParse(s, out dec))
                            {
                                // ' There can't be a number that big with a decimal point, and a BigInteger sits by itself in a blob
                                if (Conversions.ToBoolean(flo) || Conversions.ToBoolean(i))
                                    return false;
                                if (!BigInteger.TryParse(s, out bi))
                                {
                                    return false;
                                }
                                else
                                {
                                    maxB = 100;
                                    flo = 0;
                                    break;
                                }
                            }

                            if (flo == 0)
                                flo = 1;
                            if (maxB < 16)
                                maxB = 16;
                            objN[i] = dec;
                        }
                        else
                        {
                            if (uns && db < 0d)
                                uns = false;
                            if (flo == 0 && (uns && Math.Abs(db) > ulong.MaxValue || !uns && Math.Abs(db) > long.MaxValue))
                                flo = 1;
                            if (maxB < 8)
                                maxB = 8;
                            objN[i] = db;
                        }
                    }
                    else
                    {
                        if (uns && sn < 0f)
                            uns = false;
                        if (flo == 0 && (uns && Math.Abs(sn) > uint.MaxValue || !uns && Math.Abs(sn) > int.MaxValue))
                            maxB = 8;
                        objN[i] = sn;
                        if (maxB < 4)
                            maxB = 4;
                    }
                }

                if (maxB == 100)
                {
                    blob = (Blob)Converter.ConvertFrom(bi);
                    return true;
                }
                else if (maxB == 16)
                {
                    bt = BlobTypes.Decimal;
                }
                else if (Conversions.ToBoolean(flo))
                {
                    if (maxB == 4)
                    {
                        bt = BlobTypes.Single;
                    }
                    else
                    {
                        bt = BlobTypes.Double;
                    }
                }
                else if (maxB == 4)
                {
                    if (uns)
                        bt = BlobTypes.UInteger;
                    else
                        bt = BlobTypes.Integer;
                }
                else if (uns)
                    bt = BlobTypes.ULong;
                else
                    bt = BlobTypes.Long;
                switch (ft)
                {
                    case SystemBlobTypes.UInt16:
                    case SystemBlobTypes.UInt32:
                    case SystemBlobTypes.UInt64:
                        {
                            if (!uns)
                                ft = ft - 1;
                            break;
                        }
                }

                if ((int)ft == (int)BlobTypes.Invalid)
                    t = BlobTypeToType(bt);
                else
                    t = BlobTypeToType((BlobTypes)ft);
                arrOut = Array.CreateInstance(t, c + 1);
                var loopTo1 = c;
                for (i = 0; i <= loopTo1; i++)
                    arrOut(i) = objN[i];
                if (c == 0)
                {
                    blob = (Blob)Converter.ConvertFrom(arrOut((object)0));
                }
                else
                {
                    blob = (Blob)Converter.ConvertFrom(arrOut);
                }

                arrOut = null;
                return true;
            }

            // ' Look for supported parsing types.
            if (input.IndexOf("System.") == 0)
            {
                string s = input.Substring(7);
                int i = 0;
                while (" [{".IndexOf(s[i]) <= -1)
                {
                    i += 1;
                    if (i >= s.Length)
                        break;
                }

                if (i < s.Length)
                {
                    if (Conversions.ToString(s[i]) == "[")
                    {
                        if (Conversions.ToString(s[i + 1]) != "]")
                            return false;
                        input = s.Substring(i + 2);
                    }
                    else
                    {
                        input = s.Substring(i);
                    }

                    s = s.Substring(0, i);
                }
                else
                {
                    return false;
                }

                if (!Enum.TryParse(s, out ft))
                {
                    ft = SystemBlobTypes.Invalid;
                }
            }

            Color cc;

            // ' Try parsing color?
            cc = Color.FromName(input);
            if (cc.ToArgb() == 0 || input.Length >= 7)
            {
                if (input.IndexOf("argb(") == 0)
                {
                    string[] cv;
                    int argstartIndex1 = -1;
                    cv = TextTools.BatchParse(TextTools.TextBetween(input, "(", ")", startIndex: ref argstartIndex1), ",");
                    cc = Color.FromArgb((int)Conversion.Val(cv[0]), (int)Conversion.Val(cv[1]), (int)Conversion.Val(cv[2]), (int)Conversion.Val(cv[3]));
                }
                else if (input.IndexOf("rgba(") == 0)
                {
                    string[] cv;
                    int argstartIndex2 = -1;
                    cv = TextTools.BatchParse(TextTools.TextBetween(input, "(", ")", startIndex: ref argstartIndex2), ",");
                    cc = Color.FromArgb((int)(Conversion.Val(cv[0]) * 255d), (int)Conversion.Val(cv[1]), (int)Conversion.Val(cv[2]), (int)Conversion.Val(cv[3]));
                }
                else if (input.IndexOf("rgb(") == 0)
                {
                    string[] cv;
                    int argstartIndex3 = -1;
                    cv = TextTools.BatchParse(TextTools.TextBetween(input, "(", ")", startIndex: ref argstartIndex3), ",");
                    cc = Color.FromArgb(255, (int)Conversion.Val(cv[1]), (int)Conversion.Val(cv[2]), (int)Conversion.Val(cv[3]));
                }
                else if (input.IndexOf("#") == 0 && input.Length == 7)
                {
                    string cs = input.Substring(1);
                    var cv = new int[3];
                    cv[0] = int.Parse(cs.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                    cv[1] = int.Parse(cs.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                    cv[2] = int.Parse(cs.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                    cc = Color.FromArgb(255, cv[0], cv[1], cv[2]);
                }
            }

            if (cc.ToArgb() != 0)
            {
                blob.Length = 4L;
                blob.BlobType = BlobTypes.Color;
                blob.set_IntegerAt(0L, cc.ToArgb());
                return true;
            }

            return false;
        }

        public new string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool plainNum)
        {
            string ToStringRet = default;
            if (BlobType == BlobTypes.String)
            {
                ToStringRet = GrabString((IntPtr)0);
                return ToStringRet;
            }

            if (StringLength == 0)
                return new string("");
            if (Type == null)
            {
                Type = typeof(byte[]);
                TypeLen = 1;
            }

            if (Type.IsEnum == true || Type.IsArray == true && ElementType.IsEnum == true)
            {
                if (TypeLen == Length)
                {
                    var o = BlobUtil.BytesToEnum(this, Type);
                    if (plainNum)
                        return o.ToString();
                    else
                        return o.ToString() + " {" + Conversions.ToLong(o) + "}";
                }
                else
                {
                    var o = BlobUtil.BytesToEnum(this, Type);
                    string s = "";
                    if (!plainNum)
                        s += Type.FullName;
                    s += "{";
                    foreach (var x in (IEnumerable)o)
                    {
                        if (s.Length > 1)
                            s += ", ";
                        s += x.ToString();
                    }

                    s += "}";
                    return s;
                }
            }

            switch (Type)
            {
                case var @case when @case == typeof(DateTime):
                    {
                        return Conversions.ToString(BlobConverter.BytesToDate(this));
                    }

                case var case1 when case1 == typeof(char[]):
                case var case2 when case2 == typeof(string):
                case var case3 when case3 == typeof(byte):
                    {
                        return Encoding.Unicode.GetString(this);
                    }

                case var case4 when case4 == typeof(Color):
                    {
                        return ((Color)this).ToString();
                    }

                default:
                    {
                        if (IsArray)
                        {
                            if (Conversions.ToBoolean(MaxBlobPrintNum) && Count > MaxBlobPrintNum)
                            {
                                return Type.FullName + "[" + Count + "]";
                            }

                            var ct = Array.CreateInstance(ElementType, 0).GetType();
                            var a = Converter.ConvertTo(this, ct);
                            int i;
                            int c = Conversions.ToInteger(Operators.SubtractObject(a.Length, 1));
                            string s = "";
                            if (!plainNum)
                                s += Type.FullName;
                            s += "{";
                            var loopTo = c;
                            for (i = 0; i <= loopTo; i++)
                            {
                                if (Conversions.ToBoolean(i))
                                    s += ", ";
                                s += a(i).ToString();
                            }

                            s += "}";
                            return s;
                        }
                        else
                        {
                            var a = Converter.ConvertTo(this, Type);
                            return a.ToString();
                        }

                        break;
                    }
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Specifies that we do not append null terminators to strings that are concatenated with this Blob.
        /// Applies only when concatenating a string to an existing blob.
        /// </summary>
        /// <returns></returns>
        public bool StringCatNoNull
        {
            get
            {
                return _StringCatNoNull;
            }

            set
            {
                _StringCatNoNull = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of elements. This property's value depends
        /// on the BlobType.  If the BlobType is Integer, then Length = Count * 4.
        /// </summary>
        [Description("Gets or sets the number of elements.")]
        public int Count
        {
            get
            {
                if (Type is null)
                    Align();
                if (Length == 0L)
                    return 0;
                if (IsBigInteger)
                    return 1;
                return (int)(Length / (double)TypeLen);
            }

            set
            {
                if (_Locked)
                    return;
                if (Type is null)
                    Align();
                if (value == 0)
                {
                    Free();
                    return;
                }

                if (IsBigInteger)
                    return;
                Length = value * TypeLen;
                Align();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to double the size of the BufferExtend with each subsequent increase.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool AutoDouble
        {
            get
            {
                return _AutoDouble;
            }

            set
            {
                _AutoDouble = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the blob behaves as an extending buffer or an absolute memory representation.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool InBufferMode
        {
            get
            {
                bool InBufferModeRet = default;
                InBufferModeRet = _inBuffer;
                return InBufferModeRet;
            }

            set
            {
                _inBuffer = value;
                if (Conversions.ToBoolean(virtLen))
                    Recommit(virtLen);
            }
        }

        /// <summary>
        /// Gets or sets the length of the Blob data stream.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public long Length
        {
            get
            {
                return virtLen;
            }

            set
            {
                if (!ReAlloc(value))
                {
                    throw new OutOfMemoryException("Could not allocate " + value.ToString("N0") + " bytes of data. Try allocating larger contiguous chunks instead of attempting to repeatedly allocate small chunks as memory can be fragmented.");
                }
            }
        }

        public int StringLength
        {
            get
            {
                if (handle == (IntPtr)0)
                    return 0;
                return Native.CharZero(handle);
            }
        }

        /// <summary>
        /// Returns the actual length of the buffer as initialized by the operating system.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public long ActualLength
        {
            get
            {
                return actualLen;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates the buffer extension threshold, in bytes.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public long BufferExtend
        {
            get
            {
                return _bufferExtend;
            }

            set
            {
                if (value != _bufferExtend)
                {
                    _bufferExtend = value;
                    ReAlloc(virtLen);
                }
            }
        }

        /// <summary>
        /// Returns the BlobType.
        /// </summary>
        [Description("Returns the BlobType.")]
        public BlobTypes BlobType
        {
            get
            {
                if (Type == null)
                    Align();
                return _BlobType;
            }

            set
            {
                if (value <= BlobTypes.Invalid | (int)value > (int)BlobConst.UBound)
                    return;
                TypeAlign(BlobTypeToType(value));
            }
        }

        /// <summary>
        /// Defines a threshold after which arrays will not convert to strings, or zero for no limit (default).
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Description("Defines a threshold after which arrays will not convert to strings, or zero for no limit (default).")]
        public int MaxBlobPrintNum
        {
            get
            {
                return _MaxBlobPrintNum;
            }

            set
            {
                _MaxBlobPrintNum = value;
            }
        }

        /// <summary>
        /// Gets each value as if part of an array.  Returns a new blob containing the bytes for exactly that element.
        /// </summary>
        /// <param name="index"></param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Description("Gets each value as if part of an array.  Returns a new blob containing the bytes for exactly that element.")]
        public new Blob this[int index]
        {
            get
            {
                if (Length == 0L)
                    return null;
                int c = index * TypeLen;
                var bl = new Blob();
                bl.Length = TypeLen;
                bl.SetBytes((IntPtr)0, GrabBytes((IntPtr)c, TypeLen));
                bl.Type = ElementType;
                bl.TypeLen = TypeLen;
                return bl;
            }
        }

        /// <summary>
        /// Gets the last known type of the contained value.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Description("Gets the last known type of the contained value.")]
        public Type BaseType
        {
            get
            {
                if (Type is null)
                    Align();
                return Type;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the virtual element is a floating point number.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [Description("Gets a value indicating whether the virtual element is a floating point number.")]
        public bool IsFloat
        {
            get
            {
                if (Type is null)
                    Align();
                var t = BlobType;
                return t >= BlobTypes.Single & t <= BlobTypes.Decimal;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the virtual element is an array. Returns true for strings with more than one character since all strings are interally referred to as Char
        /// </summary>
        [Description("Gets a value indicating whether the virtual element is an array. Returns true for strings with more than one character since all strings are interally referred to as Char[]")]
        public bool IsArray
        {
            get
            {
                if (Type is null)
                    Align();
                return BaseType.IsArray;
            }

            set
            {
                if (_Locked)
                    return;
                object o;
                if (value == false && Type is object && Type.IsArray == true)
                {
                    Type = Type.GetElementType();
                    o = Array.CreateInstance(Type, 1);
                    Length = Microsoft.VisualBasic.Strings.Len(o((object)0));
                    o = null;
                }
                else if (value == true && Type is object && Type.IsArray == false)
                {
                    o = Array.CreateInstance(Type, 1);
                    Type = o.GetType();
                    o = null;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the virtual element is a System.Numerics.BigInteger.
        /// </summary>
        [Description("Gets a value indicating whether the virtual element is a System.Numerics.BigInteger.")]
        public bool IsBigInteger
        {
            get
            {
                if (Type is null)
                    Align();
                return BlobType == BlobTypes.BigInteger;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the virtual element is numeric
        /// </summary>
        [Description("Gets a value indicating whether the virtual element is numeric (this behavior differs from the system IsNumeric function in that Char is treated as a string. Use the TypeAlign method to force the blob to consider itself a short.")]
        public bool IsNumber
        {
            get
            {
                if (Type is null)
                    Align();
                return (int)BlobType <= (int)BlobConst.MaxMath;
            }
        }

        /// <summary>
        /// Gets or sets
        /// </summary>
        [Description("Gets or sets = a value indicating whether the virtual element is treated as a string.  If set to false, the blob converts to UShort.")]
        public bool IsString
        {
            get
            {
                switch (_BlobType)
                {
                    case BlobTypes.Char:
                    case BlobTypes.String:
                    case BlobTypes.NtString:
                        {
                            return true;
                        }

                    default:
                        {
                            return false;
                        }
                }
            }

            set
            {
                if (value == true)
                {
                    Type = Types[(int)BlobTypes.Char];
                }
                else if (ElementType == typeof(char))
                {
                    Type = Types[(int)BlobTypes.UShort];
                }

                Align();
            }
        }

        /// <summary>
        /// Gets the last known element type of the contained value.
        /// </summary>
        [Description("Gets the last known element type of the contained value.")]
        public Type ElementType
        {
            get
            {
                if (Type is null)
                    Align();
                return (Type)Interaction.IIf(IsArray, Type.GetElementType(), Type);
            }
        }

        /// <summary>
        /// Gets the size
        /// </summary>
        [Description("Gets the size (in bytes) of each element.")]
        public int ElementSize
        {
            get
            {
                if (Type is null)
                    Align();
                return TypeLen;
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Coerces the blob into the specific type.  This can result in unpredictable behavior if not used carefully.  Align() is always called in TypeAlign.
        /// </summary>
        [Description("Coerces the blob into the specific type.  This can result in unpredictable behavior if not used carefully. Align() is always called in TypeAlign.")]
        public void TypeAlign(Type type)
        {
            Type = type;
            Align();
        }

        /// <summary>
        /// Align the byte array to the byte length of the current type.  If the byte array is empty, it is initialized to contain exactly 1 element of the current type.  Align also performs array to scalar conversion when the condition that Length
        /// </summary>
        [Description("Align the byte array to the byte length of the current type.  If the byte array is empty, it is initialized to contain exactly 1 element of the current type.  Align also performs array to scalar conversion when the condition that Length=TypeLen is met, and scalar to array conversion when the condition is not met.")]
        public void Align()
        {
            if (Type == null)
                Type = typeof(byte[]);
            // If Type = GetType(String) Then Type = GetType(Char())

            TypeLen = BlobTypeSize(TypeToBlobType(Type));
            _BlobType = TypeToBlobType(ElementType);
            if (Length == 0L)
            {
                return;
            }

            long n = Length % TypeLen;
            if (!_Locked)
            {
                if (n != 0L)
                {
                    n = Length + (TypeLen - n);
                    Length = n;
                }
            }

            if (ArrayTypes[(int)_BlobType] is object && Length > TypeLen && !Type.IsArray)
            {
                Type = ArrayTypes[(int)_BlobType];
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public Image GetImage(IntPtr byteIndex)
        {
            Image GetImageRet = default;
            MemPtr mm = handle + (int)byteIndex;
            GetImageRet = Image.FromStream(new MemoryStream((byte[])mm));
            return GetImageRet;
        }

        public void SetImage(IntPtr byteIndex, Image image, System.Drawing.Imaging.ImageFormat format = null)
        {
            if (format is null)
            {
                format = System.Drawing.Imaging.ImageFormat.Png;
            }

            var ms = new MemoryStream();
            image.Save(ms, format);
            byte[] by;
            by = new byte[(int)(ms.Length - 1L + 1)];
            ms.Read(by, 0, by.Length);
            ms.Dispose();
            if (IsInvalid || Length < by.Length + Conversions.ToLong(byteIndex))
            {
                Length = (long)((IntPtr)by.Length + (int)byteIndex);
            }

            Native.MemCpy(handle + (int)byteIndex, by, (uint)by.Length);
        }

        public Image get_ImageAt(long byteIndex)
        {
            Image ImageAtRet = default;
            ImageAtRet = GetImage((IntPtr)byteIndex);
            return ImageAtRet;
        }

        public void set_ImageAt(long byteIndex, Image value)
        {
            SetImage((IntPtr)byteIndex, value);
        }

        public Color get_ColorAt(long byteIndex)
        {
            return Color.FromArgb(get_IntegerAt(byteIndex));
        }

        public void set_ColorAt(long byteIndex, Color value)
        {
            set_IntegerAt(byteIndex, value.ToArgb());
        }

        public Color get_ColorAtAbsolute(long byteIndex)
        {
            return Color.FromArgb(get_IntegerAtAbsolute(byteIndex));
        }

        public void set_ColorAtAbsolute(long byteIndex, Color value)
        {
            set_IntegerAtAbsolute(byteIndex, value.ToArgb());
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // ' To do!

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Clears the entire object.
        /// </summary>
        /// <remarks></remarks>
        public void Clear()
        {
            Free();
            BlobType = BlobTypes.Byte;
        }

        /// <summary>
        /// Allocate a new memory buffer on the process heap.
        /// </summary>
        /// <param name="length">Length of the new buffer, in bytes.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool Alloc(long length)
        {
            bool AllocRet = default;
            if (length <= 0L)
                return Free();
            if (IsInvalid == false)
            {
                return ReAlloc(length);
            }

            Recommit(length);
            switch (_MemType)
            {
                case MemAllocType.Virtual:
                    {
                        return VirtualAlloc(actualLen);
                    }

                case MemAllocType.Network:
                    {
                        return NetAlloc((int)(actualLen & 0x7FFFFFFFIL));
                    }

                default:
                    {
                        // Threading.Monitor.Enter(Me)

                        if (Native.HeapValidate(hHeap, 0U, (IntPtr)0))
                        {
                            handle = Native.HeapAlloc(hHeap, 8U, (IntPtr)actualLen);
                            actualLen = (long)Native.HeapSize(hHeap, 0U, handle);
                            AllocRet = handle != (IntPtr)0;
                        }
                        else
                        {
                            AllocRet = false;
                        }

                        break;
                    }

                    // Threading.Monitor.Exit(Me)

            }

            if (AllocRet)
            {
                _MemType = MemAllocType.Heap;
                GC.AddMemoryPressure(actualLen);
            }
            else
            {
                actualLen = 0L;
                virtLen = 0L;
            }

            return AllocRet;
        }

        /// <summary>
        /// Reallocate a memory buffer with a new size on the process heap.
        /// </summary>
        /// <param name="length">New length of the memory buffer.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool ReAlloc(long length)
        {
            bool ReAllocRet = default;
            if (IsInvalid)
                return Alloc(length);
            long oldVirt = virtLen;
            long oldLen = actualLen;
            Recommit(length);
            if (actualLen == oldLen)
            {
                // ' it is very important to be conservative here:
                if (virtLen < oldVirt)
                    ZeroMemory(virtLen, oldVirt - virtLen);
                return true;
            }

            if (_MemType == MemAllocType.Heap)
            {
                try
                {
                    // Threading.Monitor.Enter(Me)
                    if (Native.HeapValidate(hHeap, 0U, handle))
                    {
                        var p = Native.HeapReAlloc(hHeap, 8, handle, (IntPtr)actualLen);
                        if (p != (IntPtr)0)
                        {
                            handle = p;
                            actualLen = (long)Native.HeapSize(hHeap, 0U, handle);
                            ReAllocRet = true;
                        }
                        else
                        {
                            actualLen = oldLen;
                            virtLen = oldVirt;
                            ReAllocRet = false;
                        }
                    }
                    else
                    {
                        actualLen = oldLen;
                        virtLen = oldVirt;
                        ReAllocRet = false;
                    }
                }

                // Threading.Monitor.Exit(Me)

                catch (Exception ex)
                {
                    actualLen = oldLen;
                    virtLen = oldVirt;
                    ReAllocRet = false;
                }
            }
            else if (_MemType == MemAllocType.Network)
            {

                // Threading.Monitor.Enter(Me)

                var mm = new MemPtr();
                mm.NetAlloc((int)actualLen);
                if (mm.Handle == (IntPtr)0)
                {
                    actualLen = oldLen;
                    virtLen = oldVirt;
                    ReAllocRet = false;
                }
                else
                {
                    GC.AddMemoryPressure(actualLen);
                    Native.MemCpy(mm.Handle, handle, (uint)oldVirt);
                    Native.NetApiBufferFree(handle);
                    GC.RemoveMemoryPressure(oldLen);
                    handle = mm.Handle;
                    ReAllocRet = true;
                }

                // Threading.Monitor.Exit(Me)
                return ReAllocRet;
            }
            else if (_MemType == MemAllocType.Virtual)
            {

                // Threading.Monitor.Enter(Me)

                var mm = new MemPtr();
                mm.VirtualAlloc(actualLen);
                if (mm.Handle == (IntPtr)0)
                {
                    actualLen = oldLen;
                    virtLen = oldVirt;
                    ReAllocRet = false;
                }
                else
                {
                    Native.MemCpy(mm.Handle, handle, (uint)oldVirt);
                    VirtualFree();
                    _MemType = MemAllocType.Virtual;
                    handle = mm.Handle;
                    actualLen = VirtualLength();
                    ReAllocRet = true;
                }
            }

            // Threading.Monitor.Exit(Me)

            else if (_MemType == MemAllocType.Com)
            {

                // Threading.Monitor.Enter(Me)

                MemPtr mm = Marshal.AllocCoTaskMem((int)actualLen);
                if (mm.Handle == (IntPtr)0)
                {
                    actualLen = oldLen;
                    virtLen = oldVirt;
                    ReAllocRet = false;
                }
                else
                {
                    Native.MemCpy(mm.Handle, handle, (uint)oldVirt);
                    Marshal.FreeCoTaskMem(handle);
                    handle = mm.Handle;
                    ReAllocRet = true;
                }
            }

            // Threading.Monitor.Exit(Me)
            else
            {
                return false;
            }

            if (ReAllocRet)
            {
                actualLen = ActualLength;
                if (actualLen > oldLen)
                {
                    GC.AddMemoryPressure(actualLen - oldLen);
                }
                else
                {
                    GC.RemoveMemoryPressure(oldLen - actualLen);
                }
            }

            return ReAllocRet;
        }

        /// <summary>
        /// Frees the resources allocated by the current object.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Free()
        {
            bool FreeRet = default;
            FreeRet = handle == (IntPtr)0;
            long al;
            al = ActualLength;
            switch (_MemType)
            {
                case MemAllocType.Heap:
                    {

                        // Threading.Monitor.Enter(Me)

                        if (Native.HeapValidate(hHeap, 0U, handle))
                        {
                            FreeRet = Conversions.ToInteger(Native.HeapFree(hHeap, 0U, handle)) != 0;
                        }

                        break;
                    }

                // Threading.Monitor.Exit(Me)

                case MemAllocType.Virtual:
                    {
                        FreeRet = VirtualFree();
                        return FreeRet;
                    }

                case MemAllocType.Network:
                    {
                        FreeRet = NetFree();
                        return FreeRet;
                    }

                case MemAllocType.Com:
                    {
                        try
                        {
                            Marshal.FreeCoTaskMem(handle);
                            FreeRet = true;
                        }
                        catch (Exception ex)
                        {
                        }

                        break;
                    }
            }

            if (FreeRet)
            {
                handle = IntPtr.Zero;
                virtLen = 0L;
                actualLen = 0L;
                if (Conversions.ToBoolean(al))
                    GC.RemoveMemoryPressure(al);
                _MemType = MemAllocType.Invalid;
            }

            return FreeRet;
        }

        /// <summary>
        /// Calculate the comitted memory based on buffering specifications.
        /// This function should only be used as part of an internal allocation, because it changes global variables.
        /// </summary>
        /// <returns>The number of bytes to actually commit using the given memory allocation function.</returns>
        /// <remarks></remarks>
        private long Recommit(long len)
        {
            long RecommitRet = default;
            if (!_inBuffer)
            {
                actualLen = len;
                virtLen = len;
                RecommitRet = len;
                return RecommitRet;
            }

            if (len <= actualLen)
            {
                virtLen = len;
            }
            else
            {
                actualLen += len + (_bufferExtend - len % _bufferExtend);
                if (_AutoDouble)
                    _bufferExtend *= 2L;
                virtLen = len;
            }

            if (_MemType == MemAllocType.Network && _bufferExtend > int.MaxValue)
                _bufferExtend = SystemInformation.SysInfo.SystemInfo.dwPageSize;
            RecommitRet = actualLen;
            return RecommitRet;
        }

        private bool CleanupBuffer()
        {
            bool CleanupBufferRet = default;
            if (!_inBuffer)
            {
                if (virtLen != 0L)
                {
                    ReAlloc(virtLen);
                }

                return false;
            }

            if (virtLen < _bufferExtend)
            {
                CleanupBufferRet = ReAlloc(_bufferExtend);
                if (_AutoDouble)
                {
                    _bufferExtend = SystemInformation.SysInfo.SystemInfo.dwPageSize;
                }

                return CleanupBufferRet;
            }

            return true;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Allocate a network API compatible memory buffer.
        /// </summary>
        /// <param name="size">Size of the buffer to allocate, in bytes.</param>
        /// <remarks></remarks>
        public bool NetAlloc(int size)
        {
            bool NetAllocRet = default;
            // ' just ignore a full buffer.
            if (handle != (IntPtr)0)
                return true;
            long l = Recommit(size);
            Native.NetApiBufferAllocate((int)actualLen, ref handle);
            if (handle != (IntPtr)0)
            {
                NetAllocRet = true;
                _MemType = MemAllocType.Network;
                GC.AddMemoryPressure(actualLen);
            }
            else
            {
                actualLen = 0L;
                virtLen = 0L;
                handle = (IntPtr)0;
                NetAllocRet = false;
            }

            return NetAllocRet;
        }

        /// <summary>
        /// Free a network API compatible memory buffer previously allocated with NetAlloc.
        /// </summary>
        /// <remarks></remarks>
        public bool NetFree()
        {
            bool NetFreeRet = default;
            if (_MemType != MemAllocType.Network)
                return false;
            if (handle == (IntPtr)0)
                return true;
            NetFreeRet = Native.NetApiBufferFree(handle) == 0;
            if (NetFreeRet)
            {
                GC.RemoveMemoryPressure(actualLen);
                virtLen = 0L;
                actualLen = 0L;
                handle = (IntPtr)0;
            }

            return NetFreeRet;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Allocates a region of virtual memory.
        /// </summary>
        /// <param name="size">The size of the region of memory to allocate.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool VirtualAlloc(long size)
        {
            bool VirtualAllocRet = default;
            if (handle != (IntPtr)0)
                return false;
            if (size == 0L)
            {
                return VirtualFree();
            }

            long l = Recommit(size);
            handle = Native.VirtualAlloc((IntPtr)0, (IntPtr)l, VMemAllocFlags.MEM_COMMIT | VMemAllocFlags.MEM_RESERVE, MemoryProtectionFlags.PAGE_READWRITE);
            VirtualAllocRet = handle != (IntPtr)0;
            if (VirtualAllocRet)
            {
                actualLen = VirtualLength();
                virtLen = size;
                GC.AddMemoryPressure(actualLen);
            }
            else
            {
                actualLen = 0L;
                virtLen = 0L;
                handle = (IntPtr)0;
            }

            return VirtualAllocRet;
        }

        /// <summary>
        /// Frees a region of memory previously allocated with VirtualAlloc.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool VirtualFree()
        {
            bool VirtualFreeRet = default;
            if (_MemType != MemAllocType.Virtual)
                return false;
            long l;

            // ' While the function doesn't need to call VirtualFree, it hasn't necessarily failed, either.
            if (handle == (IntPtr)0)
            {
                VirtualFreeRet = true;
            }
            else
            {
                // ' see if we need to tell the garbage collector anything.
                l = VirtualLength();
                VirtualFreeRet = Native.VirtualFree(handle);

                // ' see if we need to tell the garbage collector anything.
                if (VirtualFreeRet)
                {
                    handle = (IntPtr)0;
                    GC.RemoveMemoryPressure(l);
                }
            }

            return VirtualFreeRet;
        }

        /// <summary>
        /// Returns the size of a region of virtual memory previously allocated with VirtualAlloc.
        /// </summary>
        /// <returns>The size of a virtual memory region or zero.</returns>
        /// <remarks></remarks>
        public long VirtualLength()
        {
            if (_MemType != MemAllocType.Virtual)
                return 0L;
            if (handle == (IntPtr)0)
                return 0L;
            var m = new MEMORY_BASIC_INFORMATION();
            if (Native.VirtualQuery(handle, ref m, (IntPtr)Marshal.SizeOf(m)) != (IntPtr)0)
            {
                return (long)m.RegionSize;
            }

            return 0L;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Clears all the memory
        /// </summary>
        /// <remarks></remarks>
        public virtual void ZeroMemory()
        {
            try
            {
                long l = Length;
                if (Conversions.ToBoolean(l & 0xFFFFFFFF00000000L))
                {
                    Native.n_memset(handle, 0, (IntPtr)l);
                }
                else
                {
                    Native.MemSet(handle, 0, (uint)l);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Clears all memory to the specified length.
        /// </summary>
        /// <param name="length">Length of memory to clear.</param>
        /// <remarks></remarks>
        public virtual void ZeroMemory(long length)
        {
            try
            {
                if (Conversions.ToBoolean(length & 0xFFFFFFFF00000000L))
                {
                    Native.n_memset(handle, 0, (IntPtr)length);
                }
                else
                {
                    Native.MemSet(handle, 0, (uint)length);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Clears all the memory starting at the specified byte index for the specified length.
        /// </summary>
        /// <param name="byteIndex">Byte index, relative to the memory pointer, at which to begin clearing.</param>
        /// <param name="length">Length of memory to clear.</param>
        /// <remarks></remarks>
        public virtual void ZeroMemory(long byteIndex, long length)
        {
            try
            {
                if (Conversions.ToBoolean(length & 0xFFFFFFFF00000000L))
                {
                    Native.n_memset(handle + (int)byteIndex, 0, (IntPtr)length);
                }
                else
                {
                    Native.MemSet(handle + (int)byteIndex, 0, (uint)length);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // ' Blob
        public new bool Equals(object obj)
        {
            if (obj is Blob)
            {
                return ((Blob)obj).handle == handle;
            }
            else if (obj is IntPtr)
            {
                return (IntPtr)obj == handle;
            }
            else if (obj is UIntPtr)
            {
                return Native.ToSigned((UIntPtr)obj) == handle;
            }
            else
            {
                return false;
            }
        }

        public bool Equals(SafeHandle other)
        {
            return other.DangerousGetHandle() == handle;
        }

        public bool Equals(UIntPtr other)
        {
            return Native.ToSigned(other) == handle;
        }

        public bool Equals(Blob other)
        {
            return other.handle == handle;
        }

        public bool Equals(IntPtr other)
        {
            return other == handle;
        }

        public bool Equals(MemPtr other)
        {
            return other == handle;
        }

        public bool Equals(SafePtr other)
        {
            return other.DangerousGetHandle() == handle;
        }

        public static bool operator ==(Blob v1, Blob v2)
        {
            return (long)v1.handle == (long)v2.handle;
        }

        public static bool operator !=(Blob v1, Blob v2)
        {
            return (long)v1.handle != (long)v2.handle;
        }

        public static bool operator <(Blob v1, Blob v2)
        {
            return (long)v1.handle < (long)v2.handle;
        }

        public static bool operator >(Blob v1, Blob v2)
        {
            return (long)v1.handle > (long)v2.handle;
        }

        public static bool operator <=(Blob v1, Blob v2)
        {
            return (long)v1.handle <= (long)v2.handle;
        }

        public static bool operator >=(Blob v1, Blob v2)
        {
            return (long)v1.handle >= (long)v2.handle;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        // ' Math on blob may or may not be reimplemented. Time will tell.
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public static Blob operator +(Blob operand1, Blob operand2)
        {
            if (operand2 is null || operand2.Length == 0L)
                return operand1;
            if (operand1 is null)
                operand1 = new Blob();
            IntPtr l = (IntPtr)operand1.Length;
            IntPtr l2 = (IntPtr)operand2.Length;
            operand1.Length = operand1.Length + (int)l2;
            if (l2.ToInt64() > uint.MaxValue)
            {
                Native.n_memcpy(operand1.handle + (int)l, operand2.handle, (UIntPtr)l2.ToInt64());
            }
            else
            {
                Native.MemCpy(operand1.handle + (int)l, operand2.handle, (uint)l2);
            }

            return operand1;
        }

        public static Blob operator +(Blob operand1, byte[] operand2)
        {
            if (operand2 is null || operand2.Length == 0)
                return operand1;
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            int l2 = operand2.Length;
            operand1.Length += l2;
            Native.MemCpy(operand1.handle + (int)l, operand2, (uint)l2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, sbyte[] operand2)
        {
            if (operand2 is null || operand2.Length == 0)
                return operand1;
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            int l2 = operand2.Length;
            operand1.Length += l2;
            Native.MemCpy(operand1.handle + (int)l, operand2, (uint)l2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, short[] operand2)
        {
            if (operand2 is null || operand2.Length == 0)
                return operand1;
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            int l2 = operand2.Length * 2;
            operand1.Length += l2;
            Native.MemCpy(operand1.handle + (int)l, operand2, (uint)l2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, ushort[] operand2)
        {
            if (operand2 is null || operand2.Length == 0)
                return operand1;
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            int l2 = operand2.Length * 2;
            operand1.Length += l2;
            Native.MemCpy(operand1.handle + (int)l, operand2, (uint)l2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, int[] operand2)
        {
            if (operand2 is null || operand2.Length == 0)
                return operand1;
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            int l2 = operand2.Length * 4;
            operand1.Length += l2;
            Native.MemCpy(operand1.handle + (int)l, operand2, (uint)l2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, uint[] operand2)
        {
            if (operand2 is null || operand2.Length == 0)
                return operand1;
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            int l2 = operand2.Length * 4;
            operand1.Length += l2;
            Native.MemCpy(operand1.handle + (int)l, operand2, (uint)l2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, long[] operand2)
        {
            if (operand2 is null || operand2.Length == 0)
                return operand1;
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            int l2 = operand2.Length * 8;
            operand1.Length += l2;
            Native.MemCpy(operand1.handle + (int)l, operand2, (uint)l2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, ulong[] operand2)
        {
            if (operand2 is null || operand2.Length == 0)
                return operand1;
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            int l2 = operand2.Length * 8;
            operand1.Length += l2;
            Native.MemCpy(operand1.handle + (int)l, operand2, (uint)l2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, float[] operand2)
        {
            if (operand2 is null || operand2.Length == 0)
                return operand1;
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            int l2 = operand2.Length * 4;
            operand1.Length += l2;
            Native.MemCpy(operand1.handle + (int)l, operand2, (uint)l2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, double[] operand2)
        {
            if (operand2 is null || operand2.Length == 0)
                return operand1;
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            int l2 = operand2.Length * 8;
            operand1.Length += l2;
            Native.MemCpy(operand1.handle + (int)l, operand2, (uint)l2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, decimal[] operand2)
        {
            if (operand2 is null || operand2.Length == 0)
                return operand1;
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            int l2 = operand2.Length * 16;
            operand1.Length += l2;
            Native.MemCpy(operand1.handle + (int)l, operand2, (uint)l2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, Guid[] operand2)
        {
            if (operand2 is null || operand2.Length == 0)
                return operand1;
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            int l2 = operand2.Length * 16;
            operand1.Length += l2;
            Native.MemCpy(operand1.handle + (int)l, operand2, (uint)l2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, Color[] operand2)
        {
            if (operand2 is null || operand2.Length == 0)
                return operand1;
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            int l2 = operand2.Length * 4;
            operand1.Length += l2;
            QuickCopyObject<Color[]>(operand1.handle + (int)l, operand2, (uint)l2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, byte operand2)
        {
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            operand1.Length += 1L;
            operand1.set_ByteAt(l, operand2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, sbyte operand2)
        {
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            operand1.Length += 1L;
            operand1.set_SByteAt(l, operand2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, short operand2)
        {
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            operand1.Length += 2L;
            operand1.set_ShortAtAbsolute(l, operand2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, ushort operand2)
        {
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            operand1.Length += 2L;
            operand1.set_UShortAtAbsolute(l, operand2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, int operand2)
        {
            if (operand1 is null)
                operand1 = new Blob();
            long p = operand1.virtLen;
            long l = p + 4L;
            if (l > operand1.actualLen)
            {
                operand1.ReAlloc(l);
            }
            else
            {
                operand1.virtLen = l;
            }

            operand1.set_IntegerAtAbsolute(p, operand2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, uint operand2)
        {
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            operand1.Length += 4L;
            operand1.set_UIntegerAtAbsolute(l, operand2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, long operand2)
        {
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            operand1.Length += 8L;
            operand1.set_LongAtAbsolute(l, operand2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, ulong operand2)
        {
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            operand1.Length += 8L;
            operand1.set_ULongAtAbsolute(l, operand2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, float operand2)
        {
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            operand1.Length += 4L;
            operand1.set_SingleAtAbsolute(l, operand2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, double operand2)
        {
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            operand1.Length += 8L;
            operand1.set_DoubleAtAbsolute(l, operand2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, decimal operand2)
        {
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            operand1.Length += 16L;
            operand1.set_DecimalAtAbsolute(l, operand2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, Guid operand2)
        {
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            operand1.Length += 16L;
            operand1.set_GuidAtAbsolute(l, operand2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, string operand2)
        {
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            long c;
            c = operand2.Length * 2;
            if (operand1.StringCatNoNull == false)
            {
                c += 2L;
            }

            operand1.Length += c;
            operand1.SetString((IntPtr)l, operand2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, Color operand2)
        {
            if (operand1 is null)
                operand1 = new Blob();
            long l = operand1.Length;
            operand1.Length += 4L;
            operand1.set_ColorAtAbsolute(l, operand2);
            return operand1;
        }

        public static Blob operator +(Blob operand1, Image operand2)
        {
            if (operand1 is null)
                operand1 = new Blob();
            var bl = new Blob();
            bl.SetImage((IntPtr)0, operand2);
            long l = operand1.Length;
            operand1.Length += bl.Length;
            Native.MemCpy(operand1.handle + (int)l, bl.handle, (uint)bl.Length);
            bl.Free();
            return operand1;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public static byte operator +(byte operand1, Blob operand2)
        {
            byte res = operand2.get_ByteAt(operand2._ClipNext);
            operand2._ClipNext += 1;
            return res;
        }

        public static sbyte operator +(sbyte operand1, Blob operand2)
        {
            sbyte res = operand2.get_SByteAt(operand2._ClipNext);
            operand2._ClipNext += 1;
            return res;
        }

        public static ushort operator +(ushort operand1, Blob operand2)
        {
            ushort res = operand2.get_UShortAtAbsolute(operand2._ClipNext);
            operand2._ClipNext += 2;
            return res;
        }

        public static short operator +(short operand1, Blob operand2)
        {
            short res = operand2.get_ShortAtAbsolute(operand2._ClipNext);
            operand2._ClipNext += 2;
            return res;
        }

        public static uint operator +(uint operand1, Blob operand2)
        {
            uint res = operand2.get_UIntegerAtAbsolute(operand2._ClipNext);
            operand2._ClipNext += 4;
            return res;
        }

        public static int operator +(int operand1, Blob operand2)
        {
            int res = operand2.get_IntegerAtAbsolute(operand2._ClipNext);
            operand2._ClipNext += 4;
            return res;
        }

        public static ulong operator +(ulong operand1, Blob operand2)
        {
            ulong res = operand2.get_ULongAtAbsolute(operand2._ClipNext);
            operand2._ClipNext += 8;
            return res;
        }

        public static long operator +(long operand1, Blob operand2)
        {
            long res = operand2.get_LongAtAbsolute(operand2._ClipNext);
            operand2._ClipNext += 8;
            return res;
        }

        public static float operator +(float operand1, Blob operand2)
        {
            float res = operand2.get_SingleAtAbsolute(operand2._ClipNext);
            operand2._ClipNext += 4;
            return res;
        }

        public static double operator +(double operand1, Blob operand2)
        {
            double res = operand2.get_DoubleAtAbsolute(operand2._ClipNext);
            operand2._ClipNext += 8;
            return res;
        }

        public static decimal operator +(decimal operand1, Blob operand2)
        {
            decimal res = operand2.get_DecimalAtAbsolute(operand2._ClipNext);
            operand2._ClipNext += 16;
            return res;
        }

        public static Guid operator +(Guid operand1, Blob operand2)
        {
            var res = operand2.get_GuidAtAbsolute(operand2._ClipNext);
            operand2._ClipNext += 16;
            return res;
        }

        public static string operator +(string operand1, Blob operand2)
        {
            string res = operand2.GrabString((IntPtr)operand2._ClipNext);
            operand2._ClipNext += (res.Length + 1) * 2;
            return res;
        }

        public static short[] operator +(short[] operand1, Blob operand2)
        {
            short[] res = operand2;
            int l = operand1.Length;
            Array.Resize(ref operand1, operand1.Length + res.Length);
            Array.Copy(res, 0, operand1, l, res.Length);
            return operand1;
        }

        public static ushort[] operator +(ushort[] operand1, Blob operand2)
        {
            ushort[] res = operand2;
            int l = operand1.Length;
            Array.Resize(ref operand1, operand1.Length + res.Length);
            Array.Copy(res, 0, operand1, l, res.Length);
            return operand1;
        }

        public static int[] operator +(int[] operand1, Blob operand2)
        {
            int[] res = operand2;
            int l = operand1.Length;
            Array.Resize(ref operand1, operand1.Length + res.Length);
            Array.Copy(res, 0, operand1, l, res.Length);
            return operand1;
        }

        public static uint[] operator +(uint[] operand1, Blob operand2)
        {
            uint[] res = operand2;
            int l = operand1.Length;
            Array.Resize(ref operand1, operand1.Length + res.Length);
            Array.Copy(res, 0, operand1, l, res.Length);
            return operand1;
        }

        public static long[] operator +(long[] operand1, Blob operand2)
        {
            long[] res = operand2;
            int l = operand1.Length;
            Array.Resize(ref operand1, operand1.Length + res.Length);
            Array.Copy(res, 0, operand1, l, res.Length);
            return operand1;
        }

        public static ulong[] operator +(ulong[] operand1, Blob operand2)
        {
            ulong[] res = operand2;
            int l = operand1.Length;
            Array.Resize(ref operand1, operand1.Length + res.Length);
            Array.Copy(res, 0, operand1, l, res.Length);
            return operand1;
        }

        public static float[] operator +(float[] operand1, Blob operand2)
        {
            float[] res = operand2;
            int l = operand1.Length;
            Array.Resize(ref operand1, operand1.Length + res.Length);
            Array.Copy(res, 0, operand1, l, res.Length);
            return operand1;
        }

        public static double[] operator +(double[] operand1, Blob operand2)
        {
            double[] res = operand2;
            int l = operand1.Length;
            Array.Resize(ref operand1, operand1.Length + res.Length);
            Array.Copy(res, 0, operand1, l, res.Length);
            return operand1;
        }

        public static Guid[] operator +(Guid[] operand1, Blob operand2)
        {
            Guid[] res = (Guid[])operand2;
            int l = operand1.Length;
            Array.Resize(ref operand1, operand1.Length + res.Length);
            Array.Copy(res, 0, operand1, l, res.Length);
            return operand1;
        }

        public static decimal[] operator +(decimal[] operand1, Blob operand2)
        {
            decimal[] res = operand2;
            int l = operand1.Length;
            Array.Resize(ref operand1, operand1.Length + res.Length);
            Array.Copy(res, 0, operand1, l, res.Length);
            return operand1;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public IEnumerator<byte> GetEnumerator()
        {
            IEnumerator<byte> GetEnumeratorRet = default;
            GetEnumeratorRet = new BlobEnumeratorByte(this);
            return GetEnumeratorRet;
        }

        public IEnumerator GetEnumerator1()
        {
            IEnumerator GetEnumerator1Ret = default;
            GetEnumerator1Ret = new BlobEnumeratorByte(this);
            return GetEnumerator1Ret;
        }

        public IEnumerator<char> GetEnumerator2()
        {
            IEnumerator<char> GetEnumerator2Ret = default;
            GetEnumerator2Ret = new BlobEnumeratorChar(this);
            return GetEnumerator2Ret;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public object Clone()
        {
            object CloneRet = default;
            var bl = new Blob();
            long l = Length;
            bl._MemType = _MemType;
            switch (_MemType)
            {
                case MemAllocType.Virtual:
                    {
                        bl.VirtualAlloc(l);
                        break;
                    }

                case MemAllocType.Com:
                    {
                        bl.handle = Marshal.AllocCoTaskMem((int)l);
                        bl.virtLen = l;
                        break;
                    }

                case MemAllocType.Network:
                    {
                        bl.NetAlloc((int)l);
                        break;
                    }

                default:
                    {
                        bl.Alloc(l);
                        break;
                    }
            }

            if (bl.IsInvalid)
                return null;
            if (l > int.MaxValue)
            {
                Native.n_memcpy(bl.handle, handle, (UIntPtr)l);
            }
            else
            {
                Native.MemCpy(bl.handle, handle, (uint)l);
            }

            CloneRet = bl;
            return CloneRet;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Copies the buffer into the specified byte array starting at the optional startIndex
        /// </summary>
        [Description("Copies the buffer into the specified byte array starting at the optional startIndex")]
        public void CopyTo(byte[] destinationArray, int startIndex = 0)
        {
            var gh = GCHandle.Alloc(destinationArray, GCHandleType.Pinned);
            var gpt = gh.AddrOfPinnedObject() + startIndex;
            Native.MemCpy(gpt, handle, (uint)Length);
            gh.Free();
        }

        /// <summary>
        /// Copies the buffer into the specified byte array starting at the startIndex with the specified length.
        /// </summary>
        [Description("Copies the buffer into the specified byte array starting at the optional startIndex")]
        public void CopyTo(byte[] destinationArray, int startIndex, int length)
        {
            var gh = GCHandle.Alloc(destinationArray, GCHandleType.Pinned);
            var gpt = gh.AddrOfPinnedObject() + startIndex;
            Native.MemCpy(gpt, handle, (uint)length);
            gh.Free();
        }

        /// <summary>
        /// Copies the buffer into the specified byte array starting at the startIndex with the specified length.
        /// </summary>
        [Description("Copies the buffer into the specified byte array starting at the optional startIndex")]
        public void CopyTo(int blobStartIndex, byte[] destinationArray, int startIndex, int length)
        {
            var gh = GCHandle.Alloc(destinationArray, GCHandleType.Pinned);
            var gpt = gh.AddrOfPinnedObject();
            if (blobStartIndex + length > Length)
            {
                length = (int)(Length - blobStartIndex);
            }

            Native.MemCpy(gpt, handle + blobStartIndex, (uint)length);
            gh.Free();
        }

        /// <summary>
        /// Concat into this byte array.
        /// </summary>
        [Description("Concat into this byte array.")]
        public void Concat(byte[] second)
        {
            if (Length == 0L)
            {
                Length = second.Length;
                SetBytes((IntPtr)0, second);
            }
            else
            {
                if (_Locked)
                    return;
                int c = (int)Length;
                Length += second.Length;
                Native.MemCpy(handle + c, second, (uint)second.Length);
            }

            Align();
        }

        /// <summary>
        /// Copies memory from this memory pointer into the pointer specified.
        /// </summary>
        /// <param name="ptr">The pointer to which to copy the memory.</param>
        /// <param name="size">The size of the buffer to copy.</param>
        /// <remarks></remarks>
        public void CopyTo(IntPtr ptr, IntPtr size)
        {
            if (size.ToInt64() <= uint.MaxValue)
            {
                Native.MemCpy(ptr, handle, (uint)size);
            }
            else
            {
                Native.n_memcpy(ptr, handle, (UIntPtr)(long)size);
            }
        }

        /// <summary>
        /// Copies memory from another memory pointer into this one.
        /// If this one is not yet allocated, it will automatically be allocated
        /// to the size specified.
        /// </summary>
        /// <param name="ptr">The pointer from which to copy the memory.</param>
        /// <param name="size">The size of the buffer to copy.</param>
        /// <remarks></remarks>
        public void CopyFrom(IntPtr ptr, IntPtr size)
        {
            if (handle != (IntPtr)0 || Alloc(size.ToInt64()))
            {
                if (size.ToInt64() <= uint.MaxValue)
                {
                    Native.MemCpy(handle, ptr, (uint)size);
                }
                else
                {
                    Native.n_memcpy(handle, ptr, (UIntPtr)(long)size);
                }
            }
        }

        /// <summary>
        /// Copies one memory location to another.
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="length"></param>
        /// <remarks></remarks>
        public static void CopyMemory(IntPtr dest, IntPtr src, int length)
        {
            Native.MemCpy(dest, src, (uint)length);
        }

        /// <summary>
        /// Copies one memory location to another, long version.
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="length"></param>
        /// <remarks></remarks>
        public static void CopyMemoryLong(IntPtr dest, IntPtr src, long length)
        {
            Native.n_memcpy(dest, src, (UIntPtr)length);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Returns a string from a pointer stored at a memory location in this object's pointer.
        /// </summary>
        /// <param name="index"></param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string get_StringIndirectAt(IntPtr index)
        {
            return GrabStringFromPointerAt(index);
        }

        public void set_StringIndirectAt(IntPtr index, string value)
        {
            SetStringAtPointerIndex(index, value);
        }

        /// <summary>
        /// Returns the length of a null-terminated Unicode string at the specified byteIndex.
        /// </summary>
        /// <param name="byteIndex"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int StrLen(IntPtr byteIndex)
        {
            int StrLenRet = default;
            StrLenRet = Native.CharZero(byteIndex + (int)handle);
            return StrLenRet;
        }

        /// <summary>
        /// Grabs a null-terminated Unicode string from a position relative to the memory pointer.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting byte position relative to the pointer.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public string GrabString(IntPtr byteIndex)
        {
            return null;
        }

        /// <summary>
        /// Grabs a null-terminated Unicode string from a pointer at a the specified position relative to the memory pointer.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting byte position relative to the pointer.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public string GrabStringFromPointerAtAbsolute(IntPtr byteIndex)
        {
            return null;
        }

        /// <summary>
        /// Grabs a null-terminated Unicode string from a pointer at a the specified position, in an array of pointers, relative to the memory pointer.
        /// </summary>
        /// <param name="index">A 32 or 64-bit number indicating the starting pointer collection position relative to the pointer.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public string GrabStringFromPointerAt(IntPtr index)
        {
            return null;
        }

        /// <summary>
        /// Grabs a null-terminated Unicode string from a position relative to the memory pointer with the exact specified length.
        /// No null-termination checking is performed.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting byte position relative to the pointer.</param>
        /// <param name="length">The length of the string, in characters.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GrabString(IntPtr byteIndex, int length)
        {
            string GrabStringRet = default;
            GrabStringRet = Conversions.ToString(GetCharArray(byteIndex, length));
            return GrabStringRet;
        }

        /// <summary>
        /// Grabs a null-terminated ASCII string from a position relative to the memory pointer.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GrabAsciiString(IntPtr byteIndex)
        {
            string GrabAsciiStringRet = default;
            var tp = new IntPtr((long)handle + byteIndex.ToInt64());
            int e = Native.ByteZero(tp);
            byte[] ba;
            if (e == 0)
                return "";
            ba = new byte[e];
            Native.MemCpy(ba, handle + (int)byteIndex, (uint)e);
            GrabAsciiStringRet = Encoding.ASCII.GetString(ba);
            return GrabAsciiStringRet;
        }

        /// <summary>
        /// Grabs a null-terminated ASCII string from a position relative to the memory pointer with the exact specified length.
        /// No null-termination checking is performed.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting byte position relative to the pointer.</param>
        /// <param name="length">The length of the string, in characters.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GrabAsciiString(IntPtr byteIndex, int length)
        {
            string GrabAsciiStringRet = default;
            if (handle == (IntPtr)0)
                return null;
            if (length <= 0)
                throw new IndexOutOfRangeException("length must be greater than zero");
            byte[] ba;
            ba = new byte[length];
            Native.MemCpy(ba, handle + (int)byteIndex, (uint)length);
            GrabAsciiStringRet = Encoding.ASCII.GetString(ba);
            return GrabAsciiStringRet;
        }

        /// <summary>
        /// Grabs a null-terminated UTF-8 string from a position relative to the memory pointer.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public string GrabUtf8String(IntPtr byteIndex)
        {
            return null;
        }

        /// <summary>
        /// Grabs a null-terminated UTF8 string from a position relative to the memory pointer with the exact specified length.
        /// No null-termination checking is performed.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting byte position relative to the pointer.</param>
        /// <param name="length">The length of the string, in characters.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GrabUTF8String(IntPtr byteIndex, int length)
        {
            string GrabUTF8StringRet = default;
            GrabUTF8StringRet = Encoding.UTF8.GetString(GrabBytes(byteIndex, length));
            return GrabUTF8StringRet;
        }

        /// <summary>
        /// Grabs a null-terminated Unicode string array (MULTISZ) from a position relative to the memory pointer.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string[] GrabStringArray(IntPtr byteIndex)
        {
            if (handle == (IntPtr)0)
                return null;
            char b;
            int i = 0;
            long sb = (long)byteIndex;
            string[] sout = null;
            int ct = 0;
            long l = (long)byteIndex;
            var tp = new IntPtr(l + (long)handle);
            b = get_CharAtAbsolute(l);
            while (b != '\0')
            {
                i = Native.CharZero(tp);
                Array.Resize(ref sout, ct + 1);
                sout[ct] = new string('\0', i);
                Native.QuickCopyObject(ref sout[ct], tp, (uint)(i << 1));
                l += (i << 1) + 2;
                tp = tp + ((i << 1) + 2);
                b = get_CharAtAbsolute(l);
                ct += 1;
            }

            return sout;
        }

        /// <summary>
        /// Writes the specified string array to the specified byte index using the specified optional character encoding.
        /// A null termination character is appended to the string before the encoding conversion.
        /// This function attempts to ensure sufficient memory allocation.
        /// </summary>
        /// <param name="byteIndex">The byte index within the memory buffer to begin copying.</param>
        /// <param name="strings">The string array to set.</param>
        /// <param name="enc">Optional <see cref="System.Text.Encoding" /> object (default is Windows Unicode = UTF16LE / wchar_t).</param>
        /// <returns>The total number of bytes that were stored, including the null termination character or characters.</returns>
        /// <remarks></remarks>
        public int SetStringArray(IntPtr byteIndex, string[] strings, Encoding enc)
        {
            long x;
            x = (long)byteIndex;
            foreach (var s in strings)
                x += SetString((IntPtr)x, s, enc);
            return (int)x;
        }

        /// <summary>
        /// Sets the memory at the specified byte index to the specified string using the optional specified encoding.
        /// A null termination character is appended to the string before the encoding conversion.
        /// This function attempts to ensure sufficient memory allocation.
        /// </summary>
        /// <param name="byteIndex">The byte index within the memory buffer to begin copying.</param>
        /// <param name="s">The string value to set.</param>
        /// <param name="enc">Optional <see cref="System.Text.Encoding" /> object (default is Windows Unicode = UTF16LE / wchar_t).</param>
        /// <returns>The total number of bytes that were stored, including the null termination character or characters.</returns>
        /// <remarks></remarks>
        public int SetString(IntPtr byteIndex, string s, Encoding enc)
        {
            if (enc is null)
                enc = Encoding.Unicode;
            var p = handle + (int)byteIndex;
            var b = enc.GetBytes(s + '\0');
            int x = b.Length;
            if (Length < (long)((IntPtr)x + (int)byteIndex))
                Length = (long)((IntPtr)x + (int)byteIndex);
            SetByteArray(p, b);
            return x;
        }


        /// <summary>
        /// Writes the specified string array to the specified byte index using the specified optional character encoding.
        /// A null termination character is appended to the string before the encoding conversion.
        /// This function will auto-allocate memory.
        /// </summary>
        /// <param name="byteIndex">The byte index within the memory buffer to begin copying.</param>
        /// <param name="strings">The string array to set.</param>
        /// <returns>The total number of bytes that were stored, including the null termination character or characters.</returns>
        /// <remarks></remarks>
        public int SetStringArray(IntPtr byteIndex, string[] strings)
        {
            long x = 0L;
            foreach (var s in strings)
                x += (s.Length << 1) + 2;
            if (Length < (long)((IntPtr)x + (int)byteIndex))
                Length = (long)((IntPtr)x + (int)byteIndex);
            x = (long)byteIndex;
            foreach (var s in strings)
            {
                SetString((IntPtr)x, s);
                x += (s.Length << 1) + 2;
            }

            return (int)x;
        }

        /// <summary>
        /// Sets the memory at the specified byte index to the specified string.
        /// A null termination character is appended to the string.
        /// 
        /// This method only auto-allocates if the buffer pointer is null.  If the buffer pointer is not null,
        /// it is up to the caller to make sure that sufficient memory has been allocated to execute this task.
        /// </summary>
        /// <param name="byteIndex">The byte index within the memory buffer to begin copying.</param>
        /// <param name="s">The string to set.</param>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void SetString(IntPtr byteIndex, string s)
        {
        }

        /// <summary>
        /// Sets a buffer referenced by the memory at the specified byte index to the specified string.
        /// A null termination character is appended to the string.
        /// </summary>
        /// <param name="byteIndex">The absolute position in the buffer.</param>
        /// <param name="s">The string to set.</param>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void SetStringAtPointer(IntPtr byteIndex, string s)
        {
        }

        /// <summary>
        /// Sets a buffer referenced by the memory at the specified handle index to the specified string.
        /// A null termination character is appended to the string.
        /// </summary>
        /// <param name="index">The handle index.</param>
        /// <param name="s">The string to set.</param>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void SetStringAtPointerIndex(IntPtr index, string s)
        {
        }

        /// <summary>
        /// Sets the memory at the specified index to the specified byte array.
        /// </summary>
        /// <param name="byteIndex"></param>
        /// <param name="data"></param>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public virtual void SetBytes(IntPtr byteIndex, byte[] data)
        {
        }

        /// <summary>
        /// Sets the memory at the specified index to the specified byte array for the specified length.
        /// </summary>
        /// <param name="byteIndex"></param>
        /// <param name="data"></param>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public virtual void SetBytes(IntPtr byteIndex, byte[] data, int length)
        {
        }

        /// <summary>
        /// Get an array of bytes at the specified position of the specified length.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        /// <param name="length">The number of bytes to grab.</param>
        /// <returns>A new byte array with the requested data.</returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public virtual byte[] GrabBytes(IntPtr byteIndex, int length)
        {
            return null;
        }

        public virtual byte[] GrabBytes()
        {
            return GrabBytes((IntPtr)0, (int)Length);
        }

        /// <summary>
        /// Sets the memory at the specified index to the specified sbyte array.
        /// </summary>
        /// <param name="byteIndex"></param>
        /// <param name="data"></param>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public virtual void SetSBytes(IntPtr byteIndex, byte[] data)
        {
        }

        /// <summary>
        /// Get an array of sbytes at the specified position of the specified length.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        /// <param name="length">The number of bytes to grab.</param>
        /// <returns>A new byte array with the requested data.</returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public virtual byte[] GrabSBytes(IntPtr byteIndex, int length)
        {
            return null;
        }

        /// <summary>
        /// Get an array of bytes at the specified position of the specified length.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        /// <param name="length">The number of bytes to grab.</param>
        /// <param name="data">
        /// The data buffer into which the memory will be copied.
        /// If this value is Nothing or the size of the buffer is too small, then a new buffer will be allocated.
        /// </param>
        /// <remarks></remarks>
        public virtual void GrabBytes(IntPtr byteIndex, int length, ref byte[] data)
        {
            if (handle == (IntPtr)0)
                return;
            if (data is null)
            {
                data = new byte[length];
            }
            else if (data.Length < length)
            {
                data = new byte[length];
            }

            byteGet(byteIndex, length, ref data);
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        protected void byteGet(IntPtr byteIndex, int length, ref byte[] data)
        {
        }

        /// <summary>
        /// Get an array of bytes at the specified position of the specified length.
        /// </summary>
        /// <param name="byteIndex">A 32 or 64-bit number indicating the starting position relative to the pointer.</param>
        /// <param name="length">The number of bytes to grab.</param>
        /// <param name="data">
        /// The data buffer into which the memory will be copied.
        /// If this value is Nothing or the size of the buffer is too small, then the method will fail.
        /// </param>
        /// <param name="arrayIndex">The position in the buffer at which to begin copying.</param>
        /// <remarks></remarks>
        public virtual void GrabBytes(IntPtr byteIndex, int length, ref byte[] data, int arrayIndex)
        {
            if (handle == (IntPtr)0)
                return;
            if (data is null)
            {
                throw new ArgumentNullException("data cannot be null or Nothing.");
            }
            else if (length + arrayIndex > data.Length)
            {
                throw new ArgumentOutOfRangeException("data buffer length is too small.");
            }

            var gh = GCHandle.Alloc(data, GCHandleType.Pinned);
            var pdest = gh.AddrOfPinnedObject() + arrayIndex;
            Native.MemCpy(pdest, handle + (int)byteIndex, (uint)length);
            gh.Free();
        }

        /// <summary>
        /// Returns the results of the buffer as if it were a BSTR type String.
        /// </summary>
        /// <param name="comPtr">Specifies whether or not the current MemPtr is an actual COM pointer to a BSTR.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual string BSTR(bool comPtr = true)
        {
            if (comPtr)
            {
                return GrabString((IntPtr)0, get_IntegerAt(-4));
            }
            else
            {
                return GrabString((IntPtr)4, get_IntegerAt(0L));
            }
        }

        /// <summary>
        /// Returns the contents of this buffer as a string.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual string LpwStr()
        {
            string LpwStrRet = default;
            LpwStrRet = GrabString((IntPtr)0);
            return LpwStrRet;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Gets or sets a value indicating the kind of memory to allocate.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual MemAllocType MemoryType
        {
            get
            {
                MemAllocType MemoryTypeRet = default;
                if (IsInvalid)
                    return MemAllocType.Invalid;
                MemoryTypeRet = _MemType;
                return MemoryTypeRet;
            }

            set
            {
                if (!IsInvalid)
                    throw new AccessViolationException("Cannot change memory type on an allocated object.  Free your object, first.");
                _MemType = value;
            }
        }

        /// <summary>
        /// Sets the length of the memory block.
        /// </summary>
        /// <param name="value"></param>
        /// <remarks></remarks>
        public virtual void SetLength(long value)
        {
            if (hHeap == (IntPtr)0)
                hHeap = Native.GetProcessHeap();
            Alloc(value);
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Calculate the CRC 32 for the block of memory represented by this structure.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public uint CalculateCrc32()
        {
            uint CalculateCrc32Ret = default;
            var l = new IntPtr(Length);
            CalculateCrc32Ret = Crc32.Calculate(handle, l);
            return CalculateCrc32Ret;
        }

        /// <summary>
        /// Calculate the CRC 32 for the block of memory represented by this structure.
        /// </summary>
        /// <param name="bufflen">The length, in bytes, of the marshaling buffer.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public uint CalculateCrc32(int bufflen)
        {
            uint CalculateCrc32Ret = default;
            var l = new IntPtr(Length);
            CalculateCrc32Ret = Crc32.Calculate(handle, l, bufflen: bufflen);
            return CalculateCrc32Ret;
        }

        /// <summary>
        /// Calculate the CRC 32 for the block of memory represented by this structure.
        /// </summary>
        /// <param name="length">The length, in bytes, of the buffer to check.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public uint CalculateCrc32(IntPtr length)
        {
            uint CalculateCrc32Ret = default;
            CalculateCrc32Ret = Crc32.Calculate(handle, length);
            return CalculateCrc32Ret;
        }

        /// <summary>
        /// Calculate the CRC 32 for the block of memory represented by this structure.
        /// </summary>
        /// <param name="length">The length, in bytes, of the buffer to check.</param>
        /// <param name="bufflen">The length, in bytes, of the marshaling buffer.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public uint CalculateCrc32(IntPtr length, int bufflen)
        {
            uint CalculateCrc32Ret = default;
            CalculateCrc32Ret = Crc32.Calculate(handle, length, bufflen: bufflen);
            return CalculateCrc32Ret;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Converts the contents of an unmanaged pointer into a structure.
        /// </summary>
        /// <typeparam name="T">The type of structure requested.</typeparam>
        /// <returns>New instance of T.</returns>
        /// <remarks></remarks>
        public virtual T ToStruct<T>() where T : struct
        {
            T ToStructRet = default;
            ToStructRet = (T)Marshal.PtrToStructure(handle, typeof(T));
            return ToStructRet;
        }

        /// <summary>
        /// Sets the contents of a structure into an unmanaged pointer.
        /// </summary>
        /// <typeparam name="T">The type of structure to set.</typeparam>
        /// <param name="val">The structure to set.</param>
        /// <remarks></remarks>
        public virtual void FromStruct<T>(T val) where T : struct
        {
            int cb = Marshal.SizeOf(val);
            if (handle == (IntPtr)0)
                Alloc(cb);
            Marshal.StructureToPtr(val, handle, false);
        }

        /// <summary>
        /// Converts the contents of an unmanaged pointer at the specified byte index into a structure.
        /// </summary>
        /// <typeparam name="T">The type of structure requested.</typeparam>
        /// <param name="byteIndex">The byte index relative to the pointer at which to begin copying.</param>
        /// <returns>New instance of T.</returns>
        /// <remarks></remarks>
        public virtual T ToStructAt<T>(IntPtr byteIndex) where T : struct
        {
            T ToStructAtRet = default;
            ToStructAtRet = (T)Marshal.PtrToStructure(handle + (int)byteIndex, typeof(T));
            return ToStructAtRet;
        }

        /// <summary>
        /// Sets the contents of a structure into a memory buffer at the specified byte index.
        /// </summary>
        /// <typeparam name="T">The type of structure to set.</typeparam>
        /// <param name="byteIndex">The byte index relative to the pointer at which to begin copying.</param>
        /// <param name="val">The structure to set.</param>
        /// <remarks></remarks>
        public virtual void FromStructAt<T>(IntPtr byteIndex, T val) where T : struct
        {
            int cb = Marshal.SizeOf(val);
            Marshal.StructureToPtr(val, handle + (int)byteIndex, false);
        }

        /// <summary>
        /// Copies the contents of the buffer at the specified index into a blittable structure array.
        /// </summary>
        /// <typeparam name="T">The structure type.</typeparam>
        /// <param name="byteIndex">The index at which to begin copying.</param>
        /// <returns>An array of T.</returns>
        /// <remarks></remarks>
        public virtual T[] ToBlittableStructArrayAt<T>(IntPtr byteIndex) where T : struct
        {
            if (handle == (IntPtr)0)
                return null;
            long l = Length - Conversions.ToLong(byteIndex);
            int cb = Marshal.SizeOf(new T());
            int c = (int)(l / (double)cb);
            T[] tt;
            tt = new T[c];
            var gh = GCHandle.Alloc(tt, GCHandleType.Pinned);
            if (l <= uint.MaxValue)
            {
                Native.MemCpy(gh.AddrOfPinnedObject(), handle, (uint)l);
            }
            else
            {
                CopyMemory(gh.AddrOfPinnedObject(), handle, (int)l);
            }

            gh.Free();
            return tt;
        }

        /// <summary>
        /// Copies a blittable structure array into the buffer at the specified index, initializing a new buffer, if necessary.
        /// </summary>
        /// <typeparam name="T">The structure type.</typeparam>
        /// <param name="byteIndex">The index at which to begin copying.</param>
        /// <param name="value">The structure array to copy.</param>
        /// <remarks></remarks>
        public virtual void FromBlittableStructArrayAt<T>(IntPtr byteIndex, T[] value) where T : struct
        {
            if (handle == (IntPtr)0 && byteIndex != (IntPtr)0)
                return;
            long l;
            int cb = Marshal.SizeOf(new T());
            int c = value.Count();
            l = c * cb;
            if (handle == (IntPtr)0)
            {
                if (!Alloc(l))
                    return;
            }

            var p = handle + (int)byteIndex;
            var gh = GCHandle.Alloc(value, GCHandleType.Pinned);
            if (l <= uint.MaxValue)
            {
                Native.MemCpy(p, gh.AddrOfPinnedObject(), (uint)l);
            }
            else
            {
                CopyMemory(p, gh.AddrOfPinnedObject(), (int)l);
            }

            gh.Free();
        }

        /// <summary>
        /// Copies the contents of the buffer into a blittable structure array.
        /// </summary>
        /// <typeparam name="T">The structure type.</typeparam>
        /// <returns>An array of T.</returns>
        /// <remarks></remarks>
        public virtual T[] ToBlittableStructArray<T>() where T : struct
        {
            return ToBlittableStructArrayAt<T>((IntPtr)0);
        }

        /// <summary>
        /// Copies a blittable structure array into the buffer, initializing a new buffer, if necessary.
        /// </summary>
        /// <typeparam name="T">The structure type.</typeparam>
        /// <param name="value">The structure array to copy.</param>
        /// <remarks></remarks>
        public virtual void FromBlittableStructArray<T>(T[] value) where T : struct
        {
            FromBlittableStructArrayAt((IntPtr)0, value);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Retrieves or sets an individual GUID structure at the specified absolute byte index in the buffer.
        /// </summary>
        /// <param name="index">The position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public Guid get_GuidAtAbsolute(long index)
        {
            return Guid.Empty;
            // If handle = 0 Then Return Guid.Empty
            // guidAtget(GuidAtAbsolute, CType(CLng(handle) + index, IntPtr))
            // If handle = 0 Then Return
            // guidAtset(CType(CLng(handle) + index, IntPtr), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_GuidAtAbsolute(long index, Guid value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual GUID structure at the specified index in the buffer treated as an array of GUIDs.
        /// </summary>
        /// <param name="index">The position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public Guid get_GuidAt(long index)
        {
            return Guid.Empty;
            // If handle = 0 Then Return Guid.Empty
            // guidAtget(GuidAt, CType(CLng(handle) + (index * 16), IntPtr))
            // If handle = 0 Then Return
            // guidAtset(CType(CLng(handle) + (index * 16), IntPtr), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_GuidAt(long index, Guid value)
        {
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        // ' Integral types accessable via logical indexer.
        // ' A logical index means if you were treat a block
        // ' of memory as an array of the requested type,
        // ' then the result you get will be the element
        // ' at the logical position in that array.
        // ' So a character at logical index 1 has a byte offset of 2,
        // ' for an integer at index 1 the byte offset is 4, etc...
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Retrieves or sets an individual byte at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public byte get_ByteAt(long index)
        {
            return 0;
            // byteAtget(ByteAt, New IntPtr(clng(handle) + index))
            // byteAtset(New IntPtr(clng(handle) + index), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_ByteAt(long index, byte value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual signed byte at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public sbyte get_SByteAt(long index)
        {
            return 0;
            // sbyteAtget(SByteAt, New IntPtr(clng(handle) + index))
            // sbyteAtset(New IntPtr(clng(handle) + index), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_SByteAt(long index, sbyte value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Unicode character at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public char get_CharAt(long index)
        {
            return '\0';
            // charAtget(CharAt, New IntPtr(clng(handle) + (index * 2)))
            // charAtset(New IntPtr(clng(handle) + (index * 2)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_CharAt(long index, char value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Short at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public short get_ShortAt(long index)
        {
            return 0;
            // shortAtget(ShortAt, New IntPtr(clng(handle) + (index * 2)))
            // shortAtset(New IntPtr(clng(handle) + (index * 2)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_ShortAt(long index, short value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual UShort at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public ushort get_UShortAt(long index)
        {
            return 0;
            // ushortAtget(UShortAt, New IntPtr(clng(handle) + (index * 2)))
            // ushortAtset(New IntPtr(clng(handle) + (index * 2)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_UShortAt(long index, ushort value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Integer at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public int get_IntegerAt(long index)
        {
            return 0;
            // intAtget(IntegerAt, New IntPtr(clng(handle) + (index * 4)))
            // intAtset(New IntPtr(clng(handle) + (index * 4)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_IntegerAt(long index, int value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual UInteger at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public uint get_UIntegerAt(long index)
        {
            return 0U;
            // uintAtget(UIntegerAt, New IntPtr(clng(handle) + (index * 4)))
            // uintAtset(New IntPtr(clng(handle) + (index * 4)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_UIntegerAt(long index, uint value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Long at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public long get_LongAt(long index)
        {
            return 0L;
            // longAtget(LongAt, New IntPtr(clng(handle) + (index * 8)))
            // longAtset(New IntPtr(clng(handle) + (index * 8)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_LongAt(long index, long value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual ULong at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public ulong get_ULongAt(long index)
        {
            return 0UL;
            // ulongAtget(ULongAt, New IntPtr(clng(handle) + (index * 8)))
            // ulongAtset(New IntPtr(clng(handle) + (index * 8)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_ULongAt(long index, ulong value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Single at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public float get_SingleAt(long index)
        {
            return 0f;
            // singleAtget(SingleAt, New IntPtr(CLng(handle) + (index * 4)))
            // singleAtset(New IntPtr(CLng(handle) + (index * 4)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_SingleAt(long index, float value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Double at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public double get_DoubleAt(long index)
        {
            return 0d;
            // doubleAtget(DoubleAt, New IntPtr(CLng(handle) + (index * 8)))
            // doubleAtset(New IntPtr(CLng(handle) + (index * 8)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_DoubleAt(long index, double value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Decimal at the specified logical index in the buffer.
        /// </summary>
        /// <param name="index">The index to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public decimal get_DecimalAt(long index)
        {
            return 0m;
            // decimalAtget(DecimalAt, New IntPtr(CLng(handle) + (index * 16)))
            // decimalAtset(New IntPtr(CLng(handle) + (index * 16)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_DecimalAt(long index, decimal value)
        {
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        // ' Integral types accessable via absolute byte position.
        // ' The value returned is the desired integral value
        // ' at the specified absolute byte position
        // ' in the buffer.
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Retrieves or sets an individual Unicode character at the specified byte position in the buffer.
        /// </summary>
        /// <param name="index">The byte position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public char get_CharAtAbsolute(long index)
        {
            return '\0';
            // charAtget(CharAtAbsolute, New IntPtr(clng(handle) + index))
            // charAtset(New IntPtr(clng(handle) + index), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_CharAtAbsolute(long index, char value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Short at the specified byte position in the buffer.
        /// </summary>
        /// <param name="index">The byte position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public short get_ShortAtAbsolute(long index)
        {
            return 0;
            // shortAtget(ShortAtAbsolute, New IntPtr(clng(handle) + (index)))
            // shortAtset(New IntPtr(clng(handle) + (index)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_ShortAtAbsolute(long index, short value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual UShort at the specified byte position in the buffer.
        /// </summary>
        /// <param name="index">The byte position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public ushort get_UShortAtAbsolute(long index)
        {
            return 0;
            // ushortAtget(UShortAtAbsolute, New IntPtr(clng(handle) + (index)))
            // ushortAtset(New IntPtr(clng(handle) + (index)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_UShortAtAbsolute(long index, ushort value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Integer at the specified byte position in the buffer.
        /// </summary>
        /// <param name="index">The byte position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public int get_IntegerAtAbsolute(long index)
        {
            return 0;
            // intAtget(IntegerAtAbsolute, New IntPtr(clng(handle) + (index)))
            // intAtset(New IntPtr(clng(handle) + (index)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_IntegerAtAbsolute(long index, int value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual UInteger at the specified byte position in the buffer.
        /// </summary>
        /// <param name="index">The byte position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public uint get_UIntegerAtAbsolute(long index)
        {
            return 0U;
            // uintAtget(UIntegerAtAbsolute, New IntPtr(clng(handle) + (index)))
            // uintAtset(New IntPtr(clng(handle) + (index)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_UIntegerAtAbsolute(long index, uint value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Long at the specified byte position in the buffer.
        /// </summary>
        /// <param name="index">The byte position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public long get_LongAtAbsolute(long index)
        {
            return 0L;
            // longAtget(LongAtAbsolute, New IntPtr(clng(handle) + (index)))
            // longAtset(New IntPtr(clng(handle) + (index)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_LongAtAbsolute(long index, long value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual ULong at the specified byte position in the buffer.
        /// </summary>
        /// <param name="index">The byte position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public ulong get_ULongAtAbsolute(long index)
        {
            return 0UL;
            // ulongAtget(ULongAtAbsolute, New IntPtr(clng(handle) + (index)))
            // ulongAtset(New IntPtr(clng(handle) + (index)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_ULongAtAbsolute(long index, ulong value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Single at the specified byte position in the buffer.
        /// </summary>
        /// <param name="index">The byte position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public float get_SingleAtAbsolute(long index)
        {
            return 0f;
            // singleAtget(SingleAtAbsolute, New IntPtr(CLng(handle) + (index)))
            // singleAtset(New IntPtr(CLng(handle) + (index)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_SingleAtAbsolute(long index, float value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Double at the specified byte position in the buffer.
        /// </summary>
        /// <param name="index">The byte position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public double get_DoubleAtAbsolute(long index)
        {
            return 0d;
            // doubleAtget(DoubleAtAbsolute, New IntPtr(CLng(handle) + (index)))
            // doubleAtset(New IntPtr(CLng(handle) + (index)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_DoubleAtAbsolute(long index, double value)
        {
        }

        /// <summary>
        /// Retrieves or sets an individual Decimal at the specified byte position in the buffer.
        /// </summary>
        /// <param name="index">The byte position to return.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public decimal get_DecimalAtAbsolute(long index)
        {
            return 0m;
            // decimalAtget(DecimalAtAbsolute, New IntPtr(CLng(handle) + (index)))
            // decimalAtset(New IntPtr(CLng(handle) + (index)), value)
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public void set_DecimalAtAbsolute(long index, decimal value)
        {
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Reverses the entire memory pointer.
        /// </summary>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public virtual bool Reverse(bool asChar = false)
        {
            if (handle == (IntPtr)0)
                return false;
            long l = Length;
            long i = 0L;
            long j;
            char ch;
            byte b;
            if (asChar)
            {
                l = l >> 1;
                j = l - 1L;
                do
                {
                    ch = get_CharAt(i);
                    set_CharAt(i, get_CharAt(j));
                    set_CharAt(j, ch);
                    i += 1L;
                    j -= 1L;
                }
                while (i < l);
            }
            else
            {
                j = l - 1L;
                do
                {
                    b = get_ByteAt(i);
                    set_ByteAt(i, get_ByteAt(j));
                    set_ByteAt(j, b);
                    i += 1L;
                    j -= 1L;
                }
                while (i < l);
            }

            return true;
        }

        /// <summary>
        /// Slides a block of memory toward the beginning or toward the end of the memory buffer,
        /// moving the memory around it to the other side.
        /// </summary>
        /// <param name="index">The index of the first byte in the affected block.</param>
        /// <param name="length">The length of the block.</param>
        /// <param name="offset">
        /// The offset amount of the slide.  If the amount is negative,
        /// the block slides toward the beginning of the memory buffer.
        /// If it is positive, it slides to the right.
        /// </param>
        /// <remarks></remarks>
        public virtual void Slide(long index, long length, long offset)
        {
            if (offset == 0L)
                return;
            long hl = Length;
            if (hl <= 0L)
                return;
            if (0L > index + length + offset || index + length + offset > hl)
            {
                throw new IndexOutOfRangeException("Index out of bounds DataTools.Memory.MemPtr.Slide().");
                return;
            }

            // ' if it's short and sweet, let's make it short and sweet
            // ' no need to call p/Invoke ...
            if (length == 1L)
            {
                if (offset == 1L || offset == -1)
                {
                    byte ch;
                    ch = get_ByteAt(index);
                    set_ByteAt(index, get_ByteAt(index + offset));
                    set_ByteAt(index + offset, ch);
                    return;
                }
            }
            else if (length == 2L)
            {
                if (offset == 2L || offset == -2)
                {
                    char ch;
                    ch = get_CharAtAbsolute(index);
                    set_CharAtAbsolute(index, get_CharAtAbsolute(index + offset));
                    set_CharAtAbsolute(index + offset, ch);
                    return;
                }
            }
            else if (length == 4L)
            {
                if (offset == 4L || offset == -4)
                {
                    int ch;
                    ch = get_IntegerAtAbsolute(index);
                    set_IntegerAtAbsolute(index, get_IntegerAtAbsolute(index + offset));
                    set_IntegerAtAbsolute(index + offset, ch);
                    return;
                }
            }
            else if (length == 8L)
            {
                if (offset == 8L || offset == -8)
                {
                    long ch;
                    ch = get_LongAtAbsolute(index);
                    set_LongAtAbsolute(index, get_LongAtAbsolute(index + offset));
                    set_LongAtAbsolute(index + offset, ch);
                    return;
                }
            }

            IntPtr src;
            IntPtr dest;
            src = handle + (int)index;
            dest = handle + (int)index + (int)offset;
            long a = offset < 0L ? offset * -1 : offset;
            var buff = new MemPtr(length);
            var chunk = new MemPtr(a);
            Native.MemCpy(buff.Handle, src, (uint)length);
            Native.MemCpy(chunk.Handle, dest, (uint)a);
            src = handle + (int)index + (int)offset + (int)length;
            Native.MemCpy(src, chunk.Handle, (uint)a);
            Native.MemCpy(dest, buff.Handle, (uint)length);
            chunk.Free();
            buff.Free();
        }

        /// <summary>
        /// Pulls the data in from the specified index.
        /// </summary>
        /// <param name="index">The index where contraction begins. The contraction starts at this position.</param>
        /// <param name="amount">Number of bytes to pull in.</param>
        /// <param name="removePressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public virtual long PullIn(long index, long amount, bool removePressure = false)
        {
            long hl = Length;
            if (Length == 0L || 0L > index || index >= hl - 1L)
            {
                throw new IndexOutOfRangeException("Index out of bounds DataTools.Memory.MemPtr.PullIn().");
                return -1;
            }

            long a = index + amount;
            long b = Length - a;
            Slide(a, b, -amount);
            ReAlloc(hl - amount);
            return Length;
        }

        /// <summary>
        /// Extend the buffer from the specified index.
        /// </summary>
        /// <param name="index">The index where expansion begins. The expansion starts at this position.</param>
        /// <param name="amount">Number of bytes to push out.</param>
        /// <param name="addPressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public virtual long PushOut(long index, long amount, byte[] bytes = null, bool addPressure = false)
        {
            long PushOutRet = default;
            long hl = Length;
            if (hl <= 0L)
            {
                SetLength(amount);
                PushOutRet = amount;
                return PushOutRet;
            }

            if (0L > index)
            {
                throw new IndexOutOfRangeException("Index out of bounds DataTools.Memory.MemPtr.PushOut().");
                return -1;
            }

            long ol = Length - index;
            ReAlloc(hl + amount);
            if (ol > 0L)
            {
                Slide(index, ol, amount);
            }

            if (bytes is object)
            {
                SetByteArray((IntPtr)index, bytes);
            }
            else
            {
                ZeroMemory(index, amount);
            }

            return Length;
        }

        /// <summary>
        /// Slides a block of memory as Unicode characters toward the beginning or toward the end of the buffer.
        /// </summary>
        /// <param name="index">The character index preceding the first character in the affected block.</param>
        /// <param name="length">The length of the block, in characters.</param>
        /// <param name="offset">The offset amount of the slide, in characters.  If the amount is negative, the block slides to the left, if it is positive it slides to the right.</param>
        /// <remarks></remarks>
        public virtual void SlideChar(long index, long length, long offset)
        {
            Slide(index << 1, length << 1, offset << 1);
        }

        /// <summary>
        /// Pulls the data in from the specified character index.
        /// </summary>
        /// <param name="index">The index where contraction begins. The contraction starts at this position.</param>
        /// <param name="amount">Number of characters to pull in.</param>
        /// <param name="removePressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public virtual long PullInChar(long index, long amount, bool removePressure = false)
        {
            return PullIn(index << 1, amount << 1);
        }

        /// <summary>
        /// Extend the buffer from the specified character index.
        /// </summary>
        /// <param name="index">The index where expansion begins. The expansion starts at this position.</param>
        /// <param name="amount">Number of characters to push out.</param>
        /// <param name="addPressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public virtual long PushOutChar(long index, long amount, char[] chars = null, bool addPressure = false)
        {
            return PushOut(index << 1, amount << 1, Native.ToBytes(chars));
        }

        /// <summary>
        /// Parts the string in both directions from index.
        /// </summary>
        /// <param name="index">The index from which to expand.</param>
        /// <param name="amount">The amount of expansion, in both directions, so the total expansion will be amount * 1.</param>
        /// <param name="addPressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public virtual void Part(long index, long amount, bool addPressure = false)
        {
            if (handle == (IntPtr)0)
            {
                SetLength(amount);
                return;
            }

            long l = Length;
            if (l <= 0L)
                return;
            long ol = l - index;
            ReAlloc(l + amount * 1L);
            Slide(index, ol, amount);
            Slide(index + amount + 1L, ol, amount);
        }

        /// <summary>
        /// Inserts the specified bytes at the specified index.
        /// </summary>
        /// <param name="index">Index at which to insert.</param>
        /// <param name="value">Byte array to insert</param>
        /// <param name="addPressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public virtual void Insert(long index, byte[] value, bool addPressure = false)
        {
            PushOut(index, value.Length, value);
        }

        /// <summary>
        /// Inserts the specified characters at the specified character index.
        /// </summary>
        /// <param name="index">Index at which to insert.</param>
        /// <param name="value">Character array to insert</param>
        /// <param name="addPressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public virtual void Insert(long index, char[] value, bool addPressure = false)
        {
            PushOutChar(index, value.Length, value);
        }

        /// <summary>
        /// Delete the memory from the specified index.  Calls PullIn.
        /// </summary>
        /// <param name="index">Index to start the delete.</param>
        /// <param name="amount">Amount of bytes to delete</param>
        /// <param name="removePressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public virtual void Delete(long index, long amount, bool removePressure = false)
        {
            PullIn(index, amount);
        }

        /// <summary>
        /// Delete the memory from the specified character index.  Calls PullIn.
        /// </summary>
        /// <param name="index">Index to start the delete.</param>
        /// <param name="amount">Amount of characters to delete</param>
        /// <param name="removePressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public virtual void DeleteChar(long index, long amount, bool removePressure = false)
        {
            PullInChar(index, amount);
        }

        /// <summary>
        /// Consumes the buffer in both directions from specified index.
        /// </summary>
        /// <param name="index">Index at which consuming begins.</param>
        /// <param name="amount">Amount of contraction, in both directions, so the total contraction will be amount * 1.</param>
        /// <param name="removePressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public virtual void Consume(long index, long amount, bool removePressure = false)
        {
            long hl = Length;
            if (hl <= 0L || amount > index || index >= hl - amount + 1L)
            {
                throw new IndexOutOfRangeException("Index out of bounds DataTools.Memory.Heap:Consume.");
                return;
            }

            index -= amount + 1L;
            PullIn(index, amount);
            index += amount + 1L;
            PullIn(index, amount);
        }

        /// <summary>
        /// Consumes the buffer in both directions from specified character index.
        /// </summary>
        /// <param name="index">Index at which consuming begins.</param>
        /// <param name="amount">Amount of contraction, in both directions, so the total contraction will be amount * 1.</param>
        /// <param name="removePressure">Specify whether to notify the garbage collector.</param>
        /// <remarks></remarks>
        public virtual void ConsumeChar(long index, long amount, bool removePressure = false)
        {
            long hl = Length;
            if (hl <= 0L || amount > index || index >= (hl >> 1) - (amount + 1L))
            {
                throw new IndexOutOfRangeException("Index out of bounds DataTools.Memory.Heap:Consume.");
                return;
            }

            index -= amount + 1L << 1;
            PullIn(index, amount);
            index += amount + 1L << 1;
            PullIn(index, amount);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        // ' Math for pointers is not intended to be used with memory pointers that you have
        // ' allocated with this structure, yourself. The math operators are intended to be
        // ' used with a pointer that is being casually referenced.
        // '
        // ' If you want to do math with the pointer value (increment or iterate, for example)
        // ' then you will need to make a copy of the old pointer (in order to free it, later)
        // ' as this is the only variable contained in this structure
        // ' (so as to keep it suitable for substiting IntPtr
        // ' in structures passed to p/Invoke).
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public static NativeInt operator %(NativeInt operand1, Blob operand2)
        {
            return operand1 % operand2.handle;
        }

        public static NativeInt operator +(NativeInt operand1, Blob operand2)
        {
            return operand1 + operand2.handle;
        }

        public static NativeInt operator -(NativeInt operand1, Blob operand2)
        {
            return operand1 - operand2.handle;
        }

        public static NativeInt operator *(NativeInt operand1, Blob operand2)
        {
            return operand1 * (long)operand2.handle;
        }

        public static NativeInt operator /(NativeInt operand1, Blob operand2)
        {
            return operand1 / operand2.handle;
        }

        public static NativeInt operator /(NativeInt operand1, Blob operand2)
        {
            return operand1 / operand2.handle;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public static implicit operator IntPtr(Blob operand)
        {
            return operand.handle;
        }

        public static implicit operator Blob(IntPtr operand)
        {
            return new Blob(operand, MemAllocType.Heap, false);
        }

        public static explicit operator Blob(MemPtr operand)
        {
            return operand.Handle;
        }

        public static explicit operator MemPtr(Blob operand)
        {
            return new MemPtr(operand.handle);
        }

        public static explicit operator Blob(NativeInt operand)
        {
            return new Blob(operand, (MemAllocType)Conversions.ToInteger(false));
        }

        public static explicit operator NativeInt(Blob operand)
        {
            return operand.handle;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // ' in
        public static implicit operator Blob(sbyte operand)
        {
            var mm = new Blob();
            if (mm.Alloc(1L))
            {
                mm.set_SByteAt(0L, operand);
                mm.BlobType = BlobTypes.SByte;
            }
            else
            {
                throw new InsufficientMemoryException();
            }

            return mm;
        }

        public static implicit operator Blob(byte operand)
        {
            var mm = new Blob();
            if (mm.Alloc(1L))
            {
                mm.set_ByteAt(0L, operand);
                mm.BlobType = BlobTypes.Byte;
            }
            else
            {
                throw new InsufficientMemoryException();
            }

            return mm;
        }

        public static implicit operator Blob(short operand)
        {
            var mm = new Blob();
            if (mm.Alloc(2L))
            {
                mm.set_ShortAt(0L, operand);
                mm.BlobType = BlobTypes.Short;
            }
            else
            {
                throw new InsufficientMemoryException();
            }

            return mm;
        }

        public static implicit operator Blob(ushort operand)
        {
            var mm = new Blob();
            if (mm.Alloc(2L))
            {
                mm.set_UShortAt(0L, operand);
                mm.BlobType = BlobTypes.UShort;
            }
            else
            {
                throw new InsufficientMemoryException();
            }

            return mm;
        }

        public static implicit operator Blob(int operand)
        {
            var mm = new Blob();
            if (mm.Alloc(4L))
            {
                mm.set_IntegerAt(0L, operand);
                mm.BlobType = BlobTypes.Integer;
            }
            else
            {
                throw new InsufficientMemoryException();
            }

            return mm;
        }

        public static implicit operator Blob(uint operand)
        {
            var mm = new Blob();
            if (mm.Alloc(4L))
            {
                mm.set_UIntegerAt(0L, operand);
                mm.BlobType = BlobTypes.UInteger;
            }
            else
            {
                throw new InsufficientMemoryException();
            }

            return mm;
        }

        public static implicit operator Blob(long operand)
        {
            var mm = new Blob();
            if (mm.Alloc(8L))
            {
                mm.set_LongAt(0L, operand);
                mm.BlobType = BlobTypes.Long;
            }
            else
            {
                throw new InsufficientMemoryException();
            }

            return mm;
        }

        public static implicit operator Blob(ulong operand)
        {
            var mm = new Blob();
            if (mm.Alloc(8L))
            {
                mm.set_ULongAt(0L, operand);
                mm.BlobType = BlobTypes.ULong;
            }
            else
            {
                throw new InsufficientMemoryException();
            }

            return mm;
        }

        public static implicit operator Blob(float operand)
        {
            var mm = new Blob();
            if (mm.Alloc(4L))
            {
                mm.set_SingleAt(0L, operand);
                mm.BlobType = BlobTypes.Single;
            }
            else
            {
                throw new InsufficientMemoryException();
            }

            return mm;
        }

        public static implicit operator Blob(double operand)
        {
            var mm = new Blob();
            if (mm.Alloc(8L))
            {
                mm.set_DoubleAt(0L, operand);
                mm.BlobType = BlobTypes.Double;
            }
            else
            {
                throw new InsufficientMemoryException();
            }

            return mm;
        }


        // ' out
        public static implicit operator sbyte(Blob operand)
        {
            return operand.get_SByteAt(0L);
        }

        public static implicit operator byte(Blob operand)
        {
            return operand.get_ByteAt(0L);
        }

        public static implicit operator char(Blob operand)
        {
            return operand.get_CharAt(0L);
        }

        public static implicit operator short(Blob operand)
        {
            return operand.get_ShortAt(0L);
        }

        public static implicit operator ushort(Blob operand)
        {
            return operand.get_UShortAt(0L);
        }

        public static implicit operator int(Blob operand)
        {
            return operand.get_IntegerAt(0L);
        }

        public static implicit operator uint(Blob operand)
        {
            return operand.get_UIntegerAt(0L);
        }

        public static implicit operator long(Blob operand)
        {
            return operand.get_LongAt(0L);
        }

        public static implicit operator ulong(Blob operand)
        {
            return operand.get_ULongAt(0L);
        }

        public static implicit operator float(Blob operand)
        {
            return operand.get_SingleAt(0L);
        }

        public static implicit operator double(Blob operand)
        {
            return operand.get_DoubleAt(0L);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [MethodImpl(MethodImplOptions.ForwardRef)]
        public byte[] GetByteArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetByteArray(IntPtr byteIndex, byte[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public sbyte[] GetSByteArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetSByteArray(IntPtr byteIndex, sbyte[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public char[] GetCharArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetCharArray(IntPtr byteIndex, char[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public ushort[] GetUShortArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetUShortArray(IntPtr byteIndex, ushort[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public short[] GetShortArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetShortArray(IntPtr byteIndex, short[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public uint[] GetUIntegerArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetUIntegerArray(IntPtr byteIndex, uint[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public int[] GetIntegerArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetIntegerArray(IntPtr byteIndex, int[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public ulong[] GetULongArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetULongArray(IntPtr byteIndex, ulong[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public long[] GetLongArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetLongArray(IntPtr byteIndex, long[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public float[] GetSingleArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetSingleArray(IntPtr byteIndex, float[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public double[] GetDoubleArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetDoubleArray(IntPtr byteIndex, double[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public decimal[] GetDecimalArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetDecimalArray(IntPtr byteIndex, decimal[] values)
        {
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public Guid[] GetGuidArray(IntPtr byteIndex, int length)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public void SetGuidArray(IntPtr byteIndex, Guid[] values)
        {
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // ' In

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator Blob(byte[] operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator Blob(sbyte[] operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator Blob(short[] operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator Blob(ushort[] operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator Blob(int[] operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator Blob(uint[] operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator Blob(long[] operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator Blob(ulong[] operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator Blob(float[] operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator Blob(double[] operand)
        {
            return null;
        }

        // ' Out
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator sbyte[](Blob operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator byte[](Blob operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator short[](Blob operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator ushort[](Blob operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator int[](Blob operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator uint[](Blob operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator long[](Blob operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator ulong[](Blob operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator float[](Blob operand)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator double[](Blob operand)
        {
            return null;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // ' Returns a pretty string, based on null-termination logic, instead of
        // ' returning every character in the allocated block.
        public static implicit operator string(Blob operand)
        {
            return operand.GrabString((IntPtr)0);
        }

        // ' We add 2 bytes to give us a proper null-terminated string in memory.
        public static implicit operator Blob(string operand)
        {
            var mm = new Blob();
            mm.SetString((IntPtr)0, operand);
            return mm;

            // Dim i As Integer = operand.Length << 1
            // Dim mm As New Blob(i + 2)
            // QuickCopyObject(Of String)(mm.Handle, operand, CUInt(i))
            // Return mm
        }

        // ' Here we return every character in the allocated block.
        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static implicit operator char[](Blob operand)
        {
            return null;
        }

        // ' We just set the character information into the memory buffer, verbatim.
        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static implicit operator Blob(char[] operand)
        {
            return null;
        }

        public static implicit operator Blob(string[] operand)
        {
            if (operand is null || operand.Length == 0)
                return null;
            var mm = new Blob();
            var x = default(long);
            foreach (var currentS in operand)
            {
                s = currentS;
                x += (long)((s.Length << 1) + 2);
            }

            mm.Length = x;
            x = 0L;
            foreach (var currentS1 in operand)
            {
                s = currentS1;
                mm.SetString((IntPtr)x, s);
                x += (long)((s.Length << 1) + 2);
            }

            return mm;
        }

        public static implicit operator string[](Blob operand)
        {
            return operand.GrabStringArray((IntPtr)0);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public static explicit operator Blob(Guid operand)
        {
            var mm = new Blob();
            if (mm.Alloc(16L))
            {
                mm.set_GuidAt(0L, operand);
                mm.BlobType = BlobTypes.Guid;
            }
            else
            {
                throw new OutOfMemoryException();
            }

            return mm;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static explicit operator Blob(Guid[] operand)
        {
            return null;
        }

        public static explicit operator Guid(Blob operand)
        {
            return operand.get_GuidAt(0L);
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static explicit operator Guid[](Blob operand)
        {
            return null;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public static implicit operator Blob(decimal operand)
        {
            var mm = new Blob();
            if (mm.Alloc(16L))
            {
                mm.set_DecimalAt(0L, operand);
                mm.BlobType = BlobTypes.Decimal;
            }
            else
            {
                throw new OutOfMemoryException();
            }

            return mm;
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator Blob(decimal[] operand)
        {
            return null;
        }

        public static implicit operator decimal(Blob operand)
        {
            return operand.get_DecimalAt(0L);
        }

        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
        public static implicit operator decimal[](Blob operand)
        {
            return null;
        }

        // ' Color

        public static implicit operator Blob(Color operand)
        {
            var mm = new Blob();
            if (mm.Alloc(4L))
            {
                mm.set_IntegerAt(0L, operand.ToArgb());
                mm.BlobType = BlobTypes.Color;
            }
            else
            {
                throw new OutOfMemoryException();
            }

            return mm;
        }

        public static implicit operator Blob(Color[] operand)
        {
            var mm = new Blob();
            if (mm.Alloc(operand.Length << 2))
            {
                Native.QuickCopyObject(ref operand, mm, (uint)(operand.Length << 2));
                mm.BlobType = BlobTypes.Color;
            }
            else
            {
                throw new OutOfMemoryException();
            }

            return mm;
        }

        public static implicit operator Color(Blob operand)
        {
            return Color.FromArgb(operand.get_IntegerAt(0L));
        }

        public static implicit operator Color[](Blob operand)
        {
            Color[] clr;
            long l = operand.Length >> 2;
            if (l > uint.MaxValue)
                throw new InvalidCastException();
            clr = new Color[(int)(l - 1L + 1)];
            Native.QuickCopyObject(ref clr, operand, (uint)(l << 2));
            return clr;
        }

        // ' DateTime

        public static implicit operator Blob(DateTime operand)
        {
            var mm = new Blob();
            if (mm.Alloc(8L))
            {
                mm.set_LongAt(0L, operand.ToBinary());
                mm.BlobType = BlobTypes.Date;
            }
            else
            {
                throw new OutOfMemoryException();
            }

            return mm;
        }

        public static implicit operator Blob(DateTime[] operand)
        {
            var mm = new Blob();
            if (mm.Alloc(operand.Length << 3))
            {
                Native.MemCpy(mm.handle, operand, (uint)(operand.Length << 3));
                mm.BlobType = BlobTypes.Date;
            }
            else
            {
                throw new OutOfMemoryException();
            }

            return mm;
        }

        public static implicit operator DateTime(Blob operand)
        {
            return DateTime.FromBinary(operand.get_LongAt(0L));
        }

        public static implicit operator DateTime[](Blob operand)
        {
            DateTime[] dNew;
            int i = (int)(operand.Length >> 3);
            dNew = new DateTime[i];
            Native.MemCpy(dNew, operand.handle, (uint)(dNew.Length << 3));
            return dNew;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
}