/*
 *Description: OpenApplicationCommand
 *Author: Chance.zheng
 *Creat Time: 2023/9/22 15:48:06
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookPopularUI.WPF
{
    public class OpenApplicationCommand : ICommand
    {
        public bool CanExecute(object parameter) => !Application.Current.MainWindow.IsNull();

        public void Execute(object parameter)
        {
            var win = Application.Current.MainWindow;
            CheckHelper.ActionWhenBoolean(win.IsVisible, () => win.Activate(), () => win.Show());
            CommandManager.InvalidateRequerySuggested();
        }

        public event EventHandler CanExecuteChanged;
    }
}
