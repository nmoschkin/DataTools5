using System;
using System.IO;

namespace DataTools.Text
{
    public class Base64Engine
    {
        public enum DataTypeConstants
        {
            dtBinary = 0x0,
            dtBase64 = 0x1
        }

        private BASE64STRUCT B64 = new BASE64STRUCT();

        private Base64 Base64 = new Base64();

        private bool _ShowProgress = true;

        private DataTypeConstants _DataType;

        public DataTypeConstants DataType
        {
            get => _DataType;
        }

        public int Length
        {
            get => B64.Length;
        }

        public bool ShowProgress
        {
            get => _ShowProgress;
            set
            {
                _ShowProgress = value;
            }
        }

        public byte[] Data
        {
            get => B64.Data;
        }

        public string get_DataString(bool Escape)
        {
            return System.Text.Encoding.ASCII.GetString(B64.Data);
        }

        public void set_DataString(bool Escape, string value)
        {
            B64.Data = System.Text.Encoding.ASCII.GetBytes(value);
        }

        public void Clear()
        {
            B64.Data = null;
            B64.Length = 0;
        }

        public byte[] GetDataBytes()
        {
            return B64.Data;
        }

        public System.Drawing.Image DecodeImage(string s64)
        {
            System.Drawing.Image i;
            MemoryStream st;

            set_DataString(true, s64);

            Decode(GetDataBytes(), GetDataBytes().Length);

            st = new MemoryStream(GetDataBytes());
            i = System.Drawing.Image.FromStream(st);
            return i;
        }


        /// <summary>
        /// Encodes a byte array into a Base64 byte array.
        /// </summary>
        /// <param name="input">The buffer to encode.</param>
        /// <returns>The encoded buffer.</returns>
        /// <remarks></remarks>
        public byte[] Base64Encode(byte[] input)
        {
            var b = new BASE64STRUCT();
            Base64.Encode64(input, input.Length, out b);
            return b.Data;
        }


        /// <summary>
        /// Encodes a byte array into a Base64 string.
        /// </summary>
        /// <param name="input">The buffer to encode.</param>
        /// <param name="encoding">The encoded string.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Base64Encode(byte[] input, System.Text.Encoding encoding)
        {
            if (encoding is null)
                encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(Base64Encode(input));
        }

        /// <summary>
        /// Decodes a Base64-encoded byte array.
        /// </summary>
        /// <param name="input">The input buffer.</param>
        /// <returns>Decoded contents or nothing on error.</returns>
        /// <remarks></remarks>
        public byte[] Base64Decode(byte[] input)
        {
            byte[] b = null;
            if (Base64.Decode64(input, input.Length, out b) != 0)
            {
                return b;
            }

            return null;
        }

        /// <summary>
        /// Decodes a Base64 input string into data.
        /// </summary>
        /// <param name="input">The string of Base64 characters to decode.</param>
        /// <param name="encoding">The System.Text.Encoding option used to convert the string into an array of bytes.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte[] Base64Decode(string input, System.Text.Encoding encoding)
        {
            var ib = encoding.GetBytes(input);
            byte[] b = null;
            
            Base64.Decode64(ib, b.Length, out b);
            
            return b;
        }

        public int Encode(byte[] Bytes, int Length)
        {
            byte[] b;
            b = Bytes;
            
            Base64.Encode64(b, Length, out B64);

            _DataType = DataTypeConstants.dtBase64;

            return B64.Length;
        }

        public int Decode(byte[] Bytes, int Length)
        {
            byte[] b;
            b = Bytes;

            B64.Data = null;
            B64.Data = new byte[Length - 1 + 1];

            B64.Length = Base64.Decode64(Bytes, Length, out B64.Data);

            _DataType = DataTypeConstants.dtBinary;

            return default;
        }

        public int EncodeFile(string fileName)
        {
            byte[] b;
            b = System.IO.File.ReadAllBytes(fileName);
            return Encode(b, b.Length);
        }

        public int EncodeFile(string fileName, string outputFile)
        {
            byte[] b;

            int l;

            b = System.IO.File.ReadAllBytes(fileName);
            l = Encode(b, b.Length);

            System.IO.File.WriteAllBytes(outputFile, B64.Data);

            return l;
        }

        public int DecodeFile(string fileName)
        {
            byte[] b;
            b = System.IO.File.ReadAllBytes(fileName);
            return Decode(b, b.Length);
        }

        public int DecodeFile(string fileName, string outputFile)
        {
            byte[] b;
            int l;
            b = System.IO.File.ReadAllBytes(fileName);
            l = Decode(b, b.Length);
            System.IO.File.WriteAllBytes(outputFile, B64.Data);
            return l;
        }
    }
}