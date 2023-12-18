/*
 *Description: StepStatus
 *Author: Chance.zheng
 *Creat Time: 2023/9/12 15:54:38
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 步骤状态
    /// </summary>
    public enum StepStatus
    {
        /// <summary>
        /// 完成
        /// </summary>
        Finish,

        /// <summary>
        /// 进行中
        /// </summary>
        Running,

        /// <summary>
        /// 等待中
        /// </summary>
        Waiting
    }
}
