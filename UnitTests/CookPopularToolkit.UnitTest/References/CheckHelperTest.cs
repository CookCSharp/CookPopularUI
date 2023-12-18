/*
 *Description: CheckHelperTest
 *Author: Chance.zheng
 *Creat Time: 2023/9/9 13:03:52
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
    public class CheckHelperTest
    {
        [Test(Description = "CheckHelper.ArgumentException")]
        public void TypeHelper_ArgumentException_Test()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => CheckHelper.ArgumentNullOrEmptyException(str, nameof(str)));

            IEnumerable<string> strings = null;
            Assert.Throws<ArgumentNullException>(() => CheckHelper.ArgumentNullException(strings, nameof(strings)));

            object obj = null;
            Assert.Throws<ArgumentNullException>(() => CheckHelper.ArgumentNullException(obj, nameof(obj)));

            Assert.Throws<ArgumentNullException>(() => CheckHelper.ArgumentNullExceptionWithMessage(obj, "this is an error"));

            int integer = 0;
            Assert.Throws<ArgumentException>(() => CheckHelper.ArgumentException(integer.CompareTo(0) == 0, $"{nameof(integer)} can not be 0"));

            string fileFullPath = @"C:\Users\46535\Desktop\HeatTransform123.xslt";
            Assert.Throws<FileNotFoundException>(() => CheckHelper.FileNotFoundException(fileFullPath));

            Assert.Throws<FileNotFoundException>(() => CheckHelper.FileNotFoundException(!File.Exists(fileFullPath), $"{nameof(fileFullPath)} is not existed in this computer"));

            try
            {
                //CheckHelper.FileNotFoundException(fileFullPath);
                //CheckHelper.FileNotFoundException(!File.Exists(fileFullPath), $"{nameof(fileFullPath)} is not existed in this computer");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
