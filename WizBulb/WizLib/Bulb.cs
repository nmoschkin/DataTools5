using System;
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

        public void SetScene(PredefinedScene scene, Color c, byte brightness)
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
                cmd.Params.Red = c.R;
                cmd.Params.Green = c.G;
                cmd.Params.Blue = c.B;
                //cmd.Params.Scene = scene.Code;
            }

            SendCommand(cmd);
        }



        private void SendCommand(PilotCommand cmd)
        {
            byte[] bOut = Encoding.UTF8.GetBytes(cmd.AssembleCommand());
            SendUDP(bOut);
        }

        private void SendUDP(byte[] cmd)
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

            if (r > 0)
            {
                string s = Encoding.UTF8.GetString(buffer).Trim('\x0');
            }

            sock?.Close();
            sock = null;
        }

    }
}
