using FREObject = System.IntPtr;
using Point = System.Windows.Point;

namespace TuaRua.FreSharp.Geom {
    /// <summary>
    /// 
    /// </summary>
    public static class FrePoint {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Converts a C# Point to a FREObject
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this Point value) {
            return new FREObject().Init("flash.geom.Point", value.X, value.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static Point AsPoint(this FREObject inFre) => new Point(
            inFre.GetProp("x").AsDouble(),
            inFre.GetProp("y").AsDouble());
    }
}