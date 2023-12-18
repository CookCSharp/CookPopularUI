/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：Carousel
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-27 17:02:09
 */


using CookPopularToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 轮播器
    /// </summary>
    [TemplatePart(Name = ImageCanvas, Type = typeof(Canvas))]
    [Localizability(LocalizationCategory.None)]
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(CarouselItem))]
    public class Carousel : ItemsControl
    {
        private const double AverageAngle = 360D / 6;
        private const double Radius = 300D;
        private const double CenterAngle = 180D;
        private const string ImageCanvas = "PART_ImageCanvas";

        private double _totalAngle;
        private double _originX;
        private double _currentX;
        private double _moveDistanceX;
        private double _inertiaAngle;

        private Canvas _canvasContainer;
        private CarouselItem? _carouselItem;
        private List<CarouselItem> _carouselItems;


        public int TopMostIndex
        {
            get { return (int)GetValue(TopMostIndexProperty); }
            set { SetValue(TopMostIndexProperty, value); }
        }
        public static readonly DependencyProperty TopMostIndexProperty =
            DependencyProperty.Register(nameof(TopMostIndex), typeof(int), typeof(Carousel), new PropertyMetadata(0));


        public string SelectedExplain
        {
            get => (string)GetValue(SelectedExplainProperty);
            set => SetValue(SelectedExplainProperty, value);
        }
        /// <summary>
        /// 提供<see cref="SelectedExplain"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty SelectedExplainProperty =
            DependencyProperty.Register(nameof(SelectedExplain), typeof(string), typeof(Carousel), new PropertyMetadata(default(string)));


        public bool IsShowExplain
        {
            get => (bool)GetValue(IsShowExplainProperty);
            set => SetValue(IsShowExplainProperty, value);
        }
        /// <summary>
        /// 提供<see cref="IsShowExplain"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsShowExplainProperty =
            DependencyProperty.Register(nameof(IsShowExplain), typeof(bool), typeof(Carousel), new PropertyMetadata(ValueBoxes.TrueBox));


        static Carousel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Carousel), new FrameworkPropertyMetadata(typeof(Carousel)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _canvasContainer = (Canvas)GetTemplateChild(ImageCanvas);
            this.Loaded += CarouselControl_Loaded;
            this.Unloaded += CarouselControl_Unloaded;
        }

        private void CarouselControl_Loaded(object sender, RoutedEventArgs e)
        {
            GetElements();
            GetTopMost();

            this.MouseMove += CarouselControl_MouseMove;
            this.MouseDown += CarouselControl_MouseDown;
            this.MouseUp += CarouselControl_MouseUp;
        }

        private void CarouselControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _canvasContainer.Children.Clear();
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
        }

        private void CarouselControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _moveDistanceX = 0;
            _originX = e.GetPosition(this).X;
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
        }

        private void CarouselControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            GetTopMost();
            _carouselItem = null;
            if (_inertiaAngle != 0)
            {
                CompositionTarget.Rendering -= CompositionTarget_Rendering;
                CompositionTarget.Rendering += CompositionTarget_Rendering;
            }
        }

        private void CarouselControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _currentX = e.GetPosition(this).X;
                _moveDistanceX = _currentX - _originX;
                _inertiaAngle = 5 * _moveDistanceX;

                for (int i = 0; i < _carouselItems.Count; i++)
                {
                    _carouselItems[i].Angle += _moveDistanceX;
                }

                UpdateLocaltion();
                _originX = _currentX;
            }
        }

        //获取最上层的图片索引
        private void GetTopMost()
        {
            List<int> list = new List<int>();
            foreach (var item in _carouselItems)
            {
                list.Add(Panel.GetZIndex(item));
            }

            int index = _carouselItems.FindIndex(p => Panel.GetZIndex(p) == list.Max());
            TopMostIndex = index;

            SelectedExplain = ((CarouselItem)Items[index]).Explain;
        }

        private void CompositionTarget_Rendering(object? sender, EventArgs e)
        {
            double dIntervalDegree = _inertiaAngle * 0.1;
            for (int i = 0; i < this._carouselItems.Count; i++)
            {
                _carouselItems[i].Angle += dIntervalDegree;
            }
            UpdateLocaltion();

            _inertiaAngle -= dIntervalDegree;
            if (Math.Abs(_inertiaAngle) < 0.1)
                CompositionTarget.Rendering -= CompositionTarget_Rendering;
        }

        private void GetElements()
        {
            _totalAngle = Items.Count * AverageAngle;
            _carouselItems = new List<CarouselItem>();
            for (int i = 0; i < Items.Count; i++)
            {
                var carouselItem = (CarouselItem)Items[i];
                carouselItem.MouseLeftButtonDown += ImgItem_MouseLeftButtonDown;
                carouselItem.MouseLeftButtonUp += ImgItem_MouseLeftButtonUp;
                carouselItem.Angle = i * AverageAngle;
                _carouselItems.Add(carouselItem);
            }

            UpdateLocaltion();
        }

        private void ImgItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _carouselItem = sender as CarouselItem;
        }

        private void ImgItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_carouselItem == sender)
            {
                GetTopMost();
                _inertiaAngle = CenterAngle - _carouselItem.Angle;
                _carouselItem = null;
                if (_inertiaAngle != 0)
                    CompositionTarget.Rendering += CompositionTarget_Rendering;
                e.Handled = true;
            }
        }

        private void UpdateLocaltion()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                var carouselItem = _carouselItems[i];
                if (carouselItem.Angle - CenterAngle >= _totalAngle / 2D)
                    carouselItem.Angle -= _totalAngle;
                else if (CenterAngle - carouselItem.Angle >= _totalAngle / 2D)
                    carouselItem.Angle += _totalAngle;

                if (carouselItem.Angle >= 90D && carouselItem.Angle <= 270D)
                    SetElementVisiable(carouselItem);
                else
                    SetElementInvisiable(carouselItem);
            }
        }

        private void SetElementVisiable(CarouselItem carouselItem)
        {
            if (carouselItem == null) return;
            if (!_canvasContainer.Children.Contains(carouselItem))
                _canvasContainer.Children.Add(carouselItem);

            double rad = carouselItem.Angle / 180D * Math.PI;
            double centerX = ActualWidth / 2D;
            double centerY = ActualHeight / 2D;
            double dX = -Radius * Math.Sin(rad);
            double dScale = 0.5 - Math.Cos(rad) / 2D;
            carouselItem.ScaleX = dScale;
            carouselItem.ScaleY = dScale;
            carouselItem.X = dX + centerX - carouselItem.Width * dScale / 2D;
            carouselItem.Y = centerY - carouselItem.Height * dScale / 2D;
            int nZIndex = (int)(360 * 1000 * (0.5 - Math.Cos(rad) / 2D));
            Panel.SetZIndex(carouselItem, nZIndex);
        }

        private void SetElementInvisiable(CarouselItem carouselItem)
        {
            if (carouselItem != null && _canvasContainer.Children.Contains(carouselItem))
                _canvasContainer.Children.Remove(carouselItem);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            GetTopMost();

            int index;
            int value = e.Delta > 0 ? 1 : -1;
            if (TopMostIndex + value >= Items.Count)
                index = 0;
            else if (TopMostIndex + value < 0)
                index = Items.Count - 1;
            else
                index = TopMostIndex + value;

            _inertiaAngle = CenterAngle - ((CarouselItem)Items[index]).Angle;
            if (_inertiaAngle != 0)
                CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        protected override bool IsItemItsOwnContainerOverride(object item) => item is CarouselItem;

        protected override DependencyObject GetContainerForItemOverride() => new CarouselItem();
    }
}
