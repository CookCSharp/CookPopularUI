/*
 *Description: StrokeTextBlock
 *Author: Chance.zheng
 *Creat Time: 2024/1/31 17:25:14
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 带有笔画的文本控件
    /// </summary>
    public class StrokeTextBlock : Control
    {
        private ItemsControl _itemsControl;
        private ObservableCollection<TextBlock> StrokeTextBlocks = new ObservableCollection<TextBlock>();


        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(StrokeTextBlock), new PropertyMetadata(new PropertyChangedCallback(OnTextBlockPropertyChanged)));


        public TextDecorationCollection TextDecorations
        {
            get { return (TextDecorationCollection)GetValue(TextDecorationsProperty); }
            set { SetValue(TextDecorationsProperty, value); }
        }
        public static readonly DependencyProperty TextDecorationsProperty =
            DependencyProperty.Register(nameof(TextDecorations), typeof(TextDecorationCollection), typeof(StrokeTextBlock), new PropertyMetadata(new PropertyChangedCallback(OnTextBlockPropertyChanged)));


        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register(nameof(TextWrapping), typeof(TextWrapping), typeof(StrokeTextBlock), new PropertyMetadata(new PropertyChangedCallback(OnTextBlockPropertyChanged)));


        public double LineHeight
        {
            get { return (double)GetValue(LineHeightProperty); }
            set { SetValue(LineHeightProperty, value); }
        }
        public static readonly DependencyProperty LineHeightProperty =
            DependencyProperty.Register(nameof(LineHeight), typeof(double), typeof(StrokeTextBlock), new PropertyMetadata(new PropertyChangedCallback(OnTextBlockPropertyChanged)));


        public double StrokeOpacity
        {
            get { return (double)GetValue(StrokeOpacityProperty); }
            set { SetValue(StrokeOpacityProperty, value); }
        }
        public static readonly DependencyProperty StrokeOpacityProperty =
            DependencyProperty.Register(nameof(StrokeOpacity), typeof(double), typeof(StrokeTextBlock), null);


        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, Math.Ceiling(value));
        }
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(StrokeTextBlock), new PropertyMetadata(new PropertyChangedCallback(StrokeThickness_Changed)));
        private static void StrokeThickness_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is StrokeTextBlock strokeTextBlock)
                strokeTextBlock.UpdateStrokeThickness();
        }


        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(StrokeTextBlock), new PropertyMetadata(new PropertyChangedCallback(Stroke_Changed)));
        private static void Stroke_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is StrokeTextBlock strokeTextBlock)
                strokeTextBlock.UpdateStroke();
        }


        private static void OnTextBlockPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is StrokeTextBlock strokeTextBlock)
                strokeTextBlock.UpdateTextBlocks();
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _itemsControl = (ItemsControl)GetTemplateChild("PART_ItemsControl");

            if (_itemsControl != null)
            {
                _itemsControl.ItemsSource = StrokeTextBlocks;
            }
        }

        private void UpdateTextBlocks()
        {
            foreach (TextBlock textBlock in StrokeTextBlocks)
            {
                textBlock.HorizontalAlignment = HorizontalAlignment;

                if (LineHeight > 0)
                {
                    textBlock.LineHeight = LineHeight;
                }

                textBlock.Text = Text;
                textBlock.TextWrapping = TextWrapping;
                textBlock.Foreground = Stroke;
            }
        }

        private void AddStrokeTextBlock()
        {
            TextBlock tb = new TextBlock();
            tb.RenderTransformOrigin = new Point(0.5, 0.5);
            tb.HorizontalAlignment = HorizontalAlignment;

            if (LineHeight > 0)
            {
                tb.LineHeight = LineHeight;
            }

            tb.Text = Text;
            tb.TextWrapping = TextWrapping;
            tb.Foreground = Stroke;
            StrokeTextBlocks.Add(tb);
        }

        private void UpdateStrokeThickness()
        {
            if (StrokeThickness == 0)
            {
                for (int i = 0; i < StrokeTextBlocks.Count; i++)
                {
                    StrokeTextBlocks[i].Opacity = 0;
                }
            }
            else
            {
                int strokeTextBlockCount = (int)Math.Ceiling(360 / Math.Floor(45.0 / (StrokeThickness / 2)));

                if (StrokeTextBlocks.Count < strokeTextBlockCount)
                {
                    for (int i = 0; i <= strokeTextBlockCount; i++)
                    {
                        AddStrokeTextBlock();
                    }
                }
                else if (StrokeTextBlocks.Count > strokeTextBlockCount)
                {
                    for (int i = strokeTextBlockCount; i < StrokeTextBlocks.Count; i++)
                    {
                        StrokeTextBlocks[i].Opacity = 0;
                    }
                }

                int textBlockIndex = 0;
                for (int a = 0; a < 360; a = a + (int)Math.Floor(45.0 / (this.StrokeThickness / 2)))
                {
                    double x = Math.Cos(a * (Math.PI / 180)) * this.StrokeThickness;
                    double y = Math.Tan(a * (Math.PI / 180)) * x;

                    StrokeTextBlocks[textBlockIndex].Opacity = 1;
                    StrokeTextBlocks[textBlockIndex].RenderTransform = new TranslateTransform()
                    {
                        X = x,
                        Y = y
                    };

                    textBlockIndex++;
                }
            }
        }

        private void UpdateStroke()
        {
            foreach (TextBlock textBlock in StrokeTextBlocks)
            {
                textBlock.Foreground = Stroke;
            }
        }
    }
}
