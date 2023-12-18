/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：NotifyMessage
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-21 09:16:19
 */


using CookPopularToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 消息通知的基类
    /// </summary>
    public abstract class NotifyMessageBase : ContentControl
    {
        private const string DefaultBubbleMessagePanel = nameof(DefaultBubbleMessagePanel);

        public static bool GetIsParentElement(DependencyObject obj) => (bool)obj.GetValue(IsParentElementProperty);
        public static void SetIsParentElement(DependencyObject obj, bool value) => obj.SetValue(IsParentElementProperty, ValueBoxes.BooleanBox(value));
        public static readonly DependencyProperty IsParentElementProperty =
            DependencyProperty.RegisterAttached("IsParentElement", typeof(bool), typeof(NotifyMessageBase), new PropertyMetadata(ValueBoxes.FalseBox, (s, e) =>
            {
                if ((bool)e.NewValue && s is Panel panel)
                {
                    Register(panel, DefaultBubbleMessagePanel);
                    panel.Unloaded += (s, e) => Unregister(panel);
                }
            }));


        public static string GetParentElementToken(DependencyObject obj) => (string)obj.GetValue(ParentElementTokenProperty);
        public static void SetParentElementToken(DependencyObject obj, string value) => obj.SetValue(ParentElementTokenProperty, value);
        public static readonly DependencyProperty ParentElementTokenProperty =
            DependencyProperty.RegisterAttached("ParentElementToken", typeof(string), typeof(NotifyMessageBase), new PropertyMetadata(default(string), OnTokenChanged));

        private static void OnTokenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Panel panel)
            {
                if (e.NewValue == null)
                {
                    Unregister(panel);
                }
                else
                {
                    panel.Unloaded += (s, e) => Unregister(panel);
                    Register(panel, e.NewValue.ToString());
                }
            }
        }

        private static void Unregister(Panel panel)
        {
            if (panel == null) return;
            var first = PanelDictionary.FirstOrDefault(item => ReferenceEquals(panel, item.Value));
            if (!string.IsNullOrEmpty(first.Key))
            {
                PanelDictionary.Remove(first.Key);
                panel.ContextMenu = null;
            }
        }

        private static void Register(Panel panel, string? token = default)
        {
            if (panel == null || string.IsNullOrEmpty(token)) return;

            var menuItem = new MenuItem();
            menuItem.Header = "Clear All";
            menuItem.Click += (s, e) => panel.Children.Clear();
            panel.ContextMenu = new ContextMenu
            {
                Items = { menuItem },
            };

            PanelDictionary[token!] = panel;
        }

        protected static Panel? GetDefaultMessagePanel()
        {
            var bv = PanelDictionary.TryGetValue(DefaultBubbleMessagePanel, out Panel? panel);
            return bv ? panel : null;
        }

        internal static bool GetIsShowCloseButton(DependencyObject obj) => (bool)obj.GetValue(IsShowCloseButtonProperty);
        internal static void SetIsShowCloseButton(DependencyObject obj, bool value) => obj.SetValue(IsShowCloseButtonProperty, value);
        internal static readonly DependencyProperty IsShowCloseButtonProperty =
            DependencyProperty.RegisterAttached("IsShowCloseButton", typeof(bool), typeof(NotifyMessageBase), new PropertyMetadata(ValueBoxes.TrueBox));


        protected const double AnimationTime = 0.5; //动画时间
        protected static readonly Dictionary<string, Panel> PanelDictionary = new Dictionary<string, Panel>();
        public static readonly ICommand CloseNotifyMessageCommand = new RoutedCommand(nameof(CloseNotifyMessageCommand), typeof(NotifyMessageBase));

        protected NotifyMessageBase() { }

        protected static DispatcherTimer IntervalMultiSeconds(ref DispatcherTimer? timer, double second, Action action)
        {
            timer = new DispatcherTimer(DispatcherPriority.Normal);
            timer.Interval = TimeSpan.FromSeconds(second);
            timer.Tick += delegate { action(); };
            return timer;
        }
    }

    /// <summary>
    /// 表示弹出控件的弹出位置；
    /// 由两部分组成，水平+垂直，总共9个弹出方向
    /// </summary>
    public enum PopupPosition
    {
        LeftTop,
        CenterTop,
        RightTop,
        LeftCenter,
        AllCenter,
        RightCenter,
        LeftBottom,
        CenterBottom,
        RightBottom
    }

    /// <summary>
    /// 通知消息附带的信息类
    /// </summary>
    public class NotifyMessageInfo
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// 消息图标
        /// </summary>
        public Geometry MessageIcon { get; set; }

        /// <summary>
        /// 消息图标颜色
        /// </summary>
        public Brush MessageIconBrush { get; set; }

        /// <summary>
        /// 消息通知的弹出位置
        /// </summary>
        public PopupPosition PopupPosition { get; set; }

        /// <summary>
        /// 消息在打开时如何显示动画
        /// </summary>
        public PopupAnimation PopupAnimation { get; set; } = PopupAnimation.Slide;

        /// <summary>
        /// 是否显示关闭按钮
        /// </summary>
        public bool IsShowCloseButton { get; set; } = true;

        /// <summary>
        /// 消息是否自动关闭
        /// </summary>
        public bool IsAutoClose { get; set; } = true;

        /// <summary>
        /// 消息持续时间
        /// </summary>
        /// <remarks>单位:s</remarks>
        public double Duration { get; set; } = 3;

        /// <summary>
        /// 消息关闭前触发的方法
        /// </summary>
        public Action<bool>? ActionBeforeClose { get; set; }
    }
}
