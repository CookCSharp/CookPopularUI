/*
 *Description: IDataConsumer
 *Author: Chance.zheng
 *Creat Time: 2024/1/8 15:06:37
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularUI.WPF.Draggable
{
    /// <summary>
    /// 处理拖放的数据
    /// </summary>
    public interface IDataConsumer
    {
        DataConsumerActions DataConsumerActions { get; }

        /// <summary>
        /// Occurs when mouse enters the area occupied
        /// by the dropTarget (specified in the constructor).
        /// Provide your own method if you wish; making sure
        /// to define DragEnter in DataConsumerActions.
        /// 
        /// See DropTarget_DragEnter in DropManager for additional comments.
        /// </summary>
        void DropTarget_DragEnter(object sender, System.Windows.DragEventArgs e);

        /// <summary>
        /// Occurs when mouse is over the area occupied
        /// by the dropTarget (specified in the constructor).
        /// You must likely will provide your own method; make sure
        /// to define DragOver in DataConsumerActions.
        /// </summary>
        void DropTarget_DragOver(object sender, System.Windows.DragEventArgs e);

        /// <summary>
        /// Occurs when the left mouse button is released in the area
        /// occupied by the dropTarget (specified in the constructor).
        /// You must likely will provide your own method; make sure
        /// to define Drop in DataConsumerActions.
        /// 
        /// See DropTarget_DragEnter in DropManager for additional comments.
        /// </summary>
        void DropTarget_Drop(object sender, System.Windows.DragEventArgs e);

        /// <summary>
        /// Occurs when mouse leaves the area occupied
        /// by the dropTarget (specified in the constructor).
        /// Provide your own method if you wish; making sure
        /// to define DragEnter in DataConsumerActions.
        /// 
        /// See DropTarget_DragLeave in DropManager for additional comments.
        /// </summary>
        void DropTarget_DragLeave(object sender, System.Windows.DragEventArgs e);
    }
}
