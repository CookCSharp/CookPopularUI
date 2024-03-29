﻿/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ImageBitmapExtension
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-30 20:21:36
 */


using CookPopularToolkit.Windows.Win32Api;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Resources;

namespace CookPopularToolkit.Windows
{
    /// <summary>
    /// 提供<see cref="System.Drawing.Image"/>与<see cref="Bitmap"/>一些扩展方法
    /// </summary>
    /// <remarks>
    /// WPF渲染框架并没有对外提供多少可以完全控制渲染的部分，目前可以做的有：
    /// D3DImage，用来承载使用 DirectX 各个版本渲染内容的控件
    /// WriteableBitmap，通过一段内存空间来指定如何渲染一个位图的图片
    /// HwndHost，通过承载一个子窗口以便能叠加任何种类渲染的控件
    /// </remarks>
    public static class ImageBitmapExtensions
    {
        /// <summary>
        /// Bitmap to ImageSource
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static ImageSource ToImageSource(this Bitmap bitmap)
        {
            IntPtr intPtr = bitmap.GetHbitmap();
            try
            {
                ImageSource imageSource = Imaging.CreateBitmapSourceFromHBitmap(intPtr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                return imageSource;
            }
            finally
            {
                InteropMethods.DeleteObject(intPtr);
            }
        }

        public static ImageSource ToImageSource(this Uri imageUri) => new BitmapImage(imageUri);

        public static ImageSource ToImageSource(this Icon icon)
        {
            return Imaging.CreateBitmapSourceFromHIcon(icon.Handle,
                new Int32Rect(0, 0, icon.Width, icon.Height),
                BitmapSizeOptions.FromEmptyOptions());
        }

        public static ImageSource? GetImageSource(this string imgFilePath)
        {
            BitmapFrame? imgSource = null;
            using (FileStream fs = new FileStream(imgFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                ImageSourceConverter isc = new ImageSourceConverter();
                byte[] arr = new byte[fs.Length];
                fs.Read(arr, 0, (int)fs.Length);
                MemoryStream stream = new MemoryStream(arr);
                imgSource = isc.ConvertFrom(stream) as BitmapFrame;
            }
            return imgSource;
        }

        /// <summary>
        /// 获取文件(夹)的关联图标
        /// </summary>
        /// <param name="isLargeIcon">是否返回大图标</param>
        /// <param name="fileName">文件名，为空返回文件夹关联图标，否则返回对应文件关联图标</param>
        /// <returns>获取到的图标</returns>
        public static ImageSource? GetImageSource(bool isLargeIcon, string fileName = "")
        {
            Icon? icon = GetIcon(isLargeIcon, fileName);
            if (icon != null)
                return Imaging.CreateBitmapSourceFromHIcon(icon!.Handle, new Int32Rect(0, 0, icon.Width, icon.Height), BitmapSizeOptions.FromEmptyOptions());
            else
                return default;
        }

        public static ImageSource GetImageSourceFromLnk(this string s)
        {
            return ToImageSource(GetLnkIcon(s));
        }

        /// <summary>
        /// Bitmap 转 BitmapImage
        /// </summary>
        /// <param name="bitmap">Bitmap 对象</param>
        /// <returns>转换后的 BitmapImage对象</returns>
        public static BitmapImage? ToBitmapImage(this Bitmap bitmap)
        {
            try
            {
                BitmapImage relust = new BitmapImage();
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    MemoryStream memory = new MemoryStream();
                    bitmap.Save(memoryStream, ImageFormat.Png);
                    memoryStream.Position = 0;
                    relust.BeginInit();
                    relust.CacheOption = BitmapCacheOption.OnLoad;
                    memoryStream.CopyTo(memory);
                    relust.StreamSource = memory;
                    relust.EndInit();
                    relust.Freeze();

                    return relust;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// ImageSource 转为Bitmap
        /// </summary>
        /// <param name="imageSource">imageSource 对象</param>
        /// <returns>返回 Bitmap 对象</returns>
        public static Bitmap? ToBitmap(this ImageSource imageSource)
        {
            try
            {
                BitmapSource bitmapSource = (BitmapSource)imageSource;
                Bitmap bitmap = new Bitmap(bitmapSource.PixelWidth, bitmapSource.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                BitmapData data = bitmap.LockBits(new Rectangle(System.Drawing.Point.Empty, bitmap.Size),
                                                  ImageLockMode.WriteOnly,
                                                  System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                bitmapSource.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
                bitmap.UnlockBits(data);

                return bitmap;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// <see cref="BitmapSource"/> To <see cref="Bitmap"/>
        /// </summary>
        /// <param name="bitmapSource"></param>
        /// <returns></returns>
        public static Bitmap? ToBitmap(this BitmapSource bitmapSource)
        {
            if (bitmapSource == null)
                return null;

            using (MemoryStream ms = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(ms);

                return new Bitmap(ms);
            }
        }

        public static Icon ToIcon(this Bitmap bitmap, System.Drawing.Size? size)
        {
            using (Bitmap iconBm = size.HasValue ? new Bitmap(bitmap, size.Value) : new Bitmap(bitmap))
            {
                using (Icon icon = Icon.FromHandle(iconBm.GetHicon()))
                {
                    return icon;
                }
            }
        }

        /// <summary>
        /// 转换Image为Icon
        /// </summary>
        /// <param name="image">要转换为图标的Image对象</param>
        /// <param name="nullTonull">当image为null时是否返回null。false则抛空引用异常</param>
        /// <exception cref="ArgumentNullException" />
        public static Icon? ToIcon(this Image image, bool nullTonull = false)
        {
            if (image == null)
            {
                if (nullTonull) return null;
                throw new ArgumentNullException("image");
            }

            using (MemoryStream msImg = new MemoryStream(), msIco = new MemoryStream())
            {
                image.Save(msImg, ImageFormat.Png);

                using (var bin = new BinaryWriter(msIco))
                {
                    //写图标头部
                    bin.Write((short)0);      //0-1保留
                    bin.Write((short)1);      //2-3文件类型。1=图标, 2=光标
                    bin.Write((short)1);      //4-5图像数量（图标可以包含多个图像）

                    bin.Write((byte)image.Width); //6图标宽度
                    bin.Write((byte)image.Height); //7图标高度
                    bin.Write((byte)0);      //8颜色数（若像素位深>=8，填0。这是显然的，达到8bpp的颜色数最少是256，byte不够表示）
                    bin.Write((byte)0);      //9保留。必须为0
                    bin.Write((short)0);      //10-11调色板
                    bin.Write((short)32);     //12-13位深
                    bin.Write((int)msImg.Length); //14-17位图数据大小
                    bin.Write(22);         //18-21位图数据起始字节

                    //写图像数据
                    bin.Write(msImg.ToArray());

                    bin.Flush();
                    bin.Seek(0, SeekOrigin.Begin);
                    return new Icon(msIco);
                }
            }
        }

        public static Icon? ToIcon(this ImageSource imageSource)
        {
            if (imageSource == null) return null;

            Uri uri = new Uri(imageSource.ToString());
            StreamResourceInfo streamInfo = System.Windows.Application.GetResourceStream(uri);

            if (streamInfo == null)
            {
                string msg = "The supplied image source '{0}' could not be resolved.";
                msg = string.Format(msg, imageSource);
                throw new ArgumentException(msg);
            }

            return new Icon(streamInfo.Stream);
        }

        /// <summary>
        /// 获取文件(夹)关联图标
        /// </summary>
        /// <param name="pszPath">文件(夹)名称</param>
        /// <param name="uFlags"></param>
        /// <returns>图标</returns>
        private static Icon? GetIcon(this string pszPath, uint uFlags)
        {
            InteropValues.SHFILEINFO shfi = new InteropValues.SHFILEINFO();
            IntPtr hI = InteropMethods.SHGetFileInfo(pszPath, 0, ref shfi, (uint)Marshal.SizeOf(shfi), uFlags);
            if (hI.Equals(IntPtr.Zero))
            {
                return null;
            }
            Icon? icon = Icon.FromHandle(shfi.hIcon).Clone() as Icon;
            InteropMethods.DestroyIcon(shfi.hIcon);

            return icon;
        }

        /// <summary>
        /// 获取文件(夹)的关联图标
        /// </summary>
        /// <param name="isLargeIcon">是否返回大图标</param>
        /// <param name="fileName">文件名，为空返回文件夹关联图标，否则返回对应文件关联图标</param>
        /// <returns>获取到的图标</returns>
        public static Icon? GetIcon(bool isLargeIcon, string fileName = "")
        {
            uint uFlags = (uint)InteropValues.FileInfoFlags.SHGFI_ICON;
            if (isLargeIcon)
            {
                uFlags |= (uint)InteropValues.FileInfoFlags.SHGFI_LARGEICON;
            }
            else
            {
                uFlags |= (uint)InteropValues.FileInfoFlags.SHGFI_SMALLICON;
            }
            if (string.IsNullOrWhiteSpace(fileName) == false)
            {
                uFlags |= (uint)InteropValues.FileInfoFlags.SHGFI_USEFILEATTRIBUTES;
            }

            return GetIcon(fileName, uFlags);
        }

        public static Icon GetLnkIcon(this string s)//s是要获取文件路径，返回ico格式文件
        {
            int RefInt = 0;
            var thisHandle = new IntPtr(InteropMethods.ExtractAssociatedIconA(0, s, ref RefInt));
            return Icon.FromHandle(thisHandle);
        }

        public static void SaveAsIconFile(this Bitmap bitmap, string saveFilePath, System.Drawing.Size? size)
        {
            using (Icon? icon = size.HasValue ? bitmap.ToIcon(size) : bitmap.ToIcon())
            {
                using (Stream stream = new FileStream(saveFilePath, FileMode.Create))
                {
                    icon?.Save(stream);
                }
            }
        }

        public static byte[] ToBytes(this Bitmap img, int width, int height, int channel)
        {
            byte[] bytes = new byte[width * height * channel];

            BitmapData im = img.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, img.PixelFormat);
            int stride = im.Stride;
            int offset = stride - width * channel;
            int length = stride * height;
            byte[] temp = new byte[stride * height];
            Marshal.Copy(im.Scan0, temp, 0, temp.Length);
            img.UnlockBits(im);

            int posreal = 0;
            int posscan = 0;
            for (int c = 0; c < height; c++)
            {
                for (int d = 0; d < width * channel; d++)
                {
                    bytes[posreal++] = temp[posscan++];
                }
                posscan += offset;
            }

            return bytes;
        }

        /// <summary>
        /// BitmapImage 转为byte[]
        /// </summary>
        /// <param name="bitmapImage">BitmapImage 对象</param>
        /// <returns>byte[] 数组</returns>
        public static byte[] ToBytes(this BitmapImage bitmapImage)
        {
            byte[] buffer = new byte[] { };
            try
            {
                Stream stream = bitmapImage.StreamSource;
                if (stream != null && stream.Length > 0)
                {
                    stream.Position = 0;
                    using (BinaryReader binary = new BinaryReader(stream))
                    {
                        buffer = binary.ReadBytes((int)stream.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return buffer;
        }

        /// <summary>
        /// 图片压缩
        /// </summary>
        /// <param name="bitmap">要压缩的源图像</param>
        /// <param name="height">要求的高</param>
        /// <param name="width">要求的宽</param>
        /// <returns></returns>
        public static Bitmap Compress(this Bitmap bitmap, int height, int width)
        {
            try
            {
                lock (bitmap)
                {
                    Bitmap iSource = bitmap;
                    ImageFormat imageFormat = iSource.RawFormat;
                    int sw = width, sh = height;
                    //按比例缩放
                    Bitmap ob = new Bitmap(width, height);
                    Graphics graphics = Graphics.FromImage(ob);
                    graphics.Clear(System.Drawing.Color.WhiteSmoke);
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(iSource, new System.Drawing.Rectangle((width - sw) / 2, (height - sh) / 2, sw, sh), 0, 0, iSource.Width, iSource.Height, System.Drawing.GraphicsUnit.Pixel);
                    graphics.Dispose();
                    return ob;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return bitmap;
        }

        public static WriteableBitmap WriteTo(this Array data, int pixelWidth, int pixelHeight)
        {
            //数据源长度要满足：pixelWidth * pixelHeight * 4  
            byte[] dest = new byte[pixelWidth * pixelHeight * 4];
            Buffer.BlockCopy(data, 0, dest, 0, data.Length);

            var writeableBitmap = new WriteableBitmap(pixelWidth, pixelHeight, 96.0, 96.0, PixelFormats.Bgra32, BitmapPalettes.Halftone256Transparent);

            writeableBitmap.Lock();
            //stride一般就是宽度*4，因为一个像素一般是4个byte
            writeableBitmap.WritePixels(new Int32Rect(0, 0, pixelWidth, pixelHeight), dest, pixelWidth * 4, 0);
            writeableBitmap.Unlock();

            return writeableBitmap;
        }

        public static WriteableBitmap? WriteToByUnsafe(this Array data, int pixelWidth, int pixelHeight)
        {
            byte[] dest = new byte[pixelWidth * pixelHeight * 4];
            Buffer.BlockCopy(data, 0, dest, 0, data.Length);

            unsafe
            {
                fixed (byte* ptr = dest) //数据源长度要满足：pixelWidth * pixelHeight * 4
                {
                    try
                    {
                        var writeableBitmap = new WriteableBitmap(pixelWidth, pixelHeight, 96.0, 96.0, PixelFormats.Bgra32, BitmapPalettes.Halftone256Transparent);

                        //using Bitmap bitmap = new Bitmap(pixelWidth, pixelHeight, pixelWidth * 4, System.Drawing.Imaging.PixelFormat.Format16bppArgb1555, new IntPtr(ptr));
                        //bitmap.CopyTo(writeableBitmap);

                        writeableBitmap.Lock();

                        //如果数据源是一个一次申请不断修改的数组
                        var bytes = pixelWidth * pixelHeight * writeableBitmap.Format.BitsPerPixel / 8;
                        unsafe
                        {
                            //性能依次排序
                            InteropMethods.MoveMemory(writeableBitmap.BackBuffer, new IntPtr(ptr), (uint)bytes);
                            //InteropMethods.CopyMemory(writeableBitmap.BackBuffer, new IntPtr(ptr), (uint)bytes);
                            //Buffer.MemoryCopy(ptr, writeableBitmap.BackBuffer.ToPointer(), bytes, bytes);
                        }

                        writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, pixelWidth, pixelHeight));
                        writeableBitmap.Unlock();

                        return writeableBitmap;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// 使用不安全代码将Bitmap位图转为WPF的ImageSource以获得高性能和持续小的内存占用
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="writeableBitmap"></param>
        /// <remarks>
        /// WPF官方提供了一种方法，使用 System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap()方法。
        /// 官方解释称这是托管和非托管位图相互转换所用的方法。
        /// 然而此方法有一个很严重的弊端——每次都会生成全新的位图，即便每次 DeleteObject 之后，内存依然不会即时释放。
        /// </remarks>
        /// <![CDATA[
        /// [DllImport("gdi32")]
        /// static extern int DeleteObject(IntPtr o);
        /// ]]>
        /// 见 https://blog.walterlv.com/post/convert-bitmap-to-imagesource-using-unsafe-method.html
        public static void CopyTo(this Bitmap bitmap, WriteableBitmap writeableBitmap)
        {
            CheckHelper.ArgumentNullException(bitmap, nameof(bitmap));
            CheckHelper.ArgumentNullException(writeableBitmap, nameof(writeableBitmap));

            var bWidth = bitmap.Width;
            var bHeight = bitmap.Height;
            var wbWidth = writeableBitmap.PixelWidth;
            var wbHeight = writeableBitmap.PixelHeight;

            CheckHelper.ArgumentException(wbWidth != bWidth || wbHeight != bHeight, $"{nameof(writeableBitmap)} with {nameof(bitmap)} has not equal size");

            var width = wbWidth;
            var height = wbHeight;
            var bytes = wbWidth * wbHeight * writeableBitmap.Format.BitsPerPixel / 8;

            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            writeableBitmap.Lock();

            //性能依次排序 
            InteropMethods.MoveMemory(writeableBitmap.BackBuffer, bitmapData.Scan0, (uint)bytes);
            //InteropMethods.CopyMemory(writeableBitmap.BackBuffer, bitmapData.Scan0, (uint)bytes);
            //unsafe
            //{
            //    Buffer.MemoryCopy(bitmapData.Scan0.ToPointer(), writeableBitmap.BackBuffer.ToPointer(), bytes, bytes);
            //}

            //如果WriteableBitmap不渲染，那么无论设置多大的脏区都不会对性能有任何影响
            //脏区渲染是CPU占用的最大瓶颈（因为没有脏区仅剩内存拷贝的时候CPU占用为0%）
            writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            writeableBitmap.Unlock();

            bitmap.UnlockBits(bitmapData);
        }
    }
}
