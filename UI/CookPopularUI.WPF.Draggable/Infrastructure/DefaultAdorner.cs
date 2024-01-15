﻿/*
 *Description: DefaultAdorner
 *Author: Chance.zheng
 *Creat Time: 2024/1/8 14:08:57
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
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CookPopularUI.WPF.Draggable
{
    public class DefaultAdorner : Adorner
    {
        private UIElement _child;
        private Point _adornerOrigin;
        private Point _adornerOffset;

        /// <summary>
        /// Create an adorner.
        /// The created adorner must then be added to the AdornerLayer.
        /// </summary>
        /// <param name="adornedElement">Element whose AdornerLayer will be use for displaying the adorner</param>
        /// <param name="adornerElement">Element used as adorner</param>
        /// <param name="adornerOrigin">Origin offset within the adorner</param>
        /// <param name="opacity">Adorner's opacity</param>
        public DefaultAdorner(UIElement adornedElement, UIElement adornerElement, Point adornerOrigin, double opacity) : base(adornedElement)
        {
            Rectangle rect = new Rectangle();
            rect.Width = adornerElement.RenderSize.Width;
            rect.Height = adornerElement.RenderSize.Height;

            VisualBrush visualBrush = new VisualBrush(adornerElement);
            visualBrush.Opacity = opacity;
            visualBrush.Stretch = Stretch.None;
            rect.Fill = visualBrush;

            _child = rect;
            _adornerOrigin = adornerOrigin;
        }

        /// <summary>
        /// Create an adorner with default opacity.
        /// The created adorner must then be added to the AdornerLayer.
        /// </summary>
        /// <param name="adornedElement">Element whose AdornerLayer will be use for displaying the adorner</param>
        /// <param name="adornerElement">Element used as adorner</param>
        /// <param name="adornerOrigin">Origin offset within the adorner</param>
        public DefaultAdorner(UIElement adornedElement, UIElement adornerElement, Point adornerOrigin) : this(adornedElement, adornerElement, adornerOrigin, 0.3)
        {
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup newTransform = new GeneralTransformGroup();
            newTransform.Children.Add(base.GetDesiredTransform(transform));
            newTransform.Children.Add(new TranslateTransform(_adornerOffset.X, _adornerOffset.Y));
            return newTransform;
        }

        /// <summary>
        /// Set the position of and redraw the adorner.
        /// Call when the mouse cursor position changes.
        /// </summary>
        /// <param name="position">Adorner's new position relative to AdornerLayer origin</param>
        public void SetMousePosition(Point position)
        {
            _adornerOffset.X = position.X - _adornerOrigin.X;
            _adornerOffset.Y = position.Y - _adornerOrigin.Y;
            UpdatePosition();
        }

        protected override int VisualChildrenCount => 1;

        protected override Visual GetVisualChild(int index)
        {
            System.Diagnostics.Debug.Assert(index == 0, "Index must be 0, there's only one child");
            return _child;
        }

        protected override Size MeasureOverride(Size finalSize)
        {
            _child.Measure(finalSize);
            return _child.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _child.Arrange(new Rect(finalSize));
            return finalSize;
        }

        private void UpdatePosition()
        {
            AdornerLayer adornerLayer = (AdornerLayer)Parent;
            if (adornerLayer != null)
            {
                adornerLayer.Update(AdornedElement);
            }
        }
    }
}
