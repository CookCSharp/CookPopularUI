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
    /// TabControlDemoView.xaml 的交互逻辑
    /// </summary>
    public partial class TabControlDemoView : UserControl
    {
        private static readonly FileDropConsumer FileDropDataConsumer = new FileDropConsumer(["FileDrop", "FileNameW",]);


        public TabControlDemoView()
        {
            InitializeComponent();

            // Data Provider
            TabControlDataProvider<TabControl, TabItem> tabControlDataProvider = new TabControlDataProvider<TabControl, TabItem>("TabItemObject");
            // Data Consumer
            TabControlDataConsumer<TabControl, TabItem> tabControlDataConsumer = new TabControlDataConsumer<TabControl, TabItem>(["TabItemObject"]);

            // Drag Managers
            DragManager dragHelperTabControl1 = new DragManager(tab1, tabControlDataProvider);
            DragManager dragHelperTabControl2 = new DragManager(tab2, tabControlDataProvider);
            DragManager dragHelperTabControl3 = new DragManager(tab3, tabControlDataProvider);
            DragManager dragHelperTabControl4 = new DragManager(tab4, tabControlDataProvider);

            // Drop Managers
            DropManager dropHelperTabControl1 = new DropManager(tab1, [tabControlDataConsumer, FileDropDataConsumer]);
            DropManager dropHelperTabControl2 = new DropManager(tab2, [tabControlDataConsumer, FileDropDataConsumer]);
            DropManager dropHelperTabControl3 = new DropManager(tab3, [tabControlDataConsumer, FileDropDataConsumer]);
            DropManager dropHelperTabControl4 = new DropManager(tab4, [tabControlDataConsumer, FileDropDataConsumer]);
        }
    }
}
