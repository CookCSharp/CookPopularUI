/*
 *Description: InputDemoViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/8/31 17:02:10
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImTools.ImMap;

namespace CookPopularUI.WPFDemo.ViewModels
{
    public class InputDemoViewModel : ViewModelBase
    {
        public double Number { get; set; }
        public string Chinese { get; set; }
        public int NumericValue { get; set; }
        public string AutoText { get; set; } = "Cook1:写代码的厨子1&Cook2:写代码的厨子2&Cook3:写代码的厨子3&";
        public string PasswordContent { get; set; }
        public string SearchContent { get; set; }

        public DelegateCommand ValueChangedCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(OnValueChanged)).Value;
        public DelegateCommand StartSearchCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(OnStartSearch)).Value;

        private void OnValueChanged()
        {

        }

        private void OnStartSearch()
        {

        }
    }
}
