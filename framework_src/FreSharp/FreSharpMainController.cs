using System;
using System.Collections.Generic;
using System.Linq;
using FREObject = System.IntPtr;
using FREContext = System.IntPtr;

namespace TuaRua.FreSharp { 
    /// <summary>
    /// 
    /// </summary>
    public abstract class FreSharpMainController {
        /// <summary>
        /// 
        /// </summary>
        public abstract void OnFinalize();
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, Func<FREContext, uint, FREObject[], FREObject>> FunctionsDict;

        /// <summary>
        /// 
        /// </summary>
        public FreContextSharp Context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ctx"></param>
        /// <param name="argc"></param>
        /// <param name="argv"></param>
        /// <returns></returns>
        public FREObject CallSharpFunction(string name, ref FREContext ctx, uint argc, FREObject[] argv) {
            return FunctionsDict[name].Invoke(ctx, argc, argv);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="freContext"></param>
        public void SetFreContext(ref FREContext freContext) {
            Context = new FreContextSharp(freContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        public void Trace(params object[] values) {
            var traceStr = values.Aggregate("", (current, value) => current + value + " ");
            Context?.SendEvent("TRACE", traceStr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SendEvent(string name, string value) {
            Context?.SendEvent(name, value);
        }

    }
}
