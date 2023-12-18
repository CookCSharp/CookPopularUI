/*
 *Description: OtherControlDemoViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/12/12 16:56:19
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularUI.WPF.Controls;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularUI.WPFDemo.ViewModels
{
    public class OtherControlDemoViewModel : ViewModelBase
    {
        private int index = 1;
        private List<AlertorState> AlertorStates = new List<AlertorState>() { AlertorState.Normal, AlertorState.Success, AlertorState.Warning, AlertorState.Error, AlertorState.Fatal };

        public AlertorState CurrentState { get; set; }
        public bool StartAlarm { get; set; }
        public string Content { get; set; } = "启动警报";

        public DelegateCommand StateExchangeCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(() => OnStateExchangeAction())).Value;
        public DelegateCommand StartOrStopCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(() => OnStartOrStopAction())).Value;


        private void OnStateExchangeAction()
        {
            if (index >= AlertorStates.Count)
                index = 0;
            CurrentState = AlertorStates[index];
            index++;
        }

        private void OnStartOrStopAction()
        {
            if (StartAlarm)
                Content = "解除警报";
            else
                Content = "启动警报";
        }

        private void OnStartAlarmChanged()
        {
            if (StartAlarm)
                Content = "解除警报";
            else
                Content = "启动警报";
        }
    }
}
