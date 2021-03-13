using System;
using System.Collections.Generic;
using System.Text;
using DataTools.Text.ByteOrderMark;
using System.Collections;
using System.Runtime.InteropServices;

namespace DataTools.Standard.Memory.StringBlob
{
    /// <summary>
    /// StringBlob manages unmanaged arrays of strings in memory (either the LPWSTR or BSTR varierty.)
    /// </summary>
    /// <remarks></remarks>
    public class StringBlob : List<string>
    {

        /// <summary>
        /// Formats the StringBlob into a single string using the specified criteria.
        /// </summary>
        /// <param name="format">A combination of <see cref="StringBlobFormats"/> values that indicate how the string will be rendered.</param>
        /// <param name="customFormat">(NOT IMPLEMENTED)</param>
        /// <returns></returns>
        public string ToFormattedString(StringBlobFormats format, string customFormat = "")
        {
            var sb = new StringBuilder();
            var i = 0;
            foreach(var s in this)
            {
                if (!string.IsNullOrEmpty(customFormat))
                {
                    sb.Append(string.Format(s, customFormat));
                }

                if (i > 0)
                {
                    if (0 != (format & StringBlobFormats.Commas))
                    {
                        sb.Append(",");
                    }

                    if (0 != (format & StringBlobFormats.Spaced))
                    {
                        sb.Append(" ");
                    }
                }

                if (0 != (format & StringBlobFormats.Quoted))
                {
                    sb.Append("\"");
                }

                sb.Append(s);

                if (0 != (format & StringBlobFormats.Quoted))
                {
                    sb.Append("\"");
                }

                if (0 != (format & StringBlobFormats.CrLf))
                {
                    sb.Append("\r\n");
                }

                i++;


            }

            return sb.ToString();
        }


        public static StringBlob FromNullTermPtr(IntPtr ptr)
        {
            var sb = new StringBlob();
            var b = new SafePtr(ptr);
            sb.AddRange(b.GetStringArray(0));
            return sb;
        }

        public SafePtr ToNullTermPtr()
        {
            var b = new SafePtr();

            int i = 0;

            foreach(var s in this)
            {
                i += (s.Length + 1) * sizeof(char);
            }

            i += sizeof(char);

            b.Length = i;


            int idx = 0;

            foreach (var s in this)
            {
                b.SetString(idx, s);
                idx += (s.Length + 1) * sizeof(char);
            }

            return b;
        }

        public static StringBlob FromByteArray(byte[] val)
        {
            var mm = (SafePtr)(val);

            var sb = StringBlob.FromNullTermPtr(mm.handle);
            mm.Free();

            return sb;
        }

        public byte[] ToByteArray()
        {
            var sp = ToNullTermPtr();
            return sp.ToByteArray();
        }


        /// <summary>
        /// Returns a plain-text reprentation of the string blob, with no formatting.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToFormattedString(StringBlobFormats.None);
        }


        /// <summary>
        /// Gets a string array for the specified StringBlob
        /// </summary>
        /// <param name="operand">StringBlob whose strings to return.</param>
        /// <returns></returns>
        public static string[] ToStringArray(StringBlob operand)
        {
            return operand.ToArray();
        }

        /// <summary>
        /// Copies the strings from the StringBlob into the specified array starting at the specified index.
        /// </summary>
        /// <param name="sb">Source object.</param>
        /// <param name="array">Destination array.</param>
        /// <param name="startIndex">Index within array to begin copying.</param>
        public static void ToStringArray(StringBlob sb, string[] array, int startIndex)
        {
            int c = startIndex;
            foreach (string n in sb)
            {
                array[c] = n;
                c += 1;
            }
        }

        /// <summary>
        /// Creates a StringBlob from a string array.
        /// </summary>
        /// <param name="operand">Source array.</param>
        /// <returns></returns>
        public static StringBlob FromStringArray(string[] operand)
        {
            var sb = new StringBlob();
            sb.AddRange(operand);
            return sb;
        }

        public static StringBlob operator +(StringBlob operand1, string[] operand2)
        {
            if (operand2 is null || operand2.Length == 0)
                return operand1;

            operand1.AddRange(operand2);
            return operand1;
        }

        public static StringBlob operator +(StringBlob operand1, string operand2)
        {
            if (operand2 is null || operand2.Length == 0)
                return operand1;
            
            operand1.Add(operand2);
            return operand1;
        }

        public static explicit operator StringBlob(string[] operand)
        {
            return FromStringArray(operand);
        }

        public static explicit operator string[](StringBlob operand)
        {
            return ToStringArray(operand);
        }

        public static explicit operator StringBlob(string operand)
        {
            var n = new StringBlob();
            n.Add(operand);
            return n;
        }

        public static explicit operator string(StringBlob operand)
        {
            return operand.ToString();
        }

        public object Clone()
        {
            var sb = new StringBlob();
            return sb;
        }

    }

}
