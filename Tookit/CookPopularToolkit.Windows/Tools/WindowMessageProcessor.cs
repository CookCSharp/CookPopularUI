/*
 *Description: WindowMessageProcessor
 *Author: Chance.zheng
 *Creat Time: 2024/1/16 10:03:24
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using CookPopularToolkit.Windows.Win32Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularToolkit.Windows
{
    /// <summary>
    /// 消息处理器
    /// </summary>
    public class WindowMessageProcessor
    {
        private delegate IntPtr WndProcDelegate(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private IntPtr callBackPtr = IntPtr.Zero;
        private WndProcDelegate wndProcDelegate;//声明为成员，避免被回收

        /// <summary>
        /// 接收消息的控件句柄
        /// </summary>
        public IntPtr Hwnd { get; set; }

        /// <summary>
        /// 添加消息处理程序挂钩
        /// </summary>
        public virtual void AddHook()
        {
            RemoveHook();
            wndProcDelegate = new WndProcDelegate(WndProc);
            IntPtr wndProcPtr = Marshal.GetFunctionPointerForDelegate(wndProcDelegate);
            callBackPtr = InteropMethods.SetWindowLong(Hwnd, (int)InteropValues.GetWindowLongFields.GWL_WNDPROC, wndProcPtr);
        }

        /// <summary>
        /// 移除消息处理程序挂钩
        /// </summary>
        public virtual void RemoveHook()
        {
            if (callBackPtr == IntPtr.Zero) return;
            InteropMethods.SetWindowLong(Hwnd, (int)InteropValues.GetWindowLongFields.GWL_WNDPROC, callBackPtr);
            callBackPtr = IntPtr.Zero;
        }

        /// <summary>消息处理函数</summary>
        protected virtual IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            return InteropMethods.CallWindowProc(callBackPtr, hWnd, msg, wParam, lParam);
        }
    }
}
