using System;
using System.Management;
using System.Runtime.InteropServices;
using Hwnd = System.IntPtr;
#pragma warning disable 1591

namespace TuaRua.FreSharp.Utils {
    /// <summary>
    /// 
    /// </summary>
    public static class WinApi {
        private const string User32 = "user32";
        private const string Kernel32 = "kernel32";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Tuple<int, int> GetOsVersion() {
            var result = new Tuple<int, int>(0, 0);
            var searcher = new ManagementObjectSearcher("SELECT Version FROM Win32_OperatingSystem");
            const char delimiter = '.';
            foreach (var o in searcher.Get()) {
                var os = (ManagementObject)o;
                var version = os["Version"].ToString();
                var substrings = version.Split(delimiter);
                return new Tuple<int, int>(Convert.ToInt32(substrings[0]), Convert.ToInt32(substrings[1]));
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nCmdShow"></param>
        /// <returns></returns>
        [DllImport(User32, ExactSpelling = true)]
        public static extern bool ShowWindow(Hwnd hwnd, ShowWindowCommands nCmdShow);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport(User32, ExactSpelling = true)]
        public static extern bool UpdateWindow(Hwnd hwnd);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="hWndInsertAfter"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport(User32, ExactSpelling = true)]
        public static extern bool SetWindowPos(Hwnd hwnd, Hwnd hWndInsertAfter, int x, int y, int cx, int cy,
            WindowPositionFlags flags);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport(User32, ExactSpelling = true)]
        public static extern bool RegisterTouchWindow(Hwnd hwnd, TouchWindowFlags flags);


        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]

        public struct Rect {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport(User32, ExactSpelling = true)]
        public static extern Hwnd GetWindowRect(Hwnd hWnd, ref Rect rect);

        [DllImport(User32, ExactSpelling = true)]
        public static extern bool SetLayeredWindowAttributes(Hwnd hwnd, uint crKey, byte bAlpha,
            LayeredWindowAttributeFlag dwFlags);

        [DllImport(User32)]
        public static extern int GetWindowLong(Hwnd hwnd, int nIndex);
    }

    [Flags]
    public enum WindowLongFlags {
        GWL_EXSTYLE = -20,
    }

    [Flags]
    public enum ShowWindowCommands {
        SW_FORCEMINIMIZE = 11,
        SW_HIDE = 0,
        SW_MAXIMIZE = 3,
        SW_MINIMIZE = 6,
        SW_RESTORE = 9,
        SW_SHOW = 5,
        SW_SHOWDEFAULT = 10,
        SW_SHOWMAXIMIZED = 3,
        SW_SHOWMINIMIZED = 2,
        SW_SHOWMINNOACTIVE = 7,
        SW_SHOWNA = 8,
        SW_SHOWNOACTIVATE = 4,
        SW_SHOWNORMAL = 1
    }

    [Flags]
    public enum WindowStyles {
        WS_CHILD = 0x40000000,
        WS_VISIBLE = 0x10000000,
    }

    [Flags]
    public enum WindowExStyles {
        WS_EX_LAYERED = 0x00080000,
    }

    [Flags]
    public enum LayeredWindowAttributeFlag {
        LWA_ALPHA = 0x00000002,
        LWA_COLORKEY = 0x00000001
    }

    [Flags]
    public enum WindowPositionFlags {
        SWP_ASYNCWINDOWPOS = 0x4000,
        SWP_DEFERERASE = 0x2000,
        SWP_DRAWFRAME = 0x0020,
        SWP_FRAMECHANGED = 0x0020,
        SWP_HIDEWINDOW = 0x0080,
        SWP_NOACTIVATE = 0x0010,
        SWP_NOCOPYBITS = 0x0100,
        SWP_NOMOVE = 0x0002,
        SWP_NOOWNERZORDER = 0x0200,
        SWP_NOREDRAW = 0x0008,
        SWP_NOREPOSITION = 0x0200,
        SWP_NOSENDCHANGING = 0x0400,
        SWP_NOSIZE = 0x0001,
        SWP_NOZORDER = 0x0004,
        SWP_SHOWWINDOW = 0x0040
    }

    [Flags]
    public enum TouchWindowFlags {
        TWF_FINETOUCH = 0x00000001,
        TWF_WANTPALM = 0x00000002
    }
}
