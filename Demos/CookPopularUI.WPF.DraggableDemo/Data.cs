using CookPopularUI.WPF.Draggable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CookPopularUI.WPF.DraggableDemo
{
    public class Data
    {
        public static readonly TabControlDataProvider<TabControl, TabItem> TabControlDataProvider = new TabControlDataProvider<TabControl, TabItem>("TabItemObject");
        public static readonly TabControlDataConsumer<TabControl, TabItem> TabControlDataConsumer = new TabControlDataConsumer<TabControl, TabItem>(new string[] { "TabItemObject" });
    }
}
