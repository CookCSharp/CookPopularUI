/*
 *Description: ButtonDemoViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/9/1 14:14:12
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
    public class ButtonDemoViewModel : ViewModelBase
    {
        private double _value;
        public double Value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                _value = value;
                if (_value.BetweenMinMax(0, 100))
                    IsButtonEnabled = false;
                else
                    IsButtonEnabled = true;

                RaisePropertyChanged();
            }
        }

        public bool IsButtonEnabled { get; set; }

        public string ButtonContent { get; set; }

        public DelegateCommand StartCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(() => OnStartAction())).Value;

        private void OnStartAction()
        {
            Task.Run(() =>
            {
                for (int i = 1; i <= 100; i++)
                {
                    Value = i;
                    System.Threading.Thread.Sleep(10);
                }

                ButtonContent = "成功!";
                //System.Threading.Thread.Sleep(500);
                //Value = 0;
                //ButtonContent = "开始!";
            });
        }
    }
}
