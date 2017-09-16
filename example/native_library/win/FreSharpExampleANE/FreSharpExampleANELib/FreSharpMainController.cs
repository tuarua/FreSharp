using System;
using System.Collections.Generic;
using System.Linq;
using TuaRua.FreSharp;
using FREObject = System.IntPtr;
using FREContext = System.IntPtr;

namespace FreExampleSharpLib {
    //TODO Can I move this into FreSharp
    public abstract class FreSharpMainController {
        public Dictionary<string, Func<FREContext, uint, FREObject[], FREObject>> FunctionsDict;
        protected FreContextSharp Context;
        public FREObject CallSharpFunction(string name, ref FREContext ctx, uint argc, FREObject[] argv) {
            return FunctionsDict[name].Invoke(ctx, argc, argv);
        }

        public void SetFreContext(ref FREContext freContext) {
            Context = new FreContextSharp(freContext);
        }

        public void Trace(params object[] values) {
            var traceStr = values.Aggregate("", (current, value) => current + value + " ");
            Context.SendEvent("TRACE", traceStr);
        }

        public void SendEvent(string name, string value) {
            Context.SendEvent(name, value);
        }

    }
}
