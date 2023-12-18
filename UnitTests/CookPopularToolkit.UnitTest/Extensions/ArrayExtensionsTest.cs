/*
 *Description: ArrayExtensionsTest
 *Author: Chance.zheng
 *Creat Time: 2023/10/13 14:13:38
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularToolkit.UnitTest.Extensions
{
    public class ArrayExtensionsTest
    {
        [Test(Description = "ArrayExtensions.IndexValue(this T[] array, int index)")]
        public void ArrayExtensions_Test()
        {
            byte[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var startIndex = System.Index.Start.Value;
            var endIndex = System.Index.End.Value;
            var v1 = array[System.Index.End.Value];
            var v2 = array.IndexValue(0);
            var v3 = array.IndexValue(-1);
            var v4 = array[Index.End.Value];
        }
    }
}
