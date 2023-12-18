/*
 *Description: TitleBarDemoViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/11/7 14:36:47
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularUI.WPF.Windows;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CookPopularUI.WPFDemo.ViewModels
{
    public class TitleBarDemoViewModel : ViewModelBase
    {
        public DelegateCommand LeftClickCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(OnLeftClickAction)).Value;

        private void OnLeftClickAction()
        {
            MessageDialog.Show("Left Click");
        }

        public DelegateCommand RightClickCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(OnRightClickAction)).Value;

        private void OnRightClickAction()
        {
            MessageDialog.Show("Right Click");
        }
    }
}
