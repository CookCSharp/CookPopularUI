/*
 *Description: ExitApplicationCommand
 *Author: Chance.zheng
 *Creat Time: 2023/9/22 16:40:13
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookPopularUI.WPF
{
    public class ExitApplicationCommand : ICommand
    {
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => Application.Current.Shutdown();

        public event EventHandler CanExecuteChanged;
    }
}
