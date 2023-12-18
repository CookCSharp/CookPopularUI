/*
 *Description: TypeHelperTest
 *Author: Chance.zheng
 *Creat Time: 2023/8/8 13:24:44
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace CookPopularToolkit.UnitTest
{
    public class TypeHelperTest
    {
        private class Filed
        {
            [DefaultValue("Chance")]
            [Description("对姓名的命名")]
            public string Name { get; set; }

            [DefaultValue("写代码的厨子")]
            [Description("对名称的描述")]
            public string Description { get; set; }
        }

        private class Filed1
        {
            [DefaultValue("DefaultChance")]
            [Description("Description对姓名的命名")]
            [CookPopularUI]
            public string Name { get; set; } = "InitChance1";

            [DefaultValue("Default写代码的厨子")]
            [Description("Description对名称的描述")]
            [CookPopularUI]
            public string Description { get; set; } = "InitDescription1";

            public int Age { get; set; }
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test(Description = "TypeHelper.SetPropertyValue")]
        public void TypeHelper_SetPropertyValue_Test()
        {
            Filed f1 = new Filed();
            TypeHelper.SetPropertyValueInReflect(f1, "Name", "Chance");
            var f2 = TypeHelper.SetPropertyValueInReflect<Filed>("Description", "写代码的厨子");

            Assert.That(f1.Name, Is.EqualTo("Chance"));
            Assert.That(f2.Description, Is.EqualTo("写代码的厨子"));

            Filed f3 = new Filed();
            TypeHelper.SetPropertyValueInEmit(f3, "Name", "Chance");
            var f4 = TypeHelper.SetPropertyValueInEmit<Filed>("Description", "写代码的厨子");

            Assert.That(f3.Name, Is.EqualTo("Chance"));
            Assert.That(f4.Description, Is.EqualTo("写代码的厨子"));
        }

        [Test(Description = "TypeHelper.GetPropertyValue")]
        public void TypeHelper_GetPropertyValue_Test()
        {
            Filed f1 = new Filed() { Name = "Chance", Description = "写代码的厨子" };
            var name = TypeHelper.GetPropertyValue(f1, "Name");
            var description = TypeHelper.GetPropertyInitValue<Filed>("Description");

            Assert.That(name, Is.EqualTo("Chance"));
            Assert.That(description, Is.EqualTo(null));
        }


        [Test(Description = "TypeHelper.GetPropertyValues")]
        public void TypeHelper_GetPropertyValues_Test()
        {
            Filed1 f1 = new Filed1() { Name = "Chance", Description = "写代码的厨子" };
            var properties1 = f1.GetPropertyValues();
            var properties2 = f1.GetPropertyInitValues();
            var properties3 = TypeHelper.GetPropertyInitValues<Filed1>();

            var properties4 = f1.GetAttriutePropertyValues<CookPopularUIAttribute>();
            var properties5 = TypeHelper.GetAttriutePropertyInitValues<Filed1, CookPopularUIAttribute>();

            var descriptions1 = TypeHelper.GetDescriptionAttributeValuesOfProperty<Filed1>();
            var defaultValues1 = TypeHelper.GetDefaultValueAttributeValuesOfProperty<Filed1>();

            var descriptions2 = TypeHelper.GetAttributeValuesOfProperty<Filed1, DescriptionAttribute>("Description");
            var defaultValues2 = TypeHelper.GetAttributeValuesOfProperty<Filed1, DefaultValueAttribute>("Value");
            Assert.Throws<Exception>(() => TypeHelper.GetAttributeValuesOfProperty<Filed1, CookPopularUIAttribute>("dd"));
        }
    }
}
