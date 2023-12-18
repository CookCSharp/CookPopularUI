/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：Tag
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-08 09:04:09
 */


using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace CookPopularUI.WPF.Controls
{
    public enum HeaderAligment
    {
        Left,
        Top,
    }

    /// <summary>
    /// 标签
    /// </summary>
    /// <remarks>由两个<see cref="ContentControl"/>组合而成的标签控件</remarks>
    [TemplatePart(Name = ElementFrameworkElement, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ElementTextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = ElementNumericUpDown, Type = typeof(NumericUpDown))]
    [DefaultProperty("Content")]
    [ContentProperty("Content")]
    [Localizability(LocalizationCategory.None)]
    public class Tag : HeaderedContentControl
    {
        private const string ElementFrameworkElement = "PART_Content";
        private const string ElementTextBox = "PART_TextBox";
        private const string ElementNumericUpDown = "PART_NumericUpDown";


        private bool IsInDesignMode
        {
            get
            {
                return (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;
            }
        }

        /// <summary>
        /// 标头位置，上或者左
        /// </summary>
        public HeaderAligment HeaderAligment
        {
            get => (HeaderAligment)GetValue(HeaderAligmentProperty);
            set => SetValue(HeaderAligmentProperty, value);
        }
        /// <summary>
        /// 提供<see cref="HeaderAligment"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty HeaderAligmentProperty =
            DependencyProperty.Register(nameof(HeaderAligment), typeof(HeaderAligment), typeof(Tag), new PropertyMetadata(default(HeaderAligment)));


        /// <summary>
        /// 标签头的宽度
        /// </summary>
        public GridLength HeaderWidth
        {
            get => (GridLength)GetValue(HeaderWidthProperty);
            set => SetValue(HeaderWidthProperty, value);
        }
        /// <summary>
        /// 提供<see cref="HeaderWidth"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty HeaderWidthProperty =
            DependencyProperty.Register(nameof(HeaderWidth), typeof(GridLength), typeof(Tag), new PropertyMetadata(GridLength.Auto, OnPropertiesChanged));


        /// <summary>
        /// 标签头的高度
        /// </summary>
        public GridLength HeaderHeight
        {
            get => (GridLength)GetValue(HeaderHeightProperty);
            set => SetValue(HeaderHeightProperty, value);
        }
        /// <summary>
        /// 提供<see cref="HeaderHeight"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty HeaderHeightProperty =
            DependencyProperty.Register(nameof(HeaderHeight), typeof(GridLength), typeof(Tag), new PropertyMetadata(GridLength.Auto, OnPropertiesChanged));


        /// <summary>
        /// 标签头的水平定位
        /// </summary>
        public HorizontalAlignment HeaderHorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(HeaderHorizontalAlignmentProperty);
            set => SetValue(HeaderHorizontalAlignmentProperty, value);
        }
        /// <summary>
        /// 提供<see cref="HeaderHorizontalAlignment"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty HeaderHorizontalAlignmentProperty =
            DependencyProperty.Register(nameof(HeaderHorizontalAlignment), typeof(HorizontalAlignment), typeof(Tag), new PropertyMetadata(HorizontalAlignment.Right, OnPropertiesChanged));


        /// <summary>
        /// 标签头的垂直定位
        /// </summary>
        public VerticalAlignment HeaderVerticalAlignment
        {
            get => (VerticalAlignment)GetValue(HeaderVerticalAlignmentProperty);
            set => SetValue(HeaderVerticalAlignmentProperty, value);
        }
        /// <summary>
        /// 提供<see cref="HeaderVerticalAlignment"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty HeaderVerticalAlignmentProperty =
            DependencyProperty.Register(nameof(HeaderVerticalAlignment), typeof(VerticalAlignment), typeof(Tag), new PropertyMetadata(default(VerticalAlignment), OnPropertiesChanged));


        /// <summary>
        /// 标签头与内容间距
        /// </summary>
        public Thickness HeaderMargin
        {
            get { return (Thickness)GetValue(HeaderMarginProperty); }
            set { SetValue(HeaderMarginProperty, value); }
        }
        /// <summary>
        /// 表示<see cref="HeaderMargin"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty HeaderMarginProperty =
            DependencyProperty.Register(nameof(HeaderMargin), typeof(Thickness), typeof(Tag), new PropertyMetadata(new Thickness(0, 0, 6, 0), OnPropertiesChanged));


        private static void OnPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Tag editing)
            {
                if (!DesignerProperties.GetIsInDesignMode(editing))
                {
                    d.SetValue(e.Property, e.NewValue);
                }
            }
        }
    }
}
