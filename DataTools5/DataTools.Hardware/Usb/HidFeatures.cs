// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: UsbApi
//         HID Feature Code Manipulation
//
//         Enums are documented in part from the API documentation at MSDN.
//         Other knowledge and references obtained through various sources
//         and all is considered public domain/common knowledge.
//
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using DataTools.Memory;
using DataTools.Win32Api;

namespace DataTools.Hardware.Usb
{
    public static class HidFeatures
    {
        public class HIDFeatureResult
        {
            public MemPtr data { get; set; }
            public int code { get; set; }

            public float singleVal
            {
                get
                {
                    float singleValRet = default;
                    if (code.ToString().ToLower().IndexOf("Percent") != -1)
                    {
                        MemPtr m2 = new MemPtr();
                        m2.Handle = data.Handle + 1;
                        singleValRet = m2.SingleAt(0L);
                    }
                    else
                    {
                        singleValRet = intVal;
                    }

                    return singleValRet;
                }
            }

            public long longVal
            {
                get
                {
                    long longValRet = default;
                    MemPtr m2 = new MemPtr();
                    m2.Handle = data.Handle + 1;
                    longValRet = m2.LongAt(0L);
                    return longValRet;
                }
            }

            public int intVal
            {
                get
                {
                    int intValRet = default;
                    MemPtr m2 = new MemPtr();
                    m2.Handle = data.Handle + 1;
                    intValRet = m2.IntAt(0L);
                    return intValRet;
                }
            }

            public byte[] bytes
            {
                get
                {
                    return (byte[])data;
                }
            }

            ~HIDFeatureResult()
            {
                data.Free();
            }

            public HIDFeatureResult(int i, MemPtr m)
            {
                code = i;
                data = m.Clone();
            }

            public HIDFeatureResult()
            {
            }

            public static implicit operator HIDFeatureResult(byte[] operand)
            {
                var q = new HIDFeatureResult();
                q.data = (MemPtr)operand;
                return q;
            }

            public override string ToString()
            {
                return code.ToString() + " (" + intVal + ")";
            }
        }

        /// <summary>
        /// Enumerates all HID devices in a specific HID class.
        /// </summary>
        /// <param name="u">The HID usage page type devices to return.</param>
        /// <returns>An array of HidDeviceInfo objects.</returns>
        /// <remarks></remarks>
        public static HidDeviceInfo[] HidDevicesByUsage(HidUsagePage u)
        {
            var devs = DevEnumPublic.EnumerateDevices<HidDeviceInfo>(DevProp.GUID_DEVINTERFACE_HID);
            HidDeviceInfo[] devOut = null;
            int c = 0;
            foreach (var blurb in devs)
            {
                if (blurb.HidUsagePage == u || u == HidUsagePage.Undefined)
                {
                    Array.Resize(ref devOut, c + 1);
                    devOut[c] = blurb;
                    c += 1;
                }
            }

            return devOut;
        }

        /// <summary>
        /// Opens a HID device for access.
        /// </summary>
        /// <param name="device">The HidDeviceInfo object of the device.</param>
        /// <returns>A handle to the open device (close with CloseHid).</returns>
        /// <remarks></remarks>
        public static IntPtr OpenHid(HidDeviceInfo device)
        {
            IntPtr OpenHidRet = default;
            try
            {
                OpenHidRet = IO.CreateFile(device.DevicePath, IO.GENERIC_READ, IO.FILE_SHARE_READ | IO.FILE_SHARE_WRITE, IntPtr.Zero, IO.OPEN_EXISTING, IO.FILE_ATTRIBUTE_NORMAL, default);
            }
            catch
            {
                return IntPtr.Zero;
            }

            return OpenHidRet;
        }

        /// <summary>
        /// Closes a HID device handle.
        /// </summary>
        /// <param name="handle">The handle of the device to be freed.</param>
        /// <remarks></remarks>
        public static void CloseHid(IntPtr handle)
        {
            if (handle != (IntPtr)(-1) && handle != IntPtr.Zero)
                User32.CloseHandle(handle);
        }

        /// <summary>
        /// Retrieves a feature from the device.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="code"></param>
        /// <param name="datalen"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static HIDFeatureResult GetHIDFeature(HidDeviceInfo device, int code, int datalen = 16)
        {
            HIDFeatureResult GetHIDFeatureRet = default;
            IntPtr hFile;
            hFile = IO.CreateFile(device.DevicePath, IO.GENERIC_READ, IO.FILE_SHARE_READ | IO.FILE_SHARE_WRITE, IntPtr.Zero, IO.OPEN_EXISTING, IO.FILE_ATTRIBUTE_NORMAL, default);
            if (hFile == IntPtr.Zero)
                return null;
            GetHIDFeatureRet = GetHIDFeature(hFile, code, datalen);
            User32.CloseHandle(hFile);
            return GetHIDFeatureRet;
        }

        /// <summary>
        /// Retrieves a feature from the device.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="code"></param>
        /// <param name="datalen"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static HIDFeatureResult GetHIDFeature(IntPtr device, int code, int datalen = 16)
        {
            HIDFeatureResult GetHIDFeatureRet = default;
            MemPtr mm = new MemPtr();

            int i = code;

            try
            {
                mm.AllocZero(datalen);
                mm.ByteAt(0L) = (byte)i;
                if (UsbLibHelpers.HidD_GetFeature(device, mm.Handle, (int)mm.Length))
                {
                    GetHIDFeatureRet = new HIDFeatureResult(i, mm);
                }
                else
                {
                    GetHIDFeatureRet = null;
                }

                mm.Free();
            }
            catch
            {
                mm.Free();
                return null;
            }

            return GetHIDFeatureRet;
        }
    }
}