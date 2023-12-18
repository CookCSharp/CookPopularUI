/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：BlockBarBase
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-06 14:37:47
 */


using CookPopularToolkit;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 表示块状的基类
    /// </summary>
    public abstract class BlockBarBase : FrameworkElement
    {
        private static readonly Brush BlockBarForegroundBrush = CookPopularToolkit.Windows.ResourceHelper.GetResource<Brush>("PrimaryThemeBrush")!;
        protected static readonly Brush BlockBarBackgroundBrush = CookPopularToolkit.Windows.ResourceHelper.GetResource<Brush>("UnEnabledBrush")!;

        protected Pen BorderBen { get; private set; }


        /// <summary>
        /// 块状数量
        /// </summary>
        public int BlockCount
        {
            get { return (int)GetValue(BlockCountProperty); }
            set { SetValue(BlockCountProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="BlockCount"/>依赖属性
        /// </summary>
        public static readonly DependencyProperty BlockCountProperty =
            DependencyProperty.Register(nameof(BlockCount), typeof(int), typeof(BlockBarBase),
                new FrameworkPropertyMetadata(ValueBoxes.Integer5Box, FrameworkPropertyMetadataOptions.AffectsRender, null, new CoerceValueCallback(CoerceBlockCount)));

        private static object CoerceBlockCount(DependencyObject d, object baseValue)
        {
            int input = (int)baseValue;

            if (input < 1)
            {
                return 1;
            }
            else
            {
                return input;
            }
        }


        /// <summary>
        /// 当前值
        /// </summary>
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="Value"/>依赖属性
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(int), typeof(BlockBarBase),
                new FrameworkPropertyMetadata(ValueBoxes.Integer0Box, FrameworkPropertyMetadataOptions.AffectsRender, null, new CoerceValueCallback(CoerceValue)));

        private static object CoerceValue(DependencyObject d, object baseValue)
        {
            int input = (int)baseValue;
            if (input < 0)
            {
                return 0;
            }
            else
            {
                return input;
            }
        }

        /// <summary>
        /// 相邻块状之间的间距
        /// </summary>
        public double BlockDistance
        {
            get { return (double)GetValue(BlockDistanceProperty); }
            set { SetValue(BlockDistanceProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="BlockDistance"/>依赖属性
        /// </summary>
        public static readonly DependencyProperty BlockDistanceProperty =
            DependencyProperty.Register(nameof(BlockDistance), typeof(double), typeof(BlockBarBase),
                new FrameworkPropertyMetadata((double)0, FrameworkPropertyMetadataOptions.AffectsRender, null, new CoerceValueCallback(CoerceBlockDistance)));

        private static object CoerceBlockDistance(DependencyObject d, object baseValue)
        {
            double input = (double)baseValue;
            if (input < 0 || double.IsNaN(input) || double.IsInfinity(input))
            {
                return 0;
            }
            else
            {
                return input;
            }
        }


        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }
        public static readonly DependencyProperty ForegroundProperty = Control.ForegroundProperty.AddOwner(typeof(BlockBarBase), new FrameworkPropertyMetadata(BlockBarForegroundBrush, FrameworkPropertyMetadataOptions.AffectsRender));


        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }
        public static readonly DependencyProperty BackgroundProperty = Control.BackgroundProperty.AddOwner(typeof(BlockBarBase), new FrameworkPropertyMetadata(BlockBarBackgroundBrush, FrameworkPropertyMetadataOptions.AffectsRender));


        static BlockBarBase()
        {
            MinHeightProperty.OverrideMetadata(typeof(BlockBarBase), new FrameworkPropertyMetadata((double)10));
            MinWidthProperty.OverrideMetadata(typeof(BlockBarBase), new FrameworkPropertyMetadata((double)10));
            ClipToBoundsProperty.OverrideMetadata(typeof(BlockBarBase), new FrameworkPropertyMetadata(true));
        }

        public BlockBarBase()
        {
            UpdateBorderPen();
        }

        private void UpdateBorderPen()
        {
            BorderBen = new Pen(BlockBarBackgroundBrush, 0);
            BorderBen.Freeze();
        }

        protected virtual int GetThreshold(int value, int blockCount)
        {
            int blockNumber = Math.Min(value, blockCount);

            return blockNumber;
        }
    }
}
