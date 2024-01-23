/*
 *Description: FileDroper
 *Author: Chance.zheng
 *Creat Time: 2024/1/16 10:16:29
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using CookPopularToolkit.Windows.Win32Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CookPopularToolkit.Windows
{
    public class FileDroper : WindowMessageProcessor
    {
        public event System.Windows.DragEventHandler DragDrop;
        public string[] DropFilePaths { get; private set; }
        public InteropValues.POINT DropPoint { get; private set; } //像素点

        public override void AddHook()
        {
            base.AddHook();
            if (InteropMethods.IsUserAnAdmin()) InteropMethods.RevokeDragDrop(Hwnd);
            InteropMethods.DragAcceptFiles(Hwnd, true);
            ChangeMessageFilter(Hwnd);
        }

        public override void RemoveHook()
        {
            base.RemoveHook();
            InteropMethods.DragAcceptFiles(Hwnd, false);
        }

        protected override IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            if (TryGetDropInfo(msg, wParam, out string[] filePaths, out InteropValues.POINT point))
            {
                DropPoint = point;
                DropFilePaths = filePaths;
                DragDrop?.Invoke(this, default);
                return IntPtr.Zero;
            }
            return base.WndProc(hWnd, msg, wParam, lParam);
        }

        private static void ChangeMessageFilter(IntPtr handle)
        {
            Version ver = Environment.OSVersion.Version;
            bool isVistaOrHigher = ver >= new Version(6, 0);
            bool isWin7OrHigher = ver >= new Version(6, 1);
            if (isVistaOrHigher)
            {
                var status = new InteropValues.CHANGEFILTERSTRUCT { cbSize = 8 };
                foreach (uint msg in new[] { InteropValues.WM_DROPFILES, InteropValues.WM_COPYGLOBALDATA, InteropValues.WM_COPYDATA })
                {
                    bool error = false;
                    if (isWin7OrHigher) error = !InteropMethods.ChangeWindowMessageFilterEx(handle, msg, InteropValues.MSGFLT_ALLOW, ref status);
                    else error = !InteropMethods.ChangeWindowMessageFilter(msg, InteropValues.MSGFLT_ADD);
                    if (error) throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
        }

        private static bool TryGetDropInfo(int msg, IntPtr wParam, out string[] dropFilePaths, out InteropValues.POINT dropPoint)
        {
            dropFilePaths = new string[] { };
            dropPoint = new InteropValues.POINT();
            if (msg != InteropValues.WM_DROPFILES) return false;

            uint fileCount = InteropMethods.DragQueryFile(wParam, uint.MaxValue, null!, 0);
            dropFilePaths = new string[fileCount];
            for (uint i = 0; i < fileCount; i++)
            {
                var sb = new StringBuilder((int)InteropValues.MAX_PATH);
                uint result = InteropMethods.DragQueryFile(wParam, i, sb, sb.Capacity);
                if (result > 0) dropFilePaths[i] = sb.ToString();
            }

            InteropMethods.DragQueryPoint(wParam, out dropPoint);
            InteropMethods.DragFinish(wParam);

            return true;
        }
    }
}
