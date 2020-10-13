// ************************************************* ''
// DataTools C# Native Utility Library For Windows 
//
// Module: Byte Order Marker Library
//         For Mulitple Character Encodings
// 
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using DataTools.Memory;
using System.Security.Authentication;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace DataTools.Text.ByteOrderMark
{
    public enum CPCharTypes
    {
        Undefined = 0,
        Control = 1,
        Punctuation = 2,
        Alpha = 3,
        Digit = 5,
        Symbol = 6,
        Other = 8
    }

    public enum ControlLayers
    {
        C0 = 0,
        C1 = 1
    }

    public enum ControlCodes
    {

        // C0 control constants
        NUL = 0x0,
        SOH = 0x1,
        STX = 0x2,
        ETX = 0x3,
        EOT = 0x4,
        ENQ = 0x5,
        ACK = 0x6,
        BEL = 0x7,
        BS = 0x8,
        HT = 0x9,
        LF = 0xA,
        VT = 0xB,
        FF = 0xC,
        CR = 0xD,
        SO = 0xE,
        SI = 0xF,
        DLE = 0x10,
        DC1 = 0x11,
        DC2 = 0x12,
        DC3 = 0x13,
        DC4 = 0x14,
        NAK = 0x15,
        SYN = 0x16,
        ETB = 0x17,
        CAN = 0x18,
        EM = 0x19,
        SUB = 0x1A,
        ESC = 0x1B,
        FS = 0x1,
        GS = 0x1D,
        RS = 0x1E,
        US = 0x1F,
        SP = 0x20,
        DEL = 0x7F,

        // C1 control constants
        PAD = 0x80,
        HOP = 0x81,
        BPH = 0x82,
        NBH = 0x83,
        IND = 0x84,
        NEL = 0x85,
        SSA = 0x86,
        ESA = 0x87,
        HTS = 0x88,
        HTJ = 0x89,
        VTS = 0x8A,
        PLD = 0x8B,
        PLU = 0x8,
        RI = 0x8D,
        SS2 = 0x8E,
        SS3 = 0x8F,
        DCS = 0x90,
        PU1 = 0x91,
        PU2 = 0x92,
        STS = 0x93,
        CCH = 0x94,
        MW = 0x95,
        SPA = 0x96,
        EPA = 0x97,
        SOS = 0x98,
        SGCI = 0x99,
        SCI = 0x9A,
        CSI = 0x9B,
        ST = 0x9,
        OSC = 0x9D,
        PM = 0x9E,
        APC = 0x9F,

        // EBCDIC Specific
        NSP = 0xE1,
        EO = 0xFF
    }

    public enum CPSignalEscapes
    {
        None = 0,
        Ctrl = 1,
        Esc = 2
    }



    [Description("Contains an escape sequence")]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct EscapeSequence
    {
        public CPSignalEscapes Control;
        public char Character;

        public EscapeSequence(CPSignalEscapes ctrl, char ch)
        {
            Control = ctrl;
            Character = ch;
        }

        public override string ToString()
        {
            if (Control.ToString() != "None")
                return Control.ToString() + "+" + Character;
            else
                return Character.ToString();
        }
    }

    [Description("Describes a control signal character.")]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct SignalElement
    {
        private ControlCodes _Code;
        private ControlLayers _Layer;
        private EscapeSequence _Sequence;
        private char _Symbol;
        private char _Escape;
        private string _Descrption;
        private string _Acronym;
        private string _Name;

        public ControlCodes Code
        {
            get
            {
                return _Code;
            }
            internal set
            {
                _Code = value;
            }
        }

        public ControlLayers Layer
        {
            get
            {
                return _Layer;
            }
            internal set
            {
                _Layer = value;
            }
        }

        public EscapeSequence Sequence
        {
            get
            {
                return _Sequence;
            }
            internal set
            {
                _Sequence = value;
            }
        }

        public char Symbol
        {
            get
            {
                return _Symbol;
            }
            internal set
            {
                _Symbol = value;
            }
        }

        public char Escape
        {
            get
            {
                return _Escape;
            }
            internal set
            {
                _Escape = value;
            }
        }

        public string Descrption
        {
            get
            {
                return _Descrption;
            }
            internal set
            {
                _Descrption = value;
            }
        }

        public string Acronym
        {
            get
            {
                return _Acronym;
            }
            internal set
            {
                _Acronym = value;
            }
        }

        public string Name
        {
            get
            {
                return _Name;
            }
            internal set
            {
                _Name = value;
            }
        }

        public override string ToString()
        {
            return Code.ToString();
        }

        public static SignalElement Parse(string str)
        {
            SignalElement cc = new SignalElement();
            string[] s = TextTools.Split(str, "|");
            string[] sd;

            sd = TextTools.Split(str, "\t");

            switch (sd.Length)
            {
                case 8:
                    {
                        // C0 code
                        // Signals.AddSignal("Seq	Dec	Hex	Acro	Symb	Name	C	Description")

                        cc.Layer = ControlLayers.C0;

                        if ((sd[0][0] == '^'))
                            cc.Sequence = new EscapeSequence(CPSignalEscapes.Ctrl, sd[0][1]);
                        else
                            cc.Sequence = new EscapeSequence(CPSignalEscapes.None, sd[0][0]);

                        cc.Code = (ControlCodes)Enum.Parse(typeof(ControlCodes), sd[3]);
                        cc.Acronym = cc.ToString();
                        if (sd[6] != "")
                            cc.Escape = sd[6][1];
                        cc.Descrption = sd[7].Replace("\n", "\r\n");
                        cc.Name = sd[5];
                        cc.Symbol = sd[4][0];
                        break;
                    }

                case 6:
                    {
                        // C1 code
                        // Signals.AddSignal("Seq	Dec	Hex	Acro	Name	Description")

                        cc.Layer = ControlLayers.C1;
                        cc.Sequence = new EscapeSequence(CPSignalEscapes.Esc, sd[0][0]);
                        cc.Code =(ControlCodes) Enum.Parse(typeof(ControlCodes), sd[3]);
                        cc.Acronym = cc.ToString();
                        cc.Descrption = sd[5].Replace("\n", "\r\n");
                        cc.Name = sd[4];
                        break;
                    }

                case 5:
                    {
                        cc.Layer = ControlLayers.C1;
                        cc.Sequence = new EscapeSequence(CPSignalEscapes.Esc, sd[0][0]);
                        cc.Code = (ControlCodes)Enum.Parse(typeof(ControlCodes), sd[3]);
                        cc.Acronym = cc.ToString();
                        // cc.Descrption = SearchReplace(sd(4), "\n", vbCrLf)
                        cc.Name = sd[4];
                        break;
                    }
            }

            return cc;
        }

        public static implicit operator ControlCodes(SignalElement operand)
        {
            return operand.Code;
        }

        public static explicit operator SignalElement(ControlCodes operand)
        {
            SignalElement cc = new SignalElement();
            cc.Code = operand;
            cc.Acronym = operand.ToString();

            if ((int)cc.Code < 0x80)
                cc.Layer = ControlLayers.C0;
            else
                cc.Layer = ControlLayers.C1;

            return cc;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct CodePageElement
    {
        private CPCharTypes _Type;
        private SignalElement _Signal;
        private uint _Code;
        private char _Unicode;
        private char _Ascii;
        private string _Description;

        public CPCharTypes Type
        {
            get
            {
                return _Type;
            }
            internal set
            {
                _Type = value;
            }
        }

        public SignalElement Signal
        {
            get
            {
                return _Signal;
            }
            internal set
            {
                _Signal = value;
            }
        }

        public uint Code
        {
            get
            {
                return _Code;
            }
            internal set
            {
                _Code = value;
            }
        }

        public char Unicode
        {
            get
            {
                return _Unicode;
            }
            internal set
            {
                _Unicode = value;
            }
        }

        public char Ascii
        {
            get
            {
                return _Ascii;
            }
            internal set
            {
                _Ascii = value;
            }
        }

        public string Description
        {
            get
            {
                return _Description;
            }
            internal set
            {
                _Description = value;
            }
        }

        public static CodePageElement Parse(string str)
        {
            CodePageElement cp = new CodePageElement();
            string[] s = TextTools.Split(str, "|");
            string[] sd;

            uint val;

            ControlCodes cc = ControlCodes.NUL;

            cp.Type = (CPCharTypes)Enum.Parse(typeof(CPCharTypes), s[0]);

            if ((s[1].Trim()) != "")
            {
                if (uint.TryParse(s[1], NumberStyles.HexNumber, CultureInfo.CurrentCulture, out val))
                {
                    cp.Unicode = (char)val;

                }
            }

            if (s.Length == 3)
            {
                if (uint.TryParse(s[2], NumberStyles.HexNumber, CultureInfo.CurrentCulture, out val))
                {
                    cp.Code = (char)val;

                }
                return cp;
            }

            if (uint.TryParse(s[3], NumberStyles.HexNumber, CultureInfo.CurrentCulture, out val))
            {
                cp.Code = (char)val;

            }

            if (cp.Type == CPCharTypes.Undefined)
                return cp;

            if (s[2].IndexOf(",") > -1)
            {
                sd = TextTools.Split(s[2], ",");
                cp.Description = sd[0];
                cp.Signal = new SignalElement();

                if (Enum.TryParse<ControlCodes>(sd[1], out cc))
                {
                    cp.Signal = CPGlobal.Signals[cc];
                    cp.Ascii = (char)(cp.Signal.Code);
                }
                else if (sd[1].Length > 2 && sd[1].Substring(0, 2) == "&H")
                    cp.Ascii = (char)ushort.Parse(s[1].Substring(2), NumberStyles.HexNumber, CultureInfo.CurrentCulture);
                else
                    cp.Ascii = sd[1][0];
            }
            else if (Enum.TryParse<ControlCodes>(s[2], out cc))
            {
                cp.Signal = new SignalElement();

                cp.Signal = CPGlobal.Signals[cc];
                cp.Ascii = (char)(cp.Signal.Code);
            }
            else if (s[2].Length > 2 && s[2].Substring(0, 2) == "&H")
                cp.Ascii = (char)ushort.Parse(s[2].Substring(2), NumberStyles.HexNumber, CultureInfo.CurrentCulture);
            else
                cp.Ascii = s[2][0];

            return cp;
        }
    }



    public enum BOMTYPE : short
    {
        ASCII = 0,
        UTF8 = 2,
        UTF16BE = 3,
        UTF16LE = 4,
        UTF32BE = 5,
        UTF32LE = 6,
        UTF7a = 7,
        UTF7b = 8,
        UTF7c = 9,
        UTF7d = 10,
        UTF7e = 11,
        UTF1 = 12,
        UTFEBCDIC = 13,
        SCSU = 14,
        SCSU_BOCU1 = 15,
        SCSU_GB18030 = 16
    }

    public struct BOM
    {
        internal BOMTYPE _Type;
        internal byte[] _BOM;

        private static bool mInit;
        private bool myInit;

        public static BOM UTF8;
        public static BOM UTF16BE;
        public static BOM UTF16LE;
        public static BOM UTF32BE;
        public static BOM UTF32LE;
        public static BOM UTF7a;
        public static BOM UTF7b;
        public static BOM UTF7c;
        public static BOM UTF7d;
        public static BOM UTF7e;
        public static BOM UTF1;
        public static BOM UTFEBCDIC;
        public static BOM SCSU;
        public static BOM SCSU_BOCU1;
        public static BOM SCSU_GB18030;

        static BOM()
        {
            if (mInit)
                return;
            mInit = true;

            UTF8 = new BOM(new byte[] { 0xEF, 0xBB, 0xBF }, "UTF8");
            UTF16BE = new BOM(new byte[] { 0xFE, 0xFF }, "UTF16BE");

            // Windows default UTF16LEWindows")
            UTF16LE = new BOM(new byte[] { 0xFF, 0xFE }, "UTF16LE");

            UTF32BE = new BOM(new byte[] { 0x0, 0x0, 0xFE, 0xFF }, "UTF32BE");
            UTF32LE = new BOM(new byte[] { 0xFF, 0xFE, 0x0, 0x0 }, "UTF32LE");
            UTF7a = new BOM(new byte[] { 0x2B, 0x2F, 0x76, 0x38 }, "UTF7a");
            UTF7b = new BOM(new byte[] { 0x2B, 0x2F, 0x76, 0x39 }, "UTF7b");
            UTF7c = new BOM(new byte[] { 0x2B, 0x2F, 0x76, 0x2B }, "UTF7c");
            UTF7d = new BOM(new byte[] { 0x2B, 0x2F, 0x76, 0x2F }, "UTF7d");
            UTF7e = new BOM(new byte[] { 0x2B, 0x2F, 0x76, 0x38, 0x2D }, "UTF7e");
            UTF1 = new BOM(new byte[] { 0xF7, 0x64, 0x4 }, "UTF1");
            UTFEBCDIC = new BOM(new byte[] { 0xDD, 0x73, 0x66, 0x73 }, "UTFEBCDIC");
            SCSU = new BOM(new byte[] { 0xE, 0xFE, 0xFF }, "SCSU");
            SCSU_BOCU1 = new BOM(new byte[] { 0xFB, 0xEE, 0x28 }, "SCSU_BOCU1");
            SCSU_GB18030 = new BOM(new byte[] { 0x84, 0x31, 0x95, 0x33 }, "SCSU_GB18030");
        }

        public BOM(BOM bom)
        {
            this._BOM = new byte[bom._BOM.Length - 1 + 1];
            Array.Copy(bom._BOM, _BOM, _BOM.Length);

            this._Type = bom._Type;
            myInit = true;
        }

        private BOM(byte[] bytes, string typeName)
        {
            _BOM = new byte[bytes.Length - 1 + 1];
            bytes.CopyTo(_BOM, 0);
            Enum.TryParse<BOMTYPE>(typeName, out _Type);
            myInit = true;
        }

        private BOM(byte[] bytes, BOMTYPE type)
        {
            _BOM = new byte[bytes.Length - 1 + 1];
            bytes.CopyTo(_BOM, 0);
            _Type = type;
            myInit = true;
        }

        public BOMTYPE Type
        {
            get
            {
                if (!myInit)
                {
                    _Type = BOMTYPE.UTF16LE;
                    _BOM = UTF16LE;
                    myInit = true;
                }
                return _Type;
            }
            set
            {
                SetBOM(value);
            }
        }

        public byte[] Bytes
        {
            get
            {
                if (!myInit)
                {
                    _Type = BOMTYPE.UTF16LE;
                    _BOM = UTF16LE;
                    myInit = true;
                }
                return _BOM;
            }
        }

        public void SetBOM(BOMTYPE type)
        {
            if (!myInit)
                myInit = true;
            _BOM = null;
            _Type = type;
            if (type == BOMTYPE.ASCII)
                return;
            _BOM = GetBOM(type);
        }

        public static bool ByteEquals(byte[] operand1, BOM operand2)
        {
            int i;
            int c = operand2._BOM.Length - 1;

            if (operand1.Length < operand2._BOM.Length)
                return false;

            for (i = 0; i <= c; i++)
            {
                if (operand1[i] != operand2._BOM[i])
                    return false;
            }

            return true;
        }

        public static byte[] StripBOM(byte[] value)
        {
            BOM a = new BOM();
            return StripBOM(value, ref a);
        }

        public static byte[] StripBOM(byte[] value, ref BOM BOM)
        {
            BOM b = Parse(value);
            byte[] retVal; 

            if (b._BOM != null && b._BOM.Length > 0)
            {
                byte[] by;
                int c = value.Length;

                c -= b._BOM.Length;
                by = new byte[c - 1 + 1];

                Array.Copy(value, b._BOM.Length, by, 0, c);
                retVal = by;
            }
            else
            {
                BOM.Type = BOMTYPE.ASCII;
                retVal = value;
            }

            BOM._BOM = new byte[b._BOM.Length - 1 + 1];
            b._BOM.CopyTo(BOM._BOM, 0);
            BOM._Type = b.Type;

            return retVal;
        }

        public static byte[] GetBOM(string type)
        {
            BOM b = new BOM();

            b._Type = (BOMTYPE)System.Enum.Parse(typeof(BOMTYPE), type, false);
            return GetBOM(b._Type);
        }

        public static byte[] GetBOM(BOMTYPE type)
        {
            System.Reflection.MemberInfo[] m;
            System.Reflection.FieldInfo fi;
            byte[] by = null;
            BOM b = new BOM();

            m = typeof(BOM).GetMembers();

            foreach (var mt in m)
            {
                if (mt.MemberType == System.Reflection.MemberTypes.Field)
                {
                    if (mt.Name == type.ToString())
                    {
                        fi = (System.Reflection.FieldInfo)mt;
                        b = (BOM)fi.GetValue(b);
                        by = b._BOM;

                        break;
                    }
                }
            }

            return by;
        }

        public override string ToString()
        {
            int i;
            int c;

            string s = "";

            c = _BOM.Length - 1;
            for (i = 0; i <= c; i++)
            {
                if (i != 0)
                    s += ", ";
                s += TextTools.PadHex(_BOM[i], 2, "0x", true);
            }

            return Type.ToString() + " [" + s + "]";
        }

        public byte[] Encode(byte[] value)
        {
            byte[] b;

            byte[] d;

            b = new byte[_BOM.Length + 1];
            Array.Copy(_BOM, b, _BOM.Length);

            switch (_Type)
            {
                case BOMTYPE.ASCII:
                    {
                        d = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, value);
                        break;
                    }

                case BOMTYPE.UTF16BE:
                    {
                        d = Encoding.Convert(Encoding.Unicode, Encoding.BigEndianUnicode, value);
                        break;
                    }

                case BOMTYPE.UTF16LE:
                    {
                        d = value;
                        break;
                    }

                default:
                    {
                        d = value;
                        break;
                    }
            }

            b = new byte[_BOM.Length + 1];
            Array.Copy(_BOM, b, _BOM.Length);

            if (value == null || value.Length == 0)
                return b;
            b.Concat(d);

            return b;
        }

        public byte[] Encode(string value)
        {
            return Encode(Encoding.Unicode.GetBytes(value));
        }

        public static BOM Parse(byte[] value)
        {
            BOM b;
            System.Reflection.MemberInfo[] m;
            System.Reflection.FieldInfo fi;
            byte[] by;
            int i;
            int c;

            bool y = false;

            b = new BOM();
            b.Type = BOMTYPE.ASCII;

            m = typeof(BOM).GetMembers();

            foreach (var mt in m)
            {
                if (mt.MemberType == System.Reflection.MemberTypes.Field)
                {
                    if (mt.Name != "BOM" & mt.Name != "Type")
                    {
                        fi = (System.Reflection.FieldInfo)mt;
                        if (fi.FieldType != typeof(BOM))
                            continue;

                        by = ((BOM)fi.GetValue(b))._BOM;

                        if (value.Length < by.Length)
                            continue;
                        c = by.Length - 1;

                        y = true;
                        for (i = 0; i <= c; i++)
                        {
                            if (value[i] != by[i])
                            {
                                y = false;
                                break;
                            }
                        }

                        if (y)
                        {
                            if (b._Type == BOMTYPE.ASCII || by.Length > b._BOM.Length)
                            {
                                b._Type = (BOMTYPE)System.Enum.Parse(typeof(BOMTYPE), fi.Name, false);
                                if (b._BOM != null && b._BOM.Length > 0)
                                    b._BOM = null;
                                b._BOM = new byte[by.Length - 1 + 1];
                                Array.Copy(by, b._BOM, by.Length);
                            }

                            y = false;
                        }
                    }
                }
            }

            return b;
        }

        public static implicit operator string(BOM operand)
        {
            return Encoding.ASCII.GetString(operand._BOM);
        }

        public static explicit operator BOM(string operand)
        {
            BOM b = Parse(Encoding.ASCII.GetBytes(operand));
            return b;
        }

        public static implicit operator byte[](BOM operand)
        {
            byte[] b;
            b = new byte[operand._BOM.Length - 1 + 1];
            operand._BOM.CopyTo(b, 0);
            return b;
        }

        public static explicit operator BOM(byte[] operand)
        {
            BOM b = Parse((byte[])operand);
            return b;
        }

        public static implicit operator int[](BOM operand)
        {
            int[] b = new int[operand._BOM.Length];
            for (var i = 0; i < operand._BOM.Length; i++)
            {
                b[i] = (int)operand._BOM[i];
            }

            return b;
        }

        public static explicit operator BOM(int[] operand)
        {
            byte[] b = new byte[operand.Length];

            for (var i = 0; i < operand.Length; i++)
            {
                b[i] = (byte)operand[i];
            }

            return new BOM((BOM)b);
        }

        public static bool operator ==(BOM operand1, BOM operand2)
        {
            return operand1.Equals(operand2);
        }

        public static bool operator !=(BOM operand1, BOM operand2)
        {
            return !operand1.Equals(operand2);
        }

        public static bool operator ==(BOM operand1, byte[] operand2)
        {
            return ByteEquals(operand2, operand1);
        }

        public static bool operator !=(BOM operand1, byte[] operand2)
        {
            return !ByteEquals(operand2, operand1);
        }

        public static bool operator ==(byte[] operand1, BOM operand2)
        {
            return ByteEquals(operand1, operand2);
        }

        public static bool operator !=(byte[] operand1, BOM operand2)
        {
            return !ByteEquals(operand1, operand2);
        }

        public override bool Equals(object obj)
        {
            if (obj is BOM b)
            {
                return _BOM.SequenceEqual(b._BOM);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return (int)Crc32.Calculate(_BOM);
        }
    }


    public class SignalSorter : IComparer<SignalElement>
    {
        public int Compare(SignalElement x, SignalElement y)
        {
            SignalElement s1 = (SignalElement)x;
            SignalElement s2 = (SignalElement)y;

            return (s1.Code - s2.Code);
        }
    }

    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ControlSignals : List<SignalElement>
    {

        public new void Sort()
        {
            Sort(new SignalSorter());
        }

        public void AddSignal(string str)
        {
            SignalElement cc = SignalElement.Parse(str);
            Add(cc);
        }

        public SignalElement[] InnerArray
        {
            get
            {
                return ToArray();
            }
        }

        public void AddRange(SignalElement[] sel)
        {
            base.AddRange(sel);
        }

        public void InsertRange(int index, SignalElement[] sel)
        {
            base.InsertRange(index, sel);
        }

        public SignalElement this[ControlCodes index]
        {
            get
            {
                foreach (SignalElement cc in this)
                {
                    if (cc.Code == index)
                        return cc;
                }
                return default(SignalElement);
            }
            set
            {
                this[index] = value;
            }
        }

        public new SignalElement this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = value;
            }
        }

        public new int IndexOf(SignalElement value)
        {
            return base.IndexOf(value);
        } // IndexOf(value)

        public new bool Contains(SignalElement value)
        {
            // If value is not of type SignalElement, this will return false.
            return base.Contains(value);
        } // Contains

        public ControlSignals()
        {
        }
    }


    public class CPSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            CodePageElement cp1 = (CodePageElement)x;
            CodePageElement cp2 = (CodePageElement)y;

            return ((int)cp1.Code - (int)cp2.Code);
        }
    }
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CodePage : CollectionBase
    {
        private string _Name;
        private char[] mCA;
       
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        public void Sort()
        {

            InnerList.Sort(new CPSorter());
        }

        public void AddCode(string code)
        {
            CodePageElement cp = CodePageElement.Parse(code);
            this.Add(cp);
            mCA = null;
        }

        public char[] CodeArray
        {
            get
            {
                if (mCA == null)
                    mCA = GetCodeArray();
                return mCA;
            }
        }

        public string ToUnicodeString(byte[] subject)
        {
            if (mCA == null)
                mCA = GetCodeArray();
            int i;
            int c = subject.Length - 1;

            char[] ch;

            ch = new char[c + 1];

            for (i = 0; i <= c; i++)
                ch[i] = mCA[subject[i]];

            return ch.ToString();
        }


        public byte[] FromUnicodeString(string subject)
        {
            if (mCA == null)
                mCA = GetCodeArray();
            int i;
            int c = subject.Length - 1;

            byte[] ch;

            ch = new byte[c + 1];

            for (i = 0; i <= c; i++)
                ch[i] = (byte)Array.IndexOf(mCA, subject[i]);

            return ch;
        }

        public char[] GetCodeArray()
        {
            Sort();
            CodePageElement objCP;
            char[] codes;
            int i;

            objCP = (CodePageElement)List[List.Count - 1];
            i = (int)objCP.Code;
            codes = new char[i - 1 + 1];

            foreach (CodePageElement objC in List)
                codes[objC.Code] = objC.Unicode;

            return codes;
        }

        public CodePageElement[] InnerArray
        {
            get
            {
                return (CodePageElement[])InnerList.ToArray(typeof(CodePageElement));
            }
        }

        public void AddRange(CodePageElement[] sel)
        {
            InnerList.AddRange(sel);
        }

        public void InsertRange(int index, CodePageElement[] sel)
        {
            InnerList.InsertRange(index, sel);
        }

        public CodePageElement this[int index]
        {
            get
            {
                return (CodePageElement)List[index];
            }
            set
            {
                List[index] = (CodePageElement)value;
            }
        }

        public int IndexOf(CodePageElement value)
        {
            return List.IndexOf(value);
        } // IndexOf(value)

        public void Insert(int index, CodePageElement value)
        {
            List.Insert(index, value);
        } // Insert

        public int Add(CodePageElement value)
        {
            return List.Add(value);
        }

        public new void RemoveAt(int index)
        {
            if (List.Count == 0)
                return;
            List.RemoveAt(index);
        }

        public void Remove(CodePageElement value)
        {
            List.Remove(value);
        } // Remove

        public bool Contains(CodePageElement value)
        {
            // If value is not of type CodePageElement, this will return false.
            return List.Contains(value);
        } // Contains

        protected override void OnInsert(int index, object value)
        {
        } // OnInsert

        protected override void OnRemove(int index, object value)
        {
        } // OnRemove

        protected override void OnSet(int index, object oldValue, object newValue)
        {
        } // OnSet

        protected override void OnValidate(object value)
        {
            if (!typeof(CodePageElement).IsAssignableFrom(value.GetType()))
                throw new ArgumentException("value must be of type Object.", "value");
        } // OnValidate 

        public CodePage() : base()
        {
        }
    }




    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CodePageCollection : CollectionBase
    {
        public CodePage[] InnerArray
        {
            get
            {
                return (CodePage[])InnerList.ToArray(typeof(CodePage));
            }
        }

        public void AddRange(CodePage[] sel)
        {
            InnerList.AddRange(sel);
        }

        public int Add(CodePage value)
        {
            return List.Add(value);
        }

        public void InsertRange(int index, CodePage[] sel)
        {
            InnerList.InsertRange(index, sel);
        }

        [Browsable(true)]
        public CodePage this[int index]
        {
            get
            {
                return (CodePage)List[index];
            }
            set
            {
                List[index] = (CodePage)value;
            }
        }

        [Browsable(true)]
        public CodePage this[string name]
        {
            get
            {
                name = name.ToLower();
                foreach (CodePage cp in List)
                {
                    if (cp.Name.ToLower() == name)
                        return cp;
                }

                return null;
            }
            set
            {
                int c = 0;
                foreach (CodePage cp in List)
                {
                    if (cp.Name.ToLower() == name)
                    {
                        List[c] = value;
                        break;
                    }
                    c += 1;
                }
            }
        }

        public int IndexOf(CodePage value)
        {
            return List.IndexOf(value);
        } // IndexOf(value)

        public void Insert(int index, CodePage value, string key = "")
        {
            List.Insert(index, value);
        } // Insert

        public new void RemoveAt(int index)
        {
            if (List.Count == 0)
                return;
            List.RemoveAt(index);
        }

        public void Remove(CodePage value)
        {
            List.Remove(value);
        } // Remove

        public bool Contains(CodePage value)
        {
            // If value is not of type CodePage, this will return false.
            return List.Contains(value);
        } // Contains

        protected override void OnInsert(int index, object value)
        {
        } // OnInsert

        protected override void OnRemove(int index, object value)
        {
        } // OnRemove

        protected override void OnSet(int index, object oldValue, object newValue)
        {
        } // OnSet

        protected override void OnValidate(object value)
        {
            if (!typeof(CodePage).IsAssignableFrom(value.GetType()))
                throw new ArgumentException("value must be of type Object.", "value");
        } // OnValidate 

        internal CodePageCollection() : base()
        {
        }
    }

    public static class CPGlobal
    {
        public static CodePageCollection CodePages { get; internal set; } 
        public static ControlSignals Signals { get; internal set; } 

        static CPGlobal()
        {
            Signals = new ControlSignals();
            InitControlSignalCatalog();

            CodePages = new CodePageCollection();
            InitEBCDIC();

        }

        public static string SafeTextRead(byte[] b)
        {
            BOM bm = new BOM();
            string s;

            byte[] bOut = BOM.StripBOM(b, ref bm);

            switch (bm.Type)
            {
                case BOMTYPE.UTF16LE:
                    {
                        s = Encoding.Unicode.GetString(bOut);
                        break;
                    }

                case BOMTYPE.UTF16BE:
                    {
                        s = Encoding.BigEndianUnicode.GetString(bOut);
                        break;
                    }

                case BOMTYPE.UTF8:
                    {
                        s = Encoding.UTF8.GetString(bOut);
                        break;
                    }

                case BOMTYPE.UTF7a:
                case BOMTYPE.UTF7b:
                case BOMTYPE.UTF7c:
                case BOMTYPE.UTF7d:
                case BOMTYPE.UTF7e:
                    {

                        /* TODO ERROR: Skipped WarningDirectiveTrivia */
#pragma warning disable SYSLIB0001 // Type or member is obsolete
                        s = Encoding.UTF7.GetString(bOut);
#pragma warning restore SYSLIB0001 // Type or member is obsolete
                        break;
                    }

                case BOMTYPE.UTF32LE:
                    {
                        s = Encoding.UTF32.GetString(bOut);
                        break;
                    }

                case BOMTYPE.UTF32BE:
                    {
                        UTF32Encoding nenc = new UTF32Encoding(true, false);

                        s = nenc.GetString(bOut);
                        break;
                    }

                default:
                    {
                        s = Encoding.UTF8.GetString(bOut);
                        break;
                    }
            }

            return s;
        }

        public static byte[] SafeTextWrite(string str, BOMTYPE enc = BOMTYPE.UTF16LE)
        {
            BOM bm = new BOM();
            SafePtr bl;

            bm.Type = enc;

            byte[] bOut;

            switch (bm.Type)
            {
                case BOMTYPE.UTF16LE:
                        bm = BOM.UTF16LE;
                        bOut = Encoding.Unicode.GetBytes(str);
                        break;

                case BOMTYPE.UTF16BE:
                        bm = BOM.UTF16BE;
                        bOut = Encoding.BigEndianUnicode.GetBytes(str);
                        break;

                case BOMTYPE.UTF8:
                        bm = BOM.UTF8;
                        bOut = Encoding.UTF8.GetBytes(str);
                        break;

#pragma warning disable SYSLIB0001 // Type or member is obsolete

                case BOMTYPE.UTF7a:
                    bm = BOM.UTF7a;
                    bOut = Encoding.UTF7.GetBytes(str);
                    break;

                case BOMTYPE.UTF7b:
                    bm = BOM.UTF7b;
                    bOut = Encoding.UTF7.GetBytes(str);
                    break;

                case BOMTYPE.UTF7c:
                    bm = BOM.UTF7c;
                    bOut = Encoding.UTF7.GetBytes(str);
                    break;

                case BOMTYPE.UTF7d:
                    bm = BOM.UTF7d;
                    bOut = Encoding.UTF7.GetBytes(str);
                    break;

                case BOMTYPE.UTF7e:
                        bm = BOM.UTF7e;
                        bOut = Encoding.UTF7.GetBytes(str);
                        break;

#pragma warning restore SYSLIB0001 // Type or member is obsolete


                case BOMTYPE.UTF32LE:
                    bm = BOM.UTF32LE;
                        bOut = Encoding.UTF32.GetBytes(str);
                        break;

                case BOMTYPE.UTF32BE:
                    bm = BOM.UTF32BE;
                    bOut = Encoding.UTF32.GetBytes(str);
                    Array.Reverse(bOut);

                    break;

                default:
                    bm = BOM.UTF8;
                    bOut = Encoding.UTF8.GetBytes(str);
                    break;
            }

            bl = (SafePtr)bm._BOM;

            bl += bOut;
            bOut = (byte[])bl;

            bl.Dispose();

            return bOut;
        }



        internal static void InitControlSignalCatalog()
        {

            // Signals.AddSignal("C0")
            // Signals.AddSignal("Seq	Dec	Hex	Acro	Symb	Name	C	Description")
            Signals.AddSignal(@"^@	00	00	NUL	␀	Null	\0	Originally used to allow gaps to be left on paper tape for edits. Later used for padding after a code that might take a terminal some time to process (e.g. a carriage return or line feed on a printing terminal). Now often used as a string terminator, especially in the C programming language.");
            Signals.AddSignal("^A	01	01	SOH	␁	Start of Heading		First character of a message header.");
            Signals.AddSignal("^B	02	02	STX	␂	Start of text		First character of message text, and may be used to terminate the message heading.");
            Signals.AddSignal("^C	03	03	ETX	␃	End of Text		Often used as a \"break\" character (Ctrl-C) to interrupt or terminate a program or process.");
            Signals.AddSignal("^D	04	04	EOT	␄	End of Transmission		Used on Unix to signal end-of-file condition on, or to logout from a terminal.");
            Signals.AddSignal("^E	05	05	ENQ	␅	Enquiry		Signal intended to trigger a response at the receiving end, to see if it is still present.");
            Signals.AddSignal("^F	06	06	ACK	␆	Acknowledge		Response to an ENQ, or an indication of successful receipt of a message.");
            Signals.AddSignal(@"^G	07	07	BEL	␇	Bell	\a	Originally used to sound a bell on the terminal. Later used for a beep on systems that didn't have a physical bell. May also quickly turn on and off inverse video (a visual bell).");
            Signals.AddSignal(@"^H	08	08	BS	␈	Backspace	\b	Move the cursor one position leftwards. On input, this may delete the character to the left of the cursor. On output, where in early computer technology a character once printed could not be erased, the backspace was sometimes used to generate accented characters in ASCII. For example, à could be produced using the three character sequence a BS ` (0x61 0x08 0x60). This usage is now deprecated and generally not supported. To provide disambiguation between the two potential uses of backspace, the cancel character control code was made part of the standard C1 control set.");
            Signals.AddSignal(@"^I	09	09	HT	␉	Character Tabulation, Horizontal Tabulation	\t	Position to the next character tab stop.");
            Signals.AddSignal(@"^J	10	0A	LF	␊	Line Feed	\n	On typewriters, printers, and some terminal emulators, moves the cursor down one row without affecting its column position. On Unix, used to mark end-of-line. In MS-DOS, Windows, and various network standards, LF is used following CR as part of the end-of-line mark.");
            Signals.AddSignal(@"^K	11	0B	VT	␋	Line Tabulation, Vertical Tabulation	\v	Position the form at the next line tab stop.");
            Signals.AddSignal(@"^L	12	0C	FF	␌	Form Feed	\f	On printers, load the next page. Treated as whitespace in many programming languages, and may be used to separate logical divisions in code. In some terminal emulators, it clears the screen.");
            Signals.AddSignal(@"^M	13	0D	CR	␍	Carriage Return	\r	Originally used to move the cursor to column zero while staying on the same line. On Mac OS (pre-Mac OS X), as well as in earlier systems such as the Apple II and Commodore 64, used to mark end-of-line. In MS-DOS, Windows, and various network standards, it is used preceding LF as part of the end-of-line mark. The Enter or Return key on a keyboard will send this character, but it may be converted to a different end-of-line sequence by a terminal program.");
            Signals.AddSignal("^N	14	0E	SO	␎	Shift Out		Switch to an alternate character set.");
            Signals.AddSignal("^O	15	0F	SI	␏	Shift In		Return to regular character set after Shift Out.");
            Signals.AddSignal("^P	16	10	DLE	␐	Data Link Escape		Cause the following octets to be interpreted as raw data, not as control codes or graphic characters. Returning to normal usage would be implementation dependent.");
            Signals.AddSignal("^Q	17	11	DC1	␑	Device Control One (XON)		These four control codes are reserved for device control, with the interpretation dependent upon the device they were connected. DC1 and DC2 were intended primarily to indicate activating a device while DC3 and DC4 were intended primarily to indicate pausing or turning off a device. In actual practice DC1 and DC3 (known also as XON and XOFF respectively in this usage) quickly became the de facto standard for software flow control.");
            Signals.AddSignal("^R	18	12	DC2	␒	Device Control Two		 ");
            Signals.AddSignal("^S	19	13	DC3	␓	Device Control Three (XOFF)		 ");
            Signals.AddSignal("^T	20	14	DC4	␔	Device Control Four		 ");
            Signals.AddSignal("^U	21	15	NAK	␕	Negative Acknowledge		Sent by a station as a negative response to the station with which the connection has been set up. In binary synchronous communication protocol, the NAK is used to indicate that an error was detected in the previously received block and that the receiver is ready to accept retransmission of that block. In multipoint systems, the NAK is used as the not-ready reply to a poll.");
            Signals.AddSignal("^V	22	16	SYN	␖	Synchronous Idle		Used in synchronous transmission systems to provide a signal from which synchronous correction may be achieved between data terminal equipment, particularly when no other character is being transmitted.");
            Signals.AddSignal("^W	23	17	ETB	␗	End of Transmission Block		Indicates the end of a transmission block of data when data are divided into such blocks for transmission purposes.");
            Signals.AddSignal("^X	24	18	CAN	␘	Cancel		Indicates that the data preceding it are in error or are to be disregarded.");
            Signals.AddSignal("^Y	25	19	EM	␙	End of medium		Intended as means of indicating on paper or magnetic tapes that the end of the usable portion of the tape had been reached.");
            Signals.AddSignal("^Z	26	1A	SUB	␚	Substitute		Originally intended for use as a transmission control character to indicate that garbled or invalid characters had been received. It has often been put to use for other purposes when the in-band signaling of errors it provides is unneeded, especially where robust methods of error detection and correction are used, or where errors are expected to be rare enough to make using the character for other purposes advisable.");
            Signals.AddSignal("^[	27	1B	ESC	␛	Escape		The Esc key on the keyboard will cause this character to be sent on most systems. It can be used in software user interfaces to exit from a screen, menu, or mode, or in device-control protocols (e.g., printers and terminals) to signal that what follows is a special command sequence rather than normal text. In systems based on ISO/IEC 2022, even if another set of C0 control codes are used, this octet is required to always represent the escape character.");
            Signals.AddSignal(@"^\	28	1C	FS	␜	File Separator		Can be used as delimiters to mark fields of data structures. If used for hierarchical levels, US is the lowest level (dividing plain-text data items), while RS, GS, and FS are of increasing level to divide groups made up of items of the level beneath it.");
            Signals.AddSignal("^]	29	1D	GS	␝	Group separator		 ");
            Signals.AddSignal("^^	30	1E	RS	␞	Record Separator		 ");
            Signals.AddSignal(@"^_	31	1F	US	␟	Unit separator	\nWhile not technically part of the C0 control character range, the following two characters are defined in ISO/IEC 2022 as always being available regardless of which sets of control characters and graphics characters have been registered. They can be thought of as having some characteristics of control characters.");
            Signals.AddSignal(" 	32	20	SP	␠	Space		Space is a graphic character. It has a visual representation consisting of the absence of a graphic symbol. It causes the active position to be advanced by one character position. In some applications, Space can be considered a lowest-level \"word separator\" to be used with the adjacent separator characters.");
            Signals.AddSignal("^?	127	7F	DEL	␡	Delete		Not technically part of the C0 control character range, this was originally used to mark deleted characters on paper tape, since any character could be changed to all ones by punching holes everywhere. On VT100 compatible terminals, this is the character generated by the key labelled ⌫, usually called backspace on modern machines, and does not correspond to the PC delete key.");
            // Signals.AddSignal("C1")
            // Signals.AddSignal("Seq	Dec	Hex	Acro	Name	Description")
            Signals.AddSignal("@	128	80	PAD	Padding Character	Listed with no name in Unicode. Not part of ISO/IEC 6429 (ECMA-48).");
            Signals.AddSignal("A	129	81	HOP	High Octet Preset");
            Signals.AddSignal("B	130	82	BPH	Break Permitted Here	Follows a graphic character where a line break is permitted. Roughly equivalent to a soft hyphen except that the means for indicating a line break is not necessarily a hyphen. Not part of the first edition of ISO/IEC 6429.[1]");
            Signals.AddSignal("C	131	83	NBH	No Break Here	Follows the graphic character that is not to be broken. Not part of the first edition of ISO/IEC 6429.[1]");
            Signals.AddSignal("D	132	84	IND	Index	Move the active position one line down, to eliminate ambiguity about the meaning of LF. Deprecated in 1988 and withdrawn in 1992 from ISO/IEC 6429 (1986 and 1991 respectively for ECMA-48).");
            Signals.AddSignal("E	133	85	NEL	Next Line	Equivalent to CR+LF. Used to mark end-of-line on some IBM mainframes.");
            Signals.AddSignal("F	134	86	SSA	Start of Selected Area	Used by block-oriented terminals.");
            Signals.AddSignal("G	135	87	ESA	End of Selected Area");
            Signals.AddSignal("H	136	88	HTS	Character Tabulation Set / Horizontal Tabulation Set	Causes a character tabulation stop to be set at the active position.");
            Signals.AddSignal("I	137	89	HTJ	Character Tabulation With Justification / Horizontal Tabulation With Justification	Similar to Character Tabulation, except that instead of spaces or lines being placed after the preceding characters until the next tab stop is reached, the spaces or lines are placed preceding the active field so that preceding graphic character is placed just before the next tab stop.");
            Signals.AddSignal("J	138	8A	VTS	Line Tabulation Set / Vertical Tabulation Set	Causes a line tabulation stop to be set at the active position.");
            Signals.AddSignal("K	139	8B	PLD	Partial Line Forward / Partial Line Down	Used to produce subscripts and superscripts in ISO/IEC 6429, e.g., in a printer. Subscripts use PLD text PLU while superscripts use PLU text PLD..");
            Signals.AddSignal("L	140	8C	PLU	Partial Line Backward / Partial Line Up");
            Signals.AddSignal("M	141	8D	RI	Reverse Line Feed / Reverse Index	");
            Signals.AddSignal("N	142	8E	SS2	Single-Shift 2	Next character invokes a graphic character from the G2 or G3 graphic sets respectively. In systems that conform to ISO/IEC 4873 (ECMA-43), even if a C1 set other than the default is used, these two octets may only be used for this purpose.");
            Signals.AddSignal("O	143	8F	SS3	Single-Shift 3");
            Signals.AddSignal("P	144	90	DCS	Device Control String	Followed by a string of printable characters (0x20 through 0x7E) and format effectors (0x08 through 0x0D), terminated by ST (0x9C).");
            Signals.AddSignal("Q	145	91	PU1	Private Use 1	Reserved for a function without standardized meaning for private use as required, str to the prior agreement of the sender and the recipient of the data.");
            Signals.AddSignal("R	146	92	PU2	Private Use 2");
            Signals.AddSignal("S	147	93	STS	Set Transmit State	");
            Signals.AddSignal("T	148	94	CCH	Cancel character	Destructive backspace, intended to eliminate ambiguity about meaning of BS.");
            Signals.AddSignal("U	149	95	MW	Message Waiting	");
            Signals.AddSignal("V	150	96	SPA	Start of Protected Area	Used by block-oriented terminals.");
            Signals.AddSignal("W	151	97	EPA	End of Protected Area");
            Signals.AddSignal("X	152	98	SOS	Start of String	Followed by a control string terminated by ST (0x9C) that may contain any character except SOS or ST. Not part of the first edition of ISO/IEC 6429.[1]");
            Signals.AddSignal("Y	153	99	SGCI	Single Graphic Character Introducer	Listed with no name in Unicode. Not part of ISO/IEC 6429.");
            Signals.AddSignal("Z	154	9A	SCI	Single Character Introducer	To be followed by a single printable character (0x20 through 0x7E) or format effector (0x08 through 0x0D). The intent was to provide a means by which a control function or a graphic character that would be available regardless of which graphic or control sets were in use could be defined. Definitions of what the following byte would invoke was never implemented in an international standard. Not part of the first edition of ISO/IEC 6429.[1]");
            Signals.AddSignal("[	155	9B	CSI	Control Sequence Introducer	Used to introduce control sequences that take parameters.");
            Signals.AddSignal(@"\	156	9C	ST	String Terminator	");
            Signals.AddSignal("]	157	9D	OSC	Operating System Command	Followed by a string of printable characters (0x20 through 0x7E) and format effectors (0x08 through 0x0D), terminated by ST (0x9C). These three control codes were intended for use to allow in-band signaling of protocol information, but are rarely used for that purpose.");
            Signals.AddSignal("^	158	9E	PM	Privacy Message");
            Signals.AddSignal("_	159	9F	APC	Application Program Command");
            Signals.AddSignal("_	225	E1	NSP	A figure space is a typographic unit equal to the size of a single typographic figure (numeral or letter), minus leading. Its size can fluctuate somewhat depending on which font is being used. In fonts with monospaced digits, it is equal to the width of one digit.");
            Signals.AddSignal("^	255	FF	EO	Eighty-Ones");
        }

        public static void InitEBCDIC()
        {
            CodePage objCP = new CodePage();

            objCP.AddCode("Control|0000|Null character,NUL|0");
            objCP.AddCode("Control|0001|Start of heading,SOH|1");
            objCP.AddCode("Control|0002|Start of text,STX|2");
            objCP.AddCode("Control|0003|End of text,ETX|3");
            objCP.AddCode("Control|    |SEL|4");
            objCP.AddCode("Control|0009|Horizontal tab,HT|5");
            objCP.AddCode("Control|    |RNL|6");
            objCP.AddCode("Control|007F|Delete character,DEL|7");
            objCP.AddCode("Control|    |GE|8");
            objCP.AddCode("Control|    |SPS|9");
            objCP.AddCode("Control|    |RPT|10");
            objCP.AddCode("Control|000B|Vertical tab,VT|11");
            objCP.AddCode("Control|000C|Form feed,FF|12");
            objCP.AddCode("Control|000D|Carriage return,CR|13");
            objCP.AddCode("Control|000E|Shift out,SO|14");
            objCP.AddCode("Control|000F|Shift in,SI|15");
            objCP.AddCode("Control|0010|Data Link Escape,DLE|16");
            objCP.AddCode("Control|0011|Device Control 1,DC1|17");
            objCP.AddCode("Control|0012|Device Control 2,DC2|18");
            objCP.AddCode("Control|0013|Device Control 3,DC3|19");
            objCP.AddCode("Control|    |RES ENP|20");
            objCP.AddCode("Control|0085|Newline,NL|21");
            objCP.AddCode("Control|0008|Backspace,BS|22");
            objCP.AddCode("Control|    |POC|23");
            objCP.AddCode("Control|0018|Cancel character,CAN|24");
            objCP.AddCode("Control|0019|End of medium,EM|25");
            objCP.AddCode("Control|    |UBS|26");
            objCP.AddCode("Control|    |CU1|27");
            objCP.AddCode("Control|001C|Field separator,IFS|28");
            objCP.AddCode("Control|001D|Group separator,IGS|29");
            objCP.AddCode("Control|001E|Record separator,IRS|30");
            objCP.AddCode("Control|001F|Unit separator,IUS ITB|31");
            objCP.AddCode("Control|    |DS|32");
            objCP.AddCode("Control|    |SOS|33");
            objCP.AddCode("Control|    |FS|34");
            objCP.AddCode("Control|    |WUS|35");
            objCP.AddCode("Control|    |BYP INP|36");
            objCP.AddCode("Control|000A|Line feed,LF|37");
            objCP.AddCode("Control|0017|End transmission block,ETB|38");
            objCP.AddCode("Control|001B|Escape character,ESC|39");
            objCP.AddCode("Control|    |SA|40");
            objCP.AddCode("Control|    |SFE|41");
            objCP.AddCode("Control|    |SM SW|42");
            objCP.AddCode("Control|    |CSP|43");
            objCP.AddCode("Control|    |MFA|44");
            objCP.AddCode("Control|0005|Enquire character,ENQ|45");
            objCP.AddCode("Control|0006|Acknowledge character,ACK|46");
            objCP.AddCode("Control|0007|Bell character,BEL|47");
            objCP.AddCode("Control|    |48");
            objCP.AddCode("Control|    |49");
            objCP.AddCode("Control|0016|Synchronous idle,SYN|50");
            objCP.AddCode("Control|    |IR|51");
            objCP.AddCode("Control|    |PP|52");
            objCP.AddCode("Control|    |TRN|53");
            objCP.AddCode("Control|    |NBS|54");
            objCP.AddCode("Control|0004|End-of-transmission character,EOT|55");
            objCP.AddCode("Control|    |SBS|56");
            objCP.AddCode("Control|    |IT|57");
            objCP.AddCode("Control|    |RFF|58");
            objCP.AddCode("Control|    |CU3|59");
            objCP.AddCode("Control|0014|Device control 4,DC4|60");
            objCP.AddCode("Control|0015|Negative-acknowledge character,NAK|61");
            objCP.AddCode("Control|    |62");
            objCP.AddCode("Control|001A|Substitute character,SUB|63");
            objCP.AddCode("Punctuation|0020|Space character,SP|64");
            objCP.AddCode("Punctuation|00A0|Non-breaking space,RSP|65");
            objCP.AddCode("Undefined|    | |66");
            objCP.AddCode("Undefined|    | |67");
            objCP.AddCode("Undefined|    | |68");
            objCP.AddCode("Undefined|    | |69");
            objCP.AddCode("Undefined|    | |70");
            objCP.AddCode("Undefined|    | |71");
            objCP.AddCode("Undefined|    | |72");
            objCP.AddCode("Undefined|    | |73");
            objCP.AddCode("Undefined|    | |74");
            objCP.AddCode("Punctuation|002E|full stop,.|75");
            objCP.AddCode("Punctuation|003C|less-than sign,<|76");
            objCP.AddCode("Punctuation|0028|parenthesis,(|77");
            objCP.AddCode("Punctuation|002B|+|78");
            objCP.AddCode("Symbol|007C|vertical bar,&H7C|79");
            objCP.AddCode("Punctuation|0026|ampersand,&|80");
            objCP.AddCode("Undefined|    | |81");
            objCP.AddCode("Undefined|    | |82");
            objCP.AddCode("Undefined|    | |83");
            objCP.AddCode("Undefined|    | |84");
            objCP.AddCode("Undefined|    | |85");
            objCP.AddCode("Undefined|    | |86");
            objCP.AddCode("Undefined|    | |87");
            objCP.AddCode("Undefined|    | |88");
            objCP.AddCode("Undefined|    | |89");
            objCP.AddCode("Symbol|0021|!|90");
            objCP.AddCode("Symbol|0024|$|91");
            objCP.AddCode("Punctuation|002A|Asterisk,*|92");
            objCP.AddCode("Punctuation|0029|parenthesis,)|93");
            objCP.AddCode("Punctuation|003B| |94");
            objCP.AddCode("Symbol|00AC|¬|95");
            objCP.AddCode("Punctuation|002D|Hyphen-minus,-|96");
            objCP.AddCode("Symbol|002F|/|97");
            objCP.AddCode("Undefined|    | |98");
            objCP.AddCode("Undefined|    | |99");
            objCP.AddCode("Undefined|    | |100");
            objCP.AddCode("Undefined|    | |101");
            objCP.AddCode("Undefined|    | |102");
            objCP.AddCode("Undefined|    | |103");
            objCP.AddCode("Undefined|    | |104");
            objCP.AddCode("Undefined|    | |105");
            objCP.AddCode("Punctuation|00A6|¦|106");
            objCP.AddCode("Punctuation|002C|&H2C|107");
            objCP.AddCode("Punctuation|0025|%|108");
            objCP.AddCode("Punctuation|005F|underscore,_|109");
            objCP.AddCode("Punctuation|003E|greater-than sign,>|110");
            objCP.AddCode("Punctuation|003F|?|111");
            objCP.AddCode("Undefined|    | |112");
            objCP.AddCode("Undefined|    | |113");
            objCP.AddCode("Undefined|    | |114");
            objCP.AddCode("Undefined|    | |115");
            objCP.AddCode("Undefined|    | |116");
            objCP.AddCode("Undefined|    | |117");
            objCP.AddCode("Undefined|    | |118");
            objCP.AddCode("Undefined|    | |119");
            objCP.AddCode("Undefined|    | |120");
            objCP.AddCode("Symbol|0060|`|121");
            objCP.AddCode("Punctuation|003A|colon (Punctuationuation),&H3A|122");
            objCP.AddCode("Symbol|0023|number sign,&H23|123");
            objCP.AddCode("Punctuation|0040|@|124");
            objCP.AddCode("Punctuation|0027|apostrophe,&H27|125");
            objCP.AddCode("Punctuation|003D|equals sign,&H3D|126");
            objCP.AddCode("Punctuation|0022|&H22|127");
            objCP.AddCode("Undefined|    | |128");
            objCP.AddCode("Alpha|0061|a|129");
            objCP.AddCode("Alpha|0062|b|130");
            objCP.AddCode("Alpha|0063|c|131");
            objCP.AddCode("Alpha|0064|d|132");
            objCP.AddCode("Alpha|0065|e|133");
            objCP.AddCode("Alpha|0066|f|134");
            objCP.AddCode("Alpha|0067|g|135");
            objCP.AddCode("Alpha|0068|h|136");
            objCP.AddCode("Alpha|0069|i|137");
            objCP.AddCode("Undefined|    | |138");
            objCP.AddCode("Undefined|    | |139");
            objCP.AddCode("Undefined|    | |140");
            objCP.AddCode("Undefined|    | |141");
            objCP.AddCode("Undefined|    | |142");
            objCP.AddCode("Symbol|00B1|±|143");
            objCP.AddCode("Undefined|    | |144");
            objCP.AddCode("Alpha|006A|j|145");
            objCP.AddCode("Alpha|006B|k|146");
            objCP.AddCode("Alpha|006C|l|147");
            objCP.AddCode("Alpha|006D|m|148");
            objCP.AddCode("Alpha|006E|n|149");
            objCP.AddCode("Alpha|006F|o|150");
            objCP.AddCode("Alpha|0070|p|151");
            objCP.AddCode("Alpha|0071|q|152");
            objCP.AddCode("Alpha|0072|r|153");
            objCP.AddCode("Undefined|    | |154");
            objCP.AddCode("Undefined|    | |155");
            objCP.AddCode("Undefined|    | |156");
            objCP.AddCode("Undefined|    | |157");
            objCP.AddCode("Undefined|    | |158");
            objCP.AddCode("Undefined|    | |159");
            objCP.AddCode("Undefined|    | |160");
            objCP.AddCode("Symbol|007E|~|161");
            objCP.AddCode("Alpha|0073|s|162");
            objCP.AddCode("Alpha|0074|t|163");
            objCP.AddCode("Alpha|0075|u|164");
            objCP.AddCode("Alpha|0076|v|165");
            objCP.AddCode("Alpha|0077|w|166");
            objCP.AddCode("Alpha|0078|x|167");
            objCP.AddCode("Alpha|0079|y|168");
            objCP.AddCode("Alpha|007A|z|169");
            objCP.AddCode("Undefined|    | |170");
            objCP.AddCode("Undefined|    | |171");
            objCP.AddCode("Undefined|    | |172");
            objCP.AddCode("Undefined|    | |173");
            objCP.AddCode("Undefined|    | |174");
            objCP.AddCode("Undefined|    | |175");
            objCP.AddCode("Symbol|005E|^|176");
            objCP.AddCode("Undefined|    | |177");
            objCP.AddCode("Undefined|    | |178");
            objCP.AddCode("Undefined|    | |179");
            objCP.AddCode("Undefined|    | |180");
            objCP.AddCode("Undefined|    | |181");
            objCP.AddCode("Undefined|    | |182");
            objCP.AddCode("Undefined|    | |183");
            objCP.AddCode("Undefined|    | |184");
            objCP.AddCode("Undefined|    | |185");
            objCP.AddCode("Symbol|005B|square brackets,&H5B|186");
            objCP.AddCode("Symbol|005D|square brackets,&H5D|187");
            objCP.AddCode("Undefined|    | |188");
            objCP.AddCode("Undefined|    | |189");
            objCP.AddCode("Undefined|    | |190");
            objCP.AddCode("Undefined|    | |191");
            objCP.AddCode("Symbol|007B|brace (Punctuationuation),&H7B|192");
            objCP.AddCode("Alpha|0041|A|193");
            objCP.AddCode("Alpha|0042|B|194");
            objCP.AddCode("Alpha|0043|C|195");
            objCP.AddCode("Alpha|0044|D|196");
            objCP.AddCode("Alpha|0045|E|197");
            objCP.AddCode("Alpha|0046|F|198");
            objCP.AddCode("Alpha|0047|G|199");
            objCP.AddCode("Alpha|0048|H|200");
            objCP.AddCode("Alpha|0049|I|201");
            objCP.AddCode("Punctuation|00AD|Soft hyphen,SHY|202");
            objCP.AddCode("Undefined|    | |203");
            objCP.AddCode("Undefined|    | |204");
            objCP.AddCode("Undefined|    | |205");
            objCP.AddCode("Undefined|    | |206");
            objCP.AddCode("Undefined|    | |207");
            objCP.AddCode("Symbol|007D|brace (Punctuationuation),&H7D|208");
            objCP.AddCode("Alpha|004A|J|209");
            objCP.AddCode("Alpha|004B|K|210");
            objCP.AddCode("Alpha|004C|L|211");
            objCP.AddCode("Alpha|004D|M|212");
            objCP.AddCode("Alpha|004E|N|213");
            objCP.AddCode("Alpha|004F|O|214");
            objCP.AddCode("Alpha|0050|P|215");
            objCP.AddCode("Alpha|0051|Q|216");
            objCP.AddCode("Alpha|0052|R|217");
            objCP.AddCode("Undefined|    | |218");
            objCP.AddCode("Undefined|    | |219");
            objCP.AddCode("Undefined|    | |220");
            objCP.AddCode("Undefined|    | |221");
            objCP.AddCode("Undefined|    | |222");
            objCP.AddCode("Undefined|    | |223");
            objCP.AddCode(@"Symbol|005C|\|224");
            objCP.AddCode("Symbol|2007|figure space,NSP|225");
            objCP.AddCode("Alpha|0053|S|226");
            objCP.AddCode("Alpha|0054|T|227");
            objCP.AddCode("Alpha|0055|U|228");
            objCP.AddCode("Alpha|0056|V|229");
            objCP.AddCode("Alpha|0057|W|230");
            objCP.AddCode("Alpha|0058|X|231");
            objCP.AddCode("Alpha|0059|Y|232");
            objCP.AddCode("Alpha|005A|Z|233");
            objCP.AddCode("Undefined|    | |234");
            objCP.AddCode("Undefined|    | |235");
            objCP.AddCode("Undefined|    | |236");
            objCP.AddCode("Undefined|    | |237");
            objCP.AddCode("Undefined|    | |238");
            objCP.AddCode("Undefined|    | |239");
            objCP.AddCode("Digit|0030|0 (number),0|240");
            objCP.AddCode("Digit|0031|1 (number),1|241");
            objCP.AddCode("Digit|0032|2 (number),2|242");
            objCP.AddCode("Digit|0033|3 (number),3|243");
            objCP.AddCode("Digit|0034|4 (number),4|244");
            objCP.AddCode("Digit|0035|5 (number),5|245");
            objCP.AddCode("Digit|0036|6 (number),6|246");
            objCP.AddCode("Digit|0037|7 (number),7|247");
            objCP.AddCode("Digit|0038|8 (number),8|248");
            objCP.AddCode("Digit|0039|9 (number),9|249");
            objCP.AddCode("Undefined|    | |250");
            objCP.AddCode("Undefined|    | |251");
            objCP.AddCode("Undefined|    | |252");
            objCP.AddCode("Undefined|    | |253");
            objCP.AddCode("Undefined|    | |254");
            objCP.AddCode("Control|    |Eight Ones,EO|255");

            objCP.Name = "EBCDIC";
            CodePages.Add(objCP);
        }
    }
}
