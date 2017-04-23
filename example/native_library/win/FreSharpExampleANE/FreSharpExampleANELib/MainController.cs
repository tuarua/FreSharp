using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FreSharp;
using FREObject = System.IntPtr;
using FREContext = System.IntPtr;

namespace FreExampleSharpLib {
    public class MainController : FreSharpController {
        public string[] GetFunctions() {
            FunctionsDict =
                new Dictionary<string, Func<FREObject, uint, FREObject[], FREObject>>
                {
                    {"runStringTests", RunStringTests},
                    {"runNumberTests", RunNumberTests},
                    {"runIntTests", RunIntTests},
                    {"runArrayTests", RunArrayTests},
                    {"runObjectTests", RunObjectTests},
                    {"runBitmapTests", RunBitmapTests},
                    {"runByteArrayTests", RunByteArrayTests},
                    {"runErrorTests", RunErrorTests},
                    {"runDataTests", RunDataTests},

                };


            return FunctionsDict.Select(kvp => kvp.Key).ToArray();
        }

        private FREObject RunDataTests(FREContext ctx, uint argc, FREObject[] argv) {
            return FREObject.Zero;
        }

        private FREObject RunErrorTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Error Handling test***********");

            var person = new FreObjectSharp(argv[0]);
            var testString = new FreObjectSharp(argv[1]);

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
                person.CallMethod("noMethod", null); //calling an nonexistent method
            }
            catch (Exception e) {
                Trace(e.GetType().ToString());
                Trace(e.Message);
                Trace(e.Source);
                Trace(e.StackTrace);
            }


            try {
                var unused = person.CallMethod("add", new ArrayList
                    {
                        new FreObjectSharp(100)
                    });//not passing enough args
            }
            catch (Exception e) {
                Trace(e.GetType().ToString());
                Trace(e.Message);
                Trace(e.Source);
                Trace(e.StackTrace);
            }


            try {
                testString.GetAsInt(); //get as wrong type
            }
            catch (Exception e) {
                Trace(e.GetType().ToString());
                Trace(e.Message);
                Trace(e.Source);
                Trace(e.StackTrace);
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

        private FREObject RunBitmapTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Bitmap test***********");
            var inFre = argv[0];
            if (inFre == FREObject.Zero) return FREObject.Zero;

            try {
                var bmp = new FreBitmapDataSharp(inFre).GetAsBitmap();

                if (bmp == null) return FREObject.Zero;
                Trace("C# Bitmap Width: " + bmp.Width);
                Trace("C# Bitmap Height: " + bmp.Height);
                var ret = new FreBitmapDataSharp(bmp); //convert to as3 BitmapData

                return ret.Get();
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
            var oldAge = freAge.GetAsInt();
            var newAge = new FreObjectSharp(oldAge + 10);
            person.SetProperty("age", newAge);

            var personType = person.GetType();
            Trace("person type is:" + personType);
            Trace("current person age is: " + oldAge);
            var addition = person.CallMethod("add", new ArrayList
            {
                new FreObjectSharp(100),
                new FreObjectSharp(33)
            });
            var sum = addition.GetAsInt();
            Trace("result is: " + sum);

            var dictionary = person.GetAsDictionary();
            var city = dictionary["city"] as Dictionary<string, object>;
            if (city == null) return person.Get();
            var name = city["name"];
            Trace("what is the city name: " + name);

            return person.Get();
        }

        private FREObject RunArrayTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Array test***********");
            var inFre = new FreArraySharp(argv[0]);
            var airArray = inFre.GetAsArrayList();
            var airArrayLen = inFre.GetLength();

            Trace("Array passed from AIR: " + airArray);
            Trace("AIR Array length: " + airArrayLen);

            var itemZero = inFre.GetObjectAt(0);
            var itemZeroVal = itemZero.GetAsInt();

            Trace("AIR Array item 0 before change: " + itemZeroVal);

            var newVal = new FreObjectSharp(56);
            inFre.SetObjectAt(newVal, 0);

            return inFre.Get();

        }

        private FREObject RunIntTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Int Uint test***********");
            var inFre1 = argv[0];
            var inFre2 = argv[1];
            if (inFre1 == FREObject.Zero) return FREObject.Zero;
            if (inFre2 == FREObject.Zero) return FREObject.Zero;

            try {
                var airInt = new FreObjectSharp(inFre1).GetAsInt();
                var airUint = new FreObjectSharp(inFre2).GetAsUInt();

                Trace("Int passed from AIR: " + airInt);
                Trace("Uint passed from AIR: " + airUint);
            }
            catch (Exception e) {
                //TODO Trace with params
                Console.WriteLine(@"caught in C#: type: {0} message: {1}", e.GetType(), e.Message);
            }
            const int sharpInt = -666;
            const uint sharpUInt = 888;

            var intFreType = new FreObjectSharp(sharpUInt).GetType();

            Trace("uintFreType: " + intFreType);
            return new FreObjectSharp(sharpInt).Get();


        }

        private FREObject RunNumberTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace("***********Start Number test***********");
            var inFre = argv[0];
            if (inFre == FREObject.Zero) return FREObject.Zero;
            var airNumber = new FreObjectSharp(inFre).GetAsDouble();
            Trace("Number passed from AIR: " + airNumber);
            const double sharpDouble = 34343.31;
            return new FreObjectSharp(sharpDouble).Get();
        }

        private FREObject RunStringTests(FREContext ctx, uint argc, FREObject[] argv) {
            Trace(@"***********Start String test***********");
            var inFre = argv[0];
            if (inFre == FREObject.Zero) return FREObject.Zero;
            try {
                var airString = new FreObjectSharp(inFre).GetAsString();
                Trace("String passed from AIR:" + airString);
            }
            catch (Exception e) {
                Console.WriteLine(@"caught in C#: type: {0} message: {1}", e.GetType(), e.Message);
            }

            const string sharpString = "I am a string from C#";
            return new FreObjectSharp(sharpString).Get();
        }



    }
}