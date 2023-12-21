/*
 *Description: IconDemoViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/9/22 14:50:04
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularUI.WPF.Controls;
using Prism.Commands;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularUI.WPFDemo.ViewModels
{
    public class IconDemoViewModel : ViewModelBase, IDialogResult<string>
    {
        public bool IsShowTrayIcon { get; set; } = false;

        public bool IsTaskbarIconFlash { get; set; }

        public bool IsTrayIconFlash { get; set; }

        public string NotifyText { get; set; } = "Hello~~~";

        public DelegateCommand<TaskbarIcon> SendNotifyCommand => new Lazy<DelegateCommand<TaskbarIcon>>(() => new DelegateCommand<TaskbarIcon>(OnSendNotifyAction)).Value;

        private void OnSendNotifyAction(TaskbarIcon taskbarIcon)
        {
            taskbarIcon.ShowBalloonTip("CookPopularUI.WPF", NotifyText, BalloonIcon.Info);
        }

        [DependsOn("NotifyText")]
        [AlsoNotifyFor("NotifyText")]
        public string Result { get; set; }
        public Action CloseAction { get; set; }

        private void OnNotifyTextChanged() => Result = NotifyText;
    }
}

