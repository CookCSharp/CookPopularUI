/*
 *Description: PopularTheme
 *Author: Chance.zheng
 *Creat Time: 2023/9/4 20:06:00
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace CookPopularUI.WPF.Themes
{
    public enum LanguageType
    {
        Chinese,
        English,
        //Japanese,
        //French,
        //German,
        //Korean,
    }

    public enum ThemeType
    {
        Light,
        Dark,
        Monokai,
        DogerBlue,
    }

    public class PopularTheme : ResourceDictionary, INotifyPropertyChanged
    {
        private LanguageType _language;
        public LanguageType Language
        {
            get => _language;
            set
            {
                if (_language == value) return;
                _language = value;
                RaisePropertyChanged();

                value.SetLanguage();
            }
        }

        private ThemeType _theme;
        public ThemeType Theme
        {
            get { return _theme; }
            set
            {
                if (_theme == value) return;
                _theme = value;
                RaisePropertyChanged();

                value.SetTheme();
            }
        }

        public PopularTheme()
        {
            Language.SetLanguage();
            Theme.SetTheme();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
