using System;
using System.Windows;
using System.Windows.Media;
using TuaRua.FreSharp.Utils;
using Image = System.Windows.Controls.Image;
using FREObject = System.IntPtr;

namespace TuaRua.FreSharp.Display {
    /// <summary>
    /// 
    /// </summary>
    public class FreNativeImage : Image {
        /// <summary>
        /// 
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Y { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public FreNativeImage() { }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="freObjectSharp"></param>
        public FreNativeImage(FreObjectSharp freObjectSharp) {
            var bitmap = new FreBitmapDataSharp(freObjectSharp.GetProperty("bitmapData").RawValue).GetAsBitmap();
            Width = bitmap.Width;
            Height = bitmap.Height;
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Source = BitmapUtils.BitmapToSource(bitmap);

            X = Convert.ToDouble(freObjectSharp.GetProperty("x").Value);
            Y = Convert.ToDouble(freObjectSharp.GetProperty("y").Value);
            Visibility = Convert.ToBoolean(freObjectSharp.GetProperty("visible").Value)
                ? Visibility.Visible
                : Visibility.Hidden;
            RenderTransform = new TranslateTransform(X, Y);
            Opacity = Convert.ToDouble(freObjectSharp.GetProperty("alpha").Value);
        }
    }
}