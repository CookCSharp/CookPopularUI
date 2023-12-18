/*
 *Description: OtherButtonDemoViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/9/4 11:39:17
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
    public class OtherButtonDemoViewModel : ViewModelBase
    {
        public int Value { get; set; }

        public DelegateCommand Command => new Lazy<DelegateCommand>(() => new DelegateCommand(() => OnAction())).Value;
        public DelegateCommand ButtonCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(() => OnButtonAction())).Value;
        public DelegateCommand MenuItemCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(() => OnMenuItemAction())).Value;
        public DelegateCommand DropDownOpenedCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(() => OnDropDownOpenedAction())).Value;
        public DelegateCommand DropDownClosedCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(() => OnDropDownClosedAction())).Value;
        public DelegateCommand StartCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(() => OnStartAction())).Value;


        private void OnAction()
        {
            MessageDialog.Show("Action");
        }

        private void OnButtonAction()
        {
            MessageDialog.Show("Button");
        }

        private void OnMenuItemAction()
        {
            MessageDialog.Show("MenuItem");
        }

        private void OnDropDownOpenedAction()
        {
            //MessageDialog.Show("Opened");
        }

        private void OnDropDownClosedAction()
        {
            //MessageDialog.Show("Closed");
        }

        private void OnStartAction()
        {
            Value++;
        }
    }
}
