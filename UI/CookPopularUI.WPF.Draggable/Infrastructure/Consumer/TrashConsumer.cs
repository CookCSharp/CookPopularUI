/*
 *Description: TrashConsumer
 *Author: Chance.zheng
 *Creat Time: 2024/1/26 9:39:00
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CookPopularUI.WPF.Draggable
{
    public class TrashConsumer : DataConsumerBase
    {
        public TrashConsumer(string[] dataFormats) : base(dataFormats) { }

        public override DataConsumerActions DataConsumerActions => DataConsumerActions.DragOver | DataConsumerActions.Drop | DataConsumerActions.None;

        public override void DragOverOrDrop(bool bDrop, object sender, DragEventArgs e)
        {
            IDataProvider? dataProvider = GetData(e) as IDataProvider;
            if (dataProvider != null)
            {
                if (bDrop)
                {
                    dataProvider.Unparent();
                }

                e.Effects = DragDropEffects.Move;
                e.Handled = true;
            }
        }
    }
}
