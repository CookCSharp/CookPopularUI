/*
 *Description: TabControlAssistant
 *Author: Chance.zheng
 *Creat Time: 2023/9/28 18:47:22
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace CookPopularUI.WPF.Controls
{
    [TemplatePart(Name = LeftRepeatButton, Type = typeof(RepeatButton))]
    [TemplatePart(Name = RightRepeatButton, Type = typeof(RepeatButton))]
    [TemplatePart(Name = TopRepeatButton, Type = typeof(RepeatButton))]
    [TemplatePart(Name = BottomRepeatButton, Type = typeof(RepeatButton))]
    [TemplatePart(Name = ElementTabPanel, Type = typeof(TabPanel))]
    [TemplatePart(Name = ElementClearButton, Type = typeof(Button))]
    public class TabControlAssistant
    {
        private const string LeftRepeatButton = "PART_ScrollButtonLeft";
        private const string RightRepeatButton = "PART_ScrollButtonRight";
        private const string TopRepeatButton = "PART_ScrollButtonTop";
        private const string BottomRepeatButton = "PART_ScrollButtonBottom";
        private const string ScrollViewerControl = "PART_ScrollViewerEx";
        private const string ElementTabPanel = "HeaderPanel";
        private const string ElementClearButton = "PART_CloseButton";

        public static bool GetIsAddClearButton(DependencyObject obj) => (bool)obj.GetValue(IsAddClearButtonProperty);
        public static void SetIsAddClearButton(DependencyObject obj, bool value) => obj.SetValue(IsAddClearButtonProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsAddClearButtonProperty"/>标识是否增加删除按钮
        /// </summary>
        public static readonly DependencyProperty IsAddClearButtonProperty =
            DependencyProperty.RegisterAttached("IsAddClearButton", typeof(bool), typeof(TabControlAssistant), new PropertyMetadata(ValueBoxes.FalseBox, OnIsAddClearButtonChanged));

        private static void OnIsAddClearButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            if (d is TabControl tabControl)
            {
                if ((bool)args.NewValue)
                    tabControl.SizeChanged += SizeChanged;
                else
                    tabControl.SizeChanged -= SizeChanged;

                void SizeChanged(object sender, SizeChangedEventArgs e)
                {
                    var tabItems = new List<TabItem>();
                    if (tabControl.ItemContainerGenerator.Items.OfType<TabItem>().Count() <= 0)
                    {
                        tabControl.ItemContainerGenerator.Items.ForEach(t =>
                        {
                            var tabItem = tabControl.ItemContainerGenerator.ContainerFromItem(t);
                            tabItems.Add((TabItem)tabItem);
                        });
                    }
                    else
                    {
                        tabItems.AddRange(tabControl.ItemContainerGenerator.Items.OfType<TabItem>());
                    }

                    tabItems.ForEach(t => ClearHandler(tabControl, t, (bool)args.NewValue));
                }
            }
        }

        private static void ClearHandler(TabControl tabControl, TabItem tabItem, bool value)
        {
            if (tabItem == null) return;

            var parentElement = ItemsControl.ItemsControlFromItemContainer(tabItem) as System.Windows.Controls.TabControl;
            var clearButton = tabItem.Template.FindName(ElementClearButton, tabItem) as Button;
            if (clearButton != null)
            {
                RoutedEventHandler handler = (s, e) =>
                {
                    var item = tabControl.ItemContainerGenerator.ItemFromContainer(tabItem);
                    var list = GetActualList(tabControl);
                    list?.Remove(item);
                };

                if (value)
                    clearButton.Click += handler;
                else
                    clearButton.Click -= handler;

                IList? GetActualList(ItemsControl itemsControl)
                {
                    IList? list;
                    if (itemsControl.ItemsSource != null)
                    {
                        list = itemsControl.ItemsSource as IList;
                    }
                    else
                    {
                        list = itemsControl.Items;
                    }

                    return list;
                }
            }
        }


        public static Geometry GetHeaderIcon(DependencyObject obj) => (Geometry)obj.GetValue(HeaderIconProperty);
        public static void SetHeaderIcon(DependencyObject obj, Geometry value) => obj.SetValue(HeaderIconProperty, value);
        /// <summary>
        /// 表示<see cref="TabItem.Header"/>图标
        /// </summary>
        public static readonly DependencyProperty HeaderIconProperty =
            DependencyProperty.RegisterAttached("HeaderIcon", typeof(Geometry), typeof(TabControlAssistant), new PropertyMetadata(Geometry.Empty));


        public static bool GetIsShowScrollButton(DependencyObject obj) => (bool)obj.GetValue(IsShowScrollButtonProperty);
        public static void SetIsShowScrollButton(DependencyObject obj, bool value) => obj.SetValue(IsShowScrollButtonProperty, value);
        /// <summary>
        /// 是否显示ScrollButton
        /// </summary>
        public static readonly DependencyProperty IsShowScrollButtonProperty =
            DependencyProperty.RegisterAttached("IsShowScrollButton", typeof(bool), typeof(TabControlAssistant), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender, OnIsShowScrollButtonChanged));

        private static void OnIsShowScrollButtonChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender is TabControl tab)
            {
                if ((bool)args.NewValue)
                    tab.SizeChanged += Tab_Loaded;
                else
                    tab.SizeChanged -= Tab_Loaded;
            }
        }

        private static void Tab_Loaded(object sender, RoutedEventArgs e)
        {
            var tab = (TabControl)sender;
            var leftRepeatButton = tab.Template.FindName(LeftRepeatButton, tab) as RepeatButton;
            var rightRepeatButton = tab.Template.FindName(RightRepeatButton, tab) as RepeatButton;
            var topRepeatButton = tab.Template.FindName(TopRepeatButton, tab) as RepeatButton;
            var bottomRepeatButton = tab.Template.FindName(BottomRepeatButton, tab) as RepeatButton;
            var scrollViewer = tab.Template.FindName(ScrollViewerControl, tab) as ScrollViewerEx;

            var tabItems = new List<TabItem>();
            if (tab.ItemContainerGenerator.Items.OfType<TabItem>().Count() <= 0)
            {
                tab.ItemContainerGenerator.Items.ForEach(t =>
                {
                    var tabItem = tab.ItemContainerGenerator.ContainerFromItem(t);
                    tabItems.Add((TabItem)tabItem);
                });
            }
            else
            {
                tabItems.AddRange(tab.ItemContainerGenerator.Items.OfType<TabItem>());
            }
            var maxTabItemWidth = tabItems.Max(t => t.ActualWidth);
            var maxTabItemHeight = tabItems.Max(t => t.ActualHeight);

            if (leftRepeatButton != null)
                leftRepeatButton.Click -= LeftRepeatButton_Click;
            if (rightRepeatButton != null)
                rightRepeatButton.Click -= RightRepeatButton_Click;
            if (topRepeatButton != null)
                topRepeatButton.Click -= TopRepeatButton_Click;
            if (bottomRepeatButton != null)
                bottomRepeatButton.Click -= BottomRepeatButton_Click;

            if (leftRepeatButton != null)
                leftRepeatButton.Click += LeftRepeatButton_Click;
            if (rightRepeatButton != null)
                rightRepeatButton.Click += RightRepeatButton_Click;
            if (topRepeatButton != null)
                topRepeatButton.Click += TopRepeatButton_Click; ;
            if (bottomRepeatButton != null)
                bottomRepeatButton.Click += BottomRepeatButton_Click; ;

            void LeftRepeatButton_Click(object sender, RoutedEventArgs e)
            {
                if (scrollViewer != null)
                    scrollViewer.ScrollToHorizontalOffsetWithAnimation(Math.Max(scrollViewer.CurrentHorizontalOffset - maxTabItemWidth, 0));
            }

            void RightRepeatButton_Click(object sender, RoutedEventArgs e)
            {
                if (scrollViewer != null)
                    scrollViewer.ScrollToHorizontalOffsetWithAnimation(Math.Min(scrollViewer.CurrentHorizontalOffset + maxTabItemWidth, scrollViewer.ScrollableWidth));
            }

            void TopRepeatButton_Click(object sender, RoutedEventArgs e)
            {
                if (scrollViewer != null)
                    scrollViewer.ScrollToVerticalOffsetWithAnimation(Math.Min(scrollViewer.CurrentVerticalOffset - maxTabItemHeight, 0));
            }

            void BottomRepeatButton_Click(object sender, RoutedEventArgs e)
            {
                if (scrollViewer != null)
                    scrollViewer.ScrollToVerticalOffsetWithAnimation(Math.Min(scrollViewer.CurrentVerticalOffset + maxTabItemHeight, scrollViewer.ScrollableHeight));
            }
        }


        public static double GetSlideMinWidth(DependencyObject obj) => (double)obj.GetValue(SlideMinWidthProperty);
        public static void SetSlideMinWidth(DependencyObject obj, double value) => obj.SetValue(SlideMinWidthProperty, value);
        /// <summary>
        /// <see cref="TabControl"/>Style为<see cref="TabControlSlideStyle"/>时Slide的最小宽度
        /// </summary>
        public static readonly DependencyProperty SlideMinWidthProperty =
            DependencyProperty.RegisterAttached("SlideMinWidth", typeof(double), typeof(TabControlAssistant), new PropertyMetadata(ValueBoxes.Double50Box));


        public static double GetSlideMinHeight(DependencyObject obj) => (double)obj.GetValue(SlideMinHeightProperty);
        public static void SetSlideMinHeight(DependencyObject obj, double value) => obj.SetValue(SlideMinHeightProperty, value);
        /// <summary>
        /// <see cref="TabControl"/>Style为<see cref="TabControlSlideStyle"/>时Slide的最小高度
        /// </summary>
        public static readonly DependencyProperty SlideMinHeightProperty =
            DependencyProperty.RegisterAttached("SlideMinHeight", typeof(double), typeof(TabControlAssistant), new PropertyMetadata(ValueBoxes.Double0Box));
    }
}
