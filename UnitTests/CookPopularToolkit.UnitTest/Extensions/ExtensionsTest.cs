/*
 *Description: ExtensionsTest
 *Author: Chance.zheng
 *Creat Time: 2023/8/8 20:04:51
 *.Net Version: 4.8
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularToolkit.UnitTest
{
    public class ExtensionsTest
    {
        [SetUp]
        public void Setup()
        {
            Convert.ToInt32((long)1);
        }

        [Test(Description = "BooleanExtensions.ToBoolean(int)")]
        public void Boolean_Int_Test()
        {
            Assert.IsTrue(3.ToBoolean());
            Assert.IsFalse(0.ToBoolean());
        }

        [Test(Description = "BooleanExtensions.ToBoolean(string)")]
        public void Boolean_String_Test()
        {
            Assert.IsTrue("3".ToBoolean());
            Assert.IsFalse("0".ToBoolean());

            string s = null;
            Assert.IsTrue("3".ToBoolean(null));
            Assert.IsFalse(s.ToBoolean(null));
        }

        [Test(Description = "BooleanExtensions.ToBoolean(object)")]
        public void Boolean_Object_Test()
        {
            object obj = new object();
            Assert.IsTrue(obj.ToBoolean());
            Assert.IsFalse(default(object).ToBoolean());
        }
    }
}
