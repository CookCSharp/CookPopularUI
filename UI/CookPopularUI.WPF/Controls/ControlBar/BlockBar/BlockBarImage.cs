/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：BlockBarImage
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-09 15:04:29
 */


using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Media;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 图片块
    /// </summary>
    public class BlockBarImage : BlockBarBase
    {
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="ImageSource"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(nameof(ImageSource), typeof(ImageSource), typeof(BlockBarImage), new PropertyMetadata(default(ImageSource)));


        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            Contract.Requires(!RenderSize.IsEmpty, "RenderSize");
            Contract.Requires(BlockCount > 0, "BlockCount");

            double width = (RenderSize.Width - (BlockCount - 1) * BlockDistance - BorderBen.Thickness) / BlockCount;
            double height = RenderSize.Height - BorderBen.Thickness;
            if (width <= 0 || height <= 0) return;

            for (int i = 0; i < Value; i++)
            {
                Contract.Requires(BlockCount > i, "BlockNumber");

                double left = BorderBen.Thickness / 2 + (width + BlockDistance) * i;
                var imageRect = new Rect(left, 0, width, height);
                drawingContext.DrawImage(ImageSource, imageRect);
            }
        }
    }
}
