/*
 *Description: StepBarDemoViewModel
 *Author: Chance.zheng
 *Creat Time: 2023/9/8 14:33:30
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using CookPopularUI.WPF.Controls;
using CookPopularUI.WPFDemo.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CookPopularUI.WPFDemo.ViewModels
{
    public class StepBarDemoViewModel : ViewModelBase
    {
        public int StepIndex { get; set; }
        public IList<StepBarDemoModel> DataList { get; set; }

        public DelegateCommand<Grid> PreviousCommand => new Lazy<DelegateCommand<Grid>>(() => new DelegateCommand<Grid>(OnPreviousAction)).Value;
        public DelegateCommand<Grid> NextCommand => new Lazy<DelegateCommand<Grid>>(() => new DelegateCommand<Grid>(OnNextAction)).Value;


        public StepBarDemoViewModel()
        {
            DataList = new List<StepBarDemoModel>
            {
                new StepBarDemoModel() { Header = "StepStepStep", Content = "Test1" },
                new StepBarDemoModel() { Header = "StepStepStep", Content = "Test2" },
                new StepBarDemoModel() { Header = "StepStepStep", Content = "Test3" },
                new StepBarDemoModel() { Header = "StepStepStep", Content = "Test4" },
                new StepBarDemoModel() { Header = "StepStepStep", Content = "Test5" },
            };
        }

        private void OnPreviousAction(Grid grid)
        {
            foreach (var stepBar in (grid.Children[1] as Grid).Children.OfType<StepBar>())
            {
                stepBar.Previous();
            }

            foreach (var stepBar in (grid.Children[2] as Grid).Children.OfType<StepBar>())
            {
                stepBar.Previous();
            }
        }

        private void OnNextAction(Grid grid)
        {          
            foreach (var stepBar in (grid.Children[1] as Grid).Children.OfType<StepBar>())
            {
                stepBar.Next();
            }

            foreach (var stepBar in (grid.Children[2] as Grid).Children.OfType<StepBar>())
            {
                stepBar.Next();
            }
        }
    }
}
