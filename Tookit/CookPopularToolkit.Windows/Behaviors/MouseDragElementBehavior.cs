﻿/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：MouseDragElementBehavior
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-07 17:04:38
 */


using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Image = System.Windows.Controls.Image;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using MouseEventHandler = System.Windows.Input.MouseEventHandler;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace CookPopularToolkit.Windows
{
    class MyClass
    {
        /// <summary>
        /// 鼠标拖动行为，该类是反编译微软的System.Windows.Interactivity程序集得到的
        /// </summary>
        private class MouseDragElementBehavior : Behavior<FrameworkElement>
        {
            public static readonly DependencyProperty XProperty = DependencyProperty.Register(nameof(X), typeof(double),
                typeof(MouseDragElementBehavior), new PropertyMetadata(double.NaN, OnXChanged));

            public static readonly DependencyProperty YProperty = DependencyProperty.Register(nameof(Y), typeof(double),
                typeof(MouseDragElementBehavior), new PropertyMetadata(double.NaN, OnYChanged));

            public static readonly DependencyProperty ConstrainToParentBoundsProperty =
                DependencyProperty.Register(nameof(ConstrainToParentBounds), typeof(bool),
                    typeof(MouseDragElementBehavior), new PropertyMetadata(CookPopularToolkit.ValueBoxes.FalseBox, OnConstrainToParentBoundsChanged));

            private Transform? _cachedRenderTransform;
            private Point _relativePosition;
            private bool _settingPosition;

            /// <summary>
            ///     是否固定住Y轴
            /// </summary>
            public bool LockY { get; set; }

            /// <summary>
            ///     是否固定住X轴
            /// </summary>
            public bool LockX { get; set; }

            public double X
            {
                get => (double)GetValue(XProperty);
                set => SetValue(XProperty, value);
            }

            public double Y
            {
                get => (double)GetValue(YProperty);
                set => SetValue(YProperty, value);
            }

            public bool ConstrainToParentBounds
            {
                get => (bool)GetValue(ConstrainToParentBoundsProperty);
                set => SetValue(ConstrainToParentBoundsProperty, CookPopularToolkit.ValueBoxes.BooleanBox(value));
            }

            private Rect ElementBounds
            {
                get
                {
                    var layoutRect = GetLayoutRect(AssociatedObject);
                    return new Rect(new Point(0.0, 0.0), new Size(layoutRect.Width, layoutRect.Height));
                }
            }

            /// <summary>
            /// 获取布局范围框
            /// </summary>
            /// <param name="element"></param>
            /// <returns></returns>
            private Rect GetLayoutRect(FrameworkElement element)
            {
                var num1 = element.ActualWidth;
                var num2 = element.ActualHeight;
                if (element is Image || element is MediaElement)
                    if (element.Parent is Canvas)
                    {
                        num1 = double.IsNaN(element.Width) ? num1 : element.Width;
                        num2 = double.IsNaN(element.Height) ? num2 : element.Height;
                    }
                    else
                    {
                        num1 = element.RenderSize.Width;
                        num2 = element.RenderSize.Height;
                    }
                var width = element.Visibility == Visibility.Collapsed ? 0.0 : num1;
                var height = element.Visibility == Visibility.Collapsed ? 0.0 : num2;
                var margin = element.Margin;
                var layoutSlot = LayoutInformation.GetLayoutSlot(element);
                var x = 0.0;
                var y = 0.0;
                switch (element.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        x = layoutSlot.Left + margin.Left;
                        break;
                    case HorizontalAlignment.Center:
                        x = (layoutSlot.Left + margin.Left + layoutSlot.Right - margin.Right) / 2.0 - width / 2.0;
                        break;
                    case HorizontalAlignment.Right:
                        x = layoutSlot.Right - margin.Right - width;
                        break;
                    case HorizontalAlignment.Stretch:
                        x = Math.Max(layoutSlot.Left + margin.Left,
                            (layoutSlot.Left + margin.Left + layoutSlot.Right - margin.Right) / 2.0 - width / 2.0);
                        break;
                }
                switch (element.VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        y = layoutSlot.Top + margin.Top;
                        break;
                    case VerticalAlignment.Center:
                        y = (layoutSlot.Top + margin.Top + layoutSlot.Bottom - margin.Bottom) / 2.0 - height / 2.0;
                        break;
                    case VerticalAlignment.Bottom:
                        y = layoutSlot.Bottom - margin.Bottom - height;
                        break;
                    case VerticalAlignment.Stretch:
                        y = Math.Max(layoutSlot.Top + margin.Top,
                            (layoutSlot.Top + margin.Top + layoutSlot.Bottom - margin.Bottom) / 2.0 - height / 2.0);
                        break;
                }
                return new Rect(x, y, width, height);
            }

            private FrameworkElement? ParentElement => AssociatedObject.Parent as FrameworkElement;

            private UIElement? RootElement
            {
                get
                {
                    DependencyObject reference = AssociatedObject;
                    for (var dependencyObject = reference; dependencyObject != null; dependencyObject = VisualTreeHelper.GetParent(reference))
                        reference = dependencyObject;
                    return reference as UIElement;
                }
            }

            private Transform? RenderTransform
            {
                get
                {
                    if (_cachedRenderTransform == null ||
                        !ReferenceEquals(_cachedRenderTransform, AssociatedObject.RenderTransform))
                        RenderTransform = CloneTransform(AssociatedObject.RenderTransform);
                    return _cachedRenderTransform;
                }
                set
                {
                    if (Equals(_cachedRenderTransform, value))
                        return;
                    _cachedRenderTransform = value;
                    AssociatedObject.RenderTransform = value;
                }
            }

            public event MouseEventHandler DragBegun;

            public event MouseEventHandler Dragging;

            public event MouseEventHandler DragFinished;

            private static void OnXChanged(object sender, DependencyPropertyChangedEventArgs args)
            {
                var dragElementBehavior = (MouseDragElementBehavior)sender;
                dragElementBehavior.UpdatePosition(new Point((double)args.NewValue, dragElementBehavior.Y));
            }

            private static void OnYChanged(object sender, DependencyPropertyChangedEventArgs args)
            {
                var dragElementBehavior = (MouseDragElementBehavior)sender;
                dragElementBehavior.UpdatePosition(new Point(dragElementBehavior.X, (double)args.NewValue));
            }

            private static void OnConstrainToParentBoundsChanged(object sender, DependencyPropertyChangedEventArgs args)
            {
                var dragElementBehavior = (MouseDragElementBehavior)sender;
                dragElementBehavior.UpdatePosition(new Point(dragElementBehavior.X, dragElementBehavior.Y));
            }

            private void UpdatePosition(Point point)
            {
                if (_settingPosition || AssociatedObject == null)
                    return;
                var transformOffset = GetTransformOffset(AssociatedObject.TransformToVisual(RootElement));
                ApplyTranslation(double.IsNaN(point.X) ? 0.0 : point.X - transformOffset.X,
                    double.IsNaN(point.Y) ? 0.0 : point.Y - transformOffset.Y);
            }

            private void ApplyTranslation(double x, double y)
            {
                if (ParentElement == null)
                    return;
                var point = TransformAsVector(RootElement!.TransformToVisual(ParentElement), x, y);
                x = point.X;
                y = point.Y;
                if (ConstrainToParentBounds)
                {
                    var parentElement = ParentElement;
                    var rect1 = new Rect(0.0, 0.0, parentElement.ActualWidth, parentElement.ActualHeight);
                    var rect2 = AssociatedObject.TransformToVisual(parentElement).TransformBounds(ElementBounds);
                    rect2.X += x;
                    rect2.Y += y;
                    if (!RectContainsRect(rect1, rect2))
                    {
                        if (rect2.X < rect1.Left)
                        {
                            var num = rect2.X - rect1.Left;
                            x -= num;
                        }
                        else if (rect2.Right > rect1.Right)
                        {
                            var num = rect2.Right - rect1.Right;
                            x -= num;
                        }
                        if (rect2.Y < rect1.Top)
                        {
                            var num = rect2.Y - rect1.Top;
                            y -= num;
                        }
                        else if (rect2.Bottom > rect1.Bottom)
                        {
                            var num = rect2.Bottom - rect1.Bottom;
                            y -= num;
                        }
                    }
                }
                ApplyTranslationTransform(x, y);
            }

            internal void ApplyTranslationTransform(double x, double y)
            {
                var renderTransform = RenderTransform;
                var translateTransform = renderTransform as TranslateTransform;
                if (translateTransform == null)
                {
                    var matrixTransform = renderTransform as MatrixTransform;
                    if (renderTransform is TransformGroup transformGroup1)
                    {
                        if (transformGroup1.Children.Count > 0)
                            translateTransform =
                                transformGroup1.Children[transformGroup1.Children.Count - 1] as TranslateTransform;
                        if (translateTransform == null)
                        {
                            translateTransform = new TranslateTransform();
                            transformGroup1.Children.Add(translateTransform);
                        }
                    }
                    else
                    {
                        if (matrixTransform != null)
                        {
                            var matrix = matrixTransform.Matrix;
                            //在该处对微软的类进行了修改
                            if (!LockX)
                            {
                                matrix.OffsetX += x;
                            }
                            if (!LockY)
                            {
                                matrix.OffsetY += y;
                            }
                            //修改结束
                            RenderTransform = new MatrixTransform
                            {
                                Matrix = matrix
                            };
                            return;
                        }
                        var transformGroup2 = new TransformGroup();
                        translateTransform = new TranslateTransform();
                        if (renderTransform != null)
                            transformGroup2.Children.Add(renderTransform);
                        transformGroup2.Children.Add(translateTransform);
                        RenderTransform = transformGroup2;
                    }
                }
                //在该处对微软的类进行了修改
                if (!LockX)
                {
                    translateTransform.X += x;
                }
                if (!LockY)
                {
                    translateTransform.Y += y;
                }
                //修改结束
            }

            internal static Transform? CloneTransform(Transform? transform)
            {
                if (transform == null)
                    return null;
                if (transform is ScaleTransform scaleTransform)
                    return new ScaleTransform
                    {
                        CenterX = scaleTransform.CenterX,
                        CenterY = scaleTransform.CenterY,
                        ScaleX = scaleTransform.ScaleX,
                        ScaleY = scaleTransform.ScaleY
                    };
                if (transform is RotateTransform rotateTransform)
                    return new RotateTransform
                    {
                        Angle = rotateTransform.Angle,
                        CenterX = rotateTransform.CenterX,
                        CenterY = rotateTransform.CenterY
                    };
                if (transform is SkewTransform skewTransform)
                    return new SkewTransform
                    {
                        AngleX = skewTransform.AngleX,
                        AngleY = skewTransform.AngleY,
                        CenterX = skewTransform.CenterX,
                        CenterY = skewTransform.CenterY
                    };
                if (transform is TranslateTransform translateTransform)
                    return new TranslateTransform
                    {
                        X = translateTransform.X,
                        Y = translateTransform.Y
                    };
                if (transform is MatrixTransform matrixTransform)
                    return new MatrixTransform
                    {
                        Matrix = matrixTransform.Matrix
                    };
                if (!(transform is TransformGroup transformGroup1))
                    return null;
                var transformGroup2 = new TransformGroup();
                foreach (var child in transformGroup1.Children)
                    transformGroup2.Children.Add(CloneTransform(child));
                return transformGroup2;
            }

            private void UpdatePosition()
            {
                var transformOffset = GetTransformOffset(AssociatedObject.TransformToVisual(RootElement));
                X = transformOffset.X;
                Y = transformOffset.Y;
            }

            internal void StartDrag(System.Windows.Point positionInElementCoordinates)
            {
                _relativePosition = positionInElementCoordinates;
                AssociatedObject.CaptureMouse();
                AssociatedObject.MouseMove += OnMouseMove;
                AssociatedObject.LostMouseCapture += OnLostMouseCapture;
                AssociatedObject.AddHandler(UIElement.MouseLeftButtonUpEvent,
                    new MouseButtonEventHandler(OnMouseLeftButtonUp), false);
            }

            internal void HandleDrag(Point newPositionInElementCoordinates)
            {
                var point = TransformAsVector(AssociatedObject.TransformToVisual(RootElement),
                    newPositionInElementCoordinates.X - _relativePosition.X,
                    newPositionInElementCoordinates.Y - _relativePosition.Y);
                _settingPosition = true;
                ApplyTranslation(point.X, point.Y);
                UpdatePosition();
                _settingPosition = false;
            }

            internal void EndDrag()
            {
                AssociatedObject.MouseMove -= OnMouseMove;
                AssociatedObject.LostMouseCapture -= OnLostMouseCapture;
                AssociatedObject.RemoveHandler(UIElement.MouseLeftButtonUpEvent,
                    new MouseButtonEventHandler(OnMouseLeftButtonUp));
            }

            private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                StartDrag(e.GetPosition(AssociatedObject));
                DragBegun?.Invoke(this, e);
            }

            private void OnLostMouseCapture(object sender, MouseEventArgs e)
            {
                EndDrag();
                DragFinished?.Invoke(this, e);
            }

            private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            {
                AssociatedObject.ReleaseMouseCapture();
            }

            private void OnMouseMove(object sender, MouseEventArgs e)
            {
                HandleDrag(e.GetPosition(AssociatedObject));
                Dragging?.Invoke(this, e);
            }

            private static bool RectContainsRect(Rect rect1, Rect rect2)
            {
                if (rect1.IsEmpty || rect2.IsEmpty || rect1.X > rect2.X || rect1.Y > rect2.Y ||
                    rect1.X + rect1.Width < rect2.X + rect2.Width)
                    return false;
                return rect1.Y + rect1.Height >= rect2.Y + rect2.Height;
            }

            private static Point TransformAsVector(GeneralTransform transform, double x, double y)
            {
                var point1 = transform.Transform(new Point(0.0, 0.0));
                var point2 = transform.Transform(new Point(x, y));
                return new Point(point2.X - point1.X, point2.Y - point1.Y);
            }

            private static Point GetTransformOffset(GeneralTransform transform)
            {
                return transform.Transform(new Point(0.0, 0.0));
            }

            protected override void OnAttached()
            {
                AssociatedObject.AddHandler(UIElement.MouseLeftButtonDownEvent,
                    new MouseButtonEventHandler(OnMouseLeftButtonDown), false);
            }

            protected override void OnDetaching()
            {
                AssociatedObject.RemoveHandler(UIElement.MouseLeftButtonDownEvent,
                    new MouseButtonEventHandler(OnMouseLeftButtonDown));
            }
        }
    }
}
