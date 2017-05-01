using System;
using System.Collections;
using FREObject = System.IntPtr;
namespace FreSharp.Exceptions {
    /// <summary>
    /// 
    /// </summary>
    public class FreException {
        private readonly FreObjectSharp _aneError;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public FreException(Exception e) {
            _aneError = new FreObjectSharp("com.tuarua.ANEError", new ArrayList
            {
                new FreObjectSharp(e.Message),
                new FreObjectSharp(0),
                new FreObjectSharp(e.GetType().ToString()),
                new FreObjectSharp(e.Source),
                new FreObjectSharp(e.StackTrace)
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FREObject Get() {
            return _aneError.Get();
        }
    }
}