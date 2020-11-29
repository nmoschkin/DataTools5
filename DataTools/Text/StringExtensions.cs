using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class StringExtensions
    {

        /// <summary>
        /// Partition a string into an array of strings of the specified length.
        /// </summary>
        /// <param name="s">The string to partition.</param>
        /// <param name="length">The length of the partition.</param>
        /// <returns>An array of string</returns>
        /// <remarks>
        /// The last element may be shorter than the value specified by the length parameter.
        /// </remarks>
        public static string[] Partition(this string s, int length)
        {
            List<string> lw = new List<string>();

            if (s.Length <= length) return new string[] { s };

            int i, c = s.Length;

            for (i = 0; i < c; i += length)
            {
                if ((i + length) >= s.Length)
                {
                    lw.Add(s.Substring(i));
                }
                else
                {
                    lw.Add(s.Substring(i, length));
                }
            }

            return lw.ToArray();
        }
    }
}
