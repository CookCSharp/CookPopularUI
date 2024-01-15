/*
 *Description: DataProviderActions
 *Author: Chance.zheng
 *Creat Time: 2024/1/8 14:06:12
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
    [Flags]
    public enum DataProviderActions
    {
        None = 0x00,
        /// <summary>
        /// Call IDataProvider.DragSource_QueryContinueDrag
        /// </summary>
        QueryContinueDrag = 0x01,
        /// <summary>
        /// Call IDataProvider.DragSource_GiveFeedback
        /// </summary>
        GiveFeedback = 0x02,
        /// <summary>
        /// Call IDataProvider.DoDragDrop_Done
        /// </summary>
        DoDragDrop_Done = 0x04,
    }
}
