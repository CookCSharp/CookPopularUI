/*
 *Description: ModelBase
 *Author: Chance.zheng
 *Creat Time: 2023/8/23 11:12:31
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CookPopularToolkit;

namespace CookPopularUI.WPFDemo.Models
{
    public class ModelBase : BindableBase, IChangeTracking, IRevertibleChangeTracking
    {
        Dictionary<string, object> _propertys = new Dictionary<string, object>();

        protected void SetValue(string name, object value) => _propertys.TryAdd(name, value);

        protected object GetValue(string name) => _propertys.TryGetValue(name, out object value) ? value : null;

        public bool IsChanged { get; protected set; }

        public void AcceptChanges()
        {
            _propertys.Clear();

            IsChanged = true;
        }

        public void RejectChanges()
        {
            _propertys.ForEach(p => GetType().GetRuntimeProperty(p.Key).SetValue(this, p.Value));

            AcceptChanges();
        }
    }
}
