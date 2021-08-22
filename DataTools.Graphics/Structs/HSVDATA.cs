using System;
using System.Runtime.InteropServices;

using System.Collections.Generic;

namespace DataTools.Graphics
{
    public struct HSVDATA
    {
        public double Hue;
        public double Saturation;
        public double Value;

        public HSVDATA(double h, double s, double v)
        {
            Hue = h;
            Saturation = s;
            Value = v;
        }

        public static explicit operator UniColor(HSVDATA h)
        {
            return ColorMath.HSVToColor(h);
        }

        public static explicit operator HSVDATA(UniColor c)
        {
            return ColorMath.ColorToHSV(c);
        }

        public override int GetHashCode()
        {
            List<byte> bl = new List<byte>();

            bl.AddRange(BitConverter.GetBytes(Hue));
            bl.AddRange(BitConverter.GetBytes(Saturation));
            bl.AddRange(BitConverter.GetBytes(Value));

            return (int)Streams.Crc32.Calculate(bl.ToArray());
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj is HSVDATA h)
            {
                return (h.Hue == Hue && h.Saturation == Saturation && h.Value == Value);
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return $"H: {Hue}, S: {Saturation}, V: {Value}";
        }

        public HSVDATA Abs()
        {
            return new HSVDATA(Math.Abs(Hue), Math.Abs(Saturation), Math.Abs(Value));
        }

        public static HSVDATA operator +(HSVDATA v1, HSVDATA v2)
        {
            return new HSVDATA(v1.Hue + v2.Hue, v1.Saturation + v2.Saturation, v1.Value + v2.Value);
        }

        public static HSVDATA operator -(HSVDATA v1, HSVDATA v2)
        {
            return new HSVDATA(v1.Hue - v2.Hue, v1.Saturation - v2.Saturation, v1.Value - v2.Value);
        }

        public static HSVDATA operator /(HSVDATA v1, HSVDATA v2)
        {
            return new HSVDATA(v1.Hue / v2.Hue, v1.Saturation / v2.Saturation, v1.Value / v2.Value);
        }

        public static HSVDATA operator *(HSVDATA v1, HSVDATA v2)
        {
            return new HSVDATA(v1.Hue * v2.Hue, v1.Saturation * v2.Saturation, v1.Value * v2.Value);
        }

        public static HSVDATA operator +(HSVDATA v1, double v2)
        {
            return new HSVDATA(v1.Hue + v2, v1.Saturation + v2, v1.Value + v2);
        }
        public static HSVDATA operator -(HSVDATA v1, double v2)
        {
            return new HSVDATA(v1.Hue - v2, v1.Saturation - v2, v1.Value - v2);
        }
        public static HSVDATA operator /(HSVDATA v1, double v2)
        {
            return new HSVDATA(v1.Hue / v2, v1.Saturation / v2, v1.Value / v2);
        }
        public static HSVDATA operator *(HSVDATA v1, double v2)
        {
            return new HSVDATA(v1.Hue * v2, v1.Saturation * v2, v1.Value * v2);
        }

        public static bool operator ==(HSVDATA v1, HSVDATA v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(HSVDATA v1, HSVDATA v2)
        {
            return !v1.Equals(v2);
        }


        public static bool operator >(HSVDATA v1, HSVDATA v2)
        {
            if (v1.Hue == v2.Hue)
            {
                if (v1.Saturation == v2.Saturation)
                {
                    if (v1.Value <= v2.Value)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return v1.Saturation > v2.Saturation;
                }
            }
            else
            {
                return v1.Hue > v2.Hue;
            }
        }

        public static bool operator >=(HSVDATA v1, HSVDATA v2)
        {
            if (v1.Hue == v2.Hue)
            {
                if (v1.Saturation == v2.Saturation)
                {
                    if (v1.Value < v2.Value)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return v1.Saturation >= v2.Saturation;
                }
            }
            else
            {
                return v1.Hue >= v2.Hue;
            }
        }



        public static bool operator <(HSVDATA v1, HSVDATA v2)
        {
            if (v1.Hue == v2.Hue)
            {
                if (v1.Saturation == v2.Saturation)
                {
                    if (v1.Value >= v2.Value)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return v1.Saturation < v2.Saturation;
                }
            }
            else
            {
                return v1.Hue < v2.Hue;
            }
        }

        public static bool operator <=(HSVDATA v1, HSVDATA v2)
        {
            if (v1.Hue == v2.Hue)
            {
                if (v1.Saturation == v2.Saturation)
                {
                    if (v1.Value > v2.Value)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return v1.Saturation <= v2.Saturation;
                }
            }
            else
            {
                return v1.Hue <= v2.Hue;
            }
        }


    }
}
