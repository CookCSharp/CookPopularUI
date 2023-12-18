/*
 *Description: PropertyGridDemoViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/8/21 16:29:58
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using AutoMapper;
using CookPopularToolkit;
using CookPopularUI.WPFDemo.Models;
using Prism.Commands;
using System.Windows.Media.Imaging;

namespace CookPopularUI.WPFDemo.ViewModels
{
    public class PropertyGridDemoViewModel : ViewModelBase
    {
        private IMapper _mapper;
        private int index = 1;

        public PropertyGridDemoModel DemoModel { get; set; }

        public DelegateCommand UpdateCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(OnUpdate)).Value;

        public PropertyGridDemoViewModel()
        {
            var profile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            _mapper = configuration.CreateMapper();

            DemoModel = TypeHelper.SetPropertiesDefaultValue<PropertyGridDemoModel>();
            DemoModel.Lists = new List<string>() { "Chance1", "Chance2", "Chance3" };
            DemoModel.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/CookPopularUI.WPFDemo;component/Assets/Media/tomcat.png", UriKind.Absolute));
        }

        private void OnUpdate()
        {
            DemoModel.EditableString = $"数据第{index++}次更新了";
            DemoModel.ReadOnlyString = "不可编辑文本";

            //DemoModel = _mapper.Map<PropertyGridDemoModel>(DemoModel);
        }

        private void OnDemoModelChanged()
        {
            System.Diagnostics.Debug.WriteLine("对象DemoModel变化了");
        }
    }
}
