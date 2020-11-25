using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace WizLib
{
    public delegate void BulbScanCallback(Bulb b);

    public enum ScanModes
    {
        Registration,
        GetPilot,
        GetSystemConfig
    }

    public class Bulb : ViewModelBase
    {

        public const int DefaultPort = 38899;

        public static bool HasConsole { get; set; }

        private static IPAddress localip = null;

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

        protected IPAddress addr;

        protected string name;

        protected string bulbType;

        protected Pilot settings;

        protected bool renaming;


        public Pilot Settings
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

        public string BulbType
        {
            get => bulbType;
            set
            {
                SetProperty(ref bulbType, value);
            }
        }

        public string Name
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

        public bool Renaming
        {
            get => renaming;
            set
            {
                SetProperty(ref renaming, value);
            }
        }

        public int Timeout
        {
            get => timeout;
            set
            {
                SetProperty(ref timeout, value);
            }
        }

        public IPAddress IPAddress
        {
            get => addr;
            set
            {
                SetProperty(ref addr, value);
            }
        }

        public int Port
        {
            get => port;
            set
            {
                SetProperty(ref port, value);
            }
        }

        public Bulb(IPAddress addr, int port = DefaultPort, int timeout = 10000)
        {
            this.addr = addr;
            this.port = port;
            this.timeout = timeout;
        }

        public Bulb(string addr, int port = DefaultPort, int timeout = 10000) : this(IPAddress.Parse(addr), port, timeout)
        {
        }

        public void TurnOn()
        {
            var cmd = new PilotCommand();

            cmd.Params.State = true;
            _ = SendCommand(cmd);
        }

        public void TurnOff()
        {
            var cmd = new PilotCommand();

            cmd.Params.State = false;
            _ = SendCommand(cmd);
        }

        public void Switch(bool switchOn)
        {
            if (switchOn) TurnOn();
            else TurnOff();
        }

        public LightMode Scene
        {
            get
            {
                if (settings == null || settings.Scene == null)
                {
                    return null;
                }
                else if (LightMode.LightModes.ContainsKey(settings.Scene ?? 0))
                {
                    return LightMode.LightModes[settings.Scene ?? 0];
                }
                else
                {
                    return null;
                }
            }
        }

        public void SetScene(LightMode scene)
        {
            var cmd = new PilotCommand();

            if (scene.Code == 0)
            {
                // shut off
                TurnOff();
                return;
            }
            else
            {
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
            }

            _ = SendCommand(cmd);
        }

        public void SetScene(LightMode scene, byte brightness)
        {
            var cmd = new PilotCommand();

            if (scene.Code == 0)
            {
                // shut off
                cmd.Params.State = false;
            }
            else
            {
                // set scene
                cmd.Params.State = true;
                cmd.Params.Brightness = brightness;
                cmd.Params.Scene = scene.Code;
            }

            _ = SendCommand(cmd);

        }

        public void SetScene(LightMode scene, Color c)
        {
            var cmd = new PilotCommand();

            if (scene.Code == 0)
            {
                // shut off
                cmd.Params.State = false;
            }
            else
            {
                // set scene
                cmd.Params.State = true;
                cmd.Params.Red = c.R;
                cmd.Params.Green = c.G;
                cmd.Params.Blue = c.B;
                cmd.Params.Scene = scene.Code;
            }

            _ = SendCommand(cmd);
        }

        public void SetScene(Color c, byte brightness)
        {
            var cmd = new PilotCommand();

            cmd.Params.State = true;
            cmd.Params.Brightness = brightness;
            cmd.Params.Red = c.R;
            cmd.Params.Green = c.G;
            cmd.Params.Blue = c.B;
            cmd.Params.Scene = 0;

            _ = SendCommand(cmd);
        }

        public async Task<bool> GetPilot()
        {
            var cmd = new PilotCommand() { Method = "getPilot" };
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
                cmd = JsonConvert.DeserializeObject<PilotCommand>(json);
                Settings = cmd.Result;
            }

            OnPropertyChanged("Scene");
            return true;
        }

        /// <summary>
        /// Wink the bulb
        /// </summary>
        public void Pulse(int delta = -70, int pulseTime = 500)
        {
            _ = SendCommand($"{{\"method\": \"pulse\", \"id\": 22, \"params\": {{\"delta\": {delta}, \"duration\": {pulseTime}}}}}");
            // TurnOn();
        }

        /// <summary>
        /// Send configuration back to the blub
        /// </summary>
        public void SetPilot()
        {
            if (settings == null) return;
            var cmd = new PilotCommand() { Method = "setPilot", Params = Settings.Clone(true) };
            if (cmd.Params.Color != null) cmd.Params.Scene = null;

            _ = SendCommand(cmd);
        }

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

        }

        protected async Task<PilotCommand> SendCommand(PilotCommand cmd)
        {
            var x = await SendCommand(cmd.AssembleCommand());
            return new PilotCommand(x);
        }

        protected async Task<string> SendCommand(string cmd)
        {

            byte[] bOut = Encoding.UTF8.GetBytes(cmd);
            var buffer = await SendUDP(bOut);
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

        //protected byte[] SendUDP(byte[] cmd)
        //{
        //    int maxdg = 100;
        //    int time = 100;

        //    byte[] buffer = new byte[256];

        //    var sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //    sock.Bind(new IPEndPoint(LocalAddress, port));

        //    var ep = new IPEndPoint(addr, port);

        //    sock.Connect(ep);

        //    sock.SendTimeout = sock.ReceiveTimeout = timeout;
        //    sock.Blocking = true;

        //    int dg = 0;
        //    int r = 0;

        //    sock.BeginReceive(buffer, 0, 256, SocketFlags.None, (a) =>
        //    {
        //        if (a.IsCompleted)
        //        {
        //            try
        //            {
        //                r = ((Socket)a.AsyncState)?.EndReceive(a) ?? 0;
        //            }
        //            catch
        //            {

        //            }
        //        }
        //    }, sock);

        //    while (r == 0 && dg < maxdg)
        //    {
        //        dg++;

        //        sock.Send(cmd);
        //        Task.Delay(time);
        //    }

        //    sock?.Close();
        //    sock = null;

        //    return buffer;
        //}


        protected async Task<byte[]> SendUDP(byte[] cmd, string localAddr = null)
        {

            if (HasConsole) Console.WriteLine("Send UDP...");
            List<Bulb> bulbs = new List<Bulb>();

            byte[] buffer = cmd;
            int port = DefaultPort;

            if (localAddr != null)
                LocalAddress = IPAddress.Parse(localAddr);

            if (LocalAddress == null)
            {
                return null;
            }

            var udpClient = new UdpClient();
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
                        PilotCommand p = null;

                        try
                        {
                            var recvBuffer = udpClient.Receive(ref from);
                            buffer = recvBuffer;

                            return;
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
                        //udpClient.Send(buffer, buffer.Length, "255.255.255.255", port);
                        tdelc = 0;
                    }
                }

                if (HasConsole) Console.WriteLine("Finished");
            });

            udpClient.Send(buffer, buffer.Length, addr.ToString(), port);
            await t;

            udpClient?.Close();
            udpClient?.Dispose();

            return buffer;

        }

        public static async Task<List<Bulb>> ScanForBulbs(string localAddr, ScanModes mode = ScanModes.Registration, int timeout = 5, BulbScanCallback callback = null)
        {
            if (HasConsole) Console.WriteLine("Scanning For Bulbs...");
            List<Bulb> bulbs = new List<Bulb>();

            byte[] buffer = null;
            int port = DefaultPort;

            LocalAddress = IPAddress.Parse(localAddr);

            var udpClient = new UdpClient();
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
                        PilotCommand p = null;

                        var recvBuffer = udpClient.Receive(ref from);

                        try
                        {
                            json = Encoding.UTF8.GetString(recvBuffer);
                            if (HasConsole) Console.WriteLine(json + " Addr: " + from.Address.ToString());

                            p = new PilotCommand(json);

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

            var pilot = new PilotCommand();

            if (mode == ScanModes.Registration)
            {
                pilot.Method = "registration";
                pilot.Params.PhoneMac = "94e6f7a27e66";
                pilot.Params.Register = false;
                pilot.Params.PhoneIp = localAddr;
                pilot.Params.ID = "12";
            }
            else if (mode == ScanModes.GetPilot)
            {
                pilot.Method = "getPilot";
            }
            else
            {
                pilot.Method = "getSystemConfig";
            }

            var data = pilot.AssembleCommand();
            buffer = Encoding.UTF8.GetBytes(data);

            udpClient.Send(buffer, buffer.Length, "255.255.255.255", port);
            await t;
            
            udpClient?.Close();
            udpClient?.Dispose();

            return bulbs;
        }

    }
}
