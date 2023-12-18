/*
 * Description：PropertyCheckBoxEditor 
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2023-01-10 11:34:20
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CookPopularUI.WPF.Controls
{
    public class PropertyCheckBoxEditor : PropertyItemEditorFactory
    {
        public override FrameworkElement GetElement(PropertyItem propertyItem) => new CheckBox
        {
            IsEnabled = !propertyItem.IsReadOnly,
            IsThreeState = true,
            //IsChecked = (bool)propertyItem.PropertySource.GetType().GetProperty(propertyItem.PropertyName).GetCustomAttribute<DefaultValueAttribute>().Value,
        };

        public override DependencyProperty GetDependencyProperty() => System.Windows.Controls.Primitives.ToggleButton.IsCheckedProperty;
    }
}
