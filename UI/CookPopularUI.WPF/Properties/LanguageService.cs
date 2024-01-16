/*
 *Description: LanguageService
 *Author: Chance.zheng
 *Creat Time: 2023/9/4 19:43:09
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml.Linq;

namespace CookPopularUI.WPF
{
    internal class LanguageService : INotifyPropertyChanged
    {
        private static readonly Lazy<LanguageService> _current = new Lazy<LanguageService>(() => new LanguageService());
        public static LanguageService Current => _current.Value;

        private readonly Language language = new Language();
        public Language Language => language;

        public static CultureInfo Culture => Language.Culture;

        private LanguageService() { }

        public void SwitchLanguage(string cultureName)
        {
            Language.Culture = CultureInfo.GetCultureInfo(cultureName);
            this.RaisePropertyChanged("Culture");
            UpdateLanguage();
        }

        private void UpdateLanguage()
        {
            RaisePropertyChanged(nameof(Cancel));
            RaisePropertyChanged(nameof(No));
            RaisePropertyChanged(nameof(Ok));
            RaisePropertyChanged(nameof(Yes));
            RaisePropertyChanged(nameof(DragDropAreaHintText));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        #region Properties

        public string Cancel => Language.Cancel;
        public string No => Language.No;
        public string Ok => Language.Ok;
        public string Yes => Language.Yes;
        public string DragDropAreaHintText => Language.DragDropAreaHintText;

        #endregion
    }
}
