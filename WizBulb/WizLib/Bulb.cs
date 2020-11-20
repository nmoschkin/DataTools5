using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WizLib
{

    public class Bulb
    {
        public const int DefaultPort = 38899;

        private int timeout = 60000;

        private int port = DefaultPort;

        private EndPoint ep;

        private IPAddress addr;

        private string name;

        private Pilot settings;

        public static bool HasConsole { get; set; }

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
            }
        }

        public string Name
        {
            get => name;
            set
            {
                if (name == value) return;
                name = value;
            }
        }

        public int Timeout
        {
            get => timeout;
            set
            {
                if (timeout == value) return;
                timeout = value;
            }
        }

        public IPAddress Address
        {
            get => addr;
            set
            {
                if (addr == value) return;
                addr = value;

                ep = new IPEndPoint(addr, Port);
            }
        }

        public int Port
        {
            get => port;
            set
            {
                if (port == value) return;
                port = value;

                ep = new IPEndPoint(addr, Port);
            }
        }

        public Bulb(IPAddress addr, int port = DefaultPort, int timeout = 60000)
        {
            this.addr = addr;
            this.port = port;
            this.timeout = timeout;

            ep = new IPEndPoint(this.addr, this.port);
        }

        public Bulb(string addr, int port = DefaultPort, int timeout = 60000) : this(IPAddress.Parse(addr), port, timeout)
        {
        }

        public void TurnOn()
        {
            var cmd = new PilotCommand();

            cmd.Params.State = true;
            SendCommand(cmd);
        }

        public void TurnOff()
        {
            var cmd = new PilotCommand();

            cmd.Params.State = false;
            SendCommand(cmd);
        }

        public void Switch(bool switchOn)
        {
            if (switchOn) TurnOn();
            else TurnOff();
        }

        public void SetScene(PredefinedScene scene)
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

            SendCommand(cmd);
        }

        public void SetScene(PredefinedScene scene, byte brightness)
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

            SendCommand(cmd);

        }

        public void SetScene(PredefinedScene scene, Color c)
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

            SendCommand(cmd);
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

            SendCommand(cmd);
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


        private void SettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        public void GetPilot()
        {
            var cmd = new PilotCommand() { Method = "getPilot" };
            cmd = SendCommand(cmd);

            Settings = cmd.Params;
        }

        private PilotCommand SendCommand(PilotCommand cmd)
        {
            byte[] bOut = Encoding.UTF8.GetBytes(cmd.AssembleCommand());
            var buffer = SendUDP(bOut);

            if (buffer?.Length > 0)
            {
                var s = Encoding.UTF8.GetString(buffer).Trim('\x0');
                return new PilotCommand(s);
            }
            else
            {
                return null;
            }

        }

        private byte[] SendUDP(byte[] cmd)
        {
            int maxdg = 100;
            int time = 500;

            byte[] buffer = new byte[256];

            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            sock.Connect(ep);
            sock.SendTimeout = sock.ReceiveTimeout = timeout;
            sock.Blocking = true;

            int dg = 0;
            int r = 0;

            sock.BeginReceive(buffer, 0, 256, SocketFlags.None, (a) =>
            {
                if (a.IsCompleted)
                {
                    r = 1;
                    try
                    {
                        ((Socket)a.AsyncState)?.EndReceive(a);
                    }
                    catch
                    {

                    }
                }
            }, sock);

            while (r == 0 && dg < maxdg)
            {
                dg++;

                sock.Send(cmd);
                Task.Delay(time);
            }

            sock?.Close();
            sock = null;

            return buffer;
        }



        public static async Task<List<Bulb>> ScanForBulbs(bool scanOnly = false)
        {
            if (HasConsole) Console.WriteLine("Scanning For Bulbs...");
            List<Bulb> bulbs = new List<Bulb>();

            int PORT = 38899;
            UdpClient udpClient = new UdpClient();
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Parse("192.168.1.10"), PORT));

            var from = new IPEndPoint(0, 0);
            var timeout = (DateTime.Now.AddSeconds(5));

            var t = Task.Run(async () =>
            {
                while (timeout > DateTime.Now)
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

                            if (p != null)
                            {
                                bulb = new Bulb(from.Address);
                                bulb.Settings = p.Result;
                                bulbs.Add(bulb);

                                bulb = null;
                                json = null;
                                p = null;
                            }

                        }
                        catch
                        {
                            continue;
                        }

                    }

                    await Task.Delay(100);
                }

                if (HasConsole) Console.WriteLine("Finished");
            });

            var pilot = new PilotCommand();

            if (scanOnly)
            {
                pilot.Method = "registration";
                pilot.Params.PhoneMac = "94e6f7a27e66";
                pilot.Params.Register = false;
                pilot.Params.PhoneIp = "192.168.1.10";
                pilot.Params.ID = "12";
            }
            else
            {
                pilot.Method = "getPilot";
            }

            var data = pilot.AssembleCommand();
            var buffer = Encoding.UTF8.GetBytes(data);

            udpClient.Send(buffer, buffer.Length, "255.255.255.255", PORT);
            await t;

            return bulbs;
        }

    }
}
