/*
 *Description: AttributeTest
 *Author: Chance.zheng
 *Creat Time: 2023/8/8 15:10:50
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularToolkit.UnitTest
{
    public class AttributeField
    {
        public int MyProperty { get; set; }
    }

    public class AttributeTest
    {
        [Test(Description = "InitValueAttribute")]
        public void InitValueAttribute_Test()
        {
            var s = new AttributeField();
        }
    }
}
