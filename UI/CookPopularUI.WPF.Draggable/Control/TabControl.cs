/*
 *Description: TabControl
 *Author: Chance.zheng
 *Creat Time: 2024/1/8 13:57:18
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using CookPopularToolkit.Windows;
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

        private void DragOverOrDrop(bool bDrop, object sender, System.Windows.DragEventArgs e)
        {
            TabControlDataProvider<TContainer, TObject>? dataProvider = GetData(e) as TabControlDataProvider<TContainer, TObject>;
            if (dataProvider != null)
            {
                TContainer? dragSourceContainer = dataProvider.SourceContainer as TContainer;
                TObject? dragSource = dataProvider.SourceObject as TObject;
                Debug.Assert(dragSource != null);
                Debug.Assert(dragSourceContainer != null);

                TContainer? dropTargetContainer = Interop.GetParent<TContainer>(e.Source as DependencyObject);
                TObject? dropTarget = e.Source as TObject;

                if (dragSource != null && dropTarget != null && dragSourceContainer != null && dropTargetContainer != null && dragSourceContainer == dropTargetContainer)
                {
                    int srcIndex = dragSourceContainer.Items.IndexOf(dragSource);
                    int dstIndex = dropTargetContainer.Items.IndexOf(dropTarget);

                    if (srcIndex != dstIndex)
                    {
                        bool doMove = true;
                        if (dragSource.ActualWidth < (dropTarget.ActualWidth))
                        {
                            Point point = e.GetPosition(dropTarget);
                            if (srcIndex < dstIndex)
                            {
                                doMove = point.X > dropTarget.ActualWidth - dragSource.ActualWidth;
                            }
                            else
                            {
                                doMove = point.X < dragSource.ActualWidth;
                            }
                        }
                        if (doMove)
                        {
                            dataProvider.Unparent();
                            dropTargetContainer.Items.Insert(dstIndex, dragSource);
                            dragSource.IsSelected = true;
                        }
                    }

                    e.Effects = DragDropEffects.Link;
                    e.Handled = true;
                }
                else if (dragSource != null && dropTarget != null && dragSourceContainer != null && dropTargetContainer != null && dragSourceContainer != dropTargetContainer)
                {
                    if (bDrop)
                    {
                        dataProvider.Unparent();

                        int insertIndex = dropTargetContainer.Items.IndexOf(dropTarget);
                        var position = e.GetPosition(dropTargetContainer);
                        var item = dropTargetContainer.GetItemContainerAt(position);
                        var currentPosion = e.GetPosition(item);
                        GetIndex(dropTargetContainer, item, currentPosion, ref insertIndex);

                        dropTargetContainer.Items.Insert(insertIndex, dragSource);

                        dragSource.IsSelected = true;
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

        private int GetIndex(TContainer dropTargetContainer, UIElement? element, Point point, ref int index)
        {
            if (dropTargetContainer == null || element == null)
                return index;

            var currentXPos = point.X;
            var currentYPos = point.Y;
            var targetWidth = element.RenderSize.Width;
            var targetHeight = element.RenderSize.Height;

            if (dropTargetContainer is TabControl tab)
            {
                if (tab.TabStripPlacement == Dock.Top || tab.TabStripPlacement == Dock.Bottom)
                {
                    if (currentXPos > targetWidth / 2)
                    {
                        index += 1;
                    }
                }
                else
                {
                    if (currentYPos > targetHeight / 2)
                    {
                        index += 1;
                    }
                }
            }

            return index;
        }
    }
}
