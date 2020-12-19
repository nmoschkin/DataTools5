using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Streams
{
    /// <summary>
    /// ISO 3309 CRC-32 Calculator.
    /// </summary>
    /// <remarks></remarks>
    public sealed class Crc32
    {

        private static readonly uint CRC32Poly = 0xedb88320u;

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
            uint i = 0;
            uint j = 0;
            uint l = 0;

            for (i = 0; i <= 255; i++)
            {
                j = i;
                for (l = 0; l <= 7; l++)
                {
                    if ((j & 1) == 1)
                    {
                        j = j >> 1 ^ CRC32Poly;
                    }
                    else
                    {
                        j >>= 1;
                    }
                }
                Crc32Table[i] = j;
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

        ///// <summary>
        ///// Validates a memory block against an input CRC.
        ///// </summary>
        ///// <param name="data">The memory block validate.</param>
        ///// <param name="length">The length of the memory block to validate.</param>
        ///// <param name="inputCrc">The CRC value against which the validation should occur.</param>
        ///// <returns>True if the input CRC matches the calculated CRC of the data.</returns>
        ///// <remarks></remarks>
        //public static bool Validate(IntPtr data, IntPtr length, uint inputCrc)
        //{
        //    return Calculate(data, length) == inputCrc;
        //}

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
        public static uint Calculate(byte[] data, int startIndex = 0, int length = -1, uint crc = 0xffffffffu)
        {
            if (data == null)
                throw new ArgumentNullException("data", "data cannot be equal to null.");

            if (length == -1)
                length = data.Length - startIndex;
            if (length <= 0)
                throw new ArgumentOutOfRangeException("length", "length must be -1 or a positive number.");

            int j = 0;
            int c = length;

            for (j = startIndex; j < c; j++)
            {
                crc = Crc32Table[(crc ^ data[j]) & 0xff] ^ crc >> 8;
            }

            return crc ^ 0xffffffffu;
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
        public static uint Calculate(IntPtr data, IntPtr length, uint crc = 0xffffffffu)
        {
            uint ret;

            unsafe
            {
                ret = Calculate((byte*)data, length.ToInt64(), crc);
            }

            return ret;
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
        public static unsafe uint Calculate(byte* data, long length, uint crc = 0xffffffffu)
        {
            if (data == null)
                throw new ArgumentNullException("data", "data cannot be equal to null.");

            if (length <= 0)
                throw new ArgumentOutOfRangeException("length", "length must be -1 or a positive number.");

            for (long j = 0; j < length; j++)
            {
                crc = Crc32Table[(crc ^ *data++) & 0xff] ^ crc >> 8;
            }

            return crc ^ 0xffffffffu;
        }



        /// <summary>
        /// Calculate the CRC-32 of a file.
        /// </summary>
        /// <param name="fileName">Filename of the file to calculate.</param>
        /// <returns>A 32-bit unsigned integer representing the calculated CRC.</returns>
        /// <param name="bufflen">Specify the size, in bytes, of the marshaling buffer to be used (default is 1k).</param>
        /// <remarks></remarks>
        public static uint Calculate(string fileName, int bufflen = 1024)
        {
            if (!System.IO.File.Exists(fileName))
            {
                throw new System.IO.FileNotFoundException(fileName + " could not be found.");
            }

            // our working marshal buffer will be 1k, this is a good compromise between eating up memory and efficiency.
            int blen = bufflen;
            uint crc = 0xffffffffu;

            byte[] b = null;
            System.IO.FileStream fi = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);

            long i = 0;
            long l = fi.Length;

            int e = 0;
            int j = 0;

            b = new byte[blen];

            for (i = 0; i < l; i += blen)
            {
                e = (int)(l - i);
                if (e > blen) e = blen;

                if (fi.Position != i) fi.Seek(i, System.IO.SeekOrigin.Begin);
                fi.Read(b, 0, e);

                for (j = 0; j < e; j++)
                {
                    crc = Crc32Table[(crc ^ b[j]) & 0xff] ^ crc >> 8;
                }
            }

            fi.Close();
            return crc ^ 0xffffffffu;
        }

        /// <summary>
        /// Calculate the CRC-32 of a file.
        /// </summary>
        /// <param name="stream">Stream to calculate.</param>
        /// <returns>A 32-bit unsigned integer representing the calculated CRC.</returns>
        /// <param name="bufflen">Specify the size, in bytes, of the marshaling buffer to be used (default is 1k).</param>
        /// <remarks></remarks>
        public static uint Calculate(System.IO.Stream stream, int bufflen = 1024)
        {
            // our working marshal buffer will be 1k, this is a good compromise between eating up memory and efstreamciency.
            uint crc = 0xffffffffu;

            byte[] b;

            long i, l = stream.Length;
            int e, j, blen = bufflen;

            b = new byte[blen];

            for (i = 0; i < l; i += blen)
            {
                e = (int)(l - i);
                if (e > blen) e = blen;

                if (stream.Position != i) stream.Seek(i, System.IO.SeekOrigin.Begin);
                stream.Read(b, 0, e);

                for (j = 0; j < e; j++)
                {
                    crc = Crc32Table[(crc ^ b[j]) & 0xff] ^ crc >> 8;
                }
            }

            stream.Close();
            return crc ^ 0xffffffffu;
        }



    }
}
