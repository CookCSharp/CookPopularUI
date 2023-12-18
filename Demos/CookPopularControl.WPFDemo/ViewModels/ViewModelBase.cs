/*
 *Description: ViewModelBase
 *Author: Chance.zheng
 *Creat Time: 2023/8/21 16:18:34
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularUI.WPF.Controls;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CookPopularUI.WPFDemo.ViewModels
{
    public class ViewModelBase : BindableBase
    {
        public string CenterTitle { get; set; } = "CookPopularUI.WPFDemo(Home)";

        public DelegateCommand<FrameworkElement> LoadedCommand => new Lazy<DelegateCommand<FrameworkElement>>(() => new DelegateCommand<FrameworkElement>(OnLoadedAction)).Value;

        protected virtual void OnLoadedAction(FrameworkElement element)
        {

        }
    }
}
