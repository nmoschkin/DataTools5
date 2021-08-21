using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.Win32
{

    /*
     * 
     * 
     * 
     * 
AC Voltage Volt Volt x00F0D121 7 8
AC Current centiAmp Amp x00100001 -2 16
Frequency Hertz Hertz xF001 0 8
DC Voltage centiVolt Volt x00FOD121 5 16
Time second s x1001 0 16
DC Current centiAmp Amp x00100001 -2 16
Apparent or
Active Power
VA or W VA or W xD121 7 16
Temperature °K °K x00010001 0 16
Battery
Capacity
AmpSec AmpSec x00101001 0 24
None None None x0 0 8
*/

    public enum HidPowerPhysicalUnitCode
    {
        ACVoltage = 0x00F0D121,
        ACCurrent = 0x00100001,
        Frequency = 0xF001,
        DCVoltage = 0x00F0D121,
        Time = 0x1001,
        DCCurrent = 0x00100001,
        ApparentPower = 0xD121,
        Temperature = 0x00010001,
        BatteryCapacity = 0x00101001,
        None = 0x0

    }


    public sealed class HidPowerPhysicalUnit
    {
        public string Name { get; private set; }
        
        public string PhysicalUnit { get; private set; }
            
        public string HIDUnit { get; private set; }

        public HidPowerPhysicalUnitCode Code { get; private set; }
        
        public int Exponent { get; private set; }
        
        public int FieldSize { get; private set; }
        private HidPowerPhysicalUnit(string name, string physUnit, string hidUnit, HidPowerPhysicalUnitCode unitCode, int exp, int fieldSize)
        {
            Name = name;
            PhysicalUnit = physUnit;
            HIDUnit = hidUnit;
            Code = unitCode;
            Exponent = exp;
            FieldSize = fieldSize;

            var res = units.Where((o) => o.Name == name).FirstOrDefault();
            if (res == null)
                units.Add(this);
        }

        private HidPowerPhysicalUnit(string name, string physUnit, string hidUnit, int unitCode, int exp, int fieldSize)
        {
            Name = name;
            PhysicalUnit = physUnit;
            HIDUnit = hidUnit;
            Code = (HidPowerPhysicalUnitCode)unitCode;
            Exponent = exp;
            FieldSize = fieldSize;

            var res = units.Where((o) => o.Name == name).FirstOrDefault();
            if (res == null)
                units.Add(this);
        }

        public static HidPowerPhysicalUnit GetByCode(int code)
        {
            return units.Where((o) => o.Code == (HidPowerPhysicalUnitCode)code).FirstOrDefault();
        }

        public static HidPowerPhysicalUnit GetByCode(HidPowerPhysicalUnitCode code)
        {
            return units.Where((o) => o.Code == code).FirstOrDefault();
        }

        public override string ToString()
        {
            return Name;
        }

        static List<HidPowerPhysicalUnit> units = new List<HidPowerPhysicalUnit>();

        public static IReadOnlyList<HidPowerPhysicalUnit> Units { get => units; private set => units = new List<HidPowerPhysicalUnit>(value); } 

        public static readonly HidPowerPhysicalUnit ACVoltage = new HidPowerPhysicalUnit("AC Voltage", "Volt", "Volt", 0x00F0D121, 7, 8);
        public static readonly HidPowerPhysicalUnit ACCurrent = new HidPowerPhysicalUnit( "AC Current",  "centiAmp",  "Amp",  0x00100001, - 2, 16);
        public static readonly HidPowerPhysicalUnit Frequency = new HidPowerPhysicalUnit( "Frequency",  "Hertz",  "Hertz",  0xF001,  0, 8);
        public static readonly HidPowerPhysicalUnit DCVoltage = new HidPowerPhysicalUnit( "DC Voltage",  "centiVolt",  "Volt",  0x00F0D121,  5, 16);
        public static readonly HidPowerPhysicalUnit Time = new HidPowerPhysicalUnit( "Time",  "second",  "s",  0x1001,  0, 16);
        public static readonly HidPowerPhysicalUnit DCCurrent = new HidPowerPhysicalUnit( "DC Current",  "centiAmp",  "Amp",  0x00100001, - 2, 16);
        public static readonly HidPowerPhysicalUnit ApparentPower = new HidPowerPhysicalUnit( "Apparent or Active Power",  "VA or W",  "VA or W",  0xD121,  7, 16);
        public static readonly HidPowerPhysicalUnit Temperature = new HidPowerPhysicalUnit( "Temperature",  "°K",  "°K",  0x00010001,  0, 16);
        public static readonly HidPowerPhysicalUnit BatteryCapacity = new HidPowerPhysicalUnit( "Battery Capacity",  "AmpSec",  "AmpSec",  0x00101001,  0, 24);
        public static readonly HidPowerPhysicalUnit None = new HidPowerPhysicalUnit( "None",  "None",  "None",  0x0,  0, 8);

    }
}
