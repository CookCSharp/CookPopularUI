/*
 *Description: WindowHelper
 *Author: Chance.zheng
 *Creat Time: 2023/8/9 18:21:27
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit.Windows.Win32Api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using Size = System.Windows.Size;

namespace CookPopularToolkit.Windows
{
    public sealed class WindowHelper
    {
        private static readonly BitArray _cacheValid = new BitArray((int)InteropValues.CacheSlot.NumSlots);


        private static Thickness? _paddedBorderThickness;
        /// <summary>
        /// returns the border thickness padding around captioned windows,in pixels. Windows XP/2000:  This value is not supported.
        /// </summary>
        public static Thickness PaddedBorderThickness
        {
            [SecurityCritical]
            get
            {
                if (_paddedBorderThickness == null)
                {
                    var s = SystemParameters.WindowNonClientFrameThickness;
                    var paddedBorder = InteropMethods.GetSystemMetrics(InteropValues.SM.CXPADDEDBORDER);
                    var dpi = DpiX;
                    Size frameSize = new Size(paddedBorder, paddedBorder);
                    Size frameSizeInDips = DpiHelper.DeviceSizeToLogical(frameSize, dpi / 96.0, dpi / 96.0);
                    _paddedBorderThickness = new Thickness(frameSizeInDips.Width, frameSizeInDips.Height, frameSizeInDips.Width, frameSizeInDips.Height);
                }

                return _paddedBorderThickness.Value;
            }
        }


        private static Thickness _windowResizeBorderThickness;
        public static Thickness WindowResizeBorderThickness
        {
            [SecurityCritical]
            get
            {
                lock (_cacheValid)
                {
                    while (!_cacheValid[(int)InteropValues.CacheSlot.WindowResizeBorderThickness])
                    {
                        _cacheValid[(int)InteropValues.CacheSlot.WindowResizeBorderThickness] = true;

                        var frameSize = new Size(InteropMethods.GetSystemMetrics(InteropValues.SM.CXSIZEFRAME), InteropMethods.GetSystemMetrics(InteropValues.SM.CYSIZEFRAME));
                        var frameSizeInDips = DpiHelper.DeviceSizeToLogical(frameSize, DpiHelper.DeviceDpiX / 96.0, DpiHelper.DeviceDpiY / 96.0);

                        _windowResizeBorderThickness = new Thickness(frameSizeInDips.Width, frameSizeInDips.Height, frameSizeInDips.Width, frameSizeInDips.Height);
                    }
                }

                return _windowResizeBorderThickness;
            }
        }


        public static Thickness WindowMaximizedPadding
        {
            get
            {
                InteropValues.APPBARDATA appBarData = default;
                var autoHide = InteropMethods.SHAppBarMessage(4, ref appBarData) != 0;
#if NET40
                return WindowResizeBorderThickness.Add(new Thickness(autoHide ? -8 : 0));
#elif NETCOREAPP
                var hdc = InteropMethods.GetDC(IntPtr.Zero);
                var scale = InteropMethods.GetDeviceCaps(hdc, InteropValues.DESKTOPVERTRES) / (float)InteropMethods.GetDeviceCaps(hdc, InteropValues.VERTRES);
                InteropMethods.ReleaseDC(IntPtr.Zero, hdc);
                return WindowResizeBorderThickness.Add(new Thickness((autoHide ? -4 : 4) * scale));
#else
                return WindowResizeBorderThickness.Add(new Thickness(autoHide ? -4 : 4));
#endif
            }
        }

        public static Thickness ChromeThickness
        {
            get
            {
                var w = (SystemParameters.MaximizedPrimaryScreenWidth - SystemParameters.WorkArea.Width) / 2;
                var h = (SystemParameters.MaximizedPrimaryScreenHeight - SystemParameters.WorkArea.Height) / 2;

                return new Thickness(w, h, w, h);
            }
        }


        /// <summary>
        /// Get DpiX
        /// </summary>
        public static double DpiX
        {
            get
            {
                var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
                var dpiX = (int)dpiXProperty.GetValue(null, null);
                return dpiX;
            }
        }

        /// <summary>
        /// Get Dpi
        /// </summary>
        public static double Dpi
        {
            get
            {
                var dpiProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);
                var dpi = (int)dpiProperty.GetValue(null, null);
                return dpi;
            }
        }

        public static bool IsWindowsXPOrLater
        {
            get
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    return Environment.OSVersion.Version >= new Version(5, 1, 2600);
                }

                return false;
            }
        }

        public static bool IsWindowsVistaOrLater
        {
            get
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    return Environment.OSVersion.Version >= new Version(6, 0, 6000);
                }

                return false;
            }
        }

        public static Window? GetActiveWindow() => System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

        public static Window? GetActiveWindowWithWin32Api()
        {
            var intPtr = InteropMethods.GetActiveWindow();
            if (intPtr == IntPtr.Zero)
                return null;

            var win = HwndSource.FromHwnd(intPtr).RootVisual as Window;

            return win;
        }
    }
}
