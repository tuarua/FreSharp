using FREObject = System.IntPtr;
using Rect = System.Windows.Rect;

// ReSharper disable InconsistentNaming
namespace TuaRua.FreSharp.Geom {
    /// <summary>
    /// 
    /// </summary>
    public static class FreRect {
        /// <summary>
        /// Converts a C# Rect to a FREObject
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this Rect value) {
            return new FREObject().Init("flash.geom.Rectangle", value.X, value.Y, value.Width, value.Height);
        }

        /// <summary>
        /// Converts a FREObject to a C# Rect
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static Rect AsRect(this FREObject inFre) {
            dynamic fre = new FreObjectSharp(inFre);
            return new Rect(fre.x, fre.y, fre.width, fre.height);
        }
    }
}