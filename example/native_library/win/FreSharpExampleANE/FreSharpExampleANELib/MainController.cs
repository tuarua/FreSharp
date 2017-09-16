using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Shapes;
using FreSharp.Geom;
using TuaRua.AIRNative;
using TuaRua.FreSharp;
using TuaRua.FreSharp.Display;
using TuaRua.FreSharp.Exceptions;
using static System.Windows.Media.Brushes;
using static System.Windows.Media.Color;
using FREObject = System.IntPtr;
using FREContext = System.IntPtr;
using System.Windows;

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
                    {"runErrorTests2", RunErrorTests2},
                };


            return FunctionsDict.Select(kvp => kvp.Key).ToArray();
        }

        private FREObject RunDataTests(FREContext ctx, uint argc, FREObject[] argv) {
            return FREObject.Zero;
        }

        private FREObject RunErrorTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Error Handling test***********");

            var person = argv[0];

            try {
                person.GetProp("doNotExist"); //calling a property that doesn't exist
            }
            catch (Exception e) {
                Trace(e.GetType());
                Trace(e.Message);
                Trace(e.Source);
                Trace(e.StackTrace);
            }

            try {
                person.Call("noMethod"); //calling an nonexistent method
            }
            catch (Exception e) {
                Trace(e.GetType());
                Trace(e.Message);
                Trace(e.Source);
                Trace(e.StackTrace);
            }


            try {
                var unused = person.Call("add", 100); //not passing enough args
            }
            catch (Exception e) {
                Trace(e.GetType());
                Trace(e.Message);
                Trace(e.Source);
                Trace(e.StackTrace);
            }

            return FREObject.Zero;
        }

        private FREObject RunErrorTests2(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Error Handling test 2***********");
            var testString = argv[0];
            try {
                testString.Call("noStringFunc"); //call method on a string
            }
            catch (Exception e) {
                return new FreException(e).RawValue; //return as3 error and throw in swc
            }

            return FREObject.Zero;
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

        private FREObject RunBitmapTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Bitmap test***********");
            var inFre = argv[0];
            if (inFre == FREObject.Zero) return FREObject.Zero;

            try {
                var bmd = new FreBitmapDataSharp(inFre);
                SepiaTone(bmd);
                //var bmp = bmd.GetAsBitmap(); //makes a bitmap copy
            }
            catch (Exception e) {
                Trace(e.GetType());
                Trace(e.Message);
                Trace(e.Source);
                Trace(e.StackTrace);
            }
            return FREObject.Zero;
        }

        private FREObject RunObjectTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Object test***********");
            var person = argv[0];
            if (person == FREObject.Zero) return FREObject.Zero;

            var oldAge = person.GetProp("age").AsInt();
            var newAge = oldAge + 10;
            person.SetProp("age", newAge);

            var personType = person.Type();
            Trace("person type is:", personType);
            Trace("current person age is: ", oldAge);
            var addition = person.Call("add", 100, 33);
            Trace("result is: ", addition.AsInt());

            try {
                var dictionary = person.AsDictionary();
                if (dictionary == null) return person;
                var city = dictionary["city"] as Dictionary<string, object>;
                if (city == null) return person;
                var name = city["name"];
                Trace("what is the city name: ", name);
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

            var point = new Point(10, 88);
            var frePoint = new FrePointSharp(point);
            var targetPoint = new FrePointSharp(new Point(100, 444));
            frePoint.CopyFrom(targetPoint);
            return rectangle.ToFREObject();
        }

        private FREObject RunArrayTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Array test***********");
            var inFre = new FREArray(argv[0]);

            var airArray = argv[0].ToArrayList();
            var airArrayLen = inFre.Length;

            Trace("Array passed from AIR:", airArray);
            Trace("AIR Array length:", airArrayLen);

            var itemZero = inFre.At(0);
            var itemZeroVal = itemZero.AsInt();

            Trace("AIR Array item 0 before change:", itemZeroVal);

            inFre.Set(0, 56);

            return inFre.RawValue;
        }

        private FREObject RunIntTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Int Uint test***********");
            if (argc < 2) return FREObject.Zero;
            if (argv[0] == FREObject.Zero) return FREObject.Zero;
            if (argv[1] == FREObject.Zero) return FREObject.Zero;

            var newPerson = new FREObject().Init("com.tuarua.Person");
            Trace("We created a new person. type =", newPerson.Type());

            try {
                var airInt = argv[0].AsInt();
                var airUint = argv[1].AsUInt();

                Trace("Int passed from AIR:", airInt);
                Trace("Uint passed from AIR:", airUint);
            }
            catch (Exception e) {
                Console.WriteLine($@"caught in C#: type: {e.GetType()} message: {e.Message}");
            }
            const int sharpInt = -666;
            return sharpInt.ToFREObject();
        }

        private FREObject RunNumberTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Number test***********");
            if (argv[0] == FREObject.Zero) return FREObject.Zero;
            var airNumber = argv[0].AsDouble();
            Trace("Number passed from AIR:", airNumber);
            const double sharpDouble = 34343.31;
            return sharpDouble.ToFREObject();
        }

        private FREObject RunStringTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace(@"***********Start String test***********");
            if (argv[0] == FREObject.Zero) return FREObject.Zero;
            try {
                var airString = argv[0].AsString();
                Trace("String passed from AIR:", airString);

                SendEvent("MY_EVENT", "this is a test");
            }
            catch (Exception e) {
                Console.WriteLine($@"caught in C#: type: {e.GetType()} message: {e.Message}");
            }


            //nativeRoot is actually created as a pointer when we call NativeStage.add() from AIRNativeANE
            var nativeRoot = FreStageSharp.GetRootView() as FreNativeRoot;

            var myEllipse = new Ellipse();
            var mySolidColorBrush = new SolidColorBrush {Color = FromArgb(255, 255, 255, 0)};

            // Describes the brush's color using RGB values. 
            // Each value has a range of 0-255.
            myEllipse.Fill = mySolidColorBrush;
            myEllipse.StrokeThickness = 2;
            myEllipse.Stroke = Black;

            // Set the width and height of the Ellipse.
            myEllipse.Width = 200;
            myEllipse.Height = 100;

            nativeRoot?.AddChild(myEllipse);

            const string sharpString = "I am a string from C#";
            return sharpString.ToFREObject();
        }
    }
}