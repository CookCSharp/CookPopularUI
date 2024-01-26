/*
 *Description: DataConsumerBase
 *Author: Chance.zheng
 *Creat Time: 2024/1/8 15:10:26
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CookPopularUI.WPF.Draggable
{
    public class FileDropConsumer : DataConsumerBase
    {
        public FileDropConsumer(string[] dataFormats) : base(dataFormats) { }

        public override DataConsumerActions DataConsumerActions => DataConsumerActions.DragOver | DataConsumerActions.Drop | DataConsumerActions.None;

        public override void DragOverOrDrop(bool bDrop, object sender, System.Windows.DragEventArgs e)
        {
            string[]? files = GetData(e) as string[];
            if (files != null)
            {
                e.Effects = DragDropEffects.None;
                ItemsControl? dstItemsControl = sender as ItemsControl;  // 'sender' is used when dropped in an empty area
                if (dstItemsControl != null)
                {
                    foreach (string file in files)
                    {
                        if (sender is TabControl)
                        {
                            if (bDrop)
                            {
                                TabItem item = new TabItem();
                                item.Header = System.IO.Path.GetFileName(file);
                                item.ToolTip = file;
                                dstItemsControl.Items.Insert(0, item);
                                item.IsSelected = true;
                            }
                            e.Effects = DragDropEffects.Copy;
                        }
                        else if (sender is ListBox)
                        {
                            if (bDrop)
                            {
                                ListBoxItem? dstItem = Interop.GetParent<ListBoxItem>((DependencyObject)e.Source);
                                ListBoxItem item = new ListBoxItem();
                                item.Content = System.IO.Path.GetFileName(file);
                                item.ToolTip = file;
                                if (dstItem == null)
                                    dstItemsControl.Items.Add(item);    // if dropped on an empty area
                                else
                                    dstItemsControl.Items.Insert(dstItemsControl.Items.IndexOf(dstItem), item);

                                item.IsSelected = true;
                                item.BringIntoView();
                            }
                            e.Effects = DragDropEffects.Copy;
                        }
                        else if (sender is TreeView)
                        {
                            if (bDrop)
                            {
                                if (e.Source is ItemsControl)
                                    dstItemsControl = e.Source as ItemsControl; // Dropped on a TreeViewItem
                                TreeViewItem item = new TreeViewItem();
                                item.Header = System.IO.Path.GetFileName(file);
                                item.ToolTip = file;
                                dstItemsControl!.Items.Add(item);
                                item.IsSelected = true;
                                item.BringIntoView();
                            }
                            e.Effects = DragDropEffects.Copy;
                        }
                        else
                        {
                            throw new NotSupportedException("The item type is not implemented");
                        }
                        // No need to loop through multiple
                        // files if we're not dropping them
                        if (!bDrop) break;
                    }
                }
                e.Handled = true;
            }
        }
    }
}
