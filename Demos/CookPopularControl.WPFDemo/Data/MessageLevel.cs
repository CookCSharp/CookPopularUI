/*
 *Description: MessageLevel
 *Author: Chance.zheng
 *Creat Time: 2023/8/24 18:48:15
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularUI.WPFDemo
{
    public enum MessageLevel : byte
    {
        Success,
        Info,
        Warning,
        Error,
        Fatal,
        Question,
        Custom,
    }
}
