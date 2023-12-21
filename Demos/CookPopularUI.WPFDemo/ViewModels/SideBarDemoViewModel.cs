/*
 *Description: SideBarDemoViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/10/17 9:36:23
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit.Windows;
using CookPopularUI.WPF.Controls;
using Microsoft.Xaml.Behaviors.Layout;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CookPopularUI.WPFDemo.ViewModels
{
    public class SideBarExDemoViewModel : ViewModelBase
    {
        public bool IsShowSideBar { get; set; }

        public DelegateCommand<string> ShowOrCloseCommmand => new Lazy<DelegateCommand<string>>(() => new DelegateCommand<string>(OnShowOrCloseAction)).Value;

        private void OnShowOrCloseAction(string str)
        {
            var b = bool.Parse(str);
            IsShowSideBar = b;

            //var adorner = WindowHelper.GetActiveWindowWithWin32Api().GetVisualDescendants().OfType<AdornerDecorator>().FirstOrDefault();
            //var container = new AdornerContainer(adorner.AdornerLayer)
            //{
            //    Child = new Border { Background = "#88000000".ToBrush(), Child = new Button { Content = "12312312", Width = 200 } }
            //};
            //adorner.AdornerLayer.Add(container);
        }
    }
}
