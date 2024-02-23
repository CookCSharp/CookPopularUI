/*
 *Description: Watermark
 *Author: Chance.zheng
 *Creat Time: 2024/2/1 15:56:06
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Reflection.Metadata;
using System.Windows.Media;
using CookPopularToolkit;
using System.Windows.Markup.Primitives;
using System.Windows.Markup;
using System.Windows.Shapes;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 水印
    /// </summary>
    [TemplatePart(Name = ElementRoot, Type = typeof(Border))]
    [ContentProperty(nameof(Content))]
    public class Watermark : Control
    {
        private const string ElementRoot = "PART_Root";
        private Border _borderRoot;


        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Content"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(Watermark), new PropertyMetadata(default));


        public object Mark
        {
            get => GetValue(MarkProperty);
            set => SetValue(MarkProperty, value);
        }
        public static readonly DependencyProperty MarkProperty =
            DependencyProperty.Register(nameof(Mark), typeof(object), typeof(Watermark), new FrameworkPropertyMetadata(default, FrameworkPropertyMetadataOptions.AffectsRender));


        public double MarkWidth
        {
            get => (double)GetValue(MarkWidthProperty);
            set => SetValue(MarkWidthProperty, value);
        }
        /// <summary>
        /// 提供<see cref="MarkWidth"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty MarkWidthProperty =
            DependencyProperty.Register(nameof(MarkWidth), typeof(double), typeof(Watermark), new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsRender));


        public double MarkHeight
        {
            get => (double)GetValue(MarkHeightProperty);
            set => SetValue(MarkHeightProperty, value);
        }
        /// <summary>
        /// 提供<see cref="MarkHeight"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty MarkHeightProperty =
            DependencyProperty.Register(nameof(MarkHeight), typeof(double), typeof(Watermark), new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsRender));


        public Brush MarkBrush
        {
            get => (Brush)GetValue(MarkBrushProperty);
            set => SetValue(MarkBrushProperty, value);
        }
        /// <summary>
        /// 提供<see cref="MarkBrush"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty MarkBrushProperty =
            DependencyProperty.Register(nameof(MarkBrush), typeof(Brush), typeof(Watermark), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));


        public double MarkOpactity
        {
            get => (double)GetValue(MarkOpactityProperty);
            set => SetValue(MarkOpactityProperty, value);
        }
        /// <summary>
        /// 提供<see cref="MarkOpactity"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty MarkOpactityProperty =
            DependencyProperty.Register(nameof(MarkOpactity), typeof(double), typeof(Watermark), new FrameworkPropertyMetadata(0.2, FrameworkPropertyMetadataOptions.AffectsRender));


        public Thickness MarkMargin
        {
            get => (Thickness)GetValue(MarkMarginProperty);
            set => SetValue(MarkMarginProperty, value);
        }
        /// <summary>
        /// 提供<see cref="MarkMargin"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty MarkMarginProperty =
            DependencyProperty.Register(nameof(MarkMargin), typeof(Thickness), typeof(Watermark), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsRender));


        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(Watermark), new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsRender));


        public bool IsAutoSize
        {
            get => (bool)GetValue(IsAutoSizeProperty);
            set => SetValue(IsAutoSizeProperty, value.BooleanBox());
        }
        /// <summary>
        /// 提供<see cref="IsAutoSize"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsAutoSizeProperty =
            DependencyProperty.Register(nameof(IsAutoSize), typeof(bool), typeof(Watermark), new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.AffectsRender));


        public override void OnApplyTemplate()
        {           
            _borderRoot = (Border)GetTemplateChild(ElementRoot);
        }

        protected override void OnRender(DrawingContext drawingContext) => Update();

        private void Update()
        {
            var presenter = new ContentPresenter();
            if (Mark is Geometry geometry)
            {
                presenter.Content = new Path
                {
                    Width = MarkWidth,
                    Height = MarkHeight,
                    Fill = MarkBrush,
                    Stretch = Stretch.Uniform,
                    Data = geometry
                };
            }
            else if (Mark is string str)
            {
                presenter.Content = new TextBlock
                {
                    Text = str,
                    FontSize = FontSize,
                    Foreground = MarkBrush
                };
            }
            else
            {
                presenter.Content = Mark;
            }

            Size markSize;
            if (IsAutoSize)
            {
                presenter.Measure(new Size(double.MaxValue, double.MaxValue));
                markSize = presenter.DesiredSize;
            }
            else
            {
                markSize = new Size(MarkWidth, MarkHeight);
            }

            var brush = new DrawingBrush
            {
                ViewportUnits = BrushMappingMode.Absolute,
                Stretch = Stretch.Uniform,
                TileMode = TileMode.Tile,
                Transform = new RotateTransform(Angle),
                Opacity = MarkOpactity,
                Drawing = new GeometryDrawing
                {
                    Brush = new VisualBrush(new Border
                    {
                        Background = Brushes.Transparent,
                        Padding = MarkMargin,
                        Child = presenter
                    }),
                    Geometry = new RectangleGeometry(new Rect(markSize))
                },
                Viewport = new Rect(markSize)
            };
            RenderOptions.SetCacheInvalidationThresholdMinimum(brush, 0.5);
            RenderOptions.SetCacheInvalidationThresholdMaximum(brush, 2);
            RenderOptions.SetCachingHint(brush, CachingHint.Cache);

            if (_borderRoot != null)
            {
                _borderRoot.Background = brush;
            }
        }
    }
}
