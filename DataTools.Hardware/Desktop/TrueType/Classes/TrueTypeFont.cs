// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: TrueType.
//         Code to read TrueType font file information
//         Adapted from the CodeProject article: http://www.codeproject.com/Articles/2293/Retrieving-font-name-from-TTF-file?msg=4714219#xx4714219xx
//
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using DataTools.Memory;
using DataTools.Win32;

namespace DataTools.Desktop
{
    public static class TrueTypeFont
    {

        
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Change the Endianness of a 16-bit unsigned integer
        /// </summary>
        /// <param name="val">A 16-bit number</param>
        /// <returns>The reverse endian format of val.</returns>
        /// <remarks></remarks>
        public static ushort Swap(ushort val)
        {
            byte v1;
            byte v2;
            v1 = (byte)(val & 0xFF);
            v2 = (byte)(val >> 8 & 0xFF);
            return (ushort)(v1 << 8 | v2);
        }

        /// <summary>
        /// Change the Endianness of a 32-bit unsigned integer
        /// </summary>
        /// <param name="val">A 32-bit number</param>
        /// <returns>The reverse endian format of val.</returns>
        /// <remarks></remarks>
        public static uint Swap(uint val)
        {
            ushort v1;
            ushort v2;
            v1 = (ushort)(val & 0xFFFFL);
            v2 = (ushort)(val >> 16 & 0xFFFFL);
            return (uint)Swap(v1) << 16 | Swap(v2);
        }

        /// <summary>
        /// Change the Endianness of a 64-bit unsigned integer
        /// </summary>
        /// <param name="val">A 64-bit number</param>
        /// <returns>The reverse endian format of val.</returns>
        /// <remarks></remarks>
        public static ulong Swap(ulong val)
        {
            uint v1;
            uint v2;
            v1 = (uint)(val & 0xFFFFFFFFUL);
            v2 = (uint)(val >> 32 & 0xFFFFFFFFUL);
            return (ulong)Swap(v1) << 32 | Swap(v2);
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public static string GetTTFName(string fileName)
        {
            FileStream fs;
            var oft = new TT_OFFSET_TABLE();
            var tdir = new TT_TABLE_DIRECTORY();
            var nth = new TT_NAME_TABLE_HEADER();
            var nr = new TT_NAME_RECORD();
            int i;
            int p;
            string sRet = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            catch
            {
                //Interaction.MsgBox(ex.Message, MsgBoxStyle.Critical);
                return null;
            }

            FileTools.ReadStruct<TT_OFFSET_TABLE>(fs, ref oft);
            oft.uNumOfTables = Swap(oft.uNumOfTables);
            oft.uMajorVersion = Swap(oft.uMajorVersion);
            oft.uMinorVersion = Swap(oft.uMinorVersion);

            // Not a TrueType v1.0 font!
            if (oft.uMajorVersion != 1 || oft.uMinorVersion != 0)
                return null;

            var nt = oft.uNumOfTables;

            for (i = 0; i < nt; i++)
            {
                FileTools.ReadStruct<TT_TABLE_DIRECTORY>(fs, ref tdir);
                if (tdir.Tag.ToLower() == "name")
                {
                    tdir.uLength = Swap(tdir.uLength);
                    tdir.uOffset = Swap(tdir.uOffset);
                    break;
                }
            }

            // Exhausted all records, no name record found!
            if (i >= oft.uNumOfTables)
                return null;

            fs.Seek(tdir.uOffset, SeekOrigin.Begin);

            FileTools.ReadStruct<TT_NAME_TABLE_HEADER>(fs, ref nth);

            nth.uStorageOffset = Swap(nth.uStorageOffset);
            nth.uNRCount = Swap(nth.uNRCount);

            var nthc = nth.uNRCount;

            for (i = 0; i < nthc; i++)
            {
                FileTools.ReadStruct<TT_NAME_RECORD>(fs, ref nr);
                nr.uNameID = Swap(nr.uNameID);

                if (nr.uNameID == 1)
                {
                    p = (int)fs.Position;

                    nr.uStringLength = Swap(nr.uStringLength);
                    nr.uStringOffset = Swap(nr.uStringOffset);

                    fs.Seek(tdir.uOffset + nr.uStringOffset + nth.uStorageOffset, SeekOrigin.Begin);

                    nr.uEncodingID = Swap(nr.uEncodingID);
                    nr.uPlatformID = Swap(nr.uPlatformID);

                    byte[] b;

                    b = new byte[nr.uStringLength];

                    fs.Read(b, 0, nr.uStringLength);

                    // Platform IDs: 0 = Unicode, 1 = Macintosh, 3 = Windows
                    if (nr.uPlatformID == 0)
                    {
                        sRet = System.Text.Encoding.BigEndianUnicode.GetString(b);
                    }
                    else
                    {
                        sRet = System.Text.Encoding.ASCII.GetString(b);
                    }

                    sRet = sRet.Trim('\0');
                    
                    if (!string.IsNullOrEmpty(sRet))
                    {
                        break;
                    }

                    sRet = null;
                    fs.Seek(p, SeekOrigin.Begin);
                }
            }

            return sRet;
        }

    }
}
