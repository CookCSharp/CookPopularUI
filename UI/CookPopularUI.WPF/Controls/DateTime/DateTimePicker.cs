/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DateTimePicker
 * Author： Chance_写代码的厨子
 * Create Time：2023-11-08 17:19:34
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
    [TemplatePart(Name = ElementCalendar, Type = typeof(Calendar))]
    [TemplatePart(Name = ElementClock, Type = typeof(Clock))]
    [TemplatePart(Name = ElementPopup, Type = typeof(Popup))]
    public class DateTimePicker : Control
    {
        private const string ElementCalendar = "PART_Calendar";
        private const string ElementClock = "PART_Clock";
        private const string ElementPopup = "PART_Popup";

        private Calendar? _calendar = null;
        private Clock? _clock = null;
        private Popup? _popup = null;

        public string Now
        {
            get { return (string)GetValue(NowProperty); }
            set { SetValue(NowProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="Now"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty NowProperty =
            DependencyProperty.Register(nameof(Now), typeof(string), typeof(DateTimePicker), new PropertyMetadata(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));


        public DateTimePicker()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.ConfirmCommand, (s, e) =>
            {
                string date = string.Empty, time = string.Empty;
                if (_calendar != null)
                {
                    if (_calendar.SelectedDate.HasValue)
                        date = _calendar.SelectedDate.Value.ToString("yyyy-MM-dd ");
                    else
                        date = _calendar.DisplayDate.ToString("yyyy-MM-dd ");
                }

                if (_clock != null)
                    time = _clock.CurrentTime;

                Now = date + time;

                if (_popup != null)
                    _popup.IsOpen = false;
            }));
        }

        public override void OnApplyTemplate()
        {
            if (_calendar != null)
            {
                _calendar.PreviewMouseLeftButtonUp -= Calendar_PreviewMouseLeftButtonUp;
            }

            base.OnApplyTemplate();

            _calendar = GetTemplateChild(ElementCalendar) as Calendar;
            _clock = GetTemplateChild(ElementClock) as Clock;
            _popup = GetTemplateChild(ElementPopup) as Popup;

            if (_calendar != null)
            {
                _calendar.PreviewMouseLeftButtonUp += Calendar_PreviewMouseLeftButtonUp;
            }
        }

        private void Calendar_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.Captured is CalendarItem)
            {
                Mouse.Capture(null);
            }
        }
    }
}
