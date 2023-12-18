/*
 *Description: AdornerElement
 *Author: Chance.zheng
 *Creat Time: 2023/9/8 11:40:27
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CookPopularUI.WPF.Controls
{
    public abstract class AdornerElement : Control, IDisposable
    {
        protected FrameworkElement? ElementTarget { get; set; }

        [Bindable(true), Category("Layout")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FrameworkElement Target
        {
            get => (FrameworkElement)GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register(nameof(Target), typeof(FrameworkElement), typeof(AdornerElement), new PropertyMetadata(default(FrameworkElement), OnTargetChanged));

        private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (AdornerElement)d;
            ctl.OnTargetChanged(ctl.ElementTarget, false);
            ctl.OnTargetChanged((FrameworkElement)e.NewValue, true);
        }

        protected virtual void OnTargetChanged(FrameworkElement? element, bool isNew)
        {
            if (element == null) return;

            if (!isNew)
            {
                element.Unloaded -= TargetElement_Unloaded;
                ElementTarget = null;
            }
            else
            {
                element.Unloaded += TargetElement_Unloaded;
                ElementTarget = element;
            }
        }

        private void TargetElement_Unloaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                element.Unloaded -= TargetElement_Unloaded;
                Dispose();
            }
        }


        public static void SetInstance(DependencyObject element, AdornerElement value) => element.SetValue(InstanceProperty, value);
        public static AdornerElement GetInstance(DependencyObject element) => (AdornerElement)element.GetValue(InstanceProperty);
        public static readonly DependencyProperty InstanceProperty =
            DependencyProperty.RegisterAttached("Instance", typeof(AdornerElement), typeof(AdornerElement), new PropertyMetadata(default(AdornerElement), OnInstanceChanged));

        private static void OnInstanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FrameworkElement target) return;
            var element = (AdornerElement)e.NewValue;
            element.OnInstanceChanged(target);
        }

        protected virtual void OnInstanceChanged(FrameworkElement target) => Target = target;


        public static void SetIsInstance(DependencyObject element, bool value) => element.SetValue(IsInstanceProperty, value.BooleanBox());
        public static bool GetIsInstance(DependencyObject element) => (bool)element.GetValue(IsInstanceProperty);
        public static readonly DependencyProperty IsInstanceProperty =
            DependencyProperty.RegisterAttached("IsInstance", typeof(bool), typeof(AdornerElement), new PropertyMetadata(ValueBoxes.TrueBox));


        protected abstract void Dispose();

        void IDisposable.Dispose() => Dispose();
    }
}
