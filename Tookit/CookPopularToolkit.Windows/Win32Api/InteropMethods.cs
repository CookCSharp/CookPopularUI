/*
 *Description: InteropMethods
 *Author: Chance.zheng
 *Creat Time: 2023/8/9 18:14:56
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularToolkit.Windows.Win32Api
{
    /// <summary>
    /// <see cref="MS.Win32.UnsafeNativeMethods"/>
    /// </summary>
    public sealed class InteropMethods
    {
        #region Hook

        public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        #endregion

        /// <summary>
        /// 获取窗口句柄
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        [DllImport(InteropValues.ExternDll.User32, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 设置窗体的显示或隐藏
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nCmdShow"></param>
        /// <returns></returns>
        [DllImport(InteropValues.ExternDll.User32, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport(InteropValues.ExternDll.Gdi32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true)]
        public static extern int ReleaseDC(IntPtr window, IntPtr dc);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern IntPtr GetDC(IntPtr ptr);

        [DllImport(InteropValues.ExternDll.Gdi32, SetLastError = true)]
        public static extern uint GetPixel(IntPtr dc, int x, int y);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(InteropValues.ExternDll.Gdi32, EntryPoint = nameof(DeleteObject), CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool IntDeleteObject(IntPtr hObject);

        [SecurityCritical]
        public static bool DeleteObject(IntPtr hObject)
        {
            var result = IntDeleteObject(hObject);
            return result;
        }

        // Import dwmapi.dll and define DwmSetWindowAttribute in C# corresponding to the native function.
        [DllImport(InteropValues.ExternDll.Dwmapi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern long DwmSetWindowAttribute(IntPtr hwnd, InteropValues.DWMWINDOWATTRIBUTE attribute, ref InteropValues.DWM_WINDOW_CORNER_PREFERENCE pvAttribute, uint cbAttribute);

        [DllImport(InteropValues.ExternDll.User32)]
        public static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        [DllImport(InteropValues.ExternDll.Shell32, CallingConvention = CallingConvention.StdCall)]
        public static extern uint SHAppBarMessage(int dwMessage, ref InteropValues.APPBARDATA pData);

        [DllImport(InteropValues.ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, ref InteropValues.MONITORINFO monitorInfo);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(InteropValues.ExternDll.User32)]
        public static extern int GetSystemMetrics(InteropValues.SM nIndex);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, InteropValues.WindowPositionFlags flags);

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hwnd, out InteropValues.RECT lpRect);

        [DllImport(InteropValues.ExternDll.User32)]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport(InteropValues.ExternDll.User32)]
        public static extern bool EnableMenuItem(IntPtr hMenu, int UIDEnabledItem, int uEnable);

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport(InteropValues.ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto, ExactSpelling = true)]
        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetActiveWindow();

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int GetDoubleClickTime();

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true)]
        public static extern bool GetPhysicalCursorPos(ref InteropValues.POINT lpPoint);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out InteropValues.POINT pt);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Unicode)]
        public static extern ushort RegisterClass(ref InteropValues.WNDCLASS lpWndClass);

        [DllImport(InteropValues.ExternDll.User32, EntryPoint = "RegisterWindowMessageW", CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern uint RegisterWindowMessage([MarshalAs(UnmanagedType.LPWStr)] string msg);

        [SecurityCritical]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport(InteropValues.ExternDll.User32, SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateWindowExW")]
        public static extern IntPtr CreateWindowEx(int dwExStyle,
                                                   [MarshalAs(UnmanagedType.LPWStr)] string lpClassName,
                                                   [MarshalAs(UnmanagedType.LPWStr)] string lpWindowName,
                                                   int dwStyle,
                                                   int x,
                                                   int y,
                                                   int nWidth,
                                                   int nHeight,
                                                   IntPtr hWndParent,
                                                   IntPtr hMenu,
                                                   IntPtr hInstance,
                                                   IntPtr lpParam);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport(InteropValues.ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyWindow(IntPtr hwnd);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
        public static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
        public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 4)
            {
                return SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
            }
            return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
        }

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool AttachThreadInput(in uint currentForegroundWindowThreadId, in uint thisWindowThreadId, bool isAttach);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool FlashWindowEx(ref InteropValues.FLASHWINFO pwfi);

        [DllImport(InteropValues.ExternDll.Shell32, CharSet = CharSet.Auto)]
        public static extern bool Shell_NotifyIcon(int message, [In] ref InteropValues.NOTIFYICONDATA pnid);

        [DllImport(InteropValues.ExternDll.User32)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int nMsg, IntPtr wParam, IntPtr lParam);

        [DllImport(InteropValues.ExternDll.Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport(InteropValues.ExternDll.Kernel32)]
        public static extern uint GetCurrentThreadId();

        [DllImport(InteropValues.ExternDll.User32)]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetKeyboardLayout(uint dwLayout);

        [DllImport(InteropValues.ExternDll.User32)]
        public static extern uint MapVirtualKeyEx(uint uCode, uint uMapType, IntPtr dwhkl);

        [DllImport(InteropValues.ExternDll.User32)]
        public static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out][MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);

        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern IntPtr SetClipboardViewer(IntPtr hWnd);

        /// <param name="hWndRemove">handle to window to remove</param>
        /// <param name="hWndNewNext"> handle to next window</param>
        /// <returns></returns>
        [DllImport(InteropValues.ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        [DllImport(InteropValues.ExternDll.Kernel32, EntryPoint = "RtlCopyMemory")]
        public static extern void CopyMemory(IntPtr destination, IntPtr source, uint length);

        [DllImport(InteropValues.ExternDll.Kernel32, EntryPoint = "RtlMoveMemory")]
        public static extern void MoveMemory(IntPtr dest, IntPtr src, uint count);

        [DllImport(InteropValues.ExternDll.User32, EntryPoint = "DestroyIcon", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DestroyIcon(IntPtr hIcon);

        [DllImport(InteropValues.ExternDll.Shell32, EntryPoint = "SHGetFileInfo", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref InteropValues.SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        [DllImport(InteropValues.ExternDll.Shell32, EntryPoint = "ExtractAssociatedIcon", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int ExtractAssociatedIconA(int hInst, string lpIconPath, ref int lpiIcon);


        [DllImport(InteropValues.ExternDll.User32, SetLastError = true)]
        public static extern bool ChangeWindowMessageFilterEx(IntPtr hWnd, uint msg, uint action, ref InteropValues.CHANGEFILTERSTRUCT pChangeFilterStruct);

        [DllImport(InteropValues.ExternDll.User32, SetLastError = true)]
        public static extern bool ChangeWindowMessageFilter(uint msg, uint flags);

        [DllImport(InteropValues.ExternDll.Shell32)]
        public static extern void DragAcceptFiles(IntPtr hWnd, bool fAccept);

        [DllImport(InteropValues.ExternDll.Shell32, CharSet = CharSet.Unicode)]
        public static extern uint DragQueryFile(IntPtr hWnd, uint iFile, StringBuilder lpszFile, int cch);

        [DllImport(InteropValues.ExternDll.Shell32)]
        public static extern bool DragQueryPoint(IntPtr hDrop, out InteropValues.POINT lppt);

        [DllImport(InteropValues.ExternDll.Shell32)]
        public static extern void DragFinish(IntPtr hDrop);

        [DllImport(InteropValues.ExternDll.Ole32)]
        public static extern int RevokeDragDrop(IntPtr hWnd);

        [DllImport(InteropValues.ExternDll.Shell32)]
        public static extern bool IsUserAnAdmin();

        /// <summary>
        /// 打开服务控制管理器数据库
        /// </summary>
        /// <param name="lpMachineName"></param>
        /// <param name="lpDatabaseName"></param>
        /// <param name="dwDesiredAccess"></param>
        /// <returns></returns>
        [DllImport(InteropValues.ExternDll.Advapi32, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr OpenSCManager(string? lpMachineName, string? lpDatabaseName, uint dwDesiredAccess);

        [DllImport(InteropValues.ExternDll.Advapi32, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, uint dwDesiredAccess);

        [DllImport(InteropValues.ExternDll.Advapi32, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool QueryServiceConfig(IntPtr hService, IntPtr lpServiceConfig, uint cbBufSize, out uint pcbBytesNeeded);

        [DllImport(InteropValues.ExternDll.Advapi32, SetLastError = true)]
        public static extern bool CloseServiceHandle(IntPtr hSCObject);


        [DllImport(InteropValues.ExternDll.Kernel32, SetLastError = true)]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport(InteropValues.ExternDll.Advapi32, SetLastError = true)]
        private static extern bool QueryServiceStatusEx(IntPtr hService, int infoLevel, IntPtr lpBuffer, uint cbBufSize, out uint pcbBytesNeeded);

        [DllImport(InteropValues.ExternDll.Kernel32, SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);
    }
}
