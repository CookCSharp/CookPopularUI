/*
 *Description: RectangleExtensions
 *Author: Chance.zheng
 *Creat Time: 2023/8/11 16:23:08
 *.Net Version: 2.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CookPopularToolkit
{
    public static class RectangleExtensions
    {
        public static Point Center(this Rectangle self) => new Point(self.Width / 2 + self.Left, self.Height / 2 + self.Top);

        public static Point North(this Rectangle self, int by = 0) => new Point(self.Center().X, self.Top + by);

        public static Point East(this Rectangle self, int by = 0) => new Point(self.Right + by, self.Center().Y);

        public static Point South(this Rectangle self, int by = 0) => new Point(self.Center().X, self.Bottom + by);

        public static Point West(this Rectangle self, int by = 0) => new Point(self.Left + by, self.Center().Y);

        public static Point ImmediateExteriorNorth(this Rectangle self) => self.North(-1);

        public static Point ImmediateInteriorNorth(this Rectangle self) => self.North(1);

        public static Point ImmediateExteriorEast(this Rectangle self) => self.East(1);

        public static Point ImmediateInteriorEast(this Rectangle self) => self.East(-1);

        public static Point ImmediateExteriorSouth(this Rectangle self) => self.South(1);

        public static Point ImmediateInteriorSouth(this Rectangle self) => self.South(-1);

        public static Point ImmediateExteriorWest(this Rectangle self) => self.West(-1);

        public static Point ImmediateInteriorWest(this Rectangle self) => self.West(1);

        /// <summary>
        /// Makes the rectangles dimensions a multiple of 2.
        /// </summary>
        public static Rectangle Even(this Rectangle self)
        {
            if (self.Width % 2 == 1)
            {
                --self.Width;
            }
            if (self.Height % 2 == 1)
            {
                --self.Height;
            }
            return self;
        }
    }
}
