/*
 * Description：PropertyButtonEditor 
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2022-01-10 11:34:20
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2022 All Rights Reserved.
 */


using CookPopularToolkit;
using CookPopularToolkit.Windows;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CookPopularUI.WPF.Controls
{
    public class PropertyButtonEditor : PropertyItemEditorFactory
    {
        public override FrameworkElement GetElement(PropertyItem propertyItem)
        {
            var button = new Button
            {
                IsEnabled = !propertyItem.IsReadOnly,
                Style = ResourceHelper.GetResource<Style>("ButtonOutlineStyle")!,
            };

            var commandAttribute = propertyItem?.PropertySource?.GetType()?.GetProperty(propertyItem.PropertyName)?.GetCustomAttribute<CommandAttribute>();
            if (commandAttribute != null)
            {
                var propertyInfo = commandAttribute.TargetType.GetProperty(commandAttribute.CommandName);
                CheckHelper.ArgumentNullExceptionWithMessage(propertyInfo, $"can not find command nameof {commandAttribute.CommandName} in {commandAttribute.TargetType}");
                var targetTypeInstance = Activator.CreateInstance(commandAttribute.TargetType);
                var command = propertyInfo?.GetValue(targetTypeInstance, null);

                button.Command = command != null ? (ICommand)command : null;
                button.CommandParameter = commandAttribute.Parameter;
            }

            return button;
        }

        public override DependencyProperty GetDependencyProperty() => ContentControl.ContentProperty;
    }
}
