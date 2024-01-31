/*
 *Description: ClippingBorder
 *Author: Chance.zheng
 *Creat Time: 2024/1/31 14:10:47
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 可裁剪的Border
    /// </summary>
    public class ClippingBorder : ContentControl
    {
        private ContentControl? _topLeftContentControl;
        private ContentControl? _topRightContentControl;
        private ContentControl? _bottomRightContentControl;
        private ContentControl? _bottomLeftContentControl;
        private RectangleGeometry? _topLeftClip;
        private RectangleGeometry? _topRightClip;
        private RectangleGeometry? _bottomRightClip;
        private RectangleGeometry? _bottomLeftClip;

        private Border? _border;


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Sets the corner radius on the border.")]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ClippingBorder), new PropertyMetadata(new PropertyChangedCallback(OnCornerRadiusChanged)));

        private static void OnCornerRadiusChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is ClippingBorder clippingBorder)
                clippingBorder.UpdateCornerRadius((CornerRadius)eventArgs.NewValue);
        }


        [System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Sets whether the content is clipped or not.")]
        public bool IsClipContent
        {
            get => (bool)GetValue(IsClipContentProperty);
            set => SetValue(IsClipContentProperty, value);
        }
        public static readonly DependencyProperty IsClipContentProperty =
            DependencyProperty.Register(nameof(IsClipContent), typeof(bool), typeof(ClippingBorder), new PropertyMetadata(new PropertyChangedCallback(OnIsClipContentChanged)));

        private static void OnIsClipContentChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is ClippingBorder clippingBorder)
                clippingBorder.UpdateClipContent((bool)eventArgs.NewValue);
        }


        public ClippingBorder()
        {
            DefaultStyleKey = typeof(ClippingBorder);
            SizeChanged += new SizeChangedEventHandler(ClippingBorder_SizeChanged);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _border = GetTemplateChild("PART_Border") as Border;
            _topLeftContentControl = GetTemplateChild("PART_TopLeftContentControl") as ContentControl;
            _topRightContentControl = GetTemplateChild("PART_TopRightContentControl") as ContentControl;
            _bottomRightContentControl = GetTemplateChild("PART_BottomRightContentControl") as ContentControl;
            _bottomLeftContentControl = GetTemplateChild("PART_BottomLeftContentControl") as ContentControl;

            if (_topLeftContentControl != null)
            {
                _topLeftContentControl.SizeChanged += new SizeChangedEventHandler(ContentControl_SizeChanged);
            }

            _topLeftClip = GetTemplateChild("PART_TopLeftClip") as RectangleGeometry;
            _topRightClip = GetTemplateChild("PART_TopRightClip") as RectangleGeometry;
            _bottomRightClip = GetTemplateChild("PART_BottomRightClip") as RectangleGeometry;
            _bottomLeftClip = GetTemplateChild("PART_BottomLeftClip") as RectangleGeometry;

            UpdateClipContent(IsClipContent);
            UpdateCornerRadius(CornerRadius);
        }

        private void UpdateCornerRadius(CornerRadius newCornerRadius)
        {
            if (_border != null)
            {
                _border.CornerRadius = newCornerRadius;
            }

            if (_topLeftClip != null)
            {
                _topLeftClip.RadiusX = _topLeftClip.RadiusY = newCornerRadius.TopLeft - (Math.Min(BorderThickness.Left, BorderThickness.Top) / 2);
            }

            if (_topRightClip != null)
            {
                _topRightClip.RadiusX = _topRightClip.RadiusY = newCornerRadius.TopRight - (Math.Min(BorderThickness.Top, BorderThickness.Right) / 2);
            }

            if (_bottomRightClip != null)
            {
                _bottomRightClip.RadiusX = _bottomRightClip.RadiusY = newCornerRadius.BottomRight - (Math.Min(BorderThickness.Right, BorderThickness.Bottom) / 2);
            }

            if (_bottomLeftClip != null)
            {
                _bottomLeftClip.RadiusX = _bottomLeftClip.RadiusY = newCornerRadius.BottomLeft - (Math.Min(BorderThickness.Bottom, BorderThickness.Left) / 2);
            }

            UpdateClipSize(new Size(ActualWidth, ActualHeight));
        }

        private void UpdateClipContent(bool isClipContent)
        {
            if (isClipContent)
            {
                if (_topLeftContentControl != null)
                {
                    _topLeftContentControl.Clip = _topLeftClip;
                }

                if (_topRightContentControl != null)
                {
                    _topRightContentControl.Clip = _topRightClip;
                }

                if (_bottomRightContentControl != null)
                {
                    _bottomRightContentControl.Clip = _bottomRightClip;
                }

                if (_bottomLeftContentControl != null)
                {
                    _bottomLeftContentControl.Clip = _bottomLeftClip;
                }

                UpdateClipSize(new Size(ActualWidth, ActualHeight));
            }
            else
            {
                if (_topLeftContentControl != null)
                {
                    _topLeftContentControl.Clip = null;
                }

                if (_topRightContentControl != null)
                {
                    _topRightContentControl.Clip = null;
                }

                if (_bottomRightContentControl != null)
                {
                    _bottomRightContentControl.Clip = null;
                }

                if (_bottomLeftContentControl != null)
                {
                    _bottomLeftContentControl.Clip = null;
                }
            }
        }

        private void ClippingBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (IsClipContent)
            {
                UpdateClipSize(e.NewSize);
            }
        }

        private void ContentControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (IsClipContent)
            {
                UpdateClipSize(new Size(ActualWidth, ActualHeight));
            }
        }

        private void UpdateClipSize(Size size)
        {
            if (size.Width > 0 || size.Height > 0)
            {
                double contentWidth = Math.Max(0, size.Width - BorderThickness.Left - BorderThickness.Right);
                double contentHeight = Math.Max(0, size.Height - BorderThickness.Top - BorderThickness.Bottom);

                if (_topLeftClip != null)
                {
                    _topLeftClip.Rect = new Rect(0, 0, contentWidth + (CornerRadius.TopLeft * 2), contentHeight + (CornerRadius.TopLeft * 2));
                }

                if (_topRightClip != null)
                {
                    _topRightClip.Rect = new Rect(0 - CornerRadius.TopRight, 0, contentWidth + CornerRadius.TopRight, contentHeight + CornerRadius.TopRight);
                }

                if (_bottomRightClip != null)
                {
                    _bottomRightClip.Rect = new Rect(0 - CornerRadius.BottomRight, 0 - CornerRadius.BottomRight, contentWidth + CornerRadius.BottomRight, contentHeight + CornerRadius.BottomRight);
                }

                if (_bottomLeftClip != null)
                {
                    _bottomLeftClip.Rect = new Rect(0, 0 - CornerRadius.BottomLeft, contentWidth + CornerRadius.BottomLeft, contentHeight + CornerRadius.BottomLeft);
                }
            }
        }
    }
}
