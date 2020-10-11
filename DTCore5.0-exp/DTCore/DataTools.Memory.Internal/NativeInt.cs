// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// '
// ' Module: NativeInt
// '         Sign-independent replacement for IntPtr.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;

namespace DataTools.Memory.Internal
{

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// A small structure to make having a variable-sized buffer less of a pain.
    /// Represents either a 64-bit or a 32-bit integer.
    /// </summary>
    /// <remarks></remarks>
    public struct NativeInt
    {
        private long nativeValue;
        public static readonly IntPtr Zero = IntPtr.Zero;
        public static readonly int Size = IntPtr.Size;

        /// <summary>
        /// Initialize this object with an IntPtr
        /// </summary>
        /// <param name="value"></param>
        /// <remarks></remarks>
        public NativeInt(IntPtr value)
        {
            nativeValue = (long)value;
        }

        /// <summary>
        /// Tests if this object is equal to another.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override bool Equals(object obj)
        {
            return ((NativeInt)obj).nativeValue == nativeValue;
        }

        /// <summary>
        /// Converts the number to a signed long.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public long ToInt64()
        {
            return nativeValue;
        }

        /// <summary>
        /// Returns a string representation of this number.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return nativeValue.ToString();
        }

        /// <summary>
        /// Returns the number in the specified format.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ToString(string format)
        {
            return nativeValue.ToString(format);
        }

        /// <summary>
        /// Converts the number to a signed 32.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public int ToInt32()
        {
            ulong ul = Native.ToUnsigned(nativeValue);
            ul = ul & 0xFFFFFFFFUL;
            uint us = (uint)ul;
            return Native.ToSigned(us);
        }

        public static NativeInt FromInt32(int value)
        {
            var ni = new NativeInt();
            uint us = Native.ToUnsigned(value);
            ulong ul = us;
            ni.nativeValue = Native.ToSigned(ul);
            return ni;
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public static implicit operator IntPtr(NativeInt operand)
        {
            if (IntPtr.Size == 4)
            {
                return (IntPtr)operand.ToInt32();
            }
            else
            {
                return (IntPtr)operand.ToInt64();
            }
        }

        public static implicit operator NativeInt(IntPtr operand)
        {
            if (IntPtr.Size == 8)
            {
                return new NativeInt() { nativeValue = (long)operand };
            }
            else
            {
                return FromInt32(operand.ToInt32());
            }
        }

        public static implicit operator long(NativeInt operand)
        {
            return operand.nativeValue;
        }

        public static implicit operator NativeInt(long operand)
        {
            var n = new NativeInt();
            n.nativeValue = operand;
            return n;
        }

        public static explicit operator short(NativeInt operand)
        {
            return (short)(operand.nativeValue & 0xFFFFSL);
        }

        public static explicit operator NativeInt(short operand)
        {
            return new NativeInt() { nativeValue = operand };
        }

        public static explicit operator int(NativeInt operand)
        {
            return operand.ToInt32();
        }

        public static explicit operator NativeInt(int operand)
        {
            return new NativeInt() { nativeValue = operand };
        }

        public static implicit operator double(NativeInt operand)
        {
            if (IntPtr.Size == 4)
                return operand.ToInt32();
            else
                return operand.ToInt64();
        }

        public static implicit operator NativeInt(double operand)
        {
            var b = new NativeInt();
            b.nativeValue = (long)operand;
            return b;
        }

        public static explicit operator float(NativeInt operand)
        {
            return operand.ToInt32();
        }

        public static explicit operator NativeInt(float operand)
        {
            return new NativeInt() { nativeValue = (int)operand };
        }



        // ' bitwise operations.
        public static long operator &(NativeInt a, NativeInt b)
        {
            return a.ToInt64() & b.ToInt64();
        }

        public static long operator |(NativeInt a, NativeInt b)
        {
            return a.ToInt64() | b.ToInt64();
        }

        class _failedMemberConversionMarker1
        {
        }
#error Cannot convert OperatorBlockSyntax - see comment for details
        /* Cannot convert OperatorBlockSyntax, CONVERSION ERROR: Unable to cast object of type 'Microsoft.CodeAnalysis.CSharp.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax'. in 'Public Shared Operator Xor(...' at character 6941
           at ICSharpCode.CodeConverter.CSharp.CommentConvertingVisitorWrapper.<ConvertHandledAsync>d__5`1.MoveNext()

        Input:

                Public Shared Operator Xor(a As Global.DataTools.Memory.Internal.NativeInt, b As Global.DataTools.Memory.Internal.NativeInt) As Long
                    Return (a.ToInt64 Xor b.ToInt64)
                End Operator

         */
        public static long operator !(NativeInt a)
        {
            return ~a.ToInt64();
        }

        public static long operator %(NativeInt a, NativeInt b)
        {
            return a.ToInt64() % b.ToInt64();
        }

        public static long operator /(NativeInt a, NativeInt b)
        {
            return a.ToInt64() / b.ToInt64();
        }

        public static long operator /(NativeInt a, NativeInt b)
        {
            return (long)(a.ToInt64() / (double)b.ToInt64());
        }

        public static long operator +(NativeInt a, NativeInt b)
        {
            return a.ToInt64() + b.ToInt64();
        }

        public static long operator -(NativeInt a, NativeInt b)
        {
            return a.ToInt64() - b.ToInt64();
        }

        public static bool operator ==(NativeInt a, NativeInt b)
        {
            return a.ToInt64() == b.ToInt64();
        }

        public static bool operator !=(NativeInt a, NativeInt b)
        {
            return a.ToInt64() != b.ToInt64();
        }

        public static bool operator <(NativeInt a, NativeInt b)
        {
            return a.ToInt64() < b.ToInt64();
        }

        public static bool operator >(NativeInt a, NativeInt b)
        {
            return a.ToInt64() > b.ToInt64();
        }

        public static bool operator <=(NativeInt a, NativeInt b)
        {
            return a.ToInt64() <= b.ToInt64();
        }

        public static bool operator >=(NativeInt a, NativeInt b)
        {
            return a.ToInt64() >= b.ToInt64();
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
}