/*
 *Description: WindowDemoViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/8/24 18:02:07
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit.Windows;
using CookPopularUI.WPF.Controls;
using CookPopularUI.WPF.Windows;
using CookPopularUI.WPFDemo.Views;
using CookPopularUI.WPFDemo.Windows;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace CookPopularUI.WPFDemo.ViewModels
{
    public class WindowDemoViewModel : ViewModelBase
    {
        public string Text { get; set; } = "Hello~~~,WPF";

        public DelegateCommand<string> MessageDialogShowCommand => new Lazy<DelegateCommand<string>>(() => new DelegateCommand<string>(str => OnMessageDialogShowAction(str))).Value;
        public DelegateCommand<string> ShowWindowCommand => new Lazy<DelegateCommand<string>>(() => new DelegateCommand<string>(OnShowWindowAction)).Value;


        private void OnMessageDialogShowAction(string content)
        {
            var messageBoxResult = content switch
            {
                "ShowMessageDialogSuccess" => MessageDialog.ShowSuccess("MessageDialogTextBlockStyleMessageDialogTextBlockStyleMessageDialogTextBlockStyleMessageDialogTextBlockStyle写代码的厨子_Info写代码的厨子_Info写代码的厨子_Info写代码的厨子_Info写代码的厨子_Info写代码的厨子_I写代码的厨子_Info写代码的厨子_Info写代码的厨子_Info写代码的厨子_Info写代码的厨子写代码的厨子_Info写代码的厨子_Info写代码的厨子_Info写代码的厨子_Info写代码的厨子_Info写代码的厨子_Info写代码的厨子_Info_Info写代码的厨子_Info写代码的厨子_Info写代码的厨子_Infonfo顶顶顶大大大大大大顶顶顶顶滴滴答答而温热我热温热温热微软广泛大概豆腐干梵蒂冈地方官地方官地方官法国地方官梵蒂冈豆腐干豆腐干", "Test"),//MessageDialog.ShowSuccess("写代码的厨子_Success", "Chance"),
                "ShowMessageDialogInfo" => MessageDialog.ShowInfo("写代码的厨子_Info", "Chance"),
                "ShowMessageDialogWarning" => MessageDialog.ShowWarning("写代码的厨子_Warning", "Chance"),
                "ShowMessageDialogError" => MessageDialog.ShowError("写代码的厨子_Error", "Chance"),
                "ShowMessageDialogFatal" => MessageDialog.ShowFatal("写代码的厨子_Fatal", "Chance"),
                "ShowMessageDialogQuestion" => MessageDialog.ShowQuestion("写代码的厨子_Quetion", "Chance"),
                "ShowMessageDialogCustom" => MessageDialog.Show("写代码的厨子_Custom", "Chance", MessageBoxButton.YesNoCancel, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.ServiceNotification),
                _ => throw new NotImplementedException(),
            };
        }

        private void OnShowWindowAction(string content)
        {
            object d = content switch
            {
                "ShowNormalWindow" => Show<NormalWindow>("NormalWindow"),
                "ShowNoneTitleWindow" => Show<NoneTitleBarWindow>("NoneTitleBarWindow"),
                "ShowDialogWindow" => ShowDialogWindow(),
                "ShowFixedSizeWindow" => Show<FixedSizeDemoWindow>("FixedSizeDemoWindow"),
                _ => throw new NotImplementedException(),
            };
        }

        private async Task<string> ShowDialogWindow()
        {
            Text = await DialogWindow.Show<IconDemoView>("DialogWindow", true, true)
                                     .Initialize<IconDemoViewModel>(vm => { vm.NotifyText = Text; vm.Result = Text; })
                                     .GetResultAsync<string>();

            return Text;
        }

        private object Show<T>(string title = null) where T : Window, new()
        {
            var win = new T();
            //win.Icon = new Uri("pack://application:,,,/CookPopularUI.WPFDemo;component/Assets/Media/cookcsharp.ico", UriKind.Absolute).ToImageSource();
            win.Owner = Application.Current.MainWindow;
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.Title = title;
            win.Show();
            return this;
        }
    }
}
