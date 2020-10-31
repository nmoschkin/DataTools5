using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using System.Windows.Media;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Interop;
using System.Diagnostics;
using Microsoft.VisualBasic.Logging;
using System.Windows;
using DataTools.Memory;
using System.Net;
using System.Printing.IndexedProperties;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Data;


namespace DataTools.ObjectExchange
{

    /// <summary>
    /// Flags that represent the state of an <see cref="Obex"/> instance.
    /// </summary>
    [Flags]
    public enum ObexStates : uint
    {
        
        /// <summary>
        /// The state is unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The instance is waiting for an incoming connection.
        /// </summary>
        Waiting = 1,

        /// <summary>
        /// The instance is connected.
        /// </summary>
        Connected = 2,

        /// <summary>
        /// The instance is disconnected.
        /// </summary>
        Disconnected = 4,

        /// <summary>
        /// The instance is closed.
        /// </summary>
        Closed = 8,

        /// <summary>
        /// The instance is serving.
        /// </summary>
        Serving = 0x10,

        /// <summary>
        /// The instance is listening or serving.
        /// </summary>
        ListenMask = 0x11,

        /// <summary>
        /// The instance timed out.
        /// </summary>
        Timeout = 0x80000000,

        /// <summary>
        /// The instance errored out.
        /// </summary>
        Error = 0x40000000,

        /// <summary>
        /// The listener could not be started because the socket is disconnected and not owned by the obex instance.
        /// </summary>
        ErrorDisconnected = 0x4100000,

        /// <summary>
        /// The instance has an error state.
        /// </summary>
        ErrorMask = 0xc0000000
    }


    public class StateChangedEventArgs : EventArgs
    {
        public ObexStates State { get; private set; } 

        public Exception Exception { get; private set; } = null;

        public bool IsErrorState { get; private set; } = false;

        public StateChangedEventArgs(ObexStates state)
        {
            State = state;

            if ((state & ObexStates.ErrorMask) != ObexStates.Unknown)
            {
                IsErrorState = true;
            }           
        }

        public StateChangedEventArgs(ObexStates state, Exception ex)
        {
            State = state;
            
            if ((ex != null) || ((state & ObexStates.ErrorMask) != ObexStates.Unknown))
            {
                Exception = ex;
                IsErrorState = true;
            }

        }

    }

    public class ObjectReceivedEventArgs : EventArgs
    {
        protected object obj;
        protected bool deserialized;
        protected byte[] rawdata;
        protected DateTime timestamp;

        public virtual DateTime Timestamp
        {
            get => timestamp;
            protected set
            {
                timestamp = value;
            }
        }

        public virtual bool Deserialized
        {
            get => deserialized;
            protected set
            {
                deserialized = value;
            }
        }
        public virtual object Object
        {
            get => obj;
            protected set
            {
                obj = value;
            }
        }

        public virtual byte[] RawData
        {
            get => rawdata;
            protected set
            {
                rawdata = value;
            }
        }

        public ObjectReceivedEventArgs(object obj, byte[] raw)
        {
            Object = obj;
            RawData = raw;
            Deserialized = obj != null;
            Timestamp = DateTime.Now;
        }

        public ObjectReceivedEventArgs(object obj) : this(obj, null)
        {

        }

        public ObjectReceivedEventArgs(byte[] raw) : this(null, raw)
        {
        }

    }

    public class Obex : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// The default obex code. This number was chosen randomly.
        /// </summary>
        public const int DefaultObexCode = 0x43effa38;

        /// <summary>
        /// Delegate that is called when an object is received.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ObjectReceivedEvent(object sender, ObjectReceivedEventArgs e);

        /// <summary>
        /// Event that is triggered when an object is received on a listen instance.
        /// </summary>
        public event ObjectReceivedEvent ObjectReceived;

        /// <summary>
        /// Delegate that is called when the instance state changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void StateChangedEvent(object sender, StateChangedEventArgs e);

        /// <summary>
        /// Event that is triggered when the instance state changes.
        /// </summary>
        public event StateChangedEvent StateChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        protected Socket socket;

        protected int code;
        protected bool owner;

        protected ObexStates state = ObexStates.Disconnected;
        protected CancellationTokenSource cts;

        protected Thread thRecv;
        protected JsonSerializerSettings jsettings = null;


