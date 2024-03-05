/*
 *Description: CarouselItem
 *Author: Chance.zheng
 *Creat Time: 2023/12/4 10:39:17
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
    public class CarouselItem : Control
    {
        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register(nameof(X), typeof(double), typeof(CarouselItem), new UIPropertyMetadata(ValueBoxes.Double0Box));


        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register(nameof(Y), typeof(double), typeof(CarouselItem), new UIPropertyMetadata(ValueBoxes.Double0Box));


        public double ScaleX
        {
            get { return (double)GetValue(ScaleXProperty); }
            set { SetValue(ScaleXProperty, value); }
        }
        public static readonly DependencyProperty ScaleXProperty =
            DependencyProperty.Register(nameof(ScaleX), typeof(double), typeof(CarouselItem), new UIPropertyMetadata(ValueBoxes.Double1Box));


        public double ScaleY
        {
            get { return (double)GetValue(ScaleYProperty); }
            set { SetValue(ScaleYProperty, value); }
        }
        public static readonly DependencyProperty ScaleYProperty =
            DependencyProperty.Register(nameof(ScaleY), typeof(double), typeof(CarouselItem), new UIPropertyMetadata(ValueBoxes.Double1Box));


        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register(nameof(Angle), typeof(double), typeof(CarouselItem), new PropertyMetadata(ValueBoxes.Double0Box));


        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(CarouselItem), new PropertyMetadata(default(ImageSource)));


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(CarouselItem), new PropertyMetadata(default(string)));


        public string Explain
        {
            get => (string)GetValue(ExplainProperty);
            set => SetValue(ExplainProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Explain"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ExplainProperty =
            DependencyProperty.Register(nameof(Explain), typeof(string), typeof(CarouselItem), new PropertyMetadata(default(string)));


        public string WholeScenePath
        {
            get { return (string)GetValue(WholeScenePathProperty); }
            set { SetValue(WholeScenePathProperty, value); }
        }
        public static readonly DependencyProperty WholeScenePathProperty =
            DependencyProperty.Register(nameof(WholeScenePath), typeof(string), typeof(CarouselItem), new PropertyMetadata(default(string)));
    }
}
