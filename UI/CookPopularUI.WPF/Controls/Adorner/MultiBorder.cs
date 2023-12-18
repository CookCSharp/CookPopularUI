/*
 *Description: MultiBorder
 *Author: Chance.zheng
 *Creat Time: 2023/9/28 14:40:37
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// <see cref="Border"/>的扩展控件
    /// </summary>
    /// <remarks>
    /// 表示可单独设置一个单边一种颜色一个<see cref="Thickness"/>值的控件
    /// </remarks>
    public class MultiBorder : Decorator
    {
        public Brush LeftBrush
        {
            get => (Brush)GetValue(LeftBrushProperty);
            set => SetValue(LeftBrushProperty, value);
        }
        /// <summary>
        /// 提供<see cref="LeftBrush"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty LeftBrushProperty =
            DependencyProperty.Register(nameof(LeftBrush), typeof(Brush), typeof(MultiBorder), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));


        public double LeftThickness
        {
            get => (double)GetValue(LeftThicknessProperty);
            set => SetValue(LeftThicknessProperty, value);
        }
        /// <summary>
        /// 提供<see cref="LeftThickness"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty LeftThicknessProperty =
            DependencyProperty.Register(nameof(LeftThickness), typeof(double), typeof(MultiBorder), new FrameworkPropertyMetadata(ValueBoxes.Double1Box, FrameworkPropertyMetadataOptions.AffectsRender));


        public Brush TopBrush
        {
            get => (Brush)GetValue(TopBrushProperty);
            set => SetValue(TopBrushProperty, value);
        }
        /// <summary>
        /// 提供<see cref="TopBrush"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty TopBrushProperty =
            DependencyProperty.Register(nameof(TopBrush), typeof(Brush), typeof(MultiBorder), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));


        public double TopThickness
        {
            get => (double)GetValue(TopThicknessProperty);
            set => SetValue(TopThicknessProperty, value);
        }
        /// <summary>
        /// 提供<see cref="TopThickness"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty TopThicknessProperty =
            DependencyProperty.Register(nameof(TopThickness), typeof(double), typeof(MultiBorder), new FrameworkPropertyMetadata(ValueBoxes.Double1Box, FrameworkPropertyMetadataOptions.AffectsRender));


        public Brush RightBrush
        {
            get => (Brush)GetValue(RightBrushProperty);
            set => SetValue(RightBrushProperty, value);
        }
        /// <summary>
        /// 提供<see cref="RightBrush"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty RightBrushProperty =
            DependencyProperty.Register(nameof(RightBrush), typeof(Brush), typeof(MultiBorder), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));


        public double RightThickness
        {
            get => (double)GetValue(RightThicknessProperty);
            set => SetValue(RightThicknessProperty, value);
        }
        /// <summary>
        /// 提供<see cref="RightThickness"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty RightThicknessProperty =
            DependencyProperty.Register(nameof(RightThickness), typeof(double), typeof(MultiBorder), new FrameworkPropertyMetadata(ValueBoxes.Double1Box, FrameworkPropertyMetadataOptions.AffectsRender));


        public Brush BottomBrush
        {
            get => (Brush)GetValue(BottomBrushProperty);
            set => SetValue(BottomBrushProperty, value);
        }
        /// <summary>
        /// 提供<see cref="BottomBrush"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty BottomBrushProperty =
            DependencyProperty.Register(nameof(BottomBrush), typeof(Brush), typeof(MultiBorder), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));


        public double BottomThickness
        {
            get => (double)GetValue(BottomThicknessProperty);
            set => SetValue(BottomThicknessProperty, value);
        }
        /// <summary>
        /// 提供<see cref="BottomThickness"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty BottomThicknessProperty =
            DependencyProperty.Register(nameof(BottomThickness), typeof(double), typeof(MultiBorder), new FrameworkPropertyMetadata(ValueBoxes.Double1Box, FrameworkPropertyMetadataOptions.AffectsRender));


        public Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Background"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register(nameof(Background), typeof(Brush), typeof(MultiBorder), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));


        public Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Padding"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty PaddingProperty =
            DependencyProperty.Register(nameof(Padding), typeof(Thickness), typeof(MultiBorder), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsRender));


        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var finalSize = new Size(arrangeSize.Width - Padding.Left - Padding.Right, arrangeSize.Height - Padding.Top - Padding.Bottom);
            Child?.Arrange(new Rect(new Point(Padding.Left, Padding.Top), finalSize));
            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext ctx = streamGeometry.Open())
            {
                ctx.BeginFigure(new Point(Padding.Left, Padding.Top), true, true);
                ctx.ArcTo(new Point(Padding.Left, Padding.Top), finalSize, 0D, false, SweepDirection.Clockwise, true, false);
                ctx.ArcTo(new Point(arrangeSize.Width - Padding.Left, Padding.Top), finalSize, 0D, false, SweepDirection.Clockwise, true, false);
                ctx.ArcTo(new Point(arrangeSize.Width - Padding.Left, arrangeSize.Height - Padding.Top), finalSize, 0D, false, SweepDirection.Clockwise, true, false);
                ctx.ArcTo(new Point(Padding.Left, arrangeSize.Height - Padding.Top), finalSize, 0D, false, SweepDirection.Clockwise, true, false);
                ctx.Close();
            }
            return finalSize;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var leftPen = GetPen("Left");
            drawingContext.DrawLine(leftPen, new Point(), new Point(0, Height));

            var topPen = GetPen("Top");
            drawingContext.DrawLine(topPen, new Point(), new Point(Width, 0));

            var rightPen = GetPen("Right");
            drawingContext.DrawLine(rightPen, new Point(Width, Height), new Point(Width, 0));

            var bottomPen = GetPen("Bottom");
            drawingContext.DrawLine(bottomPen, new Point(Width, Height), new Point(0, Height));

            drawingContext.DrawRectangle(Background, null, new Rect(0, 0, Width, Height));
        }

        private Pen GetPen(string direction)
        {
            return direction switch
            {
                "Left" => new Pen(LeftBrush, LeftThickness) { StartLineCap = PenLineCap.Round, EndLineCap = PenLineCap.Round },
                "Top" => new Pen(TopBrush, TopThickness) { StartLineCap = PenLineCap.Round, EndLineCap = PenLineCap.Round },
                "Right" => new Pen(RightBrush, RightThickness) { StartLineCap = PenLineCap.Round, EndLineCap = PenLineCap.Round },
                "Bottom" => new Pen(BottomBrush, BottomThickness) { StartLineCap = PenLineCap.Round, EndLineCap = PenLineCap.Round },
                _ => throw new NotImplementedException(),
            };
        }
    }
}
