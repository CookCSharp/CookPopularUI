/*
 *Description: ProgressButtonDemoViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/10/13 16:58:27
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

namespace CookPopularUI.WPFDemo.ViewModels
{
    public class ProgressButtonDemoViewModel : ViewModelBase
    {
        private double _value;
        public double Value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                _value = value;

                RaisePropertyChanged();
            }
        }

        public DelegateCommand StartCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(() => OnStartAction())).Value;

        private async void OnStartAction()
        {
            Value = 0;
            await Task.Run(async () =>
            {
                for (int i = 1; i <= 100; i++)
                {
                    Value = i;
                    await Task.Delay(10);
                }
            });
        }
    }
}
