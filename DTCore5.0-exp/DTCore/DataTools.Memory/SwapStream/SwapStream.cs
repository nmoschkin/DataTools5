// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// '
// ' Module: SwapStream
// '         Wraps FileStream around a temporary file.
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.Memory
{

    /// <summary>
    /// A stream that uses a randomly-named temporary file in the current user's application data folder as a storage backing.
    /// The file is deleted when the stream is closed.
    /// </summary>
    public class SwapStream : FileStream
    {
        private string _swapFile;
        [ThreadStatic]
        private static string _tFile;

        /// <summary>
        /// Create a new swap file.
        /// </summary>
        public SwapStream() : base(GetSwapFile(ref _tFile), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None)
        {
            _swapFile = _tFile;
            _tFile = null;
        }

        /// <summary>
        /// Create a new swap file and initialize it with the provided data.
        /// </summary>
        /// <param name="data">Data to initialize the swap file with.</param>
        /// <param name="resetSeek">Specifies whether to seek to the beginning of the file after writing the initial data.</param>
        public SwapStream(byte[] data, bool resetSeek = true) : this()
        {
            Write(data, 0, data.Length);
            if (resetSeek)
                Seek(0L, SeekOrigin.Begin);
        }

        /// <summary>
        /// Gets an unused swap file name in the current user's application data folder.
        /// </summary>
        /// <param name="refReturn"></param>
        /// <returns></returns>
        private static string GetSwapFile([Optional, DefaultParameterValue(null)] ref string refReturn)
        {
            string s;
            object pth;
            pth = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Local\Temp";
            if (Directory.Exists(Conversions.ToString(pth)) == false)
            {
                FileSystem.MkDir(Conversions.ToString(pth));
            }

            pth += @"\";
            do
                s = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(pth, Strings.Strings.MBString(DateTime.UtcNow.Ticks, 62, Strings.Strings.PadTypes.Auto)), ".tmp"));
            while (File.Exists(s));
            refReturn = s;
            return s;
        }

        /// <summary>
        /// Close the stream and delete the swap file.
        /// </summary>
        public override void Close()
        {
            base.Close();
            if (_swapFile is object)
            {
                FileSystem.Kill(_swapFile);
                _swapFile = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (_swapFile is object)
            {
                FileSystem.Kill(_swapFile);
                _swapFile = null;
            }
        }
    }
}