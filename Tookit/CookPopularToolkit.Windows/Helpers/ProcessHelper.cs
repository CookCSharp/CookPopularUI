/*
 *Description: ProcessHelper
 *Author: Chance.zheng
 *Creat Time: 2023/9/14 17:24:50
 *.Net Version: 2.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit.Windows.Win32Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;

namespace CookPopularToolkit.Windows
{
    public class ProcessHelper
    {
        /// <summary>
        /// 判断进程是否为服务
        /// </summary>
        /// <param name="process"></param>
        /// <remarks>可能不准</remarks>
        /// <returns></returns>
        public static bool CheckProcessIsService(Process process)
        {
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    string? binaryPathName = "";
                    try
                    {
                        IntPtr scManagerHandle = InteropMethods.OpenSCManager(null, null, InteropValues.SC_MANAGER_CONNECT);
                        if (scManagerHandle != IntPtr.Zero)
                        {
                            IntPtr serviceHandle = InteropMethods.OpenService(scManagerHandle, service.ServiceName, InteropValues.SERVICE_QUERY_CONFIG);
                            if (serviceHandle != IntPtr.Zero)
                            {
                                uint bytesNeeded = 0;
                                InteropMethods.QueryServiceConfig(serviceHandle, IntPtr.Zero, 0, out bytesNeeded);

                                if (bytesNeeded > 0)
                                {
                                    IntPtr qscPtr = Marshal.AllocHGlobal((int)bytesNeeded);
                                    if (InteropMethods.QueryServiceConfig(serviceHandle, qscPtr, bytesNeeded, out bytesNeeded))
                                    {
                                        InteropValues.QUERY_SERVICE_CONFIG qsc = (InteropValues.QUERY_SERVICE_CONFIG)Marshal.PtrToStructure(qscPtr, typeof(InteropValues.QUERY_SERVICE_CONFIG))!;
                                        binaryPathName = Marshal.PtrToStringAuto(qsc.lpBinaryPathName);
                                    }

                                    Marshal.FreeHGlobal(qscPtr);
                                }

                                InteropMethods.CloseServiceHandle(serviceHandle);
                            }

                            InteropMethods.CloseServiceHandle(scManagerHandle);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }

                    if (!string.IsNullOrWhiteSpace(binaryPathName) &&
                        process.MainModule != null &&
                        !string.IsNullOrWhiteSpace(process.MainModule.FileName) &&
                        binaryPathName.Contains(process.MainModule.FileName))
                        return true;
                }
            }
            catch (InvalidOperationException) //当没有足够权限访问服务时
            {
            }
            catch (Win32Exception)
            {
            }
            catch (Exception)
            {
            }

            return false;
        }
    }
}
