using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Win32
{


    public interface IHidPValueCaps
    {
        ushort UsagePage { get; }
        byte ReportID { get; }
        bool IsAlias { get; }
        ushort BitField { get; }
        ushort LinkCollection { get; }
        ushort LinkUsage { get; }
        ushort LinkUsagePage { get; }
        bool IsRange { get; }
        bool IsStringRange { get; }
        bool IsDesignatorRange { get; }
        bool IsAbsolute { get; }
        bool HasNull { get; }
        byte Reserved { get; }
        ushort BitSize { get; }
        ushort ReportCount { get; }
        uint UnitsExp { get; }
        uint Units { get; }
        int LogicalMin { get; }
        int LogicalMax { get; }
        int PhysicalMin { get; }
        int PhysicalMax { get; }

    }

    public interface IHidPValueCaps_Range : IHidPValueCaps
    {

        ushort UsageMin { get; }
        ushort UsageMax { get; }
        ushort StringMin { get; }
        ushort StringMax { get; }
        ushort DesignatorMin { get; }
        ushort DesignatorMax { get; }
        ushort DataIndexMin { get; }
        ushort DataIndexMax { get; }

    }

    public interface IHidPValueCaps_NonRange : IHidPValueCaps
    {
        ushort Usage { get; }
        ushort Reserved1 { get; }
        ushort StringIndex { get; }
        ushort Reserved3 { get; }
        ushort DesignatorIndex { get; }
        ushort Reserved4 { get; }
        ushort DataIndex { get; }
        ushort Reserved5 { get; }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HidPValueCapsRange : IHidPValueCaps_Range
    {
        ushort _UsagePage;
        byte _ReportID;
        [MarshalAs(UnmanagedType.U1)]
        bool _IsAlias;
        ushort _BitField;
        ushort _LinkCollection;
        ushort _LinkUsage;
        ushort _LinkUsagePage;
        [MarshalAs(UnmanagedType.U1)]
        bool _IsRange;
        [MarshalAs(UnmanagedType.U1)]
        bool _IsStringRange;
        [MarshalAs(UnmanagedType.U1)]
        bool _IsDesignatorRange;
        [MarshalAs(UnmanagedType.U1)]
        bool _IsAbsolute;
        [MarshalAs(UnmanagedType.U1)]
        bool _HasNull;
        byte _Reserved;
        ushort _BitSize;
        ushort _ReportCount;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U2, SizeConst = 5)]
        ushort[] _Reserved2;

        uint _UnitsExp;
        uint _Units;
        int _LogicalMin;
        int _LogicalMax;
        int _PhysicalMin;
        int _PhysicalMax;

        ushort _UsageMin;
        ushort _UsageMax;
        ushort _StringMin;
        ushort _StringMax;
        ushort _DesignatorMin;
        ushort _DesignatorMax;
        ushort _DataIndexMin;
        ushort _DataIndexMax;

        public ushort UsagePage => _UsagePage;
        public byte ReportID => _ReportID;
        public bool IsAlias => _IsAlias;
        public ushort BitField => _BitField;
        public ushort LinkCollection => _LinkCollection;
        public ushort LinkUsage => _LinkUsage;
        public ushort LinkUsagePage => _LinkUsagePage;
        public bool IsRange => _IsRange;
        public bool IsStringRange => _IsStringRange;
        public bool IsDesignatorRange => _IsDesignatorRange;
        public bool IsAbsolute => _IsAbsolute;
        public bool HasNull => _HasNull;
        public byte Reserved => _Reserved;
        public ushort BitSize => _BitSize;
        public ushort ReportCount => _ReportCount;
        public uint UnitsExp => _UnitsExp;
        public uint Units => _Units;
        public int LogicalMin => _LogicalMin;
        public int LogicalMax => _LogicalMax;
        public int PhysicalMin => _PhysicalMin;
        public int PhysicalMax => _PhysicalMax;
        public ushort UsageMin => _UsageMin;
        public ushort UsageMax => _UsageMax;
        public ushort StringMin => _StringMin;
        public ushort StringMax => _StringMax;
        public ushort DesignatorMin => _DesignatorMin;
        public ushort DesignatorMax => _DesignatorMax;
        public ushort DataIndexMin => _DataIndexMin;
        public ushort DataIndexMax => _DataIndexMax;    
    
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HidPValueCaps : IHidPValueCaps_NonRange
    {
        ushort _UsagePage;
        byte _ReportID;
        [MarshalAs(UnmanagedType.U1)]
        bool _IsAlias;
        ushort _BitField;
        ushort _LinkCollection;
        ushort _LinkUsage;
        ushort _LinkUsagePage;
        [MarshalAs(UnmanagedType.U1)]
        bool _IsRange;
        [MarshalAs(UnmanagedType.U1)]
        bool _IsStringRange;
        [MarshalAs(UnmanagedType.U1)]
        bool _IsDesignatorRange;
        [MarshalAs(UnmanagedType.U1)]
        bool _IsAbsolute;
        [MarshalAs(UnmanagedType.U1)]
        bool _HasNull;
        byte _Reserved;
        ushort _BitSize;
        ushort _ReportCount;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U2, SizeConst = 5)]
        ushort[] _Reserved2;

        uint _UnitsExp;
        uint _Units;
        int _LogicalMin;
        int _LogicalMax;
        int _PhysicalMin;
        int _PhysicalMax;

        ushort _Usage;
        ushort _Reserved1;
        ushort _StringIndex;
        ushort _Reserved3;
        ushort _DesignatorIndex;
        ushort _Reserved4;
        ushort _DataIndex;
        ushort _Reserved5;


        public ushort UsagePage => _UsagePage;
        public byte ReportID => _ReportID;
        public bool IsAlias => _IsAlias;
        public ushort BitField => _BitField;
        public ushort LinkCollection => _LinkCollection;
        public ushort LinkUsage => _LinkUsage;
        public ushort LinkUsagePage => _LinkUsagePage;
        public bool IsRange => _IsRange;
        public bool IsStringRange => _IsStringRange;
        public bool IsDesignatorRange => _IsDesignatorRange;
        public bool IsAbsolute => _IsAbsolute;
        public bool HasNull => _HasNull;
        public byte Reserved => _Reserved;
        public ushort BitSize => _BitSize;
        public ushort ReportCount => _ReportCount;
        public uint UnitsExp => _UnitsExp;
        public uint Units => _Units;
        public int LogicalMin => _LogicalMin;
        public int LogicalMax => _LogicalMax;
        public int PhysicalMin => _PhysicalMin;
        public int PhysicalMax => _PhysicalMax;
        public ushort Usage => _Usage;
        public ushort Reserved1 => _Reserved1;
        public ushort StringIndex => _StringIndex;
        public ushort Reserved3 => _Reserved3;
        public ushort DesignatorIndex => _DesignatorIndex;
        public ushort Reserved4 => _Reserved4;
        public ushort DataIndex => _DataIndex;
        public ushort Reserved5 => _Reserved5;    }
}
