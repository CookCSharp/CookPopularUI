/*
 *Description: FileHelper
 *Author: Chance.zheng
 *Creat Time: 2023/9/9 14:09:40
 *.Net Version: 2.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Text;

namespace CookPopularToolkit
{
    public class FileHelper
    {
        /// <summary>
        /// 获取当前执行程序下的某个文件的完整路径
        /// </summary>
        /// <param name="fileNameWithExtension">文件名及后缀</param>
        /// <returns></returns>
        public static string GetExecutingFullPath(string fileNameWithExtension)
        {
            string fileFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileNameWithExtension);
            CheckHelper.FileNotFoundException(fileFullPath);

            return fileFullPath;
        }
    }
}
