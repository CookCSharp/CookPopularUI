/*
 *Description: GeometryDemoViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/11/21 19:06:01
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit;
using CookPopularToolkit.Windows;
using CookPopularUI.WPF.Controls;
using ImTools;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CookPopularUI.WPFDemo.ViewModels
{
    public class GeometryInfo : BindableBase
    {
        public Geometry Data { get; set; }

        public string Tooltip { get; set; }
    }

    public class GeometryDemoViewModel : ViewModelBase
    {
        public int Count { get; set; }
        public string SearchData { get; set; }
        public string IsLoading { get; set; }

        public ObservableCollection<GeometryInfo> Geometries { get; set; }

        private ObservableCollection<GeometryInfo> _allGeometry;

        public DelegateCommand<Geometry> CopyCommand => new Lazy<DelegateCommand<Geometry>>(() => new DelegateCommand<Geometry>(OnCopyAction)).Value;

        public GeometryDemoViewModel()
        {
            var geos = Enum.GetNames<IconPatternKind>().Skip(1).Select(kind =>
            {
                string name = $"{kind}Geometry";
                return new GeometryInfo
                {
                    Data = ResourceHelper.GetResource<Geometry>(name),
                    Tooltip = name
                };
            });

            _allGeometry = new ObservableCollection<GeometryInfo>(geos);
            Geometries = new ObservableCollection<GeometryInfo>(geos);
        }

        public void UpdateGeometries()
        {
            Geometries.Clear();

            if (string.IsNullOrEmpty(SearchData))
            {
                Geometries = new ObservableCollection<GeometryInfo>(_allGeometry);
            }
            else
            {
                Geometries.AddRange(_allGeometry.Where(info => info.Tooltip.Contains(SearchData, StringComparison.CurrentCultureIgnoreCase)));
            }
        }

        private void OnCopyAction(Geometry data)
        {
            var figures = PathGeometry.CreateFromGeometry(data).Figures;
            var clipboardTextBackup = figures.ToCombiner();

            var clipboardText = string.Join(null, figures);

            Clipboard.SetText(clipboardText);

            ToastMessage.ShowInfo("已复制到剪贴板");
        }
    }
}
