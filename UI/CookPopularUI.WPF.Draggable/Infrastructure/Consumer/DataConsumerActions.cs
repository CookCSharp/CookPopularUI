/*
 *Description: DataConsumerActions
 *Author: Chance.zheng
 *Creat Time: 2024/1/8 15:06:59
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
    public enum DataConsumerActions
    {
        None = 0x00,
        DragEnter = 0x01,
        DragOver = 0x02,
        Drop = 0x04,
        DragLeave = 0x08,
        AllowDropMask = DragEnter | DragOver | Drop | DragLeave,
    }
}
