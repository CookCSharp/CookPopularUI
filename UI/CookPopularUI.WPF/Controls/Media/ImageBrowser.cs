/*
 *Description: ImageBrowser
 *Author: Chance.zheng
 *Creat Time: 2023/11/17 9:46:13
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ValueBoxes = CookPopularToolkit.ValueBoxes;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 图片选择器
    /// </summary>
    [TemplatePart(Name = ElementToggleButton, Type = typeof(ToggleButton))]
    public class ImageBrowser : Control
    {
        private const string ElementToggleButton = "PART_SelectImage";
        private ToggleButton _toggleButton;
        private Uri _originSource;

        private static void OnImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageBrowser imageBrowser)
            {
                if (imageBrowser.Source != null)
                {
                    imageBrowser.SetValue(ImageBrushPropertyKey, new ImageBrush(BitmapFrame.Create(imageBrowser.Source, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.None)) { Stretch = imageBrowser.Stretch });
                    if (imageBrowser.IsShowSource)
                        imageBrowser.SetCurrentValue(ToolTipProperty, imageBrowser.Source.AbsolutePath);
                    else
                        imageBrowser.ClearValue(ToolTipProperty);
                    imageBrowser.SetValue(HasValuePropertyKey, ValueBoxes.TrueBox);
                    imageBrowser.OnSelected(imageBrowser);

                    imageBrowser._originSource = imageBrowser.Source;
                }

                imageBrowser.UpdateSelectedState();
            }
        }


        public Uri Source
        {
            get => (Uri)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Source"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(Uri), typeof(ImageBrowser), new PropertyMetadata(default(Uri), OnImageChanged));


        public Brush ImageBrush
        {
            get => (Brush)GetValue(ImageBrushProperty);
            set => SetValue(ImageBrushProperty, value);
        }
        public static readonly DependencyPropertyKey ImageBrushPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ImageBrush), typeof(Brush), typeof(ImageBrowser), new PropertyMetadata(default(Brush)));
        /// <summary>
        /// 提供<see cref="ImageBrush"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ImageBrushProperty = ImageBrushPropertyKey.DependencyProperty;


        public Stretch Stretch
        {
            get => (Stretch)GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Stretch"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(ImageBrowser), new PropertyMetadata(default(Stretch), OnImageChanged));


        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }
        /// <summary>
        /// 提供<see cref="StrokeThickness"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(ImageBrowser), new FrameworkPropertyMetadata(ValueBoxes.Double1Box, FrameworkPropertyMetadataOptions.AffectsRender));


        public DoubleCollection StrokeDashArray
        {
            get => (DoubleCollection)GetValue(StrokeDashArrayProperty);
            set => SetValue(StrokeDashArrayProperty, value);
        }
        /// <summary>
        /// 提供<see cref="StrokeDashArray"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty StrokeDashArrayProperty =
            DependencyProperty.Register(nameof(StrokeDashArray), typeof(DoubleCollection), typeof(ImageBrowser), new FrameworkPropertyMetadata(default(DoubleCollection), FrameworkPropertyMetadataOptions.AffectsRender));


        public string DefaultExtension
        {
            get => (string)GetValue(DefaultExtensionProperty);
            set => SetValue(DefaultExtensionProperty, value);
        }
        /// <summary>
        /// 提供<see cref="DefaultExtension"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DefaultExtensionProperty =
            DependencyProperty.Register(nameof(DefaultExtension), typeof(string), typeof(ImageBrowser), new PropertyMetadata(".png"));


        public string Filter
        {
            get => (string)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Filter"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register(nameof(Filter), typeof(string), typeof(ImageBrowser), new PropertyMetadata(".png|*.png|.jpg|*.jpg|.jpeg|*.jpeg|.bmp|*.bmp|All|*.*"));


        /// <summary>
        /// 是否有了图片
        /// </summary>
        public bool HasValue
        {
            get => (bool)GetValue(HasValueProperty);
            set => SetValue(HasValueProperty, ValueBoxes.BooleanBox(value));
        }
        public static readonly DependencyPropertyKey HasValuePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(HasValue), typeof(bool), typeof(ImageBrowser), new PropertyMetadata(ValueBoxes.FalseBox));
        /// <summary>
        /// 提供<see cref="HasValue"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty HasValueProperty = HasValuePropertyKey.DependencyProperty;


        /// <summary>
        /// 是否显示图片源
        /// </summary>
        public bool IsShowSource
        {
            get => (bool)GetValue(IsShowSourceProperty);
            set => SetValue(IsShowSourceProperty, ValueBoxes.BooleanBox(value));
        }
        /// <summary>
        /// 提供<see cref="IsShowSource"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsShowSourceProperty =
            DependencyProperty.Register(nameof(IsShowSource), typeof(bool), typeof(ImageBrowser), new PropertyMetadata(ValueBoxes.FalseBox, OnImageChanged));


        /// <summary>
        /// 图片选中事件
        /// </summary>
        [Category("Behavior")]
        [Description("图片选中事件")]
        public event RoutedEventHandler Selected
        {
            add => this.AddHandler(SelectedEvent, value);
            remove => this.RemoveHandler(SelectedEvent, value);
        }
        public static readonly RoutedEvent SelectedEvent =
            EventManager.RegisterRoutedEvent(nameof(Selected), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ImageBrowser));

        protected virtual void OnSelected(object source)
        {
            RoutedEventArgs arg = new RoutedEventArgs(SelectedEvent, source);
            this.RaiseEvent(arg);
        }

        /// <summary>
        /// 图片选中事件
        /// </summary>
        [Category("Behavior")]
        [Description("图片选中事件")]
        public event RoutedEventHandler UnSelected
        {
            add => this.AddHandler(UnSelectedEvent, value);
            remove => this.RemoveHandler(UnSelectedEvent, value);
        }
        public static readonly RoutedEvent UnSelectedEvent =
            EventManager.RegisterRoutedEvent(nameof(UnSelected), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ImageBrowser));

        protected virtual void OnUnSelected(object source)
        {
            RoutedEventArgs arg = new RoutedEventArgs(UnSelectedEvent, source);
            this.RaiseEvent(arg);
        }


        public ImageBrowser()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.ResetCommand, ResetAction));
            CommandBindings.Add(new CommandBinding(ControlCommands.FileOrFolderBrowserCommand, ImageBrowserAction));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _toggleButton = (ToggleButton)GetTemplateChild(ElementToggleButton);

            UpdateSelectedState();
        }

        private void UpdateSelectedState()
        {
            if (_toggleButton != null)
            {
                _toggleButton.IsChecked = HasValue;
            }
        }

        private void ResetAction(object sender, ExecutedRoutedEventArgs e)
        {
            Source = _originSource;
        }

        private void ImageBrowserAction(object sender, ExecutedRoutedEventArgs e)
        {
            if (!HasValue)
            {
                var dialog = new OpenFileDialog
                {
                    RestoreDirectory = true,
                    Filter = Filter,
                    DefaultExt = DefaultExtension,
                };

                if (dialog.ShowDialog() == true)
                {
                    SetValue(SourceProperty, new Uri(dialog.FileName, UriKind.RelativeOrAbsolute));
                }
            }
            else
            {
                ClearValue(SourceProperty);
                ClearValue(ImageBrushPropertyKey);
                ClearValue(HasValuePropertyKey);
                ClearValue(ToolTipProperty);
                OnUnSelected(this);
            }

            UpdateSelectedState();
        }
    }
}
