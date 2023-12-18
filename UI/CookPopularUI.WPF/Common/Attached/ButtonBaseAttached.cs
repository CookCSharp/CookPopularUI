/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：提供ButtonBase的附加属性
 * Author： Chance_写代码的厨子
 * Create Time：2021-02-20 11:05:19
 */


using CookPopularToolkit;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace CookPopularUI.WPF
{
    /// <summary>
    /// 提供<see cref="ButtonBase"/>的附加属性
    /// </summary>
    public class ButtonBaseAttached
    {
        public static bool GetIsShowRipple(DependencyObject obj) => (bool)obj.GetValue(IsShowRippleProperty);
        public static void SetIsShowRipple(DependencyObject obj, bool value) => obj.SetValue(IsShowRippleProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsShowRippleProperty"/>是否显示按钮点击的波纹效果
        /// </summary>
        public static readonly DependencyProperty IsShowRippleProperty =
            DependencyProperty.RegisterAttached("IsShowRipple", typeof(bool), typeof(ButtonBaseAttached), new PropertyMetadata(ValueBoxes.TrueBox));

        public static bool GetIsAutoStart(DependencyObject obj) => (bool)obj.GetValue(IsAutoStartProperty);
        public static void SetIsAutoStart(DependencyObject obj, bool value) => obj.SetValue(IsAutoStartProperty, ValueBoxes.BooleanBox(value));
        public static readonly DependencyProperty IsAutoStartProperty =
            DependencyProperty.RegisterAttached("IsAutoStart", typeof(bool), typeof(ButtonBaseAttached), new PropertyMetadata(ValueBoxes.FalseBox));


        public static Uri GetGifSource(DependencyObject obj) => (Uri)obj.GetValue(GifSourceProperty);
        public static void SetGifSource(DependencyObject obj, Uri value) => obj.SetValue(GifSourceProperty, value);
        public static readonly DependencyProperty GifSourceProperty =
            DependencyProperty.RegisterAttached("GifSource", typeof(Uri), typeof(ButtonBaseAttached), new PropertyMetadata(default(Uri)));


        public static Stream GetGifStream(DependencyObject obj) => (Stream)obj.GetValue(GifStreamProperty);
        public static void SetGifStream(DependencyObject obj, Stream value) => obj.SetValue(GifStreamProperty, value);
        /// <summary>
        /// 提供<see cref="Stream"/>的Gif图像
        /// </summary>
        public static readonly DependencyProperty GifStreamProperty =
            DependencyProperty.RegisterAttached("GifStream", typeof(Stream), typeof(ButtonBaseAttached), new PropertyMetadata(default(Stream)));


        public static double GetGifWidth(DependencyObject obj) => (double)obj.GetValue(GifWidthProperty);
        public static void SetGifWidth(DependencyObject obj, double value) => obj.SetValue(GifWidthProperty, value);
        public static readonly DependencyProperty GifWidthProperty =
            DependencyProperty.RegisterAttached("GifWidth", typeof(double), typeof(ButtonBaseAttached), new PropertyMetadata(ValueBoxes.Double0Box));


        public static double GetGifHeight(DependencyObject obj) => (double)obj.GetValue(GifHeightProperty);
        public static void SetGifHeight(DependencyObject obj, double value) => obj.SetValue(GifHeightProperty, value);
        public static readonly DependencyProperty GifHeightProperty =
            DependencyProperty.RegisterAttached("GifHeight", typeof(double), typeof(ButtonBaseAttached), new PropertyMetadata(ValueBoxes.Double0Box));


        public static ImageSource GetImageSource(DependencyObject obj) => (ImageSource)obj.GetValue(ImageSourceProperty);
        public static void SetImageSource(DependencyObject obj, ImageSource value) => obj.SetValue(ImageSourceProperty, value);
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.RegisterAttached("ImageSource", typeof(ImageSource), typeof(ButtonBaseAttached), new PropertyMetadata(default(ImageSource)));


        public static double GetImageWidth(DependencyObject obj) => (double)obj.GetValue(ImageWidthProperty);
        public static void SetImageWidth(DependencyObject obj, double value) => obj.SetValue(ImageWidthProperty, value);
        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.RegisterAttached("ImageWidth", typeof(double), typeof(ButtonBaseAttached), new PropertyMetadata(ValueBoxes.Double0Box));


        public static double GetImageHeight(DependencyObject obj) => (double)obj.GetValue(ImageHeightProperty);
        public static void SetImageHeight(DependencyObject obj, double value) => obj.SetValue(ImageHeightProperty, value);
        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.RegisterAttached("ImageHeight", typeof(double), typeof(ButtonBaseAttached), new PropertyMetadata(ValueBoxes.Double0Box));
    }
}
