﻿/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：Window
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-24 09:43:30
 */


using CookPopularToolkit;
using CookPopularToolkit.Windows;
using CookPopularToolkit.Windows.Win32Api;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;
using ValueBoxes = CookPopularToolkit.ValueBoxes;

namespace CookPopularUI.WPF.Windows
{
    /// <summary>
    /// <see cref="NormalWindow"/>表示标准的窗体
    /// </summary>
    [TemplatePart(Name = TitleBarArea, Type = typeof(System.Windows.Controls.Grid))]
    [TemplatePart(Name = WindowIconButton, Type = typeof(System.Windows.Controls.Button))]
    public class NormalWindow : Window
    {
        private const string TitleBarArea = "PART_TitleBarArea";
        private const string WindowIconButton = "PART_WindowIcon";

        private WindowStyle _preWindowStyle;
        private WindowState _preWindowState;
        private ResizeMode _preResizeMode;
        private Button _windowIconButton;


        /// <summary>
        /// Represents a Windows icon, which is a small bitmap image that is used to represent an object
        /// </summary>
        public System.Drawing.Icon? WindowIcon
        {
            get
            {
                WindowInteropHelper interopHelper = new WindowInteropHelper(this);
                var exePath = Process.GetCurrentProcess().MainModule?.FileName;
                //System.Windows.Forms.Application.ExecutablePath
                return System.Drawing.Icon.ExtractAssociatedIcon(exePath!);
            }
        }

        #region DependencyProperties

