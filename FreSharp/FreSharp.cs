using System;
using System.Collections;
using System.Linq;
using FRESharpCore;
using FREContextSharp = System.IntPtr;
namespace FreSharp {
    public class FreSharpHelper {
        public static FRESharpCLR Core = new FRESharpCLR();

        public void SetFreContext(ref FREContextSharp freContext) {
            Core.setFREContext(freContext);
        }

        public void DispatchEvent(string name, string value) {
           Core.dispatchEvent(name, value);
        }

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

        public static uint GetArgsC(ArrayList args) {
            uint cnt = 0;
            if (args != null) {
                cnt = (uint)args.Count;
            }
            return cnt;
        }
    }
}
