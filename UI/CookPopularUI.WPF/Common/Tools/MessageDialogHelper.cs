/*
 *Description: MessageDialogHelper
 *Author: Chance.zheng
 *Creat Time: 2023/9/12 11:30:12
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularToolkit;
using CookPopularUI.WPF.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CookPopularUI.WPF.Tools
{
    public class MessageDialogHelper
    {
        public static void ShowFileError(string fileFullPath, bool isShow = true)
        {
            try
            {
                CheckHelper.FileNotFoundException(fileFullPath);
            }
            catch (FileNotFoundException fe)
            {
                if (isShow)
                    MessageDialog.ShowError(fe.Message, fe.FileName);
                else
                    throw new FileNotFoundException(fe.Message);
            }
            catch (Exception ex)
            {
                if (isShow)
                    MessageDialog.ShowError(ex.Message);
                else
                    throw new Exception(ex.Message);
            }
        }
    }
}
