/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：TextElementAttached
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-18 16:35:11
 */


using CookPopularToolkit;
using CookPopularToolkit.Windows;
using CookPopularUI.WPF.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using ValueBoxes = CookPopularToolkit.ValueBoxes;

namespace CookPopularUI.WPF
{
    /// <summary>
    /// <see cref="TextElementAttached"/>表示可输入文本控件的附加属性基类
    /// </summary>
    /// <remarks>
    /// 包括<see cref="TextBox"/>、<see cref="RichTextBox"/>、<see cref="PasswordBox"/>、
    /// <see cref="ComboBox"/>、<see cref="NumericUpDown"/>、<see cref="SearchBar"/>
    /// </remarks>
    public class TextElementAttached
    {
        private const string ElementTextBox = "PART_TextBox";
        private const string ElementClearButton = "PART_ClearButton";

        public static string GetPlaceHolder(DependencyObject obj) => (string)obj.GetValue(PlaceHolderProperty);
        public static void SetPlaceHolder(DependencyObject obj, string value) => obj.SetValue(PlaceHolderProperty, value);
        /// <summary>
        /// <see cref="PlaceHolderProperty"/>提供占位符内容
        /// </summary>
        public static readonly DependencyProperty PlaceHolderProperty =
            DependencyProperty.RegisterAttached("PlaceHolder", typeof(string), typeof(TextElementAttached), new PropertyMetadata(default(string)));


        public static Brush GetPlaceHolderBrush(DependencyObject obj) => (Brush)obj.GetValue(PlaceHolderBrushProperty);
        public static void SetPlaceHolderBrush(DependencyObject obj, Brush value) => obj.SetValue(PlaceHolderBrushProperty, value);
        /// <summary>
        /// <see cref="PlaceHolderBrushProperty"/>占位符内容文本颜色
        /// </summary>
        public static readonly DependencyProperty PlaceHolderBrushProperty =
            DependencyProperty.RegisterAttached("PlaceHolderBrush", typeof(Brush), typeof(TextElementAttached), new PropertyMetadata(default(Brush)));


        public static bool GetIsAddClearButton(DependencyObject obj) => (bool)obj.GetValue(IsAddClearButtonProperty);
        public static void SetIsAddClearButton(DependencyObject obj, bool value) => obj.SetValue(IsAddClearButtonProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsAddClearButtonProperty"/>表示是否增加清除文本内容按钮
        /// </summary>
        public static readonly DependencyProperty IsAddClearButtonProperty =
            DependencyProperty.RegisterAttached("IsAddClearButton", typeof(bool), typeof(TextElementAttached),
                new PropertyMetadata(ValueBoxes.FalseBox, IsAddClearButtonValueChanged));

        private static void IsAddClearButtonValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Control input && (bool)e.NewValue)
            {
                if (input.IsLoaded)
                    ClearHandler(input);
                else
                    input.Loaded += (s, e) => ClearHandler(input);
            }
        }

        private static void ClearHandler(Control input)
        {
            var value = GetIsAddClearButton(input);
            var clearButton = input.GetTemplateChild<Button>(ElementClearButton);
            if (clearButton != null)
            {
                RoutedEventHandler handler = (s, e) =>
                {
                    if (input is ComboBox comboBox && ItemsControlAttached.GetItemControlType(comboBox) != ItemControlType.Text)
                    {
                        var listBox = comboBox.Template.FindName("PART_ListBox", comboBox) as ListBox;
                        listBox?.UnselectAll();
                    }
                    (input as ComboBox)?.SetCurrentValue(ComboBox.TextProperty, null);
                    (input as TextBox)?.SetCurrentValue(TextBox.TextProperty, null);
                    if (input is PasswordBox pb) pb.Password = null;

                    SetIsTextClear(input, true);
                };

                if (value)
                    clearButton.Click += handler;
                else
                    clearButton.Click -= handler;
            }
        }


        public static bool GetIsTextClear(DependencyObject obj) => (bool)obj.GetValue(IsTextClearProperty);
        private static void SetIsTextClear(DependencyObject obj, bool value) => obj.SetValue(IsTextClearProperty, value);
        /// <summary>
        /// <see cref="IsTextClearProperty"/>表示是否清除了文本
        /// </summary>
        private static readonly DependencyProperty IsTextClearProperty =
            DependencyProperty.RegisterAttached("IsTextClear", typeof(bool), typeof(TextElement), new PropertyMetadata(ValueBoxes.FalseBox));


        public static string GetStringFormat(DependencyObject obj) => (string)obj.GetValue(StringFormatProperty);
        public static void SetStringFormat(DependencyObject obj, string value) => obj.SetValue(StringFormatProperty, value);
        public static readonly DependencyProperty StringFormatProperty =
            DependencyProperty.RegisterAttached("StringFormat", typeof(string), typeof(TextElementAttached), new PropertyMetadata("#0.0"));
    }
}
