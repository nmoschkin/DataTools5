// ************************************************* ''
// DataTools C# Native Utility Library For Windows 
//
// Module: SystemInfo
//         Provides basic information about the
//         current computer.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Navigation;
using DataTools.Memory;
using DataTools.Text;

namespace DataTools.SystemInformation
{
    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Contains a static list of Windows versions and their characteristics.
    /// </summary>
    /// <remarks></remarks>
    public sealed class OSVersionNames
    {
        private string _Name;
        private int _VerMaj;
        private int _VerMin;
        private bool _Server;
        private static List<OSVersionNames> _col = new List<OSVersionNames>();

        /// <summary>
        /// Returns a list of all OS versions recognized by this assembly.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<OSVersionNames> Versions
        {
            get
            {
                return _col;
            }
        }

        /// <summary>
        /// Finds the OSVersionNames object for the operating system data specified in the osx parameter.
        /// </summary>
        /// <param name="osx">Version information of the operating system for which to retrieve the object.</param>
        /// <returns>An OSVersionNames object.</returns>
        /// <remarks></remarks>
        internal static OSVersionNames FindVersion(OSVERSIONINFOEX osx)
        {
            return FindVersion(osx.dwMajorVersion, osx.dwMinorVersion, osx.wProductType != OSProductType.NTWorkstation);
        }

        /// <summary>
        /// Finds the OSVersionNames object for the operating system data specified.
        /// </summary>
        /// <param name="verMaj">The major version of the operating system to find.</param>
        /// <param name="verMin">The minor version of the operating system to find.</param>
        /// <param name="server">Indicates that the querent is looking for a server OS.</param>
        /// <returns>An OSVersionNames object.</returns>
        /// <remarks></remarks>
        public static OSVersionNames FindVersion(int verMaj, int verMin, bool server = false)
        {
            foreach (var v in _col)
            {
                if (v.VersionMajor == verMaj && v.VersionMinor == verMin && v.Server == server)
                    return v;
            }

            return null;
        }

        /// <summary>
        /// Returns the name of the operating system.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Name
        {
            get
            {
                return _Name;
            }
        }

        /// <summary>
        /// Returns the major version
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int VersionMajor
        {
            get
            {
                return _VerMaj;
            }
        }

        /// <summary>
        /// Returns the minor version
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int VersionMinor
        {
            get
            {
                return _VerMin;
            }
        }

        /// <summary>
        /// Indicates that this version is a server.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Server
        {
            get
            {
                return _Server;
            }
        }

        private OSVersionNames(string name, int vermaj, int vermin, bool serv = false)
        {
            _Name = name;
            _VerMaj = vermaj;
            _VerMin = vermin;
            _Server = serv;
        }

        static OSVersionNames()
        {
            _col.Add(new OSVersionNames("Windows 10", 10, 0, false));
            _col.Add(new OSVersionNames("Windows Server 2016", 10, 0, true));
            _col.Add(new OSVersionNames("Windows 8.1", 6, 3, false));
            _col.Add(new OSVersionNames("Windows Server 2012 R2", 6, 3, true));
            _col.Add(new OSVersionNames("Windows 8", 6, 2, false));
            _col.Add(new OSVersionNames("Windows Server 2012", 6, 2, true));
            _col.Add(new OSVersionNames("Windows 7", 6, 1, false));
            _col.Add(new OSVersionNames("Windows Server 2008 R2", 6, 1, true));
            _col.Add(new OSVersionNames("Windows Vista", 6, 0, false));
            _col.Add(new OSVersionNames("Windows Server 2008", 6, 0, true));

            // Nothing in this project is designed to work on Window XP, so looking for it is pointless.
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
