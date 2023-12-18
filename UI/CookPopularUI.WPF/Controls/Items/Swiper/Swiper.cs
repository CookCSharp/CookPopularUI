/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：Swiper
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-25 15:06:06
 */


using CookPopularToolkit.Windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using ValueBoxes = CookPopularToolkit.ValueBoxes;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 滑块视图容器
    /// </summary>
    [TemplatePart(Name = LastViewButton, Type = typeof(System.Windows.Controls.Button))]
    [TemplatePart(Name = NextViewButton, Type = typeof(System.Windows.Controls.Button))]
    [TemplatePart(Name = ContentViewTranslate, Type = typeof(TranslateTransform))]
    [TemplatePart(Name = ContentView, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = DotsPanelContainer, Type = typeof(StackPanel))]
    public class Swiper : ItemsControl
    {
        private const string LastViewButton = "PART_LastView";
        private const string NextViewButton = "PART_NextView";
        private const string ContentView = "PART_Content";
        private const string ContentViewTranslate = "PART_Translate";
        private const string DotsPanelContainer = "PART_Panel";

        private static readonly Duration DefaultDuration = new Duration(TimeSpan.FromMilliseconds(500));
        public static readonly ICommand LastViewCommand = new RoutedCommand(nameof(LastViewCommand), typeof(Swiper));
        public static readonly ICommand NextViewCommand = new RoutedCommand(nameof(NextViewCommand), typeof(Swiper));

        private Button _lastButton;
        private Button _nextButton;
        private ContentPresenter contentView;
        private TranslateTransform translateTransform;
        private StackPanel dotsPanel;
        private DispatcherTimer? _autoPlayTimer;

        static Swiper()
        {
            CommandManager.RegisterClassCommandBinding(typeof(Swiper), new CommandBinding(LastViewCommand, Executed));
            CommandManager.RegisterClassCommandBinding(typeof(Swiper), new CommandBinding(NextViewCommand, Executed));
        }

        private static void Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var swiper = (Swiper)sender;
            if (swiper.IsCyclePlay) //循环播放
            {
                if (e.Command == LastViewCommand)
                    swiper.CurrentIndex -= 1;
                else if (e.Command == NextViewCommand)
                    swiper.CurrentIndex += 1;
            }
            else  //不循环播放，到末端就停止
            {
                if (e.Command == LastViewCommand)
                    swiper.CurrentIndex -= 1;
                else if (e.Command == NextViewCommand)
                    swiper.CurrentIndex += 1;
            }
        }

        public Swiper()
        {
            this.Loaded += (s, e) =>
            {
                contentView = (ContentPresenter)GetTemplateChild(ContentView);
                _lastButton = (Button)GetTemplateChild(LastViewButton);
                _nextButton = (Button)GetTemplateChild(NextViewButton);
                dotsPanel = (StackPanel)GetTemplateChild(DotsPanelContainer);

                translateTransform = new TranslateTransform();
                contentView.RenderTransform = translateTransform;

                _lastButton.Click += (s, e) => translateTransform.BeginAnimation(TranslateTransform.XProperty, CreateAnimation(-300D, Duration));
                _nextButton.Click += (s, e) => translateTransform.BeginAnimation(TranslateTransform.XProperty, CreateAnimation(300D, Duration));

                if (!IsCyclePlay)
                {
                    _lastButton.IsEnabled = false;
                    if (this.Items.Count.Equals(0))
                        _nextButton.IsEnabled = false;
                }

                for (int i = 0; i < Items.Count; i++)
                {
                    dotsPanel.Children.Add(CreateDot());
                }
                ((RadioButton)dotsPanel.Children[0]).IsChecked = true; //与CurrentIndex对应
            };

            this.Unloaded += (s, e) =>
            {
                dotsPanel?.Children.Clear();
            };
        }

        private DoubleAnimation CreateAnimation(double from, Duration duration)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = from;
            doubleAnimation.To = 0;
            doubleAnimation.Duration = duration;

            return doubleAnimation;
        }

        private RadioButton CreateDot()
        {
            RadioButton rb = new RadioButton();
            rb.Content = null;
            rb.Width = 20;
            rb.Height = 20;
            rb.BorderBrush = IndicatorDotBrush;
            rb.Style = ResourceHelper.GetResource<Style>("RadioButtonFillFullyStyle");
            rb.Margin = new Thickness(10, 0, 10, 0);
            rb.Checked += (s, e) =>
            {
                rb.Background = IndicatorActiveDotBrush;
                CurrentIndex = dotsPanel.Children.IndexOf(rb);
            };
            rb.Unchecked += (s, e) => rb.Background = Brushes.Transparent;

            return rb;
        }


        /// <summary>
        /// 当前显示视图的索引
        /// </summary>
        public int CurrentIndex
        {
            get { return (int)GetValue(CurrentIndexProperty); }
            set { SetValue(CurrentIndexProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="CurrentIndex"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty CurrentIndexProperty =
            DependencyProperty.Register("CurrentIndex", typeof(int), typeof(Swiper),
                new PropertyMetadata(ValueBoxes.IntegerMinus1Box, OnCurrentIndexChanged));

        private static void OnCurrentIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var swiper = d as Swiper;
            if (swiper != null && swiper.IsLoaded)
            {
                if (swiper._lastButton == null || swiper._nextButton == null) return;

                if (swiper.CurrentIndex.Equals(-1))
                    swiper.CurrentIndex = swiper.Items.Count - 1;
                else if (swiper.CurrentIndex.Equals(swiper.Items.Count))
                    swiper.CurrentIndex = 0;

                swiper._lastButton.IsEnabled = true;
                swiper._nextButton.IsEnabled = true;

                if (!swiper.IsCyclePlay)
                {
                    if (swiper.CurrentIndex <= 0)
                        swiper._lastButton.IsEnabled = false;
                    else if (swiper.CurrentIndex >= swiper.Items.Count - 1)
                        swiper._nextButton.IsEnabled = false;
                }

                swiper.CurrentItem = swiper.Items[swiper.CurrentIndex];
                ((RadioButton)swiper.dotsPanel.Children[swiper.CurrentIndex]).IsChecked = true;
            }
        }


        /// <summary>
        /// 当前显示的视图
        /// </summary>
        public object CurrentItem
        {
            get { return GetValue(CurrentItemProperty); }
            set { SetValue(CurrentItemProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="CurrentItem"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty CurrentItemProperty =
            DependencyProperty.Register(nameof(CurrentItem), typeof(object), typeof(Swiper), new PropertyMetadata(default));


        /// <summary>
        /// 是否循环播放
        /// </summary>
        public bool IsCyclePlay
        {
            get { return (bool)GetValue(IsCyclePlayProperty); }
            set { SetValue(IsCyclePlayProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="IsCyclePlay"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsCyclePlayProperty =
            DependencyProperty.Register(nameof(IsCyclePlay), typeof(bool), typeof(Swiper), new PropertyMetadata(ValueBoxes.TrueBox));


        /// <summary>
        /// 是否显示面板指示点
        /// </summary>
        public bool IsShowIndicatorDots
        {
            get { return (bool)GetValue(IsShowIndicatorDotsProperty); }
            set { SetValue(IsShowIndicatorDotsProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// 标识<see cref="IsShowIndicatorDots"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsShowIndicatorDotsProperty =
            DependencyProperty.Register(nameof(IsShowIndicatorDots), typeof(bool), typeof(Swiper), new PropertyMetadata(ValueBoxes.TrueBox));


        /// <summary>
        /// 指示点颜色
        /// </summary>
        public Brush IndicatorDotBrush
        {
            get { return (Brush)GetValue(IndicatorDotBrushProperty); }
            set { SetValue(IndicatorDotBrushProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="IndicatorDotBrush"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IndicatorDotBrushProperty =
            DependencyProperty.Register(nameof(IndicatorDotBrush), typeof(Brush), typeof(Swiper), new PropertyMetadata(default(Brush)));


        /// <summary>
        /// 选中点的颜色
        /// </summary>
        public Brush IndicatorActiveDotBrush
        {
            get { return (Brush)GetValue(IndicatorActiveDotBrushProperty); }
            set { SetValue(IndicatorActiveDotBrushProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="IndicatorActiveDotBrush"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IndicatorActiveDotBrushProperty =
            DependencyProperty.Register(nameof(IndicatorActiveDotBrush), typeof(Brush), typeof(Swiper), new PropertyMetadata(default(Brush)));


        /// <summary>
        /// 是否自动切换
        /// </summary>
        public bool IsAutoPlay
        {
            get { return (bool)GetValue(IsAutoPlayProperty); }
            set { SetValue(IsAutoPlayProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="IsAutoPlay"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsAutoPlayProperty =
            DependencyProperty.Register(nameof(IsAutoPlay), typeof(bool), typeof(Swiper),
                new PropertyMetadata(ValueBoxes.FalseBox, OnIsAutoPlayChanged));

        private static void OnIsAutoPlayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var swiper = d as Swiper;
            if (swiper != null)
            {
                swiper._autoPlayTimer = null;
                swiper._autoPlayTimer = new DispatcherTimer(DispatcherPriority.Normal);
                swiper._autoPlayTimer.Interval = TimeSpan.FromSeconds(swiper.Interval);
                swiper._autoPlayTimer.Tick += (s, e) =>
                {
                    swiper.CurrentIndex += 1;
                    swiper.translateTransform?.BeginAnimation(TranslateTransform.XProperty, swiper.CreateAnimation(300D, new Duration(TimeSpan.FromSeconds(swiper.Interval))));
                };
                swiper._autoPlayTimer.IsEnabled = (bool)e.NewValue;
            }
        }


        /// <summary>
        /// 自动切换间隔时长(s)
        /// </summary>
        public double Interval
        {
            get { return (double)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="Interval"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register(nameof(Interval), typeof(double), typeof(Swiper),
                new PropertyMetadata(ValueBoxes.Double1Box, (d, e) => ((Swiper)d)._autoPlayTimer!.Interval = TimeSpan.FromSeconds(((Swiper)d).Interval * 1.1D)));


        /// <summary>
        /// 滑动动画时长
        /// </summary>
        public Duration Duration
        {
            get { return (Duration)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="Duration"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register(nameof(Duration), typeof(Duration), typeof(Swiper), new PropertyMetadata(DefaultDuration));
    }
}