using FREObject = System.IntPtr;
using Rect = System.Windows.Rect;

namespace TuaRua.FreSharp.Geom {
    /// <summary>
    /// 
    /// </summary>
    public static class FreRectangle {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static FREObject ToFREObject(this Rect value) {
            return new FREObject().Init("flash.geom.Rectangle", value.X, value.Y, value.Width, value.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static Rect AsRect(this FREObject inFre) => new Rect(
            inFre.GetProp("x").AsDouble(),
            inFre.GetProp("y").AsDouble(),
            inFre.GetProp("width").AsDouble(),
            inFre.GetProp("height").AsDouble()
        );
    }
}