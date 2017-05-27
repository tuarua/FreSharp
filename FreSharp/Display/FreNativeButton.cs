using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TuaRua.FreSharp.Utils;
using Image = System.Windows.Controls.Image;
using FREObject = System.IntPtr;
using FREContext = System.IntPtr;

namespace TuaRua.FreSharp.Display {
    /// <summary>
    /// 
    /// </summary>
    public class FreNativeButton : Image {
        /// <summary>
        /// 
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Y { get; set; }

        private readonly BitmapSource _upState;
        private readonly BitmapSource _overState;
        private readonly BitmapSource _downState;

        private readonly string _id;
        private const string AsCallbackEvent = "TRFRESHARP.as.CALLBACK";
        private FREContext _ctx;

        /// <summary>
        /// 
        /// </summary>
        public FreNativeButton() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="freObjectSharp"></param>
        /// <param name="id"></param>
        /// <param name="ctx"></param>
        public FreNativeButton(FreObjectSharp freObjectSharp, string id, ref FREContext ctx) {
            _ctx = ctx;
            _upState = BitmapUtils.BitmapToSource(
                new FreBitmapDataSharp(freObjectSharp.GetProperty("upStateData").RawValue).GetAsBitmap());
            _overState =
                BitmapUtils.BitmapToSource(new FreBitmapDataSharp(freObjectSharp.GetProperty("overStateData").RawValue)
                    .GetAsBitmap());
            _downState =
                BitmapUtils.BitmapToSource(new FreBitmapDataSharp(freObjectSharp.GetProperty("downStateData").RawValue)
                    .GetAsBitmap());
            _id = id;

            Width = _upState.Width;
            Height = _upState.Height;
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Source = _upState;
            MouseEnter += Button_MouseEnter;
            MouseLeave += Button_MouseLeave;
            MouseDown += Button_MouseDown;
            MouseUp += Button_MouseUp;

            X = Convert.ToDouble(freObjectSharp.GetProperty("x").Value);
            Y = Convert.ToDouble(freObjectSharp.GetProperty("y").Value);
            Visibility = Convert.ToBoolean(freObjectSharp.GetProperty("visible").Value)
                ? Visibility.Visible
                : Visibility.Hidden;
            RenderTransform = new TranslateTransform(X, Y);
            Opacity = Convert.ToDouble(freObjectSharp.GetProperty("alpha").Value);
        }

        private void Button_MouseUp(object sender, MouseButtonEventArgs e) {
            var sf = $"{{\"id\": \"{_id}\", \"event\": \"mouseUp\"}}";
            FreSharpHelper.DispatchEvent(ref _ctx, AsCallbackEvent, sf);

            sf = $"{{\"id\": \"{_id}\", \"event\": \"click\"}}";
            FreSharpHelper.DispatchEvent(ref _ctx, AsCallbackEvent, sf);

            Source = _overState;
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e) {
            var sf = $"{{\"id\": \"{_id}\", \"event\": \"mouseDown\"}}";
            FreSharpHelper.DispatchEvent(ref _ctx, AsCallbackEvent, sf);
            Source = _downState;
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e) {
            var sf = $"{{\"id\": \"{_id}\", \"event\": \"mouseOver\"}}";
            FreSharpHelper.DispatchEvent(ref _ctx, AsCallbackEvent, sf);
            Source = _overState;
            Cursor = Cursors.Hand;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e) {
            var sf = $"{{\"id\": \"{_id}\", \"event\": \"mouseOut\"}}";
            FreSharpHelper.DispatchEvent(ref _ctx, AsCallbackEvent, sf);
            Source = _upState;
            Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        public void Update(FREObject prop, FREObject value) {
            var propName = Convert.ToString(new FreObjectSharp(prop).Value);
            if (propName == "x") {
                X = Convert.ToDouble(new FreObjectSharp(value).Value);
                RenderTransform = new TranslateTransform(X, Y);
            }
            else if (propName == "y") {
                Y = Convert.ToDouble(new FreObjectSharp(value).Value);
                RenderTransform = new TranslateTransform(X, Y);
            }
            else if (propName == "alpha") {
                Opacity = Convert.ToDouble(new FreObjectSharp(value).Value);
            }
            else if (propName == "visible") {
                Visibility = Convert.ToBoolean(new FreObjectSharp(value).Value)
                    ? Visibility.Visible
                    : Visibility.Hidden;
            }
        }
    }
}