        /// <summary>
        /// 表示标题栏前置颜色
        /// </summary>
        public Brush ClientTitleBarForeground
        {
            get { return (Brush)GetValue(ClientTitleBarForegroundProperty); }
            set { SetValue(ClientTitleBarForegroundProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="ClientTitleBarForeground"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ClientTitleBarForegroundProperty =
            DependencyProperty.Register(nameof(ClientTitleBarForeground), typeof(Brush), typeof(NormalWindow), new PropertyMetadata(default(Brush)));


        /// <summary>
        /// 表示标题栏背景颜色
        /// </summary>
        public Brush ClientTitleBarBackground
        {
            get { return (Brush)GetValue(ClientTitleBarBackgroundProperty); }
            set { SetValue(ClientTitleBarBackgroundProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="ClientTitleBarBackground"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ClientTitleBarBackgroundProperty =
            DependencyProperty.Register(nameof(ClientTitleBarBackground), typeof(Brush), typeof(NormalWindow), new PropertyMetadata(default(Brush)));


        /// <summary>
        /// 表示标题栏字体大小
        /// </summary>
        public double ClientTitleBarFontsize
        {
            get => (double)GetValue(ClientTitleBarFontsizeProperty);
            set => SetValue(ClientTitleBarFontsizeProperty, value);
        }
        /// <summary>
        /// 提供<see cref="ClientTitleBarFontsize"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ClientTitleBarFontsizeProperty =
            DependencyProperty.Register(nameof(ClientTitleBarFontsize), typeof(double), typeof(NormalWindow), new PropertyMetadata(14D));


        /// <summary>
        /// 标题栏附加内容
        /// </summary>
        /// <remarks>除了Window图标和Title以外的其它内容</remarks>
        public object ClientTitleBarAdditionalContent
        {
            get { return GetValue(ClientTitleBarAdditionalContentProperty); }
            set { SetValue(ClientTitleBarAdditionalContentProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="ClientTitleBarAdditionalContent"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ClientTitleBarAdditionalContentProperty =
            DependencyProperty.Register(nameof(ClientTitleBarAdditionalContent), typeof(object), typeof(NormalWindow), new PropertyMetadata(default(object)));


        /// <summary>
        /// 标题栏高度
        /// </summary>
        public double ClientTitleBarHeight
        {
            get { return (double)GetValue(ClientTitleBarHeightProperty); }
            set { SetValue(ClientTitleBarHeightProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="ClientTitleBarHeight"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ClientTitleBarHeightProperty =
            DependencyProperty.Register(nameof(ClientTitleBarHeight), typeof(double), typeof(NormalWindow), new PropertyMetadata(ValueBoxes.Double30Box));


        /// <summary>
        /// 是否显示窗口标题
        /// </summary>
        public bool IsShowTitle
        {
            get { return (bool)GetValue(IsShowTitleProperty); }
            set { SetValue(IsShowTitleProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// 标识<see cref="IsShowTitle"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsShowTitleProperty =
            DependencyProperty.Register(nameof(IsShowTitle), typeof(bool), typeof(NormalWindow), new PropertyMetadata(ValueBoxes.TrueBox));


        /// <summary>
        /// 是否显示标题栏
        /// </summary>
        internal bool IsShowClientTitleBar
        {
            get { return (bool)GetValue(IsShowClientTitleBarProperty); }
            set { SetValue(IsShowClientTitleBarProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// 标识<see cref="IsShowClientTitleBar"/>的依赖属性
        /// </summary>
        internal static readonly DependencyProperty IsShowClientTitleBarProperty =
            DependencyProperty.Register(nameof(IsShowClientTitleBar), typeof(bool), typeof(NormalWindow), new PropertyMetadata(ValueBoxes.TrueBox));


        /// <summary>
        /// 窗口是否全屏
        /// </summary>
        public bool IsFullScreen
        {
            get { return (bool)GetValue(IsFullScreenProperty); }
            set { SetValue(IsFullScreenProperty, value.BooleanBox()); }
        }
        /// <summary>
        /// 标识<see cref="IsFullScreen"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsFullScreenProperty =
            DependencyProperty.Register(nameof(IsFullScreen), typeof(bool), typeof(NormalWindow), new PropertyMetadata(ValueBoxes.FalseBox, OnIsFullScreenChanged));


        private static void OnIsFullScreenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NormalWindow win)
            {
                if (win.IsFullScreen)
                {
                    win.IsShowClientTitleBar = false;
                    win.ClientTitleBarHeight = 0;
                    win._preWindowStyle = win.WindowStyle;
                    win._preWindowState = win.WindowState;
                    win._preResizeMode = win.ResizeMode;
                    win.WindowStyle = WindowStyle.None;
                    win.WindowState = WindowState.Maximized;
                    win.WindowState = WindowState.Minimized;
                    win.WindowState = WindowState.Maximized;
                }
                else
                {
                    win.IsShowClientTitleBar = true;
                    win.ClientTitleBarHeight = win.ClientTitleBarHeight;
                    win.WindowStyle = win._preWindowStyle;
                    win.WindowState = win._preWindowState;
                    win.ResizeMode = win._preResizeMode;
                }
            }
        }


        /// <summary>
        /// NonClient是否激活
        /// </summary>
        public bool IsNonClientActive
        {
            get { return (bool)GetValue(IsNonClientActiveProperty); }
            set { SetValue(IsNonClientActiveProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// 标识<see cref="IsNonClientActive"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsNonClientActiveProperty =
            DependencyProperty.Register(nameof(IsNonClientActive), typeof(bool), typeof(NormalWindow), new PropertyMetadata(ValueBoxes.FalseBox, OnIsNoneClientActiveChanged));

        private static void OnIsNoneClientActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NormalWindow window)
            {
                IntPtr handle = window.EnsureHandle();
                window.GetHwndSource()?.AddHook(window.WndProc);
            }
        }
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == InteropValues.WM_NCACTIVATE)
                SetValue(IsNonClientActiveProperty, wParam == new IntPtr(1));

            return IntPtr.Zero;
        }


        /// <summary>
        /// 是否显示窗体Icon图标
        /// </summary>
        public bool IsShowIcon
        {
            get => (bool)GetValue(IsShowIconProperty);
            set => SetValue(IsShowIconProperty, value.BooleanBox());
        }
        /// <summary>
        /// 提供<see cref="IsShowIcon"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsShowIconProperty =
            DependencyProperty.Register(nameof(IsShowIcon), typeof(bool), typeof(NormalWindow), new PropertyMetadata(ValueBoxes.TrueBox));


        /// <summary>
        /// 窗体图标宽度
        /// </summary>
        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="IconWidth"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register(nameof(IconWidth), typeof(double), typeof(NormalWindow), new PropertyMetadata(ValueBoxes.Double20Box));


        /// <summary>
        /// 窗体图标高度
        /// </summary>
        public double IconHeight
        {
            get { return (double)GetValue(IconHeightProperty); }
            set { SetValue(IconHeightProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="IconHeight"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.Register(nameof(IconHeight), typeof(double), typeof(NormalWindow), new PropertyMetadata(ValueBoxes.Double20Box));

        #endregion


        static NormalWindow()
        {
            StyleProperty.OverrideMetadata(typeof(NormalWindow), new FrameworkPropertyMetadata(ResourceHelper.GetResource<Style>(typeof(NormalWindow))));
        }

        public NormalWindow()
        {
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, (s, e) => { SystemCommands.MinimizeWindow(this); e.Handled = true; }));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, (s, e) => { SystemCommands.MaximizeWindow(this); e.Handled = true; }));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, (s, e) => { SystemCommands.RestoreWindow(this); e.Handled = true; }));
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, (s, e) => Close(), (s, e) => e.CanExecute = true));
            CommandBindings.Add(new CommandBinding(SystemCommands.ShowSystemMenuCommand, (s, e) => ShowSystemMenu(e), (s, e) => e.CanExecute = true));

            if (Icon == null) SetDefaultWindowIcon();

            var windowChrome = new WindowChrome
            {
                CornerRadius = default,
                GlassFrameThickness = new Thickness(-1),
                ResizeBorderThickness = new Thickness(2),
                UseAeroCaptionButtons = false
            };
            BindingOperations.SetBinding(windowChrome, WindowChrome.CaptionHeightProperty, new Binding(ClientTitleBarHeightProperty.Name) { Source = this });
            WindowChrome.SetWindowChrome(this, windowChrome);

            this.Loaded += NormalWindow_Loaded;
        }

        public override void OnApplyTemplate()
        {
            if (_windowIconButton != null)
            {
                _windowIconButton.MouseDoubleClick -= (s, e) => Close();
            }

            base.OnApplyTemplate();

            _windowIconButton = (Button)GetTemplateChild(WindowIconButton);
            if (_windowIconButton != null)
            {
                _windowIconButton.MouseDoubleClick += (s, e) => Close();
            }
        }

        public void ShowSystemMenu(ExecutedRoutedEventArgs e)
        {
            Point point = this.PointToScreen(new Point(0, 0));
            var dipScale = WindowHelper.DpiX / 96d;
            if (this.WindowState == WindowState.Maximized)
            {
                //保证最大化时不改变标题高度
                point.X += (SystemParameters.WindowNonClientFrameThickness.Left + WindowHelper.PaddedBorderThickness.Left) * dipScale;
                point.Y += (SystemParameters.WindowNonClientFrameThickness.Top + WindowHelper.PaddedBorderThickness.Top +
                            SystemParameters.WindowResizeBorderThickness.Top - this.BorderThickness.Top) * dipScale;
            }
            else
            {
                point.X += this.BorderThickness.Left * dipScale;
                point.Y += SystemParameters.WindowNonClientFrameThickness.Top * dipScale;
            }

            CompositionTarget compositionTarget = PresentationSource.FromVisual(this).CompositionTarget;
            SystemCommands.ShowSystemMenu(this, compositionTarget.TransformFromDevice.Transform(point));
            e.Handled = true;
        }

        public void SetDefaultWindowIcon()
        {
            Icon = WindowIcon?.ToBitmap().ToImageSource();
            //System.Drawing.Icon icon = new System.Drawing.Icon("ApplicationIcon.ico");
            //InteropMethods.SendMessage(new WindowInteropHelper(this).Handle, 0x80/*WM_SETICON*/, 1 /*ICON_LARGE*/, icon.Handle);
        }

        private void NormalWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var workWidth = SystemParameters.WorkArea.Width;
            var workHeight = SystemParameters.WorkArea.Height;
            if (Width > workWidth || Height > workHeight)
            {
                Width = workWidth * 0.8;
                Height = workHeight * 0.8;
                Left = (workWidth - Width) / 2;
                Top = (workHeight - Height) / 2;
            }

            //SetWindowRound();
        }

        /// <summary>
        /// 设置Window圆角
        /// </summary>
        private void SetWindowRound()
        {
            IntPtr hWnd = this.EnsureHandle();
            var attribute = InteropValues.DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
            var preference = InteropValues.DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
            InteropMethods.DwmSetWindowAttribute(hWnd, attribute, ref preference, sizeof(uint));
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            /*****
             * 设置了SizeToContent="WidthAndHeight"时Window需要计算ClientArea的尺寸然后再确定Window的尺寸，
             * 但使用WindowChrome自定义Window时程序以为整个ControlTempalte的内容都是ClientArea，
             * 把它当作了ClientArea的尺寸，再加上non-client的尺寸就得出了错误的Window尺寸。
             * ControleTemplate的内容没办法遮住整个WindowChrome的内容，于是就出现了这些黑色的区域
             * 所以我们需要重新计算一次
             */
            //https://www.cnblogs.com/dino623/p/problems_of_WindowChrome.html
            if (SizeToContent == SizeToContent.WidthAndHeight && WindowChrome.GetWindowChrome(this) != null)
            {
                InvalidateMeasure();
            }

            if (this.WindowState == WindowState.Maximized)
            {
                //解决窗口以最大化启动,点击还原窗口时居中显示
                this.SizeChanged += NormalWindow_SizeChanged;
            }

            this.GetHwndSource()?.AddHook(HwndSourceHook);
        }

        private void NormalWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //InteropMethods.GetWindowRect(new WindowInteropHelper(this).Handle, out var rect);
            //int w = rect.Right - rect.Left;
            //int h = rect.Bottom - rect.Top;
            //int sw = InteropMethods.GetSystemMetrics(InteropValues.SM.CXSCREEN);
            //int sh = InteropMethods.GetSystemMetrics(InteropValues.SM.CYSCREEN);

            var dpiRatio = System.Drawing.Graphics.FromHwnd(this.EnsureHandle()).DpiX / 96;
            double width = Width * dpiRatio;
            double height = Height * dpiRatio;
            double workWidth = SystemParameters.WorkArea.Width * dpiRatio;
            double workHeight = SystemParameters.WorkArea.Height * dpiRatio;
            int x = (int)((workWidth - width) / 2);
            int y = (int)((workHeight - height) / 2);
            int w = (int)width;
            int h = (int)height;

            //var method3 = typeof(Window).GetMethod("SetupInitialState", BindingFlags.NonPublic | BindingFlags.Instance);
            //method3.Invoke(this, new object[] { (workWidth - width) / 2, (workHeight - height) / 2, Width, Height });

            this.Hide();
            InteropMethods.SetWindowPos(this.EnsureHandle(), IntPtr.Zero, x, y, w, h, InteropValues.WindowPositionFlags.SWP_ASYNCWINDOWPOS);
            this.Show();

            this.SizeChanged -= NormalWindow_SizeChanged;
        }

        private IntPtr HwndSourceHook(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            switch (msg)
            {
                case InteropValues.WM_WINDOWPOSCHANGED:
                    Padding = WindowState == WindowState.Maximized ? WindowHelper.WindowMaximizedPadding : Padding;
                    break;
                case InteropValues.WM_GETMINMAXINFO:
                    WmGetMinMaxInfo(hwnd, lparam);
                    Padding = WindowState == WindowState.Maximized ? WindowHelper.WindowMaximizedPadding : Padding;
                    break;
                case InteropValues.WM_NCHITTEST:
                    // https://developercommunity.visualstudio.com/t/overflow-exception-in-windowchrome/167357
                    try
                    {
                        _ = lparam.ToInt32();
                    }
                    catch (OverflowException)
                    {
                        handled = true;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        private void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            var mmi = (InteropValues.MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(InteropValues.MINMAXINFO))!;
            var monitor = InteropMethods.MonitorFromWindow(hwnd, InteropValues.MONITOR_DEFAULTTONEAREST);

            if (monitor != IntPtr.Zero && mmi != null)
            {
                InteropValues.APPBARDATA appBarData = default;
                var autoHide = InteropMethods.SHAppBarMessage(4, ref appBarData) != 0;
                if (autoHide)
                {
                    var monitorInfo = default(InteropValues.MONITORINFO);
                    monitorInfo.cbSize = (uint)Marshal.SizeOf(typeof(InteropValues.MONITORINFO));
                    InteropMethods.GetMonitorInfo(monitor, ref monitorInfo);
                    var rcWorkArea = monitorInfo.rcWork;
                    var rcMonitorArea = monitorInfo.rcMonitor;
                    mmi.ptMaxPosition.X = Math.Abs(rcWorkArea.Left - rcMonitorArea.Left);
                    mmi.ptMaxPosition.Y = Math.Abs(rcWorkArea.Top - rcMonitorArea.Top);
                    mmi.ptMaxSize.X = Math.Abs(rcWorkArea.Right - rcWorkArea.Left);
                    mmi.ptMaxSize.Y = Math.Abs(rcWorkArea.Bottom - rcWorkArea.Top - 1);
                }
            }

            Marshal.StructureToPtr(mmi!, lParam, true);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            SetValue(IsNonClientActiveProperty, true);
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            SetValue(IsNonClientActiveProperty, false);
        }
    }
}
