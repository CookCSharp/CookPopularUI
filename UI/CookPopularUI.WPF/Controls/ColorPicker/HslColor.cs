/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：HslColor
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-09 15:48:02
 */


using CookPopularToolkit.Windows.Win32Api;
using System;
using System.Windows;
using System.Windows.Media;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 使用HSL(Hue, Saturation, Lightness )来描绘颜色而不是RGB (Red, Green, Blue)
    /// </summary>
    public class HslColor
    {
        /// <summary>
        /// 色度[0°,360°)
        /// </summary>
        public double Hue { get; protected set; }

        /// <summary>
        /// 饱和度[0,1]
        /// </summary>
        public double Saturation { get; protected set; }

        /// <summary>
        /// 亮度[0,1]
        /// </summary>
        public double Lightness { get; protected set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="hue"></param>
        /// <param name="saturation"></param>
        /// <param name="lightness"></param>
        public HslColor(double hue, double saturation, double lightness)
        {
            if (hue < 0)
                throw new ArgumentOutOfRangeException(string.Format("Hue: {0}", hue));

            if (hue > 360)
            {
                hue = ((int)hue) % 360;

                if (hue > 360)
                    hue = 360;
            }

            if (saturation < 0 || saturation > 1.0)
                throw new ArgumentOutOfRangeException(string.Format("Saturation: {0}", saturation));

            if (lightness < 0 || lightness > 1.0)
                throw new ArgumentOutOfRangeException(string.Format("Lightness: {0}", lightness));

            this.Hue = hue;
            this.Saturation = saturation;
            this.Lightness = lightness;
        }

        /// <summary>
        /// 将RGB转为HSL
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static HslColor RGBToHSL(Color? color)
        {
            if (color is null)
                return new HslColor(0, 0, 0);

            System.Drawing.Color convColor = System.Drawing.Color.FromArgb(color.Value.A, color.Value.R, color.Value.G, color.Value.B);

            int max = Math.Max(convColor.R, Math.Max(convColor.G, convColor.B));
            int min = Math.Min(convColor.R, Math.Min(convColor.G, convColor.B));

            double hue = convColor.GetHue();
            if (hue > 360)
                hue = 360;

            double saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            double value = max / 255d;

            return new HslColor(hue, saturation, value);
        }

        public static Color ToColor(HslColor hsl)
        {
            var h = hsl.Hue * (1.0 / 360);
            var s = hsl.Saturation * (1.0 / 100);
            var l = hsl.Lightness * (1.0 / 100);

            double r, g, b;
            if (s == 0)
            {
                r = l * 255;
                g = l * 255;
                b = l * 255;
            }
            else
            {
                double var_2;
                if (l < 0.5) var_2 = l * (1 + s);
                else var_2 = (l + s) - (s * l);

                var var_1 = 2 * l - var_2;

                r = 255 * hsv_rbg(var_1, var_2, h + (1.0 / 3));
                g = 255 * hsv_rbg(var_1, var_2, h);
                b = 255 * hsv_rbg(var_1, var_2, h - (1.0 / 3));
            }

            double hsv_rbg(double v1, double v2, double vH)
            {
                if (vH < 0) vH += 1;
                if (vH > 1) vH -= 1;
                if ((6 * vH) < 1) return (v1 + (v2 - v1) * 6 * vH);
                if ((2 * vH) < 1) return (v2);
                if ((3 * vH) < 2) return (v1 + (v2 - v1) * ((2.0 / 3) - vH) * 6);
                return (v1);
            }

            return Color.FromRgb((byte)Math.Round(r), (byte)Math.Round(g), (byte)Math.Round(b));
        }

        /// <summary>
        /// 将HSL转为RGB
        /// </summary>
        /// <param name="hslColor"></param>
        /// <returns></returns>
        public static Color HSLToRGB(HslColor hslColor)
        {
            int hi = Convert.ToInt32(Math.Floor(hslColor.Hue / 60)) % 6;
            double f = hslColor.Hue / 60 - Math.Floor(hslColor.Hue / 60);

            double value = hslColor.Lightness * 255;
            byte v = Convert.ToByte(value);
            byte p = Convert.ToByte(value * (1 - hslColor.Saturation));
            byte q = Convert.ToByte(value * (1 - f * hslColor.Saturation));
            byte t = Convert.ToByte(value * (1 - (1 - f) * hslColor.Saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        /// <summary>
        /// 获取鼠标点所在位置的颜色
        /// </summary>
        /// <returns></returns>
        public static Color GetPixelColor()
        {
            InteropMethods.GetCursorPos(out var mousePoint);
            IntPtr hdc = InteropMethods.GetDC(IntPtr.Zero);
            uint pixel = InteropMethods.GetPixel(hdc, mousePoint.X, mousePoint.Y);
            InteropMethods.ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromRgb((byte)(pixel & 0x000000FF), (byte)((pixel & 0x0000FF00) >> 8), (byte)((pixel & 0x00FF0000) >> 16));
            return color;
        }
    }
}
