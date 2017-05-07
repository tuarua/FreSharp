using System;
using System.Collections.Generic;
using TuaRua.FreSharp;
using FREObject = System.IntPtr;
using FREContext = System.IntPtr;

namespace FreExampleSharpLib {
    public abstract class FreSharpController {
        public Dictionary<string, Func<FREContext, uint, FREObject[], FREObject>> FunctionsDict;

        public FREObject CallSharpFunction(string name, ref FREContext ctx, uint argc, FREObject[] argv) {
            return FunctionsDict[name].Invoke(ctx, argc, argv);
        }

        public void SetFreContext(ref FREContext freContext) {
            FreSharpHelper.SetFreContext(ref freContext);
        }

        public void Trace(string value) {
            FreSharpHelper.DispatchEvent("TRACE", value);
        }

    }
}
