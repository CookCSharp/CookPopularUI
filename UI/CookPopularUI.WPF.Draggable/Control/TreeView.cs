/*
 *Description: TreeView
 *Author: Chance.zheng
 *Creat Time: 2024/1/23 19:24:27
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

namespace CookPopularUI.WPF.Draggable
{
    public class TreeViewDataProvider<TContainer, TObject> : DataProviderBase<TContainer, TObject> where TContainer : ItemsControl where TObject : ItemsControl
    {
        public TreeViewDataProvider(string dataFormatString) : base(dataFormatString) { }

        public override void Unparent()
        {
            TObject? item = SourceObject as TObject;
            TContainer? container = Interop.GetParent<TContainer>(item, false);

            Debug.Assert(item != null, "Unparent expects a non-null item");
            Debug.Assert(container != null, "Unparent expects a non-null container");

            if ((container != null) && (item != null))
                container.Items.Remove(item);
        }
    }

    public class TreeViewDataConsumer<TContainer, TObject> : DataConsumerBase where TContainer : ItemsControl where TObject : ItemsControl
    {
        public TreeViewDataConsumer(string[] dataFormats) : base(dataFormats) { }

        public override void DragOverOrDrop(bool bDrop, object sender, DragEventArgs e)
        {
            TreeViewDataProvider<TContainer, TObject>? dataProvider = GetData(e) as TreeViewDataProvider<TContainer, TObject>;
            if (dataProvider != null)
            {
                TContainer? dragSourceContainer = dataProvider.SourceContainer as TContainer;
                TreeViewItem? dragSourceObject = dataProvider.SourceObject as TreeViewItem;
                Debug.Assert(dragSourceContainer != null);
                Debug.Assert(dragSourceObject != null);

                TContainer? dropTargetContainer = Interop.GetParent<TContainer>(sender as DependencyObject);
                Debug.Assert(dropTargetContainer != null);
                TObject? dropTarget = e.Source as TObject;

                if (dropTarget == null)
                {
                    if (bDrop && dropTargetContainer != null)
                    {
                        dataProvider.Unparent();
                        dropTargetContainer.Items.Add(dragSourceObject);
                    }
                    e.Effects = DragDropEffects.Move;
                    e.Handled = true;
                }
                else
                {
                    bool IsAncestor = dragSourceObject!.IsAncestorOf(dropTarget);
                    if ((dataProvider.KeyStates & DragDropKeyStates.ShiftKey) != 0)
                    {
                        ItemsControl? shiftDropTarget = Interop.GetParent<ItemsControl>(dropTarget, false);
                        Debug.Assert(shiftDropTarget != null);
                        if (!IsAncestor)
                        {
                            if (bDrop)
                            {
                                dataProvider.Unparent();
                                Debug.Assert(shiftDropTarget != null);
                                shiftDropTarget!.Items.Insert(shiftDropTarget.Items.IndexOf(dropTarget), dragSourceObject);
                            }
                            e.Effects = DragDropEffects.Link;
                            e.Handled = true;
                        }
                        else
                        {
                            e.Effects = DragDropEffects.None;
                            e.Handled = true;
                        }
                    }
                    else
                    {
                        if (!IsAncestor && (dragSourceObject != dropTarget))
                        {
                            if (bDrop)
                            {
                                dataProvider.Unparent();
                                dropTarget.Items.Add(dragSourceObject);
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
                if (bDrop && e.Handled && (e.Effects != DragDropEffects.None))
                {
                    dragSourceObject!.IsSelected = true;
                    dragSourceObject.BringIntoView();
                }
            }
        }
    }
}
