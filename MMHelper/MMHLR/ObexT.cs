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

    public class ObjectReceivedEventArgs<T> : ObjectReceivedEventArgs
    {
        public new T Object
        {
            get => (T)obj;
            protected set
            {
                obj = value;
            }
        }

        public ObjectReceivedEventArgs(T obj, byte[] raw) : base(obj, raw)
        {
        }

        public ObjectReceivedEventArgs(T obj) : base(obj, null)
        {
        }

        public ObjectReceivedEventArgs(byte[] raw) : base(null, raw)
        {
        }

    }

    public class Obex<T> : Obex
    {
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
            return ReceiveObject(true, out _);
        }


        public new T ReceiveObject(bool wait)
        {
            return ReceiveObject(wait, out _);
        }

        public new T ReceiveObject(bool wait, out byte[] raw, JsonSerializerSettings jsettings = null)
        {
            return (T)base.ReceiveObject(wait, out raw, jsettings);
        }

        public Obex(Socket socket, bool fOwn = true, JsonSerializerSettings jsettings = null) : base(socket, fOwn, DefaultObexCode, jsettings)
        {
            var type = typeof(T);
            ObexCode = unchecked((int)Crc32.Calculate(Encoding.UTF8.GetBytes(type.AssemblyQualifiedName)));
        }

    }

}
