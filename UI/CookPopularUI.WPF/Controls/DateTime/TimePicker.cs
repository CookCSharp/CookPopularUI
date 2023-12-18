/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：TimePicker
 * Author： Chance_写代码的厨子
 * Create Time：2021-07-27 17:19:34
 */


using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 时间选择器
    /// </summary>
    [TemplatePart(Name = ElementClock, Type = typeof(Clock))]
    [TemplatePart(Name = ElementPopup, Type = typeof(Popup))]
    public class TimePicker : Control
    {
        private const string ElementClock = "PART_Clock";
        private const string ElementPopup = "PART_Popup";

        private Clock? _clock = null;
        private Popup? _popup = null;

        public string CurrentTime
        {
            get { return (string)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="CurrentTime"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register(nameof(CurrentTime), typeof(string), typeof(TimePicker), new PropertyMetadata(DateTime.Now.ToString("HH:mm:ss")));


        public TimePicker()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.ConfirmCommand, (s, e) =>
            {
                if (_clock != null)
                    CurrentTime = _clock.CurrentTime;

                if (_popup != null)
                    _popup.IsOpen = false;
            }));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _clock = GetTemplateChild(ElementClock) as Clock;
            _popup = GetTemplateChild(ElementPopup) as Popup;
        }
    }
}
