/*
 *Description: DragDropData
 *Author: Chance.zheng
 *Creat Time: 2024/1/22 20:09:08
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using CookPopularToolkit;
using CookPopularToolkit.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CookPopularUI.WPF.Draggable
{
    public class DragDropData
    {
        private static readonly Dictionary<FrameworkElement, DragManager> DragManagerCache = new Dictionary<FrameworkElement, DragManager>();
        private static readonly Dictionary<FrameworkElement, DropManager> DropManagerCache = new Dictionary<FrameworkElement, DropManager>();


        public static bool GetIsDragSource(DependencyObject obj) => (bool)obj.GetValue(IsDragSourceProperty);
        public static void SetIsDragSource(DependencyObject obj, bool value) => obj.SetValue(IsDragSourceProperty, value);
        public static readonly DependencyProperty IsDragSourceProperty =
            DependencyProperty.RegisterAttached("IsDragSource", typeof(bool), typeof(DragDropData), new PropertyMetadata(false, OnDragDropDataChanged));


        public static bool GetIsDropTarget(DependencyObject obj) => (bool)obj.GetValue(IsDropTargetProperty);
        public static void SetIsDropTarget(DependencyObject obj, bool value) => obj.SetValue(IsDropTargetProperty, value);
        public static readonly DependencyProperty IsDropTargetProperty =
            DependencyProperty.RegisterAttached("IsDropTarget", typeof(bool), typeof(DragDropData), new PropertyMetadata(false, OnDragDropDataChanged));


        public static IDataProvider GetDataProvider(DependencyObject obj) => (IDataProvider)obj.GetValue(DataProviderProperty);
        public static void SetDataProvider(DependencyObject obj, IDataProvider value) => obj.SetValue(DataProviderProperty, value);
        public static readonly DependencyProperty DataProviderProperty =
            DependencyProperty.RegisterAttached("DataProvider", typeof(IDataProvider), typeof(DragDropData), new PropertyMetadata(default, OnDragDropDataChanged));


        public static IDataConsumer GetDataConsumer(DependencyObject obj) => (IDataConsumer)obj.GetValue(DataConsumerProperty);
        public static void SetDataConsumer(DependencyObject obj, IDataConsumer value) => obj.SetValue(DataConsumerProperty, value);
        public static readonly DependencyProperty DataConsumerProperty =
            DependencyProperty.RegisterAttached("DataConsumer", typeof(IDataConsumer), typeof(DragDropData), new PropertyMetadata(default, OnDragDropDataChanged));


        private static void OnDragDropDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => Init(d);

        private static void Init(DependencyObject dependencyObject)
        {
            if (dependencyObject is not ItemsControl) return;

            var isDragSource = GetIsDragSource(dependencyObject);
            var isDropTarget = GetIsDropTarget(dependencyObject);
            var dataProvider = GetDataProvider(dependencyObject);
            var dataConsumer = GetDataConsumer(dependencyObject);

            FrameworkElement? element = (FrameworkElement)dependencyObject;

            if (element != null && (!isDragSource || dataProvider == null))
            {
                if (DragManagerCache.TryGetValue(element, out DragManager dragManager))
                {
                    dragManager.Unregister();
                    DragManagerCache.Remove(element);
                }
            }

            if (element != null && (!isDropTarget || dataConsumer == null))
            {
                if (DropManagerCache.TryGetValue(element, out DropManager dropManager))
                {
                    dropManager.Unregister();
                    DropManagerCache.Remove(element);
                }
            }

            if (dataProvider != null && element != null && isDragSource)
            {
                if (!DragManagerCache.ContainsKey(element))
                {
                    var dragManager = new DragManager(element, dataProvider);
                    DragManagerCache.Add(element, dragManager);
                }
            }

            if (dataConsumer != null && element != null && isDropTarget)
            {
                var dropManager = new DropManager(element, new IDataConsumer[] { dataConsumer });
                if (!DropManagerCache.ContainsKey(element))
                {
                    DropManagerCache.Add(element, dropManager);
                }
                else
                {
                    DropManagerCache[element] = dropManager;
                }
            }
        }
    }
}
