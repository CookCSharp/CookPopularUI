using CookPopularUI.WPF.Draggable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CookPopularUI.WPF.DraggableDemo.Views
{
    /// <summary>
    /// ListBoxDemoView.xaml 的交互逻辑
    /// </summary>
    public partial class ListBoxDemoView : UserControl
    {
        private static readonly FileDropConsumer FileDropDataConsumer = new FileDropConsumer(["FileDrop", "FileNameW",]);

        public ListBoxDemoView()
        {
            InitializeComponent();

            // Data Provider
            ListBoxDataProvider<ListBox, ListBoxItem> listBoxDataProvider =
                new ListBoxDataProvider<ListBox, ListBoxItem>("ListBoxItemObject");

            // Data Consumer
            ListBoxDataConsumer<ListBox, ListBoxItem> listBoxDataConsumer =
                new ListBoxDataConsumer<ListBox, ListBoxItem>(new string[] { "ListBoxItemObject" });

            // Data Consumer of TreeViewItems
            //TreeViewItemToListBoxItem<ItemsControl, TreeViewItem> treeViewItemToListBoxItem =
            //    new TreeViewItemToListBoxItem<ItemsControl, TreeViewItem>(new string[] { "TreeViewItemObject" });

            // Drag Managers
            DragManager dragHelperListBox0 = new DragManager(listBox1, listBoxDataProvider);
            DragManager dragHelperListBox1 = new DragManager(listBox2, listBoxDataProvider);

            // Drop Managers
            DropManager dropHelperListBox0 = new DropManager(listBox1, new IDataConsumer[]
            {
                listBoxDataConsumer,
                //treeViewItemToListBoxItem,
                FileDropDataConsumer,
            });
            DropManager dropHelperListBox1 = new DropManager(listBox2, new IDataConsumer[]
            {
                listBoxDataConsumer,
                //treeViewItemToListBoxItem,
                FileDropDataConsumer,
            });
        }
    }
}
