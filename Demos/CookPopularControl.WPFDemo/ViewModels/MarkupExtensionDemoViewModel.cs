/*
 *Description: MarkupExtensionDemoViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/11/22 14:40:36
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularUI.WPF.Tools;
using CookPopularUI.WPF.Windows;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularUI.WPFDemo.ViewModels
{
    public class MarkupExtensionDemoViewModel : ViewModelBase
    {
        public bool IsClick { get; set; }

        public DelegateCommand YesCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(() => MessageDialog.Show("Excute YesCommand"))).Value;
        public DelegateCommand NoCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(() => MessageDialog.Show("Excute NoCommand"))).Value;
    }
}
