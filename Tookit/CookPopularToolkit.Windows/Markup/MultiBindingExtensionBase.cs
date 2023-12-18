/*
 *Description: MultiBindingExtensionBase
 *Author: Chance.zheng
 *Creat Time: 2023/11/22 14:03:23
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CookPopularToolkit.Windows
{
    public abstract class MultiBindingExtensionBase : MultiBinding
    {
        public new IMultiValueConverter Converter
        {
            get => base.Converter;
            protected set
            {
                if (base.Converter != null)
                {
                    throw new InvalidOperationException($"The {GetType().Name}.Converter property is readonly.");
                }

                base.Converter = value;
            }
        }
    }
}
