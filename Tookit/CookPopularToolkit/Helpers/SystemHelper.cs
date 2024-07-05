/*
 *Description: SystemHelper
 *Author: Chance.zheng
 *Creat Time: 2023/9/14 17:26:14
 *.Net Version: 2.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Principal;
using System.Text;

namespace CookPopularToolkit
{
    public class SystemHelper
    {
        /// <summary>
        /// 检测是否为管理员权限
        /// </summary>
        /// <returns></returns>
        public static bool CheckIfAdmin()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            var isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);

            //AppDomain appDomain = Thread.GetDomain();
            //appDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            //var windowsPrincipal = (WindowsPrincipal)Thread.CurrentPrincipal;
            //var isAdmin = windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);

            return isAdmin;
        }

        /// <summary>
        /// 使用管理员权限启动程序
        /// </summary>
        /// <param name="arguments">不能为<c>null</c></param>
        public static void RunAsAdmin(string arguments = "")
        {
            try
            {
                var isAdmin = CheckIfAdmin();
                if (!isAdmin)
                {
                    var process = Process.GetCurrentProcess();
                    var fileName = process.MainModule.FileName;
                    Process.Start(new ProcessStartInfo { FileName = fileName, UseShellExecute = true, Verb = "runas", Arguments = arguments });
                    process.Kill();
                    process.WaitForExit();
                }
            }
            catch (Win32Exception ex) when (ex.NativeErrorCode == 1223)
            {
                Console.WriteLine("用户取消了权限提升。");
            }
            catch (Exception ex)
            {
                Console.WriteLine("启动程序时发生错误：" + ex.Message);
            }
        }
    }
}
