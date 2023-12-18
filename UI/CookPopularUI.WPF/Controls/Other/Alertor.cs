/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：Alertor
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-10 09:43:57
 */


using CookPopularToolkit;
using CookPopularToolkit.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ValueBoxes = CookPopularToolkit.ValueBoxes;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 警报器
    /// </summary>
    [TemplatePart(Name = ElementAlarm, Type = (typeof(Shape)))]
    [TemplatePart(Name = ElementSwitch, Type = (typeof(SwitchButton)))]
    [Localizability(LocalizationCategory.None)]
    public class Alertor : Control
    {
        private const string ElementAlarm = "PART_Alarm";
        private const string ElementSwitch = "PART_OpenAlarm";
        private static readonly List<string> colors = new List<string>() { ResourceHelper.GetResource<Color>("PrimaryForegroundColor").ToString(), "#32AA32", "#FFA500", "#FF0000", "#800000" };

        private Storyboard _storyboard;
        private Shape _alarm;
        private SwitchButton _switch;

        /// <summary>
        /// 单次警报的时长
        /// </summary>
        /// <remarks>单位：s</remarks>
        public double Duration
        {
            get { return (double)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="Duration"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(double), typeof(Alertor), new PropertyMetadata(ValueBoxes.Double1Box));


        /// <summary>
        /// 启动警报
        /// </summary>
        public bool StartAlarm
        {
            get { return (bool)GetValue(StartAlarmProperty); }
            set { SetValue(StartAlarmProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// 提供<see cref="StartAlarm"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty StartAlarmProperty =
            DependencyProperty.Register(nameof(StartAlarm), typeof(bool), typeof(Alertor), new PropertyMetadata(ValueBoxes.FalseBox, OnStartAlarmChanged, OnCoerceStartAlarmValue));

        private static object OnCoerceStartAlarmValue(DependencyObject d, object baseValue)
        {
            if (d is Alertor alertor)
            {
                if (alertor.CurrentState == AlertorState.Normal || alertor.CurrentState == AlertorState.Success)
                    return false;
                else
                    return baseValue;
            }

            return baseValue;
        }

        private static void OnStartAlarmChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Alertor alertor)
            {
                if ((bool)e.NewValue)
                {
                    alertor.StartAlarmAnimation((int)alertor.CurrentState);
                }
                else
                {
                    alertor._storyboard.Stop();
                }
            }
        }


        /// <summary>
        /// 警报器当前状态
        /// </summary>
        public AlertorState CurrentState
        {
            get { return (AlertorState)GetValue(CurrentStateProperty); }
            set { SetValue(CurrentStateProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="CurrentState"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty CurrentStateProperty =
            DependencyProperty.Register(nameof(CurrentState), typeof(AlertorState), typeof(Alertor), new PropertyMetadata(default(AlertorState), OnCurrentStatePropertyChanged));

        private static void OnCurrentStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Alertor alertor)
            {
                var currentState = (AlertorState)e.NewValue;
                alertor._alarm.Fill = colors[(int)currentState].ToBrush();
                alertor.StartAlarmAnimation((int)currentState);
                if (currentState == AlertorState.Warning || currentState == AlertorState.Error || currentState == AlertorState.Fatal)
                {
                    alertor._switch.IsEnabled = true;
                }
                else
                {
                    alertor.StartAlarm = false;
                    alertor._switch.IsEnabled = false;
                }

                alertor.OnStateChanged((AlertorState)e.OldValue, (AlertorState)e.NewValue);
            }
        }

        private void StartAlarmAnimation(int index)
        {
            if (StartAlarm && (CurrentState == AlertorState.Warning || CurrentState == AlertorState.Error || CurrentState == AlertorState.Fatal))
            {
                ColorAnimation colorAnimation = new ColorAnimation()
                {
                    Duration = new Duration(TimeSpan.FromSeconds(Duration)),
                    From = Colors.Transparent,
                    To = (Color)ColorConverter.ConvertFromString(colors[index]),
                    RepeatBehavior = RepeatBehavior.Forever,
                };
                Storyboard.SetTarget(colorAnimation, _alarm);
                Storyboard.SetTargetProperty(colorAnimation, new PropertyPath("(Shape.Fill).(SolidColorBrush.Color)"));
                _storyboard.Children.Clear();
                _storyboard.Children.Add(colorAnimation);
                _storyboard.Begin();
            }
        }

        protected virtual void OnStateChanged(AlertorState oldValue, AlertorState newValue)
        {
            RoutedPropertyChangedEventArgs<AlertorState> arg = new RoutedPropertyChangedEventArgs<AlertorState>(oldValue, newValue, StateChangedEvent);
            this.RaiseEvent(arg);
        }

        [Description("警报器状态变化时发生")]
        public event RoutedPropertyChangedEventHandler<AlertorState> StateChanged
        {
            add { this.AddHandler(StateChangedEvent, value); }
            remove { this.RemoveHandler(StateChangedEvent, value); }
        }
        /// <summary>
        /// <see cref="StateChangedEvent"/>标识警报器状态变化时的事件
        /// </summary>
        public static readonly RoutedEvent StateChangedEvent =
            EventManager.RegisterRoutedEvent("StateChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<AlertorState>), typeof(Alertor));


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _storyboard = new Storyboard();
            _alarm = (Shape)GetTemplateChild(ElementAlarm);
            _switch = (SwitchButton)GetTemplateChild(ElementSwitch);
        }
    }

    /// <summary>
    /// 警报器状态
    /// </summary>
    public enum AlertorState
    {
        Normal,
        Success,
        Warning,
        Error,
        Fatal
    }
}
