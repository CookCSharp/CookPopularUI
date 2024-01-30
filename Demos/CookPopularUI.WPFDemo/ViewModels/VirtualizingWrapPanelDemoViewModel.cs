using CommunityToolkit.Mvvm.Input;
using CookPopularUI.WPF.Controls;
using CookPopularUI.WPFDemo.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace CookPopularUI.WPFDemo.ViewModels
{
    public partial class VirtualizingWrapPanelDemoViewModel : BindableBase
    {
        public Orientation Orientation { get; set; }
        public SpacingMode SpacingMode { get; set; } = SpacingMode.Uniform;
        public bool StretchItems { get; set; }
        public VirtualizationCacheLengthUnit CacheUnit { get; set; } = VirtualizationCacheLengthUnit.Page;
        public VirtualizationCacheLength CacheLength { get; set; } = new VirtualizationCacheLength(1);
        public VirtualizationMode VirtualizationMode { get; set; } = VirtualizationMode.Recycling;
        public ScrollUnit ScrollUnit { get; set; } = ScrollUnit.Pixel;
        public bool IsScrollByPixel => ScrollUnit == ScrollUnit.Pixel;
        public bool IsScrollByItem => ScrollUnit == ScrollUnit.Item;
        public double ScrollLineDelta { get; set; } = 16;
        public double MouseWheelDelta { get; set; } = 48;
        public int ScrollLineDeltaItem { get; set; } = 1;
        public int MouseWheelDeltaItem { get; set; } = 3;
        public ScrollBarVisibility HorizontalScrollBarVisibility { get; set; } = ScrollBarVisibility.Auto;
        public ScrollBarVisibility VerticalScrollBarVisibility { get; set; } = ScrollBarVisibility.Auto;
        public Size ItemSize { get; set; } = Size.Empty;
        public long MemoryUsageInMB { get; set; }


        public IItemSizeProvider ItemSizeProvider { get; } = new TestItemSizeProvider();
        public Orientation OrientationGroupPanel { get; set; } = Orientation.Vertical;
        public bool IsWrappingKeyboardNavigationEnabled { get; set; }
        public ICollectionView CollectionView { get; }
        public ObservableCollection<VirtualizingWrapPanelDemoModel> Items { get; } = new ObservableCollection<VirtualizingWrapPanelDemoModel>();


        public VirtualizingWrapPanelDemoViewModel()
        {
            AddItems();
            CollectionView = CollectionViewSource.GetDefaultView(Items);
            CollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(VirtualizingWrapPanelDemoModel.Group)));
        }

        public void AddItems()
        {
            for (int i = 0; i < 5000; i++)
            {
                Items.Add(new VirtualizingWrapPanelDemoModel("Group " + (i / 100 + 1), i + 1));
            }
        }

        private void OnCacheUnitChanged()
        {
            switch (CacheUnit)
            {
                case VirtualizationCacheLengthUnit.Item:
                    CacheLength = new VirtualizationCacheLength(10, 10);
                    break;
                case VirtualizationCacheLengthUnit.Page:
                    CacheLength = new VirtualizationCacheLength(1, 1);
                    break;
                case VirtualizationCacheLengthUnit.Pixel:
                    CacheLength = new VirtualizationCacheLength(100, 100);
                    break;
            }
        }

        private void OnOrientationChanged()
        {
            OrientationGroupPanel = Orientation == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal;
        }

        [RelayCommand]
        private void RefreshMemoryUsage()
        {
            GC.GetTotalMemory(true);
            using (Process process = Process.GetCurrentProcess())
            {
                MemoryUsageInMB = process.PrivateMemorySize64 / (1024 * 1024);
            }
        }
    }
}
