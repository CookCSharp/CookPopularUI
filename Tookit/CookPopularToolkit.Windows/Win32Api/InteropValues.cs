/*
 *Description: InteropValues
 *Author: Chance.zheng
 *Creat Time: 2023/8/9 18:13:43
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CookPopularToolkit.Windows.Win32Api
{
    public sealed class InteropValues
    {
        #region Const

        public static class ExternDll
        {
            public const string User32 = "user32.dll",
                                Gdi32 = "gdi32.dll",
                                GdiPlus = "gdiplus.dll",
                                Kernel32 = "kernel32.dll",
                                Shell32 = "shell32.dll",
                                MsImg = "msimg32.dll",
                                NTdll = "ntdll.dll",
                                Dwmapi = "dwmapi.dll",
                                Ole32 = "ole32.dll",
                                Advapi32 = "advapi32.dll";
        }

        public const int BITSPIXEL = 12,
                         PLANES = 14,
                         BI_RGB = 0,
                         DIB_RGB_COLORS = 0,
                         E_FAIL = unchecked((int)0x80004005),
                         HWND_NOTOPMOST = -2,
                         HWND_TOPMOST = -1,
                         NIF_MESSAGE = 0x00000001,
                         NIF_ICON = 0x00000002,
                         NIF_TIP = 0x00000004,
                         NIF_INFO = 0x00000010,
                         NIM_ADD = 0x00000000,
                         NIM_MODIFY = 0x00000001,
                         NIM_DELETE = 0x00000002,
                         NIIF_NONE = 0x00000000,
                         NIIF_INFO = 0x00000001,
                         NIIF_WARNING = 0x00000002,
                         NIIF_ERROR = 0x00000003,
                         WM_ACTIVATE = 0x0006,
                         WM_QUIT = 0x0012,
                         WM_GETMINMAXINFO = 0x0024,
                         WM_WINDOWPOSCHANGING = 0x0046,
                         WM_WINDOWPOSCHANGED = 0x0047,
                         WM_SETICON = 0x0080,
                         WM_NCCREATE = 0x0081,
                         WM_NCDESTROY = 0x0082,
                         WM_NCHITTEST = 0x0084,
                         WM_NCACTIVATE = 0x0086,
                         WM_NCRBUTTONDOWN = 0x00A4,
                         WM_NCRBUTTONUP = 0x00A5,
                         WM_NCRBUTTONDBLCLK = 0x00A6,
                         WM_NCUAHDRAWCAPTION = 0x00AE,
                         WM_NCUAHDRAWFRAME = 0x00AF,
                         WM_KEYDOWN = 0x0100,
                         WM_KEYUP = 0x0101,
                         WM_SYSKEYDOWN = 0x0104,
                         WM_SYSKEYUP = 0x0105,
                         WM_SYSCOMMAND = 0x112,
                         WM_MOUSEMOVE = 0x0200,
                         WM_LBUTTONUP = 0x0202,
                         WM_LBUTTONDBLCLK = 0x0203,
                         WM_RBUTTONUP = 0x0205,
                         WM_CLIPBOARDUPDATE = 0x031D,
                         WM_USER = 0x0400,
                         WS_VISIBLE = 0x10000000,
                         WS_EX_TOPMOST = 0x0008,
                         WS_EX_DLGMODALFRAME = 0x0001,
                         MF_BYCOMMAND = 0x00000000,
                         MF_BYPOSITION = 0x400,
                         MF_GRAYED = 0x00000001,
                         MF_SEPARATOR = 0x800,
                         TB_GETBUTTON = WM_USER + 23,
                         TB_BUTTONCOUNT = WM_USER + 24,
                         TB_GETITEMRECT = WM_USER + 29,
                         VERTRES = 10,
                         DESKTOPVERTRES = 117,
                         LOGPIXELSX = 88,
                         LOGPIXELSY = 90,
                         SC_CLOSE = 0xF060,
                         SC_SIZE = 0xF000,
                         SC_MOVE = 0xF010,
                         SC_MINIMIZE = 0xF020,
                         SC_MAXIMIZE = 0xF030,
                         SC_RESTORE = 0xF120,
                         SRCCOPY = 0x00CC0020,
                         MONITOR_DEFAULTTONEAREST = 0x00000002,
                         SC_STATUS_PROCESS_INFO = 0;

        public const uint WM_COPYGLOBALDATA = 0x0049,
                          WM_COPYDATA = 0x004A,
                          WM_DROPFILES = 0x0233,
                          MSGFLT_ALLOW = 1,
                          MSGFLT_ADD = 1,
                          MAX_PATH = 260,        
                          SC_MANAGER_CONNECT = 0x0001,
                          SERVICE_QUERY_CONFIG = 0x0001,
                          ERROR_INSUFFICIENT_BUFFER = 0x7a;

        #endregion

        #region Enum

        [Flags]
        [SuppressMessage("Design", "CA1069:不应复制枚举值", Justification = "<挂起>")]
        public enum WindowPositionFlags
        {
            /// <summary>
            ///     If the calling thread and the thread that owns the window are attached to different input queues, the system posts
            ///     the request to the thread that owns the window. This prevents the calling thread from blocking its execution while
            ///     other threads process the request.
            /// </summary>
            SWP_ASYNCWINDOWPOS = 0x4000,

            /// <summary>
            ///     Prevents generation of the WM_SYNCPAINT message.
            /// </summary>
            SWP_DEFERERASE = 0x2000,

            /// <summary>
            ///     Draws a frame (defined in the window's class description) around the window.
            /// </summary>
            SWP_DRAWFRAME = 0x0020,

            /// <summary>
            ///     Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to the window, even if
            ///     the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the window's
            ///     size is being changed.
            /// </summary>
            SWP_FRAMECHANGED = 0x0020,

            /// <summary>
            ///     Hides the window.
            /// </summary>
            SWP_HIDEWINDOW = 0x0080,

            /// <summary>
            ///     Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the
            ///     topmost or non-topmost group (depending on the setting of the hWndInsertAfter parameter).
            /// </summary>
            SWP_NOACTIVATE = 0x0010,

            /// <summary>
            ///     Discards the entire contents of the client area. If this flag is not specified, the valid contents of the client
            ///     area are saved and copied back into the client area after the window is sized or repositioned.
            /// </summary>
            SWP_NOCOPYBITS = 0x0100,

            /// <summary>
            ///     Retains the current position (ignores X and Y parameters).
            /// </summary>
            SWP_NOMOVE = 0x0002,

            /// <summary>
            ///     Does not change the owner window's position in the Z order.
            /// </summary>
            SWP_NOOWNERZORDER = 0x0200,

            /// <summary>
            ///     Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to the client area,
            ///     the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a
            ///     result of the window being moved. When this flag is set, the application must explicitly invalidate or redraw any
            ///     parts of the window and parent window that need redrawing.
            /// </summary>
            SWP_NOREDRAW = 0x0008,

            /// <summary>
            ///     Same as the SWP_NOOWNERZORDER flag.
            /// </summary>
            SWP_NOREPOSITION = 0x0200,

            /// <summary>
            ///     Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
            /// </summary>
            SWP_NOSENDCHANGING = 0x0400,

            /// <summary>
            ///     Retains the current size (ignores the cx and cy parameters).
            /// </summary>
            SWP_NOSIZE = 0x0001,

            /// <summary>
            ///     Retains the current Z order (ignores the hWndInsertAfter parameter).
            /// </summary>
            SWP_NOZORDER = 0x0004,

            /// <summary>
            ///     Displays the window.
            /// </summary>
            SWP_SHOWWINDOW = 0x0040,

            TOPMOST = SWP_NOACTIVATE | SWP_NOOWNERZORDER | SWP_NOSIZE | SWP_NOMOVE | SWP_NOREDRAW | SWP_NOSENDCHANGING,
        }

        // The enum flag for DwmSetWindowAttribute's second parameter, which tells the function what attribute to set.
        public enum DWMWINDOWATTRIBUTE
        {
            DWMWA_WINDOW_CORNER_PREFERENCE = 33
        }

        // The DWM_WINDOW_CORNER_PREFERENCE enum for DwmSetWindowAttribute's third parameter, which tells the function
        // what value of the enum to set.
        public enum DWM_WINDOW_CORNER_PREFERENCE
        {
            DWMWCP_DEFAULT = 0, //让系统决定是否对窗口采用圆角设置
            DWMWCP_DONOTROUND = 1, //绝不对窗口采用圆角设置
            DWMWCP_ROUND = 2, //适当时采用圆角设置
            DWMWCP_ROUNDSMALL = 3  //适当时可采用半径较小的圆角设置
        }

        public enum CacheSlot
        {
            DpiX,

            FocusBorderWidth,
            FocusBorderHeight,
            HighContrast,
            MouseVanish,

            DropShadow,
            FlatMenu,
            WorkAreaInternal,
            WorkArea,

            IconMetrics,

            KeyboardCues,
            KeyboardDelay,
            KeyboardPreference,
            KeyboardSpeed,
            SnapToDefaultButton,
            WheelScrollLines,
            MouseHoverTime,
            MouseHoverHeight,
            MouseHoverWidth,

            MenuDropAlignment,
            MenuFade,
            MenuShowDelay,

            ComboBoxAnimation,
            ClientAreaAnimation,
            CursorShadow,
            GradientCaptions,
            HotTracking,
            ListBoxSmoothScrolling,
            MenuAnimation,
            SelectionFade,
            StylusHotTracking,
            ToolTipAnimation,
            ToolTipFade,
            UIEffects,

            MinimizeAnimation,
            Border,
            CaretWidth,
            ForegroundFlashCount,
            DragFullWindows,
            NonClientMetrics,

            ThinHorizontalBorderHeight,
            ThinVerticalBorderWidth,
            CursorWidth,
            CursorHeight,
            ThickHorizontalBorderHeight,
            ThickVerticalBorderWidth,
            MinimumHorizontalDragDistance,
            MinimumVerticalDragDistance,
            FixedFrameHorizontalBorderHeight,
            FixedFrameVerticalBorderWidth,
            FocusHorizontalBorderHeight,
            FocusVerticalBorderWidth,
            FullPrimaryScreenWidth,
            FullPrimaryScreenHeight,
            HorizontalScrollBarButtonWidth,
            HorizontalScrollBarHeight,
            HorizontalScrollBarThumbWidth,
            IconWidth,
            IconHeight,
            IconGridWidth,
            IconGridHeight,
            MaximizedPrimaryScreenWidth,
            MaximizedPrimaryScreenHeight,
            MaximumWindowTrackWidth,
            MaximumWindowTrackHeight,
            MenuCheckmarkWidth,
            MenuCheckmarkHeight,
            MenuButtonWidth,
            MenuButtonHeight,
            MinimumWindowWidth,
            MinimumWindowHeight,
            MinimizedWindowWidth,
            MinimizedWindowHeight,
            MinimizedGridWidth,
            MinimizedGridHeight,
            MinimumWindowTrackWidth,
            MinimumWindowTrackHeight,
            PrimaryScreenWidth,
            PrimaryScreenHeight,
            WindowCaptionButtonWidth,
            WindowCaptionButtonHeight,
            ResizeFrameHorizontalBorderHeight,
            ResizeFrameVerticalBorderWidth,
            SmallIconWidth,
            SmallIconHeight,
            SmallWindowCaptionButtonWidth,
            SmallWindowCaptionButtonHeight,
            VirtualScreenWidth,
            VirtualScreenHeight,
            VerticalScrollBarWidth,
            VerticalScrollBarButtonHeight,
            WindowCaptionHeight,
            KanjiWindowHeight,
            MenuBarHeight,
            VerticalScrollBarThumbHeight,
            IsImmEnabled,
            IsMediaCenter,
            IsMenuDropRightAligned,
            IsMiddleEastEnabled,
            IsMousePresent,
            IsMouseWheelPresent,
            IsPenWindows,
            IsRemotelyControlled,
            IsRemoteSession,
            ShowSounds,
            IsSlowMachine,
            SwapButtons,
            IsTabletPC,
            VirtualScreenLeft,
            VirtualScreenTop,

            PowerLineStatus,

            IsGlassEnabled,
            UxThemeName,
            UxThemeColor,
            WindowCornerRadius,
            WindowGlassColor,
            WindowGlassBrush,
            WindowNonClientFrameThickness,
            WindowResizeBorderThickness,

            NumSlots
        }

        public enum SM
        {
            CXSCREEN = 0,
            CYSCREEN = 1,
            CXVSCROLL = 2,
            CYHSCROLL = 3,
            CYCAPTION = 4,
            CXBORDER = 5,
            CYBORDER = 6,
            CXFIXEDFRAME = 7,
            CYFIXEDFRAME = 8,
            CYVTHUMB = 9,
            CXHTHUMB = 10,
            CXICON = 11,
            CYICON = 12,
            CXCURSOR = 13,
            CYCURSOR = 14,
            CYMENU = 15,
            CXFULLSCREEN = 16,
            CYFULLSCREEN = 17,
            CYKANJIWINDOW = 18,
            MOUSEPRESENT = 19,
            CYVSCROLL = 20,
            CXHSCROLL = 21,
            DEBUG = 22,
            SWAPBUTTON = 23,
            CXMIN = 28,
            CYMIN = 29,
            CXSIZE = 30,
            CYSIZE = 31,
            CXFRAME = 32,
            CXSIZEFRAME = CXFRAME,
            CYFRAME = 33,
            CYSIZEFRAME = CYFRAME,
            CXMINTRACK = 34,
            CYMINTRACK = 35,
            CXDOUBLECLK = 36,
            CYDOUBLECLK = 37,
            CXICONSPACING = 38,
            CYICONSPACING = 39,
            MENUDROPALIGNMENT = 40,
            PENWINDOWS = 41,
            DBCSENABLED = 42,
            CMOUSEBUTTONS = 43,
            SECURE = 44,
            CXEDGE = 45,
            CYEDGE = 46,
            CXMINSPACING = 47,
            CYMINSPACING = 48,
            CXSMICON = 49,
            CYSMICON = 50,
            CYSMCAPTION = 51,
            CXSMSIZE = 52,
            CYSMSIZE = 53,
            CXMENUSIZE = 54,
            CYMENUSIZE = 55,
            ARRANGE = 56,
            CXMINIMIZED = 57,
            CYMINIMIZED = 58,
            CXMAXTRACK = 59,
            CYMAXTRACK = 60,
            CXMAXIMIZED = 61,
            CYMAXIMIZED = 62,
            NETWORK = 63,
            CLEANBOOT = 67,
            CXDRAG = 68,
            CYDRAG = 69,
            SHOWSOUNDS = 70,
            CXMENUCHECK = 71,
            CYMENUCHECK = 72,
            SLOWMACHINE = 73,
            MIDEASTENABLED = 74,
            MOUSEWHEELPRESENT = 75,
            XVIRTUALSCREEN = 76,
            YVIRTUALSCREEN = 77,
            CXVIRTUALSCREEN = 78,
            CYVIRTUALSCREEN = 79,
            CMONITORS = 80,
            SAMEDISPLAYFORMAT = 81,
            IMMENABLED = 82,
            CXFOCUSBORDER = 83,
            CYFOCUSBORDER = 84,
            TABLETPC = 86,
            MEDIACENTER = 87,
            CXPADDEDBORDER = 92,
            REMOTESESSION = 0x1000,
            REMOTECONTROL = 0x2001
        }

        /// <summary>
        /// 用于在 <see cref="InteropMethods.GetWindowLong"/> 的 int index 传入
        /// </summary>
        /// 代码：[GetWindowLong function (Windows)](https://msdn.microsoft.com/en-us/library/windows/desktop/ms633584(v=vs.85).aspx )
        public enum GetWindowLongFields
        {
            /// <summary>
            /// 设定一个新的扩展风格
            /// Retrieves the extended window styles
            /// </summary>
            GWL_EXSTYLE = -20,

            /// <summary>
            /// 设置一个新的应用程序实例句柄
            /// Retrieves a handle to the application instance
            /// </summary>
            GWL_HINSTANCE = -6,

            /// <summary>
            /// 改变子窗口的父窗口
            /// Retrieves a handle to the parent window, if any
            /// </summary>
            GWL_HWNDPARENT = -8,

            /// <summary>
            ///  设置一个新的窗口标识符
            /// Retrieves the identifier of the window
            /// </summary>
            GWL_ID = -12,

            /// <summary>
            /// 设定一个新的窗口风格
            /// Retrieves the window styles
            /// </summary>
            GWL_STYLE = -16,

            /// <summary>
            /// 设置与窗口有关的32位值。每个窗口均有一个由创建该窗口的应用程序使用的32位值
            /// Retrieves the user data associated with the window. This data is intended for use by the application that created the window. Its value is initially zero
            /// </summary>
            GWL_USERDATA = -21,

            /// <summary>
            /// 为窗口设定一个新的处理函数
            /// Retrieves the address of the window procedure, or a handle representing the address of the window procedure. You must use the CallWindowProc function to call the window procedure
            /// </summary>
            GWL_WNDPROC = -4,
        }

        public enum HookType
        {
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }

        public enum WM
        {
            NULL = 0x0000,
            CREATE = 0x0001,
            DESTROY = 0x0002,
            MOVE = 0x0003,
            SIZE = 0x0005,
            ACTIVATE = 0x0006,
            SETFOCUS = 0x0007,
            KILLFOCUS = 0x0008,
            ENABLE = 0x000A,
            SETREDRAW = 0x000B,
            SETTEXT = 0x000C,
            GETTEXT = 0x000D,
            GETTEXTLENGTH = 0x000E,
            PAINT = 0x000F,
            CLOSE = 0x0010,
            QUERYENDSESSION = 0x0011,
            QUIT = 0x0012,
            QUERYOPEN = 0x0013,
            ERASEBKGND = 0x0014,
            SYSCOLORCHANGE = 0x0015,
            SHOWWINDOW = 0x0018,
            ACTIVATEAPP = 0x001C,
            SETCURSOR = 0x0020,
            MOUSEACTIVATE = 0x0021,
            CHILDACTIVATE = 0x0022,
            QUEUESYNC = 0x0023,
            GETMINMAXINFO = 0x0024,

            WINDOWPOSCHANGING = 0x0046,
            WINDOWPOSCHANGED = 0x0047,

            CONTEXTMENU = 0x007B,
            STYLECHANGING = 0x007C,
            STYLECHANGED = 0x007D,
            DISPLAYCHANGE = 0x007E,
            GETICON = 0x007F,
            SETICON = 0x0080,
            NCCREATE = 0x0081,
            NCDESTROY = 0x0082,
            NCCALCSIZE = 0x0083,
            NCHITTEST = 0x0084,
            NCPAINT = 0x0085,
            NCACTIVATE = 0x0086,
            GETDLGCODE = 0x0087,
            SYNCPAINT = 0x0088,
            NCMOUSEMOVE = 0x00A0,
            NCLBUTTONDOWN = 0x00A1,
            NCLBUTTONUP = 0x00A2,
            NCLBUTTONDBLCLK = 0x00A3,
            NCRBUTTONDOWN = 0x00A4,
            NCRBUTTONUP = 0x00A5,
            NCRBUTTONDBLCLK = 0x00A6,
            NCMBUTTONDOWN = 0x00A7,
            NCMBUTTONUP = 0x00A8,
            NCMBUTTONDBLCLK = 0x00A9,

            SYSKEYDOWN = 0x0104,
            SYSKEYUP = 0x0105,
            SYSCHAR = 0x0106,
            SYSDEADCHAR = 0x0107,
            COMMAND = 0x0111,
            SYSCOMMAND = 0x0112,

            MOUSEMOVE = 0x0200,
            LBUTTONDOWN = 0x0201,
            LBUTTONUP = 0x0202,
            LBUTTONDBLCLK = 0x0203,
            RBUTTONDOWN = 0x0204,
            RBUTTONUP = 0x0205,
            RBUTTONDBLCLK = 0x0206,
            MBUTTONDOWN = 0x0207,
            MBUTTONUP = 0x0208,
            MBUTTONDBLCLK = 0x0209,
            MOUSEWHEEL = 0x020A,
            XBUTTONDOWN = 0x020B,
            XBUTTONUP = 0x020C,
            XBUTTONDBLCLK = 0x020D,
            MOUSEHWHEEL = 0x020E,


            CAPTURECHANGED = 0x0215,

            ENTERSIZEMOVE = 0x0231,
            EXITSIZEMOVE = 0x0232,

            IME_SETCONTEXT = 0x0281,
            IME_NOTIFY = 0x0282,
            IME_CONTROL = 0x0283,
            IME_COMPOSITIONFULL = 0x0284,
            IME_SELECT = 0x0285,
            IME_CHAR = 0x0286,
            IME_REQUEST = 0x0288,
            IME_KEYDOWN = 0x0290,
            IME_KEYUP = 0x0291,

            NCMOUSELEAVE = 0x02A2,

            DESTROYCLIPBOARD = 0x0307,
            DRAWCLIPBOARD = 0x0308,
            PAINTCLIPBOARD = 0x0309,
            VSCROLLCLIPBOARD = 0x030A,
            SIZECLIPBOARD = 0x030B,

            ASKCBFORMATNAME = 0x030C,
            CHANGECBCHAIN = 0x030D,
            HSCROLLCLIPBOARD = 0x030E,
            QUERYNEWPALETTE = 0x030F,
            PALETTEISCHANGING = 0x0310,
            PALETTECHANGED = 0x0311,
            HOTKEY = 0x0312,
            PRINT = 0x0317,
            PRINTCLIENT = 0x0318,
            APPCOMMAND = 0x0319,
            THEMECHANGED = 0x031A,

            DWMCOMPOSITIONCHANGED = 0x031E,
            DWMNCRENDERINGCHANGED = 0x031F,
            DWMCOLORIZATIONCOLORCHANGED = 0x0320,
            DWMWINDOWMAXIMIZEDCHANGE = 0x0321,

            #region Windows 7
            DWMSENDICONICTHUMBNAIL = 0x0323,
            DWMSENDICONICLIVEPREVIEWBITMAP = 0x0326,
            #endregion

            USER = 0x0400,

            // This is the hard-coded message value used by WinForms for Shell_NotifyIcon.
            // It's relatively safe to reuse.
            TRAYMOUSEMESSAGE = 0x800, //WM_USER + 1024
            APP = 0x8000,
        }

        public enum FileInfoFlags : uint
        {
            SHGFI_ICON = 0x000000100,                 //get icon
            SHGFI_DISPLAYNAME = 0x000000200,          //get display name
            SHGFI_TYPENAME = 0x000000400,             //get type name
            SHGFI_ATTRIBUTES = 0x000000800,           //get attributes
            SHGFI_ICONLOCATION = 0x000001000,         //get icon location
            SHGFI_EXETYPE = 0x000002000,              //get exe type
            SHGFI_SYSICONINDEX = 0x000004000,         //get system icon index
            SHGFI_LINKOVERLAY = 0x000008000,          //put a link overlay on icon
            SHGFI_SELECTED = 0x000010000,             //show icon in selected state
            SHGFI_ATTR_SPECIFIED = 0x000020000,       //get only specified attributes
            SHGFI_LARGEICON = 0x000000000,            //get large icon
            SHGFI_SMALLICON = 0x000000001,            //get small icon
            SHGFI_OPENICON = 0x000000002,             //get open icon
            SHGFI_SHELLICONSIZE = 0x000000004,        //get shell size icon
            SHGFI_PIDL = 0x000000008,                 //pszPath is a pidl
            SHGFI_USEFILEATTRIBUTES = 0x0000000010,   //use passed dwFileAttribute
            SHGFI_ADDOVERLAYS = 0x000000020,          //apply the appropriate overlays
            SHGFI_OVERLAYINDEX = 0x000000040          //Get the index of the overlya
        }

        #endregion

        #region Struct

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }

        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        [DebuggerDisplay("X:{Left}, Y:{Top}, Width:{Width}, Height:{Height}")]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(Rect rect)
            {
                Left = (int)rect.Left;
                Top = (int)rect.Top;
                Right = (int)rect.Right;
                Bottom = (int)rect.Bottom;
            }

            public System.Windows.Point Position => new System.Windows.Point(Left, Top);
            public System.Windows.Size Size => new System.Windows.Size(Width, Height);

            public int Height
            {
                get => Bottom - Top;
                set => Bottom = Top + value;
            }

            public int Width
            {
                get => Right - Left;
                set => Right = Left + value;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct APPBARDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public uint uCallbackMessage;
            public uint uEdge;
            public RECT rc;
            public int lParam;
        }

        public struct MONITORINFO
        {
            public uint cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WNDCLASS
        {
            public uint style;
            public Delegate lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszMenuName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszClassName;
        }

        /// <summary>
        /// 包含系统应在指定时间内闪烁窗口次数和闪烁状态的信息
        /// </summary>
        public struct FLASHWINFO
        {
            /// <summary>
            /// 结构大小
            /// </summary>
            public uint cbSize;

            /// <summary>
            /// 要闪烁或停止的窗口句柄
            /// </summary>
            public IntPtr hwnd;

            /// <summary>
            /// 闪烁的类型
            /// </summary>
            public uint dwFlags;

            /// <summary>
            /// 闪烁窗口的次数
            /// </summary>
            public uint uCount;

            /// <summary>
            /// 窗口闪烁的频度，毫秒为单位；若该值为0，则为默认图标的闪烁频度
            /// </summary>
            public uint dwTimeout;
        }

        public enum FlashType : uint
        {
            /// <summary>
            /// 停止闪烁。
            /// </summary>
            FLASHW_STOP = 0,

            /// <summary>
            /// 只闪烁标题。
            /// </summary>
            FALSHW_CAPTION = 1,

            /// <summary>
            /// 只闪烁任务栏。
            /// </summary>
            FLASHW_TRAY = 2,

            /// <summary>
            /// 标题和任务栏同时闪烁。
            /// </summary>
            FLASHW_ALL = 3,

            /// <summary>
            /// 
            /// </summary>
            FLASHW_PARAM1 = 4,

            /// <summary>
            /// 
            /// </summary>
            FLASHW_PARAM2 = 12,

            /// <summary>
            /// 无条件闪烁任务栏直到发送停止标志或者窗口被激活，如果未激活，停止时高亮。
            /// </summary>
            FLASHW_TIMER = FLASHW_TRAY | FLASHW_PARAM1,

            /// <summary>
            /// 未激活时闪烁任务栏直到发送停止标志或者窗体被激活，停止后高亮。
            /// </summary>
            FLASHW_TIMERNOFG = FLASHW_TRAY | FLASHW_PARAM2,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class NOTIFYICONDATA
        {
            public int cbSize = Marshal.SizeOf(typeof(NOTIFYICONDATA));
            public IntPtr hWnd;
            public int uID;
            public int uFlags;
            public int uCallbackMessage;
            public IntPtr hIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szTip = string.Empty;
            public int dwState = 0x01;
            public int dwStateMask = 0x01;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szInfo = string.Empty;
            public int uTimeoutOrVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szInfoTitle = string.Empty;
            public int dwInfoFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CHANGEFILTERSTRUCT
        {
            public uint cbSize;
            public uint ExtStatus;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct QUERY_SERVICE_CONFIG
        {
            public uint dwServiceType;
            public uint dwStartType;
            public uint dwErrorControl;
            public IntPtr lpBinaryPathName; //服务的可执行文件路径
            public IntPtr lpLoadOrderGroup;
            public uint dwTagId;
            public IntPtr lpDependencies;
            public IntPtr lpServiceStartName;
            public IntPtr lpDisplayName;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SERVICE_STATUS_PROCESS
        {
            public uint dwServiceType;
            public uint dwCurrentState;
            public uint dwControlsAccepted;
            public uint dwWin32ExitCode;
            public uint dwServiceSpecificExitCode;
            public uint dwCheckPoint;
            public uint dwWaitHint;
            public uint dwProcessId;
            public uint dwServiceFlags;
        }

        #endregion
    }
}
