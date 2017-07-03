using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Shapes;
using FreSharp.Geom;
using TuaRua.AIRNative;
using TuaRua.FreSharp;
using TuaRua.FreSharp.Display;
using TuaRua.FreSharp.Exceptions;
using TuaRua.FreSharp.Geom;
using static System.Windows.Media.Brushes;
using static System.Windows.Media.Color;
using FREObject = System.IntPtr;
using FREContext = System.IntPtr;

namespace FreExampleSharpLib {
    public class MainController : FreSharpController {
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

            var person = new FreObjectSharp(argv[0]);

            try {
                person.GetProperty("doNotExist"); //calling a property that doesn't exist
            }
            catch (Exception e) {
                Trace(e.GetType().ToString());
                Trace(e.Message);
                Trace(e.Source);
                Trace(e.StackTrace);
            }

            try {
                person.CallMethod("noMethod"); //calling an nonexistent method
            }
            catch (Exception e) {
                Trace(e.GetType().ToString());
                Trace(e.Message);
                Trace(e.Source);
                Trace(e.StackTrace);
            }


            try {
                var unused = person.CallMethod("add", 100); //not passing enough args
            }
            catch (Exception e) {
                Trace(e.GetType().ToString());
                Trace(e.Message);
                Trace(e.Source);
                Trace(e.StackTrace);
            }

            return FREObject.Zero;
        }

        private FREObject RunErrorTests2(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Error Handling test 2***********");
            var testString = new FreObjectSharp(argv[0]);
            try {
                testString.CallMethod("noStringFunc"); //call method on a string
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
            Trace("Encoded to Base64: " + base64Encoded);
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
                Trace(e.GetType().ToString());
                Trace(e.Message);
                Trace(e.Source);
                Trace(e.StackTrace);
            }
            return FREObject.Zero;
        }

        private FREObject RunObjectTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Object test***********");
            var inFre1 = argv[0];
            if (inFre1 == FREObject.Zero) return FREObject.Zero;

            var person = new FreObjectSharp(inFre1);
            var freAge = person.GetProperty("age");
            var oldAge = Convert.ToInt32(freAge.Value);
            var newAge = new FreObjectSharp(oldAge + 10);
            person.SetProperty("age", newAge);

            var personType = person.GetType();
            Trace("person type is:" + personType);
            Trace("current person age is: " + oldAge);
            var addition = person.CallMethod("add", 100, 33);
            var sum = addition.Value;
            Trace("result is: " + sum);

            var dictionary = person.Value as Dictionary<string, object>;
            if (dictionary == null) return person.RawValue;
            var city = dictionary["city"] as Dictionary<string, object>;
            if (city == null) return person.RawValue;
            var name = city["name"];
            Trace("what is the city name: " + name);


            return person.RawValue;
        }

        private FREObject RunExtensibleTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Extensible test***********");

            // FreRectangleSharp is a new FreXXXSharp type which is extended from FreObjectSharp
            // It lives in the package FreSharp.Geom
            var rectangle = new FreRectangleSharp(argv[0]).Value;
            rectangle.Width = 999;
            rectangle.Height = 111;
            var ret = new FreRectangleSharp(rectangle);

            var point = new Point(10, 88);
            // FrePointSharp is a new FreXXXSharp type which is extended from FreObjectSharp
            // It is created in the local project. It is based off flash.geom.Point
            // This enables more and more as3 classes to be ported to FRE !!

            var frePoint = new FrePointSharp(point);
            var targetPoint = new FrePointSharp(new Point(100, 444));
            frePoint.CopyFrom(targetPoint);

            Trace(frePoint.RawValue.ToString());

            return ret.RawValue;
        }

        private FREObject RunArrayTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Array test***********");
            var inFre = new FreArraySharp(argv[0]);
            var airArray = inFre.GetAsArrayList();
            var airArrayLen = inFre.Length;

            Trace("Array passed from AIR: " + airArray);
            Trace("AIR Array length: " + airArrayLen);

            var itemZero = inFre.GetObjectAt(0);
            var itemZeroVal = Convert.ToInt32(itemZero.Value);

            Trace("AIR Array item 0 before change: " + itemZeroVal);

            var newVal = new FreObjectSharp(56);
            inFre.SetObjectAt(newVal, 0);

            return inFre.RawValue;
        }

        private FREObject RunIntTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Int Uint test***********");
            var inFre1 = argv[0];
            var inFre2 = argv[1];
            if (inFre1 == FREObject.Zero) return FREObject.Zero;
            if (inFre2 == FREObject.Zero) return FREObject.Zero;

            try {
                var airInt = Convert.ToInt32(new FreObjectSharp(inFre1).Value);
                var airUint = Convert.ToUInt32(new FreObjectSharp(inFre2).Value);

                Trace("Int passed from AIR: " + airInt);
                Trace("Uint passed from AIR: " + airUint);
            }
            catch (Exception e) {
                Console.WriteLine($@"caught in C#: type: {e.GetType()} message: {e.Message}");
            }
            const int sharpInt = -666;
            const uint sharpUInt = 888;

            var intFreType = new FreObjectSharp(sharpUInt).GetType();

            Trace("uintFreType: " + intFreType);
            return new FreObjectSharp(sharpInt).RawValue;
        }


        private FREObject RunNumberTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Number test***********");
            var inFre = argv[0];
            if (inFre == FREObject.Zero) return FREObject.Zero;
            var airNumber = (double) new FreObjectSharp(inFre).Value;
            Trace("Number passed from AIR: " + airNumber);
            const double sharpDouble = 34343.31;
            return new FreObjectSharp(sharpDouble).RawValue;
        }


        private FREObject RunStringTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace(@"***********Start String test***********");
            var inFre = argv[0];
            if (inFre == FREObject.Zero) return FREObject.Zero;
            try {
                var airString = Convert.ToString(new FreObjectSharp(inFre).Value);
                Trace("String passed from AIR:" + airString);
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
            return new FreObjectSharp(sharpString).RawValue;
        }
    }
}