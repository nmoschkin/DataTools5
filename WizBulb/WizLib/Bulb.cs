﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Newtonsoft.Json;
using WizLib.Localization;
using System.Net.NetworkInformation;

namespace WizLib
{
    public delegate void BulbScanCallback(Bulb b);

    /// <summary>
    /// Bulb scanning modes indicating which command to broadcast to discover bulbs on the local network.
    /// </summary>
    public enum ScanModes
    {
        /// <summary>
        /// Scan using the 'registration' command
        /// </summary>
        Registration,

        /// <summary>
        /// Scan using the 'getPilot' command
        /// </summary>
        GetPilot,

        /// <summary>
        /// Scan using the 'getSystemConfig' command (default)
        /// </summary>
        GetSystemConfig
    }

    /// <summary>
    /// Encapsulates the characteristics and behavior of a Philips Wiz light bulb.
    /// </summary>
    public class Bulb : ViewModelBase //, IComparable
    {

        /// <summary>
        /// The default port for Wiz bulbs.
        /// </summary>
        public const int DefaultPort = 38899;

        /// <summary>
        /// Gets or sets a value indicating that a console is active 
        /// on the currently running application.
        /// </summary>
        public static bool HasConsole { get; set; }

        private static IPAddress localip = null;

        /// <summary>
        /// Gets or sets the working local IP address.
        /// </summary>
        public static IPAddress LocalAddress
        {
            get => localip;
            set
            {
                if (!(value?.Equals(localip) ?? false))
                {
                    localip = value;

                }

            }
        }

        protected int timeout = 60000;

        protected int port = DefaultPort;

        protected string addr;

        protected string name;

        protected string bulbType;

        protected BulbParams settings;

        protected bool renaming;

        protected static bool udpActive;

