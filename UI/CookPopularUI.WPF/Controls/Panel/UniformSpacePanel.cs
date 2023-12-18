/*
 * Description：UniformSpacePanel 
 * Author： Chance_写代码的厨子
 * Company: Chance
 * Create Time：2021-11-14 02:28:30
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) Chance 2021 All Rights Reserved.
 */


using CookPopularToolkit;
using CookPopularToolkit.Windows;
using Microsoft.CodeAnalysis;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace CookPopularUI.WPF.Controls
{
    [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
    public enum UniformType
    {
        /// <summary>
        /// 每一项等间距，即子项之间、子项与两端都是等间距
        /// </summary>
        Every,
        /// <summary>
        /// 只有子项等间距，即子项之间等间距，子项与两端不是等间距
        /// </summary>
        Item,
        /// <summary>
        /// 只有子项之间等间距，子项与两端间距为0
        /// </summary>
        OnlyItem
    }

    [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
    public enum ItemWrapping
    {
        NoWrap = 1,
        Wrap = 2
    }

    /// <summary>
    /// 按照水平或垂直方向等份布局的面板
    /// </summary>
    /// <remarks>
    /// 支持等份布局、子项间等间距布局（包含与两端0间距），换行/列时依然支持，效果如下:
    /// 1.Every       --1--1--1--1--
    /// 2.Item        --1----1----1----1--
    /// 3.OnlyItem    1--1--1--1
    /// </remarks>
    public class UniformSpacePanel : Panel
    {
        /// <summary>
        /// 子项对齐方式
        /// </summary>
        /// <remarks>
        /// 如果值为<see cref="UniformType.Every"/>，则子项之间及子项与边界之间间距相等；
        /// 如果值为<see cref="UniformType.Item"/>，则相邻子项之间间距相等，两端子项与边界之间间距相等
        /// </remarks>
        public UniformType UniformType
        {
            get { return (UniformType)GetValue(UniformTypeProperty); }
            set { SetValue(UniformTypeProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="UniformType"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty UniformTypeProperty =
            DependencyProperty.Register(nameof(UniformType), typeof(UniformType), typeof(UniformSpacePanel), new FrameworkPropertyMetadata(default(UniformType), FrameworkPropertyMetadataOptions.AffectsMeasure));


        /// <summary>
        /// 子项自动换行
        /// </summary>
        public ItemWrapping ItemWrapping
        {
            get => (ItemWrapping)GetValue(ItemWrappingProperty);
            set => SetValue(ItemWrappingProperty, value);
        }
        /// <summary>
        /// 提供<see cref="ItemWrapping"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ItemWrappingProperty =
            DependencyProperty.Register(nameof(ItemWrapping), typeof(ItemWrapping), typeof(UniformSpacePanel), new FrameworkPropertyMetadata(default(ItemWrapping), FrameworkPropertyMetadataOptions.AffectsMeasure));


        /// <summary>
        /// 子项排列方向
        /// </summary>
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Orientation"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            StackPanel.OrientationProperty.AddOwner(typeof(UniformSpacePanel), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure));


        /// <summary>
        /// 子项水平与垂直间距
        /// </summary>
        public HVDistance ItemHVSpace
        {
            get => (HVDistance)GetValue(ItemHVSpaceProperty);
            set => SetValue(ItemHVSpaceProperty, value);
        }
        /// <summary>
        /// 提供<see cref="ItemHVSpace"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ItemHVSpaceProperty =
            DependencyProperty.Register(nameof(ItemHVSpace), typeof(HVDistance), typeof(UniformSpacePanel), new FrameworkPropertyMetadata(new HVDistance(double.NaN, double.NaN), FrameworkPropertyMetadataOptions.AffectsMeasure), IsItemHVSpaceValid);

        /// <summary>
        /// 子项宽度
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public double ItemWidth
        {
            get => (double)GetValue(ItemWidthProperty);
            set => SetValue(ItemWidthProperty, value);
        }
        /// <summary>
        /// 提供<see cref="ItemWidth"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register(nameof(ItemWidth), typeof(double), typeof(UniformSpacePanel), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), IsWidthHeightValid);


        /// <summary>
        /// 子项高度
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public double ItemHeight
        {
            get => (double)GetValue(ItemHeightProperty);
            set => SetValue(ItemHeightProperty, value);
        }
        /// <summary>
        /// 提供<see cref="ItemHeight"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register(nameof(ItemHeight), typeof(double), typeof(UniformSpacePanel), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), IsWidthHeightValid);


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <remarks>
        /// 仅<c><see cref="Orientation.Vertical"/></c>时有效
        /// </remarks>
        public HorizontalAlignment ItemHorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(ItemHorizontalAlignmentProperty);
            set => SetValue(ItemHorizontalAlignmentProperty, value);
        }
        /// <summary>
        /// 提供<see cref="ItemHorizontalAlignment"/>的依赖属性
        /// </summary>
        /// <remarks>当<see cref="Orientation"/>为<see cref="Orientation.Vertical"/>使用</remarks>
        public static readonly DependencyProperty ItemHorizontalAlignmentProperty =
            DependencyProperty.Register(nameof(ItemHorizontalAlignment), typeof(HorizontalAlignment), typeof(UniformSpacePanel), new FrameworkPropertyMetadata(HorizontalAlignment.Stretch, FrameworkPropertyMetadataOptions.AffectsMeasure));


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <remarks>
        /// 仅<c><see cref="Orientation.Horizontal"/></c>时有效
        /// </remarks>
        public VerticalAlignment ItemVerticalAlignment
        {
            get => (VerticalAlignment)GetValue(ItemVerticalAlignmentProperty);
            set => SetValue(ItemVerticalAlignmentProperty, value);
        }
        /// <summary>
        /// 提供<see cref="ItemVerticalAlignment"/>的依赖属性
        /// </summary>
        /// <remarks>当<see cref="Orientation"/>为<see cref="Orientation.Horizontal"/>使用</remarks>
        public static readonly DependencyProperty ItemVerticalAlignmentProperty =
            DependencyProperty.Register(nameof(ItemVerticalAlignment), typeof(VerticalAlignment), typeof(UniformSpacePanel), new FrameworkPropertyMetadata(VerticalAlignment.Stretch, FrameworkPropertyMetadataOptions.AffectsMeasure));


        private static bool IsWidthHeightValid(object value)
        {
            var v = (double)value;
            return double.IsNaN(v) || v >= 0 && !double.IsPositiveInfinity(v);
        }

        private static bool IsItemHVSpaceValid(object value)
        {
            if (value is HVDistance hvDistance)
            {
                return double.IsNaN(hvDistance.H) || hvDistance.H >= 0 && !double.IsPositiveInfinity(hvDistance.H)
                   && (double.IsNaN(hvDistance.V) || hvDistance.V >= 0 && !double.IsPositiveInfinity(hvDistance.V));
            }

            return false;
        }


        private double _dx, _dy;

        public UniformSpacePanel()
        {
            ClipToBounds = true;
        }

        /// <summary>
        /// 给所有子项分配空间
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns>
        /// 所有子项总空间
        /// </returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            var count = InternalChildren.Count;
            var itemHSpace = double.IsNaN(ItemHVSpace.H) ? 0 : ItemHVSpace.H;
            var itemVSpace = double.IsNaN(ItemHVSpace.V) ? 0 : ItemHVSpace.V;

            List<double> widths = new List<double>();
            List<double> heights = new List<double>();

            for (int i = 0; i < count; i++)
            {
                var element = InternalChildren[i];
                var itemWidth = !double.IsNaN(ItemWidth) ? ItemWidth : availableSize.Width;
                var itemHeight = !double.IsNaN(ItemHeight) ? ItemHeight : availableSize.Height;
                var childSize = new Size(itemWidth, itemHeight);
                element.Measure(childSize);

                var availableItemWidth = !double.IsNaN(ItemWidth) ? ItemWidth : element.DesiredSize.Width;
                var availableItemHeight = !double.IsNaN(ItemHeight) ? ItemHeight : element.DesiredSize.Height;
                widths.Add(availableItemWidth);
                heights.Add(availableItemHeight);
            }

            if (ItemWrapping == ItemWrapping.Wrap)
            {
                if (!MathHelper.IsFiniteDouble(availableSize.Width))
                    availableSize.Width = 0;

                if (!MathHelper.IsFiniteDouble(availableSize.Height))
                    availableSize.Height = 0;

                return availableSize;
            }

            if (double.IsInfinity(availableSize.Width) || availableSize.Width < widths.Sum())
                availableSize.Width = Orientation == Orientation.Horizontal ? widths.Sum() : widths.Max();

            if (double.IsInfinity(availableSize.Height) || availableSize.Height < heights.Sum())
                availableSize.Height = Orientation == Orientation.Horizontal ? heights.Max() : heights.Sum();

            availableSize.Width += (count + 1) * itemHSpace;
            availableSize.Height += (count + 1) * itemVSpace;

            return availableSize;
        }

        private (double, double) CalDistanceXY(Size finalSize, double itemWidth, double itemHeight, double itemHSpace, double itemVSpace)
        {
            double elementsWidth = itemHSpace, distanceX = 0;
            double elementsHeight = itemVSpace, distanceY = 0;

            if (Orientation == Orientation.Horizontal)
            {
                for (int i = 0; i < InternalChildren.Count; i++)
                {
                    if (double.IsNaN(ItemWidth))
                        elementsWidth += (InternalChildren[i].DesiredSize.Width + itemHSpace);
                    else
                        elementsWidth += (ItemWidth + itemHSpace);

                    if (finalSize.Width - elementsWidth >= 0)
                    {
                        distanceX = (finalSize.Width - elementsWidth) / (i + 2);
                    }
                    else
                    {
                        break;
                    }
                }

                _dx = (finalSize.Width - elementsWidth) / (InternalChildren.Count - 1);
                distanceY = (finalSize.Height - itemHeight - itemVSpace) / 2;
            }
            else
            {
                for (int i = 0; i < InternalChildren.Count; i++)
                {
                    if (double.IsNaN(ItemHeight))
                        elementsHeight += (InternalChildren[i].DesiredSize.Height + itemVSpace);
                    else
                        elementsHeight += (ItemHeight + itemVSpace);

                    if (finalSize.Height - elementsHeight >= 0)
                    {
                        distanceY = (finalSize.Height - elementsHeight) / (i + 2);
                    }
                    else
                    {
                        break;
                    }
                }

                distanceX = (finalSize.Width - itemWidth - itemHSpace) / 2;
                _dy = (finalSize.Height - elementsHeight) / (InternalChildren.Count - 1);
            }

            return (distanceX, distanceY);
        }

        /// <summary>
        /// 获取行/列数、每行/列的元素数
        /// </summary>
        /// <param name="finalWidthHeight"></param>
        /// <param name="itemSpace"></param>
        /// <param name="distance"></param>
        /// <param name="isHorizontal"></param>
        /// <returns></returns>
        private Dictionary<int, int> GetLineItems(double finalWidthHeight, double itemSpace, double distance, bool isHorizontal)
        {
            var count = InternalChildren.Count;
            Dictionary<int, int> lineItems = new Dictionary<int, int>();
            if (ItemWrapping == ItemWrapping.Wrap)
            {
                var width_height = GetWidthHeight();
                var remain = (int)width_height % (int)finalWidthHeight;
                var score = (int)width_height / (int)finalWidthHeight;
                var lines = remain == 0 ? score : score + 1;

                for (int line = 0; line < lines; line++)
                {
                    var lineWidthHeight = finalWidthHeight - itemSpace;
                    int lineCount = 0;
                    double itemWidthHeight = 0;
                    for (int i = 0; i < count; i++)
                    {
                        if (isHorizontal)
                        {
                            itemWidthHeight = !double.IsNaN(ItemWidth) ? ItemWidth : InternalChildren[i].DesiredSize.Width;
                        }
                        else
                        {
                            itemWidthHeight = !double.IsNaN(ItemHeight) ? ItemHeight : InternalChildren[i].DesiredSize.Height;
                        }

                        if (lineWidthHeight - itemWidthHeight - itemSpace >= 0)
                        {
                            lineCount++;
                            lineWidthHeight -= itemWidthHeight + itemSpace;
                        }
                    }

                    lineItems.Add(line + 1, lineCount);
                }

                var remainLineCount = count - lineItems.Values.Sum();
                CheckHelper.ActionWhenTrue(remainLineCount > 0, () => lineItems.Add(lines + 1, remainLineCount));
            }
            else
            {
                lineItems.Add(1, count);
            }

            return lineItems;

            double GetWidthHeight()
            {
                double width = distance, height = distance;
                for (int i = 0; i < count; i++)
                {
                    if (double.IsNaN(ItemWidth))
                        width += InternalChildren[i].DesiredSize.Width + distance;
                    else
                        width += ItemWidth + distance;
                }

                for (int i = 0; i < count; i++)
                {
                    if (double.IsNaN(ItemHeight))
                        height += InternalChildren[i].DesiredSize.Height + distance;
                    else
                        height += ItemHeight + distance;
                }

                return isHorizontal ? width : height;
            }
        }

        private (int, int) GetLineCountIndex(Dictionary<int, int> lineItems, int index)
        {
            bool isBreak = false;
            int count = 0;      //第几位
            int lineIndex = 0;  //第几行/列
            int countIndex = 0; //第lineIndex行/列第几个元素
            for (int i = 0; i < lineItems.Count; i++)
            {
                for (int j = 1; j <= lineItems.ElementAt(i).Value; j++)
                {
                    countIndex = j;
                    count++;

                    if (count == index + 1)
                    {
                        lineIndex = i + 1;
                        isBreak = true;
                        break;
                    }
                }
                if (isBreak) break;
            }

            return (lineIndex, countIndex);
        }

        private Point CalChildXLocation(int lineIndex, int countIndex, int count, double distanceX, double itemHSpace, double itemVSpace, double locationY, double maxDistance = 0)
        {
            double x = distanceX + itemHSpace;
            switch (UniformType)
            {
                case UniformType.Every:
                    for (int i = countIndex; i > 1; i--)
                    {
                        var itemWidth = !double.IsNaN(ItemWidth) ? ItemWidth : InternalChildren[count - i].DesiredSize.Width;
                        x += itemWidth + distanceX + itemHSpace;
                    }
                    break;
                case UniformType.Item:
                    for (int i = 0; i < countIndex; i++)
                    {
                        var itemWidth = !double.IsNaN(ItemWidth) ? ItemWidth : InternalChildren[count - (countIndex - i)].DesiredSize.Width;
                        x = maxDistance * i + (maxDistance - itemWidth) / 2;
                    }
                    break;
                case UniformType.OnlyItem:
                    x = 0;
                    for (int i = 1; i < countIndex; i++)
                    {
                        var itemWidth = !double.IsNaN(ItemWidth) ? ItemWidth : InternalChildren[count - i].DesiredSize.Width;
                        x += _dx + itemWidth;
                    }
                    break;
                default:
                    break;
            }

            var y = locationY + ((lineIndex - 1) * (double.IsNaN(ItemHeight) ? InternalChildren[count - 1].DesiredSize.Height + itemVSpace : ItemHeight + itemVSpace));
            var point = new Point(x, y);

            return point;
        }

        private Point CalChildYLocation(int lineIndex, int countIndex, int count, double distanceY, double itemHSpace, double itemVSpace, double locationX, double maxDistance = 0)
        {
            double y = distanceY + itemVSpace;
            switch (UniformType)
            {
                case UniformType.Every:
                    for (int i = countIndex; i > 1; i--)
                    {
                        var itemHeight = !double.IsNaN(ItemHeight) ? ItemHeight : InternalChildren[count - i].DesiredSize.Height;
                        y += (itemHeight + distanceY + itemVSpace);
                    }
                    break;
                case UniformType.Item:
                    for (int i = 0; i < countIndex; i++)
                    {
                        var itemHeight = !double.IsNaN(ItemHeight) ? ItemHeight : InternalChildren[count - (countIndex - i)].DesiredSize.Height;
                        y = maxDistance * i + (maxDistance - itemHeight) / 2;
                    }
                    break;
                case UniformType.OnlyItem:
                    y = 0;
                    for (int i = 1; i < countIndex; i++)
                    {
                        var itemHeight = !double.IsNaN(ItemHeight) ? ItemHeight : InternalChildren[count - i].DesiredSize.Height;
                        y += _dy + itemHeight;
                    }
                    break;
                default:
                    break;
            }

            var x = locationX + ((lineIndex - 1) * (double.IsNaN(ItemWidth) ? InternalChildren[count - 1].DesiredSize.Width + itemHSpace : ItemWidth + itemHSpace));
            var point = new Point(x, y);

            return point;
        }

        /// <summary>
        /// 定位子项
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns>
        /// 所有子项最终空间
        /// </returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (finalSize.IsZero())
                return default;

            var count = InternalChildren.Count;
            var location = new Point(); //元素定位的起始坐标

            var itemHSpace = double.IsNaN(ItemHVSpace.H) ? 0 : ItemHVSpace.H;
            var itemVSpace = double.IsNaN(ItemHVSpace.V) ? 0 : ItemHVSpace.V;

            for (int i = 0; i < count; i++)
            {
                var element = InternalChildren[i];

                var itemWidth = !double.IsNaN(ItemWidth) ? ItemWidth : element.DesiredSize.Width;
                var itemHeight = !double.IsNaN(ItemHeight) ? ItemHeight : element.DesiredSize.Height;
                var childSize = new Size(itemWidth, itemHeight);

                var distanceXY = CalDistanceXY(finalSize, itemWidth, itemHeight, itemHSpace, itemVSpace);
                var distanceX = distanceXY.Item1;
                var distanceY = distanceXY.Item2;

                if (Orientation == Orientation.Horizontal)
                {
                    var lineItems = GetLineItems(finalSize.Width, itemHSpace, distanceX, true);
                    var linecountIndex = GetLineCountIndex(lineItems, i);

                    //若未设置ItemHeight的值，则假定每个子项高度一致
                    //或者设置了ItemHeight的值
                    if (ItemVerticalAlignment == VerticalAlignment.Top)
                        location.Y = 0;
                    else if (ItemVerticalAlignment == VerticalAlignment.Bottom)
                        location.Y = finalSize.Height - itemVSpace - (itemHeight + itemVSpace) * lineItems.Count;
                    else
                        location.Y = (finalSize.Height - itemVSpace - (itemHeight + itemVSpace) * lineItems.Count) / 2D;

                    location.Y = Math.Max(0, location.Y + itemVSpace);
                    double maxDistanceX = (UniformType == UniformType.Item || UniformType == UniformType.OnlyItem) ? finalSize.Width / lineItems.ElementAt(linecountIndex.Item1 - 1).Value : 0;
                    location = CalChildXLocation(linecountIndex.Item1, linecountIndex.Item2, i + 1, distanceX, itemHSpace, itemVSpace, location.Y, maxDistanceX);
                }
                else
                {
                    var lineItems = GetLineItems(finalSize.Height, itemVSpace, distanceY, false);
                    var linecountIndex = GetLineCountIndex(lineItems, i);

                    //若未设置ItemWidth的值，则假定每个子项宽度一致
                    //或者设置了ItemWidth的值
                    if (ItemHorizontalAlignment == HorizontalAlignment.Left)
                        location.X = 0;
                    else if (ItemHorizontalAlignment == HorizontalAlignment.Right)
                        location.X = finalSize.Width - itemHSpace - (itemWidth + itemHSpace) * lineItems.Count;
                    else
                        location.X = (finalSize.Width - itemHSpace - (itemWidth + itemHSpace) * lineItems.Count) / 2D;

                    location.X = Math.Max(0, location.X + itemHSpace);
                    double maxDistanceY = (UniformType == UniformType.Item || UniformType == UniformType.OnlyItem) ? finalSize.Height / lineItems.ElementAt(linecountIndex.Item1 - 1).Value : 0;
                    location = CalChildYLocation(linecountIndex.Item1, linecountIndex.Item2, i + 1, distanceY, itemHSpace, itemVSpace, location.X, maxDistanceY);
                }

                element?.Arrange(new Rect(location, childSize));
            }

            return finalSize;
        }
    }
}
