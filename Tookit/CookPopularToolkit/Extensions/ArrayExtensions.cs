/*
 *Description: ArrayExtensions
 *Author: Chance.zheng
 *Creat Time: 2023/8/11 17:15:28
 *.Net Version: 2.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Text;

namespace CookPopularToolkit
{
    public static class ArrayExtensions
    {
        public static bool IsNullOrEmpty<T>(this T[] array) => array == null || array.Length == 0;

        /// <summary>
        /// 获取集合中指定索引的值的，<paramref name="index"/>的值可为负数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T IndexValue<T>(this T[] array, int index)
        {
            if (index >= 0)
            {
                CheckHelper.ArgumentOutOfRangeException(Math.Abs(index) >= array.Length, nameof(index), index, "index was out of range");

                return array[index];
            }
            else
            {
                CheckHelper.ArgumentOutOfRangeException(Math.Abs(index) > array.Length, nameof(index), index, "index was out of range");

                //return array[Index.FromEnd(array.Length + index).Value];
#if NETSTANDARD2_0
                return array[Index.FromEnd(array.Length + index).Value];
#elif NETSTANDARD2_1_OR_GREATER
                return array[System.Index.FromEnd(array.Length + index).Value];
#endif
            }
        }
    }
}
