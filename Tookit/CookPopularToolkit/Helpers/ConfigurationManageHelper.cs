/*
 *Description: ConfigurationManageHelper
 *Author: Chance.zheng
 *Creat Time: 2023/9/12 11:20:03
 *.Net Version: 2.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace CookPopularToolkit
{
    public class ConfigurationManageHelper
    {
        public static void AddItem(string key, string value, string sectionName = "appSettings")
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings.Add(key, value);
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(sectionName);
        }

        public static void DeleteItem(string key, string sectionName = "appSettings")
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings.Remove(key);
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(sectionName);
        }

        public static void ModifyItem(string key, string value, string sectionName = "appSettings")
        {
            ModifyItem(key, value, sectionName, null);
        }

        public static void ModifyItem(string key, string value, string sectionName = "appSettings", string? exePath = null)
        {
            Configuration configuration;
            if (!string.IsNullOrEmpty(exePath) && exePath!.EndsWith(".exe"))
                configuration = ConfigurationManager.OpenExeConfiguration(exePath);
            else
                configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(sectionName);
        }

        public static string ReadItem(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }
    }
}
