/*
 *Description: WindowsNotificationCommand
 *Author: Chance.zheng
 *Creat Time: 2023/9/22 18:23:32
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit.Windows;
using CookPopularUI.WPF.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookPopularUI.WPF
{
    public class WindowsNotificationCommand : ICommand
    {
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            TaskbarIcon? taskbarIcon = null;
            if (parameter == null)
            {
                taskbarIcon = Application.Current.MainWindow.GetVisualDescendants().OfType<TaskbarIcon>().FirstOrDefault();
            }
            else
            {
                taskbarIcon = (TaskbarIcon)parameter;
            }

            if (taskbarIcon != null)
            {
                taskbarIcon.ShowBalloonTip("CookPopularUI.WPF", "Welcome To CookPopularUI.WPF", BalloonIcon.Info);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
