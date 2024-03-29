﻿/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：TreeViewModel
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-10 09:28:59
 */

using System.Collections.ObjectModel;

namespace CookPopularUI.WPFDemo.Models
{
    public class TreeViewModel
    {
        public string Header { get; set; }

        public int HeaderIndex { get; set; }

        public int Level { get; set; }

        public TreeViewModel Root { get; set; }

        public ObservableCollection<TreeViewModel> Children { get; set; }
    }

    public class TreeViewModelStandard
    {
        public string Header { get; set; }

        public int Level { get; set; }

        public int ParentLevel { get; set; }

        public bool IsParent { get; set; }

        public ObservableCollection<TreeViewModelStandard> Children { get; set; }
    }
}
