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
//  All Rights Reserved. Tua Rua Ltd.

#endregion

using FREObject = System.IntPtr;
using Rect = System.Windows.Rect;

// ReSharper disable InconsistentNaming
namespace TuaRua.FreSharp.Geom {
    /// <summary>
    /// Wrapper Class for C# System.Windows.Rect
    /// </summary>
    public static class FreRect {
        /// <summary>
        /// Converts a C# Rect to a FREObject.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this Rect value) {
            return new FREObject().Init("flash.geom.Rectangle", value.X, value.Y, value.Width, value.Height);
        }

        /// <summary>
        /// Converts a FREObject to a C# Rect.
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static Rect AsRect(this FREObject inFre) {
            dynamic fre = new FreObjectSharp(inFre);
            return new Rect(fre.x, fre.y, fre.width, fre.height);
        }
    }
}