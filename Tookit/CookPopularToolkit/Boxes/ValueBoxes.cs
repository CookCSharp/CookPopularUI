/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：$Do something$ 
 * Author： Chance_写代码的厨子
 * Create Time：2021-02-19 16:05:08
 */


using System.Windows;

namespace CookPopularToolkit
{
    /// <summary>
    /// 装箱后的值类型（用于提高效率）
    /// </summary>
    public static class ValueBoxes
    {
        public static readonly object TrueBox = true;
        public static readonly object FalseBox = false;
        public static readonly object? NullBox = null;

        public static object BooleanBox(this bool value) => value ? TrueBox : FalseBox;

        public static object? BooleanNullBox(this bool? value)
        {
            if (value.HasValue)
            {
                return value.Value ? TrueBox : FalseBox;
            }
            else
            {
                return NullBox;
            }
        }

        public static readonly object IntegerMinus1Box = -1;
        public static readonly object Integer0Box = 0;
        public static readonly object Integer5Box = 5;
        public static readonly object Integer10Box = 10;
        public static readonly object Integer15Box = 15;
        public static readonly object Integer30Box = 30;

        public static readonly object Double0Box = 0.0;
        public static readonly object Double1Box = 1.0;
        public static readonly object Double3Box = 3.0;
        public static readonly object Double5Box = 5.0;
        public static readonly object Double10Box = 10.0;
        public static readonly object Double20Box = 20.0;
        public static readonly object Double30Box = 30.0;
        public static readonly object Double50Box = 50.0;
        public static readonly object Double200Box = 200.0;
    }
}
