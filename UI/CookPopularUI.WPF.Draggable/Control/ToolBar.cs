/*
 *Description: ToolBar
 *Author: Chance.zheng
 *Creat Time: 2024/1/25 20:02:23
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
    public class ToolBarDataProvider<TContainer, TObject> : DataProviderBase<TContainer, TObject> where TContainer : ItemsControl where TObject : UIElement
    {
        public override DataProviderActions DataProviderActions => DataProviderActions.GiveFeedback | DataProviderActions.None;

        public ToolBarDataProvider(string dataFormatString) : base(dataFormatString) { }

        public override bool IsSupportedContainerAndObject(bool initFlag, object dragSourceContainer, object dragSourceObject, object dragOriginalSourceObject)
        {
            TObject? sourceObject = dragSourceObject as TObject;
            if (sourceObject == null)
                sourceObject = Interop.GetParent<TObject>(dragSourceObject as DependencyObject, false);

            if (initFlag)
            {
                Init();
                SourceContainer = dragSourceContainer;
                SourceObject = sourceObject!;
                OriginalSourceObject = dragOriginalSourceObject;
            }

            return (dragSourceContainer is TContainer) && (sourceObject != null);
        }
    }

    public class ToolBarDataConsumer<TContainer, TObject> : DataConsumerBase where TContainer : ItemsControl where TObject : UIElement
    {
        public ToolBarDataConsumer(string[] dataFormats) : base(dataFormats) { }

        public override void DragOverOrDrop(bool bDrop, object sender, DragEventArgs e)
        {
            ToolBarDataProvider<TContainer, TObject>? dataProvider = GetData(e) as ToolBarDataProvider<TContainer, TObject>;
            if (dataProvider != null)
            {
                TContainer? dragSourceContainer = dataProvider.SourceContainer as TContainer;
                TObject? dragSourceObject = dataProvider.SourceObject as TObject;
                Debug.Assert(dragSourceObject != null);
                Debug.Assert(dragSourceContainer != null);

                TContainer? dropTargetContainer = sender as TContainer;
                TObject? dropTarget = e.Source as TObject;
                if (dropTarget == null)
                    dropTarget = Interop.GetParent<TObject>(e.Source as DependencyObject, false);

                if (dropTargetContainer != null)
                {
                    if (bDrop)
                    {
                        dataProvider.Unparent();
                        if (dropTarget == null)
                            dropTargetContainer.Items.Add(dragSourceObject);
                        else
                            dropTargetContainer.Items.Insert(dropTargetContainer.Items.IndexOf(dropTarget), dragSourceObject);
                    }
                    e.Effects = (dropTarget == null) ? DragDropEffects.Move : DragDropEffects.Link;
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
