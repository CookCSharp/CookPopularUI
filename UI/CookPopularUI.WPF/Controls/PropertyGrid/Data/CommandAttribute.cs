/*
 * Description：CommandAttribute 
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2023-08-22 19:49:13
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 表示<see cref="Button"/>的命令
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        private const string CommandDefaultName = "DefaultButtonCommand";

        public Type TargetType { get; set; }

        public string CommandName { get; set; }

        public object? Parameter { get; set; }

        public CommandAttribute(Type targetType) : this(targetType, CommandDefaultName, null)
        {

        }

        public CommandAttribute(Type targetType, string commandName) : this(targetType, commandName, null)
        {

        }

        public CommandAttribute(Type targetType, string commandName, object? parameter)
        {
            TargetType = targetType;
            CommandName = commandName;
            Parameter = parameter;
        }
    }
}
