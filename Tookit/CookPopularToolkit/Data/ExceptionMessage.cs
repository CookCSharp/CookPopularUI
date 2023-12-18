/*
 *Description: ExceptionMessage
 *Author: Chance.zheng
 *Creat Time: 2023/9/9 12:13:12
 *.Net Version: 6.0
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
    internal static class ExceptionMessage
    {
        public static string GetArgumentNullOrEmptyExceptionMessage(string paramName) => $"{paramName} can not be null or empty";

        public static string GetFileNotFoundExceptionMessage(string paramName) => $"can not find the file path of {paramName}";
    }
}
