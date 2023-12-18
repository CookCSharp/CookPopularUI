/*
 * Description：ControlCommands 
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2022-01-07 17:09:24
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2022 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookPopularUI.WPF
{
    /// <summary>
    /// 统一所有控件中所用到的命令
    /// </summary>
    public static class ControlCommands
    {
        public static ICommand OpenApplicationCommand = new OpenApplicationCommand();
        public static ICommand HideApplicationCommand = new HideApplicationCommand();
        public static ICommand ExitApplicationCommand = new ExitApplicationCommand();
        public static ICommand WindowsNotificationCommand = new WindowsNotificationCommand();

        /// <summary>
        /// 按照类别排序
        /// </summary>
        public static ICommand SortByCategoryCommand { get; } = new RoutedCommand(nameof(SortByCategoryCommand), typeof(ControlCommands));

        /// <summary>
        /// 按照名称排序
        /// </summary>
        public static ICommand SortByNameCommand { get; } = new RoutedCommand(nameof(SortByNameCommand), typeof(ControlCommands));

        public static ICommand ConfirmCommand { get; } = new RoutedCommand(nameof(ConfirmCommand), typeof(ControlCommands));
        public static ICommand YesCommand { get; } = new RoutedCommand(nameof(YesCommand), typeof(ControlCommands));
        public static ICommand NoCommand { get; } = new RoutedCommand(nameof(NoCommand), typeof(ControlCommands));
        public static ICommand CancelCommand { get; } = new RoutedCommand(nameof(CancelCommand), typeof(ControlCommands));
        public static ICommand NextCommand { get; } = new RoutedCommand(nameof(NextCommand), typeof(ControlCommands));
        public static ICommand PreviousCommand { get; } = new RoutedCommand(nameof(PreviousCommand), typeof(ControlCommands));
        public static ICommand ResetCommand { get; } = new RoutedCommand(nameof(ResetCommand), typeof(ControlCommands));
        public static ICommand FileOrFolderBrowserCommand { get; } = new RoutedCommand(nameof(FileOrFolderBrowserCommand), typeof(ControlCommands));
        public static ICommand DoubleClickCommand { get; } = new RoutedCommand(nameof(DoubleClickCommand), typeof(ControlCommands));
    }
}
