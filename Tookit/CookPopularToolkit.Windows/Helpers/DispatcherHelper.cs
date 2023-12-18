/*
 *Description: DispatcherHelper
 *Author: Chance.zheng
 *Creat Time: 2023/8/17 20:12:58
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
    public sealed class DispatcherHelper
    {
        public static async Task AppInvokeAsync(Action action)
        {
            if (System.Windows.Application.Current != null)
                await System.Windows.Application.Current.Dispatcher.InvokeAsync(action, DispatcherPriority.Normal);
        }

        public static void AppInvoke(Action action)
        {
            if (System.Windows.Application.Current != null)
                System.Windows.Application.Current.Dispatcher.Invoke(action, DispatcherPriority.Normal);
        }

        public static void AppSyncPost(SendOrPostCallback action, SynchronizationContext synchronizationContext, object? state = null)
        {
            if (synchronizationContext == null) synchronizationContext = new DispatcherSynchronizationContext(System.Windows.Application.Current.Dispatcher, DispatcherPriority.Normal);
            SynchronizationContext.SetSynchronizationContext(synchronizationContext);
            SynchronizationContext.Current?.Post(action, state);
        }

        public static void AppSyncSend(SendOrPostCallback action, SynchronizationContext synchronizationContext, object? state = null)
        {
            if (synchronizationContext == null) synchronizationContext = new DispatcherSynchronizationContext(System.Windows.Application.Current.Dispatcher, DispatcherPriority.Normal);
            SynchronizationContext.SetSynchronizationContext(synchronizationContext);
            SynchronizationContext.Current?.Send(action, state);
        }

        /// <summary>
        /// 循环执行<see cref="Dispatcher"/>
        /// </summary>
        public static void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            DispatcherOperation exitOperation = Dispatcher.CurrentDispatcher.InvokeAsync(new Action(() => frame.Continue = false), DispatcherPriority.Background);
            //DispatcherOperation exitOperation = Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(t => (t as DispatcherFrame)!.Continue = false), frame);
            Dispatcher.PushFrame(frame);

            if (exitOperation.Status != DispatcherOperationStatus.Executing)
            {
                exitOperation.Abort();
            }
        }
    }
}
