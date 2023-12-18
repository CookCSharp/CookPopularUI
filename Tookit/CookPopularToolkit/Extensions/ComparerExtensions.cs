/*
 *Description: ComparerExtensions
 *Author: Chance.zheng
 *Creat Time: 2023/8/21 10:54:18
 *.Net Version: 2.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Text;

namespace CookPopularToolkit
{
    public static class ComparerExtensions
    {
        public static IComparer<T> ToComparer<T>(this Comparison<T> comparison)
        {
            return Comparer<T>.Create(comparison);
        }
    }
}
