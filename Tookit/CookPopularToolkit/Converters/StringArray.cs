/*
 *Description: StringArray
 *Author: Chance.zheng
 *Creat Time: 2024/1/23 13:55:08
 *.Net Version: 2.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularToolkit
{
    [TypeConverter(typeof(StringToArraryConverter))]
    public struct StringArray
    {
        private Array _array;
        public Array Array
        {
            get { return _array; }
            set { _array = value; }
        }

        public StringArray(params string[] str)
        {
            _array = str;
        }

        public StringArray(Array array)
        {
            _array = array;
        }
    }
}
