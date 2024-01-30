﻿/*
 *Description: VirtualizingGridView
 *Author: Chance.zheng
 *Creat Time: 2023/12/7 15:08:13
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Windows.Input;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// Simple control that displays a gird of items. Depending on the orientation, the items are either stacked horizontally or vertically 
    /// until the items are wrapped to the next row or column. The control is using virtualization to support large amount of items.
    /// <p class="note">In order to work properly all items must have the same size.</p>
    /// </summary>
    public class VirtualizingGridView : ListView
    {
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(VirtualizingGridView), new FrameworkPropertyMetadata(Orientation.Horizontal));

        public static readonly DependencyProperty SpacingModeProperty = DependencyProperty.Register(nameof(SpacingMode), typeof(SpacingMode), typeof(VirtualizingGridView), new FrameworkPropertyMetadata(SpacingMode.Uniform));

        public static readonly DependencyProperty StretchItemsProperty = DependencyProperty.Register(nameof(StretchItems), typeof(bool), typeof(VirtualizingGridView), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsWrappingKeyboardNavigationEnabledProperty = DependencyProperty.Register(nameof(IsWrappingKeyboardNavigationEnabled), typeof(bool), typeof(VirtualizingGridView), new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value that specifies the orientation in which items are arranged. The default value is <see cref="Orientation.Horizontal"/>.
        /// </summary>
        public Orientation Orientation { get => (Orientation)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

        /// <summary>
        /// Gets or sets the spacing mode used when arranging the items. The default value is <see cref="SpacingMode.Uniform"/>.
        /// </summary>
        public SpacingMode SpacingMode { get => (SpacingMode)GetValue(SpacingModeProperty); set => SetValue(SpacingModeProperty, value); }

        /// <summary>
        /// Gets or sets a value that specifies if the items get stretched to fill up remaining space. The default value is false.
        /// </summary>
        /// <remarks>
        /// The MaxWidth and MaxHeight properties of the ItemContainerStyle can be used to limit the stretching. 
        /// In this case the use of the remaining space will be determined by the SpacingMode property. 
        /// </remarks>
        public bool StretchItems { get => (bool)GetValue(StretchItemsProperty); set => SetValue(StretchItemsProperty, value); }

        /// <summary>
        /// Enables a improved wrapping keyboard navigation. The default value is false.
        /// </summary>
        public bool IsWrappingKeyboardNavigationEnabled { get => (bool)GetValue(IsWrappingKeyboardNavigationEnabledProperty); set => SetValue(IsWrappingKeyboardNavigationEnabledProperty, value); }

        static VirtualizingGridView()
        {
            ItemContainerStyleProperty.OverrideMetadata(typeof(VirtualizingGridView), new FrameworkPropertyMetadata(new Style
            {
                Setters = {
                    new Setter {
                        Property = MarginProperty,
                        Value = new Thickness(0)
                    },
                    new Setter {
                        Property = PaddingProperty,
                        Value = new Thickness(4)
                    },
                    new Setter {
                        Property = HorizontalContentAlignmentProperty,
                        Value = HorizontalAlignment.Stretch
                    },
                     new Setter {
                        Property = VerticalContentAlignmentProperty,
                        Value = VerticalAlignment.Stretch
                    }
                }
            }));
        }

        public VirtualizingGridView()
        {
            var factory = new FrameworkElementFactory(typeof(VirtualizingWrapPanel));
            factory.SetBinding(VirtualizingWrapPanel.OrientationProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(Orientation)),
                Mode = BindingMode.OneWay
            });
            factory.SetBinding(VirtualizingWrapPanel.SpacingModeProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(SpacingMode)),
                Mode = BindingMode.OneWay
            });
            factory.SetBinding(VirtualizingWrapPanel.StretchItemsProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(StretchItems)),
                Mode = BindingMode.OneWay
            });
            ItemsPanel = new ItemsPanelTemplate(factory);

            VirtualizingPanel.SetCacheLengthUnit(this, VirtualizationCacheLengthUnit.Page);
            VirtualizingPanel.SetCacheLength(this, new VirtualizationCacheLength(1));

            VirtualizingPanel.SetIsVirtualizingWhenGrouping(this, true);

            PreviewKeyDown += VirtualizingGridView_PreviewKeyDown;
        }

        private void VirtualizingGridView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!IsWrappingKeyboardNavigationEnabled) return;

            var gridView = (VirtualizingGridView)sender;

            var currentItem = gridView.ItemContainerGenerator.ItemFromContainer((DependencyObject)Keyboard.FocusedElement);

            int targetIndex;
            if (Orientation == Orientation.Horizontal)
            {
                switch (e.Key)
                {
                    case Key.Left:
                        targetIndex = gridView.Items.IndexOf(currentItem) - 1;
                        break;
                    case Key.Right:
                        targetIndex = gridView.Items.IndexOf(currentItem) + 1;
                        break;
                    default:
                        return;
                }
            }
            else
            {
                switch (e.Key)
                {
                    case Key.Up:
                        targetIndex = gridView.Items.IndexOf(currentItem) - 1;
                        break;
                    case Key.Down:
                        targetIndex = gridView.Items.IndexOf(currentItem) + 1;
                        break;
                    default:
                        return;
                }
            }

            if (targetIndex >= 0 && targetIndex < gridView.Items.Count)
            {
                ((UIElement)gridView.ItemContainerGenerator.ContainerFromIndex(targetIndex)).Focus();
            }

            e.Handled = true;
        }
    }
}
