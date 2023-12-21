/*
 *Description: DialogBoxDemoViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/11/10 14:09:58
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularUI.WPF.Controls;
using CookPopularUI.WPFDemo.UserControls;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularUI.WPFDemo.ViewModels
{
    public class DialogBoxDemoViewModel : ViewModelBase
    {
        public string Text { get; set; } = "写代码的厨子";
        public string InternalText { get; set; } = "Chance";


        public DelegateCommand OpenCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(() => OnOpenActionAsync())).Value;
        public DelegateCommand OpenInternalCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(() => OnOpenInternalAction())).Value;

        private async void OnOpenActionAsync()
        {
            Text = await DialogBox.Show<DialogContentView>().Initialize<DialogContentViewModel>(vm => { vm.Result = Text; }).GetResultAsync<string>();
        }

        private async void OnOpenInternalAction()
        {
            InternalText = await DialogBox.Show<DialogContentView>("DialogBoxContainer").Initialize<DialogContentViewModel>(vm => { vm.Result = InternalText; }).GetResultAsync<string>();
        }
    }
}
