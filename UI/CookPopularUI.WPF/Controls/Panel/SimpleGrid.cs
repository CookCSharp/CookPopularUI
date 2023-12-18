/*
 *Description: SimpleGrid
 *Author: Chance.zheng
 *Creat Time: 2021-02-20 10:14:22
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2021 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CookPopularUI.WPF.Controls
{
    public class SimpleGrid : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            var size = new Size();
            foreach (UIElement? child in InternalChildren)
            {
                if (child != null)
                {
                    child.Measure(availableSize);
                    size.Width = Math.Max(size.Width, child.DesiredSize.Width);
                    size.Height = Math.Max(size.Height, child.DesiredSize.Height);
                }
            }
            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement? child in InternalChildren)
            {
                child?.Arrange(new Rect(finalSize));
            }
            return finalSize;
        }
    }
}
