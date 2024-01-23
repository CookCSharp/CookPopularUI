/*
 *Description: DragManager
 *Author: Chance.zheng
 *Creat Time: 2024/1/8 14:17:31
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace CookPopularUI.WPF.Draggable
{
    /// <summary>
    /// Manage drag events for IDataProviders
    /// </summary>
    public class DragManager
    {
        private UIElement _dragSource;
        private IDataProvider[] _dragDropObjects;

        private IDataProvider? _dragDropObject;
        private Point _startPosition;
        private bool _isDragging;

        /// <summary>
        /// Manage dragging data object from <code>dragSource</code> FrameworkElement.
        /// Hook various PreviewMouse* events in order to determine when a drag starts.
        /// </summary>
        /// <param name="dragSource">The FrameworkElement which contains objects to be dragged</param>
        /// <param name="dragDropObjects">Array of objects to be dragged, implementing IDataProvider</param>
        public DragManager(FrameworkElement dragSource, IDataProvider[] dragDropObjects)
        {
            _dragSource = dragSource;
            Debug.Assert(dragSource != null, "dragSource cannot be null");
            _dragDropObjects = dragDropObjects;

            _dragSource.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(DragSource_PreviewMouseLeftButtonDown);
            _dragSource.PreviewMouseMove += new MouseEventHandler(DragSource_PreviewMouseMove);
            _dragSource.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(DragSource_PreviewMouseLeftButtonUp);
        }

        /// <summary>
        /// Manage dragging data object from <code>dragSource</code> FrameworkElement.
        /// Hook various PreviewMouse* events in order to determine when a drag starts.
        /// </summary>
        /// <param name="dragSource">The FrameworkElement which contains objects to be dragged</param>
        /// <param name="dragDropObject">Object to be dragged, implementing IDataProvider</param>
        public DragManager(FrameworkElement dragSource, IDataProvider dragDropObject) : this(dragSource, new IDataProvider[] { dragDropObject })
        {
        }

        public void Unregister()
        {
            _dragSource.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(DragSource_PreviewMouseLeftButtonDown);
            _dragSource.PreviewMouseMove -= new MouseEventHandler(DragSource_PreviewMouseMove);
            _dragSource.PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(DragSource_PreviewMouseLeftButtonUp);
        }

        /// <summary>
        /// Check for a supported SourceContainer/SourceObject.
        /// If found, get ready for a possible drag operation.
        /// </summary>
        private void DragSource_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            foreach (IDataProvider dragDropObject in _dragDropObjects)
            {
                if (dragDropObject.IsSupportedContainerAndObject(true, sender, e.Source, e.OriginalSource))
                {
                    Debug.Assert(sender.Equals(_dragSource));

                    _dragDropObject = dragDropObject;

                    _startPosition = e.GetPosition(sender as IInputElement);

                    _dragDropObject.StartPosition = e.GetPosition(e.Source as IInputElement);

                    if (_dragDropObject.NeedsCaptureMouse)
                        _dragSource.CaptureMouse();

                    break;
                }
            }
        }

        /// <summary>
        /// If the mouse is moved (dragged) a minimum distance
        /// over a supported SourceContainer/SourceObject,
        /// initiate a drag operation.
        /// </summary>
        private void DragSource_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if ((_dragDropObject != null) && !_isDragging && _dragDropObject.IsSupportedContainerAndObject(false, sender, e.Source, e.OriginalSource))
            {
                Point currentPosition = e.GetPosition(sender as IInputElement);
                if ((Math.Abs(currentPosition.X - _startPosition.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    (Math.Abs(currentPosition.Y - _startPosition.Y) > SystemParameters.MinimumVerticalDragDistance))
                {

                    //While dragging a ListBoxItem, another one can be selected
                    //This doesn't seem to happen with TreeView or TabControl
                    if (sender is ListBox)
                        _dragDropObject.SourceObject = e.Source;

                    _isDragging = true;

                    if (_dragDropObject.AddAdorner)
                    {
                        _dragDropObject.DragAdorner = new DefaultAdorner((UIElement)Application.Current.MainWindow.Content, (UIElement)_dragDropObject.SourceObject, _dragDropObject.StartPosition);
                        Visual visual = (Visual)Application.Current.MainWindow.Content;
                        AdornerLayer.GetAdornerLayer(visual).Add(_dragDropObject.DragAdorner);
                    }

                    DragDropEffects resultEffects = DoDragDrop_Start(e);

                    if (_dragDropObject.NeedsCaptureMouse)
                        _dragSource.ReleaseMouseCapture();

                    DoDragDrop_Done(resultEffects);

                    if (_dragDropObject.AddAdorner)
                    {
                        AdornerLayer.GetAdornerLayer((Visual)Application.Current.MainWindow.Content).Remove(_dragDropObject.DragAdorner);
                    }

                    Mouse.OverrideCursor = null;

                    _dragDropObject = null;
                    _isDragging = false;
                }
            }
        }

        /// <summary>
        /// When MouseLeftButtonUp event occurs, abandon
        /// any drag operation that may be in progress
        /// </summary>
        private void DragSource_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_dragDropObject != null)
            {
                if (_dragDropObject.NeedsCaptureMouse)
                    _dragSource.ReleaseMouseCapture();
                _dragDropObject = null;
                _isDragging = false;
            }
        }

        private void DragSource_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if ((_dragDropObject?.DataProviderActions & DataProviderActions.QueryContinueDrag) != 0)
                _dragDropObject?.DragSource_QueryContinueDrag(sender, e);
        }

        private void DragSource_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (_dragDropObject != null && _dragDropObject.AddAdorner)
            {
                Point point = Interop.GetWin32CursorPos();
                DefaultAdorner dragAdorner = _dragDropObject.DragAdorner;
                dragAdorner.SetMousePosition(dragAdorner.AdornedElement.PointFromScreen(point));
            }

            if ((_dragDropObject?.DataProviderActions & DataProviderActions.GiveFeedback) != 0)
                _dragDropObject?.DragSource_GiveFeedback(sender, e);
        }

        /// <summary>
        /// Prepare for and begin a drag operation.
        /// Hook the events needed by the data provider.
        /// </summary>
        private DragDropEffects DoDragDrop_Start(MouseEventArgs e)
        {
            DragDropEffects resultEffects = DragDropEffects.None;

            DataObject data = new DataObject();
            _dragDropObject?.SetData(ref data);

            bool hookQueryContinueDrag = false;
            bool hookGiveFeedback = false;

            if ((_dragDropObject?.DataProviderActions & DataProviderActions.QueryContinueDrag) != 0)
                hookQueryContinueDrag = true;

            if ((_dragDropObject?.DataProviderActions & DataProviderActions.GiveFeedback) != 0)
                hookGiveFeedback = true;

            if (_dragDropObject != null && _dragDropObject.AddAdorner)
                hookGiveFeedback = true;

            QueryContinueDragEventHandler? queryContinueDrag = null;
            GiveFeedbackEventHandler? giveFeedback = null;

            if (hookQueryContinueDrag)
            {
                queryContinueDrag = new QueryContinueDragEventHandler(DragSource_QueryContinueDrag);
                _dragSource.QueryContinueDrag += queryContinueDrag;
            }
            if (hookGiveFeedback)
            {
                giveFeedback = new GiveFeedbackEventHandler(DragSource_GiveFeedback);
                _dragSource.GiveFeedback += giveFeedback;
            }

            try
            {
                //Set 'dragSource' to desired value (dragSource or item being dragged)
                //'dragSource' is passed to QueryContinueDrag as Source and OriginalSource
                DependencyObject dragSource;
                dragSource = _dragSource;
                resultEffects = DragDrop.DoDragDrop(dragSource, data, _dragDropObject!.AllowedEffects);
            }
            catch
            {
                Debug.WriteLine("DragDrop.DoDragDrop threw an exception");
            }

            if (queryContinueDrag != null)
                _dragSource.QueryContinueDrag -= queryContinueDrag;
            if (giveFeedback != null)
                _dragSource.GiveFeedback -= giveFeedback;

            return resultEffects;
        }

        /// <summary>
        /// Called after DragDrop.DoDragDrop() returns.
        /// Typically during a file move, for example, the file is deleted here.
        /// However, when moving a TabItem from one TabControl to another the
        /// source TabItem must be unparented from the source TabControl
        /// before it can be added to the destination TabControl.
        /// So most of the time when moving items between item controls,
        /// this method isn't used.
        /// </summary>
        /// <param name="resultEffects">The drop operation that was performed</param>
        private void DoDragDrop_Done(DragDropEffects resultEffects)
        {
            if ((_dragDropObject?.DataProviderActions & DataProviderActions.DoDragDrop_Done) != 0)
                _dragDropObject?.DoDragDrop_Done(resultEffects);
        }
    }
}
