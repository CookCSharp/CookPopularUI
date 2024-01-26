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
    public class ListBoxDataProvider<TContainer, TObject> : DataProviderBase<TContainer, TObject> where TContainer : ItemsControl where TObject : FrameworkElement
    {
        public ListBoxDataProvider(string dataFormatString) : base(dataFormatString) { }
    }

    public class ListBoxDataConsumer<TContainer, TObject> : DataConsumerBase where TContainer : ItemsControl where TObject : ListBoxItem
    {
        public ListBoxDataConsumer(string[] dataFormats) : base(dataFormats) { }

        public override void DragOverOrDrop(bool bDrop, object sender, System.Windows.DragEventArgs e)
        {
            ListBoxDataProvider<TContainer, TObject>? dataProvider = GetData(e) as ListBoxDataProvider<TContainer, TObject>;
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
                        int index = 0;
                        if (dragSourceContainer == dropTargetContainer)
                        {
                            index = dropTargetContainer.Items.IndexOf(dropTarget);
                        }

                        dataProvider.Unparent();

                        if(dragSourceContainer != dropTargetContainer)
                        {
                            index = dropTargetContainer.Items.IndexOf(dropTarget);
                        }

                        if (dropTarget == null)
                            dropTargetContainer.Items.Add(dragSource);
                        else
                            dropTargetContainer.Items.Insert(index, dragSource);

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
