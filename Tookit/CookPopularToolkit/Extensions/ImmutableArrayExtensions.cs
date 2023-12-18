/*
 *Description: ImmutableArrayExtensions
 *Author: Chance.zheng
 *Creat Time: 2023/8/21 10:14:23
 *.Net Version: 2.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace CookPopularToolkit
{
    /// <summary>
    /// The collection of extension methods for the <see cref="ImmutableArray{T}"/> type
    /// </summary>
    public static class ImmutableArrayExtensions
    {
        public static ImmutableArray<T> ToImmutableArrayOrEmpty<T>(this IEnumerable<T>? items)
        {
            if (items == null)
            {
                return ImmutableArray.Create<T>();
            }

            if (items is ImmutableArray<T> array)
            {
                return array.NullToEmpty();
            }

            return ImmutableArray.CreateRange<T>(items);
        }

        /// <summary>
        /// Converts a sequence to an immutable array.
        /// </summary>
        /// <typeparam name="T">Elemental type of the sequence.</typeparam>
        /// <param name="items">The sequence to convert.</param>
        /// <returns>An immutable copy of the contents of the sequence.</returns>
        /// <exception cref="ArgumentNullException">If items is null (default)</exception>
        /// <remarks>If the sequence is null, this will throw <see cref="ArgumentNullException"/></remarks>
        public static ImmutableArray<T> AsImmutable<T>(this IEnumerable<T> items)
        {
            return ImmutableArray.CreateRange<T>(items);
        }

        /// <summary>
        /// Converts a sequence to an immutable array.
        /// </summary>
        /// <typeparam name="T">Elemental type of the sequence.</typeparam>
        /// <param name="items">The sequence to convert.</param>
        /// <returns>An immutable copy of the contents of the sequence.</returns>
        /// <remarks>If the sequence is null, this will return an empty array.</remarks>
        public static ImmutableArray<T> AsImmutableOrEmpty<T>(this IEnumerable<T>? items)
        {
            if (items == null)
            {
                return ImmutableArray<T>.Empty;
            }

            return ImmutableArray.CreateRange<T>(items);
        }

        /// <summary>
        /// Converts a sequence to an immutable array.
        /// </summary>
        /// <typeparam name="T">Elemental type of the sequence.</typeparam>
        /// <param name="items">The sequence to convert.</param>
        /// <returns>An immutable copy of the contents of the sequence.</returns>
        /// <remarks>If the sequence is null, this will return the default (null) array.</remarks>
        public static ImmutableArray<T> AsImmutableOrNull<T>(this IEnumerable<T>? items)
        {
            if (items == null)
            {
                return default;
            }

            return ImmutableArray.CreateRange<T>(items);
        }

        /// <summary>
        /// Converts an array to an immutable array. The array must not be null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The sequence to convert</param>
        /// <returns></returns>
        public static ImmutableArray<T> AsImmutable<T>(this T[] items)
        {
            Debug.Assert(items != null);
            return ImmutableArray.Create<T>(items);
        }

        /// <summary>
        /// Converts a array to an immutable array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The sequence to convert</param>
        /// <returns></returns>
        /// <remarks>If the sequence is null, this will return the default (null) array.</remarks>
        public static ImmutableArray<T> AsImmutableOrNull<T>(this T[]? items)
        {
            if (items == null)
            {
                return default;
            }

            return ImmutableArray.Create<T>(items);
        }

        /// <summary>
        /// Converts an array to an immutable array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The sequence to convert</param>
        /// <returns>If the array is null, this will return an empty immutable array.</returns>
        public static ImmutableArray<T> AsImmutableOrEmpty<T>(this T[]? items)
        {
            if (items == null)
            {
                return ImmutableArray<T>.Empty;
            }

            return ImmutableArray.Create<T>(items);
        }

        /// <summary>
        /// Reads bytes from specified <see cref="MemoryStream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>Read-only content of the stream.</returns>
        public static ImmutableArray<byte> ToImmutable(this MemoryStream stream)
        {
            return ImmutableArray.Create<byte>(stream.ToArray());
        }

        /// <summary>
        /// Maps an immutable array to another immutable array.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="items">The array to map</param>
        /// <param name="map">The mapping delegate</param>
        /// <returns>If the items's length is 0, this will return an empty immutable array</returns>
        public static ImmutableArray<TResult> SelectAsArray<TItem, TResult>(this ImmutableArray<TItem> items, Func<TItem, TResult> map)
        {
            return ImmutableArray.CreateRange(items, map);
        }

        /// <summary>
        /// Maps an immutable array to another immutable array.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TArg"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="items">The sequence to map</param>
        /// <param name="map">The mapping delegate</param>
        /// <param name="arg">The extra input used by mapping delegate</param>
        /// <returns>If the items's length is 0, this will return an empty immutable array.</returns>
        public static ImmutableArray<TResult> SelectAsArray<TItem, TArg, TResult>(this ImmutableArray<TItem> items, Func<TItem, TArg, TResult> map, TArg arg)
        {
            return ImmutableArray.CreateRange(items, map, arg);
        }

        public static bool Any<T, TArg>(this ImmutableArray<T> array, Func<T, TArg, bool> predicate, TArg arg)
        {
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                var a = array[i];

                if (predicate(a, arg))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool All<T, TArg>(this ImmutableArray<T> array, Func<T, TArg, bool> predicate, TArg arg)
        {
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                var a = array[i];

                if (!predicate(a, arg))
                {
                    return false;
                }
            }

            return true;
        }

        public static async Task<bool> AnyAsync<T>(this ImmutableArray<T> array, Func<T, Task<bool>> predicateAsync)
        {
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                var a = array[i];

                if (await predicateAsync(a).ConfigureAwait(false))
                {
                    return true;
                }
            }

            return false;
        }

        public static async Task<bool> AnyAsync<T, TArg>(this ImmutableArray<T> array, Func<T, TArg, Task<bool>> predicateAsync, TArg arg)
        {
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                var a = array[i];

                if (await predicateAsync(a, arg).ConfigureAwait(false))
                {
                    return true;
                }
            }

            return false;
        }

        public static async ValueTask<T?> FirstOrDefaultAsync<T>(this ImmutableArray<T> array, Func<T, Task<bool>> predicateAsync)
        {
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                var a = array[i];

                if (await predicateAsync(a).ConfigureAwait(false))
                {
                    return a;
                }
            }

            return default;
        }

        public static TValue? FirstOrDefault<TValue, TArg>(this ImmutableArray<TValue> array, Func<TValue, TArg, bool> predicate, TArg arg)
        {
            foreach (var val in array)
            {
                if (predicate(val, arg))
                {
                    return val;
                }
            }

            return default;
        }

        /// <summary>
        /// Casts the immutable array of a Type to an immutable array of its base type.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImmutableArray<TBase> Cast<TDerived, TBase>(this ImmutableArray<TDerived> items) where TDerived : class, TBase
        {
            return ImmutableArray<TBase>.CastUp(items);
        }

        /// <summary>
        /// Determines whether this instance and another immutable array are equal.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <param name="comparer">The comparer to determine if the two arrays are equal.</param>
        /// <returns>True if the two arrays are equal</returns>
        public static bool SetEquals<T>(this ImmutableArray<T> array1, ImmutableArray<T> array2, IEqualityComparer<T> comparer)
        {
            if (array1.IsDefault)
            {
                return array2.IsDefault;
            }
            else if (array2.IsDefault)
            {
                return false;
            }

            var count1 = array1.Length;
            var count2 = array2.Length;

            // avoid constructing HashSets in these common cases
            if (count1 == 0)
            {
                return count2 == 0;
            }
            else if (count2 == 0)
            {
                return false;
            }
            else if (count1 == 1 && count2 == 1)
            {
                var item1 = array1[0];
                var item2 = array2[0];

                return comparer.Equals(item1, item2);
            }

            var set1 = new HashSet<T>(array1, comparer);
            var set2 = new HashSet<T>(array2, comparer);

            // internally recognizes that set2 is a HashSet with the same comparer (http://msdn.microsoft.com/en-us/library/bb346516.aspx)
            return set1.SetEquals(set2);
        }

        /// <summary>
        /// Returns an empty array if the input array is null (default)
        /// </summary>
        public static ImmutableArray<T> NullToEmpty<T>(this ImmutableArray<T> array)
        {
            return array.IsDefault ? ImmutableArray<T>.Empty : array;
        }

        /// <summary>
        /// Returns an empty array if the input nullable value type is null or the underlying array is null (default)
        /// </summary>
        public static ImmutableArray<T> NullToEmpty<T>(this ImmutableArray<T>? array) => array switch
        {
            null or { IsDefault: true } => ImmutableArray<T>.Empty,
            { } underlying => underlying
        };

        public static bool HasAnyErrors<T>(this ImmutableArray<T> diagnostics) where T : Diagnostic
        {
            foreach (var diagnostic in diagnostics)
            {
                if (diagnostic.Severity == DiagnosticSeverity.Error)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasDuplicates<T>(this ImmutableArray<T> array, IEqualityComparer<T>? comparer = null)
        {
            switch (array.Length)
            {
                case 0:
                case 1:
                    return false;
                case 2:
                    comparer ??= EqualityComparer<T>.Default;
                    return comparer.Equals(array[0], array[1]);
                default:
                    var set = new HashSet<T>(comparer);
                    foreach (var i in array)
                    {
                        if (!set.Add(i))
                        {
                            return true;
                        }
                    }
                    return false;
            }
        }

        public static int Count<T>(this ImmutableArray<T> items, Func<T, bool> predicate)
        {
            if (items.IsEmpty)
            {
                return 0;
            }

            int count = 0;
            for (int i = 0; i < items.Length; ++i)
            {
                if (predicate(items[i]))
                {
                    ++count;
                }
            }

            return count;
        }

        public static int Sum<T>(this ImmutableArray<T> items, Func<T, int> selector)
        {
            var sum = 0;
            foreach (var item in items)
                sum += selector(item);

            return sum;
        }

        public static Location FirstOrNone(this ImmutableArray<Location> items)
        {
            return items.IsEmpty ? Location.None : items[0];
        }

        public static bool IsSorted<T>(this ImmutableArray<T> array, IComparer<T> comparer)
        {
            for (var i = 1; i < array.Length; i++)
            {
                if (comparer.Compare(array[i - 1], array[i]) > 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
