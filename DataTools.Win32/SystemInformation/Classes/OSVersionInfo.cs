using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ComponentModel;

using DataTools.Win32;
using DataTools.Text;

namespace DataTools.SystemInformation
{
    public class OSVersionInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private CancellationTokenSource cts;

        private bool isWatching;

        private Task monitorTask;

        public bool BeginMonitorMemoryUsage(int millisecondTimeout = 5000)
        {
            if (millisecondTimeout <= 500) throw new ArgumentOutOfRangeException(nameof(millisecondTimeout));

            if (isWatching) return false;

            cts = new CancellationTokenSource();

            try
            {
                monitorTask = Task.Run(async () =>
                {

                    try
                    {
                        while (!cts.IsCancellationRequested)
                        {

                            OnPropertyChanged(nameof(AvailPhysicalMemory));
                            await Task.Delay(millisecondTimeout);
                        }
                    }
                    catch
                    {
                        return;
                    }


                }, cts.Token);

                isWatching = true;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool StopMonitorMemoryUsage()
        {
            if (!isWatching || cts == null) return false;
            cts.Cancel(true);

            var b = monitorTask.Wait(2000);

            monitorTask = null;

            return b;
        }

        internal OSVERSIONINFOEX Source { get; private set; }
        internal string displayVersion;

        internal OSVersionInfo(OSVERSIONINFOEX source, string displayVersion)
        {
            
            if (source.dwBuildNumber >= 22000 && source.dwMajorVersion == 10)
            {
                source.dwMajorVersion = 11;
            }

            Source = source;
            this.displayVersion = displayVersion;
        }

        /// <summary>
        /// The major version of the current operating system.
        /// </summary>
        /// <remarks></remarks>
        public int MajorVersion
        {
            get => Source.dwMajorVersion;
        }

        /// <summary>
        /// The minor verison of the current operating system.
        /// </summary>
        /// <remarks></remarks>
        public int MinorVersion
        {
            get => Source.dwMinorVersion;
        }

        /// <summary>
        /// The build number of the current operating system.
        /// </summary>
        /// <remarks></remarks>
        public int BuildNumber
        {
            get => Source.dwBuildNumber;
        }

        /// <summary>
        /// The platform Id of the current operating system.
        /// Currently, this value should always be 2.
        /// </summary>
        /// <remarks></remarks>
        public int PlatformId
        {
            get => Source.dwPlatformId;
        }

        /// <summary>
        /// The Service Pack name (if applicable)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ServicePack
        {
            get => Source.szCSDVersion;
        }

        /// <summary>
        /// Service pack major verison number.
        /// </summary>
        /// <remarks></remarks>
        public short ServicePackMajor
        {
            get => Source.wServicePackMajor;
        }

        /// <summary>
        /// Server pack minor verison number.
        /// </summary>
        /// <remarks></remarks>
        public short ServicePackMinor
        {
            get => Source.wServicePackMinor;
        }

        /// <summary>
        /// The Windows Suite mask.
        /// </summary>
        /// <remarks></remarks>
        public OSSuiteMask SuiteMask
        {
            get => Source.wSuiteMask;
        }

        /// <summary>
        /// The product type flags.
        /// </summary>
        /// <remarks></remarks>
        public OSProductType ProductType
        {
            get => Source.wProductType;
        }
        
        public string DisplayVersion
        {
            get => displayVersion;
        }

        /// <summary>
        /// Returns a verbose description of the operating system, including version and build number.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string VerboseDescription
        {
            get
            {
                return string.Format("{0} {5} ({4}) ({1}.{2} Build {3})", Description, MajorVersion, MinorVersion, BuildNumber, Architecture.ToString(), DisplayVersion);
            }
        }

        /// <summary>
        /// Returns the processor architecture of the current system.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ArchitectureType Architecture
        {
            get
            {
                return SysInfo.nativeEnv.ProcessorArchitecture;
            }
        }

        /// <summary>
        /// Returns the number of logical processors on the current system.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Processors
        {
            get
            {
                return SysInfo.nativeEnv.NumberOfProcessors;
            }
        }

        /// <summary>
        /// Returns the amount of total physical memory on the system.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public FriendlySizeLong TotalPhysicalMemory
        {
            get
            {
                return SysInfo._memInfo.TotalPhysicalMemory;
            }
        }

        /// <summary>
        /// Returns the amount of available physical memory currently on the system.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public FriendlySizeLong AvailPhysicalMemory
        {
            get
            {
                // obviously this property is not very useful unless it is live, so let's
                // refresh our data before sending out the result...
                SysInfo.GlobalMemoryStatusEx(ref SysInfo._memInfo);
                return SysInfo._memInfo.AvailPhysicalMemory;
            }
        }

        /// <summary>
        /// Returns a description of the operating system.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Description
        {
            get
            {
                if (string.IsNullOrEmpty(ServicePack))
                {
                    return Name;
                }
                else
                {
                    return Name + " " + ServicePack;
                }
            }
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
                var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                if (key is object)
                {
                    string s = (string)(key.GetValue("ProductName"));
                    key.Close();

                    if (BuildNumber >= 22000 || MajorVersion == 11)
                    {
                        s = s.Replace(" 10", " 11");
                    }
                    return s;
                }

                return null;
            }
        }

        /// <summary>
        /// Returns true if this version is greater than or equal to Windows 11
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsWindows11
        {
            get
            {
                return MajorVersion >= 11 ? true : false;
            }
        }

        /// <summary>
        /// Returns true if this version is greater than or equal to Windows 10
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsWindows10
        {
            get
            {
                return MajorVersion >= 10 ? true : false;
            }
        }

        /// <summary>
        /// Returns true if this version is greater than or equal to Windows 8.1
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsWindows81
        {
            get
            {
                return MajorVersion > 6 ? true : MajorVersion == 6 && MinorVersion >= 3;
            }
        }

        /// <summary>
        /// Returns true if this version is greater than or equal to Windows 8
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsWindows8
        {
            get
            {
                return MajorVersion > 6 ? true : MajorVersion == 6 && MinorVersion >= 2;
            }
        }

        /// <summary>
        /// Returns true if this version is greater than or equal to Windows 7
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsWindows7
        {
            get
            {
                return MajorVersion > 6 ? true : MajorVersion == 6 && MinorVersion >= 1;
            }
        }

        /// <summary>
        /// Returns true if this version is greater than or equal to Windows Vista
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsVista
        {
            get
            {
                return MajorVersion > 6 ? true : MajorVersion == 6 && MinorVersion >= 0;
            }
        }

        /// <summary>
        /// Returns true if this version is a server greater than or equal to Windows Server 2008
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsServer2008
        {
            get
            {
                return IsVista && IsServer;
            }
        }

        /// <summary>
        /// Returns true if this version is a server greater than or equal to Windows Server 2008 R2
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsServer2008R2
        {
            get
            {
                return IsWindows7 && IsServer;
            }
        }

        /// <summary>
        /// Returns true if this version is a server greater than or equal to Windows Server 2012
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsServer2012
        {
            get
            {
                return IsWindows8 && IsServer;
            }
        }

        /// <summary>
        /// Returns true if this version is a server greater than or equal to Windows Server 2012 R2
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsServer2012R2
        {
            get
            {
                return IsWindows81 && IsServer;
            }
        }
        /// <summary>
        /// Returns true if this version is a server greater than or equal to Windows Server 2016
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsServer2016
        {
            get
            {
                return IsWindows10 && IsServer;
            }
        }

        /// <summary>
        /// Returns true if the current operating system is a Windows Server OS.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsServer
        {
            get
            {
                return ProductType != OSProductType.NTWorkstation;
            }
        }

        /// <summary>
        /// Returns true on a multi-user system.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsMultiUser
        {
            get
            {
                return (int)(SuiteMask & OSSuiteMask.SingleUser) == 0;
            }
        }

        /// <summary>
        /// Returns the Windows version constant.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public WindowsVersionConstants WindowsVersion
        {
            get
            {
                return (WindowsVersionConstants)((MajorVersion << 8) | MinorVersion);
            }
        }


        /// <summary>
        /// Converts this object into its string representation.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Returns the current computer's firmware type.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public FirmwareType FirmwareType
        {
            get
            {
                FirmwareType FirmwareTypeRet = default;
                SysInfo.GetFirmwareType(ref FirmwareTypeRet);
                return FirmwareTypeRet;
            }
        }

        /// <summary>
        /// Returns true if this system was booted from a virtual hard drive.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsVhdBoot
        {
            get
            {
                bool IsVhdBootRet = default;
                SysInfo.IsNativeVhdBoot(ref IsVhdBootRet);
                return IsVhdBootRet;
            }
        }

        public static explicit operator string(OSVersionInfo operand)
        {
            return operand.ToString();
        }

        public static explicit operator WindowsVersionConstants(OSVersionInfo operand)
        {
            return (WindowsVersionConstants)(operand.MajorVersion << 8 | operand.MinorVersion);
        }


    }
}
