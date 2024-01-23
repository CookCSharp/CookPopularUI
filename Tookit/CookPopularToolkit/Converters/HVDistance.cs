/*
 *Description: HVDistance
 *Author: Chance.zheng
 *Creat Time: 2023/8/25 23:03:08
 *.Net Version: 2.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CookPopularToolkit
{
    //[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
    [TypeConverter(typeof(HVDistanceConverter))]
    public struct HVDistance : IEquatable<HVDistance>
    {
        private double _h;
        private double _v;


        public double H
        {
            get { return _h; }
            set { _h = value; }
        }

        public double V
        {
            get { return _v; }
            set { _v = value; }
        }

        public HVDistance(double hv)
        {
            _h = _v = hv;
        }

        public HVDistance(double h, double v)
        {
            _h = h;
            _v = v;
        }

        /// <summary>
        /// This function compares to the provided object for type and value equality.
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>True if object is a HVDistance and all sides of it are equal to this HVDistance'.</returns>
        public override bool Equals(object obj)
        {
            if (obj is HVDistance)
            {
                HVDistance otherObj = (HVDistance)obj;
                return (this == otherObj);
            }
            return false;
        }

        /// <summary>
        /// Compares this instance of HVDistance with another instance.
        /// </summary>
        /// <param name="hvDistance">HVDistance instance to compare.</param>
        /// <returns> <c>true</c> if this HVDistance instance has the same value 
        /// and unit type as HVDistance.</returns>
        public bool Equals(HVDistance hvDistance)
        {
            return (this == hvDistance);
        }

        /// <summary>
        /// This function returns a hash code.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return _h.GetHashCode() ^ _v.GetHashCode();
        }


        public static bool operator ==(HVDistance hv1, HVDistance hv2)
        {
            return ((hv1._h == hv2._h || (double.IsNaN(hv1._h) && double.IsNaN(hv2._h)))
                 && (hv1._v == hv2._v || (double.IsNaN(hv1._v) && double.IsNaN(hv2._v)))
                   );
        }

        public static bool operator !=(HVDistance hv1, HVDistance hv2)
        {
            return (!(hv1 == hv2));
        }
    }
}
