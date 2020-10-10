// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// ' Adapted for C#/Xamarin
// '
// ' Module: CacheStream
// '         Wraps FileStream around a temporary file in the cache directory.
// ' 
// ' Copyright (C) 2011-2015, 2019 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.IO;

using DataTools.Text;

namespace CoreCT.Stream
{

    /// <summary>
    /// A stream that uses a randomly-named temporary file in the current program's cache as a storage backing.
    /// The file is deleted when the stream is closed.
    /// </summary>
    public class CacheStream : FileStream
    {
        private string _swapFile;

        [ThreadStatic]
        private static string _tFile;

        /// <summary>
        /// Create a new swap file.
        /// </summary>
        public CacheStream() : base(GetSwapFile(ref _tFile), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None)
        {
            _swapFile = _tFile;
            _tFile = null;
        }

        /// <summary>
        /// Create a new swap file and initialize it with the provided data.
        /// </summary>
        /// <param name="data">Data to initialize the swap file with.</param>
        /// <param name="resetSeek">Specifies whether to seek to the beginning of the file after writing the initial data.</param>
        public CacheStream(byte[] data, bool resetSeek = true) : this()
        {
            Write(data, 0, data.Length);
            if (resetSeek)
                Seek((long)0, SeekOrigin.Begin);
        }

        /// <summary>
        /// Gets an unused swap file name in the current user's application data folder.
        /// </summary>
        /// <param name="refReturn"></param>
        /// <returns></returns>
        private static string GetSwapFile(ref string refReturn)
        {
            string s;
            string pth;

            System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication().CreateDirectory("swapstream");

            pth = Xamarin.Essentials.FileSystem.CacheDirectory + "/swapstream/";

            if (Directory.Exists(pth) == false)
                Directory.CreateDirectory(pth);

            do
            {
                s = pth + MBStrings.GetString(DateTime.UtcNow.Ticks, 62) + ".tmp";
            }
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
            Dispose(true);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            try
            {
                if (_swapFile != null)
                {
                    File.Delete(_swapFile);
                    _swapFile = null;
                }
            }
            catch { }
        }

    }
}
