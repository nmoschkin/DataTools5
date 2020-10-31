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


#if X64
namespace MMHLR64
#else
namespace MMHLR32
#endif
{

    /// <summary>
    /// Contains information about an object received event.
    /// </summary>
    public class ObjectReceivedEventArgs<T> : ObjectReceivedEventArgs
    {
        /// <summary>
        /// Retrieve the received object.
        /// </summary>
        public new T Object
        {
            get => (T)obj;
            protected set
            {
                obj = value;
            }
        }

        public ObjectReceivedEventArgs(T obj, byte[] raw, string typeName, Type type) : base(obj, raw, typeName, type)
        {
        }

        public ObjectReceivedEventArgs(T obj) : base(obj, null, null, null)
        {
        }

        public ObjectReceivedEventArgs(byte[] raw) : base(null, raw, null, null)
        {
        }

    }

    /// <summary>
    /// Generic-typed socket-based object exchanger.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Obex<T> : Obex
    {

        /// <summary>
        /// Delegate that is called when an object is received.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ObjectReceivedEvent<T>(object sender, ObjectReceivedEventArgs<T> e);
        /// <summary>
        /// Event that is triggered when the instance state changes.
        /// </summary>
        public new event StateChangedEvent StateChanged;

        /// <summary>
        /// Event that is triggered when an object is received on a listen instance.
        /// </summary>
        public new event ObjectReceivedEvent<T> ObjectReceived;

        /// <summary>
        /// The obex exchange code.
        /// The instance does not accept objects without this exchange code.
        /// </summary>
        public new int ObexCode
        {
            get => code;
            private set
            {
                if (code == value) return;
                code = value;

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
        public new static Obex<T> CreateServer(EndPoint local, bool fStart = false, JsonSerializerSettings jsettings = null)
        {
            return CreateServer(local, null, null, fStart, jsettings);
        }

        /// <summary>
        /// Create an object exchange server.
        /// </summary>
        /// <param name="local">The local endpoint.</param>
        /// <param name="stateFunc">The state changed handler.</param>
        /// <param name="recvFunc">The object received handler.</param>
        /// <param name="fStart">Whether to start the server.</param>
        /// <param name="jsettings">Json settings.</param>
        /// <returns></returns>
        public static Obex<T> CreateServer(
            EndPoint local,
            StateChangedEvent stateFunc,
            ObjectReceivedEvent<T> recvFunc,
            bool fStart = false,
            JsonSerializerSettings jsettings = null)
        {

            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            var obex = new Obex<T>(socket, true);

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
        public new static Obex<T> CreateClient(EndPoint local, JsonSerializerSettings jsettings = null)
        {
            return CreateClient(local, null, null, null, 0, jsettings);
        }
        
        /// <summary>
        /// Create an object exchange client.
        /// </summary>
        /// <param name="local">Local endpoint.</param>
        /// <param name="remote">Remote endpoint. Set to null to defer connecting.</param>
        /// <param name="timeout">The connect timeout in milliseconds.</param>
        /// <param name="jsettings">Json settings.</param>
        /// <returns>A new object exchanger.</returns>
        public static Obex<T> CreateClient(EndPoint local, EndPoint remote, int timeout, JsonSerializerSettings jsettings = null)
        {
            return CreateClient(local, remote, null, null, timeout, jsettings);
        }

        /// <summary>
        /// Create an object exchange client.
        /// </summary>
        /// <param name="remote">Remote endpoint. Set to null to defer connecting.</param>
        /// <param name="timeout">The connect timeout in milliseconds.</param>
        /// <param name="jsettings">Json settings.</param>
        /// <returns>A new object exchanger.</returns>
        public new static Obex<T> CreateClient(EndPoint remote, int timeout, JsonSerializerSettings jsettings = null)
        {
            return CreateClient(null, remote, null, null, timeout, jsettings);
        }

        /// <summary>
        /// Create an object exchange client.
        /// </summary>
        /// <param name="jsettings">Json settings.</param>
        /// <returns>A new object exchanger.</returns>
        public new static Obex<T> CreateClient(JsonSerializerSettings jsettings = null)
        {
            return CreateClient(null, null, null, null, 0, jsettings);
        }

        /// <summary>
        /// Create an object exchange client.
        /// </summary>
        /// <param name="local">Local endpoint.</param>
        /// <param name="remote">Remote endpoint. Set to null to defer connecting.</param>
        /// <param name="stateFunc">The state changed handler.</param>
        /// <param name="recvFunc">The object received handler.</param>
        /// <param name="timeout">The connect timeout in milliseconds.</param>
        /// <param name="jsettings">Json settings.</param>
        /// <returns>A new object exchanger.</returns>
        public static Obex<T> CreateClient(
            EndPoint local,
            EndPoint remote,
            StateChangedEvent stateFunc,
            ObjectReceivedEvent<T> recvFunc,
            int timeout,
            JsonSerializerSettings jsettings)
        {

            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            var obex = new Obex<T>(socket, true);

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



        protected new bool SendObject(object obj, JsonSerializerSettings jsettings = null)
        {
            return base.SendObject(obj, jsettings);
        }

        public bool SendObject(T obj, JsonSerializerSettings jsettings = null)
        {
            return base.SendObject(obj, jsettings);
        }

        public new T ReceiveObject()
        {
            return ReceiveObject(true, out _, out _);
        }


        public new T ReceiveObject(bool wait)
        {
            return ReceiveObject(wait, out _, out _);
        }

        public new T ReceiveObject(bool wait, out byte[] raw, out string typeName, JsonSerializerSettings jsettings = null)
        {
            return (T)base.ReceiveObject(wait, out raw, out typeName, jsettings);
        }

        protected override void ListenThreadProc(object param)
        {
            bool disc = false;
            bool fAccept = (bool)param;

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

                while (!(cts?.IsCancellationRequested ?? true))
                {
                    buff = new byte[4];
                    r = socket.Receive(buff, 0, 4, SocketFlags.Peek);

                    if (r == 4)
                    {
                        string tn;
                        var obj = ReceiveObject(true, out buff, out tn);
                        ObjectReceived?.Invoke(this, new ObjectReceivedEventArgs<T>(obj, buff, tn, typeof(T)));
                    }

                    Thread.Yield();
                }

            }
            catch (Exception ex)
            {
                state = ObexStates.Error;
                _ = Task.Run(() =>
                {
                    StateChanged?.Invoke(this, new StateChangedEventArgs(state, ex));

                });
            }

            if (disc == true)
            {
                socket?.Dispose();
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

        }


        /// <summary>
        /// Start the listener.
        /// </summary>
        /// <param name="priority">Thread priority.</param>
        /// <param name="fAccept">True to accept a new connection.</param>
        public override void StartListening(ThreadPriority priority = ThreadPriority.Lowest, bool fAccept = true)
        {
            if (fDisposed) throw new ObjectDisposedException(nameof(Obex));

            if ((state & ObexStates.ListenMask) != ObexStates.Unknown) return;

            cts = new CancellationTokenSource();

            thRecv = new Thread(ListenThreadProc);

            thRecv.Priority = priority;
            Debugger.NotifyOfCrossThreadDependency();
            thRecv.Start(fAccept);

        }


        public Obex(Socket socket, bool fOwn = true, JsonSerializerSettings jsettings = null) : base(socket, fOwn, DefaultObexCode, jsettings)
        {
            var type = typeof(T);
            ObexCode = unchecked((int)Crc32.Calculate(Encoding.UTF8.GetBytes(type.AssemblyQualifiedName)));
        }

    }

}
