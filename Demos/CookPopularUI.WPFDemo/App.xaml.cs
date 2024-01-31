using AutoMapper;
using CookPopularUI.WPF.Controls;
using CookPopularUI.WPF.Themes;
using CookPopularUI.WPFDemo.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using PropertyChanged;
using System.Windows.Controls;

namespace CookPopularUI.WPFDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public App()
        {
            var color1 = SystemColors.ControlColor;
            var color2 = SystemColors.ControlLightColor;
            var color3 = SystemColors.ControlLightLightColor;
            var color4 = SystemColors.ControlDarkColor;
            var brush = SystemColors.ActiveBorderColor;

            var geo = new System.Windows.Media.RectangleGeometry(new Rect(new Point(0, 0), new Point(10, 30)));
            var ss = geo.GetOutlinedPathGeometry().Figures;

            //XamlTool.GetAllXamlFile();
        }

        public static IContainerProvider UnityContainer { get; private set; }

        private void ConfigureService(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AnimationDemoView>();
            containerRegistry.RegisterForNavigation<BlockBarDemoView>();
            containerRegistry.RegisterForNavigation<BorderDemoView>();
            containerRegistry.RegisterForNavigation<ButtonDemoView>();
            containerRegistry.RegisterForNavigation<CarouselDemoView>();
            containerRegistry.RegisterForNavigation<CheckBoxDemoView>();
            containerRegistry.RegisterForNavigation<ColorPickerDemoView>();
            containerRegistry.RegisterForNavigation<ComboBoxDemoView>();
            containerRegistry.RegisterForNavigation<DataGridDemoView>();
            containerRegistry.RegisterForNavigation<DateTimeDemoView>();
            containerRegistry.RegisterForNavigation<DialogBoxDemoView>();
            containerRegistry.RegisterForNavigation<ExpanderDemoView>();
            containerRegistry.RegisterForNavigation<GeometryDemoView>();
            containerRegistry.RegisterForNavigation<GroupBoxDemoView>();
            containerRegistry.RegisterForNavigation<IconDemoView>();
            containerRegistry.RegisterForNavigation<InputDemoView>();
            containerRegistry.RegisterForNavigation<LabelDemoView>();
            containerRegistry.RegisterForNavigation<ListBoxDemoView>();
            containerRegistry.RegisterForNavigation<ListViewDemoView>();
            containerRegistry.RegisterForNavigation<MarkupExtensionDemoView>();
            containerRegistry.RegisterForNavigation<MediaDemoView>();
            containerRegistry.RegisterForNavigation<MenuDemoView>();
            containerRegistry.RegisterForNavigation<MessageDemoView>();
            containerRegistry.RegisterForNavigation<OtherButtonDemoView>();
            containerRegistry.RegisterForNavigation<OtherControlDemoView>();
            containerRegistry.RegisterForNavigation<PanelDemoView>();
            containerRegistry.RegisterForNavigation<ProgressBarDemoView>();
            containerRegistry.RegisterForNavigation<ProgressButtonDemoView>();
            containerRegistry.RegisterForNavigation<PropertyGridDemoView>();
            containerRegistry.RegisterForNavigation<RadioButtonDemoView>();
            containerRegistry.RegisterForNavigation<ScrollViewerDemoView>();
            containerRegistry.RegisterForNavigation<ShapeDemoView>();
            containerRegistry.RegisterForNavigation<SliderDemoView>();
            containerRegistry.RegisterForNavigation<SideBarDemoView>();
            containerRegistry.RegisterForNavigation<SideBarExDemoView>();
            containerRegistry.RegisterForNavigation<StepBarDemoView>();
            containerRegistry.RegisterForNavigation<SwiperDemoView>();
            containerRegistry.RegisterForNavigation<TabControlDemoView>();
            containerRegistry.RegisterForNavigation<TitleBarDemoView>();
            containerRegistry.RegisterForNavigation<ToolBarDemoView>();
            containerRegistry.RegisterForNavigation<ToggleButtonDemoView>();
            containerRegistry.RegisterForNavigation<TreeViewDemoView>();
            containerRegistry.RegisterForNavigation<VirtualizingWrapPanelDemoView>();
            containerRegistry.RegisterForNavigation<WindowDemoView>();
            containerRegistry.RegisterForNavigation<WrapPanelFillDemoView>();

            containerRegistry.RegisterForNavigation<HomeDemoView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ConfigureService(containerRegistry);
            UnityContainer = Container;

            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion<HomeDemoView>("MainWindowContent");
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
    }
}
