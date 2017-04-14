using System;
namespace FreSharp {
    /// <summary>
    ///  FreArraySharp wraps a C FREObject (Array or Vector) with helper methods.
    /// </summary>
    public class FreArraySharp {
        private readonly IntPtr _freArray = IntPtr.Zero;
        /// <summary>
        /// Creates an Empty C# FreArray.
        /// </summary>
        public FreArraySharp() { }
        /// <summary>
        /// Creates a C# FreArray from a C FREObject.
        /// </summary>
        /// <param name="freArray"></param>
        public FreArraySharp(IntPtr freArray) {
            _freArray = freArray;
        }

        /// <summary>
        /// Returns the length of the C# FreArray.
        /// </summary>
        /// <returns></returns>
        public uint GetLength() {
            return FreSharpHelper.Core.getArrayLength(_freArray);
        }

        /// <summary>
        /// Returns the C# FreObject from the C# FreArray at i.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public FreObjectSharp GetObjectAt(uint i) {
            return new FreObjectSharp(FreSharpHelper.Core.getObjectAt(_freArray, i));
        }

        /// <summary>
        /// Sets the C# FreObject in the C# FreArray at i.
        /// </summary>
        public void SetObjectAt(FreObjectSharp value, uint i) {
            FreSharpHelper.Core.setObjectAt(_freArray, i, value.Get());
        }

        /// <summary>
        /// Returns the associated C FREObject of the C# FreArray.
        /// </summary>
        /// <returns></returns>
        public IntPtr Get() {
            return _freArray;
        }

    }
}
