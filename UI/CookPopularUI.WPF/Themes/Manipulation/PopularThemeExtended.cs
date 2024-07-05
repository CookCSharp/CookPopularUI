/*
 *Description: PopularThemeExtended
 *Author: Chance.zheng
 *Creat Time: 2023/9/25 16:59:17
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit;
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
        private const string DefaultControlPattern = @"pack://application:,,,/CookPopularUI.WPF;component/Themes/DefaultPopularControl.xaml";
        private const string DefaultColorPattern = @"^pack:\/\/application:,,,\/CookPopularUI.WPF;component\/Themes\/([A-Za-z]*)Color.xaml$";
        private const string CustomColorPattern = @"^pack:\/\/application:,,,\/CookPopularUI.WPF;component\/Themes\/Colors\/([A-Za-z]*)Color.xaml$";

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

        public static LanguageType GetLanguage() => GetCurrentCulture().Name switch
        {
            "es" => LanguageType.English,
            "zh-Hans" => LanguageType.Chinese,
            _ => throw new NotImplementedException(),
        };

        public static CultureInfo GetCurrentCulture() => Language.Culture ?? CultureInfo.CurrentUICulture;

        public static string GetLanguageKeyValue(string key) => Language.ResourceManager.GetString(key, Language.Culture) ?? throw new ArgumentException("not valid param of key's value", nameof(key));

        public static void SetTheme(this ThemeType theme)
        {
            var defaultColorResourceDictionary = Application.Current.Resources.MergedDictionaries.FirstOrDefault(r =>
            {
                if (r is PopularTheme popularTheme)
                {
                    return popularTheme.Theme == ThemeType.Light;
                }
                else
                {
                    return Regex.IsMatch(r.Source.OriginalString, DefaultColorPattern);
                }
            });
            var customColorResourceDictionaries = Application.Current.Resources.MergedDictionaries.Where(r =>
            {
                if (r is PopularTheme popularTheme)
                {
                    return popularTheme.Theme != ThemeType.Light;
                }
                else
                {
                    return Regex.IsMatch(r.Source.OriginalString, CustomColorPattern);
                }
            });
            Application.Current.Resources.MergedDictionaries.Remove(defaultColorResourceDictionary);
            customColorResourceDictionaries.ForEach(r =>
            {
                Application.Current.Resources.MergedDictionaries.Remove(r);
            });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri($"pack://application:,,,/CookPopularUI.WPF;component/Themes/Colors/{theme}Color.xaml", UriKind.Absolute) });
            if (!Application.Current.Resources.MergedDictionaries.Contains(r => r.Source.OriginalString.Equals(DefaultControlPattern)))
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/CookPopularUI.WPF;component/Themes/DefaultPopularControl.xaml", UriKind.Absolute) });
        }

        public static ThemeType GetCurrentTheme()
        {
            foreach (var dic in Application.Current.Resources.MergedDictionaries.Reverse())
            {
                if (dic is PopularTheme)
                {
                    return ((PopularTheme)dic).Theme;
                }
                else
                {
                    var themeString = dic.Source.OriginalString.Split('/').IndexValue(-1).Replace("Color.xaml", "");
                    if (Enum.TryParse(themeString, out ThemeType theme))
                    {
                        return theme;
                    }
                }
            }

            return default;
        }
    }
}
