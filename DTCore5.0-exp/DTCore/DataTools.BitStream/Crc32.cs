// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// '
// ' Module: Implements ISO-3309 CRC-32 Calculator
// '         And Validator
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using DataTools.Memory;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.BitStream
{

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// ISO 3309 CRC-32 Calculator.
    /// </summary>
    /// <remarks></remarks>
    public sealed class Crc32
    {
        private static readonly uint CRC32Poly = 0xEDB88320U;
        private static uint[] Crc32Table = new uint[256];

        private Crc32()
        {
            // this is not a creatable object.
        }

        /// <summary>
        /// Initialize the CRC table from the polynomial.
        /// </summary>
        /// <remarks></remarks>
        static Crc32()
        {
            uint i;
            uint j;
            uint l;
            for (i = 0U; i <= 255U; i++)
            {
                j = i;
                for (l = 0U; l <= 7U; l++)
                {
                    if (Conversions.ToBoolean(j & 1L))
                    {
                        j = j >> 1 ^ CRC32Poly;
                    }
                    else
                    {
                        j = j >> 1;
                    }
                }

                Crc32Table[(int)i] = j;
            }
        }

        /// <summary>
        /// Validates a byte array against an input CRC.
        /// </summary>
        /// <param name="data">The byte array to validate.</param>
        /// <param name="inputCrc">The CRC value against which the validation should occur.</param>
        /// <returns>True if the input CRC matches the calculated CRC of the data.</returns>
        /// <remarks></remarks>
        public static bool Validate(byte[] data, uint inputCrc)
        {
            return Calculate(data) == inputCrc;
        }

        /// <summary>
        /// Validates a memory block against an input CRC.
        /// </summary>
        /// <param name="data">The memory block validate.</param>
        /// <param name="length">The length of the memory block to validate.</param>
        /// <param name="inputCrc">The CRC value against which the validation should occur.</param>
        /// <returns>True if the input CRC matches the calculated CRC of the data.</returns>
        /// <remarks></remarks>
        public static bool Validate(IntPtr data, IntPtr length, uint inputCrc)
        {
            return Calculate(data, length) == inputCrc;
        }

        /// <summary>
        /// Validates a file against an input CRC.
        /// </summary>
        /// <param name="fileName">Filename of the file to validate.</param>
        /// <param name="inputCrc">The CRC value against which the validation should occur.</param>
        /// <returns>True if the input CRC matches the calculated CRC of the data.</returns>
        /// <remarks></remarks>
        public static bool Validate(string fileName, uint inputCrc)
        {
            return Calculate(fileName) == inputCrc;
        }

        /// <summary>
        /// Calculate the CRC-32 of an array of bytes.
        /// </summary>
        /// <param name="data">Byte array containing the bytes to calculate.</param>
        /// <param name="startIndex">Specifies the starting index to begin the calculation (default is 0).</param>
        /// <param name="length">Specify the length of the byte array to check (default is -1, or all bytes).</param>
        /// <param name="crc">Input CRC value for ongoing calculations (default is FFFFFFFFh).</param>
        /// <returns>A 32-bit unsigned integer representing the calculated CRC.</returns>
        /// <remarks></remarks>
        public static uint Calculate(byte[] data, int startIndex = 0, int length = -1, uint crc = 0xFFFFFFFFU)
        {
            uint CalculateRet = default;
            if (data is null)
                throw new ArgumentNullException("data", "data cannot be equal to null.");
            if (length == -1)
                length = data.Length - startIndex;
            if (length <= 0)
                throw new ArgumentOutOfRangeException("length", "length must be -1 or a positive number.");
            int j;
            int c = length - 1;
            var loopTo = c;
            for (j = startIndex; j <= loopTo; j++)
                crc = Crc32Table[(int)((crc ^ data[j]) & 0xFFL)] ^ crc >> 8;
            CalculateRet = crc ^ 0xFFFFFFFFU;
            return CalculateRet;
        }

        /// <summary>
        /// Calculate the CRC-32 of a memory pointer.
        /// </summary>
        /// <param name="data">Pointer containing the bytes to calculate.</param>
        /// <param name="length">Specify the length, in bytes, of the data to be checked.</param>
        /// <param name="crc">Input CRC value for ongoing calculations (default is FFFFFFFFh).</param>
        /// <param name="bufflen">Specify the size, in bytes, of the marshaling buffer to be used (default is 1k).</param>
        /// <returns>A 32-bit unsigned integer representing the calculated CRC.</returns>
        /// <remarks></remarks>
        public static uint Calculate(IntPtr data, IntPtr length, uint crc = 0xFFFFFFFFU, int bufflen = 1024)
        {
            uint CalculateRet = default;
            if (length.ToInt64() <= 0L)
                throw new ArgumentOutOfRangeException("length", "length must be a positive number.");
            if (data == IntPtr.Zero)
                throw new ArgumentNullException("data", "data cannot be equal to null.");

            // ' our working marshal buffer will be 1k, this is a good compromise between eating up memory and efficiency.
            int blen = bufflen;
            byte[] b;
            var mm = new MemPtr(data);
            long i;
            long l = length.ToInt64();
            long c = l - 1L;
            int e;
            int j;
            b = new byte[blen];
            var loopTo = c;
            for (i = 0L; (long)blen >= 0 ? i <= loopTo : i >= loopTo; i += blen)
            {
                if (l - i > blen)
                {
                    e = blen;
                }
                else
                {
                    e = (int)(l - i);
                }

                mm.GrabBytes((IntPtr)i, e, ref b);
                e -= 1;
                var loopTo1 = e;
                for (j = 0; j <= loopTo1; j++)
                    crc = Crc32Table[(int)((crc ^ b[j]) & 0xFFL)] ^ crc >> 8;
            }

            CalculateRet = crc ^ 0xFFFFFFFFU;
            return CalculateRet;
        }

        /// <summary>
        /// Calculate the CRC-32 of a file.
        /// </summary>
        /// <param name="fileName">Filename of the file to calculate.</param>
        /// <param name="bufflen">Specify the size, in bytes, of the read buffer to be used (default is 1k).</param>
        /// <returns>A 32-bit unsigned integer representing the calculated CRC.</returns>
        /// <remarks></remarks>
        public static uint Calculate(string fileName, int bufflen = 1024)
        {
            uint CalculateRet = default;
            if (!System.IO.File.Exists(fileName))
            {
                throw new System.IO.FileNotFoundException(fileName + " could not be found.");
                return 0U;
            }

            CalculateRet = Calculate(new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read), bufflen);
            return CalculateRet;
        }

        /// <summary>
        /// Calculate the CRC-32 of a Stream.
        /// </summary>
        /// <param name="stream">The Stream to calculate.</param>
        /// <param name="bufflen">Specify the size, in bytes, of the read buffer to be used (default is 1k).</param>
        /// <returns>A 32-bit unsigned integer representing the calculated CRC.</returns>
        /// <remarks></remarks>
        public static uint Calculate(System.IO.Stream stream, int bufflen = 1024, bool closeStream = true)
        {
            uint CalculateRet = default;
            // ' our working marshal buffer will be 1k, this is a good compromise between eating up memory and efficiency.
            int blen = bufflen;
            uint crc = 0xFFFFFFFFU;
            byte[] b;
            long i;
            long l = stream.Length;
            long c = l - 1L;
            int e;
            int j;
            b = new byte[blen];
            var loopTo = c;
            for (i = 0L; (long)blen >= 0 ? i <= loopTo : i >= loopTo; i += blen)
            {
                if (l - i > blen)
                {
                    e = blen;
                }
                else
                {
                    e = (int)(l - i);
                }

                if (stream.Position != i)
                    stream.Seek(i, System.IO.SeekOrigin.Begin);
                stream.Read(b, 0, e);
                e -= 1;
                var loopTo1 = e;
                for (j = 0; j <= loopTo1; j++)
                    crc = Crc32Table[(int)((crc ^ b[j]) & 0xFFL)] ^ crc >> 8;
            }

            if (closeStream)
                stream.Close();
            CalculateRet = crc ^ 0xFFFFFFFFU;
            return CalculateRet;
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
}