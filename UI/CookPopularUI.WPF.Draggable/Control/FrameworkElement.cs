/*
 *Description: FrameworkElement
 *Author: Chance.zheng
 *Creat Time: 2024/1/26 15:55:12
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
using System.Windows;
using System.Windows.Controls;

namespace CookPopularUI.WPF.Draggable
{
    public class FrameworkElementDataProvider<TContainer, TObject> : DataProviderBase<TContainer, TObject> where TContainer : FrameworkElement where TObject : FrameworkElement
    {
        public FrameworkElementDataProvider(string dataFormatString) : base(dataFormatString) { }
    }

    public class FrameworkElementDataConsumer<TContainer, TObject> : DataConsumerBase where TContainer : FrameworkElement where TObject : FrameworkElement
    {
        public FrameworkElementDataConsumer(string[] dataFormats) : base(dataFormats) { }
    }
}
