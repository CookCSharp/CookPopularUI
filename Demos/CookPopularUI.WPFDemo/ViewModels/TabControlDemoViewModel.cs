/*
 *Description: TabControlDemoViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/11/17 15:52:12
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CookPopularUI.WPFDemo.ViewModels
{
    public class TabItemInfo
    {
        public string Header { get; set; }

        public Geometry Icon { get; set; }

        public string Content { get; set; }
    }

    public class TabControlDemoViewModel : ViewModelBase
    {
        public ObservableCollection<TabItemInfo> Tabs { get; set; }

        public TabControlDemoViewModel()
        {
            Tabs = new ObservableCollection<TabItemInfo>();
            Tabs.Add(new TabItemInfo { Header = "First", Icon = ResourceHelper.GetResource<Geometry>("AlertorGeometry"), Content = "第一个页面" });
            Tabs.Add(new TabItemInfo { Header = "Second", Icon = ResourceHelper.GetResource<Geometry>("EyeGeometry"), Content = "第二个页面" });
            Tabs.Add(new TabItemInfo { Header = "Third", Icon = ResourceHelper.GetResource<Geometry>("FileGeometry"), Content = "第三个页面" });
            Tabs.Add(new TabItemInfo { Header = "Fourth", Icon = ResourceHelper.GetResource<Geometry>("LanguageGeometry"), Content = "第四个页面" });
            Tabs.Add(new TabItemInfo { Header = "Fifth", Icon = ResourceHelper.GetResource<Geometry>("MinusGeometry"), Content = "第五个页面" });
            Tabs.Add(new TabItemInfo { Header = "Sixth", Icon = ResourceHelper.GetResource<Geometry>("PlusNoCircleGeometry"), Content = "第六个页面" });
            Tabs.Add(new TabItemInfo { Header = "Seventh", Icon = ResourceHelper.GetResource<Geometry>("RefreshGeometry"), Content = "第七个页面" });
            Tabs.Add(new TabItemInfo { Header = "Eighth", Icon = ResourceHelper.GetResource<Geometry>("SuccessGeometry"), Content = "第八个页面" });
        }
    }
}
