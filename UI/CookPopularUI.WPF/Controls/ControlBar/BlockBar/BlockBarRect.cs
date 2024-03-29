﻿/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：BlockBarRec
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-06 15:06:09
 */


using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 表示矩形块
    /// </summary>
    public class BlockBarRect : BlockBarBase
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            Contract.Requires(!RenderSize.IsEmpty, "RenderSize");
            Contract.Requires(BlockCount > 0, "BlockCount");
            
            double width = (RenderSize.Width - (BlockCount - 1) * BlockDistance - BorderBen.Thickness) / BlockCount;
            double height = RenderSize.Height - BorderBen.Thickness;
            if (width <= 0 || height <= 0) return;

            for (int i = 0; i < BlockCount; i++)
            {
                Contract.Requires(BlockCount > i, "BlockNumber");

                double left = BorderBen.Thickness / 2 + (width + BlockDistance) * i;
                var rect = new Rect(left, BorderBen.Thickness / 2, width, height);
                if (!rect.IsEmpty)
                {
                    int threshold = GetThreshold(Value, BlockCount);
                    drawingContext.DrawRectangle((i < threshold) ? Foreground : Background, BorderBen, rect);
                }
            }
        }
    }
}
