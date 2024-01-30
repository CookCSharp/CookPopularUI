/*
 *Description: VirtualizingWrapPanelDemoModel
 *Author: Chance.zheng
 *Creat Time: 2024/1/29 16:03:43
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using CookPopularUI.WPF.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CookPopularUI.WPFDemo.Models
{
    internal class TestItemSizeProvider : IItemSizeProvider
    {
        public Size GetSizeForItem(object item)
        {
            var testItem = (VirtualizingWrapPanelDemoModel)item;
            return new Size(testItem.Size.Width + 10, testItem.Size.Height + 4);
        }
    }

    public class VirtualizingWrapPanelDemoModel : ModelBase
    {
        private static readonly int MinWidth = 100;
        private static readonly int MaxWidth = 200;
        private static readonly int MinHeight = 100;
        private static readonly int MaxHeight = 200;

        public string Group { get; }

        public int Number { get; }

        public Color Background { get; }

        public Size SizeLazy
        {
            get
            {
                if (sizeLazy == Size.Empty)
                {
                    Task.Delay(1000).ContinueWith((_) =>
                    {
                        sizeLazy = Size;
                        OnPropertyChanged(new PropertyChangedEventArgs(nameof(SizeLazy)));
                    });
                    return new Size(MinWidth, MinHeight);
                }
                return sizeLazy;
            }
        }

        public DateTime CurrentDateTime
        {
            get
            {
                sizeLazy = Size.Empty;
                return DateTime.Now;
            }
        }

        public Size Size { get; }

        private static Random random = new Random();

        private Size sizeLazy = Size.Empty;

        public VirtualizingWrapPanelDemoModel(string group, int number)
        {
            Group = group;
            Number = number;
            byte[] randomBytes = new byte[3];
            random.NextBytes(randomBytes);
            Background = Color.FromRgb(randomBytes[0], randomBytes[1], randomBytes[2]);
            var width = random.Next(MinWidth, MaxWidth);
            var height = random.Next(MinHeight, MaxHeight);
            Size = new Size(width, height);
        }

        public override string ToString()
        {
            return $"TestItem({Number})";
        }
    }
}
