/*
 *Description: Interop
 *Author: Chance.zheng
 *Creat Time: 2024/1/8 14:37:03
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CookPopularUI.WPF.Draggable
{
    internal class Interop
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Win32Point
        {
            public int X;
            public int Y;
        };

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetCursorPos(ref Win32Point pos);

        public static Point GetWin32CursorPos()
        {
            Win32Point position = new Win32Point();
            GetCursorPos(ref position);
            return new Point(position.X, position.Y);
        }

        public static T? GetParent<T>(DependencyObject? element, bool isIncludeSelf = true) where T : DependencyObject
        {
            while (element != null)
            {
                if (isIncludeSelf && element is T)
                    return element as T;

                if (element is Visual)
                    element = VisualTreeHelper.GetParent(element);
                else if (element is FrameworkContentElement)
                    element = ((FrameworkContentElement)element).Parent;
                else
                    element = null;

                if (!isIncludeSelf && element is T)
                    return element as T;
            }

            return null;
        }

        public static void GetIndex(FrameworkElement dropTargetContainer, UIElement? element, Point point, ref int index)
        {
            if (dropTargetContainer == null || element == null)
                return;

            var currentXPos = point.X;
            var currentYPos = point.Y;
            var targetWidth = element.RenderSize.Width;
            var targetHeight = element.RenderSize.Height;

            if (dropTargetContainer is TabControl tab)
            {
                if (tab.TabStripPlacement == Dock.Top || tab.TabStripPlacement == Dock.Bottom)
                {
                    Get(Orientation.Horizontal, ref index);
                }
                else
                {
                    Get(Orientation.Vertical, ref index);
                }
            }
            else if (dropTargetContainer is StackPanel stackPanel)
            {
                Get(stackPanel.Orientation, ref index);
            }
            else if (dropTargetContainer is WrapPanel wrapPanel)
            {
                Get(wrapPanel.Orientation, ref index);
            }
            else if (dropTargetContainer is DockPanel dockPanel)
            {
                if (dockPanel.Children.Count <= 0)
                {
                    index = 0;
                }
                else
                {
                    var child = dockPanel.Children[0];
                    var dock = DockPanel.GetDock(child);
                    if (dock == Dock.Left || dock == Dock.Right)
                    {
                        Get(Orientation.Horizontal, ref index);
                    }
                    else
                    {
                        Get(Orientation.Vertical, ref index);
                    }
                }
            }

            void Get(Orientation orientation, ref int index)
            {
                if (orientation == Orientation.Horizontal)
                {
                    if (currentXPos > targetWidth / 2)
                    {
                        index += 1;
                    }
                }
                else
                {
                    if (currentYPos > targetHeight / 2)
                    {
                        index += 1;
                    }
                }
            }
        }
    }
}
