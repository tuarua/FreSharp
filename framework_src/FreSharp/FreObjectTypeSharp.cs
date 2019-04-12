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

namespace TuaRua.FreSharp {
    /// <summary>
    /// Similar to FreObjectType. Added extra types to match Actionscript
    /// </summary>
    public enum FreObjectTypeSharp {
        /// <summary>
        /// Object
        /// </summary>
        Object = 0,

        /// <summary>
        /// Number
        /// </summary>
        Number,

        /// <summary>
        /// String
        /// </summary>
        String,

        /// <summary>
        /// Bytearray
        /// </summary>
        Bytearray,

        /// <summary>
        /// Array
        /// </summary>
        Array,

        /// <summary>
        /// Vector
        /// </summary>
        Vector,

        /// <summary>
        /// Bitmapdata
        /// </summary>
        Bitmapdata,

        /// <summary>
        /// Boolean
        /// </summary>
        Boolean,

        /// <summary>
        /// Null
        /// </summary>
        Null,

        /// <summary>
        /// Int
        /// </summary>
        Int,

        /// <summary>
        /// Class
        /// </summary>
        Class,

        /// <summary>
        /// flash.geom.Rectangle
        /// </summary>
        Rectangle,

        /// <summary>
        /// flash.geom.Point
        /// </summary>
        Point,

        /// <summary>
        /// Date
        /// </summary>
        Date
    }
}