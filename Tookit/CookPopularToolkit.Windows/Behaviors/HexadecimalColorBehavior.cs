/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：HexadecimalColorBehavior
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-09 19:20:42
 */


using System.Windows;
using System.Windows.Controls;

namespace CookPopularToolkit.Windows
{
    public class HexadecimalColorBehavior : AllowableCharactersInputElementBehavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            SetRegularExpression(AssociatedObject, RegularPatterns.HexColorPatter);
        }
    }
}
