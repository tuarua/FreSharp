using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using TuaRua.FreSharp;
using TuaRua.FreSharp.Display;
using TuaRua.FreSharp.Exceptions;
using TuaRua.FreSharp.Geom;
using FREObject = System.IntPtr;
using FREContext = System.IntPtr;
using Point = System.Windows.Point;

namespace FreExampleSharpLib {
    public class MainController : FreSharpMainController {
        public string[] GetFunctions() {
            FunctionsDict =
                new Dictionary<string, Func<FREObject, uint, FREObject[], FREObject>> {
                    {"runStringTests", RunStringTests},
                    {"runNumberTests", RunNumberTests},
                    {"runIntTests", RunIntTests},
                    {"runArrayTests", RunArrayTests},
                    {"runObjectTests", RunObjectTests},
                    {"runExtensibleTests", RunExtensibleTests},
                    {"runBitmapTests", RunBitmapTests},
                    {"runByteArrayTests", RunByteArrayTests},
                    {"runErrorTests", RunErrorTests},
                    {"runDataTests", RunDataTests},
                    {"runDateTests", RunDateTests},
                    {"runColorTests", RunColorTests},
                };
            return FunctionsDict.Select(kvp => kvp.Key).ToArray();
        }

        private FREObject RunStringTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace(@"***********Start String test***********");
            if (argv[0] == FREObject.Zero) return FREObject.Zero;

            FreSharpLogger.GetInstance().Context = Context;

            var airString = argv[0].AsString();
            Trace("String passed from AIR:", airString);
            DispatchEvent("MY_EVENT", "this is a test");

            const string sharpString = "I am a string from C# with UTF-8: Björk Guðmundsdóttir " +
                                       "Sinéad O’Connor 久保田  利伸 Михаил Горбачёв Садриддин Айнӣ " +
                                       "Tor Åge Bringsværd 章子怡 €";
            return sharpString.ToFREObject();
        }

        private FREObject RunNumberTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Number test***********");
            if (argv[0] == FREObject.Zero) return FREObject.Zero;
            var airDouble = argv[0].AsDouble();
            const double testDouble = 31.99;
            Trace("Number passed from AIR as Double:", testDouble.Equals(airDouble) ? "PASS" : "FAIL", airDouble);

