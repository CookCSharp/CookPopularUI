using CookPopularUI.WPF.Controls;
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

namespace CookPopularUI.WPFDemo.Views
{
    /// <summary>
    /// OtherControlDemoView.xaml 的交互逻辑
    /// </summary>
    public partial class OtherControlDemoView : UserControl
    {
        public OtherControlDemoView()
        {
            InitializeComponent();

            dragDropArea.DragDroped += DragDropArea_DragDroped;
        }

        private void DragDropArea_DragDroped(object sender, string[] files)
        {
            tbDropFiles.Text = string.Join(Environment.NewLine, files);
        }
    }
}
