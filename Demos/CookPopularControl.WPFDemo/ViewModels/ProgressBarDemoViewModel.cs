/*
 *Description: ProgressBarDemoViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/8/28 16:42:54
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularUI.WPFDemo.ViewModels
{
    public class ProgressBarDemoViewModel : ViewModelBase
    {
        public double Value { get; set; }

        public DelegateCommand StartCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(OnStart)).Value;

        private async void OnStart()
        {
            Value = 0;
            await Task.Run(() =>
            {
                for (int i = 1; i <= 100; i++)
                {
                    Value = i;
                    Thread.Sleep(50);
                }
            });
        }
    }
}
