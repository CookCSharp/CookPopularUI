/*
 *Description: ListBoxDemoViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/10/14 14:10:25
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
using Prism.Commands;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace CookPopularUI.WPFDemo.ViewModels
{
    public class PersonInfo
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public string Description { get; set; }

        public int Index { get; set; }
    }

    public class ListBoxDemoViewModel : ViewModelBase
    {
        public ObservableCollection<object> Lists { get; set; }

        public ObservableCollection<SelectorItem> IconLists { get; set; }

        public ObservableCollection<SelectorItem> ImageLists { get; set; }

        public ObservableCollection<SelectorItem> GifLists { get; set; }

        public ObservableCollection<PersonInfo> PersonInfos { get; set; }


        public ListBoxDemoViewModel()
        {
            Lists = new ObservableCollection<object>() { "Lori", "Chance", "写代码的厨子" };
            IconLists = new ObservableCollection<SelectorItem>();
            ImageLists = new ObservableCollection<SelectorItem>();
            GifLists = new ObservableCollection<SelectorItem>();

            IconLists.Add(new SelectorItem { GeometryData = App.Current.Resources["EyeGeometry"] as Geometry, Content = "第一个Icon" });
            IconLists.Add(new SelectorItem { GeometryData = App.Current.Resources["LockGeometry"] as Geometry, Content = "第二个Icon" });
            IconLists.Add(new SelectorItem { GeometryData = App.Current.Resources["LeafGeometry"] as Geometry, Content = "第三个Icon" });

            var source1 = new BitmapImage(new Uri("/CookPopularUI.WPFDemo;component/Assets/Media/tomcat.png", UriKind.Relative));
            var source2 = new BitmapImage(new Uri("/CookPopularUI.WPFDemo;component/Assets/Media/c.png", UriKind.Relative));
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

            PersonInfos = new ObservableCollection<PersonInfo>();
            PersonInfos.Add(new PersonInfo { Name = "Lori", Age = 18, Description = "Teacher", Index = 0 });
            PersonInfos.Add(new PersonInfo { Name = "Chance", Age = 28, Description = "Cook", Index = 1 });
            PersonInfos.Add(new PersonInfo { Name = "写代码的厨子", Age = 38, Description = "Cook With Programmer", Index = 2 });
        }

        protected override void OnLoadedAction(FrameworkElement element)
        {
            var listBox = element as ListBox;
            ItemsControlAttached.AddItemCheckedHandler(listBox, OnItemCheckedAction);
            ItemsControlAttached.AddItemDeleteHandler(listBox, (s, e) => OnRemoveAction(e.OriginalSource as ListBoxItem));
        }

        private void OnItemCheckedAction(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var source = e.OriginalSource as ListBoxItem;
            var isChecked = e.NewValue;
        }

        private void OnRemoveAction(ListBoxItem listBoxItem)
        {
            Lists.Remove(listBoxItem.Content);
        }
    }
}
