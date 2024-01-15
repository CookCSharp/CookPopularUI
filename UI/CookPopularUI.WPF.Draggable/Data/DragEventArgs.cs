/*
 *Description: DragEventArgs
 *Author: Chance.zheng
 *Creat Time: 2024/1/5 17:12:05
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CookPopularUI.WPF.Draggable
{
    /// <summary>
    /// Delegate for creating drag events
    /// </summary>
    /// <param name="sender">The dragging sender.</param>
    /// <param name="args">Drag event args.</param>
    public delegate void DragEventHander(object sender, DragEventArgs args);

    public class DragEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the horizontal change of the drag
        /// </summary>
        public double HorizontalChange { get; set; }

        /// <summary>
        /// Gets or sets the vertical change of the drag
        /// </summary>
        public double VerticalChange { get; set; }

        /// <summary>
        /// Gets or sets the mouse event args
        /// </summary>
        public MouseEventArgs MouseEventArgs { get; set; }

        public DragEventArgs(double horizontalChange, double verticalChange, MouseEventArgs mouseEventArgs)
        {
            this.HorizontalChange = horizontalChange;
            this.VerticalChange = verticalChange;
            this.MouseEventArgs = mouseEventArgs;
        }
    }
}
