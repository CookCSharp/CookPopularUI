/*
 *Description: ProcessHelper
 *Author: Chance.zheng
 *Creat Time: 2023/9/14 17:24:50
 *.Net Version: 2.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.Text;

namespace CookPopularToolkit
{
    public class ProcessHelper
    {
        /// <summary>
        /// 获取<paramref name="process"/>的父进程
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static Process GetParentProcess(Process process)
        {
            var processName = Process.GetProcessById(process.Id).ProcessName;
            var processes = Process.GetProcessesByName(processName);
            string? processIndexName = null;

            for (int i = 0; i < processes.Length; i++)
            {
                processIndexName = i == 0 ? processName : processName + "#" + i;
                var processId = new PerformanceCounter("Process", "Creating Process ID", processIndexName);
                if ((int)processId.NextValue() == process.Id)
                {
                    break;
                }
            }

            var parentId = new PerformanceCounter("Process", "Creating Process ID", processIndexName!);
            var parentProcess = Process.GetProcessById((int)parentId.NextValue());

            return parentProcess;
        }
    }
}
