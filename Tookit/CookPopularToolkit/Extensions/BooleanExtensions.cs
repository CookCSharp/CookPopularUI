/*
 *Description: BooleanExtensions
 *Author: Chance.zheng
 *Creat Time: 2023/8/8 20:02:46
 *.Net Version: 2.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularToolkit
{
    public static class BooleanExtensions
    {
        public static int ToInt(this bool value) => value ? 1 : 0;

        public static bool ToBoolean(this string value, string standard = "0") => value != standard;

        public static bool ToBoolean(this object value)
        {
            if (value is int i)
                return i.ToBoolean();
            if (value is long l)
                return l.ToBoolean();
            else if (value is string s)
                return s.ToBoolean();
            else
                return value != null;
        }
    }
}
