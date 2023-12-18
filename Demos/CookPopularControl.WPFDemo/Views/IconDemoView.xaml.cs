using CookPopularUI.WPF.Controls;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// IconDemoView.xaml 的交互逻辑
    /// </summary>
    public partial class IconDemoView : UserControl
    {
        private static readonly ResourceDictionary GeometryFigureDictionary = new ResourceDictionary { Source = new Uri(@"pack://application:,,,/CookPopularUI.WPF;component/Resources/Dictionaries/GeometryFigure.xaml", UriKind.Absolute) };

        public IconDemoView()
        {
            InitializeComponent();

            //this.Loaded += IconDemoView_Loaded;
        }

        private void IconDemoView_Loaded(object sender, RoutedEventArgs e)
        {
            var list = new List<string>();
            foreach (var key in GeometryFigureDictionary.Keys)
            {
                if (key != null)
                    list.Add(key.ToString()!.Replace("Geometry", ""));
            }
            list.Sort();

            var desktopDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
            using var fs = new FileStream(System.IO.Path.Combine(desktopDirectory, "1.txt"), FileMode.OpenOrCreate);
            StreamWriter streamWriter = new StreamWriter(fs);
            streamWriter.Write(string.Join(",\n", list));
            streamWriter.Flush();
            streamWriter.Close();
        }
    }
}
