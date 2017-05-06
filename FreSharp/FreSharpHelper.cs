using System;
using System.Collections;
using System.Linq;
using FRESharpCore;
using FREContextSharp = System.IntPtr;
namespace FreSharp {
    /// <summary>
    /// Creates a new FreSharp Helper
    /// </summary>
    public class FreSharpHelper {
        /// <summary>
        /// 
        /// </summary>
        public static FRESharpCLR Core = new FRESharpCLR();

        /// <summary>
        /// Sets the C# FreContext to use to dispatch events.
        /// </summary>
        /// <param name="freContext"></param>
        public void SetFreContext(ref FREContextSharp freContext) {
            Core.setFREContext(freContext);
        }

        /// <summary>
        /// Dispatches an event. Mimics FREDispatchStatusEventAsync
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void DispatchEvent(string name, string value) {
            Core.dispatchEvent(name, value);
        }

        /// <summary>
        /// Converts ArrayList into argv which can be passed to library calls
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static FREContextSharp[] ArgsToArgv(ArrayList args) {
            var cnt = GetArgsC(args);
            var arr = new IntPtr[cnt];
            if (args == null) return arr;
            var argArr = args.ToArray();
            uint i;
            for (i = 0; i < cnt; ++i) {
                var fre = argArr.ElementAt((int)i) as FreObjectSharp;
                if (fre == null) break;
                arr.SetValue(fre.Get(), i);
            }
            return arr;
        }

        /// <summary>
        /// Gets length of ArrayList as argc which can be passed to library calls
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static uint GetArgsC(ArrayList args) {
            uint cnt = 0;
            if (args != null) {
                cnt = (uint)args.Count;
            }
            return cnt;
        }
    }
}
