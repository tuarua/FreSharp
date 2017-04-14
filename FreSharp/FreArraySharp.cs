using System;
namespace FreSharp {
    public class FreArraySharp {
        private readonly IntPtr _freArray = IntPtr.Zero;
        public FreArraySharp() { }
        public FreArraySharp(IntPtr freArray) {
            _freArray = freArray;
        }

        public uint GetLength() {
            return FreSharpHelper.Core.getArrayLength(_freArray);
        }

        public FreObjectSharp GetObjectAt(uint i) {
            return new FreObjectSharp(FreSharpHelper.Core.getObjectAt(_freArray, i));
        }

        public void SetObjectAt() { //TODO

        }

        public IntPtr Get() {
            return _freArray;
        }

    }
}
