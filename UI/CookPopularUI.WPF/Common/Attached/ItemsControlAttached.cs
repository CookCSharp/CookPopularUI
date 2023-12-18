/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ItemsControlAttached
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-24 11:02:41
 */


using CookPopularToolkit;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace CookPopularUI.WPF
{
    /// <summary>
    /// ItemsControl子项类型
    /// </summary>
    public enum ItemControlType
    {
        Text,
        CheckBox,
        Button,
        /// <summary>
        /// Geometry Data
        /// </summary>
        Icon,
        ImageGif,
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 当ListBoxItem的子项显示Icon或Image或Gif时必须使用该类，方可加入ItemsSource
    /// </remarks>
    public class SelectorItem
    {
        public Geometry GeometryData { get; set; }

        public ImageSource ImageSource { get; set; }

        public Uri GifSource { get; set; }

        public object Content { get; set; }
    }

    public class ItemsControlAttached
    {
        private const string ComboBoxItems_ListBox = "PART_ListBox";
        private const string ItemCheckBox = "ItemCheckBox";
        private const string ItemButton = "ItemButton";
        private const string ItemIcon = "ItemIcon";
        private const string ItemImageGif = "ItemImageGif";

        public static Brush GetItemsListBrush(DependencyObject obj) => (Brush)obj.GetValue(ItemsListBrushProperty);
        public static void SetItemsListBrush(DependencyObject obj, Brush value) => obj.SetValue(ItemsListBrushProperty, value);
        /// <summary>
        /// <see cref="ItemsListBrushProperty"/>标识<see cref="ItemsControl"/>下拉列表的背景色附加属性
        /// </summary>
        public static readonly DependencyProperty ItemsListBrushProperty =
            DependencyProperty.RegisterAttached("ItemsListBrush", typeof(Brush), typeof(ItemsControlAttached), new PropertyMetadata(default(Brush)));


        public static Brush GetItemMouseOverBrush(DependencyObject obj) => (Brush)obj.GetValue(ItemMouseOverBrushProperty);
        public static void SetItemMouseOverBrush(DependencyObject obj, Brush value) => obj.SetValue(ItemMouseOverBrushProperty, value);
        /// <summary>
        /// 标识<see cref="ItemMouseOverBrushProperty"/>提供<see cref="ItemsControl"/>子项的背景色附加属性
        /// </summary>
        public static readonly DependencyProperty ItemMouseOverBrushProperty =
            DependencyProperty.RegisterAttached("ItemMouseOverBrush", typeof(Brush), typeof(ItemsControlAttached), new PropertyMetadata(default(Brush)));


        public static Brush GetItemSelectedBrush(DependencyObject obj) => (Brush)obj.GetValue(ItemSelectedBrushProperty);
        public static void SetItemSelectedBrush(DependencyObject obj, Brush value) => obj.SetValue(ItemSelectedBrushProperty, value);
        /// <summary>
        /// <see cref="ItemSelectedBrushProperty"/>标识<see cref="ItemsControl"/>子项选中的后背景色附加属性
        /// </summary>
        public static readonly DependencyProperty ItemSelectedBrushProperty =
            DependencyProperty.RegisterAttached("ItemSelectedBrush", typeof(Brush), typeof(ItemsControlAttached), new PropertyMetadata(default(Brush)));


        [AttachedPropertyBrowsableForType(typeof(ItemsControl))]
        public static ItemControlType GetItemControlType(DependencyObject obj) => (ItemControlType)obj.GetValue(ItemControlTypeProperty);
        public static void SetItemControlType(DependencyObject obj, ItemControlType value) => obj.SetValue(ItemControlTypeProperty, value);
        /// <summary>
        /// <see cref="ItemControlTypeProperty"/>标识<see cref="ItemsControl"/>子项类型的附加属性
        /// </summary>
        public static readonly DependencyProperty ItemControlTypeProperty =
            DependencyProperty.RegisterAttached("ItemControlType", typeof(ItemControlType), typeof(ItemsControlAttached),
                new FrameworkPropertyMetadata(default(ItemControlType), FrameworkPropertyMetadataOptions.AffectsRender, OnItemControlTypeChanged));

        private static void OnItemControlTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Selector selector)
            {
                if (selector.IsLoaded)
                    SetSelectorItems(selector);
                else
                    selector.Loaded += (s, e) => SetSelectorItems(selector);
            }
        }

        private static void SetSelectorItems(Selector selector)
        {
            if (selector is ComboBox comboBox && GetItemControlType(selector) != ItemControlType.Text)
            {
                var listBox = comboBox.Template.FindName(ComboBoxItems_ListBox, comboBox) as ListBox;
                if (listBox != null)
                {
                    //也可以在xaml中绑定Items
                    listBox.ItemsSource = comboBox.ItemsSource ?? comboBox.Items;

                    listBox.SelectionChanged += (s, e) =>
                    {
                        var comboBoxText = string.Empty;
                        foreach (var item in listBox.SelectedItems)
                        {
                            if (item is string)
                                comboBoxText += item.ToString() + ",";
                            if (item is SelectorItem pic)
                                comboBoxText += pic.Content.ToString() + ",";
                        }
                        if (comboBoxText.Length > 0)
                            comboBox.Text = comboBoxText.Remove(comboBoxText.Length - 1);
                        else
                            comboBox.Text = comboBoxText;
                    };
                }
            }
            else if (selector is ListBox listBox && GetItemControlType(selector) != ItemControlType.Text)
            {

            }
        }


        public static double GetItemWidth(DependencyObject obj) => (double)obj.GetValue(ItemWidthProperty);
        public static void SetItemWidth(DependencyObject obj, double value) => obj.SetValue(ItemWidthProperty, value);
        /// <summary>
        /// <see cref="ItemWidthProperty"/>标识<see cref="ItemsControl"/>子项的宽度
        /// </summary>
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.RegisterAttached("ItemWidth", typeof(double), typeof(ItemsControlAttached), new PropertyMetadata(double.NaN));


        public static double GetItemHeight(DependencyObject obj) => (double)obj.GetValue(ItemHeightProperty);
        public static void SetItemHeight(DependencyObject obj, double value) => obj.SetValue(ItemHeightProperty, value);
        /// <summary>
        /// <see cref="ItemHeightProperty"/>标识<see cref="ItemsControl"/>子项的高度
        /// </summary>
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.RegisterAttached("ItemHeight", typeof(double), typeof(ItemsControlAttached), new PropertyMetadata(ValueBoxes.Double20Box));


        public static double GetItemControlWidth(DependencyObject obj) => (double)obj.GetValue(ItemControlWidthProperty);
        public static void SetItemControlWidth(DependencyObject obj, double value) => obj.SetValue(ItemControlWidthProperty, value);
        /// <summary>
        /// <see cref="ItemControlWidthProperty"/>标识<see cref="ItemsControl"/>子项控件的宽度
        /// </summary>
        public static readonly DependencyProperty ItemControlWidthProperty =
            DependencyProperty.RegisterAttached("ItemControlWidth", typeof(double), typeof(ItemsControlAttached), new PropertyMetadata(ValueBoxes.Double20Box));


        public static double GetItemControlHeight(DependencyObject obj) => (double)obj.GetValue(ItemControlHeightProperty);
        public static void SetItemControlHeight(DependencyObject obj, double value) => obj.SetValue(ItemControlHeightProperty, value);
        /// <summary>
        /// <see cref="ItemControlHeightProperty"/>标识<see cref="ItemsControl"/>子项控件的高度
        /// </summary>
        public static readonly DependencyProperty ItemControlHeightProperty =
            DependencyProperty.RegisterAttached("ItemControlHeight", typeof(double), typeof(ItemsControlAttached), new PropertyMetadata(ValueBoxes.Double20Box));


        public static bool GetIsFirst(DependencyObject obj) => (bool)obj.GetValue(IsFirstProperty);
        public static void SetIsFirst(DependencyObject obj, bool value) => obj.SetValue(IsFirstProperty, value.BooleanBox());
        /// <summary>
        /// <see cref="IsFirstProperty"/>标识<see cref="ItemsControl"/>子项是否为第一个
        /// </summary>
        public static readonly DependencyProperty IsFirstProperty =
            DependencyProperty.RegisterAttached("IsFirst", typeof(bool), typeof(ItemsControlAttached), new PropertyMetadata(ValueBoxes.FalseBox));


        public static bool GetIsLast(DependencyObject obj) => (bool)obj.GetValue(IsLastProperty);
        public static void SetIsLast(DependencyObject obj, bool value) => obj.SetValue(IsLastProperty, value.BooleanBox());
        /// <summary>
        /// <see cref="IsLastProperty"/>标识<see cref="ItemsControl"/>子项是否为最后一个
        /// </summary>
        public static readonly DependencyProperty IsLastProperty =
            DependencyProperty.RegisterAttached("IsLast", typeof(bool), typeof(ItemsControlAttached), new PropertyMetadata(ValueBoxes.FalseBox));


        #region ItemControlType=CheckBox

        public static bool GetIsCheckBoxChecked(DependencyObject obj) => (bool)obj.GetValue(IsCheckBoxCheckedProperty);
        public static void SetIsCheckBoxChecked(DependencyObject obj, bool value) => obj.SetValue(IsCheckBoxCheckedProperty, ValueBoxes.BooleanBox(value));
        public static readonly DependencyProperty IsCheckBoxCheckedProperty =
            DependencyProperty.RegisterAttached("IsCheckBoxChecked", typeof(bool), typeof(ItemsControlAttached),
                new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits, OnIsCheckBoxCheckedChanged));

        private static void OnIsCheckBoxCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var checkBox = d as CheckBox;
            if (checkBox != null)
            {
                var listBoxItem = checkBox.TemplatedParent as ListBoxItem;
                if (listBoxItem != null)
                {
                    checkBox.Checked += (s, e) => OnItemChecked(listBoxItem, false, true);
                    checkBox.Unchecked += (s, e) => OnItemChecked(listBoxItem, true, false);
                }
            }
        }


        /// <summary>
        /// <see cref="ItemCheckedEvent"/>标识子项ItemCheck是否选中的事件
        /// </summary>
        public static readonly RoutedEvent ItemCheckedEvent =
            EventManager.RegisterRoutedEvent("ItemChecked", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(ItemsControlAttached));
        public static void AddItemCheckedHandler(DependencyObject d, RoutedPropertyChangedEventHandler<object> handler)
        {
            (d as UIElement)?.AddHandler(ItemCheckedEvent, handler);
        }
        public static void RemoveItemCheckedHandler(DependencyObject d, RoutedPropertyChangedEventHandler<object> handler)
        {
            (d as UIElement)?.RemoveHandler(ItemCheckedEvent, handler);
        }
        private static void OnItemChecked(object sender, object oldValue, object newValue)
        {
            var element = sender as UIElement;
            RoutedPropertyChangedEventArgs<object> arg = new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, ItemCheckedEvent);
            element?.RaiseEvent(arg);
        }


        [Obsolete("备用", false)]
        public static string GetCheckBoxContent(DependencyObject obj) => (string)obj.GetValue(CheckBoxContentProperty);
        [Obsolete("备用", false)]
        public static void SetCheckBoxContent(DependencyObject obj, string value) => obj.SetValue(CheckBoxContentProperty, value);
        [Obsolete("备用", false)]
        public static readonly DependencyProperty CheckBoxContentProperty =
            DependencyProperty.RegisterAttached("CheckBoxContent", typeof(string), typeof(ItemsControlAttached), new PropertyMetadata(default(string)));

        #endregion

        #region ItemControlType=Button

        public static object GetButtonContent(DependencyObject obj) => obj.GetValue(ButtonContentProperty);
        public static void SetButtonContent(DependencyObject obj, object value) => obj.SetValue(ButtonContentProperty, value);
        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.RegisterAttached("ButtonContent", typeof(object), typeof(ItemsControlAttached), new PropertyMetadata(default(object)));

        public static bool GetIsButtonDeleteItem(DependencyObject obj) => (bool)obj.GetValue(IsButtonDeleteItemProperty);
        public static void SetIsButtonDeleteItem(DependencyObject obj, bool value) => obj.SetValue(IsButtonDeleteItemProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsButtonDeleteItemProperty"/>用于触发Button的Click事件
        /// </summary>
        public static readonly DependencyProperty IsButtonDeleteItemProperty =
            DependencyProperty.RegisterAttached("IsButtonDeleteItem", typeof(bool), typeof(ItemsControlAttached),
                new FrameworkPropertyMetadata(ValueBoxes.FalseBox, OnIsButtonDeleteItemChanged));

        private static void OnIsButtonDeleteItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = d as Button;
            if (button != null)
            {
                var listBoxItem = button.TemplatedParent as ListBoxItem;
                if (listBoxItem != null)
                {
                    button.Click += (s, e) => OnItemDelete(listBoxItem);
                }
            }
        }

        /// <summary>
        /// <see cref="ItemDeleteEvent"/>标识是否删除该项的事件
        /// </summary>
        public static readonly RoutedEvent ItemDeleteEvent =
            EventManager.RegisterRoutedEvent("ItemDelete", RoutingStrategy.Bubble, typeof(RoutedEventArgs), typeof(ItemsControlAttached));
        public static void AddItemDeleteHandler(DependencyObject d, RoutedEventHandler handler)
        {
            (d as UIElement)?.AddHandler(ItemDeleteEvent, handler);
        }
        public static void RemoveItemDeleteHandler(DependencyObject d, RoutedEventHandler handler)
        {
            (d as UIElement)?.RemoveHandler(ItemDeleteEvent, handler);
        }

        private static void OnItemDelete(object sender)
        {
            var element = sender as UIElement;
            RoutedEventArgs arg = new RoutedEventArgs(ItemDeleteEvent, sender);
            element?.RaiseEvent(arg);
        }

        #endregion

        #region ItemControlType=Icon

        /**
         * 当使用Icon时，在设置的ItemsSource每个子项中必须含有属性GeometryData,属于<see cref="Geometry"/>
         * 因为ComboBoxItem.Template中Path的绑定方式为Content.GeometryData
         * **/

        public static Brush GetIconFill(DependencyObject obj) => (Brush)obj.GetValue(IconFillProperty);
        public static void SetIconFill(DependencyObject obj, Brush value) => obj.SetValue(IconFillProperty, value);
        public static readonly DependencyProperty IconFillProperty =
            DependencyProperty.RegisterAttached("IconFill", typeof(Brush), typeof(ItemsControlAttached), new PropertyMetadata(default(Brush)));

        #endregion

        #region ItemControlType=Image/Gif

        public static ImageSource GetImageSource(DependencyObject obj) => (ImageSource)obj.GetValue(ImageSourceProperty);
        public static void SetImageSource(DependencyObject obj, ImageSource value) => obj.SetValue(ImageSourceProperty, value);
        [Description("仅当ItemsSource的每个子项都为同一张图片时使用")]
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.RegisterAttached("ImageSource", typeof(ImageSource), typeof(ItemsControlAttached), new PropertyMetadata(default(ImageSource)));

        public static Uri GetGifSource(DependencyObject obj) => (Uri)obj.GetValue(GifSourceProperty);
        public static void SetGifSource(DependencyObject obj, Uri value) => obj.SetValue(GifSourceProperty, value);
        [Description("仅当ItemsSource的每个子项都为同一张图片时使用")]
        public static readonly DependencyProperty GifSourceProperty =
            DependencyProperty.RegisterAttached("GifSource", typeof(Uri), typeof(ItemsControlAttached), new PropertyMetadata(default(Uri)));

        public static bool GetIsPreviewImageGif(DependencyObject obj) => (bool)obj.GetValue(IsPreviewImageGifProperty);
        public static void SetIsPreviewImageGif(DependencyObject obj, bool value) => obj.SetValue(IsPreviewImageGifProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsPreviewImageGifProperty"/>表示是否可以预览图片
        /// </summary>
        public static readonly DependencyProperty IsPreviewImageGifProperty =
            DependencyProperty.RegisterAttached("IsPreviewImageGif", typeof(bool), typeof(ItemsControlAttached), new PropertyMetadata(ValueBoxes.FalseBox));

        #endregion
    }
}
