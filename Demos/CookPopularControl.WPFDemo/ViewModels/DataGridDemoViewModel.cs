/*
 *Description: DataGridDemoViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/12/8 11:04:51
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace CookPopularUI.WPFDemo.ViewModels
{
    public class DataGridDemoViewModel : ViewModelBase
    {
        public static IEnumerable<string> Movies = new[] { "僵尸叔叔", "警察故事", "速度与激情", "侏罗纪公园" };

        public ObservableCollection<Person> Persions { get; set; }

        public ObservableCollection<dynamic> TestDatas { get; set; }

        public DelegateCommand AddCommand => new Lazy<DelegateCommand>(() => new DelegateCommand(OnAddAction)).Value;


        public DataGridDemoViewModel()
        {
            Persions = new ObservableCollection<Person>();
            TestDatas = new ObservableCollection<dynamic>();

            InitPerson();
        }

        private void InitPerson()
        {
            Persions.Add(new Person { Name = "张三", Gender = "男", Age = 18, IsWorkingOnIT = true, FavoriteMovie = "僵尸叔叔", Job = "软件开发", Department = "肿瘤事业部", Status = OrderStatus.None });
            Persions.Add(new Person { Name = "李四", Gender = "女", Age = 32, IsWorkingOnIT = false, FavoriteMovie = "警察故事", Job = "技术支持", Department = "心脏事业部", Status = OrderStatus.New });
            Persions.Add(new Person { Name = "王五", Gender = "未知", Age = 22, IsWorkingOnIT = false, FavoriteMovie = "僵尸叔叔", Job = "实施", Department = "心电事业部", Status = OrderStatus.Processing });
            Persions.Add(new Person { Name = "赵六", Gender = "男", Age = 38, IsWorkingOnIT = true, FavoriteMovie = "侏罗纪公园", Job = "数据挖掘", Department = "卒中事业部", Status = OrderStatus.Shipped });
            Persions.Add(new Person { Name = "朱七", Gender = "男", Age = 15, IsWorkingOnIT = true, FavoriteMovie = "速度与激情", Job = "测试", Department = "急救事业部", Status = OrderStatus.Received });
        }

        public void InitTestData(object obj, RoutedEventArgs arg)
        {
            //TestDatas = new ObservableCollection<dynamic>();
            //TestDatas.Add(new Person { Name = "张三", Gender = "男", Age = 18, IsWorkingOnIT = true, FavoriteMovie = "僵尸叔叔", Job = "软件开发", Department = "肿瘤事业部" });

            DataGrid dataGrid = (DataGrid)obj;
            dataGrid.Columns.Clear();

            var columns = new ObservableCollection<Column>
            {
                new Column { TypeName = "bool?", TypeProperty = true },
                new Column { TypeName = "bool", TypeProperty = true },
                new Column { TypeName = "string", TypeProperty = "僵尸叔叔" },
                new Column { TypeName = "enum", TypeProperty = OrderStatus.None }
            };

            dataGrid.Columns.Add(new DataGridTextColumn() { Header = "姓名", Binding = new Binding("Name") });
            dataGrid.Columns.Add(new DataGridTextColumn() { Header = "性别", Binding = new Binding("Gender") });
            foreach (var column in columns)
            {
                switch (column.TypeName)
                {
                    case "bool?":
                        dataGrid.Columns.Add(new DataGridCheckBoxColumn() { Header = "从事IT", Binding = new Binding("TypeProperty") { Source = column, Mode = BindingMode.TwoWay } });
                        break;
                    case "bool":
                        dataGrid.Columns.Add(new DataGridTemplateColumn() { Header = "介绍", CellTemplate = GetDataTemplate(column.TypeProperty) });
                        break;
                    case "string":
                        dataGrid.Columns.Add(new DataGridTextColumn() { Header = "喜爱电影", Binding = new Binding("TypeProperty") { Source = column, Mode = BindingMode.TwoWay } });
                        break;
                    case "enum":
                        dataGrid.Columns.Add(new DataGridComboBoxColumn() { Header = "介绍", SelectedValueBinding = new Binding("TypeProperty") { Source = column, Mode = BindingMode.TwoWay }, ItemsSource = Enum.GetValues<OrderStatus>() });
                        break;
                    default:
                        break;
                }
            }

            List<PP> pps = [new PP { Name = "First", Gender = "Man", Columns = columns }];
            dataGrid.ItemsSource = pps;

            DataTemplate GetDataTemplate(dynamic data)
            {
                return (DataTemplate)XamlReader.Parse($"<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><ui:SwitchButton xmlns:ui=\"https://github.cookpopularui/2021/xaml\" IsChecked=\"{data}\"/></DataTemplate>");
            }
        }

        private void OnAddAction()
        {
            Persions.Add(new Person { Name = "郑十", Gender = "女", Age = 60, IsWorkingOnIT = false, FavoriteMovie = "僵尸叔叔", Job = "机械设计", Department = "试剂事业三部" });
        }
    }


    public enum OrderStatus { None, New, Processing, Shipped, Received };
    public class Person
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public bool IsWorkingOnIT { get; set; }
        public string FavoriteMovie { get; set; }
        public string Job { get; set; }
        public string Department { get; set; }
        public OrderStatus Status { get; set; }
    }



    public class PP
    {
        public string Name { get; set; }
        public string Gender { get; set; }

        public ObservableCollection<Column> Columns { get; set; }
    }

    public class Column
    {
        public string TypeName { get; set; }

        public object TypeProperty { get; set; }
    }
}
