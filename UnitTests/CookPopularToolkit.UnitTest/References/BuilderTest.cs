/*
 *Description: BuilderTest
 *Author: Chance.zheng
 *Creat Time: 2023/8/8 14:09:15
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularToolkit.UnitTest
{
    public class BuilderTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test(Description = "ObjectBuilder.CreateObject")]
        public void ObjectBuilder_CreateObject_Test()
        {
            List<ObjectBuilder.PropertyDefine> list = new List<ObjectBuilder.PropertyDefine>
            {
                new ObjectBuilder.PropertyDefine() { PropertyName = "EmployeeID", PropertyType = typeof(int), PropertyValue = 1 },
                new ObjectBuilder.PropertyDefine() { PropertyName = "EmployeeName", PropertyType = typeof(string), PropertyValue = "Chance" },
                new ObjectBuilder.PropertyDefine() { PropertyName = "Designation", PropertyType = typeof(string), PropertyValue = "写代码的厨子" }
            };

            dynamic instance = ObjectBuilder.CreateObject(list)!;

            Assert.That(instance.EmployeeID, Is.EqualTo(1));
            Assert.That(instance.EmployeeName, Is.EqualTo("Chance"));
            Assert.That(instance.Designation, Is.EqualTo("写代码的厨子"));
        }
    }
}
