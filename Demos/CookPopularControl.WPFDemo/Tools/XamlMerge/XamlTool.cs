/*
 *Description: XamlTool
 *Author: Chance.zheng
 *Creat Time: 2023/12/5 14:23:21
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularUI.WPFDemo
{
    public class XamlTool
    {
        [DebuggerNonUserCode]
        public static void GetAllXamlFile()
        {
            var arrayPath = Directory.GetCurrentDirectory().Split("\\").Reverse().Skip(6).Reverse();
            var rootPath = string.Join("\\", arrayPath);
            var files = Directory.GetFiles(Path.Combine(rootPath, "UI\\CookPopularUI.WPF"), "*.xaml", SearchOption.AllDirectories).Skip(3);

            using var fs = new FileStream(Path.Combine(rootPath, "Demos\\CookPopularControl.WPFDemo\\Tools\\XamlMerge\\Theme.txt"), FileMode.Create, FileAccess.Write);
            var sw = new StreamWriter(fs);
            foreach (var file in files)
            {
                sw.WriteLine(file);
            }
            sw.Flush();
            sw.Close();
        }
    }
}
