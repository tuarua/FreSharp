#region License

// Copyright 2017 Tua Rua Ltd.
// 
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
// 
//  http://www.apache.org/licenses/LICENSE-2.0
// 
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
// 
//  Additional Terms
//  No part, or derivative of this Air Native Extensions's code is permitted 
//  to be sold as the basis of a commercially packaged Air Native Extension which 
//  undertakes the same purpose as this software. That is, a WebView for Windows, 
//  OSX and/or iOS and/or Android.
//  All Rights Reserved. Tua Rua Ltd.

#endregion

using System;
using System.Drawing;
using System.Dynamic;
using System.Windows;
using TuaRua.FreSharp.Geom;
using FREObject = System.IntPtr;
using Point = System.Windows.Point;
// ReSharper disable InconsistentNaming

namespace TuaRua.FreSharp {
    /// <inheritdoc />
    /// <summary>
    /// Wraps a FREObject in a dynamic object which allows us to perform easy gets and sets of it's properties
    /// </summary>
    public class FreObjectSharp : DynamicObject {
        private static FREObject _rawValue;

        /// <summary>
        /// Returns the type of the FreObjectSharp
        /// </summary>
        public FreObjectTypeSharp Type() => FreSharpHelper.GetType(_rawValue);

        /// <summary>
        /// Converts the RawValue FREObject to a C# object
        /// </summary>
        public object Value => FreSharpHelper.GetAsObject(_rawValue);

        /// <summary>
        /// Create a FreObjectSharp from base class of FREObject
        /// </summary>
        /// <param name="freObject"></param>
        // ReSharper disable once InheritdocConsiderUsage
        public FreObjectSharp(FREObject freObject) => _rawValue = freObject;

        /// <summary>
        /// Creates a FreObjectSharp
        /// </summary>
        /// <param name="className">Name of the Class</param>
        // ReSharper disable once InheritdocConsiderUsage
        public FreObjectSharp(string className) => _rawValue = new FREObject().Init(className);

        /// <summary>
        /// Creates a FreObjectSharp
        /// </summary>
        /// <param name="className">Name of the Class</param>
        /// <param name="args">Arguments to pass to the Class</param>
        // ReSharper disable once InheritdocConsiderUsage
        public FreObjectSharp(string className, params object[] args) => _rawValue = new FREObject().Init(className, args);

        /// <summary>
        /// Indicates whether an object has a specified property defined.
        /// </summary>
        /// <param name="name">The property of the FREObject. </param>
        /// <returns></returns>
        public bool hasOwnProperty(string name) => _rawValue.Call("hasOwnProperty", name).AsBool();

        /// <inheritdoc />
        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            if (binder.Name == "RawValue") {
                result = _rawValue;
                return true;
            }
            result = FreSharpHelper.GetAsPrimitiveObject(_rawValue.GetProp(binder.Name));
            return true;
        }

        /// <inheritdoc />
        public override bool TrySetMember(SetMemberBinder binder, object value) {
            _rawValue.SetProp(binder.Name, value);
            return true;
        }

        /// <inheritdoc />
        public override bool TryConvert(ConvertBinder binder, out object result) {
            var type = binder.Type;
            if (type == typeof(FREObject)) {
                result = _rawValue;
                return true;
            }

            if (type == typeof(string)) {
                result = _rawValue.AsString();
                return true;
            }

            if (type == typeof(double)) {
                result = _rawValue.AsDouble();
                return true;
            }

            if (type == typeof(bool)) {
                result = _rawValue.AsBool();
                return true;
            }

            if (type == typeof(uint)) {
                result = _rawValue.AsUInt();
                return true;
            }

            if (type == typeof(int) || type == typeof(long) || type == typeof(short)) {
                result = _rawValue.AsInt();
                return true;
            }

            if (type == typeof(Rect)) {
                result = _rawValue.AsRect();
                return true;
            }

            if (type == typeof(Point)) {
                result = _rawValue.AsPoint();
                return true;
            }

            if (type == typeof(DateTime)) {
                result = _rawValue.AsDateTime();
                return true;
            }

            if (type == typeof(Color)) {
                result = _rawValue.AsColor();
                return true;
            }

            result = null;
            return true;
        }
    }
}