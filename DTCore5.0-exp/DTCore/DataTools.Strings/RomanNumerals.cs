// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library 
// '
// ' Module: Print and Interpret Roman Numerals
// ' 
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License   
// ' ************************************************* ''

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;

namespace DataTools.Strings
{

    /// <summary>
    /// Methods to output roman numeral characters.
    /// </summary>
    /// <remarks></remarks>
    public enum RomanNumeralStyle
    {

        /// <summary>
        /// Modern, conventional roman numeral notation.
        /// </summary>
        /// <remarks></remarks>
        Modern,

        /// <summary>
        /// Antique roman numeral notation.
        /// </summary>
        /// <remarks></remarks>
        Antique
    }

    /// <summary>
    /// Static class to provide roman numeral translation.
    /// </summary>
    /// <remarks></remarks>
    public class RomanNumerals
    {

        /// <summary>
        /// This class is not creatable.
        /// </summary>
        /// <remarks></remarks>
        private RomanNumerals()
        {
        }

        /// <summary>
        /// Converts a roman numeral string into an integer.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int ToInteger(string x)
        {
            var ch = TextTools.NoSpace(x.ToUpper().Trim()).ToCharArray();
            int c = 0;
            int d = ch.Length - 1;
            var vals = new List<int>();
            for (int i = 0, loopTo = d; i <= loopTo; i++)
            {
                switch (ch[i])
                {
                    case 'M':
                        {
                            vals.Add(1000);
                            break;
                        }

                    case 'D':
                        {
                            vals.Add(500);
                            break;
                        }

                    case 'C':
                        {
                            vals.Add(100);
                            break;
                        }

                    case 'L':
                        {
                            vals.Add(50);
                            break;
                        }

                    case 'X':
                        {
                            vals.Add(10);
                            break;
                        }

                    case 'V':
                        {
                            vals.Add(5);
                            break;
                        }

                    case 'I':
                        {
                            vals.Add(1);
                            break;
                        }
                }
            }

            d = vals.Count - 1;
            for (int i = d; i >= 0; i -= 1)
            {
                if (i < d)
                {
                    if (vals[i + 1] > vals[i])
                    {
                        c -= vals[i];
                        continue;
                    }
                }

                c += vals[i];
            }

            return c;
        }

        /// <summary>
        /// Converts an integer into a roman numeral string.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToNumerals(int x, RomanNumeralStyle style = RomanNumeralStyle.Modern)
        {
            if (x == 0)
                return "NVL";
            if (x < 0)
                x = -x;
            string s = "" + x;
            string @out = "";
            var ch = s.ToCharArray();
            switch (ch.Length)
            {
                case 1:
                    {
                        return FirstToNum(ch[0], style);
                    }

                case 2:
                    {
                        return SecondToNum(ch[0], style) + FirstToNum(ch[1], style);
                    }

                case 3:
                    {
                        return ThirdToNum(ch[0], style) + SecondToNum(ch[1], style) + FirstToNum(ch[2], style);
                    }

                case var @case when @case >= 4:
                    {
                        return FourthToNum(s) + ThirdToNum(ch[1], style) + SecondToNum(ch[2], style) + FirstToNum(ch[3], style);
                    }
            }

            return null;
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private static readonly string[] FirstNums = new[] { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };
        private static readonly string[] FirstNumsAntique = new[] { "", "I", "II", "III", "IIII", "V", "VI", "VII", "VIII", "VIIII" };

        /// <summary>
        /// Converts the first integer (from right-to-left) into a roman numeral.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string FirstToNum(char ch, RomanNumeralStyle style = RomanNumeralStyle.Modern)
        {
            return style == RomanNumeralStyle.Antique ? FirstNumsAntique[int.Parse(Conversions.ToString(ch))] : FirstNums[int.Parse(Conversions.ToString(ch))];
        }

        private static readonly string[] SecondNums = new[] { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
        private static readonly string[] SecondNumsAntique = new[] { "", "X", "XX", "XXX", "XXXX", "L", "LX", "LXX", "LXXX", "LXXXX" };

        /// <summary>
        /// Converts the second integer (from right-to-left) into a roman numeral.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string SecondToNum(char ch, RomanNumeralStyle style = RomanNumeralStyle.Modern)
        {
            return style == RomanNumeralStyle.Antique ? SecondNumsAntique[int.Parse(Conversions.ToString(ch))] : SecondNums[int.Parse(Conversions.ToString(ch))];
        }

        private static readonly string[] ThirdNums = new[] { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" };
        private static readonly string[] ThirdNumsAntique = new[] { "", "C", "CC", "CCC", "CCCC", "D", "DC", "DCC", "DCCC", "DCCCC" };

        /// <summary>
        /// Converts the third integer (from right-to-left) into a roman numeral.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string ThirdToNum(char ch, RomanNumeralStyle style = RomanNumeralStyle.Modern)
        {
            return style == RomanNumeralStyle.Antique ? ThirdNumsAntique[int.Parse(Conversions.ToString(ch))] : ThirdNums[int.Parse(Conversions.ToString(ch))];
        }

        /// <summary>
        /// Converts subsequent integers into a series of M's.
        /// </summary>
        /// <param name="textNum"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string FourthToNum(string textNum)
        {
            try
            {
                string s = textNum.Substring(0, textNum.Length - 3);
                var o = new StringBuilder();
                int v = Conversions.ToInteger(TextTools.FVal(s));
                for (int i = 1, loopTo = v; i <= loopTo; i++)
                    o.Append("M");
                return o.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }
}