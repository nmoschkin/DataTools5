using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.MathTools
{
    /// <summary>
    /// Implementation of binary search.
    /// </summary>
    public static class BinarySearch
    {


        /// <summary>
        /// Find an object in the specified sorted array of objects that implement <see cref="IComparable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to search.</typeparam>
        /// <param name="values">The array of values to search.</param>
        /// <param name="value">The value to find.</param>
        /// <param name="first">Set true to return the index of the first occurrence of value, otherwise, the first found index will be returned.</param>
        /// <returns>The index to the specified element, or -1 if not found.</returns>
        /// <remarks>
        /// T must implement <see cref="IComparable{T}"/>.
        /// </remarks>
        public static int Search<T>(T[] values, T value, bool first = true) where T : IComparable<T>
        {

            var comp = new Comparison<T>((a, b) =>
            {
                return a.CompareTo(b);
            });


            return Search(values, value, comp, first);
        }

        /// <summary>
        /// Find an object in the specified sorted array.
        /// </summary>
        /// <typeparam name="T">The type of the object to search.</typeparam>
        /// <param name="values">The array of values to search.</param>
        /// <param name="value">The value to find.</param>
        /// <param name="comparison">The comparison function to use.</param>
        /// <param name="first">Set true to return the index of the first occurrence of value, otherwise, the first found index will be returned.</param>
        /// <returns>The index to the specified element, or -1 if not found.</returns>
        public static int Search<T>(T[] values, T value, Comparison<T> comparison, bool first = true) 
        {
            int lo = 0, hi = values.Length - 1;

            while(true)
            {
                if (lo > hi) break;

                int p = ((hi + lo) / 2);
                T elem = values[p];

                int c = comparison(value, values[p]);
                if (c == 0)
                {
                    if (first && p > 0)
                    {
                        p--;

                        do
                        {
                            c = comparison(value, values[p]);

                            if (c != 0)
                            {
                                break;
                            }
                        } while (--p >= 0);

                        ++p;
                    }

                    return p;
                }
                else if (c < 0)
                {
                    hi = p - 1;
                }
                else 
                {
                    lo = p + 1;
                }
            }

            return -1;
        }


        /// <summary>
        /// Find an object in the specified sorted array of objects that implement <see cref="IComparable{U}"/>.
        /// </summary>
        /// <typeparam name="T">The type of class object to search.</typeparam>
        /// <typeparam name="U">The type of the property to search.</typeparam>
        /// <param name="values">The array of values to search.</param>
        /// <param name="value">The value of the specified property to find.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// <param name="propertyName">The name of the property to search.</param>
        /// <param name="retobj">Contains the object found, or null if not found.</param>
        /// <param name="first">Set true to return the index of the first occurrence of value, otherwise, the first found index will be returned.</param>
        /// <returns>The index to the specified element, or -1 if not found.</returns>
        /// <remarks>
        /// T must be a class type.
        /// propertyName must specify an instance property.
        /// </remarks>
        public static int Search<T, U>(T[] values, U value, IComparer<U> comparer, string propertyName, out T retobj, bool first = true) where T : class
        {
            return Search(values, value, comparer.Compare, propertyName, out retobj, first);
        }


        /// <summary>
        /// Find an object in the specified sorted array of objects with a property whose type implements <see cref="IComparable{U}"/>.
        /// </summary>
        /// <typeparam name="T">The type of class object to search.</typeparam>
        /// <typeparam name="U">The type of the property to search.</typeparam>
        /// <param name="values">The array of values to search.</param>
        /// <param name="value">The value of the specified property to find.</param>
        /// <param name="propertyName">The name of the property to search.</param>
        /// <param name="retobj">Contains the object found, or null if not found.</param>
        /// <param name="first">Set true to return the index of the first occurrence of value, otherwise, the first found index will be returned.</param>
        /// <returns>The index to the specified element, or -1 if not found.</returns>
        /// <remarks>
        /// T must be a class type.
        /// U must implement <see cref="IComparable{U}"/>.
        /// propertyName must specify an instance property.
        /// </remarks>
        public static int Search<T, U>(T[] values, U value, string propertyName, out T retobj, bool first = true) where T : class where U : IComparable<U>
        {
            var comp = new Comparison<U>((a, b) =>
            {
                return a.CompareTo(b);
            });


            return Search(values, value, comp, propertyName, out retobj, first);
        }


        /// <summary>
        /// Find an object in the specified sorted array.
        /// </summary>
        /// <typeparam name="T">The type of class object to search.</typeparam>
        /// <typeparam name="U">The type of the property to search.</typeparam>
        /// <param name="values">The array of values to search.</param>
        /// <param name="value">The value of the specified property to find.</param>
        /// <param name="comparison">The comparison function to use.</param>
        /// <param name="propertyName">The name of the property to search.</param>
        /// <param name="retobj">Contains the object found, or null if not found.</param>
        /// <param name="first">Set true to return the index of the first occurrence of value, otherwise, the first found index will be returned.</param>
        /// <returns>The index to the specified element, or -1 if not found.</returns>
        /// <remarks>
        /// T must be a class type.
        /// propertyName must specify an instance property.
        /// </remarks>
        public static int Search<T, U>(T[] values, U value, Comparison<U> comparison, string propertyName, out T retobj, bool first = true) where T: class
        {
            if (values == null || values.Length == 0)
            {
                retobj = null;
                return -1;
            }

            int lo = 0, hi = values.Length - 1;
            PropertyInfo prop = typeof(T).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (prop == null) throw new ArgumentException(nameof(propertyName));

            U comp;

            while (true)
            {
                if (lo > hi) break;

                int p = ((hi + lo) / 2);

                T elem = values[p];

                comp = (U)prop.GetValue(elem);
                
                int c = comparison(value, comp);
                if (c == 0)
                {
                    if (first && p > 0)
                    {
                        p--;

                        do
                        {
                            elem = values[p];
                            comp = (U)prop.GetValue(elem);

                            c = comparison(value, comp);

                            if (c != 0)
                            {
                                break;
                            }
                        } while (--p >= 0);

                        ++p;
                        elem = values[p];
                       
                    }

                    retobj = elem;
                    return p;
                }
                else if (c < 0)
                {
                    hi = p - 1;
                }
                else
                {
                    lo = p + 1;
                }
            }

            retobj = null;
            return -1;
        }



    }
}
