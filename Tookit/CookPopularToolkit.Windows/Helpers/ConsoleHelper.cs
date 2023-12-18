/*
 *Description: ConsoleHelper
 *Author: Chance.zheng
 *Creat Time: 2023/8/9 18:25:02
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit.Windows.Win32Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularToolkit.Windows
{
    public sealed class ConsoleHelper
    {
        /// <summary>
        /// 设置控制台的显示与隐藏
        /// </summary>
        /// <param name="isShowConsole">是否显示控制台</param>
        /// <param name="consoleTitle">控制台标题</param>
        public static void ShowOrHideConsole(bool isShowConsole, string? consoleTitle = null)
        {
            consoleTitle = string.IsNullOrEmpty(consoleTitle) ? Console.Title : consoleTitle;
            IntPtr hWnd = InteropMethods.FindWindow("ConsoleWindowClass", consoleTitle!);
            if (hWnd != IntPtr.Zero)
            {
                if (isShowConsole)
                    InteropMethods.ShowWindow(hWnd, 1);
                else
                    InteropMethods.ShowWindow(hWnd, 0);
            }
        }
    }
}
