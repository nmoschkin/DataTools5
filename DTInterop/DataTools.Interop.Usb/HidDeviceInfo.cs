// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library - Interop
// '
// ' Module: HidDeviceInfo
// '         USB HID Device derived class.
// '
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using CoreCT.Memory;
using CoreCT.Text;
using DataTools.Interop.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.Interop.Usb
{

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// An object that represents a Human Interface Device USB device on the system.
    /// </summary>
    /// <remarks></remarks>
    public class HidDeviceInfo : DeviceInfo
    {
        protected HidUsagePage _HidPage;
        protected string _SerialNumber;
        protected string _ProductString;
        protected string _PhysicalDescriptor;
        protected string _HidManufacturer;

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Returns the raw byte data for a Hid feature code.
        /// </summary>
        /// <param name="featureCode">The Hid feature code to retrieve.</param>
        /// <param name="result">Receives the result of the operation.</param>
        /// <param name="expectedSize">The expected size, in bytes, of the result.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool HidGetFeature(byte featureCode, ref byte[] result, int expectedSize)
        {
            bool HidGetFeatureRet = default;
            var hfile = HidFeatures.OpenHid(this);
            if (hfile == IntPtr.Zero)
                return false;
            var mm = new MemPtr();
            mm.Alloc(expectedSize + 1);
            mm.ByteAt(0L) = featureCode;
            if (!UsbLibHelpers.HidD_GetFeature(hfile, mm, expectedSize))
            {
                HidGetFeatureRet = false;
            }
            else
            {
                HidGetFeatureRet = true;
                result = mm.ToByteArray(1L, expectedSize);
            }

            HidFeatures.CloseHid(hfile);
            mm.Free();
            return HidGetFeatureRet;
        }

        /// <summary>
        /// Returns the short value of a Hid feature code.
        /// </summary>
        /// <param name="featureCode">The Hid feature code to retrieve.</param>
        /// <param name="result">Receives the result of the operation.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool HidGetFeature(byte featureCode, ref short result)
        {
            bool HidGetFeatureRet = default;
            var hfile = HidFeatures.OpenHid(this);
            if (hfile == IntPtr.Zero)
                return false;
            var mm = new MemPtr();
            mm.Alloc(3L);
            mm.ByteAt(0L) = featureCode;
            if (!UsbLibHelpers.HidD_GetFeature(hfile, mm, 3))
            {
                HidGetFeatureRet = false;
            }
            else
            {
                HidGetFeatureRet = true;
                result = mm.ShortAtAbsolute(1L);
            }

            mm.Free();
            HidFeatures.CloseHid(hfile);
            return HidGetFeatureRet;
        }

        /// <summary>
        /// Returns the integer value of a Hid feature code.
        /// </summary>
        /// <param name="featureCode">The Hid feature code to retrieve.</param>
        /// <param name="result">Receives the result of the operation.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool HidGetFeature(byte featureCode, ref int result)
        {
            bool HidGetFeatureRet = default;
            var hfile = HidFeatures.OpenHid(this);
            if (hfile == IntPtr.Zero)
                return false;
            var mm = new MemPtr();
            mm.Alloc(5L);
            mm.ByteAt(0L) = featureCode;
            if (!UsbLibHelpers.HidD_GetFeature(hfile, mm, 5))
            {
                HidGetFeatureRet = false;
            }
            else
            {
                HidGetFeatureRet = true;
                result = mm.IntAtAbsolute(1L);
            }

            mm.Free();
            HidFeatures.CloseHid(hfile);
            return HidGetFeatureRet;
        }

        /// <summary>
        /// Returns the long value of a Hid feature code.
        /// </summary>
        /// <param name="featureCode">The Hid feature code to retrieve.</param>
        /// <param name="result">Receives the result of the operation.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool HidGetFeature(byte featureCode, ref long result)
        {
            bool HidGetFeatureRet = default;
            var hfile = HidFeatures.OpenHid(this);
            if (hfile == IntPtr.Zero)
                return false;
            var mm = new MemPtr();
            mm.Alloc(9L);
            mm.ByteAt(0L) = featureCode;
            if (!UsbLibHelpers.HidD_GetFeature(hfile, mm, 9))
            {
                HidGetFeatureRet = false;
            }
            else
            {
                HidGetFeatureRet = true;
                result = mm.LongAtAbsolute(1L);
            }

            mm.Free();
            HidFeatures.CloseHid(hfile);
            return HidGetFeatureRet;
        }

        /// <summary>
        /// Sets the raw byte value of a Hid feature code.
        /// </summary>
        /// <param name="featureCode">The Hid feature code to set.</param>
        /// <param name="value">The value to set.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool HidSetFeature(byte featureCode, byte[] value)
        {
            bool HidSetFeatureRet = default;
            var hfile = HidFeatures.OpenHid(this);
            if (hfile == IntPtr.Zero)
                return false;
            var mm = new MemPtr();
            mm.Alloc(value.Length + 1);
            mm.FromByteArray(value, 1L);
            mm.ByteAt(0L) = featureCode;
            if (!UsbLibHelpers.HidD_SetFeature(hfile, mm, (int)mm.Length))
            {
                HidSetFeatureRet = false;
            }
            else
            {
                HidSetFeatureRet = true;
            }

            mm.Free();
            HidFeatures.CloseHid(hfile);
            return HidSetFeatureRet;
        }

        /// <summary>
        /// Sets the short value of a Hid feature code.
        /// </summary>
        /// <param name="featureCode">The Hid feature code to set.</param>
        /// <param name="value">The value to set.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool HidSetFeature(byte featureCode, short value)
        {
            bool HidSetFeatureRet = default;
            var hfile = HidFeatures.OpenHid(this);
            if (hfile == IntPtr.Zero)
                return false;
            var mm = new MemPtr();
            mm.Alloc(3L);
            mm.ByteAt(0L) = featureCode;
            mm.ShortAtAbsolute(1L) = value;
            if (!UsbLibHelpers.HidD_SetFeature(hfile, mm, 3))
            {
                HidSetFeatureRet = false;
            }
            else
            {
                HidSetFeatureRet = true;
            }

            HidFeatures.CloseHid(hfile);
            mm.Free();
            return HidSetFeatureRet;
        }

        /// <summary>
        /// Sets the integer value of a Hid feature code.
        /// </summary>
        /// <param name="featureCode">The Hid feature code to set.</param>
        /// <param name="value">The value to set.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool HidSetFeature(byte featureCode, int value)
        {
            bool HidSetFeatureRet = default;
            var hfile = HidFeatures.OpenHid(this);
            if (hfile == IntPtr.Zero)
                return false;
            var mm = new MemPtr();
            mm.Alloc(5L);
            mm.ByteAt(0L) = featureCode;
            mm.IntAtAbsolute(1L) = value;
            if (!UsbLibHelpers.HidD_SetFeature(hfile, mm, 5))
            {
                HidSetFeatureRet = false;
            }
            else
            {
                HidSetFeatureRet = true;
            }

            HidFeatures.CloseHid(hfile);
            mm.Free();
            return HidSetFeatureRet;
        }

        /// <summary>
        /// Sets the long value of a Hid feature code.
        /// </summary>
        /// <param name="featureCode">The Hid feature code to set.</param>
        /// <param name="value">The value to set.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool HidSetFeature(byte featureCode, long value)
        {
            bool HidSetFeatureRet = default;
            var hfile = HidFeatures.OpenHid(this);
            if (hfile == IntPtr.Zero)
                return false;
            var mm = new MemPtr();
            mm.Alloc(9L);
            mm.ByteAt(0L) = featureCode;
            mm.LongAtAbsolute(1L) = value;
            if (!UsbLibHelpers.HidD_SetFeature(hfile, mm, 9))
            {
                HidSetFeatureRet = false;
            }
            else
            {
                HidSetFeatureRet = true;
            }

            HidFeatures.CloseHid(hfile);
            mm.Free();
            return HidSetFeatureRet;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /// <summary>
        /// Returns the HID device manufacturer.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string HidManufacturer
        {
            get
            {
                if (_HidManufacturer is null)
                {
                    var dev = HidFeatures.OpenHid(this);
                    MemPtr mm = new MemPtr();
                    mm.AllocZero(512L);
                    UsbLibHelpers.HidD_GetManufacturerString(dev, mm, (int)mm.Length);
                    _HidManufacturer = mm.GetString(0L);
                    mm.Free();
                    HidFeatures.CloseHid(dev);
                }

                return _HidManufacturer;
            }

            internal set
            {
                _HidManufacturer = value;
            }
        }

        /// <summary>
        /// Returns the HID device serial number.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string SerialNumber
        {
            get
            {
                if (_SerialNumber is null)
                {
                    var dev = HidFeatures.OpenHid(this);
                    MemPtr mm = new MemPtr();
                    mm.AllocZero(512L);
                    UsbLibHelpers.HidD_GetSerialNumberString(dev, mm, (int)mm.Length);
                    _SerialNumber = (string)mm;
                    mm.Free();
                    HidFeatures.CloseHid(dev);
                }

                return _SerialNumber;
            }

            internal set
            {
                _SerialNumber = value;
            }
        }

        /// <summary>
        /// Returns the HID device product descriptor.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ProductString
        {
            get
            {
                if (_ProductString is null)
                {
                    var dev = HidFeatures.OpenHid(this);
                    MemPtr mm = new MemPtr();
                    mm.AllocZero(512L);
                    UsbLibHelpers.HidD_GetProductString(dev, mm, (int)mm.Length);
                    _ProductString = (string)mm;
                    mm.Free();
                    HidFeatures.CloseHid(dev);
                }

                return _ProductString;
            }

            internal set
            {
                _ProductString = value;
            }
        }

        /// <summary>
        /// Returns the ProductString if available, or the FriendlyName, otherwise.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string FriendlyName
        {
            get
            {
                if (string.IsNullOrEmpty(ProductString) == false)
                    return ProductString;
                else
                    return base.FriendlyName;
            }

            internal set
            {
                base.FriendlyName = value;
            }
        }

        /// <summary>
        /// Returns the HID device physical descriptor.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string PhysicalDescriptor
        {
            get
            {
                if (_PhysicalDescriptor is null)
                {
                    var dev = HidFeatures.OpenHid(this);
                    MemPtr mm = new MemPtr();
                    mm.AllocZero(512L);
                    UsbLibHelpers.HidD_GetPhysicalDescriptor(dev, mm, (int)mm.Length);
                    _PhysicalDescriptor = (string)mm;
                    mm.Free();
                    HidFeatures.CloseHid(dev);
                }

                return _PhysicalDescriptor;
            }

            internal set
            {
                _PhysicalDescriptor = value;
            }
        }

        /// <summary>
        /// Returns the HID usage description.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string HidUsageDescription
        {
            get
            {
                return Utility.GetEnumDescription(_HidPage);
            }
        }

        /// <summary>
        /// Returns the HID usage page type.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public HidUsagePage HidUsagePage
        {
            get
            {
                return _HidPage;
            }
        }

        protected override void ParseHw()
        {
            base.ParseHw();
            string[] v;

            // ' this is how we determine the HID class of the device. I've found this to be a very reliable method.
            foreach (string hw in _HardwareIds)
            {
                int i = hw.IndexOf("HID_DEVICE_UP:");
                if (i >= 0)
                {
                    v = TextTools.Split(hw.Substring(i), ":");
                    if (v.Length > 1)
                    {
                        ushort hp;
                        if (ushort.TryParse(v[1].Replace("_U", ""), System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.CurrentCulture.NumberFormat, out hp))
                        {
                            _HidPage = (HidUsagePage)Conversions.ToUShort(hp);
                            if ((int)_HidPage > 0xFF)
                            {
                                _HidPage = HidUsagePage.Reserved;
                                if (v.Length > 2)
                                {
                                    if (ushort.TryParse(v[1].Replace("_U", ""), System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.CurrentCulture.NumberFormat, out hp))
                                    {
                                        _HidPage = (HidUsagePage)Conversions.ToUShort(hp);
                                        if ((int)_HidPage > 0xFF)
                                            _HidPage = HidUsagePage.Reserved;
                                    }
                                }

                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                }
            }
        }

        public override string UIDescription
        {
            get
            {
                if (string.IsNullOrEmpty(ProductString) == false)
                {
                    return ProductString;
                }
                else if (string.IsNullOrEmpty(Description) == false)
                {
                    return Description;
                }
                else if (string.IsNullOrEmpty(FriendlyName) == false)
                {
                    return FriendlyName;
                }
                else
                {
                    return ToString();
                }
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(FriendlyName))
            {
                return Description + " (" + HidUsageDescription + ")";
            }
            else
            {
                return FriendlyName + ", " + Description + " (" + HidUsageDescription + ")";
            }
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
}