/*
 *Description: TabControl
 *Author: Chance.zheng
 *Creat Time: 2024/1/8 13:57:18
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
using System.Windows.Input;

namespace CookPopularUI.WPF.Draggable
{
    /// <summary>
    /// This Data Provider represents TabItems.
    /// Note that custom cursors are used.
    /// When a TabItem is dragged within its
    /// original container, the cursor is an arrow,
    /// otherwise its a custom cursor with an
    /// arrow and a page.
    /// </summary>
    /// <typeparam name="TContainer">Drag source container type</typeparam>
    /// <typeparam name="TObject">Drag source object type</typeparam>
    public class TabControlDataProvider<TContainer, TObject> : DataProviderBase<TContainer, TObject>, IDataProvider
        where TContainer : ItemsControl
        where TObject : TabItem
    {
        private static Cursor MovePageCursor = new Cursor(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("CookPopularUI.WPF.Draggable.Assets.MovePage.cur"));
        private static Cursor MovePageNotCursor = new Cursor(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("CookPopularUI.WPF.Draggable.Assets.MovePageNot.cur"));

        public TabControlDataProvider(string dataFormatString) : base(dataFormatString) { }

        public override DragDropEffects AllowedEffects => DragDropEffects.None | DragDropEffects.Move | DragDropEffects.Link;
        public override DataProviderActions DataProviderActions => DataProviderActions.None | DataProviderActions.GiveFeedback;

        public override void DragSource_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (e.Effects == DragDropEffects.Move)
            {
                e.UseDefaultCursors = false;
                Mouse.OverrideCursor = MovePageCursor;
                e.Handled = true;
            }
            else if (e.Effects == DragDropEffects.Link)
            {
                // Drag tabs around
                e.UseDefaultCursors = false;
                Mouse.OverrideCursor = Cursors.Arrow;
                e.Handled = true;
            }
            else
            {
                e.UseDefaultCursors = false;
                Mouse.OverrideCursor = MovePageNotCursor;
                e.Handled = true;
            }
        }

        public override void Unparent()
        {
            TObject? item = SourceObject as TObject;
            TContainer? container = SourceContainer as TContainer;

            Debug.Assert(item != null, "Unparent expects a non-null item");
            Debug.Assert(container != null, "Unparent expects a non-null container");

            if ((container != null) && (item != null))
                container.Items.Remove(item);
        }
    }


    /// <summary>
    /// This data consumer looks for TabItems.
    /// When the TabItem is dragged within its original
    /// control, the TabItems are rearranged accordingly.
    /// When dropped, it is inserted as the first TabItem.
    /// </summary>
    /// <typeparam name="TContainer">Drag source and drop destination container type</typeparam>
    /// <typeparam name="TObject">Drag source and drop destination object type</typeparam>
    public class TabControlDataConsumer<TContainer, TObject> : DataConsumerBase, IDataConsumer
        where TContainer : ItemsControl
        where TObject : TabItem
    {
        public TabControlDataConsumer(string[] dataFormats) : base(dataFormats) { }

        public override DataConsumerActions DataConsumerActions =>
            DataConsumerActions.DragEnter | DataConsumerActions.DragOver | DataConsumerActions.Drop | DataConsumerActions.None;

        public override void DropTarget_DragEnter(object sender, System.Windows.DragEventArgs e) => DragOverOrDrop(false, sender, e);

        public override void DropTarget_DragOver(object sender, System.Windows.DragEventArgs e) => DragOverOrDrop(false, sender, e);

        public override void DropTarget_Drop(object sender, System.Windows.DragEventArgs e) => DragOverOrDrop(true, sender, e);

        /// <summary>
        /// First determine whether the drag data is supported.
        /// Next check if it's a move within the same TabControl,
        /// and rearrange the TabItems.
        /// Finally handle the actual drop when <code>bDrop</code> is true.
        /// </summary>
        /// <param name="bDrop">True to perform an actual drop, otherwise just return e.Effects</param>
        /// <param name="sender">DragDrop event <code>sender</code></param>
        /// <param name="e">DragDrop event arguments</param>
        private void DragOverOrDrop(bool bDrop, object sender, System.Windows.DragEventArgs e)
        {
            TabControlDataProvider<TContainer, TObject>? dataProvider = GetData(e) as TabControlDataProvider<TContainer, TObject>;
            if (dataProvider != null)
            {
                TContainer? dragSourceContainer = dataProvider.SourceContainer as TContainer;
                TObject? dragSourceObject = dataProvider.SourceObject as TObject;
                Debug.Assert(dragSourceObject != null);
                Debug.Assert(dragSourceContainer != null);

                TContainer? dropContainer = Interop.GetParent<TContainer>(e.Source as DependencyObject);
                TObject? dropTarget = e.Source as TObject;

                if (dropContainer != null && dragSourceContainer == dropContainer && dropTarget != null)
                {
                    int srcIndex = dragSourceContainer.Items.IndexOf(dragSourceObject);
                    int dstIndex = dropContainer.Items.IndexOf(dropTarget);
                    if (srcIndex != dstIndex)
                    {
                        // Only move when there's no chance of oscillation
                        bool doMove = true;
                        if (dragSourceObject!.ActualWidth < (dropTarget.ActualWidth))
                        {
                            Point point = e.GetPosition(dropTarget);
                            if (srcIndex < dstIndex)
                            {
                                doMove = point.X > dropTarget.ActualWidth - dragSourceObject.ActualWidth;
                            }
                            else
                            {
                                doMove = point.X < dragSourceObject.ActualWidth;
                            }
                        }
                        if (doMove)
                        {
                            dataProvider.Unparent();
                            dropContainer.Items.Insert(dstIndex, dragSourceObject);
                            dragSourceObject.IsSelected = true;
                        }
                    }
                    e.Effects = DragDropEffects.Link;
                    e.Handled = true;
                }
                else if (dropContainer != null)
                {
                    // 拖动到指定位置
                    if (bDrop)
                    {
                        dataProvider.Unparent();
                        dropContainer.Items.Insert(dropContainer.Items.IndexOf(dropTarget), dragSourceObject);
                        dragSourceObject!.IsSelected = true;
                    }
                    e.Effects = DragDropEffects.Move;
                    e.Handled = true;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                    e.Handled = true;
                }
            }
        }
    }
}
