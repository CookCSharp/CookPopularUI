/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DrawingPropertyMetadataOptions
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:29:45
 */


using System;

namespace CookPopularUI.WPF.Controls
{
    [Flags]
    public enum DrawingPropertyMetadataOptions
    {
        None = 0,

        AffectsMeasure = 1,

        AffectsRender = 16
    }
}
