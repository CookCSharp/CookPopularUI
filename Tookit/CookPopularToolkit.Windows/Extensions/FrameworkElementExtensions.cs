/*
 *Description: FrameworkElementExtensions
 *Author: Chance.zheng
 *Creat Time: 2023/8/18 18:06:42
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

namespace CookPopularToolkit.Windows
{
    public static class FrameworkElementExtensions
    {
        /// <summary>
        /// Get the bounds of an element relative to another element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="otherElement">
        /// The element relative to the other element.
        /// </param>
        /// <returns>
        /// The bounds of the element relative to another element, or null if
        /// the elements are not related.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="otherElement"/> is null.
        /// </exception>
        public static Rect? GetBoundsRelativeTo(this FrameworkElement element, UIElement otherElement)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            else if (otherElement == null)
            {
                throw new ArgumentNullException("otherElement");
            }

            try
            {
                System.Windows.Point origin, bottom;
                GeneralTransform transform = element.TransformToVisual(otherElement);
                if (transform != null &&
                    transform.TryTransform(new System.Windows.Point(), out origin) &&
                    transform.TryTransform(new System.Windows.Point(element.ActualWidth, element.ActualHeight), out bottom))
                {
                    return new Rect(origin, bottom);
                }
            }
            catch (ArgumentException)
            {
                // Ignore any exceptions thrown while trying to transform
            }

            return null;
        }

        /// <summary>
        /// Perform an action when the element's LayoutUpdated event fires.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="action">The action to perform.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="action"/> is null.
        /// </exception>
        public static void InvokeOnLayoutUpdated(this UIElement element, Action action)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            else if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            // Create an event handler that unhooks itself before calling the
            // action and then attach it to the LayoutUpdated event.
            EventHandler? handler = null;
            handler = (s, e) =>
            {
                element.LayoutUpdated -= handler;
                action();
            };
            element.LayoutUpdated += handler;
        }

        /// <summary>
        /// Retrieves all the logical children of a framework element using a
        /// breadth-first search. For performance reasons this method manually
        /// manages the stack instead of using recursion.
        /// </summary>
        /// <param name="parent">The parent framework element.</param>
        /// <returns>The logical children of the framework element.</returns>
        public static IEnumerable<FrameworkElement> GetLogicalChildren(this FrameworkElement parent)
        {
            Debug.Assert(parent != null, "The parent cannot be null.");

            Popup? popup = parent as Popup;
            if (popup != null)
            {
                FrameworkElement? popupChild = popup.Child as FrameworkElement;
                if (popupChild != null)
                {
                    yield return popupChild;
                }
            }

            // If control is an items control return all children using the
            // Item container generator.
            ItemsControl? itemsControl = parent as ItemsControl;
            if (itemsControl != null)
            {
                foreach (FrameworkElement logicalChild in Enumerable.Range(0, itemsControl.Items.Count)
                                                                    .Select(index => itemsControl.ItemContainerGenerator.ContainerFromIndex(index))
                                                                    .OfType<FrameworkElement>())
                {
                    yield return logicalChild;
                }
            }

            string parentName = parent!.Name;
            Queue<FrameworkElement> queue = new Queue<FrameworkElement>(parent.GetVisualChildren().OfType<FrameworkElement>());

            while (queue.Count > 0)
            {
                FrameworkElement element = queue.Dequeue();
                if (element.Parent == parent || element is System.Windows.Controls.UserControl)
                {
                    yield return element;
                }
                else
                {
                    foreach (FrameworkElement visualChild in element.GetVisualChildren().OfType<FrameworkElement>())
                    {
                        queue.Enqueue(visualChild);
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves all the logical descendents of a framework element using a
        /// breadth-first search. For performance reasons this method manually
        /// manages the stack instead of using recursion.
        /// </summary>
        /// <param name="parent">The parent framework element.</param>
        /// <returns>The logical children of the framework element.</returns>
        public static IEnumerable<FrameworkElement> GetLogicalDescendents(this FrameworkElement parent)
        {
            Debug.Assert(parent != null, "The parent cannot be null.");

            return FunctionalProgramming.TraverseBreadthFirst(parent!, node => node.GetLogicalChildren(), node => true);
        }

        /// <summary>
        /// 将<see cref="FrameworkElement"/>保存为位图
        /// </summary>
        /// <param name="element">元素</param>
        /// <param name="fileName">文件路径及文件名</param>
        /// <param name="imageFormat">图片格式</param>
        /// <param name="size">保存的图片大小,以pixels为单位</param>
        public static void SaveAsPicture(this FrameworkElement element, string fileName, ImageFormat? imageFormat = default, System.Drawing.Size? size = null)
        {
            var dpiX = DpiHelper.DeviceDpiX;
            var dpiY = DpiHelper.DeviceDpiY;

            double elementWidth = 0;
            double elementHeight = 0;
            CheckElementSide(ref elementWidth, ref elementHeight);
            int width = (int)(elementWidth * DpiHelper.GetScaleX());
            int height = (int)(elementHeight * DpiHelper.GetScaleX());

            var bitmapSource = new RenderTargetBitmap(width, height, dpiX, dpiY, PixelFormats.Default);
            bitmapSource.Render(element);

            imageFormat ??= ImageFormat.Png;

            //下面这种方式生成文件很慢
            //using FileStream fs = File.Open(fileName, FileMode.OpenOrCreate);
            //BitmapEncoder encoder = null;
            //if (imageFormat == ImageFormat.Jpeg)
            //    encoder = new JpegBitmapEncoder();
            //else if (imageFormat == ImageFormat.Png)
            //    encoder = new PngBitmapEncoder();
            //else if (imageFormat == ImageFormat.Bmp)
            //    encoder = new BmpBitmapEncoder();
            //else if (imageFormat == ImageFormat.Gif)
            //    encoder = new GifBitmapEncoder();
            //else if (imageFormat == ImageFormat.Tiff)
            //    encoder = new TiffBitmapEncoder();
            //else
            //    throw new InvalidDataException();
            //encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            //encoder.Save(fs);

            //生成透明背景图片
            //using var bitmap = new Bitmap(fs);
            //bitmap.MakeTransparent();
            //bitmap.Save(fileName);

            //生成透明背景图片
            var pixels = new int[width * height];
            bitmapSource.CopyPixels(pixels, width * 4, 0);
            using (var bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb))
            {
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                        bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(pixels[y * width + x]));

                if (size.HasValue)
                {
                    using (var newBitmap = new Bitmap(bitmap, size.Value))
                    {
                        newBitmap.Save(fileName, imageFormat);
                    }
                }
                else
                    bitmap.Save(fileName, imageFormat);
            }

            void CheckElementSide(ref double elementWidth, ref double elementHeight)
            {
                if (!double.IsNaN(element.ActualWidth) && element.ActualWidth.CompareTo(0) > 0)
                    elementWidth = element.ActualWidth;
                else if (element.Width.CompareTo(0) > 0)
                    elementWidth = element.Width;
                else
                    elementWidth = 100;

                if (!double.IsNaN(element.ActualHeight) && element.ActualHeight.CompareTo(0) > 0)
                    elementHeight = element.ActualHeight;
                else if (element.Height.CompareTo(0) > 0)
                    elementHeight = element.Height;
                else
                    elementHeight = 100;
            }
        }
    }

    /// <summary>
    /// Collection of functions for functional programming tasks.
    /// </summary>
    public static class FunctionalProgramming
    {
        /// <summary>
        /// Traverses a tree by accepting an initial value and a function that
        /// retrieves the child nodes of a node.
        /// </summary>
        /// <typeparam name="T">The type of the stream.</typeparam>
        /// <param name="initialNode">The initial node.</param>
        /// <param name="getChildNodes">A function that retrieves the child
        /// nodes of a node.</param>
        /// <param name="traversePredicate">A predicate that evaluates a node
        /// and returns a value indicating whether that node and it's children
        /// should be traversed.</param>
        /// <returns>A stream of nodes.</returns>
        internal static IEnumerable<T> TraverseBreadthFirst<T>(T initialNode, Func<T, IEnumerable<T>> getChildNodes, Func<T, bool> traversePredicate)
        {
            Queue<T> queue = new Queue<T>();
            queue.Enqueue(initialNode);

            while (queue.Count > 0)
            {
                T node = queue.Dequeue();
                if (traversePredicate(node))
                {
                    yield return node;
                    IEnumerable<T> childNodes = getChildNodes(node);
                    foreach (T childNode in childNodes)
                    {
                        queue.Enqueue(childNode);
                    }
                }
            }
        }
    }
}
