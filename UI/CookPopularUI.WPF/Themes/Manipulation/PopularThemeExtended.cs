/*
 *Description: PopularThemeExtended
 *Author: Chance.zheng
 *Creat Time: 2023/9/25 16:59:17
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit;
using CookPopularUI.WPF.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace CookPopularUI.WPF.Themes
{
    /// <summary>
    /// 主题扩展类
    /// </summary>
    public static class PopularThemeExtended
    {
        private const string DefaultPattern = @"^pack:\/\/application:,,,\/CookPopularUI.WPF;component\/Themes\/([A-Za-z]*)Color.xaml$";
        private const string CustomPattern = @"^pack:\/\/application:,,,\/CookPopularUI.WPF;component\/Themes\/Colors\/([A-Za-z]*)Color.xaml$";

        //private static readonly PopularThemeExtended _instance = new PopularThemeExtended();
        //public static PopularThemeExtended Instance => _instance;
        ///// <summary>
        ///// 显式的静态构造函数用来告诉C#编译器在其内容实例化之前不要标记其类型
        ///// </summary>
        //static PopularThemeExtended() { }


        public static void SetLanguage(this LanguageType language)
        {
            switch (language)
            {
                case LanguageType.Chinese:
                    LanguageService.Current.SwitchLanguage("zh-Hans");
                    break;
                case LanguageType.English:
                default:
                    LanguageService.Current.SwitchLanguage("es");
                    break;
            }
        }

        public static CultureInfo GetCurrentCulture() => Language.Culture ?? CultureInfo.CurrentUICulture;

        public static string GetLanguageKeyValue(string key) => Language.ResourceManager.GetString(key, Language.Culture) ?? throw new ArgumentException("not valid param of key's value", nameof(key));

        public static void SetTheme(this ThemeType theme)
        {
            var defaultColorResourceDictionary = Application.Current.Resources.MergedDictionaries.FirstOrDefault(r => Regex.IsMatch(r.Source.OriginalString, DefaultPattern));
            var customColorResourceDictionaries = Application.Current.Resources.MergedDictionaries.Where(r => Regex.IsMatch(r.Source.OriginalString, CustomPattern));
            Application.Current.Resources.MergedDictionaries.Remove(defaultColorResourceDictionary);
            customColorResourceDictionaries.ForEach(r => Application.Current.Resources.MergedDictionaries.Remove(r));
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri($"pack://application:,,,/CookPopularUI.WPF;component/Themes/Colors/{theme}Color.xaml", UriKind.Absolute) });
            //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/CookPopularUI.WPF;component/Themes/DefaultPopularControl.xaml", UriKind.Absolute) });
        }

        public static ThemeType GetCurrentTheme()
        {
            foreach (var dic in Application.Current.Resources.MergedDictionaries.Reverse())
            {
                var themeString = dic.Source.OriginalString.Split('/').IndexValue(-1).Replace("Color.xaml", "");
                if (Enum.TryParse(themeString, out ThemeType theme))
                {
                    return theme;
                }
            }

            return default;
        }
    }
}
