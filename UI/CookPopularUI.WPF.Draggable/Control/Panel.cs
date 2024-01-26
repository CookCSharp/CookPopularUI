/*
 *Description: Panel
 *Author: Chance.zheng
 *Creat Time: 2024/1/26 11:29:30
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;
using System.Windows.Shapes;

namespace CookPopularUI.WPF.Draggable
{
    public class PanelDataProvider<TContainer, TObject> : DataProviderBase<TContainer, TObject> where TContainer : Panel where TObject : UIElement
    {
        public override bool AddAdorner => true;
        public override DataProviderActions DataProviderActions => DataProviderActions.GiveFeedback | DataProviderActions.None;


        public PanelDataProvider(string dataFormatString) : base(dataFormatString) { }

        public override bool IsSupportedContainerAndObject(bool initFlag, object dragSourceContainer, object dragSourceObject, object dragOriginalSourceObject)
        {
            TObject? sourceObject = dragSourceObject as TObject;
            if (sourceObject == null)
            {
                sourceObject = Interop.GetParent<TObject>(dragSourceObject as DependencyObject, false);
            }

            if (initFlag)
            {
                Init();
                SourceContainer = dragSourceContainer;
                SourceObject = sourceObject!;
                OriginalSourceObject = dragOriginalSourceObject;
            }

            return dragSourceContainer is TContainer && sourceObject != null;
        }

        public override void DragSource_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (e.Effects == DragDropEffects.Move)
            {
                e.UseDefaultCursors = true;
                e.Handled = true;
            }
        }

        public override void Unparent()
        {
            TContainer? panel = SourceContainer as TContainer;
            TObject? item = SourceObject as TObject;

            Debug.Assert(item != null, "Unparent expects a non-null item");
            Debug.Assert(panel != null, "Unparent expects a non-null panel");

            if (panel != null && item != null)
                panel.Children.Remove(item);
        }
    }

    public class PanelDataConsumer<TContainer, TObject> : DataConsumerBase where TContainer : Panel where TObject : UIElement
    {
        public PanelDataConsumer(string[] dataFormats) : base(dataFormats) { }

        public override void DragOverOrDrop(bool bDrop, object sender, DragEventArgs e)
        {
            PanelDataProvider<TContainer, TObject>? dataProvider = GetData(e) as PanelDataProvider<TContainer, TObject>;
            if (dataProvider != null)
            {
                TContainer? dragSourceContainer = dataProvider.SourceContainer as TContainer;
                TObject? dragSourceObject = dataProvider.SourceObject as TObject;
                Debug.Assert(dragSourceObject != null);

                TContainer? dropTargetContainer = sender as TContainer;
                TObject? dropTarget = e.Source as TObject;

                if (dropTargetContainer != null)
                {
                    if (bDrop)
                    {
                        // 移动整个面板时
                        if (dragSourceContainer == dragSourceObject)
                        {
                            return;
                        }
                        dataProvider.Unparent();

                        if (dragSourceObject != null && dropTargetContainer is Canvas)
                        {
                            dropTargetContainer.Children.Add(dragSourceObject);

                            Point dropPosition = e.GetPosition(dropTargetContainer);
                            Point objectOrigin = dataProvider.StartPosition;
                            Canvas.SetLeft(dragSourceObject, dropPosition.X - objectOrigin.X);
                            Canvas.SetTop(dragSourceObject, dropPosition.Y - objectOrigin.Y);
                        }
                        else if (dragSourceObject != null && dropTarget != null)
                        {
                            int insertIndex = dropTargetContainer.Children.IndexOf(dropTarget);
                            var currentPosion = e.GetPosition(dropTarget);
                            Interop.GetIndex(dropTargetContainer, dropTarget, currentPosion, ref insertIndex);
                            if (dropTargetContainer.Children.Contains(dropTarget))
                                dropTargetContainer.Children.Insert(insertIndex, dragSourceObject);
                            else
                                dropTargetContainer.Children.Add(dragSourceObject);
                        }
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
