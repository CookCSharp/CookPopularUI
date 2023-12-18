/*
 *Description: ControlExtensions
 *Author: Chance.zheng
 *Creat Time: 2023/11/13 15:02:52
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularToolkit.Windows
{
    public static class ControlExtensions
    {
        public static T GetTemplateChild<T>(this System.Windows.Controls.Control control, string elementName)
        {
            CheckHelper.ArgumentNullException(control, nameof(control));
            return (T)control.Template.FindName(elementName, control);
        }
    }
}
