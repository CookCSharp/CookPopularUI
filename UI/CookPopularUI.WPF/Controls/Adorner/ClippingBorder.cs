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
        private ContentControl? topLeftContentControl;
        private ContentControl? topRightContentControl;
        private ContentControl? bottomRightContentControl;
        private ContentControl? bottomLeftContentControl;

        private RectangleGeometry? topLeftClip;
        private RectangleGeometry? topRightClip;
        private RectangleGeometry? bottomRightClip;
        private RectangleGeometry? bottomLeftClip;

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
            topLeftContentControl = GetTemplateChild("PART_TopLeftContentControl") as ContentControl;
            topRightContentControl = GetTemplateChild("PART_TopRightContentControl") as ContentControl;
            bottomRightContentControl = GetTemplateChild("PART_BottomRightContentControl") as ContentControl;
            bottomLeftContentControl = GetTemplateChild("PART_BottomLeftContentControl") as ContentControl;

            if (topLeftContentControl != null)
            {
                topLeftContentControl.SizeChanged += new SizeChangedEventHandler(ContentControl_SizeChanged);
            }

            topLeftClip = GetTemplateChild("PART_TopLeftClip") as RectangleGeometry;
            topRightClip = GetTemplateChild("PART_TopRightClip") as RectangleGeometry;
            bottomRightClip = GetTemplateChild("PART_BottomRightClip") as RectangleGeometry;
            bottomLeftClip = GetTemplateChild("PART_BottomLeftClip") as RectangleGeometry;

            UpdateClipContent(IsClipContent);
            UpdateCornerRadius(CornerRadius);
        }

        private void UpdateCornerRadius(CornerRadius newCornerRadius)
        {
            if (_border != null)
            {
                _border.CornerRadius = newCornerRadius;
            }

            if (topLeftClip != null)
            {
                topLeftClip.RadiusX = topLeftClip.RadiusY = newCornerRadius.TopLeft - (Math.Min(BorderThickness.Left, BorderThickness.Top) / 2);
            }

            if (topRightClip != null)
            {
                topRightClip.RadiusX = topRightClip.RadiusY = newCornerRadius.TopRight - (Math.Min(BorderThickness.Top, BorderThickness.Right) / 2);
            }

            if (bottomRightClip != null)
            {
                bottomRightClip.RadiusX = bottomRightClip.RadiusY = newCornerRadius.BottomRight - (Math.Min(BorderThickness.Right, BorderThickness.Bottom) / 2);
            }

            if (bottomLeftClip != null)
            {
                bottomLeftClip.RadiusX = bottomLeftClip.RadiusY = newCornerRadius.BottomLeft - (Math.Min(BorderThickness.Bottom, BorderThickness.Left) / 2);
            }

            UpdateClipSize(new Size(ActualWidth, ActualHeight));
        }

        private void UpdateClipContent(bool isClipContent)
        {
            if (isClipContent)
            {
                if (topLeftContentControl != null)
                {
                    topLeftContentControl.Clip = topLeftClip;
                }

                if (topRightContentControl != null)
                {
                    topRightContentControl.Clip = topRightClip;
                }

                if (bottomRightContentControl != null)
                {
                    bottomRightContentControl.Clip = bottomRightClip;
                }

                if (bottomLeftContentControl != null)
                {
                    bottomLeftContentControl.Clip = bottomLeftClip;
                }

                UpdateClipSize(new Size(ActualWidth, ActualHeight));
            }
            else
            {
                if (topLeftContentControl != null)
                {
                    topLeftContentControl.Clip = null;
                }

                if (topRightContentControl != null)
                {
                    topRightContentControl.Clip = null;
                }

                if (bottomRightContentControl != null)
                {
                    bottomRightContentControl.Clip = null;
                }

                if (bottomLeftContentControl != null)
                {
                    bottomLeftContentControl.Clip = null;
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

                if (topLeftClip != null)
                {
                    topLeftClip.Rect = new Rect(0, 0, contentWidth + (CornerRadius.TopLeft * 2), contentHeight + (CornerRadius.TopLeft * 2));
                }

                if (topRightClip != null)
                {
                    topRightClip.Rect = new Rect(0 - CornerRadius.TopRight, 0, contentWidth + CornerRadius.TopRight, contentHeight + CornerRadius.TopRight);
                }

                if (bottomRightClip != null)
                {
                    bottomRightClip.Rect = new Rect(0 - CornerRadius.BottomRight, 0 - CornerRadius.BottomRight, contentWidth + CornerRadius.BottomRight, contentHeight + CornerRadius.BottomRight);
                }

                if (bottomLeftClip != null)
                {
                    bottomLeftClip.Rect = new Rect(0, 0 - CornerRadius.BottomLeft, contentWidth + CornerRadius.BottomLeft, contentHeight + CornerRadius.BottomLeft);
                }
            }
        }
    }
}
