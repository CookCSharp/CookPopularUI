/*
 *Description: StringArray
 *Author: Chance.zheng
 *Creat Time: 2024/1/23 13:56:08
 *.Net Version: 2.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularToolkit
{
    public class StringToArraryConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object source)
        {
            if (source != null && source is string value)
            {
                return new StringArray(value.Split(','));
            }

            return base.ConvertFrom(context, culture, source);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                var v = (StringArray)value;
                return string.Join(",", v.Array);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
