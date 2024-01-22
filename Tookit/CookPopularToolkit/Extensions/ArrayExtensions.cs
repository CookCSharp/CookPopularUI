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

        /// <summary>  
        /// byte数组转int数组  
        /// </summary>  
        /// <param name="src">源byte数组</param>  
        /// <param name="offset">起始位置</param>  
        /// <returns></returns>  
        public static int[] ToIntArray(this byte[] src, int offset)
        {
            int[] values = new int[src.Length / 4];
            for (int i = 0; i < src.Length / 4; i++)
            {
                int value = (src[offset] & 0xFF)
                          | ((src[offset + 1] & 0xFF) << 8)
                          | ((src[offset + 2] & 0xFF) << 16)
                          | ((src[offset + 3] & 0xFF) << 24);
                values[i] = value;

                offset += 4;
            }

            return values;
        }

        /// <summary>  
        /// byte数组转int二维数组  
        /// </summary>  
        /// <param name="src">源byte数组</param>  
        /// <param name="offset">起始位置</param>  
        /// <returns></returns>  
        public static int[,] ToIntTwoArray(this byte[] src, int offset)
        {
            int[,] values = new int[src.Length / 8, 2];
            for (int i = 0; i < src.Length / 8; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    int value = (src[offset] & 0xFF)
                              | ((src[offset + 1] & 0xFF) << 8)
                              | ((src[offset + 2] & 0xFF) << 16)
                              | ((src[offset + 3] & 0xFF) << 24);
                    values[i, j] = value;

                    offset += 4;
                }
            }

            return values;
        }

        /// <summary>  
        /// int数组转byte数组  
        /// </summary>  
        /// <param name="src">源int数组</param> 
        /// <param name="offset">起始位置,一般为0</param>  
        /// <returns>values</returns>  
        public static byte[] ToBytes(int[] src, int offset)
        {
            byte[] values = new byte[src.Length * 4];
            for (int i = 0; i < src.Length; i++)
            {
                values[offset + 3] = (byte)((src[i] >> 24) & 0xFF);
                values[offset + 2] = (byte)((src[i] >> 16) & 0xFF);
                values[offset + 1] = (byte)((src[i] >> 8) & 0xFF);
                values[offset] = (byte)(src[i] & 0xFF);
                offset += 4;
            }

            return values;
        }
    }
}
