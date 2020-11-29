// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: PrinterDeviceInfo
//         Descendant class of DeviceInfo for 
//         printers.
//
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''


using System.Collections.Generic;
using System.Linq;
using DataTools.Win32Api;
using static DataTools.Hardware.DevEnumApi;

namespace DataTools.Hardware.Printers
{

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Describes a printer.
    /// </summary>
    /// <remarks></remarks>
    public class PrinterDeviceInfo : DeviceInfo
    {
        private PrinterObject _printInfo;

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private static IEnumerable<PrinterDeviceInfo> _allPrinters = null;

        /// <summary>
        /// Refreshes the list of all system printers.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool RefreshPrinters()
        {
            var r = new List<PrinterDeviceInfo>();
            var p = DeviceEnum.EnumPrinters();
            if (p is null || p.Count() < 1)
            {
                IEnumerable<PrinterObject> pr = PrinterObjects.Printers;
                var ap = new List<PrinterDeviceInfo>();
                var icn = GetClassIcon(DevProp.GUID_DEVCLASS_PRINTER);
                foreach (var pe in pr)
                {
                    var f = new PrinterDeviceInfo();
                    f.FriendlyName = pe.PrinterName;
                    f.PrinterInfo = pe;
                    f.DeviceClassIcon = icn;
                    ap.Add(f);
                }

                _allPrinters = ap;
            }
            else
            {
                r.AddRange(p);
                if (r[0].FriendlyName.Contains("Root Print Queue"))
                {
                    r.RemoveAt(0);
                }

                _allPrinters = r;
            }

            return _allPrinters is object && _allPrinters.Count() > 0;
        }

        /// <summary>
        /// Returns the list of all system printers.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IEnumerable<PrinterDeviceInfo> AllPrinters
        {
            get
            {
                return _allPrinters;
            }
        }

        /// <summary>
        /// Returns a PrinterDeviceInfo object based on the printer name.
        /// </summary>
        /// <param name="printerName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static PrinterDeviceInfo GetPrinterFromName(string printerName)
        {
            var l = AllPrinters;
            foreach (var p in l)
            {
                if ((p.FriendlyName ?? "") == (printerName ?? ""))
                    return p;
            }

            return null;
        }

        #region PrinterObject

        /// <summary>
        /// Returns the detailed printer information object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public PrinterObject PrinterInfo
        {
            get
            {
                return _printInfo;
            }

            internal set
            {
                _printInfo = value;
            }
        }

        public override string UIDescription
        {
            get
            {
                if (string.IsNullOrEmpty(FriendlyName))
                    return base.UIDescription;
                else
                    return FriendlyName;
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(FriendlyName))
                return base.ToString();
            else
                return FriendlyName;
        }
            
    }

    #endregion
}