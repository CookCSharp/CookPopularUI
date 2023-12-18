/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：TaskbarIcon
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-30 16:41:35
 */


using CookPopularToolkit.Windows;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shell;
using System.Windows.Threading;
using ValueBoxes = CookPopularToolkit.ValueBoxes;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 系统托盘图标与任务栏图标
    /// </summary>
    public class TaskbarIcon : TrayIcon
    {
        private DispatcherTimer timer;
        private ImageSource originalIcon;


        /// <summary>
        /// 是否显示系统托盘
        /// </summary>
        public bool IsShow
        {
            get => (bool)GetValue(IsShowProperty);
            set => SetValue(IsShowProperty, ValueBoxes.BooleanBox(value));
        }
        /// <summary>
        /// 提供<see cref="IsShow"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsShowProperty =
            DependencyProperty.Register(nameof(IsShow), typeof(bool), typeof(TaskbarIcon), new PropertyMetadata(ValueBoxes.FalseBox));


        /// <summary>
        /// 是否开启任务栏闪烁
        /// </summary>
        public bool IsStartTaskbarFlash
        {
            get { return (bool)GetValue(IsStartTaskbarFlashProperty); }
            set { SetValue(IsStartTaskbarFlashProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// 提供<see cref="IsStartTaskbarFlash"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsStartTaskbarFlashProperty =
            DependencyProperty.Register(nameof(IsStartTaskbarFlash), typeof(bool), typeof(TaskbarIcon),
                new PropertyMetadata(ValueBoxes.FalseBox, OnTaskbarFlashChanged));

        private static void OnTaskbarFlashChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var taskbarIcon = d as TaskbarIcon;
            if (taskbarIcon != null)
            {
                if ((bool)e.NewValue)
                {
                    if (taskbarIcon.IsLoaded)
                    {
                        var win = Window.GetWindow(taskbarIcon);
                        win.SafeActivate();
                        win.FlashWindow(false);
                    }
                    else
                    {
                        taskbarIcon.Loaded += (s, e) =>
                        {
                            var win = Window.GetWindow(taskbarIcon);
                            win.SafeActivate();
                            win.FlashWindow(false);
                        };
                    }
                }
            }
        }


        /// <summary>
        /// 是否启用系统托盘图标闪烁
        /// </summary>
        public bool IsStartTrayIconFlash
        {
            get { return (bool)GetValue(IsStartTrayIconFlashProperty); }
            set { SetValue(IsStartTrayIconFlashProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// 提供<see cref="IsStartTrayIconFlash"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsStartTrayIconFlashProperty =
            DependencyProperty.Register(nameof(IsStartTrayIconFlash), typeof(bool), typeof(TaskbarIcon),
                new PropertyMetadata(ValueBoxes.FalseBox, OnTaskbarIconFlashPropertyChanged));

        private static void OnTaskbarIconFlashPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var taskbarIcon = d as TaskbarIcon;
            if (taskbarIcon != null)
            {
                if ((bool)e.NewValue)
                    taskbarIcon.timer.Start();
                else
                {
                    taskbarIcon.IconSource = taskbarIcon.originalIcon;
                    taskbarIcon.timer.Stop();
                }
            }
        }

        public TaskbarIcon()
        {
            this.Loaded += TaskbarIcon_Loaded;
            this.SetBinding(VisibilityProperty, new Binding() { Source = this, Path = new PropertyPath("IsShow"), Mode = BindingMode.TwoWay, Converter = new BooleanToVisibilityConverter() });
        }

        private void TaskbarIcon_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Window win in Application.Current.Windows)
            {
                win.Closing += (s, e) =>
                {
                    if (Visibility == Visibility.Visible)
                    {
                        win.Hide();
                        e.Cancel = true;
                    }
                };
            }

            BitmapImage tranparentIcon = new BitmapImage(new Uri("pack://application:,,,/CookPopularUI.WPF;component/Resources/Images/CookPopularUI.ico", UriKind.Absolute));
            originalIcon = IconSource;
            timer = new DispatcherTimer(DispatcherPriority.Normal);
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += (s, e) =>
            {
                if (IconSource.Equals(originalIcon))
                    IconSource = tranparentIcon;
                else
                    IconSource = originalIcon;
            };

            CommandBindings.Add(new CommandBinding(ControlCommands.WindowsNotificationCommand));
        }
    }
}