        /// <summary>
        /// Gets or sets a <see cref="JsonSerializerSettings"/> object that will be used during the serialization/deserialization proecess for objects.
        /// Default settings are instantiated if none are specified during instance creation, but the configuration can be set to null, here.
        /// </summary>
        public JsonSerializerSettings JsonSettings
        {
            get => jsettings;
            set
            {
                if (jsettings == value) return;
                jsettings = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the state of the current obex instance.
        /// </summary>
        public virtual ObexStates State
        {
            get => state;
            protected set
            {
                if (state == value) return;
                state = value;

                StateChanged?.Invoke(this, new StateChangedEventArgs(value));
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Returns true if this obex owns the communications socket that is in use.
        /// If this is false, new connections cannot be made by this instance.
        /// </summary>
        public virtual bool IsSocketOwner
        {
            get => owner;
            protected set
            {
                if (owner == value) return;
                owner = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The obex exchange code.
        /// The instance does not accept objects without this exchange code.
        /// </summary>
        public virtual int ObexCode
        {
            get => code;
            set
            {
                if (code == value) return;
                code = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the socket associated with this instance.
        /// </summary>
        internal Socket Socket
        {
            get => socket;
            set
            {
                if (socket == value) return;
                socket = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Create an object exchange server.
        /// </summary>
        /// <param name="local">The local endpoint.</param>
        /// <param name="fStart">Whether to start the server.</param>
        /// <param name="jsettings">Json settings.</param>
        /// <returns></returns>
        public static Obex CreateServer(EndPoint local, bool fStart = false, JsonSerializerSettings jsettings = null)
        {
            return CreateServer(local, null, null, fStart, DefaultObexCode, jsettings);
        }

        /// <summary>
        /// Create an object exchange server.
        /// </summary>
        /// <param name="local">The local endpoint.</param>
        /// <param name="fStart">Whether to start the server.</param>
        /// <param name="code">The obex code.</param>
        /// <param name="jsettings">Json settings.</param>
        /// <returns></returns>
        public static Obex CreateServer(EndPoint local, int code, bool fStart = false, JsonSerializerSettings jsettings = null)
        {
            return CreateServer(local, null, null, fStart, code, jsettings);
        }


        /// <summary>
        /// Create an object exchange server.
        /// </summary>
        /// <param name="local">The local endpoint.</param>
        /// <param name="stateFunc">The state changed handler.</param>
        /// <param name="recvFunc">The object received handler.</param>
        /// <param name="fStart">Whether to start the server.</param>
        /// <param name="code">The obex code.</param>
        /// <param name="jsettings">Json settings.</param>
        /// <returns></returns>
        public static Obex CreateServer(
            EndPoint local,
            StateChangedEvent stateFunc,
            ObjectReceivedEvent recvFunc,
            bool fStart = false,
            int code = DefaultObexCode,
            JsonSerializerSettings jsettings = null)
        {
            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            var obex = new Obex(socket, true, code);

            if (jsettings == null)
            {
                jsettings = new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                    Converters = new JsonConverter[] { new ImageConverter(), new BinaryConverter() }
                };
            }

            obex.JsonSettings = jsettings;

            socket.Bind(local);

            socket.Blocking = true;
            socket.ReceiveBufferSize = socket.SendBufferSize = 10240;

            socket.Listen();

            if (stateFunc != null)
            {
                obex.StateChanged += stateFunc;
            }

            if (recvFunc != null)
            {
                obex.ObjectReceived += recvFunc;
            }

            if (fStart) obex.StartListening();

            return obex;
        }


        /// <summary>
        /// Create an object exchange client.
        /// </summary>
        /// <param name="local">Local endpoint.</param>
        /// <param name="jsettings">Json settings.</param>
        /// <returns>A new object exchanger.</returns>
        public static Obex CreateClient(EndPoint local, JsonSerializerSettings jsettings = null)
        {
            return CreateClient(local, null, null, null, 0, DefaultObexCode, jsettings);
        }
        /// <summary>
        /// Create an object exchange client.
        /// </summary>
        /// <param name="local">Local endpoint.</param>
        /// <param name="code">The obex code.</param>
        /// <param name="jsettings">Json settings.</param>
        /// <returns>A new object exchanger.</returns>
        public static Obex CreateClient(EndPoint local, int code, JsonSerializerSettings jsettings = null)
        {
            return CreateClient(local, null, null, null, 0, code, jsettings);
        }

        /// <summary>
        /// Create an object exchange client.
        /// </summary>
        /// <param name="local">Local endpoint.</param>
        /// <param name="remote">Remote endpoint. Set to null to defer connecting.</param>
        /// <param name="timeout">The connect timeout in milliseconds.</param>
        /// <param name="code">The obex code.</param>
        /// <param name="jsettings">Json settings.</param>
        /// <returns>A new object exchanger.</returns>
        public static Obex CreateClient(EndPoint local, EndPoint remote, int timeout, int code, JsonSerializerSettings jsettings = null)
        {
            return CreateClient(local, remote, null, null, timeout, code, jsettings);
        }

        /// <summary>
        /// Create an object exchange client.
        /// </summary>
        /// <param name="remote">Remote endpoint. Set to null to defer connecting.</param>
        /// <param name="timeout">The connect timeout in milliseconds.</param>
        /// <param name="code">The obex code.</param>
        /// <param name="jsettings">Json settings.</param>
        /// <returns>A new object exchanger.</returns>
        public static Obex CreateClient(EndPoint remote, int timeout, int code, JsonSerializerSettings jsettings = null)
        {
            return CreateClient(null, remote, null, null, timeout, code, jsettings);
        }

        /// <summary>
        /// Create an object exchange client.
        /// </summary>
        /// <param name="jsettings">Json settings.</param>
        /// <returns>A new object exchanger.</returns>
        public static Obex CreateClient(JsonSerializerSettings jsettings = null)
        {
            return CreateClient(null, null, null, null, 0, DefaultObexCode, jsettings);
        }
        
        /// <summary>
        /// Create an object exchange client.
        /// </summary>
        /// <param name="local">Local endpoint.</param>
        /// <param name="remote">Remote endpoint. Set to null to defer connecting.</param>
        /// <param name="stateFunc">The state changed handler.</param>
        /// <param name="recvFunc">The object received handler.</param>
        /// <param name="timeout">The connect timeout in milliseconds.</param>
        /// <param name="code">The obex code.</param>
        /// <param name="jsettings">Json settings.</param>
        /// <returns>A new object exchanger.</returns>
        public static Obex CreateClient(
            EndPoint local,
            EndPoint remote,
            StateChangedEvent stateFunc,
            ObjectReceivedEvent recvFunc,
            int timeout,
            int code,
            JsonSerializerSettings jsettings)
        {

            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            var obex = new Obex(socket, true, code);

            if (jsettings == null)
            {
                jsettings = new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                    Converters = new JsonConverter[] { new ImageConverter(), new BinaryConverter() }
                };
            }

            obex.JsonSettings = jsettings;

            if (local != null)
            {
                socket.Bind(local);
            }

            socket.Blocking = true;
            socket.ReceiveBufferSize = socket.SendBufferSize = 10240;

            if (stateFunc != null)
            {
                obex.StateChanged += stateFunc;
            }

            if (recvFunc != null)
            {
                obex.ObjectReceived += recvFunc;
            }

            if (remote != null)
            {
                obex.Connect(remote, timeout);
            }

            return obex;
        }

        /// <summary>
        /// Connect to a remote endpoint.
        /// </summary>
        /// <param name="ep">Remote endpoint.</param>
        /// <param name="timeout">Timeout in milliseconds.</param>
        /// <returns></returns>
        public bool Connect(EndPoint ep, int timeout = 60000)
        {
            if (fDisposed) throw new ObjectDisposedException(nameof(Obex));
            if (State != ObexStates.Disconnected) return false;

            var ctl = new CancellationTokenSource();

            socket.BeginConnect(ep, (aresult) =>
            {
                var socket = ((Obex)aresult.AsyncState).socket;

                while (!ctl.IsCancellationRequested)
                {
                    if (socket.Poll(100, SelectMode.SelectWrite)) break;
                    Thread.Yield();
                }

                if (ctl.IsCancellationRequested)
                {
                    return;
                }

                socket.EndConnect(aresult);

            }, this);

            var ts = new TimeSpan(0, 0, 0, 0, timeout);
            var n = DateTime.Now;

            while (DateTime.Now - n < ts)
            {
                Thread.Yield();
            }

            if (!socket.Connected)
            {
                ctl.Cancel();
                socket.Close();
                State = ObexStates.Timeout;

                return false;
            }
            else
            {
                State = ObexStates.Connected;
                return true;
            }

        }

        /// <summary>
        /// Start the listener.
        /// </summary>
        /// <param name="priority">Thread priority.</param>
        /// <param name="fAccept">True to accept a new connection.</param>
        public void StartListening(ThreadPriority priority = ThreadPriority.Lowest, bool fAccept = true)
        {
            if (fDisposed) throw new ObjectDisposedException(nameof(Obex));

            if ((state & ObexStates.ListenMask) != ObexStates.Unknown) return;

            cts = new CancellationTokenSource();

            thRecv = new Thread(() =>
            {
                bool disc = false;

                try
                {
                    if (socket == null) return;

                    if (!socket.Connected)
                    {
                        if (fAccept && IsSocketOwner)
                        {
                            disc = true;
                            socket.Blocking = true;

                            _ = Task.Run(() =>
                            {
                                State = ObexStates.Waiting;
                            });

                            socket = socket.Accept();
                        }
                        else
                        {
                            State = ObexStates.ErrorDisconnected;
                            return;
                        }
                    }

                    _ = Task.Run(() =>
                    {
                        State = ObexStates.Serving | ObexStates.Connected;
                    });

                    byte[] buff;
                    int r;

                    while (!cts.IsCancellationRequested)
                    {
                        buff = new byte[4];
                        r = socket.Receive(buff, 0, 4, SocketFlags.Peek);

                        if (r == 4)
                        {
                            var obj = ReceiveObject(true, out buff);
                            ObjectReceived?.Invoke(this, new ObjectReceivedEventArgs(obj, buff));
                        }

                        Thread.Yield();
                    }

                }
                catch(Exception ex)
                {
                    state = ObexStates.Error;
                    _ = Task.Run(() =>
                    {
                        StateChanged?.Invoke(this, new StateChangedEventArgs(state, ex));

                    });
                }

                if (disc == true)
                {
                    socket.Close();
                    _ = Task.Run(() =>
                    {
                        State = ObexStates.Closed;
                    });
                }
                else
                {
                    _ = Task.Run(() =>
                    {
                        State = ObexStates.Connected;
                    });
                }

                cts = null;

            });

            thRecv.Priority = priority;
            thRecv.Start();

        }
              
        public Obex(Socket socket, bool fOwn = true, int code = DefaultObexCode, JsonSerializerSettings jsettings = null)
        {
            if (socket == null) throw new ArgumentNullException(nameof(socket));
            if (code == 0) throw new ArgumentOutOfRangeException(nameof(code));

            if (jsettings == null)
            {
                jsettings = new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                    Converters = new JsonConverter[] { new ImageConverter(), new BinaryConverter() }
                };

            }

            JsonSettings = jsettings;
            IsSocketOwner = fOwn;

            Socket = socket;
            ObexCode = code;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected bool fDisposed;

        public bool IsDisposed
        {
            get => fDisposed;
        }

        protected void Dispose(bool fdisposing)
        {
            if (!fDisposed)
            {
                if (owner)
                {
                    try
                    {
                        cts?.Cancel();
                        cts = null;

                        if (socket != null)
                        {
                            socket.Dispose();
                            socket = null;
                        }

                        if (fdisposing)
                        {
                            State = ObexStates.Closed;
                        }
                    }
                    catch { }
                }

                fDisposed = true;
            }

        }

        ~Obex()
        {
            Dispose(false);
        }

        public virtual bool SendObject(object obj, JsonSerializerSettings jsettings = null)
        {
            try
            {
                if (jsettings == null)
                {
                    jsettings = JsonSettings;
                }

                var s = JsonConvert.SerializeObject(obj, jsettings);
                byte[] objdata = Encoding.UTF8.GetBytes(s);

                var t = obj.GetType().AssemblyQualifiedName;
                byte[] assy = Encoding.UTF8.GetBytes(t);

                int size = objdata.Length + assy.Length + (sizeof(int) * 3);

                MemoryStream ms = new MemoryStream();

                ms.Write(BitConverter.GetBytes(size));
                ms.Write(BitConverter.GetBytes(ObexCode));
                ms.Write(BitConverter.GetBytes(assy.Length));
                ms.Write(assy);
                ms.Write(objdata);

                socket.Send(ms.ToArray());
                return true;
            }
            catch
            {
                return false;
            }
        }

        public object ReceiveObject()
        {
            return ReceiveObject(true, out _);
        }


        public object ReceiveObject(bool wait)
        {
            return ReceiveObject(wait, out _);
        }

        public virtual object ReceiveObject(bool wait, out byte[] raw, JsonSerializerSettings jsettings = null)
        {
            if (jsettings == null)
            {
                jsettings = JsonSettings;
            }

            if (socket == null || (socket != null && socket.Connected == false))
            {
                Thread.Yield();
                raw = null;
                return default;
            }

            Monitor.Enter(socket);

            int tnsize;
            int total;
            int code;
            string typename;
            int count = 4;
            byte[] input = new byte[count];

            int t = 0, c = count;
            int r;

            socket.Blocking = wait;

            while (t < c)
            {
                r = socket.Receive(input, t, c, SocketFlags.None);

                t += r;
                c -= r;

                if (t >= count)
                {
                    break;
                }
                else if (wait == false)
                {
                    raw = input;
                    return default;
                }

                Thread.Sleep(0);
            }

            count = BitConverter.ToInt32(input);

            t = 4;
            c = count - 4;
           
            Array.Resize(ref input, count);

            while (t < c)
            {
                r = socket.Receive(input, t, c, SocketFlags.None);

                t += r;
                c -= r;

                if (t >= count)
                {
                    break;
                }
                else if (wait == false)
                {
                    raw = input;
                    return default;
                }

                Thread.Sleep(0);
            }

            Monitor.Exit(socket);

            raw = input;

            code = BitConverter.ToInt32(input, sizeof(int));
            if (code != ObexCode) return default;

            tnsize = BitConverter.ToInt32(input, sizeof(int) * 2);
            typename = Encoding.UTF8.GetString(input, sizeof(int) * 3, tnsize);

            string s = Encoding.UTF8.GetString(input, (sizeof(int) * 3) + tnsize, count - ((sizeof(int) * 3) + tnsize));

            Type typ = Type.GetType(typename);

            try
            {
                var obj = JsonConvert.DeserializeObject(s, typ, jsettings);
                return obj;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }


        }

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    public class ImageConverter : JsonConverter<ImageSource>
    {
        public override ImageSource ReadJson(JsonReader reader, Type objectType, [AllowNull] ImageSource existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value != null && (reader.Value is string s))
            {
                byte[] fin = BinaryConverter.FromBase64String(s);

                BitmapImage bi = new BitmapImage();

                bi.BeginInit();
                bi.StreamSource = new MemoryStream(fin);
                bi.EndInit();

                return bi;
            }
            else
            {
                throw new JsonException();
            }
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] ImageSource value, JsonSerializer serializer)
        {


            if (value is BitmapImage bi)
            {
                if (bi.StreamSource == null) return;

                byte[] pret;
                pret = new byte[bi.StreamSource.Length];

                if (bi.StreamSource.CanSeek)
                {
                    bi.StreamSource.Seek(0, SeekOrigin.Begin);
                }

                bi.StreamSource.Read(pret, 0, pret.Length);
                writer.WriteValue(BinaryConverter.ToBase64String(pret));
            }
            else
            {
                return;
            }

        }
    }

    public class StructConverter<T> : JsonConverter<T> where T : struct
    {
        public override T ReadJson(JsonReader reader, Type objectType, [AllowNull] T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {

            if (reader.Value is string s)
            {
                SafePtr mm = (SafePtr)BinaryConverter.FromBase64String(s);
                var t = mm.ToStruct<T>();
                mm.Dispose();
                return t;
            }
            else
            {
                return default;
            }

        }

        public override void WriteJson(JsonWriter writer, [AllowNull] T value, JsonSerializer serializer)
        {
            SafePtr mm = new SafePtr();
            mm.FromStruct(value);

            writer.WriteValue(BinaryConverter.ToBase64String((byte[])mm));
            mm.Dispose();
        }
    }


    public class BinaryConverter : JsonConverter<byte[]>
    {

        public static byte[] FromBase64String(string s)
        {
            FromBase64Transform t = new FromBase64Transform();

            byte[] pret = Encoding.ASCII.GetBytes(s);
            byte[] fin = t.TransformFinalBlock(pret, 0, pret.Length);

            return fin;
        }

        public static string ToBase64String(byte[] data)
        {
            byte[] fin;
            ToBase64Transform t = new ToBase64Transform();

            fin = t.TransformFinalBlock(data, 0, data.Length);
            return Encoding.ASCII.GetString(fin);
        }

        public override byte[] ReadJson(JsonReader reader, Type objectType, [AllowNull] byte[] existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value != null && (reader.Value is string s))
            {
                return FromBase64String(s);
            }
            else
            {
                throw new JsonException();
            }
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] byte[] value, JsonSerializer serializer)
        {
            if (value is byte[] bi)
            {
                writer.WriteValue(ToBase64String(bi));
            }
            else
            {
                return;
            }
        }
    }


}
