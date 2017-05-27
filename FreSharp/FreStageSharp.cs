using System;
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media;
using TuaRua.FreSharp.Geom;
using TuaRua.FreSharp.Utils;
using Hwnd = System.IntPtr;
using FREObject = System.IntPtr;
using FREContext = System.IntPtr;
using Color = System.Windows.Media.Color;

namespace TuaRua.FreSharp {
    /// <summary>
    /// 
    /// </summary>
    public class FreStageSharp {
        private static Rectangle _viewPort;
        private static bool _visible;

        /// <summary>
        /// 
        /// </summary>
        public static bool Transparent;

        private static Hwnd _childWindow;
        private static Hwnd _airWindow;
        private static Visual _rootView;
        private static HwndSourceParameters _parameters;
        private static bool _isAdded;

        /// <summary>
        /// 
        /// </summary>
        public static Color BackgroundColor;

        /// <summary>
        /// 
        /// </summary>
        public enum FreNativeType {
            /// <summary>
            /// 
            /// </summary>
            Image = 0,
            /// <summary>
            /// 
            /// </summary>
            Button,
            /// <summary>
            /// 
            /// </summary>
            Sprite
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="argc"></param>
        /// <param name="argv"></param>
        /// <returns></returns>
        public static FREObject Init(FREContext ctx, uint argc, FREObject[] argv) {
            var inFre0 = argv[0];
            var inFre1 = argv[1];
            var inFre2 = argv[2];
            var inFre3 = argv[3];
            if (inFre0 == FREObject.Zero) return FREObject.Zero;
            if (inFre1 == FREObject.Zero) return FREObject.Zero;
            if (inFre2 == FREObject.Zero) return FREObject.Zero;
            if (inFre3 == FREObject.Zero) return FREObject.Zero;

            var rgb = FreSharpHelper.GetAsUInt(new FreObjectSharp(inFre3).RawValue);
            BackgroundColor = Color.FromRgb(
                Convert.ToByte((rgb >> 16) & 0xff),
                Convert.ToByte((rgb >> 8) & 0xff),
                Convert.ToByte((rgb >> 0) & 0xff));

            _viewPort = new FreRectangleSharp(inFre0).Value;
            _visible = Convert.ToBoolean(new FreObjectSharp(inFre1).Value);
            Transparent = Convert.ToBoolean(new FreObjectSharp(inFre2).Value);
            _airWindow =
                System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle; //get the reference to the AIR Window

            _parameters = new HwndSourceParameters();
            _parameters.SetPosition(_viewPort.X, _viewPort.Y);
            _parameters.SetSize(_viewPort.Width, _viewPort.Height);
            _parameters.ParentWindow = _airWindow;
            _parameters.WindowName = "AIR Native Stage Window";
            _parameters.WindowStyle = _visible
                ? (int) (WindowStyles.WS_CHILD | WindowStyles.WS_VISIBLE)
                : (int) WindowStyles.WS_CHILD;

            if (Transparent && WinApi.GetOsVersion().Item1 > 7) {
                _parameters.ExtendedWindowStyle = (int) WindowExStyles.WS_EX_LAYERED;
                _parameters.UsesPerPixelTransparency = true;
            }

            _parameters.AcquireHwndFocusInMenuMode = false; //TODO ??
            return FREObject.Zero;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="argc"></param>
        /// <param name="argv"></param>
        /// <returns></returns>
        public static FREObject AddRoot(FREContext ctx, uint argc, FREObject[] argv) {
            if (_isAdded) return FREObject.Zero;
            var nativeRoot = new FreNativeRoot();
            if (!Transparent) {
                nativeRoot.Background = new SolidColorBrush(BackgroundColor);
            }
            _rootView = nativeRoot;
            var source = new HwndSource(_parameters) {RootVisual = _rootView};
            _childWindow = source.Handle;
            WinApi.RegisterTouchWindow(_childWindow, TouchWindowFlags.TWF_WANTPALM);

            nativeRoot.Init();
            return FREObject.Zero;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="argc"></param>
        /// <param name="argv"></param>
        /// <returns></returns>
        public static FREObject Update(FREContext ctx, uint argc, FREObject[] argv) {
            var inFre0 = argv[0];
            if (inFre0 == FREObject.Zero) return FREObject.Zero;

            var inFre1 = argv[1];
            if (inFre1 == FREObject.Zero) return FREObject.Zero;

            var propName = Convert.ToString(new FreObjectSharp(inFre0).Value);

            if (propName == "visible") {
                SetVisibility(Convert.ToBoolean(new FreObjectSharp(inFre1).Value));
            }
            else if (propName == "viewPort") {
                SetViewPort(new FreRectangleSharp(inFre1).Value);
            }

            return FREObject.Zero;
        }

        private static void SetViewPort(Rectangle rect) {
            var tmpX = rect.X;
            var tmpY = rect.Y;
            var tmpWidth = rect.Width;
            var tmpHeight = rect.Height;

            var updateWidth = false;
            var updateHeight = false;
            var updateX = false;
            var updateY = false;

            if (tmpWidth != _viewPort.Width) {
                _viewPort.Width = tmpWidth;
                updateWidth = true;
            }

            if (tmpHeight != _viewPort.Height) {
                _viewPort.Height = tmpHeight;
                updateHeight = true;
            }

            if (tmpX != _viewPort.X) {
                _viewPort.X = tmpX;
                updateX = true;
            }

            if (tmpY != _viewPort.Y) {
                _viewPort.Y = tmpY;
                updateY = true;
            }

            if (!updateX && !updateY && !updateWidth && !updateHeight) return;
            var flgs = (WindowPositionFlags) 0;
            if (!updateWidth && !updateHeight) {
                flgs |= WindowPositionFlags.SWP_NOSIZE;
            }
            if (!updateX && !updateY) {
                flgs |= WindowPositionFlags.SWP_NOMOVE;
            }
            WinApi.SetWindowPos(_childWindow, new Hwnd(0), _viewPort.X, _viewPort.Y, _viewPort.Width, _viewPort.Height,
                flgs);
            WinApi.UpdateWindow(_childWindow);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static void SetVisibility(bool value) {
            var existing = _visible;
            _visible = value;
            if (existing == _visible) return;
            WinApi.ShowWindow(_childWindow, _visible ? ShowWindowCommands.SW_SHOWNORMAL : ShowWindowCommands.SW_HIDE);
            WinApi.UpdateWindow(_childWindow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Visual GetRootView() {
            return _rootView;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsAdded() {
            return _isAdded;
        }
    }
}