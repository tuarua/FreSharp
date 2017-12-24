using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TuaRua.FreSharp;
using TuaRua.FreSharp.Exceptions;
using FREObject = System.IntPtr;
using FREContext = System.IntPtr;
using Hwnd = System.IntPtr;

namespace HelloWorldLib {
   public class MainController : FreSharpMainController {
        private Hwnd _airWindow;

        // Must have this function. It exposes the methods to our entry C++.
        public string[] GetFunctions() {
            FunctionsDict =
                new Dictionary<string, Func<FREObject, uint, FREObject[], FREObject>> {
                     {"init", InitController}
                    ,{ "sayHello", SayHello}


                };
            return FunctionsDict.Select(kvp => kvp.Key).ToArray();
        }

        public FREObject SayHello(FREContext ctx, uint argc, FREObject[] argv) {
            if (argv[0] == FREObject.Zero) return FREObject.Zero;
            if (argv[1] == FREObject.Zero) return FREObject.Zero;
            if (argv[2] == FREObject.Zero) return FREObject.Zero;

            try {
                var myString = argv[0].AsString();
                var uppercase = argv[1].AsBool();
                var numRepeats = argv[2].AsInt();

                for (int i = 0; i < numRepeats; i++) {
                    Trace(i);
                }

                var ret = myString;
                if (uppercase) {
                    ret = ret.ToUpper();
                }

                SendEvent("MY_EVENT", "ok");


                return ret.ToFREObject();

            } catch (Exception e) {
                return new FreException(e).RawValue; //return as3 error and throw in swc
            }

        }

        public FREObject InitController(FREContext ctx, uint argc, FREObject[] argv) {
            // get a reference to the AIR Window HWND
            _airWindow = Process.GetCurrentProcess().MainWindowHandle;
            return FREObject.Zero;
        }

       public override void OnFinalize() {
           
       }
   }


}
