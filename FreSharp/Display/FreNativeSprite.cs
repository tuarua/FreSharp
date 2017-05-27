using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FREObject = System.IntPtr;
namespace TuaRua.FreSharp.Display {
    class FreNativeSprite : Canvas {

        public FreNativeSprite(FreObjectSharp freObjectSharp) {
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;

            X = Convert.ToDouble(freObjectSharp.GetProperty("x").Value);
            Y = Convert.ToDouble(freObjectSharp.GetProperty("y").Value);
            Visibility = Convert.ToBoolean(freObjectSharp.GetProperty("visible").Value)
                ? Visibility.Visible
                : Visibility.Hidden;
            RenderTransform = new TranslateTransform(X, Y);
            Opacity = Convert.ToDouble(freObjectSharp.GetProperty("alpha").Value);
        }

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
        /// <param name="child"></param>
        public void AddChild(UIElement child) {
            Children.Add(child);
        }

        public void Update(FREObject prop, FREObject value) {
            var propName = Convert.ToString(new FreObjectSharp(prop).Value);
            if (propName == "x") {
                X = Convert.ToDouble(new FreObjectSharp(value).Value);
                RenderTransform = new TranslateTransform(X, Y);
            } else if (propName == "y") {
                Y = Convert.ToDouble(new FreObjectSharp(value).Value);
                RenderTransform = new TranslateTransform(X, Y);
            } else if (propName == "alpha") {
                Opacity = Convert.ToDouble(new FreObjectSharp(value).Value);
            } else if (propName == "visible") {
                Visibility = Convert.ToBoolean(new FreObjectSharp(value).Value)
                    ? Visibility.Visible
                    : Visibility.Hidden;
            }
        }
    }
}