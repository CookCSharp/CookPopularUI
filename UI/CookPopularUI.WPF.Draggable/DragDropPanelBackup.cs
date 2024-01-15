///*
// *Description: DragDropPanel
// *Author: Chance.zheng
// *Creat Time: 2024/1/4 11:37:25
// *.Net Version: 4.6
// *CLR Version: 4.0.30319.42000
// *Copyright © CookCSharp 2024 All Rights Reserved.
// */


//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Xml.Linq;

//namespace CookPopularUI.WPF.Draggable
//{
//    /// <summary>
//    /// 拖拽面板
//    /// </summary>
//    public class DragDropPanel : Panel
//    {
//        private readonly Dictionary<UIElement, Rect> ElementsPosion = new Dictionary<UIElement, Rect>();

//        private static DragDropPanel? _originalDragDropPanel;
//        private static DragDropPanel? _currentDragDropPanel;
//        private static FrameworkElement? _draggableTransferElement; //通过拖拽传递到下一个DragDropPanel的控件

//        private FrameworkElement? _draggableElement;
//        private Point _mouseLeftButtonDownPoint;
//        private int _draggableElementZIndex;


//        public Orientation Orientation
//        {
//            get => (Orientation)GetValue(OrientationProperty);
//            set => SetValue(OrientationProperty, value);
//        }
//        /// <summary>
//        /// 提供<see cref="Orientation"/>的依赖属性
//        /// </summary>
//        public static readonly DependencyProperty OrientationProperty =
//            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(DragDropPanel), new PropertyMetadata(Orientation.Vertical));

//        public DragDropPanel()
//        {
//            Background = new SolidColorBrush(Colors.Transparent);
//        }

//        protected override Size MeasureOverride(Size availableSize)
//        {
//            var panelDesiredSize = new Size();
//            foreach (UIElement child in InternalChildren)
//            {
//                child.Measure(availableSize);

//                if (Orientation == Orientation.Vertical)
//                {
//                    panelDesiredSize.Width = double.IsInfinity(availableSize.Width) ? child.DesiredSize.Width : availableSize.Width;
//                    panelDesiredSize.Height += child.DesiredSize.Height;
//                }
//                else
//                {
//                    panelDesiredSize.Width += child.DesiredSize.Height;
//                    panelDesiredSize.Height = double.IsInfinity(availableSize.Height) ? child.DesiredSize.Height : availableSize.Height;
//                }
//            }

//            return panelDesiredSize;
//        }

//        protected override Size ArrangeOverride(Size finalSize)
//        {
//            double x = 0, y = 0;
//            foreach (UIElement child in InternalChildren)
//            {
//                var point = new Point(x, y);
//                var width = child.DesiredSize.Width;
//                var height = child.DesiredSize.Height;

//                if (Orientation == Orientation.Vertical)
//                    width = finalSize.Width;
//                else
//                    height = finalSize.Height;

//                var size = new Size(width, height);
//                var rect = new Rect(point, size);
//                child.Arrange(rect);
//                if (ElementsPosion.ContainsKey(child))
//                    ElementsPosion[child] = rect;
//                else
//                    ElementsPosion.Add(child, rect);

//                if (Orientation == Orientation.Vertical)
//                    y += child.DesiredSize.Height;
//                else
//                    x += child.DesiredSize.Width;
//            }

//            return finalSize;
//        }

//        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
//        {
//            var position = e.GetPosition(this);
//            var hitTestResult = InputHitTest(position) as FrameworkElement;
//            var parentElements = GetVisualAncestorsAndSelfIterator(hitTestResult!).Skip(1);
//            _draggableElement = (FrameworkElement)LogicalTreeHelper.GetChildren(this)
//                                                                   .OfType<DependencyObject>()
//                                                                   .Intersect(parentElements)
//                                                                   .FirstOrDefault();
//            if (_draggableElement != null && InternalChildren.Contains(_draggableElement))
//            {
//                _mouseLeftButtonDownPoint = e.GetPosition(_draggableElement);
//                _draggableElementZIndex = GetZIndex(_draggableElement);
//                SetZIndex(_draggableElement, InternalChildren.Count);
//                //AddDecorate(_draggableElement);

//                e.Handled = true;
//            }

//            base.OnPreviewMouseLeftButtonDown(e);
//        }

//        protected override void OnPreviewMouseMove(MouseEventArgs e)
//        {
//            var point = e.GetPosition(this);
//            if (e.LeftButton == MouseButtonState.Pressed && _draggableElement != null)
//            {
//                //当前拖拽控件不可命中测试，以供命中下一层的换位控件
//                _draggableElement.IsHitTestVisible = false;
//                if (GetZIndex(_draggableElement) == InternalChildren.Count)
//                {
//                    var targetPoint = new Point(point.X - _mouseLeftButtonDownPoint.X - _draggableElement.Margin.Left, point.Y - _mouseLeftButtonDownPoint.Y - _draggableElement.Margin.Top);
//                    var draggableElementOriginalPosition = GetElementPosion(_draggableElement);
//                    var offset = new Point(targetPoint.X - draggableElementOriginalPosition.X, targetPoint.Y - draggableElementOriginalPosition.Y);
//                    _draggableElement.RenderTransform = new TranslateTransform(offset.X, offset.Y);
//                }

//                e.Handled = true;
//            }

//            base.OnPreviewMouseMove(e);
//        }

//        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
//        {
//            _mouseLeftButtonDownPoint = default;

//            if (_draggableElement != null)
//            {
//                //RemoveDecorate(_draggableElement);
//                SetZIndex(_draggableElement, _draggableElementZIndex);
//                _draggableElement.IsHitTestVisible = true;
//                _draggableElement.RenderTransform = null;
//                _draggableElement = null;
//            }

//            e.Handled = true;

//            base.OnPreviewMouseLeftButtonUp(e);
//        }

//        protected override void OnMouseEnter(MouseEventArgs e)
//        {
//            //_originalDragDropPanel = _currentDragDropPanel;
//            _currentDragDropPanel = e.Source as DragDropPanel;

//            if (_currentDragDropPanel != null && _originalDragDropPanel != null && _draggableTransferElement != null)
//            {
//                _originalDragDropPanel.Children.Remove(_draggableTransferElement);
//                _originalDragDropPanel._draggableElement = null;

//                SetZIndex(_draggableTransferElement, InternalChildren.Count + 1);
//                _currentDragDropPanel.Children.Add(_draggableTransferElement);
//                //this.AddOverlay(_draggableTransferElement);
//            }

//            e.Handled = true;

//            base.OnMouseEnter(e);
//        }

//        protected override void OnMouseLeave(MouseEventArgs e)
//        {
//            _originalDragDropPanel = e.Source as DragDropPanel;
//            _draggableTransferElement = _draggableElement;

//            base.OnMouseLeave(e);
//        }


//        private Point GetElementPosion(UIElement element)
//        {
//            var existElement = ElementsPosion.TryGetValue(element, out var posion);
//            if (existElement)
//            {
//                return new Point(posion.X, posion.Y);
//            }

//            return default;
//        }

//        private IEnumerable<DependencyObject> GetVisualAncestorsAndSelfIterator(DependencyObject element)
//        {
//            Debug.Assert(element != null, "element should not be null!");

//            for (DependencyObject obj = element!; obj != null; obj = VisualTreeHelper.GetParent(obj))
//            {
//                yield return obj;
//            }
//        }
//    }
//}
