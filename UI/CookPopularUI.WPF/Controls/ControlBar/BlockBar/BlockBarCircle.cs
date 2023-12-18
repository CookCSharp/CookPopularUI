/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：BlockBarCircle
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-06 14:47:58
 */


using System.Windows;
using System.Windows.Media;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 表示圆形块
    /// </summary>
    public class BlockBarCircle : BlockBarBase
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            Size effectiveRenderSize = new Size(this.RenderSize.Width - BorderBen.Thickness, this.RenderSize.Height - BorderBen.Thickness);

            double circleDiameter = (effectiveRenderSize.Width - (BlockCount - 1) * BlockDistance) / BlockCount;
            if (circleDiameter > effectiveRenderSize.Height)
            {
                circleDiameter = effectiveRenderSize.Height;
            }

            double circleRadius = circleDiameter / 2;
            Point center = new Point();

            int threshHold = GetThreshold(this.Value, this.BlockCount);

            for (int i = 0; i < this.BlockCount; i++)
            {
                double startLeft = BorderBen.Thickness / 2 + (circleDiameter + BlockDistance) * i;
                double startTop = BorderBen.Thickness / 2 + (effectiveRenderSize.Height - circleDiameter) / 2;
                center.X = startLeft + circleRadius;
                center.Y = startTop + circleRadius;

                drawingContext.DrawEllipse((i < threshHold) ? Foreground : Background, BorderBen, center, circleRadius, circleRadius);
            }
        }
    }
}
