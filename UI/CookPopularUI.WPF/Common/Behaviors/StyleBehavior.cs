/*
 *Description: StyleBehavior
 *Author: Chance.zheng
 *Creat Time: 2023/10/26 11:12:55
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookPopularUI.WPF
{
    /// <summary>
    /// 动态变更样式
    /// </summary>
    /// <![CDATA[
    /// <Button.Style>
    ///     <Style TargetType = "{x:Type Button}" >
    ///         < Setter Property="Margin" Value="10"/>
    ///         <Setter Property = "Behavior.AutoMergeStyle" Value="{x:Type Button}"/>
    ///     </Style>
    /// </Button.Style>
    /// ]]>
    public class StyleBehavior
    {
        public static Type GetAutoMergeStyle(DependencyObject obj) => (Type)obj.GetValue(AutoMergeStyleProperty);
        public static void SetAutoMergeStyle(DependencyObject obj, Type value) => obj.SetValue(AutoMergeStyleProperty, value);
        public static readonly DependencyProperty AutoMergeStyleProperty =
            DependencyProperty.RegisterAttached("AutoMergeStyle", typeof(Type), typeof(StyleBehavior), new PropertyMetadata(default(Type), OnAutoMergeStyleChanged));

        private static void OnAutoMergeStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue)
            {
                return;
            }

            FrameworkElement control = (FrameworkElement)d;
            if (control == null)
            {
                throw new NotSupportedException("AutoMergeStyle can only used in FrameworkElement");
            }

            Type type = (Type)e.NewValue;
            if (type != null)
            {
                control.SetResourceReference(BaseOnStyleProperty, type);
            }
            else
            {
                control.ClearValue(BaseOnStyleProperty);
            }
        }


        public static Style GetBaseOnStyle(DependencyObject obj) => (Style)obj.GetValue(BaseOnStyleProperty);
        public static void SetBaseOnStyle(DependencyObject obj, Style value) => obj.SetValue(BaseOnStyleProperty, value);
        public static readonly DependencyProperty BaseOnStyleProperty =
            DependencyProperty.RegisterAttached("BaseOnStyle", typeof(Style), typeof(StyleBehavior), new PropertyMetadata(default(Style), OnBaseOnStyleChanged));

        private static void OnBaseOnStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue)
            {
                return;
            }

            FrameworkElement control = (FrameworkElement)d;
            if (control == null)
            {
                throw new NotSupportedException("BaseOnStyle can only used in FrameworkElement");
            }

            Style baseOnStyle = (Style)e.NewValue;
            Style originalStyle = GetOriginalStyle(control);
            if (originalStyle == null)
            {
                originalStyle = control.Style;
                SetOriginalStyle(control, originalStyle);
            }
            Style newStyle = originalStyle;

            if (originalStyle.IsSealed)
            {
                newStyle = new Style();
                newStyle.TargetType = originalStyle.TargetType;

                //1. Merge
                Merge(newStyle, originalStyle);

                //2. Set BaseOn Style
                newStyle.BasedOn = baseOnStyle;
            }
            else
            {
                originalStyle.BasedOn = baseOnStyle;
            }

            control.Style = newStyle;
        }

        private static void Merge(Style style, Style otherStyle)
        {
            foreach (SetterBase currentSetter in otherStyle.Setters)
            {
                style.Setters.Add(currentSetter);
            }

            foreach (System.Windows.TriggerBase currentTrigger in otherStyle.Triggers)
            {
                style.Triggers.Add(currentTrigger);
            }

            foreach (object? key in otherStyle.Resources.Keys)
            {
                if (key != null)
                    style.Resources[key] = otherStyle.Resources[key];
            }
        }


        public static Style GetOriginalStyle(DependencyObject obj) => (Style)obj.GetValue(OriginalStyleProperty);
        public static void SetOriginalStyle(DependencyObject obj, Style value) => obj.SetValue(OriginalStyleProperty, value);
        public static readonly DependencyProperty OriginalStyleProperty =
            DependencyProperty.RegisterAttached("OriginalStyle", typeof(Style), typeof(StyleBehavior), new PropertyMetadata(default(Style)));
    }
}
