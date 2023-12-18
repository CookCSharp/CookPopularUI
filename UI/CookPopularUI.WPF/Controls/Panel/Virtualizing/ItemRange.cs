/*
 *Description: ItemRange
 *Author: Chance.zheng
 *Creat Time: 2023/12/7 11:40:57
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularUI.WPF.Controls
{
    public struct ItemRange
    {
        public int StartIndex { get; }
        public int EndIndex { get; }

        public ItemRange(int startIndex, int endIndex) : this()
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        public bool Contains(int itemIndex)
        {
            return itemIndex >= StartIndex && itemIndex <= EndIndex;
        }
    }
}
