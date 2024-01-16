/*
 *Description: DragDropArea
 *Author: Chance.zheng
 *Creat Time: 2024/1/15 17:46:56
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using CookPopularToolkit.Windows;
using CookPopularUI.WPF.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace CookPopularUI.WPF.Controls
{
    public class DragDropArea : Control
    {
        public event EventHandler<string[]> DragDroped;
        private FileDroper _droper;
        private double _dpiX;
        private double _dpiY;


        private static readonly DependencyPropertyKey DropFilePathsPropertyKey =
                DependencyProperty.RegisterReadOnly(nameof(DropFilePaths), typeof(string[]), typeof(DragDropArea), new PropertyMetadata(new string[] { }));
        public string[] DropFilePaths
        {
            get => (string[])GetValue(DropFilePathsProperty);
            private set => SetValue(DropFilePathsPropertyKey, value);
        }
        public static readonly DependencyProperty DropFilePathsProperty = DropFilePathsPropertyKey.DependencyProperty;


        public DragDropArea()
        {
            _droper = new FileDroper();
            Loaded += DragDropArea_Loaded;
            Unloaded += DragDropArea_Unloaded;
            _droper.DragDrop += DragDrop;
        }

        private void DragDropArea_Loaded(object sender, RoutedEventArgs e)
        {
            var matrix = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
            _dpiX = matrix.M11;
            _dpiY = matrix.M22;

            _droper.Hwnd = ((HwndSource)PresentationSource.FromVisual(this)).Handle;
            //_droper.Hwnd = new WindowInteropHelper(Window.GetWindow(this)).Handle;
            _droper.AddHook();
        }

        private void DragDropArea_Unloaded(object sender, RoutedEventArgs e)
        {
            _droper.RemoveHook();
        }

        private void DragDrop(object? sender, DragEventArgs e)
        {
            var rect = this.GetBoundsRelativeTo(Window.GetWindow(this));
            var winPoint = new Point(_droper.DropPoint.X / _dpiX, _droper.DropPoint.Y / _dpiY);
            if (rect != null)
            {
                if (winPoint.X >= rect.Value.X && winPoint.X <= rect.Value.X + rect.Value.Width &&
                    winPoint.Y >= rect.Value.Y && winPoint.Y <= rect.Value.Y + rect.Value.Height)
                {
                    DropFilePaths = _droper.DropFilePaths;
                    DragDroped?.Invoke(this, _droper.DropFilePaths);
                }
            }
        }
    }
}
