/*
 *Description: RangeSlider
 *Author: Chance.zheng
 *Creat Time: 2024/2/23 16:49:58
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using CookPopularToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CookPopularUI.WPF.Controls
{
    public class RangeSlider : Control
    {
        private const double ThumbSize = 9.6; //_rangeStartThumb.ActualWidth = 9.6
        private const string ElementRangeStartThumb = "RangeStartThumb";
        private const string ElementRangeCenterThumb = "RangeCenterThumb";
        private const string ElementRangeEndThumb = "RangeEndThumb";
        private const string ElementSelectedRangeBorder = "SelectedRangeBorder";

        public event EventHandler RangeChanged;

        private Border _selectedRangeBorder;
        private Thumb _rangeStartThumb;
        private Thumb _rangeCenterThumb;
        private Thumb _rangeEndThumb;


        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(RangeSlider), new PropertyMetadata(default(Orientation)));


        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(RangeSlider), new PropertyMetadata(ValueBoxes.Double0Box, new PropertyChangedCallback(OnRangeBoundsChanged)));


        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(RangeSlider), new PropertyMetadata(ValueBoxes.Double1Box, new PropertyChangedCallback(OnRangeBoundsChanged)));

        private static void OnRangeBoundsChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            RangeSlider rangeSlider = (RangeSlider)d;
            if (rangeSlider.Maximum - rangeSlider.Minimum < rangeSlider.MinimumRangeSpan)
            {
                rangeSlider.MinimumRangeSpan = rangeSlider.Maximum - rangeSlider.Minimum;
            }
            rangeSlider.UpdateSelectedRangeMinimumActualSpan();
            rangeSlider.UpdateRange(true);
        }


        public double RangeStart
        {
            get => (double)GetValue(RangeStartProperty);
            set => SetValue(RangeStartProperty, value);
        }
        public static readonly DependencyProperty RangeStartProperty =
            DependencyProperty.Register(nameof(RangeStart), typeof(double), typeof(RangeSlider), new PropertyMetadata(ValueBoxes.Double0Box, new PropertyChangedCallback(OnRangeChanged)));


        public double RangeEnd
        {
            get => (double)GetValue(RangeEndProperty);
            set => SetValue(RangeEndProperty, value);
        }
        public static readonly DependencyProperty RangeEndProperty =
            DependencyProperty.Register(nameof(RangeEnd), typeof(double), typeof(RangeSlider), new PropertyMetadata(ValueBoxes.Double1Box, new PropertyChangedCallback(OnRangeChanged)));

        private static void OnRangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            RangeSlider rangeSlider = (RangeSlider)d;
            switch (args.Property.Name)
            {
                case nameof(RangeStart):
                    rangeSlider.RangeStart = Math.Max(rangeSlider.Minimum, (double)args.NewValue);
                    break;
                case nameof(RangeEnd):
                    rangeSlider.RangeEnd = Math.Min(rangeSlider.Maximum, (double)args.NewValue);
                    break;
                default:
                    break;
            }

            rangeSlider.UpdateSlider();

            if (rangeSlider.RangeChanged != null)
            {
                rangeSlider.RangeChanged(rangeSlider, EventArgs.Empty);
            }
        }


        public double MinimumRangeSpan
        {
            get => (double)GetValue(MinimumRangeSpanProperty);
            set => SetValue(MinimumRangeSpanProperty, value);
        }
        public static readonly DependencyProperty MinimumRangeSpanProperty =
            DependencyProperty.Register(nameof(MinimumRangeSpan), typeof(double), typeof(RangeSlider), new PropertyMetadata(ValueBoxes.Double0Box, OnMinimumRangeSpanChanged));

        private static void OnMinimumRangeSpanChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RangeSlider rangeSlider)
            {
                if (rangeSlider.IsLoaded)
                    OnLoaded();
                else
                    rangeSlider.Loaded += (s, e) => OnLoaded();

                void OnLoaded()
                {
                    rangeSlider.MinimumRangeSpan = Math.Min(rangeSlider.Maximum - rangeSlider.Minimum, (double)e.NewValue);
                    rangeSlider.UpdateSelectedRangeMinimumActualSpan();
                    rangeSlider.UpdateRange(false);
                }
            }
        }


        public RangeSlider()
        {
            SizeChanged += new SizeChangedEventHandler(RangeSlider_SizeChanged);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _selectedRangeBorder = (Border)GetTemplateChild(ElementSelectedRangeBorder);
            _rangeStartThumb = (Thumb)GetTemplateChild(ElementRangeStartThumb);
            _rangeCenterThumb = (Thumb)GetTemplateChild(ElementRangeCenterThumb);
            _rangeEndThumb = (Thumb)GetTemplateChild(ElementRangeEndThumb);

            if (_rangeStartThumb != null)
            {
                _rangeStartThumb.DragDelta += new DragDeltaEventHandler(RangeStartThumb_DragDelta);
                _rangeStartThumb.SizeChanged += new SizeChangedEventHandler(RangeThumb_SizeChanged);
            }

            if (_rangeCenterThumb != null)
            {
                _rangeCenterThumb.DragDelta += new DragDeltaEventHandler(RangeCenterThumb_DragDelta);
            }

            if (_rangeEndThumb != null)
            {
                _rangeEndThumb.DragDelta += new DragDeltaEventHandler(RangeEndThumb_DragDelta);
                _rangeEndThumb.SizeChanged += new SizeChangedEventHandler(RangeThumb_SizeChanged);
            }
        }

        private void RangeSlider_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateSelectedRangeMinimumActualSpan();
            UpdateSlider();
        }

        private void RangeThumb_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateSelectedRangeMinimumActualSpan();
        }

        private void RangeStartThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (_selectedRangeBorder != null)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    double startMargin = Math.Min(ActualWidth - _selectedRangeBorder.MinWidth - ThumbSize, Math.Max(0, _selectedRangeBorder.Margin.Left + e.HorizontalChange));
                    double endMargin = _selectedRangeBorder.Margin.Right;

                    if (ActualWidth - startMargin - endMargin - ThumbSize < _selectedRangeBorder.MinWidth)
                    {
                        endMargin = ActualWidth - startMargin - ThumbSize - _selectedRangeBorder.MinWidth;
                    }

                    _selectedRangeBorder.Margin = new Thickness(startMargin, _selectedRangeBorder.Margin.Top, endMargin, _selectedRangeBorder.Margin.Bottom);
                }
                else
                {
                    double startMargin = Math.Min(ActualHeight - _selectedRangeBorder.MinHeight - ThumbSize, Math.Max(0, _selectedRangeBorder.Margin.Bottom - e.VerticalChange));
                    double endMargin = _selectedRangeBorder.Margin.Top;

                    if (ActualHeight - startMargin - endMargin - ThumbSize < _selectedRangeBorder.MinHeight)
                    {
                        endMargin = ActualHeight - startMargin - ThumbSize - _selectedRangeBorder.MinHeight;
                    }

                    _selectedRangeBorder.Margin = new Thickness(_selectedRangeBorder.Margin.Left, endMargin, _selectedRangeBorder.Margin.Right, startMargin);
                }
                UpdateRange(true);
            }
        }

        private void RangeCenterThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (_selectedRangeBorder != null)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    double startMargin = _selectedRangeBorder.Margin.Left + e.HorizontalChange;
                    double endMargin = _selectedRangeBorder.Margin.Right - e.HorizontalChange;

                    if (startMargin + e.HorizontalChange <= 0)
                    {
                        startMargin = 0;
                        endMargin = ActualWidth - 2 * ThumbSize - (RangeEnd - RangeStart) / (Maximum - Minimum) * (ActualWidth - 2 * ThumbSize);
                    }
                    else if (endMargin - e.HorizontalChange <= 0)
                    {
                        endMargin = 0;
                        startMargin = ActualWidth - 2 * ThumbSize - (RangeEnd - RangeStart) / (Maximum - Minimum) * (ActualWidth - 2 * ThumbSize);
                    }

                    if (!double.IsNaN(startMargin) && !double.IsNaN(endMargin))
                    {
                        _selectedRangeBorder.Margin = new Thickness(startMargin, _selectedRangeBorder.Margin.Top, endMargin, _selectedRangeBorder.Margin.Bottom);
                    }
                }
                else
                {
                    double startMargin = _selectedRangeBorder.Margin.Bottom - e.VerticalChange;
                    double endMargin = _selectedRangeBorder.Margin.Top + e.VerticalChange;

                    if (startMargin + e.HorizontalChange <= 0)
                    {
                        startMargin = 0;
                        endMargin = ActualHeight - 2 * ThumbSize - (RangeEnd - RangeStart) / (Maximum - Minimum) * (ActualHeight - 2 * ThumbSize);
                    }
                    else if (endMargin - e.HorizontalChange <= 0)
                    {
                        endMargin = 0;
                        startMargin = ActualHeight - 2 * ThumbSize - (RangeEnd - RangeStart) / (Maximum - Minimum) * (ActualHeight - 2 * ThumbSize);
                    }

                    if (!double.IsNaN(startMargin) && !double.IsNaN(endMargin))
                    {
                        _selectedRangeBorder.Margin = new Thickness(_selectedRangeBorder.Margin.Left, endMargin, _selectedRangeBorder.Margin.Right, startMargin);
                    }
                }

                UpdateRange(true);
            }
        }

        private void RangeEndThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (_selectedRangeBorder != null)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    double startMargin = _selectedRangeBorder.Margin.Left;
                    double endMargin = Math.Min(ActualWidth - _selectedRangeBorder.MinWidth - ThumbSize, Math.Max(0, _selectedRangeBorder.Margin.Right - e.HorizontalChange));

                    if (ActualWidth - startMargin - endMargin - ThumbSize < _selectedRangeBorder.MinWidth)
                    {
                        startMargin = ActualWidth - endMargin - ThumbSize - _selectedRangeBorder.MinWidth;
                    }

                    _selectedRangeBorder.Margin = new Thickness(startMargin, _selectedRangeBorder.Margin.Top, endMargin, _selectedRangeBorder.Margin.Bottom);
                }
                else
                {
                    double startMargin = _selectedRangeBorder.Margin.Bottom;
                    double endMargin = Math.Min(ActualHeight - _selectedRangeBorder.MinHeight - ThumbSize, Math.Max(0, _selectedRangeBorder.Margin.Top + e.VerticalChange));

                    if (ActualHeight - startMargin - endMargin - ThumbSize < _selectedRangeBorder.MinHeight)
                    {
                        startMargin = ActualHeight - endMargin - ThumbSize - _selectedRangeBorder.MinHeight;
                    }

                    _selectedRangeBorder.Margin = new Thickness(_selectedRangeBorder.Margin.Left, endMargin, _selectedRangeBorder.Margin.Right, startMargin);
                }
                UpdateRange(true);
            }
        }

        private void UpdateSelectedRangeMinimumActualSpan()
        {
            if (_selectedRangeBorder != null && _rangeStartThumb != null && _rangeEndThumb != null)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    _selectedRangeBorder.MinWidth = ThumbSize + MinimumRangeSpan / (Maximum - Minimum) * (ActualWidth - 2 * ThumbSize);
                }
                else
                {
                    _selectedRangeBorder.MinHeight = ThumbSize + MinimumRangeSpan / (Maximum - Minimum) * (ActualHeight - 2 * ThumbSize);
                }
            }
        }

        private void UpdateSlider()
        {
            if (_selectedRangeBorder != null)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    double startMargin = RangeStart / (Maximum - Minimum) * (ActualWidth - 2 * ThumbSize);
                    double endMargin = (Maximum - RangeEnd) / (Maximum - Minimum) * (ActualWidth - 2 * ThumbSize);

                    if (!double.IsNaN(startMargin) && !double.IsNaN(endMargin))
                    {
                        _selectedRangeBorder.Margin = new Thickness(startMargin, _selectedRangeBorder.Margin.Top, endMargin, _selectedRangeBorder.Margin.Bottom);
                    }
                }
                else
                {
                    double startMargin = RangeStart / (Maximum - Minimum) * (ActualHeight - 2 * ThumbSize);
                    double endMargin = (Maximum - RangeEnd) / (Maximum - Minimum) * (ActualHeight - 2 * ThumbSize);

                    if (!double.IsNaN(startMargin) && !double.IsNaN(endMargin))
                    {
                        _selectedRangeBorder.Margin = new Thickness(_selectedRangeBorder.Margin.Left, endMargin, _selectedRangeBorder.Margin.Right, startMargin);
                    }
                }
            }
        }

        private void UpdateRange(bool raiseEvent)
        {
            if (_selectedRangeBorder != null)
            {
                bool rangeChanged = false;
                double rangeStart, rangeEnd;
                if (Orientation == Orientation.Horizontal)
                {
                    rangeStart = (Maximum - Minimum) * _selectedRangeBorder.Margin.Left / (ActualWidth - 2 * ThumbSize) + Minimum;
                    rangeEnd = Maximum - (Maximum - Minimum) * _selectedRangeBorder.Margin.Right / (ActualWidth - 2 * ThumbSize);
                }
                else
                {
                    rangeStart = (Maximum - Minimum) * _selectedRangeBorder.Margin.Bottom / (ActualHeight - 2 * ThumbSize) + Minimum;
                    rangeEnd = Maximum - (Maximum - Minimum) * (_selectedRangeBorder.Margin.Top / (ActualHeight - 2 * ThumbSize));
                }

                if (rangeEnd - rangeStart < MinimumRangeSpan)
                {
                    if (rangeStart + MinimumRangeSpan > Maximum)
                    {
                        rangeStart = Maximum - MinimumRangeSpan;
                    }

                    rangeEnd = Math.Min(Maximum, rangeStart + MinimumRangeSpan);
                }

                if (rangeStart != RangeStart || rangeEnd != RangeEnd)
                {
                    rangeChanged = true;
                }

                RangeStart = rangeStart;
                RangeEnd = rangeEnd;

                if (raiseEvent && rangeChanged && RangeChanged != null)
                {
                    RangeChanged(this, EventArgs.Empty);
                }
            }
        }
    }
}
