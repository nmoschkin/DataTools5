// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// '
// ' Module: TypeConverter for Blob
// '         
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using DataTools.Memory.Internal;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.Memory
{

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    public class BlobConverter : ExpandableObjectConverter
    {

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */

        public static bool StructureToBlob(object operand, ref Blob instance)
        {
            int cb = Marshal.SizeOf(operand);
            instance = new Blob();
            instance.Length = cb;
            Marshal.StructureToPtr(operand, instance.DangerousGetHandle(), true);
            return true;
        }

        /// <summary>
        /// Attempts to recover a structure from a blob.  If it is unsuccessful, it will return the bytes in the instance instead.
        /// </summary>
        /// <param name="operand"></param>
        /// <param name="instance"></param>
        /// <remarks></remarks>
        public static bool BlobToStructure(Blob operand, ref object instance, Type type = null, string COMObjectFullName = "")
        {
            if (type is null)
            {
                if (COMObjectFullName is object && !string.IsNullOrEmpty(COMObjectFullName))
                {
                    instance = Interaction.CreateObject(COMObjectFullName);
                    type = instance.GetType();
                }
                else if (instance is null)
                {
                    return false;
                }
                else
                {
                    type = instance.GetType();
                }
            }

            instance = Marshal.PtrToStructure(operand.DangerousGetHandle(), type);
            return instance is object;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /// <summary>
        /// Converts the specified image into a byte array using the specified System.Drawing.Imaging.ImageFormat object.
        /// </summary>
        /// <param name="operand">The image to convert</param>
        /// <param name="fmt">The format object to use for conversion</param>
        /// <returns>A byte array representing the image in the specified format</returns>
        /// <remarks></remarks>
        public static byte[] ImageToBytes(Image operand, System.Drawing.Imaging.ImageFormat fmt)
        {
            var ms = new MemoryStream();
            operand.Save(ms, fmt);
            var b = ms.ToArray();
            ms.Close();
            return b;
        }

        /// <summary>
        /// Converts the specified image into a byte array using the System.Drawing.Imagein.ImageFormat.MemoryBmp object.
        /// </summary>
        /// <param name="operand">The image to convert</param>
        /// <returns>A byte array representing the image as a memory bitmap</returns>
        /// <remarks></remarks>
        public static byte[] ImageToBytes(Image operand)
        {
            return ImageToBytes(operand, System.Drawing.Imaging.ImageFormat.MemoryBmp);
        }


        /// <summary>
        /// Attempts to convert the specified byte array into an Image object.
        /// </summary>
        /// <param name="operand"></param>
        /// <returns>A new image based on the byte array passed.</returns>
        /// <remarks></remarks>
        public static Image BytesToImage(byte[] operand)
        {
            var ms = new MemoryStream(operand);
            Image img;
            img = Image.FromStream(ms);
            ms.Close();
            return img;
        }

        public static DateTime BytesToDate(byte[] b, int startIndex = 0)
        {
            DateTime BytesToDateRet = default;
            BytesToDateRet = Native.ToDate(b, 0U);
            return BytesToDateRet;
        }

        public static byte[] DateToBytes(DateTime d)
        {
            byte[] DateToBytesRet = default;
            DateToBytesRet = Native.ToBytes(d);
            return DateToBytesRet;
        }

        public static DateTime[] BytesToDateArray(byte[] b)
        {
            DateTime[] BytesToDateArrayRet = default;
            BytesToDateArrayRet = Native.ToDates(b, 0U);
            return BytesToDateArrayRet;
        }

        public static byte[] DateArrayToBytes(DateTime[] d)
        {
            byte[] DateArrayToBytesRet = default;
            DateArrayToBytesRet = Native.ToBytes(d);
            return DateArrayToBytesRet;
        }

        public static byte[] ToByteArray(sbyte[] s)
        {
            byte[] ToByteArrayRet = default;
            var sb = GCHandle.Alloc(s, GCHandleType.Pinned);
            ToByteArrayRet = new byte[s.Length];
            Native.MemCpy(ToByteArrayRet, sb.AddrOfPinnedObject(), (uint)s.Length);
            sb.Free();
            return ToByteArrayRet;
        }

        public static sbyte[] ToSByteArray(byte[] s)
        {
            sbyte[] ToSByteArrayRet = default;
            var sb = GCHandle.Alloc(s, GCHandleType.Pinned);
            ToSByteArrayRet = new sbyte[s.Length];
            Native.MemCpy(ToSByteArrayRet, sb.AddrOfPinnedObject(), (uint)s.Length);
            sb.Free();
            return ToSByteArrayRet;
        }

        public static ushort[] ToUShortArray(short[] s)
        {
            ushort[] ToUShortArrayRet = default;
            var sb = GCHandle.Alloc(s, GCHandleType.Pinned);
            ToUShortArrayRet = new ushort[s.Length];
            Native.MemCpy(ToUShortArrayRet, sb.AddrOfPinnedObject(), (uint)(s.Length << 1));
            sb.Free();
            return ToUShortArrayRet;
        }

        public static short[] ToShortArray(ushort[] s)
        {
            short[] ToShortArrayRet = default;
            var sb = GCHandle.Alloc(s, GCHandleType.Pinned);
            ToShortArrayRet = new short[s.Length];
            Native.MemCpy(ToShortArrayRet, sb.AddrOfPinnedObject(), (uint)(s.Length << 1));
            sb.Free();
            return ToShortArrayRet;
        }

        public static uint[] ToUIntegerArray(int[] s)
        {
            uint[] ToUIntegerArrayRet = default;
            var sb = GCHandle.Alloc(s, GCHandleType.Pinned);
            ToUIntegerArrayRet = new uint[s.Length];
            Native.MemCpy(ToUIntegerArrayRet, sb.AddrOfPinnedObject(), (uint)(s.Length << 2));
            sb.Free();
            return ToUIntegerArrayRet;
        }

        public static int[] ToIntegerArray(uint[] s)
        {
            int[] ToIntegerArrayRet = default;
            var sb = GCHandle.Alloc(s, GCHandleType.Pinned);
            ToIntegerArrayRet = new int[s.Length];
            Native.MemCpy(ToIntegerArrayRet, sb.AddrOfPinnedObject(), (uint)(s.Length << 2));
            sb.Free();
            return ToIntegerArrayRet;
        }

        public static ulong[] ToULongArray(long[] s)
        {
            ulong[] ToULongArrayRet = default;
            var sb = GCHandle.Alloc(s, GCHandleType.Pinned);
            ToULongArrayRet = new ulong[s.Length];
            Native.MemCpy(ToULongArrayRet, sb.AddrOfPinnedObject(), (uint)(s.Length << 3));
            sb.Free();
            return ToULongArrayRet;
        }

        public static long[] ToLongArray(ulong[] s)
        {
            long[] ToLongArrayRet = default;
            var sb = GCHandle.Alloc(s, GCHandleType.Pinned);
            ToLongArrayRet = new long[s.Length];
            Native.MemCpy(ToLongArrayRet, sb.AddrOfPinnedObject(), (uint)(s.Length << 3));
            sb.Free();
            return ToLongArrayRet;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            int i;
            int c = (int)BlobConst.UBound;
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                if (sourceType == Blob.Types[i])
                    return true;
                if (sourceType == Blob.ArrayTypes[i])
                    return true;
            }

            if (sourceType.IsValueType)
            {
                return true;
            }

            if (sourceType.IsClass && sourceType.BaseType == Blob.Types[(int)BlobTypes.Image])
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            int i;
            int c = (int)BlobConst.UBound;
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                if (destinationType == Blob.Types[i])
                    return true;
                if (destinationType == Blob.ArrayTypes[i])
                    return true;
            }

            if (destinationType.IsValueType)
            {
                return true;
            }

            if (destinationType.IsClass && destinationType.BaseType == Blob.Types[(int)BlobTypes.Image])
                return true;
            if (destinationType == typeof(InstanceDescriptor))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is null)
                return null;
            if (value.GetType().IsEnum == false && value.GetType().IsArray == true)
            {
                if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(value.Length, 0, false)))
                    return Array.CreateInstance(typeof(byte), 0);
            }

            var bl = new Blob();
            var vType = value.GetType();
            Type eType;
            if (vType == typeof(Blob))
            {
                bl = new Blob((Blob)value);
                return bl;
            }

            if (vType.IsArray)
                eType = value.GetType().GetElementType();
            else
                eType = vType;
            if (vType.IsClass && (vType.BaseType == Blob.Types[(int)BlobTypes.Image] || vType == Blob.Types[(int)BlobTypes.Image]))
            {
                bl = new Blob(ImageToBytes((Image)value));
                bl.BlobType = BlobTypes.Image;
                return bl;
            }

            if (vType.IsEnum == true)
            {
                value = BlobUtil.EnumToBytes(value);
                bl.Type = vType;
                bl = new Blob((byte[])value);
                bl.TypeLen = (int)bl.Length;
                return bl;
            }
            else if (eType.IsEnum == true)
            {
                bl = BlobUtil.EnumToBytes(value);
                bl.Type = vType;
                bl.TypeLen = Marshal.SizeOf(value((object)0));
                return bl;
            }

            switch (vType)
            {
                case var @case when @case == typeof(bool):
                    {
                        bool bol = Conversions.ToBoolean(value);
                        if (bl is null)
                            bl = new Blob();
                        bl.Type = vType;
                        bl.TypeLen = 1;
                        if (bl.Length < 1L)
                            bl.Length = 1L;
                        if (bol)
                        {
                            bl.set_ByteAt(0L, 1);
                        }
                        else
                        {
                            bl.set_ByteAt(0L, 0);
                        }

                        return bl;
                    }

                case var case1 when case1 == typeof(BigInteger):
                    {
                        BigInteger be = (BigInteger)value;
                        bl = be.ToByteArray();
                        bl.Type = vType;
                        bl.TypeLen = (int)bl.Length;
                        return bl;
                    }

                case var case2 when case2 == typeof(DateTime):
                    {
                        bl = DateToBytes(Conversions.ToDate(value));
                        bl.Type = vType;
                        bl.TypeLen = Marshal.SizeOf(value);
                        return bl;
                    }

                case var case3 when case3 == typeof(DateTime[]):
                    {
                        bl = DateArrayToBytes((DateTime[])value);
                        bl.Type = vType;
                        bl.TypeLen = Marshal.SizeOf(value((object)0));
                        return bl;
                    }

                case var case4 when case4 == typeof(byte[]):
                    {
                        bl.Type = vType;
                        bl.TypeLen = 1;
                        bl = (byte[])value;
                        return bl;
                    }

                case var case5 when case5 == typeof(sbyte[]):
                    {
                        bl = ToByteArray((sbyte[])value);
                        bl.Type = vType;
                        bl.TypeLen = 1;
                        return bl;
                    }

                case var case6 when case6 == typeof(string):
                    {
                        if (value is null)
                        {
                            value = "";
                        }
                        else
                        {
                            value = Conversions.ToString(value).Trim('\0');
                        }

                        if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(value, "", false)))
                        {
                            bl.Type = typeof(string);
                            bl.TypeLen = Marshal.SizeOf('A');
                            return bl;
                        }

                        bl = Blob.Parse(value);

                        // bl.Type = GetType(String)
                        // bl.TypeLen = Marshal.SizeOf("A"c)
                        // bl.Alloc(CStr(value).Length * 2)
                        // Native.CopyMemory(bl.handle, CStr(value), CType(CStr(value).Length * 2, IntPtr))
                        return bl;
                    }

                case var case7 when case7 == typeof(Guid):
                    {
                        bl = (Blob)(bl + value.ToByteArray);
                        bl.BlobType = BlobTypes.Guid;
                        bl.ToString();
                        return bl;
                    }

                case var case8 when case8 == typeof(Guid[]):
                    {
                        bl.BlobType = BlobTypes.Guid;
                        foreach (Guid g in (IEnumerable)value)
                            bl += g.ToByteArray();
                        return bl;
                    }

                case var case9 when case9 == typeof(Color):
                    {
                        Color cl;
                        cl = (Color)value;
                        bl = cl.ToArgb();
                        bl.BlobType = BlobTypes.Color;
                        return bl;
                    }

                case var case10 when case10 == typeof(Color[]):
                    {
                        Color[] cl;
                        cl = (Color[])value;
                        bl.BlobType = BlobTypes.Color;
                        bl.Length = 4 * cl.Length;
                        var gh = GCHandle.Alloc(cl, GCHandleType.Pinned);
                        Native.MemCpy(bl.DangerousGetHandle(), gh.AddrOfPinnedObject(), (uint)bl.Length);
                        gh.Free();
                        return bl;
                    }

                case var case11 when case11 == typeof(decimal[]):
                    {
                        decimal[] dec = (decimal[])value;
                        bl.Length = dec.Length * 16;
                        bl.BlobType = BlobTypes.Decimal;
                        Native.MemCpy(bl.DangerousGetHandle(), dec, (uint)(dec.Length * 16));
                        break;
                    }

                case var case12 when case12 == typeof(decimal):
                    {
                        bl.BlobType = BlobTypes.Decimal;
                        bl.Length = 16L;
                        bl.set_DecimalAt(0L, Conversions.ToDecimal(value));
                        break;
                    }

                case var case13 when case13 == typeof(double):
                case var case14 when case14 == typeof(float):
                case var case15 when case15 == typeof(long):
                case var case16 when case16 == typeof(ulong):
                case var case17 when case17 == typeof(int):
                case var case18 when case18 == typeof(uint):
                case var case19 when case19 == typeof(short):
                case var case20 when case20 == typeof(ushort):
                case var case21 when case21 == typeof(byte):
                case var case22 when case22 == typeof(sbyte):
                case var case23 when case23 == typeof(char):
                    {
                        switch (vType)
                        {
                            case var case24 when case24 == typeof(ulong):
                                {
                                    bl.Length = 8L;
                                    bl.set_ULongAt(0L, Conversions.ToULong(value));
                                    break;
                                }

                            case var case25 when case25 == typeof(uint):
                                {
                                    bl.Length = 4L;
                                    bl.set_UIntegerAt(0L, Conversions.ToUInteger(value));
                                    break;
                                }

                            case var case26 when case26 == typeof(ushort):
                                {
                                    bl.Length = 2L;
                                    bl.set_UShortAt(0L, Conversions.ToUShort(value));
                                    break;
                                }

                            case var case27 when case27 == typeof(sbyte):
                                {
                                    var u = BlobConverter.ToByteArray(new[] { value });
                                    bl = u;
                                    bl.Type = vType;
                                    bl.TypeLen = Marshal.SizeOf(u[0]);
                                    break;
                                }

                            case var case28 when case28 == typeof(byte):
                                {
                                    bl = new[] { value };
                                    bl.Type = vType;
                                    bl.TypeLen = Marshal.SizeOf(value);
                                    break;
                                }

                            default:
                                {
                                    bl = BitConverter.GetBytes(value);
                                    bl.Type = vType;
                                    bl.TypeLen = Marshal.SizeOf(value);
                                    break;
                                }
                        }

                        bl.BlobType = Blob.TypeToBlobType(vType);
                        return bl;
                    }

                case var case29 when case29 == typeof(double[]):
                case var case30 when case30 == typeof(float[]):
                case var case31 when case31 == typeof(long[]):
                case var case32 when case32 == typeof(ulong[]):
                case var case33 when case33 == typeof(int[]):
                case var case34 when case34 == typeof(uint[]):
                case var case35 when case35 == typeof(short[]):
                case var case36 when case36 == typeof(ushort[]):
                case var case37 when case37 == typeof(sbyte[]):
                case var case38 when case38 == typeof(char[]):
                    {
                        object a;
                        switch (vType)
                        {
                            case var case39 when case39 == typeof(sbyte[]):
                                {
                                    a = ToByteArray((sbyte[])value);
                                    break;
                                }

                            case var case40 when case40 == typeof(ushort[]):
                                {
                                    a = ToShortArray((ushort[])value);
                                    break;
                                }

                            case var case41 when case41 == typeof(uint[]):
                                {
                                    a = ToIntegerArray((uint[])value);
                                    break;
                                }

                            case var case42 when case42 == typeof(ulong[]):
                                {
                                    a = ToLongArray((ulong[])value);
                                    break;
                                }

                            default:
                                {
                                    a = value;
                                    break;
                                }
                        }

                        int l;
                        int e;
                        l = Conversions.ToInteger(value.Length);
                        e = Marshal.SizeOf(value((object)0));
                        bl.Length = l * e;
                        Marshal.Copy(a, (object)0, (object)bl.DangerousGetHandle(), l);
                        bl.Type = vType;
                        bl.TypeLen = e;
                        return bl;
                    }
            }

            if (value.GetType().IsValueType)
            {
                StructureToBlob(value, ref bl);
                return bl;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            Blob bl = (Blob)value;
            bl.Align();
            if (bl.fOwn == true)
            {
                if (bl.Length == 0L)
                {
                    if (destinationType == typeof(string))
                    {
                        return "";
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else if (destinationType == typeof(string))
            {
                return bl.GrabString((IntPtr)0);
            }

            if (ReferenceEquals(destinationType, typeof(InstanceDescriptor)))
            {

                // ' See the next class converter for details on
                // ' InstanceDescriptor conversion

                System.Reflection.ConstructorInfo objC;
                // objC = objT.GetType.GetConstructor(New Type() {GetType(Single), GetType(Single), GetType(Single), GetType(Single), GetType(Ruler.RulerUnits)})
                // Return New InstanceDescriptor(objC, New Object() {objT.Left, objT.Top, objT.Width, objT.Height, objT.Units()})

                objC = bl.GetType().GetConstructor(new Type[] { typeof(byte[]), typeof(Type) });
                return new InstanceDescriptor(objC, new object[] { (Blob)value, ((Blob)value).Type });
            }

            if (destinationType.IsEnum == true)
            {
                return BlobUtil.BytesToEnum(bl, destinationType);
            }
            else if (destinationType.IsArray == true && destinationType.GetElementType().IsEnum == true)
            {
                return BlobUtil.BytesToEnum(bl, destinationType.GetElementType());
            }

            if (destinationType.IsClass && (destinationType.BaseType == Blob.Types[(int)BlobTypes.Image] || destinationType == Blob.Types[(int)BlobTypes.Image]))
            {
                return BytesToImage(bl.GrabBytes());
            }

            switch (destinationType)
            {
                case var @case when @case == typeof(bool):
                    {
                        return bl.get_ByteAt(0L) != 0;
                    }

                case var case1 when case1 == typeof(BigInteger):
                    {
                        return new BigInteger((byte[])bl);
                    }

                case var case2 when case2 == typeof(DateTime):
                    {
                        return BytesToDate(bl);
                    }

                case var case3 when case3 == typeof(DateTime[]):
                    {
                        return BytesToDateArray(bl);
                    }

                case var case4 when case4 == typeof(byte[]):
                    {
                        byte[] a;
                        a = new byte[(int)(bl.Length - 1L + 1)];
                        Array.Copy((byte[])bl, a, bl.Length);
                        return a;
                    }

                case var case5 when case5 == typeof(sbyte[]):
                    {
                        return ToSByteArray(bl);
                    }

                case var case6 when case6 == typeof(Guid[]):
                case var case7 when case7 == typeof(Guid):
                    {
                        if (destinationType == typeof(Guid))
                        {
                            return new Guid(bl.GrabBytes((IntPtr)0, 16));
                        }

                        int l = 16;
                        int e = (int)(bl.Length / (double)l);
                        int i;
                        int c = (int)(bl.Length - 1L);
                        Guid[] gs;
                        gs = new Guid[e];
                        e = 0;
                        var loopTo = c;
                        for (i = 0; l >= 0 ? i <= loopTo : i >= loopTo; i += l)
                        {
                            gs[e] = new Guid(bl.GrabBytes((IntPtr)i, l));
                            e += 1;
                        }

                        return gs;
                    }

                case var case8 when case8 == typeof(Color[]):
                case var case9 when case9 == typeof(Color):
                    {
                        if (destinationType == typeof(Color))
                        {
                            Color cc;
                            cc = Color.FromArgb(bl);
                            return cc;
                        }

                        int l = 4;
                        int e = (int)(bl.Length / (double)l);
                        int i;
                        int c = (int)(bl.Length - 1L);
                        Color[] cs;
                        cs = new Color[e];
                        e = 0;
                        var ptr = bl.DangerousGetHandle();
                        var loopTo1 = c;
                        for (i = 0; l >= 0 ? i <= loopTo1 : i >= loopTo1; i += l)
                        {
                            Native.CopyMemory(ref l, ptr, (IntPtr)4);
                            cs[e] = Color.FromArgb(l);
                            ptr = ptr + l;
                            e += 1;
                        }

                        return cs;
                    }

                case var case10 when case10 == typeof(string):
                    {
                        if (bl.Length == 0L)
                            return "";
                        return bl.ToString();
                    }

                case var case11 when case11 == typeof(decimal[]):
                case var case12 when case12 == typeof(decimal):
                    {
                        decimal[] d;
                        int[] ints = bl;
                        if (Conversions.ToBoolean(ints.Length % 4))
                        {
                            throw new ArgumentException("Byte array is not aligned for the Decimal data type.");
                            return null;
                        }

                        if (destinationType == typeof(decimal))
                        {
                            if (ints.Length != 4)
                                Array.Resize(ref ints, 4);
                            return new decimal(ints);
                        }

                        var dec = new int[4];
                        int e = bl.Count - 1;
                        int i;
                        d = new decimal[e + 1];
                        var loopTo2 = e;
                        for (i = 0; i <= loopTo2; i++)
                        {
                            Array.Copy(ints, i, dec, 0, 4);
                            d[i] = new decimal(dec);
                        }

                        return d;
                    }

                case var case13 when case13 == typeof(double):
                    {
                        return BitConverter.ToDouble(bl, 0);
                    }

                case var case14 when case14 == typeof(float):
                    {
                        return BitConverter.ToSingle(bl, 0);
                    }

                case var case15 when case15 == typeof(ulong):
                    {
                        var u = ToULongArray(new[] { BitConverter.ToInt64(bl, 0) });
                        return u[0];
                    }

                case var case16 when case16 == typeof(long):
                    {
                        return BitConverter.ToInt64(bl, 0);
                    }

                case var case17 when case17 == typeof(uint):
                    {
                        var u = ToUIntegerArray(new[] { BitConverter.ToInt32(bl, 0) });
                        return u[0];
                    }

                case var case18 when case18 == typeof(int):
                    {
                        return BitConverter.ToInt32(bl, 0);
                    }

                case var case19 when case19 == typeof(ushort):
                    {
                        var u = ToUShortArray(new[] { BitConverter.ToInt16(bl, 0) });
                        return u[0];
                    }

                case var case20 when case20 == typeof(short):
                    {
                        return BitConverter.ToInt16(bl, 0);
                    }

                case var case21 when case21 == typeof(char):
                    {
                        return BitConverter.ToChar(bl, 0);
                    }

                case var case22 when case22 == typeof(byte):
                    {
                        return bl.get_ByteAt(0L);
                    }

                case var case23 when case23 == typeof(sbyte):
                    {
                        var u = ToSByteArray(bl);
                        return u[0];
                    }

                case var case24 when case24 == typeof(double[]):
                case var case25 when case25 == typeof(float[]):
                case var case26 when case26 == typeof(long[]):
                case var case27 when case27 == typeof(ulong[]):
                case var case28 when case28 == typeof(int[]):
                case var case29 when case29 == typeof(uint[]):
                case var case30 when case30 == typeof(short[]):
                case var case31 when case31 == typeof(ushort[]):
                case var case32 when case32 == typeof(char[]):
                    {
                        object a = Array.CreateInstance(destinationType.GetElementType(), 1);
                        int l;
                        int e;
                        int f = 0;
                        IntPtr ptr;
                        byte[] b = bl;
                        l = Marshal.SizeOf(a((object)0));
                        e = (int)(b.Length / (double)l);
                        l = b.Length;
                        switch (destinationType.GetElementType())
                        {
                            case var case33 when case33 == typeof(sbyte):
                                {
                                    a = Array.CreateInstance(typeof(byte), e);
                                    break;
                                }

                            case var case34 when case34 == typeof(ushort):
                                {
                                    a = Array.CreateInstance(typeof(short), e);
                                    break;
                                }

                            case var case35 when case35 == typeof(uint):
                                {
                                    a = Array.CreateInstance(typeof(int), e);
                                    break;
                                }

                            case var case36 when case36 == typeof(ulong):
                                {
                                    a = Array.CreateInstance(typeof(long), e);
                                    break;
                                }

                            default:
                                {
                                    a = Array.CreateInstance(destinationType.GetElementType(), e);
                                    break;
                                }
                        }

                        ptr = Marshal.AllocCoTaskMem(l);
                        Marshal.Copy(b, 0, ptr, l);
                        Marshal.Copy(ptr, a, (object)0, e);
                        Marshal.FreeCoTaskMem(ptr);
                        switch (destinationType.GetElementType())
                        {
                            case var case37 when case37 == typeof(sbyte):
                                {
                                    a = ToSByteArray((byte[])a);
                                    break;
                                }

                            case var case38 when case38 == typeof(ushort):
                                {
                                    a = ToUShortArray((short[])a);
                                    break;
                                }

                            case var case39 when case39 == typeof(uint):
                                {
                                    a = ToUIntegerArray((int[])a);
                                    break;
                                }

                            case var case40 when case40 == typeof(ulong):
                                {
                                    a = ToULongArray((long[])a);
                                    break;
                                }
                        }

                        return a;
                    }
            }

            if (destinationType.IsValueType)
            {
                object o = null;
                BlobToStructure(bl, ref o);
                return o;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            var bl = new Blob();
            return bl;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        public BlobConverter()
        {
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
}