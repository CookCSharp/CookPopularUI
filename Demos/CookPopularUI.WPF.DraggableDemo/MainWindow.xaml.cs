using CookPopularUI.WPF.Draggable;
using CookPopularUI.WPF.DraggableDemo.Views;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CookPopularUI.WPF.DraggableDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> DemoViewNames { get; set; }
        public FrameworkElement? View { get; set; }


        //private ObservableCollection<DragDropPanel> panels1 = new ObservableCollection<DragDropPanel>();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
            //DemoViewNames = new ObservableCollection<string>() { "TreeView" };
            DemoViewNames = new ObservableCollection<string>() { "ListBox", "TabControl", "ToolBar", "TreeView" };


            //dragDropPanelHost1.ItemsSource = panels1;
            //for (int i = 0; i < 8; i++)
            //{
            //    AddPanel1();
            //}
            //addPanelButton1.Click += new RoutedEventHandler(AddPanelButton_Click1);
            //removePanelButton1.Click += new RoutedEventHandler(RemovePanelButton_Click1);
        }

        //private void AddPanelButton_Click1(object sender, RoutedEventArgs e)
        //{
        //    AddPanel1();
        //}

        //private void AddPanel1()
        //{
        //    panels1.Add(new DragDropPanel()
        //    {
        //        Margin = new Thickness(15),
        //        //Header = string.Format("{0} {1}", "PanelX", dragDropPanelHost1.Items.Count + 1),
        //        Content = new TextBlock()
        //        {
        //            Text = "C O N T E N T",
        //            FontFamily = new FontFamily("Verdana"),
        //            FontSize = 14,
        //            VerticalAlignment = VerticalAlignment.Center,
        //            HorizontalAlignment = HorizontalAlignment.Center,
        //            Foreground = Brushes.Black
        //        },
        //        Background = Brushes.Yellow,
        //    });
        //}

        //private void RemovePanelButton_Click1(object sender, RoutedEventArgs e)
        //{
        //    if (panels1.Count > 0)
        //    {
        //        panels1.RemoveAt(panels1.Count - 1);
        //    }
        //}


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = (ListBox)sender;
            View = Activator.CreateInstance(Type.GetType($"CookPopularUI.WPF.DraggableDemo.Views.{listBox.SelectedItem}DemoView")!) as FrameworkElement;
        }
    }
}