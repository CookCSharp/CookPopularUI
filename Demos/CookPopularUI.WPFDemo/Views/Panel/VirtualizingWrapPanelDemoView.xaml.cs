using CookPopularUI.WPFDemo.Models;
using CookPopularUI.WPFDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CookPopularUI.WPFDemo.Views
{
    /// <summary>
    /// VirtualizingWrapPanelDemoView.xaml 的交互逻辑
    /// </summary>
    public partial class VirtualizingWrapPanelDemoView : UserControl
    {
        public VirtualizingWrapPanelDemoView()
        {
            InitializeComponent();
            _vm = this.DataContext as VirtualizingWrapPanelDemoViewModel;
            App.Current.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private VirtualizingWrapPanelDemoViewModel _vm;

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _vm.CollectionView.Filter = new Predicate<object>((item) =>
            {
                if (int.TryParse(filterTextBox.Text, out int filterValue))
                {
                    return ((VirtualizingWrapPanelDemoModel)item).Number > filterValue;
                }
                return true;
            });
        }

        private void ScrollIntoViewTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ScrollIntoView();
            }
        }

        private void ScrollIntoViewTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ScrollIntoView();
        }

        private void ScrollIntoView()
        {
            if (int.TryParse(scrollIntoViewTextBox.Text, out int itemNumber)
                && _vm.Items.Where(item => item.Number == itemNumber).FirstOrDefault() is { } item)
            {
                var itemsControl = FindItemsControl();

                if (itemsControl is ListView listView)
                {
                    listView?.ScrollIntoView(item);
                }
                else if (itemsControl is ListBox listBox)
                {
                    listBox?.ScrollIntoView(item);
                }
            }
            scrollIntoViewTextBox.Clear();
        }

        private ItemsControl FindItemsControl()
        {
            var content = (DependencyObject)tabControl.SelectedContent;
            return content as ItemsControl ?? GetChildOfType<ItemsControl>(content)!;
        }

        private static T GetChildOfType<T>(DependencyObject element) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null) return result;
            }
            return null;
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Debug.WriteLine(e.Exception);

            if (e.Exception is InvalidOperationException)
            {
                e.Handled = true;
                _vm.VirtualizationMode = _vm.VirtualizationMode == VirtualizationMode.Standard
                                        ? VirtualizationMode.Recycling
                                        : VirtualizationMode.Standard;
                MessageBox.Show(e.Exception.Message);
            }
        }
    }
}
