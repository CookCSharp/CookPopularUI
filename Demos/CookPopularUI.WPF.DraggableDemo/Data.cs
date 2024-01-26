using CookPopularUI.WPF.Draggable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CookPopularUI.WPF.DraggableDemo
{
    public class Data
    {
        public static readonly FileDropConsumer FileDropDataConsumer = new FileDropConsumer(new string[] { "FileDrop", "FileNameW" });

        public static readonly TabControlDataProvider<TabControl, TabItem> TabControlDataProvider = new TabControlDataProvider<TabControl, TabItem>("TabControlObject");
        public static readonly TabControlDataConsumer<TabControl, TabItem> TabControlDataConsumer = new TabControlDataConsumer<TabControl, TabItem>(new string[] { "TabControlObject" });

        public static readonly ListBoxDataProvider<ListBox, ListBoxItem> ListBoxDataProvider = new ListBoxDataProvider<ListBox, ListBoxItem>("ListBoxObject");
        public static readonly ListBoxDataConsumer<ListBox, ListBoxItem> ListBoxDataConsumer = new ListBoxDataConsumer<ListBox, ListBoxItem>(new string[] { "ListBoxObject" });

        public static readonly TreeViewDataProvider<ItemsControl, TreeViewItem> TreeViewDataProvider = new TreeViewDataProvider<ItemsControl, TreeViewItem>("TreeViewObject");
        public static readonly TreeViewDataConsumer<ItemsControl, TreeViewItem> TreeViewDataConsumer = new TreeViewDataConsumer<ItemsControl, TreeViewItem>(new string[] { "TreeViewObject" });

        public static readonly ToolBarDataProvider<ToolBar, Button> ToolBarButtonDataProvider = new ToolBarDataProvider<ToolBar, Button>("ToolbarObject");
        public static readonly ToolBarDataConsumer<ToolBar, Button> ToolBarButtonDataConsumer = new ToolBarDataConsumer<ToolBar, Button>(new string[] { "ToolbarObject" });

        public static readonly TrashConsumer TrashConsumer = new TrashConsumer(new string[]
        {
            "TabControlObject",
            "ListBoxObject",
            "TreeViewObject",
            "ToolbarObject",
            "CanvasTextBlockObject",
            "CanvasRectangleObject",
            "CanvasButtonObject",
        });
    }
}
