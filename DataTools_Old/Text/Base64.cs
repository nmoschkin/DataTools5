using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace DataTools.Text
{
    public static class Base64
    {

        public static byte[] ToBase64(byte[] input)
        {

            // Retrieve the input and output file streams.
            MemoryStream inputStream =
                new MemoryStream(input);
            MemoryStream outputStream =
                new MemoryStream();

            // Create a new ToBase64Transform object to convert to base 64.
            ToBase64Transform base64Transform = new ToBase64Transform();

            // Create a new byte array with the size of the output block size.
            byte[] outputBytes = new byte[base64Transform.OutputBlockSize];

            // Retrieve the file contents into a byte array.
            byte[] inputBytes = new byte[inputStream.Length];
            inputStream.Read(inputBytes, 0, inputBytes.Length);

            // Verify that multiple blocks can not be transformed.
            if (!base64Transform.CanTransformMultipleBlocks)
            {
                // Initializie the offset size.
                int inputOffset = 0;

                // Iterate through inputBytes transforming by blockSize.
                int inputBlockSize = base64Transform.InputBlockSize;

                while (inputBytes.Length - inputOffset > inputBlockSize)
                {
                    base64Transform.TransformBlock(
                        inputBytes,
                        inputOffset,
                        inputBytes.Length - inputOffset,
                        outputBytes,
                        0);

                    inputOffset += base64Transform.InputBlockSize;
                    outputStream.Write(
                        outputBytes,
                        0,
                        base64Transform.OutputBlockSize);
                }

                // Transform the final block of data.
                outputBytes = base64Transform.TransformFinalBlock(
                    inputBytes,
                    inputOffset,
                    inputBytes.Length - inputOffset);

                outputStream.Write(outputBytes, 0, outputBytes.Length);
            }

            // Determine if the current transform can be reused.
            if (!base64Transform.CanReuseTransform)
            {
                // Free up any used resources.
                base64Transform.Clear();
            }

            inputStream.Close();

            var ret = outputStream.ToArray();
            outputStream.Close();

            return ret;

        }

        public static byte[] FromBase64(byte[] input)
        {

            // Retrieve the input and output file streams.
            MemoryStream inputStream =
                new MemoryStream(input);
            MemoryStream outputStream =
                new MemoryStream();

            // Create a new ToBase64Transform object to convert to base 64.
            FromBase64Transform base64Transform = new FromBase64Transform();

            // Create a new byte array with the size of the output block size.
            byte[] outputBytes = new byte[base64Transform.OutputBlockSize];

            // Retrieve the file contents into a byte array.
            byte[] inputBytes = new byte[inputStream.Length];
            inputStream.Read(inputBytes, 0, inputBytes.Length);

            // Initializie the offset size.
            int inputOffset = 0;

            // Iterate through inputBytes transforming by blockSize.
            int inputBlockSize = base64Transform.InputBlockSize;

            while (inputBytes.Length - inputOffset > inputBlockSize)
            {
                base64Transform.TransformBlock(
                    inputBytes,
                    inputOffset,
                    4,
                    outputBytes,
                    0);

                inputOffset += base64Transform.InputBlockSize;
                outputStream.Write(
                    outputBytes,
                    0,
                    base64Transform.OutputBlockSize);
            }

            // Transform the final block of data.
            outputBytes = base64Transform.TransformFinalBlock(
                inputBytes,
                inputOffset,
                inputBytes.Length - inputOffset);

            outputStream.Write(outputBytes, 0, outputBytes.Length);

            // Determine if the current transform can be reused.
            if (!base64Transform.CanReuseTransform)
            {
                // Free up any used resources.
                base64Transform.Clear();
            }

            inputStream.Close();

            var ret = outputStream.ToArray();
            outputStream.Close();

            return ret;

        }




    }
}
