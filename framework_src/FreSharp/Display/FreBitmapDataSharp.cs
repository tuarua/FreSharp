﻿#region License

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
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using FRESharpCore;
using FREObject = System.IntPtr;
// ReSharper disable UnusedMember.Global
namespace TuaRua.FreSharp.Display {
    /// <summary>
    /// FreBitmapDataSharp wraps a C FREBitmapData with helper methods.
    /// </summary>
    public class FreBitmapDataSharp {
        /// <summary>
        /// Returns the associated C FREBitmapData2 of the C# FREBitmapData.
        /// </summary>
        /// <returns></returns>
        public FREObject RawValue { get; }

        private readonly FREBitmapDataCLR _bmd = new FREBitmapDataCLR();

        /// <summary>
        /// An int that specifies the width, in pixels, of the bitmap. 
        /// This value corresponds to the width property of the ActionScript BitmapData class object. 
        /// This field is read-only. 
        /// </summary>
        public int Width;

        /// <summary>
        /// An int that specifies the height, in pixels, of the bitmap. 
        /// This value corresponds to the height property of the ActionScript BitmapData class object. 
        /// This field is read-only. 
        /// </summary>
        public int Height;

        /// <summary>
        /// A bool that indicates whether the bitmap supports per-pixel transparency. 
        /// This value corresponds to the transparent property of the ActionScript BitmapData class object. 
        /// If the value is non-zero, then the pixel format is ARGB32. 
        /// If the value is zero, the pixel format is _RGB32. Whether the value is big endian or 
        /// little endian depends on the host device. This field is read-only.
        /// </summary>
        public bool HasAlpha;

        /// <summary>
        /// A bool that indicates whether the bitmap pixels are stored as premultiplied color values. 
        /// This field is read-only. 
        /// </summary>
        public bool IsPremultiplied;

        /// <summary>
        /// A bool that indicates the order in which the rows of bitmap data in the image are stored. 
        /// A non-zero value means that the bottom row of the image appears first in the image data 
        /// (in other words, the first value in the bits32 array is the first pixel of the last row in the image). 
        /// This field is read-only.
        /// </summary>
        public bool IsInvertedY;

        /// <summary>
        /// An int that specifies the number of int values per scanline. 
        /// This value is typically the same as the width parameter. 
        /// This field is read-only.
        /// </summary>
        public int LineStride32;

        /// <summary>
        /// A pointer to a int. This value is an array of int values. Each value is one pixel of the bitmap.
        /// </summary>
        public IntPtr Bits32;

        /// <summary>
        /// Creates a C# FREBitmapData from a C FREBitmapData2
        /// </summary>
        /// <param name="freBitmapData"></param>
        public FreBitmapDataSharp(FREObject freBitmapData) {
            RawValue = freBitmapData;
        }

        /// <summary>
        /// Creates a C# FREBitmapData from a C# Bitmap
        /// </summary>
        /// <param name="bitmap"></param>
        public FreBitmapDataSharp(Bitmap bitmap) {
            uint resultPtr = 0;
            RawValue = FreSharpHelper.Core.getFREObject(bitmap, _bmd, ref resultPtr);
        }

        /// <summary>
        /// Calls FREReleaseBitmapData on the C FREBitmapData2
        /// </summary>
        public void Release() {
            FreSharpHelper.Core.releaseBitmapData(RawValue);
        }

        /// <summary>
        /// Calls FREAcquireBitmapData2 on the C FREBitmapData2
        /// </summary>
        public void Acquire() {
            FreSharpHelper.Core.acquireBitmapData(RawValue, _bmd);
            Width = (int) _bmd.width;
            Bits32 = _bmd.bits32;
            Height = (int) _bmd.height;
            HasAlpha = _bmd.hasAlpha == 1;
            IsInvertedY = _bmd.isInvertedY == 1;
            IsPremultiplied = _bmd.isPremultiplied == 1;
            LineStride32 = (int) _bmd.lineStride32;
        }

        /// <summary>
        /// Calls FREInvalidateBitmapDataRect on the C FREBitmapData2
        /// </summary>
        public void InvalidateBitmapDataRect(uint x, uint y, uint width, uint height) {
            FreSharpHelper.Core.invalidateBitmapDataRect(RawValue, x, y, width, height);
        }

        /// <summary>
        /// Converts the FREBitmapData into a C# Bitmap
        /// </summary>
        [Obsolete("GetAsBitmap is obsoleted, please use AsBitmap instead.", true)]
        public Bitmap GetAsBitmap() {
            var bitmap = new Bitmap(1, 1);
            Acquire();
            /*
            ///https://msdn.microsoft.com/en-us/library/zy1a2d14(v=vs.110).aspx
            */

            try {
                bitmap = new Bitmap(Width, Height, LineStride32 * 4,
                    PixelFormat.Format32bppArgb, Bits32);
                if (IsInvertedY) {
                    bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.InnerException);
            }

            Release();

            return bitmap;
        }

        /// <summary>
        /// Copies the FREBitmapData into a C# Bitmap
        /// </summary>
        public Bitmap AsBitmap() {
            var bitmap = new Bitmap(1, 1);
            Acquire();
            /*
            ///https://msdn.microsoft.com/en-us/library/zy1a2d14(v=vs.110).aspx
            */

            try {
                bitmap = new Bitmap(Width, Height, LineStride32 * 4,
                    PixelFormat.Format32bppArgb, Bits32);
                if (IsInvertedY) {
                    bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.InnerException);
            }

            Release();

            return bitmap;
        }

        /// <summary>
        /// Copies the FREBitmapData into a C# ByteArray
        /// </summary>
        public byte[] AsByteArray() {
            Acquire();
            var ptr = Bits32;
            var byteBuffer = new byte[LineStride32 * Height * 4];
            Marshal.Copy(ptr, byteBuffer, 0, byteBuffer.Length);
            Release();
            return byteBuffer;
        }
    }
}