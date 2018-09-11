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
// ReSharper disable MemberCanBeMadeStatic.Local

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
            Warning("I am a test warning");
            Info("I am a test info");
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

            Trace("Number passed from AIR as Double:", airDouble, testDouble.Equals(airDouble) ? "✅" : "❌");

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

            Trace("Number passed from AIR as Int:", testInt, testInt.Equals(airInt) ? "✅" : "❌");
            Trace("Number passed from AIR as UInt:", testUInt, testUInt.Equals(airUint) ? "✅" : "❌");

            const int sharpInt = -666;
            return sharpInt.ToFREObject();
        }

        private FREObject RunArrayTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Array test***********");
            if (argc < 4) {
                return new FreException("Not enough args").RawValue;
            }

            var airArray = new FREArray(argv[0]);
            var inFre1 = new FREArray(argv[1]);
            var inFre2 = new FREArray(argv[2]);
            var inFre3 = new FREArray(argv[3]);

            var airArrayList = airArray.AsArrayList();
            Trace("Convert FREArray to ArrayList :", airArrayList.Count, airArray.Length.Equals(6) ? "✅" : "❌");

            airArray.Push(77.ToFREObject(), 88);
            Trace("Get FREArray length after 2 appends:", airArray.Length, airArray.Length.Equals(8) ? "✅" : "❌");

            airArray[0] = 123.ToFREObject();
            foreach (var fre in airArray) {
                Trace("iterate over FREArray", fre.AsInt());
            }

            var airVectorString = inFre1.AsStringArray();
            var airVectorNumber = inFre2.AsDoubleArray();
            var airVectorBoolean = inFre3.AsBoolArray();

            Trace("Vector.<String> passed from AIR :",
                string.Join(",", airVectorString.ToArray()),
                string.Join(",", airVectorString.ToArray()).Equals("a,b,c,d") ? "✅" : "❌");

            Trace("Vector.<Number> passed from AIR :",
                string.Join(",", airVectorNumber.ToArray()),
                string.Join(",", airVectorNumber.ToArray()).Equals("1,0.5,2,3.3") ? "✅" : "❌");

            Trace("Vector.<Boolean> passed from AIR :",
                string.Join(",", airVectorBoolean.ToArray()),
                string.Join(",", airVectorBoolean.ToArray()).Equals("True,True,False,True") ? "✅" : "❌");

            var newFreArray = new FREArray("Object", 5, true);
            Trace("New FREArray of fixed length :", newFreArray.Length, newFreArray.Length.Equals(5) ? "✅" : "❌");

            airArray.Set(0, 56);
            var itemZero = airArray[0].AsInt();
            Trace("Set item 0 of FREArray:", itemZero, itemZero.Equals(56) ? "✅" : "❌");

            var marks = new[] {99, 98, 92, 97, 95};
            return marks.ToFREObject();
        }

        private FREObject RunObjectTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Object test***********");
            var person = argv[0];
            if (person == FREObject.Zero) return FREObject.Zero;

            var newPerson = new FREObject().Init("com.tuarua.Person");
            Trace("New Person.ToString()", newPerson.toString());

            Trace("New Person has property name:",
                newPerson.hasOwnProperty("name"), newPerson.hasOwnProperty("name") ? "✅" : "❌");
            Trace("New Person is of type CLASS:",
                newPerson.Type(), newPerson.Type().Equals(FreObjectTypeSharp.Class) ? "✅" : "❌");

            dynamic sharpPerson = new FreObjectSharp("com.tuarua.Person", "Ben McBobster", 80);
            Trace("sharpPerson.RawValue.ToString()",
                ((FREObject) sharpPerson.RawValue()).toString()); //case sensitive, calls as3 toString NOT c# ToString()

            var oldAge = person.GetProp("age").AsInt();
            Trace("Get property as Int :", oldAge, oldAge.Equals(21) ? "✅" : "❌");
            var newAge = oldAge + 10;
            person.SetProp("age", newAge);
            Trace("Set property to Int :", person.GetProp("age").AsInt(),
                person.GetProp("age").AsInt().Equals(31) ? "✅" : "❌");

            var addition = person.Call("add", 100, 33);
            Trace("Call add :", 131, addition.AsInt().Equals(133) ? "✅" : "❌");

            try {
                var dictionary = person.AsDictionary();
                if (dictionary == null) return person;
                var city = (Dictionary<string, object>) dictionary["city"];
                if (city == null) return person;
                var name = city["name"];
                Trace("Get property from Dict :", name, name.Equals("Portland") ? "✅" : "❌");

                var sharpPersonType = sharpPerson.Type();
                Trace("Dynamic Person is of type CLASS:",
                    sharpPersonType, sharpPersonType.Equals(FreObjectTypeSharp.Class) ? "✅" : "❌");
                Trace("Dynamic Person has property name:",
                    sharpPerson.hasOwnProperty("name"), sharpPerson.hasOwnProperty("name") ? "✅" : "❌");

                dynamic sharpCity = new FreObjectSharp("com.tuarua.City");
                FreObjectTypeSharp sharpCityType = sharpCity.Type();

                Trace("Dynamic City is of type CLASS:",
                    sharpCityType, sharpCityType.Equals(FreObjectTypeSharp.Class) ? "✅" : "❌");

                sharpCity.name = "San Francisco";

                int sharpAge = sharpPerson.age;
                string sharpName = sharpPerson.name;
                string sharpOptional = sharpPerson.opt;
                double sharpHeight = sharpPerson.height;
                bool sharpIsMan = sharpPerson.isMan;

                Trace("Dynamic age as int:", sharpAge, sharpAge.Equals(80) ? "✅" : "❌");
                Trace("Dynamic name as string:", sharpName, sharpName.Equals("Ben McBobster") ? "✅" : "❌");
                Trace("Dynamic Optional string:", sharpOptional == null,
                    (sharpOptional == null).Equals(true) ? "✅" : "❌");
                Trace("Dynamic height as double:", sharpHeight, sharpHeight.Equals(1.8) ? "✅" : "❌");
                Trace("Dynamic isMan as bool:", sharpIsMan, sharpIsMan.Equals(false) ? "✅" : "❌");

                sharpPerson.age = 999;
                sharpPerson.height = 1.88;
                sharpPerson.isMan = true;
                sharpPerson.city = sharpCity;

                Trace("Dynamic age as int:", (int) sharpPerson.age,
                    ((int) sharpPerson.age).Equals(999) ? "✅" : "❌");
                Trace("Dynamic height as double:", (double) sharpPerson.height,
                    ((double) sharpPerson.height).Equals(1.88) ? "✅" : "❌");
                Trace("Dynamic isMan as bool:",  (bool)sharpPerson.isMan, 
                    ((bool)sharpPerson.isMan).Equals(true) ? "✅" : "❌");

                sharpPerson.age = 111.ToFREObject();
                sharpPerson.height = 2;

                Trace("Dynamic age as int:", (int)sharpPerson.age, ((int)sharpPerson.age).Equals(111) ? "✅" : "❌");
                Trace("Dynamic height as double:", (double)sharpPerson.height,
                    ((double)sharpPerson.height).Equals(2) ? "✅" : "❌");
            }
            catch (Exception e) {
                Trace(e.GetType());
                Trace(e.Message);
                Trace(e.Source);
                Trace(e.StackTrace);
            }

            return sharpPerson.RawValue();
        }

        private FREObject RunExtensibleTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Extensible test***********");
            var rectangle = argv[0].AsRect();
            Trace("Rect :", rectangle, rectangle.X.Equals(50.9) ? "✅" : "❌");
            var frePoint = new Point(10, 88).ToFREObject();
            Trace("Point :", frePoint.toString(),
                frePoint.GetProp("x").AsInt() == 10 && frePoint.GetProp("y").AsInt() == 88
                    ? "✅"
                    : "❌");
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
            Trace("Base64 :", base64Encoded,
                base64Encoded.Equals("QyMgaW4gYW4gQU5FLiBTYXkgd2hhYWFhdCE=") ? "✅" : "❌");
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
            var objectAs = argv[0];
            Context.SetActionScriptData(objectAs);
            return Context.GetActionScriptData();
        }

        private FREObject RunDateTests(FREContext ctx, uint argc, FREObject[] argv) {
            var airDate = argv[0].AsDateTime();
            return airDate.ToFREObject();
        }

        private FREObject RunColorTests(FREContext ctx, uint argc, FREObject[] argv) {
            var airColor = argv[0].AsColor();
            return Color.FromArgb(airColor.A, airColor.R, airColor.G, airColor.B).ToFREObject();
        }

        public override void OnFinalize() { }
        public override string TAG => "MainController";
    }
}