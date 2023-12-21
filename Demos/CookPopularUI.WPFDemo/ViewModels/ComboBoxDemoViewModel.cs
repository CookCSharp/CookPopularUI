/*
 *Description: ComboBoxDemoViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/11/13 19:43:15
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularUI.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace CookPopularUI.WPFDemo.ViewModels
{
    public class ComboBoxDemoViewModel : ViewModelBase
    {
        public ObservableCollection<object> Lists { get; set; }

        public ObservableCollection<SelectorItem> IconLists { get; set; }

        public ObservableCollection<SelectorItem> ImageLists { get; set; }

        public ObservableCollection<SelectorItem> GifLists { get; set; }

        public ComboBoxDemoViewModel()
        {
            Lists = new ObservableCollection<object>() { "111", "222", "333" };
            IconLists = new ObservableCollection<SelectorItem>();
            ImageLists = new ObservableCollection<SelectorItem>();
            GifLists = new ObservableCollection<SelectorItem>();

            IconLists.Add(new SelectorItem { GeometryData = App.Current.Resources["EyeGeometry"] as Geometry, Content = "第一个Icon" });
            IconLists.Add(new SelectorItem { GeometryData = App.Current.Resources["LockGeometry"] as Geometry, Content = "第二个Icon" });
            IconLists.Add(new SelectorItem { GeometryData = App.Current.Resources["LeafGeometry"] as Geometry, Content = "第三个Icon" });

            var source1 = new BitmapImage(new Uri("/CookPopularUI.WPFDemo;component/Assets/Media/tomcat.png", UriKind.Relative));
            var source2 = new BitmapImage(new Uri("/CookPopularUI.WPFDemo;component/Assets/Media/programmer.gif", UriKind.Relative));
            var source3 = new BitmapImage(new Uri("/CookPopularUI.WPFDemo;component/Assets/Media/timg.png", UriKind.Relative));
            ImageLists.Add(new SelectorItem { ImageSource = source1, Content = "第一张图片" });
            ImageLists.Add(new SelectorItem { ImageSource = source2, Content = "第二张图片" });
            ImageLists.Add(new SelectorItem { ImageSource = source3, Content = "第三张图片" });

            var uri1 = new Uri("/CookPopularUI.WPFDemo;component/Assets/Media/cook.gif", UriKind.Relative);
            var uri2 = new Uri("/CookPopularUI.WPFDemo;component/Assets/Media/tomcat.gif", UriKind.Relative);
            var uri3 = new Uri("/CookPopularUI.WPFDemo;component/Assets/Media/programmer.gif", UriKind.Relative);
            GifLists.Add(new SelectorItem { GifSource = uri1, Content = "第一张Gif" });
            GifLists.Add(new SelectorItem { GifSource = uri2, Content = "第二张Gif" });
            GifLists.Add(new SelectorItem { GifSource = uri3, Content = "第三张Gif" });
        }

        protected override void OnLoadedAction(FrameworkElement element)
        {
            var comboBox = element as ComboBox;
            ItemsControlAttached.AddItemCheckedHandler(comboBox, OnItemCheckedAction);
            ItemsControlAttached.AddItemDeleteHandler(comboBox, OnItemDeleteAction);
        }

        private void OnItemCheckedAction(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var comboBox = sender as ComboBox;
            var o = e.OriginalSource as ComboBoxItem;
            var s = e.Source;
        }

        private void OnItemDeleteAction(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var o = e.OriginalSource as ListBoxItem;
            var s = e.Source;

            Lists.Remove(o.Content);
        }
    }
}
