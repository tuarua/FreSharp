using System;
using FREObject = System.IntPtr;
namespace TuaRua.FreSharp.Exceptions {
    /// <summary>
    /// 
    /// </summary>
    public class FreException {
        /// <summary>
        /// Returns a ANEError as a FREObject
        /// </summary>
        public FREObject RawValue { get; }

        /// <summary>
        /// Creates a FreException from a C# Exception
        /// </summary>
        /// <param name="e"></param>
        public FreException(Exception e) {
            RawValue = new FREObject().Init("com.tuarua.fre.ANEError", e.Message, 0, e.GetType().ToString(), e.Source, e.StackTrace);
        }

    }
}