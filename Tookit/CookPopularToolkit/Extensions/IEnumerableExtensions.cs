/*
 *Description: IEnumerableExtensions
 *Author: Chance.zheng
 *Creat Time: 2023/8/21 10:07:48
 *.Net Version: 2.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text;

namespace CookPopularToolkit
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable ForEach(this IEnumerable source, Action<object> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            //Contract.Requires(source != null);
            //Contract.Requires(action != null);

            // perf optimization. try to not use enumerator if possible
            if (source is IList list)
            {
                for (int i = 0, count = list.Count; i < count; i++)
                {
                    action(list[i]);
                }
            }
            else
            {
                foreach (var value in source)
                {
                    action(value);
                }
            }

            return source;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            // perf optimization. try to not use enumerator if possible
            if (source is IList<T> list)
            {
                for (int i = 0, count = list.Count; i < count; i++)
                {
                    action(list[i]);
                }
            }
            else
            {
                var enumerable = source as IList<T> ?? source.ToList();
                foreach (var value in enumerable)
                {
                    action(value);
                }
            }

            return source;
        }

        public static async void ForEach<TSource>(this IEnumerable<TSource> source, Func<TSource, Task> action)
        {
            Contract.Requires(source != null);
            Contract.Requires(action != null);

            foreach (TSource item in source!)
            {
                await action!(item);
            }
        }

        public static bool All(this IEnumerable<bool> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            foreach (var b in source)
            {
                if (!b)
                {
                    return false;
                }
            }

            return true;
        }

        public static T[] AsArray<T>(this IEnumerable<T> source) => source as T[] ?? source.ToArray();

        private static int EnumeratingIndexOf<T>(this IEnumerable<T> sequence, T value, IEqualityComparer<T> comparer)
        {
            int i = 0;
            foreach (var item in sequence)
            {
                if (comparer.Equals(item, value))
                {
                    return i;
                }

                i++;
            }

            return -1;
        }

        public static int IndexOf<T>(this IEnumerable<T> sequence, T value, IEqualityComparer<T> comparer)
        {
            return sequence switch
            {
                IReadOnlyList<T> readOnlyList => IndexOf(readOnlyList, value, comparer),
                _ => EnumeratingIndexOf(sequence, value, comparer)
            };
        }

        public static int IndexOf<T>(this IEnumerable<T> sequence, T value)
        {
            return sequence switch
            {
                IList<T> list => list.IndexOf(value),
                IReadOnlyList<T> readOnlyList => IndexOf(readOnlyList, value, EqualityComparer<T>.Default),
                _ => EnumeratingIndexOf(sequence, value, EqualityComparer<T>.Default)
            };
        }

        public static int IndexOf<T>(this IReadOnlyList<T> list, T value, IEqualityComparer<T> comparer)
        {
            for (int i = 0, length = list.Count; i < length; i++)
            {
                if (comparer.Equals(list[i], value))
                {
                    return i;
                }
            }

            return -1;
        }

        public static bool Contains<T>(this IEnumerable<T> sequence, Func<T, bool> predicate)
        {
            return sequence.Any(predicate);
        }

        /// <summary>
        /// Returns the only element of specified sequence if it has exactly one, and default(TSource) otherwise.
        /// Unlike <see cref="Enumerable.SingleOrDefault{TSource}(IEnumerable{TSource})"/> doesn't throw if there is more than one element in the sequence.
        /// </summary>
        public static TSource? AsSingleton<TSource>(this IEnumerable<TSource>? source)
        {
            if (source == null)
            {
                return default;
            }

            if (source is IList<TSource> list)
            {
                return (list.Count == 1) ? list[0] : default;
            }

            using IEnumerator<TSource> e = source.GetEnumerator();
            if (!e.MoveNext())
            {
                return default;
            }

            TSource result = e.Current;
            if (e.MoveNext())
            {
                return default;
            }

            return result;
        }

        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new ReadOnlyCollection<T>(source.ToList());
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, T value)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            foreach (var v in source)
            {
                yield return v;
            }

            yield return value;
        }

        public static ISet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source as ISet<T> ?? new HashSet<T>(source);
        }

        public static ISet<T> ToHashSet<T>(this IEnumerable<T> source, IEqualityComparer<T>? comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new HashSet<T>(source, comparer);
        }

        public static bool SetEquals<T>(this IEnumerable<T> source1, IEnumerable<T> source2)
        {
            if (source1 == null)
            {
                throw new ArgumentNullException(nameof(source1));
            }

            if (source2 == null)
            {
                throw new ArgumentNullException(nameof(source2));
            }

            return source1.ToHashSet().SetEquals(source2);
        }

        public static bool SetEquals<T>(this IEnumerable<T> source1, IEnumerable<T> source2, IEqualityComparer<T>? comparer)
        {
            if (source1 == null)
            {
                throw new ArgumentNullException(nameof(source1));
            }

            if (source2 == null)
            {
                throw new ArgumentNullException(nameof(source2));
            }

            return source1.ToHashSet(comparer).SetEquals(source2);
        }

        public static IReadOnlyCollection<T> ToCollection<T>(this IEnumerable<T> sequence)
            => (sequence is IReadOnlyCollection<T> collection) ? collection : sequence.ToList();

        public static T? FirstOrNull<T>(this IEnumerable<T> source) where T : struct
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Cast<T?>().FirstOrDefault();
        }

        public static T? FirstOrNull<T>(this IEnumerable<T> source, Func<T, bool> predicate) where T : struct
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Cast<T?>().FirstOrDefault(v => predicate(v!.Value));
        }

        public static T? LastOrNull<T>(this IEnumerable<T> source) where T : struct
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Cast<T?>().LastOrDefault();
        }

        public static T? SingleOrNull<T>(this IEnumerable<T> source, Func<T, bool> predicate) where T : struct
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Cast<T?>().SingleOrDefault(v => predicate(v!.Value));
        }

        public static bool IsSingle<T>(this IEnumerable<T> list)
        {
            using var enumerator = list.GetEnumerator();
            return enumerator.MoveNext() && !enumerator.MoveNext();
        }

        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            if (source is IReadOnlyCollection<T> readOnlyCollection)
            {
                return readOnlyCollection.Count == 0;
            }

            if (source is ICollection<T> genericCollection)
            {
                return genericCollection.Count == 0;
            }

            if (source is IList<T> list)
            {
                return list.Count == 0;
            }

            if (source is ICollection collection)
            {
                return collection.Count == 0;
            }

            if (source is string str)
            {
                return str.Length == 0;
            }

            foreach (var _ in source)
            {
                return false;
            }

            return true;
        }

        public static bool SequenceEqual<T>(this IEnumerable<T>? first, IEnumerable<T>? second, Func<T, T, bool> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            if (first == second)
            {
                return true;
            }

            if (first == null || second == null)
            {
                return false;
            }

            using (var enumerator = first.GetEnumerator())
            using (var enumerator2 = second.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (!enumerator2.MoveNext() || !comparer(enumerator.Current, enumerator2.Current))
                    {
                        return false;
                    }
                }

                if (enumerator2.MoveNext())
                {
                    return false;
                }
            }

            return true;
        }

        public static T? AggregateOrDefault<T>(this IEnumerable<T> source, Func<T, T, T> func)
        {
            using (var e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    return default;
                }

                var result = e.Current;
                while (e.MoveNext())
                {
                    result = func(result, e.Current);
                }

                return result;
            }
        }

        public static IEnumerable<TObject> Except<TObject>(this IEnumerable<TObject> first, params TObject[] second)
        {
            return first.Except((IEnumerable<TObject>)second);
        }

        /// <summary>
        /// Ensures the count of a list is at least the given count. Creates with a given factory.
        /// </summary>
        public static bool EnsureListCountAtLeast<T>(this IList<T> list, int count, Func<T>? factory = null)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if (list.Count < count)
            {
                List<T>? list2 = list as List<T>;
                if (list2 != null && factory == null)
                {
                    list2.AddRange(new T[count - list.Count]);
                }
                else
                {
                    for (int i = list.Count; i < count; i++)
                    {
                        if (factory != null)
                            list.Add(factory());
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ensures the count of a list to a given count. Creates with a given factory or removes items when necessary.
        /// If Input IList is a List, AddRange or RemoveRange is used when there's no factory.
        /// </summary>
        public static bool EnsureListCount<T>(this IList<T> list, int count, Func<T>? factory = null)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if (list.EnsureListCountAtLeast(count, factory))
            {
                return true;
            }
            if (list.Count > count)
            {
                List<T>? list2 = list as List<T>;
                if (list2 != null)
                {
                    list2.RemoveRange(count, list.Count - count);
                }
                else
                {
                    for (int i = list.Count - 1; i >= count; i--)
                    {
                        list.RemoveAt(i);
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add a range of items to the end of a collection.
        /// If a collection is a list, List.AddRange is used.
        /// </summary>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> newItems)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            List<T>? list = collection as List<T>;
            if (list != null)
            {
                list.AddRange(newItems);
                return;
            }
            foreach (T item in newItems)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// Removes the last item from the given list.
        /// </summary>
        public static void RemoveLast<T>(this IList<T> list)
        {
            list.RemoveAt(list.Count - 1);
        }

        /// <summary>
        /// Get the duplicate item from the give source
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TKey> GetDuplicates<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            CheckHelper.ArgumentNullException(source, "source");
            CheckHelper.ArgumentNullException(keySelector, "keySelector");

            return source.GroupBy(keySelector).Where(g => g.Count() > 1).Select(y => y.Key);
        }

        /// <summary>
        /// Get the single item from the give source
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TKey> GetSingles<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            CheckHelper.ArgumentNullException(source, "source");
            CheckHelper.ArgumentNullException(keySelector, "keySelector");

            return source.GroupBy(keySelector).Where(g => g.Count() == 1).Select(y => y.Key);
        }

        /// <summary>
        /// 向某个集合中插入<see cref="char"/>字符组成新的字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="insertsymbol"></param>
        /// <returns>由集合组成的字符串</returns>
        public static string ToCombiner<T>(this IEnumerable<T> ts, char insertsymbol)
        {
            return ToCombiner(ts, insertsymbol.ToString());
        }

        /// <summary>
        /// 向某个集合中插入<see cref="string"/>字符串组成新的字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="insertsymbol"></param>
        /// <returns>由集合组成的字符串</returns>
        public static string ToCombiner<T>(this IEnumerable<T> ts, string insertsymbol = "")
        {
            if (ts.Count() <= 0) return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in ts)
            {
                stringBuilder.Append(item + insertsymbol);
            }

            return stringBuilder.Remove(stringBuilder.Length - insertsymbol.Length - 1, insertsymbol.Length).ToString();
        }
    }
}
