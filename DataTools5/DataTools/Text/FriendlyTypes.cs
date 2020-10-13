// ************************************************* ''
// DataTools C# Native Utility Library For Windows 
//
// Module: Friendly numeric types.
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace DataTools.Text
{
    public static class TicksAndSecs
    {

        /// <summary>
        /// Convert ticks to seconds (divide by 10,000,000 and round)
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static uint TicksToSecs(long ticks)
        {
            double dd = ticks;
            dd /= 10000000;
            return System.Convert.ToUInt32(dd);
        }

        /// <summary>
        /// Convert seconds to ticks (multiply by 10,000,000)
        /// </summary>
        /// <param name="secs"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static long SecsToTicks(uint secs)
        {
            double dd = secs;
            dd *= 10000000;
            return System.Convert.ToInt64(dd);
        }
    }

    /// <summary>
    ///     ''' A friendly-formatting Unix time to CLI TimeSpan marshaling structure.
    ///     ''' </summary>
    ///     ''' <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct FriendlySeconds
    {
        private int _value;

        public override string ToString()
        {
            TimeSpan ts = new TimeSpan(0, 0, _value);
            return ts.ToString();
        }

        public string ToString(string format)
        {
            TimeSpan ts = new TimeSpan(0, 0, _value);
            return ts.ToString(format);
        }

        public FriendlySeconds(int val)
        {
            _value = val;
        }

        public static implicit operator FriendlySeconds(TimeSpan operand)
        {
            return new FriendlySeconds((int)operand.TotalSeconds);
        }

        public static implicit operator TimeSpan(FriendlySeconds operand)
        {
            return new TimeSpan(0, 0, operand._value);
        }

        public static implicit operator FriendlySeconds(uint operand)
        {
            return new FriendlySeconds((int)operand);
        }

        public static implicit operator uint(FriendlySeconds operand)
        {
            return (uint)operand._value;
        }

        public static implicit operator FriendlySeconds(int operand)
        {
            return new FriendlySeconds(operand);
        }

        public static implicit operator int(FriendlySeconds operand)
        {
            return operand._value;
        }

        public static implicit operator FriendlySeconds(ushort operand)
        {
            return new FriendlySeconds(operand);
        }

        public static implicit operator ushort(FriendlySeconds operand)
        {
            return (ushort)operand._value;
        }

        public static implicit operator FriendlySeconds(short operand)
        {
            return new FriendlySeconds(operand);
        }

        public static implicit operator short(FriendlySeconds operand)
        {
            return (short)operand._value;
        }

        public static implicit operator FriendlySeconds(ulong operand)
        {
            return new FriendlySeconds((int)operand);
        }

        public static implicit operator ulong(FriendlySeconds operand)
        {
            return (ulong)operand._value;
        }

        public static implicit operator FriendlySeconds(long operand)
        {
            return new FriendlySeconds((int)operand);
        }

        public static implicit operator long(FriendlySeconds operand)
        {
            return operand._value;
        }

        public static implicit operator FriendlySeconds(byte operand)
        {
            return new FriendlySeconds(operand);
        }

        public static implicit operator byte(FriendlySeconds operand)
        {
            return (byte)operand._value;
        }

        public static implicit operator FriendlySeconds(sbyte operand)
        {
            return new FriendlySeconds(operand);
        }

        public static implicit operator sbyte(FriendlySeconds operand)
        {
            return (sbyte)operand._value;
        }
    }

    /// <summary>
    ///     ''' A friendly-formatting Unix time to CLI DateTime marshaling structure.
    ///     ''' </summary>
    ///     ''' <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct FriendlyUnixTime
    {
        private uint _value;

        public override string ToString()
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(_value).ToLocalTime().ToString();
        }

        public string ToString(string format)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(_value).ToLocalTime().ToString(format);
        }

        public FriendlyUnixTime(uint val)
        {
            _value = val;
        }

        public static implicit operator FriendlyUnixTime(DateTime operand)
        {
            return new FriendlyUnixTime(TicksAndSecs.TicksToSecs(operand.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks));
        }

        public static implicit operator DateTime(FriendlyUnixTime operand)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(operand._value).ToLocalTime();
        }

        public static implicit operator FriendlyUnixTime(uint operand)
        {
            return new FriendlyUnixTime(operand);
        }

        public static implicit operator uint(FriendlyUnixTime operand)
        {
            return operand._value;
        }

        public static implicit operator FriendlyUnixTime(ulong operand)
        {
            return new FriendlyUnixTime((uint)operand);
        }

        public static implicit operator ulong(FriendlyUnixTime operand)
        {
            return operand._value;
        }

        public static implicit operator FriendlyUnixTime(long operand)
        {
            return new FriendlyUnixTime((uint)operand);
        }

        public static implicit operator long(FriendlyUnixTime operand)
        {
            return operand._value;
        }

        public static implicit operator string(FriendlyUnixTime operand)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(operand._value).ToLocalTime().ToString("O");
        }

        public static explicit operator FriendlyUnixTime(string operand)
        {
            return new FriendlyUnixTime(TicksAndSecs.TicksToSecs(DateTime.Parse(operand).ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks));
        }

        public static FriendlyUnixTime operator +(FriendlyUnixTime operand1, short operand2)
        {
            operand1._value += (uint)operand2;
            return operand1;
        }

        public static FriendlyUnixTime operator -(FriendlyUnixTime operand1, short operand2)
        {
            operand1._value -= (uint)operand2;
            return operand1;
        }

        public static FriendlyUnixTime operator +(FriendlyUnixTime operand1, ushort operand2)
        {
            operand1._value += operand2;
            return operand1;
        }

        public static FriendlyUnixTime operator -(FriendlyUnixTime operand1, ushort operand2)
        {
            operand1._value -= operand2;
            return operand1;
        }

        public static FriendlyUnixTime operator +(FriendlyUnixTime operand1, int operand2)
        {
            operand1._value += (uint)operand2;
            return operand1;
        }

        public static FriendlyUnixTime operator -(FriendlyUnixTime operand1, int operand2)
        {
            operand1._value -= (uint)operand2;
            return operand1;
        }

        public static FriendlyUnixTime operator +(FriendlyUnixTime operand1, uint operand2)
        {
            operand1._value += operand2;
            return operand1;
        }

        public static FriendlyUnixTime operator -(FriendlyUnixTime operand1, uint operand2)
        {
            operand1._value -= operand2;
            return operand1;
        }

        public static FriendlyUnixTime operator +(FriendlyUnixTime operand1, long operand2)
        {
            operand1._value += (uint)operand2;
            return operand1;
        }

        public static FriendlyUnixTime operator -(FriendlyUnixTime operand1, long operand2)
        {
            operand1._value -= (uint)operand2;
            return operand1;
        }

        public static FriendlyUnixTime operator +(FriendlyUnixTime operand1, ulong operand2)
        {
            operand1._value += (uint)operand2;
            return operand1;
        }

        public static FriendlyUnixTime operator -(FriendlyUnixTime operand1, ulong operand2)
        {
            operand1._value -= (uint)operand2;
            return operand1;
        }
    }

    /// <summary>
    ///     ''' Stores a number whose default ToString() format is pretty size.
    ///     ''' </summary>
    ///     ''' <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct FriendlySizeLong : INotifyPropertyChanged
    {
        private ulong _Value;
        public event PropertyChangedEventHandler PropertyChanged;

        public ulong Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Double"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public double DoubleValue
        {
            get
            {
                return System.Convert.ToDouble(Value);
            }
            set
            {
                this.Value = System.Convert.ToUInt64(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Double"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public FriendlySizeLong(long v)
        {
            _Value = ((ulong)v & 0x7FFFFFFFFFFFFFFFUL);
            PropertyChanged = null;
        }

        public FriendlySizeLong(ulong v)
        {
            _Value = v;
            PropertyChanged = null;
        }

        public static implicit operator FriendlySizeLong(long operand)
        {
            return new FriendlySizeLong(operand);
        }

        public static implicit operator long(FriendlySizeLong operand)
        {
            return (long)(operand.Value);
        }

        public static implicit operator FriendlySizeLong(ulong operand)
        {
            return new FriendlySizeLong(operand);
        }

        public static implicit operator ulong(FriendlySizeLong operand)
        {
            return operand.Value;
        }

        public static explicit operator string(FriendlySizeLong operand)
        {
            return operand.ToString();
        }

        public static implicit operator FriendlySizeLong(double operand)
        {
            return new FriendlySizeLong() { DoubleValue = operand };
        }

        public static implicit operator double(FriendlySizeLong operand)
        {
            return operand.DoubleValue;
        }

        public override string ToString()
        {
            return TextTools.PrintFriendlySize(Value);
        }
    }

    /// <summary>
    ///     ''' Stores a number whose default ToString() format is pretty speed.
    ///     ''' </summary>
    ///     ''' <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct FriendlySpeedLong : INotifyPropertyChanged
    {
        private ulong _Value;
        public event PropertyChangedEventHandler PropertyChanged;

        public ulong Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Double"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public double DoubleValue
        {
            get
            {
                return System.Convert.ToDouble(Value);
            }
            set
            {
                this.Value = System.Convert.ToUInt64(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Double"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public FriendlySpeedLong(long v)
        {
            _Value = ((ulong)v & 0x7FFFFFFFFFFFFFFFL);
            PropertyChanged = null;
        }

        public FriendlySpeedLong(ulong v)
        {
            _Value = v;
            PropertyChanged = null;
        }

        public static implicit operator FriendlySpeedLong(long operand)
        {
            return new FriendlySpeedLong(operand);
        }

        public static implicit operator long(FriendlySpeedLong operand)
        {
            return (long)(operand.Value & 0x7FFFFFFFFFFFFFFFUL);
        }

        public static implicit operator FriendlySpeedLong(ulong operand)
        {
            return new FriendlySpeedLong(operand);
        }

        public static implicit operator ulong(FriendlySpeedLong operand)
        {
            return operand.Value;
        }

        public static explicit operator string(FriendlySpeedLong operand)
        {
            return operand.ToString();
        }

        public static implicit operator FriendlySpeedLong(double operand)
        {
            return new FriendlySpeedLong() { DoubleValue = operand };
        }

        public static implicit operator double(FriendlySpeedLong operand)
        {
            return operand.DoubleValue;
        }

        public override string ToString()
        {
            return TextTools.PrintFriendlySpeed((long)Value);
        }
    }



    /// <summary>
    ///     ''' Stores a number whose default ToString() format is pretty size.
    ///     ''' </summary>
    ///     ''' <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct FriendlySizeInteger : INotifyPropertyChanged
    {
        private uint _Value;
        public event PropertyChangedEventHandler PropertyChanged;

        public uint Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Single"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public float SingleValue
        {
            get
            {
                return System.Convert.ToSingle(Value);
            }
            set
            {
                this.Value = System.Convert.ToUInt32(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Single"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public FriendlySizeInteger(int v)
        {
            _Value = (uint)v;
            PropertyChanged = null;
        }

        public FriendlySizeInteger(uint v)
        {
            _Value = v;
            PropertyChanged = null;
        }

        public static implicit operator FriendlySizeInteger(int operand)
        {
            return new FriendlySizeInteger(operand);
        }

        public static implicit operator int(FriendlySizeInteger operand)
        {
            return System.Convert.ToInt32(operand.Value & 0x7FFFFFFFU);
        }

        public static implicit operator FriendlySizeInteger(uint operand)
        {
            return new FriendlySizeInteger(operand);
        }

        public static implicit operator uint(FriendlySizeInteger operand)
        {
            return operand.Value;
        }

        public static explicit operator string(FriendlySizeInteger operand)
        {
            return operand.ToString();
        }

        public static implicit operator FriendlySizeInteger(float operand)
        {
            return new FriendlySizeInteger() { SingleValue = operand };
        }

        public static implicit operator float(FriendlySizeInteger operand)
        {
            return operand.SingleValue;
        }

        public override string ToString()
        {
            return TextTools.PrintFriendlySize(Value);
        }
    }

    /// <summary>
    ///     ''' Stores a number whose default ToString() format is pretty speed.
    ///     ''' </summary>
    ///     ''' <remarks></remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct FriendlySpeedInteger : INotifyPropertyChanged
    {
        private uint _Value;
        public event PropertyChangedEventHandler PropertyChanged;

        public uint Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Single"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public float SingleValue
        {
            get
            {
                return System.Convert.ToSingle(Value);
            }
            set
            {
                this.Value = System.Convert.ToUInt32(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Single"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public FriendlySpeedInteger(int v)
        {
            _Value = (uint)(v & 0x7FFFFFFF);
            PropertyChanged = null;
        }

        public FriendlySpeedInteger(uint v)
        {
            _Value = v;
            PropertyChanged = null;
        }

        public static implicit operator FriendlySpeedInteger(int operand)
        {
            return new FriendlySpeedInteger(operand);
        }

        public static implicit operator int(FriendlySpeedInteger operand)
        {
            return System.Convert.ToInt32(operand.Value & 0x7FFFFFFFU);
        }

        public static implicit operator FriendlySpeedInteger(uint operand)
        {
            return new FriendlySpeedInteger(operand);
        }

        public static implicit operator uint(FriendlySpeedInteger operand)
        {
            return operand.Value;
        }

        public static explicit operator string(FriendlySpeedInteger operand)
        {
            return operand.ToString();
        }

        public static implicit operator FriendlySpeedInteger(float operand)
        {
            return new FriendlySpeedInteger() { SingleValue = operand };
        }

        public static implicit operator float(FriendlySpeedInteger operand)
        {
            return operand.SingleValue;
        }

        public override string ToString()
        {
            return TextTools.PrintFriendlySpeed(Value);
        }
    }
}
