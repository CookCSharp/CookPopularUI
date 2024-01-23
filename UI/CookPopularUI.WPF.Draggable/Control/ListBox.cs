/*
 *Description: ListBox
 *Author: Chance.zheng
 *Creat Time: 2024/1/16 14:57:13
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
using System.Windows.Controls;
using System.Windows;

namespace CookPopularUI.WPF.Draggable
{
    public class ListBoxDataProvider<TContainer, TObject> : DataProviderBase<TContainer, TObject>, IDataProvider
        where TContainer : ItemsControl
        where TObject : FrameworkElement
    {
        public ListBoxDataProvider(string dataFormatString) : base(dataFormatString) { }

        public override DragDropEffects AllowedEffects => DragDropEffects.Move | DragDropEffects.Link | DragDropEffects.None;

        public override DataProviderActions DataProviderActions => DataProviderActions.QueryContinueDrag | DataProviderActions.GiveFeedback | DataProviderActions.None;

        public override void DragSource_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (e.Effects == DragDropEffects.Move)
            {
                e.UseDefaultCursors = true;
                e.Handled = true;
            }
            else if (e.Effects == DragDropEffects.Link)
            {
                e.UseDefaultCursors = true;
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


    public class ListBoxDataConsumer<TContainer, TObject> : DataConsumerBase, IDataConsumer
        where TContainer : ItemsControl
        where TObject : ListBoxItem
    {
        public ListBoxDataConsumer(string[] dataFormats) : base(dataFormats) { }

        public override DataConsumerActions DataConsumerActions => DataConsumerActions.DragEnter | DataConsumerActions.DragOver | DataConsumerActions.Drop | DataConsumerActions.None;

        public override void DropTarget_DragEnter(object sender, System.Windows.DragEventArgs e) => DragOverOrDrop(false, sender, e);

        public override void DropTarget_DragOver(object sender, System.Windows.DragEventArgs e) => DragOverOrDrop(false, sender, e);

        public override void DropTarget_Drop(object sender, System.Windows.DragEventArgs e) => DragOverOrDrop(true, sender, e);

        private void DragOverOrDrop(bool bDrop, object sender, System.Windows.DragEventArgs e)
        {
            ListBoxDataProvider<TContainer, TObject>? dataProvider = this.GetData(e) as ListBoxDataProvider<TContainer, TObject>;
            if (dataProvider != null)
            {
                TContainer? dragSourceContainer = dataProvider.SourceContainer as TContainer;
                TObject? dragSource = dataProvider.SourceObject as TObject;
                Debug.Assert(dragSource != null);
                Debug.Assert(dragSourceContainer != null);

                TContainer? dropTargetContainer = Interop.GetParent<TContainer>(e.Source as DependencyObject);
                TObject? dropTarget = e.Source as TObject;

                if (dragSource != null && dropTargetContainer != null)
                {
                    if (bDrop)
                    {
                        dataProvider.Unparent();
                        if (dropTarget == null)
                            dropTargetContainer.Items.Add(dragSource);
                        else
                            dropTargetContainer.Items.Insert(dropTargetContainer.Items.IndexOf(dropTarget), dragSource);

                        dragSource.IsSelected = true;
                        dragSource.BringIntoView();
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
