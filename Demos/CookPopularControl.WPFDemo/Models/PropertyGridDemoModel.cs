/*
 *Description: PropertyGridDemoModel
 *Author: Chance.zheng
 *Creat Time: 2023/8/21 16:31:55
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using AutoMapper;
using CookPopularUI.WPF.Controls;
using DryIoc;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
#if !NETFRAMEWORK
using System.Diagnostics.Metrics;
#endif
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CookPopularUI.WPFDemo.Models
{
    public enum Country
    {
        China,
        Japan,
        Italy,
        USA,
        UK
    }

    public class PropertyGridDemoModel : BindableBase
    {
        [Category("ValueType")]
        [Description("这是Int")]
        [Range(1, 110)]
        [DefaultValue(2)]
        public int Integer { get; set; }

        [Category("ValueType")]
        [Description("这是Double")]
        [Range(1.1, 110.5)]
        [DefaultValue(5.5)]
        public double Double { get; set; }

        [Category("ValueType")]
        [Description("这是一个布尔型，开关控件")]
        [DefaultValue(true)]
        public bool Boolean { get; set; }

        [Category("ValueType")]
        [Description("这是一个布尔型，勾选框控件")]
        [DefaultValue(true)]
        public bool? CheckBox { get; set; }


        [Category("Text")]
        [Description("这是可编辑文本")]
        [DefaultValue("写代码的厨子")]
        public string EditableString { get; set; }

        [Category("Text")]
        [Description("这是只读文本")]
        [DefaultValue("CookPopularUI")]
        [Editable(false)]
        public string ReadOnlyString { get; set; }


        [Category("Items")]
        [Description("这是Enum")]
        [DefaultValue(Country.Italy)]
        public Country Enum { get; set; }

        [Category("Items")]
        [Description("这是IEnumerable<string>")]
        [Index(2)]
        public IList<string> Lists { get; set; }


        [Description("这是按钮")]
        [DefaultValue("ShowMessage")]
        [Command(typeof(PropertyGridDemoModel), "ButtonCommand", "Chance")]
        public object Button { get; set; }

        [Description("这是图片")]
        public ImageSource ImageSource { get; set; }

        [Description("水平方向")]
        [DefaultValue(HorizontalAlignment.Center)]
        public HorizontalAlignment HorizontalAlignment { get; set; }

        [Description("竖直方向")]
        [DefaultValue(VerticalAlignment.Bottom)]
        public VerticalAlignment VerticalAlignment { get; set; }

        [Browsable(false)]
        public DelegateCommand<string> ButtonCommand => new DelegateCommand<string>(t => { MessageBox.Show("ShowMessage按钮点击了，" + t); });
    }
}
