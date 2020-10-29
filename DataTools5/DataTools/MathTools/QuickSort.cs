using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.MathTools
{
    /// <summary>
    /// Implementation of quick sort.
    /// </summary>
    public static class QuickSort
    {
        /// <summary>
        /// Sort an array of objects that implement <see cref="IComparable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to sort.</typeparam>
        /// <param name="values">The array of values to sort.</param>
        public static void Sort<T>(ref T[] values) where T: IComparable<T>
        {
            if (values == null || values.Length == 0) return;

            var comp = new Comparison<T>((a, b) =>
            {
                return a.CompareTo(b);
            });

            int lo = 0;
            int hi = values.Length - 1;

            Sort<T>(ref values, comp, lo, hi);
        }

        /// <summary>
        /// Sort an array of objects.
        /// </summary>
        /// <typeparam name="T">The type of object to sort.</typeparam>
        /// <param name="values">The array of values to sort.</param>
        /// <param name="comparer">The comparer to use.</param>
        public static void Sort<T>(ref T[] values, IComparer<T> comparer)
        {
            if (values == null || values.Length == 0) return;

            int lo = 0;
            int hi = values.Length - 1;

            Sort<T>(ref values, comparer.Compare, lo, hi);
        }

        /// <summary>
        /// Sort an array of objects.
        /// </summary>
        /// <typeparam name="T">The type of object to sort.</typeparam>
        /// <param name="values">The array of values to sort.</param>
        /// <param name="comparison">The comparison function to use.</param>
        public static void Sort<T>(ref T[] values, Comparison<T> comparison)
        {
            if (values == null || values.Length == 0) return;

            int lo = 0;
            int hi = values.Length - 1;

            Sort<T>(ref values, comparison, lo, hi);
        }

        private static void Sort<T>(ref T[] values, Comparison<T> comparison, int lo, int hi)
        {
            if (lo < hi)
            {
                int p = Partition(ref values, comparison, lo, hi);

                Sort(ref values, comparison, lo, p);
                Sort(ref values, comparison, p + 1, hi);

            }
        }

        private static int Partition<T>(ref T[] values, Comparison<T> comparison, int lo, int hi)
        {
            var ppt = (hi + lo) / 2;
            var pivot = values[ppt];

            int i = lo - 1;
            int j = hi + 1;

            while (true)
            {
                do
                {
                    ++i;
                } while (i <= hi && comparison(values[i], pivot) < 0);
                do
                {
                    --j;
                } while (j >= 0 && comparison(values[j], pivot) > 0);
                if (i >= j) return j;

                T sw = values[i];

                values[i] = values[j];
                values[j] = sw;
            }
        }

    }
}
