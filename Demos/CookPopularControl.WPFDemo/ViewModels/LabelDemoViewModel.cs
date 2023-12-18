/*
 *Description: LabelDemoViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/12/12 10:39:54
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit.Windows;
using CookPopularUI.WPF;
using CookPopularUI.WPF.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CookPopularUI.WPFDemo.ViewModels
{
    public class LabelDemoViewModel : ViewModelBase
    {
        public List<GroupLableItem> GroupLists { get; set; }
        public List<GroupLableItem> GroupHeaderLists { get; set; }
        public List<GroupLableItem> GroupImageLists { get; set; }
        public List<GroupLableItem> GroupIconLists { get; set; }

        private List<string> Letters = new List<string> { "A", "B", "C", "D", "E", "F", "G" };

        public LabelDemoViewModel()
        {
            GroupLists = new List<GroupLableItem>();
            for (int i = 0; i < 6; i++)
            {
                GroupLists.Add(new GroupLableItem { Content = $"写代码的厨子{i + 1}" });
            }

            GroupHeaderLists = new List<GroupLableItem>();
            for (int i = 0; i < 6; i++)
            {
                GroupHeaderLists.Add(new GroupLableItem { Header = Letters[i], Content = $"写代码的厨子{i + 1}" });
            }

            GroupImageLists = new List<GroupLableItem>();
            for (int i = 0; i < 6; i++)
            {
                GroupImageLists.Add(new GroupLableItem { Header = "pack://application:,,,/CookPopularUI.WPFDemo;component/Assets/Media/tomcat.png", Content = $"写代码的厨子{i + 1}" });
            }

            GroupIconLists = new List<GroupLableItem>();
            for (int i = 0; i < 6; i++)
            {
                GroupIconLists.Add(new GroupLableItem { Header = ResourceHelper.GetResource<Geometry>("StarGeometry"), Content = $"写代码的厨子{i + 1}" });
            }
        }

        public void ItemClosedAction(object sender, RoutedPropertySingleEventArgs<object> e)
        {

        }
    }
}
