/*
 *Description: MinimizedPosition
 *Author: Chance.zheng
 *Creat Time: 2024/1/5 17:34:10
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
    /// <summary>
    /// The dock panel docking positions.
    /// </summary>
    public enum MinimizedPosition
    {
        /// <summary>
        /// Docks the panels along the top.
        /// </summary>
        Top,

        /// <summary>
        /// Docks the panels along the bottom.
        /// </summary>
        Bottom,

        /// <summary>
        /// Docks the panels down the left side.
        /// </summary>
        Left,

        /// <summary>
        /// Docks the panels down the rights side,
        /// </summary>
        Right,

        /// <summary>
        /// Docks no panels
        /// </summary>
        None
    }
}
