﻿/*
 *Description: DispatcherThrottle
 *Author: Chance.zheng
 *Creat Time: 2023/11/10 11:21:27
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CookPopularToolkit.Windows
{
    /// <summary>
    /// Implements a simple throttle that uses the dispatcher to delay the target action.<para/>
    /// Calling <see cref="Tick()"/> multiple times will result in on single call to the action as soon as
    /// the dispatcher of the thread that created the throttle is dispatching calls of the specified priority.
    /// </summary>
    public class DispatcherThrottle
    {
        private readonly Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;
        private readonly Action _target;
        private readonly DispatcherPriority _priority;

        private int _counter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatcherThrottle"/> class.
        /// </summary>
        /// <param name="target">The target action to invoke when the throttle condition is hit.</param>
        public DispatcherThrottle(Action target) : this(DispatcherPriority.Normal, target)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatcherThrottle" /> class.
        /// </summary>
        /// <param name="priority">The priority of the dispatcher.</param>
        /// <param name="target">The target action to invoke when the throttle condition is hit.</param>
        public DispatcherThrottle(DispatcherPriority priority, Action target)
        {
            _target = target;
            _priority = priority;
        }

        /// <summary>
        /// Ticks this instance to trigger the throttle.
        /// </summary>
        public void Tick()
        {
            if (Interlocked.CompareExchange(ref _counter, 1, 0) != 0)
                return;

            _dispatcher.BeginInvoke(_priority, delegate ()
            {
                _target();

                Interlocked.Exchange(ref _counter, 0);
            });
        }
    }
}
