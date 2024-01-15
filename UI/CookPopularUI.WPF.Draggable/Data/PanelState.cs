/*
 *Description: PanelState
 *Author: Chance.zheng
 *Creat Time: 2024/1/5 17:21:38
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularUI.WPF.Draggable
{
    public enum PanelState
    {
        /// <summary>
        /// The normal state for a panel.
        /// </summary>
        Restored,

        /// <summary>
        /// The maxmized state for a panel.
        /// </summary>
        Maximized,

        /// <summary>
        /// The minimized state for a panel.
        /// </summary>
        Minimized
    }
}
