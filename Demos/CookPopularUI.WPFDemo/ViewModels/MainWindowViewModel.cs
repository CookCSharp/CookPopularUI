/*
 *Description: MainWindowViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/8/21 16:18:17
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit;
using CookPopularUI.WPF.Themes;
using CookPopularUI.WPFDemo.Views;
using Microsoft.Xaml.Behaviors;
using PropertyChanged;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CookPopularUI.WPFDemo.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        [DoNotNotify]
        public ThemeType Theme { get; set; }
        [DoNotNotify]
        public LanguageType Language { get; set; }
        public bool IsShowSideBar { get; set; }
        public ObservableCollection<string> DemoViewNames { get; set; }
        public ObservableCollection<FrameworkElement> DemoViews { get; set; }
        public int SelectedViewIndex { get; set; } = -1;


        public DelegateCommand GoToHomePageCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(OnGoToHomePageAction)).Value;
        public DelegateCommand ThemeSwitchCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(OnThemeSwitchAction)).Value;
        public DelegateCommand LanguageSwitchCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(OnLanguageSwitchAction)).Value;

        public MainWindowViewModel()
        {
            DemoViewNames = new ObservableCollection<string>();
            var viewNames = this.GetType().Assembly.GetTypes()
                                .Where(element => element.Name.EndsWith("DemoView"))
                                .Select(element => element.Name)
                                .OrderBy(element => element);
            DemoViewNames.AddRange(viewNames);
        }

        private void OnSelectedViewIndexChanged()
        {
            CenterTitle = $"CookPopularUI.WPF({DemoViewNames[SelectedViewIndex].Replace("DemoView", "")})";
            App.UnityContainer.Resolve<IRegionManager>().RequestNavigate("MainWindowContent", DemoViewNames[SelectedViewIndex]);
            IsShowSideBar = false;
        }

        private void OnGoToHomePageAction()
        {
            CenterTitle = $"CookPopularUI.WPF(Home)";
            App.UnityContainer.Resolve<IRegionManager>().RequestNavigate("MainWindowContent", nameof(HomeDemoView));
        }

        private void OnThemeSwitchAction()
        {
            Theme = PopularThemeExtended.GetCurrentTheme();
            if (Theme == ThemeType.Dark)
                Theme = ThemeType.Light;
            else if (Theme == ThemeType.Light)
                Theme = ThemeType.Dark;

            Theme.SetTheme();
        }

        private void OnLanguageSwitchAction()
        {
            Language = PopularThemeExtended.GetLanguage();
            if (Language == LanguageType.Chinese)
                Language = LanguageType.English;
            else if (Language == LanguageType.English)
                Language = LanguageType.Chinese;

            Language.SetLanguage();
        }
    }
}
