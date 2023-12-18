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
    }
}