            const double sharpDouble = 34343.31;
            return sharpDouble.ToFREObject();
        }

        private FREObject RunIntTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Int Uint test***********");
            if (argc < 2) return FREObject.Zero;
            if (argv[0] == FREObject.Zero) return FREObject.Zero;
            if (argv[1] == FREObject.Zero) return FREObject.Zero;
            var airInt = argv[0].AsInt();
            var airUint = argv[1].AsUInt();

            const int testInt = -54;
            const uint testUInt = 66;

            Trace("Number passed from AIR as Int:", testInt.Equals(airInt) ? "PASS" : "FAIL", testInt);
            Trace("Number passed from AIR as UInt:", testUInt.Equals(airUint) ? "PASS" : "FAIL", testUInt);

            const int sharpInt = -666;
            return sharpInt.ToFREObject();
        }

        private FREObject RunArrayTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Array test***********");
            if (argc < 4) {
                return new FreException("Not enough args").RawValue;
            }

            var inFre0 = new FREArray(argv[0]);
            var inFre1 = new FREArray(argv[1]);
            var inFre2 = new FREArray(argv[2]);
            var inFre3 = new FREArray(argv[3]);

            var airArray = inFre0.ToArrayList();
            var airArrayLen = inFre0.Length;

            inFre0[1] = 123.ToFREObject();
            foreach (var fre in inFre0) {
                Trace("iterate over FREArray", fre.AsInt());
            }

            var airVectorString = inFre1.AsStringArray();
            var airVectorNumber = inFre2.AsDoubleArray();
            var airVectorBoolean = inFre3.AsBoolArray();

            Trace("Array passed from AIR:", string.Join(",", airArray.ToArray()));
            Trace("AIR Array length:", airArrayLen);

            Trace("Vector.<String> passed from AIR:", string.Join(",", airVectorString.ToArray()));
            Trace("Vector.<Number> passed from AIR:", string.Join(",", airVectorNumber.ToArray()));
            Trace("Vector.<Boolean> passed from AIR:", string.Join(",", airVectorBoolean.ToArray()));

            var newFreArray = new FREArray("Object", 5, true);
            Trace("New Array of Objects should be 5?", newFreArray.Length);

            var itemZero = inFre0[0];
            var itemZeroVal = itemZero.AsInt();

            Trace("AIR Array item 0 before change:", itemZeroVal);

            inFre0.Set(0, 56);

            var marks = new[] {99, 98, 92, 97, 95};
            return marks.ToFREObject();
        }

        private FREObject RunObjectTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Object test***********");
            var person = argv[0];
            if (person == FREObject.Zero) return FREObject.Zero;

            var newPerson = new FREObject().Init("com.tuarua.Person");
            Trace("newPerson.ToString()", newPerson.toString());
            Trace("newPerson.hasOwnProperty('name')", newPerson.hasOwnProperty("name"));
            Trace("We created a new person. type =", newPerson.Type());

            var oldAge = person.GetProp("age").AsInt();
            var newAge = oldAge + 10;
            person.SetProp("age", newAge);

            var personType = person.Type();
            Trace($"person type is: {personType}");
            Trace("current person age is: ", oldAge);
            var addition = person.Call("add", 100, 33);
            Trace("result is: ", addition.AsInt());

            try {
                var dictionary = person.AsDictionary();
                if (dictionary == null) return person;
                var city = (Dictionary<string, object>) dictionary["city"];
                if (city == null) return person;
                var name = city["name"];
                Trace("what is the city name: ", name);

                dynamic sharpPerson = new FreObjectSharp("com.tuarua.Person", "Ben McBobster", 80);
                Trace("sharpPerson.RawValue.ToString()",
                    ((FREObject) sharpPerson.RawValue).toString()); //case sensitive, calls as3 toString NOT c# ToString()

                var sharpPersonType = sharpPerson.Type();
                Trace($"sharpPerson type is: {sharpPersonType}");

                Trace("sharpPerson.hasOwnProperty(\"name\")", sharpPerson.hasOwnProperty("name"));

                int sharpAge = sharpPerson.age;
                string sharpName = sharpPerson.name;
                string sharpOptional = sharpPerson.opt;
                double sharpHeight = sharpPerson.height;
                bool sharpIsMan = sharpPerson.isMan;
                Trace("FreObjectSharp age 1", sharpAge);
                Trace("FreObjectSharp name 1", sharpName);
                Trace("FreObjectSharp opt 1 is null?:", sharpOptional == null);
                Trace("FreObjectSharp height 1", sharpHeight);
                Trace("FreObjectSharp isMan 1", sharpIsMan);

                sharpPerson.age = 999;
                sharpPerson.height = 1.88;
                sharpPerson.isMan = true;

                Trace("FreObjectSharp age 2", (int) sharpPerson.age);
                Trace("FreObjectSharp height 2", (double) sharpPerson.height);
                Trace("FreObjectSharp isMan 2", (bool) sharpPerson.isMan);

                sharpPerson.age = 111.ToFREObject();
                sharpPerson.height = 2;

                Trace("FreObjectSharp age 3", (int) sharpPerson.age);
                Trace("FreObjectSharp height 3", (double) sharpPerson.height);
                var sharpNameCity = (FREObject) sharpPerson.city;
                Trace("sharpNameCity", sharpNameCity.toString());
            }
            catch (Exception e) {
                Trace(e.GetType());
                Trace(e.Message);
                Trace(e.Source);
                Trace(e.StackTrace);
            }

            return person;
        }

        private FREObject RunExtensibleTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Extensible test***********");
            var rectangle = argv[0].AsRect();
            rectangle.Width = 999;
            rectangle.Height = 111;

            var unused = new Point(10, 88).ToFREObject();
            return rectangle.ToFREObject();
        }

        private FREObject RunBitmapTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Bitmap test***********");
            var inFre = argv[0];
            if (inFre == FREObject.Zero) return FREObject.Zero;

            try {
                var bmd = new FreBitmapDataSharp(inFre);
                SepiaTone(bmd);
                // var bmp = bmd.AsBitmap(); //makes a bitmap copy
            }
            catch (Exception e) {
                Trace(e.GetType());
                Trace(e.Message);
                Trace(e.Source);
                Trace(e.StackTrace);
            }

            return FREObject.Zero;
        }

        private static void SepiaTone(FreBitmapDataSharp freBitmapDataSharp) {
            freBitmapDataSharp.Acquire();
            var ptr = freBitmapDataSharp.Bits32;
            var byteBuffer = new byte[freBitmapDataSharp.LineStride32 * freBitmapDataSharp.Height * 4];
            Marshal.Copy(ptr, byteBuffer, 0, byteBuffer.Length);
            const byte maxValue = 255;
            for (var k = 0; k < byteBuffer.Length; k += 4) {
                var r = byteBuffer[k] * 0.189f + byteBuffer[k + 1] * 0.769f + byteBuffer[k + 2] * 0.393f;
                var g = byteBuffer[k] * 0.168f + byteBuffer[k + 1] * 0.686f + byteBuffer[k + 2] * 0.349f;
                var b = byteBuffer[k] * 0.131f + byteBuffer[k + 1] * 0.534f + byteBuffer[k + 2] * 0.272f;

                byteBuffer[k + 2] = r > maxValue ? maxValue : (byte) r;
                byteBuffer[k + 1] = g > maxValue ? maxValue : (byte) g;
                byteBuffer[k] = b > maxValue ? maxValue : (byte) b;
            }

            Marshal.Copy(byteBuffer, 0, ptr, byteBuffer.Length);
            freBitmapDataSharp.InvalidateBitmapDataRect(0, 0, Convert.ToUInt32(freBitmapDataSharp.Width),
                Convert.ToUInt32(freBitmapDataSharp.Height));
            freBitmapDataSharp.Release();
        }

        private FREObject RunByteArrayTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start ByteArray test***********");
            var inFre = argv[0];
            if (inFre == FREObject.Zero) return FREObject.Zero;
            var ba = new FreByteArraySharp(inFre);
            ba.Acquire();
            var byteData = ba.Bytes;
            var base64Encoded = Convert.ToBase64String(byteData);
            ba.Release();
            Trace("Encoded to Base64: ", base64Encoded);
            return FREObject.Zero;
        }

        private FREObject RunErrorTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Error Handling test***********");
            var person = argv[0];
            var s = argv[1];
            var i = argv[2];
            var unused1 = person.GetProp("doNotExist"); //getting a property that doesn't exist
            var unused2 = person.Call("noMethod"); //calling a method that doesn't exist
            var unused3 = person.Call("add", 100); //not passing enough args
            s.Call("noStringFunc"); //call method on a string
            var unused4 = i.AsString(); //try to convert int to string
            return FREObject.Zero;
        }

        private FREObject RunDataTests(FREContext ctx, uint argc, FREObject[] argv) {
            return FREObject.Zero;
        }

        private FREObject RunDateTests(FREContext ctx, uint argc, FREObject[] argv) {
            var airDate = argv[0].AsDateTime();
            Trace(airDate.Day, airDate.Month, airDate.Year, airDate.Hour, airDate.Minute);
            return new DateTime(1990, 11, 25, 23, 19, 15, 0).ToFREObject();
        }

        private FREObject RunColorTests(FREContext ctx, uint argc, FREObject[] argv) {
            var airColor = argv[0].AsColor();
            Trace("A", airColor.A, "R", airColor.R, "G", airColor.G, "B", airColor.B);
            return Color.FromArgb(airColor.A, airColor.R, airColor.G, airColor.B).ToFREObject();
        }

        public override void OnFinalize() { }
    }
}