        /// <summary>
        /// Gets or sets the settings object used to configure this bulb.
        /// </summary>
        public virtual BulbParams Settings
        {
            get => settings;
            set
            {
                if (settings == value) return;

                if (settings != null)
                {
                    settings.PropertyChanged -= SettingsChanged;
                }

                settings = value;

                if (settings != null)
                {
                    settings.PropertyChanged += SettingsChanged;
                }

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the bulb type.
        /// </summary>
        public virtual string BulbType
        {
            get => bulbType;
            internal set
            {
                SetProperty(ref bulbType, value);
            }
        }

        /// <summary>
        /// Gets or sets the name of this buld.
        /// </summary>
        public virtual string Name
        {
            get
            {
                if (name == null)
                {
                    name = ToString();
                }

                return name;
            }
            set
            {
                SetProperty(ref name, value);
            }
        }

        public virtual bool Renaming
        {
            get => renaming;
            set
            {
                SetProperty(ref renaming, value);
            }
        }

        /// <summary>
        /// Gets or sets the default timeout for commands (in milliseconds).
        /// </summary>
        public virtual int Timeout
        {
            get => timeout;
            set
            {
                SetProperty(ref timeout, value);
            }
        }

        /// <summary>
        /// Gets the bulb's IP address.
        /// </summary>
        public virtual string IPAddress
        {
            get => addr;
            internal set
            {
                SetProperty(ref addr, value);
            }
        }


        /// <summary>
        /// Gets or sets the communications port (default is 38899).
        /// </summary>
        public virtual int Port
        {
            get => port;
            set
            {
                SetProperty(ref port, value);
            }
        }

        /// <summary>
        /// Gets or sets the brightness of the bulb 
        /// using whole values between 1 and 100.
        /// </summary>
        /// <remarks>
        /// This property is live.
        /// </remarks>
        public virtual byte? Brightness
        {
            get => Settings?.Brightness;
            set
            {
                if (value == null) return;

                if (Settings == null)
                {
                    Settings = new BulbParams();
                }
                
                if (Settings.Brightness == value) return;

                Settings.Brightness = value;

                var stg = new BulbCommand(BulbMethod.SetPilot);
                stg.Params.Brightness = value;

                _ = SendCommand(stg);
            }
        }

        /// <summary>
        /// Gets or sets whether or not the bulb is on.
        /// </summary>
        /// <remarks>
        /// This property is live.
        /// </remarks>
        public virtual bool? IsPoweredOn
        {
            get => Settings?.State;
            set
            {
                if (value == null) return;

                if (Settings == null)
                {
                    Settings = new BulbParams();
                }

                if (Settings.State == value) return;

                Settings.State = value;

                var stg = new BulbCommand(BulbMethod.SetPilot);
                stg.Params.State = value;

                _ = SendCommand(stg).ContinueWith((a) => _ = GetPilot());

            }
        }

        /// <summary>
        /// Gets a description of the current lighting mode.
        /// </summary>
        public virtual string Scene
        {
            get
            {
                if (settings == null)
                {
                    return LightMode.LightModes[0].ToString();
                }
                else if (settings.Scene == null && settings.State == null)
                {
                    return AppResources.UnknownState;
                }
                else if (settings.State == false)
                {
                    return AppResources.Off;
                }
                else if ((settings.Scene ?? 0) == 0)
                {
                    if (settings.Color != null)
                    {
                        var c = (Color)settings.Color;
                        var s = LightMode.FindNameForColor(c);

                        if (s != null)
                        {
                            return $"{AppResources.CustomColor}: {s}";
                        }
                        else
                        {
                            return $"{AppResources.CustomColor}: RGB ({c.R}, {c.G}, {c.B})";
                        }
                    }
                    else
                    {
                        return LightMode.LightModes[0].ToString();
                    }
                }
                else if (LightMode.LightModes.ContainsKey(settings.Scene ?? 0))
                {
                    return LightMode.LightModes[settings.Scene ?? 0].ToString();
                }
                else
                {
                    return LightMode.LightModes[0].ToString();
                }
            }
        }


        /// <summary>
        /// Instantiate a new bulb object.
        /// </summary>
        /// <param name="addr">IP address of the bulb.</param>
        /// <param name="port">Port number for the bulb.</param>
        /// <param name="timeout">Timeout for bulb commands.</param>
        public Bulb(IPAddress addr, int port = DefaultPort, int timeout = 10000)
        {
            this.addr = addr.ToString();
            this.port = port;
            this.timeout = timeout;
        }

        /// <summary>
        /// Instantiate a new bulb object.
        /// </summary>
        /// <param name="addr">IP address of the bulb.</param>
        /// <param name="port">Port number for the bulb.</param>
        /// <param name="timeout">Timeout for bulb commands.</param>
        public Bulb(string addr, int port = DefaultPort, int timeout = 10000) : this(System.Net.IPAddress.Parse(addr), port, timeout)
        {
        }

        /// <summary>
        /// Turn the bulb on.
        /// </summary>
        /// <returns>Result from the bulb.</returns>
        public virtual async Task<BulbCommand> TurnOn()
        {
            var cmd = new BulbCommand(BulbMethod.SetPilot);

            cmd.Params.State = true;
            var ret = await SendCommand(cmd);
            await GetPilot();
            return ret;
        }

        /// <summary>
        /// Turn the bulb off.
        /// </summary>
        /// <returns>Result from the bulb.</returns>
        public virtual async Task<BulbCommand> TurnOff()
        {
            var cmd = new BulbCommand(BulbMethod.SetPilot);

            cmd.Params.State = false;
            var ret = await SendCommand(cmd);
            await GetPilot();
            return ret;
        }

        /// <summary>
        /// Toggles the on/off state of the bulb.
        /// </summary>
        /// <returns>Result from the bulb.</returns>
        public virtual async Task<BulbCommand> Switch()
        {
            if (Settings == null)
            {
                Settings = new BulbParams();
            }
            
            if (Settings.State ?? false)
            {
                return await TurnOff();
            }
            else
            {
                return await TurnOn();
            }
        }


        /// <summary>
        /// Sets the light mode to the specified scene.
        /// </summary>
        /// <param name="scene">Light mode to enable.</param>
        /// <returns>Result from the bulb.</returns>
        public virtual async Task<BulbCommand> SetLightMode(LightMode scene)
        {
            var cmd = new BulbCommand(BulbMethod.SetPilot);

            if (scene.Settings != null)
            {
                cmd.Params = scene.Settings;
            }
            else
            {
                // set scene
                cmd.Params.State = true;
                cmd.Params.Scene = scene.Code;
            }

            var ret = await SendCommand(cmd);
            await GetPilot();
            return ret;
        }

        /// <summary>
        /// Sets the light mode to the specified scene and brightness.
        /// </summary>
        /// <param name="scene">Light mode to enable.</param>
        /// <param name="brightness">Brightness (a whole-number value between 1 and 100)</param>
        /// <returns>Result from the bulb.</returns>
        public virtual async Task<BulbCommand> SetLightMode(LightMode scene, byte brightness)
        {
            var cmd = new BulbCommand(BulbMethod.SetPilot);

            // set scene
            cmd.Params.State = true;
            cmd.Params.Brightness = brightness;
            cmd.Params.Scene = scene.Code;

            var ret = await SendCommand(cmd);
            await GetPilot();
            return ret;

        }

        /// <summary>
        /// Sets the light mode to the specified custom color.
        /// </summary>
        /// <param name="c">Color to enable.</param>
        /// <returns>Result from the bulb.</returns>
        public virtual async Task<BulbCommand> SetLightMode(Color c)
        {
            var cmd = new BulbCommand(BulbMethod.SetPilot);

            // set scene
            cmd.Params.State = true;
            cmd.Params.Red = c.R;
            cmd.Params.Green = c.G;
            cmd.Params.Blue = c.B;

            var ret = await SendCommand(cmd);
            await GetPilot();
            return ret;
        }

        /// <summary>
        /// Sets the light mode to the specified custom color and brightness.
        /// </summary>
        /// <param name="c">Color to enable.</param>
        /// <param name="brightness">Brightness (a whole-number value between 1 and 100)</param>
        /// <returns>Result from the bulb.</returns>
        public virtual async Task<BulbCommand> SetLightMode(Color c, byte brightness)
        {
            var cmd = new BulbCommand(BulbMethod.SetPilot);

            cmd.Params.State = true;
            cmd.Params.Brightness = brightness;
            cmd.Params.WarmWhite = 0;
            cmd.Params.ColdWhite = 0;
            cmd.Params.Red = c.R;
            cmd.Params.Green = c.G;
            cmd.Params.Blue = c.B;

            var ret = await SendCommand(cmd);
            await GetPilot();
            return ret;
        }

        /// <summary>
        /// Refresh the bulb settings with 'getPilot'.
        /// </summary>
        /// <returns>True if successful.</returns>
        public virtual async Task<bool> GetPilot()
        {
            return await GetMethod(BulbMethod.GetPilot);
        }

        /// <summary>
        /// Refresh the bulb settings with 'getSystemConfig'.
        /// </summary>
        /// <returns>True if successful.</returns>
        public virtual async Task<bool> GetSystemConfig()
        {
            return await GetMethod(BulbMethod.GetSystemConfig);
        }


        /// <summary>
        /// Run a 'get' method on the bulb.
        /// </summary>
        /// <param name="m">The method to run (must be a 'get' method)</param>
        /// <returns>True if successful.</returns>
        internal async Task<bool> GetMethod(BulbMethod m)
        {
            if (m.IsSetMethod || m.IsInboundOnly) return false;

            var cmd = new BulbCommand(m);
            string json;
            
            try
            {
                json = cmd.AssembleCommand();
                json = await SendCommand(json);
            }
            catch
            {
                return false;
            }

            if (string.IsNullOrEmpty(json)) return false;

            if (settings != null)
            {
                cmd.Result = Settings;
                JsonConvert.PopulateObject(json, cmd);
            }
            else
            {
                cmd = JsonConvert.DeserializeObject<BulbCommand>(json);
                Settings = cmd.Result;
            }

            OnPropertyChanged("Scene");
            return true;
        }

        /// <summary>
        /// Pulse the bulb
        /// </summary>
        public void Pulse(int delta = -70, int pulseTime = 500)
        {
            var cmd = new BulbCommand(BulbMethod.Pulse);
            cmd.Params = new BulbParams()
            {
                Delta = delta,
                Duration = pulseTime
            };

            _ = SendCommand(cmd);
            // TurnOn();
        }

        /// <summary>
        /// Send configuration back to the blub
        /// </summary>
        internal void SetPilot()
        {
            if (settings == null) return;
            
            var cmd = new BulbCommand(BulbMethod.SetPilot) 
            { 
                Params = Settings.Clone(true) 
            };

            if (cmd.Params.Color != null) cmd.Params.Scene = null;

            _ = SendCommand(cmd);
        }

        /// <summary>
        /// Returns a string representation of the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(name))
            {
                return name;
            }
            else if (Settings?.MACAddress != null)
            {
                return Settings?.MACAddress;
            }
            else
            {
                return addr?.ToString();
            }

        }


        protected void SettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BulbParams.Brightness))
            {
                OnPropertyChanged(nameof(Brightness));
            }
            else if (e.PropertyName == nameof(BulbParams.State))
            {
                OnPropertyChanged(nameof(IsPoweredOn));
            }
        }

        protected async Task<BulbCommand> SendCommand(BulbCommand cmd, bool wait = false)
        {
            var x = await SendCommand(cmd.AssembleCommand(), wait);
            return new BulbCommand(x);
        }

        protected async Task<string> SendCommand(string cmd, bool wait = false)
        {

            byte[] bOut = Encoding.UTF8.GetBytes(cmd);
            var buffer = await SendUDP(bOut, waitForChannel: wait);
            string json;

            if (buffer?.Length > 0)
            {
                json = Encoding.UTF8.GetString(buffer).Trim('\x0');
                if (HasConsole) Console.WriteLine(json + " Addr: " + IPAddress.ToString());

                return json;
            }
            else
            {
                return null;
            }

        }

        protected async Task<byte[]> SendUDP(byte[] cmd, string localAddr = null, bool waitForChannel = false)
        {

            //if (!waitForChannel)
            //{
            //    if (udpActive)
            //    {
            //        return null;
            //    }
            //}
            //else
            //{
            //    while (udpActive)
            //    {
            //        await Task.Delay(100);
            //    }
            //}

            udpActive = true;

            if (HasConsole) Console.WriteLine("Send UDP...");
            List<Bulb> bulbs = new List<Bulb>();

            byte[] buffer = cmd;
            int port = DefaultPort;

            if (localAddr != null)
                LocalAddress = System.Net.IPAddress.Parse(localAddr);

            if (LocalAddress == null)
            {
                udpActive = false;
                Monitor.Exit(udpActive);

                return null;
            }

            var udpClient = new UdpClient();
            udpClient.ExclusiveAddressUse = false;
            udpClient.Client.Bind(new IPEndPoint(LocalAddress, DefaultPort));
            udpClient.DontFragment = true;

            var from = new IPEndPoint(0, 0);
            var timeupVal = DateTime.Now.AddSeconds(timeout);

            var t = Task.Run(async () =>
            {
                int tdelc = 0;

                while (timeupVal > DateTime.Now)
                {
                    if (udpClient.Available > 0)
                    {
                        string json = null;
                        Bulb bulb = null;
                        BulbCommand p = null;

                        try
                        {
                            var recvBuffer = udpClient.Receive(ref from);
                            if (addr == from.Address.ToString())
                            {
                                buffer = recvBuffer;
                                return;
                            }
                        }
                        catch
                        {
                            continue;
                        }

                    }

                    await Task.Delay(100);
                    //tdelc++;

                    //if (tdelc >= 5)
                    //{
                    //    udpClient.Send(buffer, buffer.Length, "255.255.255.255", port);
                    //    tdelc = 0;
                    //}
                }

                if (HasConsole) Console.WriteLine("Finished");
            });

            udpClient.Send(buffer, buffer.Length, addr, port);
            await t;

            udpClient?.Close();
            udpClient?.Dispose();

            udpActive = false;

            return buffer;

        }

        /// <summary>
        /// Scan for bulbs on the specified network.
        /// </summary>
        /// <param name="localAddr">The local IP address to bind to.</param>
        /// <param name="macAddr">The MAC address of the local interface being bound.</param>
        /// <param name="mode">The broadcast <see cref="ScanMode"/> to use when scanning.</param>
        /// <param name="timeout">Timeout for scan, in whole seconds.</param>
        /// <param name="callback">Callback function that is called for each discovered bulb.</param>
        /// <param name="waitForChannel">Wait for the UDP channel to become free if it is in use.</param>
        /// <returns></returns>
        public static async Task<List<Bulb>> ScanForBulbs(string localAddr, string macAddr, ScanModes mode = ScanModes.Registration, int timeout = 5, BulbScanCallback callback = null, bool waitForChannel = false)
        {
            //if (!waitForChannel)
            //{
            //    if (udpActive)
            //    {
            //        return null;
            //    }
            //}
            //else
            //{
            //    while (udpActive)
            //    {
            //        await Task.Delay(100);
            //    }
            //}

            udpActive = true;

            if (HasConsole)
            {
                Console.Clear();
                Console.WriteLine("Scanning For Bulbs...");
            }
            List<Bulb> bulbs = new List<Bulb>();

            byte[] buffer = null;
            int port = DefaultPort;

            LocalAddress = System.Net.IPAddress.Parse(localAddr);

            var udpClient = new UdpClient();

            udpClient.ExclusiveAddressUse = false;
            udpClient.Client.Bind(new IPEndPoint(localip, DefaultPort));
            udpClient.DontFragment = true;

            var from = new IPEndPoint(0, 0);
            var timeupVal = DateTime.Now.AddSeconds(timeout);

            var t = Task.Run(async () =>
            {
                int tdelc = 0;

                while (timeupVal > DateTime.Now)
                {
                    if (udpClient.Available > 0)
                    {
                        string json = null;
                        Bulb bulb = null;
                        BulbCommand p = null;

                        var recvBuffer = udpClient.Receive(ref from);

                        try
                        {
                            json = Encoding.UTF8.GetString(recvBuffer);
                            if (HasConsole) Console.WriteLine(json + " Addr: " + from.Address.ToString());

                            p = new BulbCommand(json);

                            if (p != null && p.Result?.MACAddress != null)
                            {
                                bulb = new Bulb(from.Address);
                                bulb.Settings = p.Result;

                                bool already = false;

                                foreach (var bchk in bulbs)
                                {
                                    if (bchk.Settings.MACAddress == bulb.Settings.MACAddress)
                                    {
                                        already = true;
                                        break;
                                    }
                                }

                                if (already) continue;

                                bulbs.Add(bulb);

                                json = null;
                                p = null;

                                if (callback != null)
                                {
                                    callback(bulb);
                                }

                                bulb = null;
                            }
                        }
                        catch
                        {
                            continue;
                        }

                    }

                    await Task.Delay(100);
                    tdelc++;
                    if (tdelc >= 5)
                    {
                        udpClient.Send(buffer, buffer.Length, "255.255.255.255", port);
                        tdelc = 0;
                    }
                }

                if (HasConsole) Console.WriteLine("Finished");
            });

            var pilot = new BulbCommand();

            if (mode == ScanModes.Registration)
            {
                pilot.Method = BulbMethod.Registration;
                pilot.Params.PhoneMac = macAddr;
                pilot.Params.Register = false;
                pilot.Params.PhoneIp = localAddr;
                pilot.Params.ID = "12";
            }
            else if (mode == ScanModes.GetPilot)
            {
                pilot.Method = BulbMethod.GetPilot;
            }
            else
            {
                pilot.Method = BulbMethod.GetSystemConfig;
            }

            var data = pilot.AssembleCommand();
            buffer = Encoding.UTF8.GetBytes(data);

            udpClient.Send(buffer, buffer.Length, "255.255.255.255", port);
            await t;
            
            udpClient?.Close();
            udpClient?.Dispose();

            udpActive = false;

            return bulbs;
        }

        //public int CompareTo(object obj)
        //{
        //    if (obj is Bulb other)
        //    {
        //        if (Scene != null && other.Scene != null)
        //        {
        //            return Scene.CompareTo(other.Scene);
        //        }
        //        else 
        //        {
        //            var i = string.Compare(Name, other.Name);

        //            if (i == 0)
        //            {

        //                i = string.Compare(IPAddress.ToString(), other.IPAddress.ToString());
        //                if (i == 0)
        //                {
        //                    i = string.Compare(Settings?.MACAddress, other.Settings?.MACAddress);

        //                }
        //            }

        //            return i;
        //        }
        //    }
        //    else
        //    {
        //        return -1;
        //    }
        //}
    }

    //public class BulbComparer : System.Collections.IComparer
    //{
    //    public string PropertyName { get; set; } = "Scene";

    //    public int Compare(object a, object b)
    //    {
    //        var la = a as Bulb;
    //        var lb = b as Bulb;

    //        if (la == null && lb == null)
    //        {
    //            return 0;
    //        }
    //        else if (la == null && lb != null)
    //        {
    //            return -1;
    //        }
    //        else if (la != null && lb == null)
    //        {
    //            return 1;
    //        }
    //        else
    //        {
    //            var sa = la.Scene;
    //            var sb = lb.Scene;

    //            if (sa == null && sb == null)
    //            {
    //                return 0;
    //            }
    //            else if (sa == null && sb != null)
    //            {
    //                return -1;
    //            }
    //            else if (sa != null && sb == null)
    //            {
    //                return 1;
    //            }
    //            else
    //            {
    //                return sa.CompareTo(sb);
    //            }

    //        }
    //    }
    //}
}
