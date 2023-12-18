/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：SingleClickDataGridCellBase
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-28 14:59:19
 */


using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 单击进行编辑<see cref="DataGridCell"/>的基类
    /// </summary>
    public abstract class DataGridCellSingleClickBase : DataGridColumn
    {
        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            return BaseGenerateElement(cell, dataItem);
        }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            return BaseGenerateEditingElement(cell, dataItem);
        }

        protected abstract FrameworkElement BaseGenerateElement(DataGridCell cell, object dataItem);

        protected abstract FrameworkElement BaseGenerateEditingElement(DataGridCell cell, object dataItem);

        /// <summary>
        /// 单击直接进行编辑，不需要先获取DataGridCell的焦点
        /// </summary>
        protected override object PrepareCellForEdit(FrameworkElement element, RoutedEventArgs e)
        {
            object x = base.PrepareCellForEdit(element, e);
            if ((e is MouseButtonEventArgs) && ((MouseButtonEventArgs)e).ChangedButton == MouseButton.Left)
            {
                var dataGridCell = new DataGridCell() { Content = element };
                ((DataGrid)e.Source).CurrentCell = new DataGridCellInfo(dataGridCell);
                ((DataGrid)e.Source).BeginEdit();

                if (element is ComboBox cb)
                {
                    cb.IsDropDownOpen = true;
                    e.Handled = true;
                }
                else if (element is ToggleButton tb)
                {
                    var newMouseEvent = new MouseButtonEventArgs(((MouseButtonEventArgs)e).MouseDevice, 0, MouseButton.Left)
                    {
                        RoutedEvent = Mouse.MouseDownEvent,
                        Source = e.Source
                    };

                    tb.RaiseEvent(newMouseEvent);
                }
            }
            return x;
        }
    }
}
