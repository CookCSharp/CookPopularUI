/*
 *Description: CarouselDemoViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/11/22 17:55:53
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularUI.WPF.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CookPopularUI.WPFDemo.ViewModels
{
    public class CarouselDemoViewModel : ViewModelBase
    {
        public List<CarouselItem> ImageLists { get; set; }

        public CarouselDemoViewModel()
        {
            ImageLists = new List<CarouselItem>();
        }

        protected override void OnLoadedAction(FrameworkElement element)
        {
            ImageLists.Add(new CarouselItem { Source = GetImageSource(1), Title = "第一张图片", Explain = "当前显示第一张图片的介绍" });
            ImageLists.Add(new CarouselItem { Source = GetImageSource(2), Title = "第二张图片", Explain = "当前显示第二张图片的介绍" });
            ImageLists.Add(new CarouselItem { Source = GetImageSource(3), Title = "第三张图片", Explain = "当前显示第三张图片的介绍" });
            ImageLists.Add(new CarouselItem { Source = GetImageSource(4), Title = "第四张图片", Explain = "当前显示第四张图片的介绍" });
            ImageLists.Add(new CarouselItem { Source = GetImageSource(5), Title = "第五张图片", Explain = "当前显示第五张图片的介绍" });
            ImageLists.Add(new CarouselItem { Source = GetImageSource(6), Title = "第六张图片", Explain = "当前显示第六张图片的介绍" });
            ImageLists.Add(new CarouselItem { Source = GetImageSource(7), Title = "第七张图片", Explain = "当前显示第七张图片的介绍" });
            ImageLists.Add(new CarouselItem { Source = GetImageSource(8), Title = "第八张图片", Explain = "当前显示第八张图片的介绍" });
            ImageLists.Add(new CarouselItem { Source = GetImageSource(9), Title = "第九张图片", Explain = "当前显示第九张图片的介绍" });
        }

        private ImageSource GetImageSource(int number)
        {
            string path = "pack://application:,,,/CookPopularUI.WPFDemo;component/Assets/CarouselImages";
            BitmapImage bitmap = new BitmapImage(new Uri($"{path}/{number}.jpg", UriKind.Absolute));

            return bitmap;
        }

        private void CarouselViewDemo_UnLoaded(object sender, RoutedEventArgs e)
        {
            ImageLists.Clear();
        }
    }
}
