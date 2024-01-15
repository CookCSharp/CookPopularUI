/*
 *Description: DragDropPanelBase
 *Author: Chance.zheng
 *Creat Time: 2024/1/5 17:05:14
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
using System.Windows.Input;

namespace CookPopularUI.WPF.Draggable
{
    /// <summary>
    /// 拖拽面板基类
    /// </summary>
    public class DragDropPanelBase : AnimatedHeaderedContentControl
    {
        /// <summary>
        /// The GripBar Template Part Name.
        /// </summary>
        private const string ElementGripBarElement = "GripBarElement";

        /// <summary>
        /// Stores the current z-index
        /// </summary>
        private static int currentZIndex = 1;

        /// <summary>
        /// Stores the last drag position.
        /// </summary>
        private Point lastDragPosition;

        /// <summary>
        /// Is dragging flag.
        /// </summary>
        private bool dragging = false;

        /// <summary>
        /// The drag started event.
        /// </summary>
        public event DragEventHander DragStarted;

        /// <summary>
        /// The drag moved event.
        /// </summary>
        public event DragEventHander DragMoved;

        /// <summary>
        /// The drag finished event.
        /// </summary>
        public event DragEventHander DragFinished;

        /// <summary>
        /// The Panel Focused event.
        /// </summary>
        public event EventHandler PanelFocused;


        /// <summary>
        /// Gets or sets the panels current z-index.
        /// </summary>
        internal static int CurrentZIndex
        {
            get { return currentZIndex; }
            set { currentZIndex = value; }
        }

        /// <summary>
        /// Gets or sets the last drag position.
        /// </summary>
        internal Point LastDragPosition
        {
            get => lastDragPosition;
            set => lastDragPosition = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether dragging is enabled.
        /// </summary>
        public bool DraggingEnabled
        {
            get { return (bool)GetValue(DraggingEnabledProperty); }
            set { SetValue(DraggingEnabledProperty, value); }
        }
        /// <summary>
        /// The DraggingEnabled Dependency Property.
        /// </summary>
        public static readonly DependencyProperty DraggingEnabledProperty =
            DependencyProperty.Register(nameof(DraggingEnabled), typeof(bool), typeof(DragDropPanelBase), new PropertyMetadata(true));


        /// <summary>
        /// Base method for updating the panels position.
        /// </summary>
        /// <param name="pos">The new position.</param>
        public virtual void UpdatePosition(Point pos)
        {
            Canvas.SetLeft(this, Math.Max(0, pos.X));
            Canvas.SetTop(this, Math.Max(0, pos.Y));
        }

        /// <summary>
        /// Base method for updating the panel size.
        /// </summary>
        /// <param name="width">The new width.</param>
        /// <param name="height">The new height.</param>
        public virtual void UpdateSize(double width, double height)
        {
            Width = Math.Max(MinWidth, width);
            Height = Math.Max(MinHeight, height);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            FrameworkElement? gripBarElement = GetTemplateChild(ElementGripBarElement) as FrameworkElement;
            if (gripBarElement != null)
            {
                gripBarElement.MouseLeftButtonDown += new MouseButtonEventHandler(GripBarElement_MouseLeftButtonDown);
                gripBarElement.MouseMove += new MouseEventHandler(GripBarElement_MouseMove);
                gripBarElement.MouseLeftButtonUp += new MouseButtonEventHandler(GripBarElement_MouseLeftButtonUp);
            }
        }

        /// <summary>
        /// Brings the panel to the front of the canvas.
        /// </summary>
        /// <param name="e">Mouse Button Event Args.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Bring the panel to the front
            Panel.SetZIndex(this, CurrentZIndex++);

            if (PanelFocused != null)
            {
                PanelFocused(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Brings the panel to the front.
        /// </summary>
        /// <param name="e">Routed Event Args.</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            // Bring the panel to the front
            Panel.SetZIndex(this, CurrentZIndex++);

            if (PanelFocused != null)
            {
                PanelFocused(this, EventArgs.Empty);
            }
        }


        /// <summary>
        /// Starts the dragging.
        /// </summary>
        /// <param name="sender">The grip bar.</param>
        /// <param name="e">Mouse button event args.</param>
        private void GripBarElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Capture the mouse
            ((FrameworkElement)sender).CaptureMouse();

            // Store the start position
            LastDragPosition = e.GetPosition(sender as UIElement);

            // Set dragging to true
            dragging = true;

            // Fire the drag started event
            if (DragStarted != null)
            {
                DragStarted(this, new DragEventArgs(0, 0, e));
            }
        }

        /// <summary>
        /// Ends the dragging.
        /// </summary>
        /// <param name="sender">The grip bar.</param>
        /// <param name="e">Mouse button event args.</param>
        private void GripBarElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Capture the mouse
            ((FrameworkElement)sender).ReleaseMouseCapture();

            // Set dragging to true
            dragging = false;

            Point position = e.GetPosition(sender as UIElement);

            // Fire the drag finished event
            if (DragFinished != null)
            {
                DragFinished(this, new DragEventArgs(position.X - LastDragPosition.X, position.Y - LastDragPosition.Y, e));
            }
        }

        /// <summary>
        /// Drags the panel (if the panel is being dragged).
        /// </summary>
        /// <param name="sender">The grip bar.</param>
        /// <param name="e">Mouse event args.</param>
        private void GripBarElement_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point position = e.GetPosition(sender as UIElement);
                
                // Move the panel
                UpdatePosition(new Point(Canvas.GetLeft(this) + position.X - LastDragPosition.X, Canvas.GetTop(this) + position.Y - LastDragPosition.Y));

                // Fire the drag moved event
                if (DragMoved != null)
                {
                    DragMoved(this, new DragEventArgs(position.X - LastDragPosition.X, position.Y - LastDragPosition.Y, e));
                }
            }
        }
    }
}
