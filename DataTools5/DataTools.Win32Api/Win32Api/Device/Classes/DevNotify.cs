using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using DataTools.Memory;

namespace DataTools.Win32Api
{
    public static class DevNotify
    {

        public const uint DEVICE_NOTIFY_WINDOW_HANDLE = 0x00000000;
        public const uint DEVICE_NOTIFY_SERVICE_HANDLE = 0x00000001;
        public const uint DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 0x00000004;

        public const uint DBT_DEVICEARRIVAL = 0x8000;  // system detected a new device
        public const uint DBT_DEVICEQUERYREMOVE = 0x8001;  // wants to remove, may fail
        public const uint DBT_DEVICEQUERYREMOVEFAILED = 0x8002;  // removal aborted
        public const uint DBT_DEVICEREMOVEPENDING = 0x8003;  // about to remove, still avail.
        public const uint DBT_DEVICEREMOVECOMPLETE = 0x8004;  // device is gone
        public const uint DBT_DEVICETYPESPECIFIC = 0x8005;  // type specific event

        public const uint DBT_CUSTOMEVENT = 0x8006;  // user-defined event

        public const uint DBT_DEVTYP_OEM = 0x00000000;  // oem-defined device type
        public const uint DBT_DEVTYP_DEVNODE = 0x00000001;  // devnode number
        public const uint DBT_DEVTYP_VOLUME = 0x00000002;  // logical volume
        public const uint DBT_DEVTYP_PORT = 0x00000003;  // serial, parallel
        public const uint DBT_DEVTYP_NET = 0x00000004;  // network resource

        public const uint DBT_DEVTYP_DEVICEINTERFACE = 0x00000005;  // device interface class
        public const uint DBT_DEVTYP_HANDLE = 0x00000006;  // file system handle

        [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "RegisterDeviceNotificationW")]
        public static extern IntPtr RegisterDeviceNotification(
          IntPtr hRecipient,
          IntPtr NotificationFilter,
          uint Flags
        );


        [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "UnregisterDeviceNotification")]
        public static extern bool UnregisterDeviceNotification(
            IntPtr Handle
        );


        public static IntPtr DoRegisterDeviceClassNotification(IntPtr hWnd, Guid devclass) 
        {
            var mm = new MemPtr();

            var bh = new DEV_BROADCAST_HDR();
            var di = new DEV_BROADCAST_DEVICEINTERFACE();

            bh.dbch_size = Marshal.SizeOf<DEV_BROADCAST_HDR>();
            di.dbcc_size = Marshal.SizeOf<DEV_BROADCAST_DEVICEINTERFACE>();

            bh.dbch_devicetype = DBT_DEVTYP_DEVICEINTERFACE;
            di.dbcc_classguid = devclass;

            mm.Alloc(bh.dbch_size + di.dbcc_size);

            mm.FromStruct(bh);
            mm.FromStructAt(bh.dbch_size, di);

            var ret = RegisterDeviceNotification(hWnd, mm, DEVICE_NOTIFY_WINDOW_HANDLE);
            mm.Free();

            return ret;
        }



    }
}
