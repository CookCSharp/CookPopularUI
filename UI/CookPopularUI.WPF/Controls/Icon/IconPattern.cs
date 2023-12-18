/*
 *Description: IconPattern
 *Author: Chance.zheng
 *Creat Time: 2023/9/12 13:54:20
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 图标/图案
    /// </summary>
    public class IconPattern : Shape
    {
        private static readonly ResourceDictionary GeometryFigureDictionary = new ResourceDictionary { Source = new Uri(@"pack://application:,,,/CookPopularUI.WPF;component/Resources/Dictionaries/GeometryFigure.xaml", UriKind.Absolute) };
        private Geometry _originData;

        protected override Geometry DefiningGeometry => Data;

        /// <summary>
        /// 当<see cref="Kind"/>值为<see cref="IconPatternKind.Custom"/>时
        /// 获取或设置用于指定所要绘制的形状的<see cref="Geometry"/>
        /// </summary>
        public Geometry Data
        {
            get { return (Geometry)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="Data"/>依赖属性。
        /// </summary>
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(Geometry), typeof(IconPattern),
                new FrameworkPropertyMetadata(Geometry.Empty, FrameworkPropertyMetadataOptions.AffectsRender, OnDataChanged));

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IconPattern iconPattern && iconPattern.Kind == IconPatternKind.Custom)
            {
                iconPattern._originData = iconPattern.Data;
            }
        }


        /// <summary>
        /// 图标类型
        /// </summary>
        public IconPatternKind Kind
        {
            get => (IconPatternKind)GetValue(KindProperty);
            set => SetValue(KindProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Kind"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty KindProperty =
            DependencyProperty.Register(nameof(Kind), typeof(IconPatternKind), typeof(IconPattern), new PropertyMetadata(IconPatternKind.Custom, OnKindPropertyChanged));

        private static void OnKindPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IconPattern iconPattern)
            {
                if (iconPattern.Kind != IconPatternKind.Custom)
                    iconPattern.Data = (Geometry)GeometryFigureDictionary[$"{iconPattern.Kind}Geometry"];
                else
                    iconPattern.Data = iconPattern._originData;
            }
        }
    }
}
