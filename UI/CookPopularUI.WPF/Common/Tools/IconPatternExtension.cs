/*
 *Description: IconPatternExtension
 *Author: Chance.zheng
 *Creat Time: 2023/9/12 14:59:59
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit.Windows;
using CookPopularUI.WPF.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace CookPopularUI.WPF.Tools
{
    [MarkupExtensionReturnType(typeof(IconPattern))]
    public class IconPatternExtension : MarkupDependencyObjectBase
    {
        public IconPatternExtension() { }

        public IconPatternExtension(IconPatternKind kind)
        {
            Kind = kind;
        }

        public IconPatternExtension(IconPatternKind kind, double size)
        {
            Kind = kind;
            Size = size;
        }

        public IconPatternExtension(IconPatternKind kind, double size, Brush fill)
        {
            Kind = kind;
            Size = size;
            Fill = fill;
        }

        [ConstructorArgument(nameof(Kind))]
        public IconPatternKind Kind { get; set; }

        [ConstructorArgument(nameof(Size))]
        public double? Size { get; set; }

        [ConstructorArgument(nameof(Fill))]
        public Brush Fill { get; set; }

        public override object? ProvideValue(IServiceProvider serviceProvider)
        {
            var instance = Activator.CreateInstance(typeof(IconPattern));
            if (instance == null) return default(IconPattern);

            var iconPattern = (IconPattern)instance;
            iconPattern.Kind = Kind;
            iconPattern.Fill = Fill;
            if (Size.HasValue)
            {
                iconPattern.Height = Size.Value;
                iconPattern.Width = Size.Value;
            }

            if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget provideValueTarget &&
                provideValueTarget.TargetObject is DependencyObject targetObject &&
                provideValueTarget.TargetProperty is DependencyProperty targetProperty)
            {
                targetObject?.SetValue(targetProperty, iconPattern);
            }

            return iconPattern;
        }
    }
}
