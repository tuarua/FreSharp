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
using Point = System.Windows.Point;

// ReSharper disable InconsistentNaming

namespace TuaRua.FreSharp.Geom {
    /// <summary>
    /// Wrapper Class for C# System.Windows.Point
    /// </summary>
    public static class FrePoint {
        /// <summary>
        /// Converts a C# Point to a FREObject
        /// </summary>
        /// <param name="value"></param>
        /// <returns>FREObject</returns>
        public static FREObject ToFREObject(this Point value) {
            return new FREObject().Init("flash.geom.Point", value.X, value.Y);
        }

        /// <summary>
        /// Converts a FREObject to a C# Point
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns>Point</returns>
        public static Point AsPoint(this FREObject inFre) {
            dynamic fre = new FreObjectSharp(inFre);
            return new Point(fre.x, fre.y);
        }
    }
}