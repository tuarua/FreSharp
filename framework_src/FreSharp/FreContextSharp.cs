using System;
using FREContext = System.IntPtr;

namespace TuaRua.FreSharp {
    /// <summary>
    /// 
    /// </summary>
    public class FreContextSharp {
        private FREContext _rawValue = FREContext.Zero;

        //public FREContext RawValue { get; set; } = FREContext.Zero;
        /// <summary>
        /// Creates an empty C# FREContext
        /// </summary>
        public FreContextSharp() { }

        /// <summary>
        /// Creates a C# FREObject from a C FREObject
        /// </summary>
        /// <param name="freContext"></param>
        public FreContextSharp(FREContext freContext) {
            _rawValue = freContext;
        }

        [Obsolete("SendEvent is deprecated, please use DispatchEvent instead.", true)]
        public void SendEvent(string name, string value) {
            FreSharpHelper.DispatchEvent(ref _rawValue, name, value);
        }

        /// <summary>
        /// Dispatches an event. Mimics FREDispatchStatusEventAsync
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void DispatchEvent(string name, string value)
        {
            FreSharpHelper.DispatchEvent(ref _rawValue, name, value);
        }
        

    }
}