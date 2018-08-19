using FREObject = System.IntPtr;
using Point = System.Windows.Point;

// ReSharper disable InconsistentNaming

namespace TuaRua.FreSharp.Geom {
    /// <summary>
    /// 
    /// </summary>
    public static class FrePoint {
        /// <summary>
        /// Converts a C# Point to a FREObject
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this Point value) {
            return new FREObject().Init("flash.geom.Point", value.X, value.Y);
        }

        /// <summary>
        /// Converts a FREObject to a C# Point
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static Point AsPoint(this FREObject inFre) {
            dynamic fre = new FreObjectSharp(inFre);
            return new Point(fre.x, fre.y);
        }
    }
}