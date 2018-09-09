#pragma once
#include "FlashRuntimeExtensions.h"
using namespace System;
using namespace Drawing;
using namespace Imaging;
using namespace IO;
using namespace Runtime::InteropServices;

using FREObjectCLR = IntPtr;
using FREContextCLR = IntPtr;

namespace FRESharpCore {

	public ref struct FREBitmapDataCLR {
		uint32_t width;
		uint32_t height;
		uint32_t hasAlpha;
		uint32_t isPremultiplied;
		uint32_t lineStride32;
		uint32_t isInvertedY; 
		IntPtr bits32;
	};

	public ref struct FREByteArrayCLR {
		uint32_t length;
		IntPtr bytes;
	};

	public ref class FRESharpCLR {
	public:
		FRESharpCLR();

		String^ getString(FREObjectCLR freObject, UInt32% freResult);
		Int32 getInt32(FREObjectCLR freObject, UInt32% freResult);
		UInt32 getUInt32(FREObjectCLR freObject, UInt32% freResult);
		UInt32 getArrayLength(FREObjectCLR freObject, UInt32% freResult);
		FREObjectCLR getObjectAt(FREObjectCLR freObject, UInt32 i, UInt32% freResult);
		void setObjectAt(FREObjectCLR freObject, UInt32 i, FREObjectCLR value, UInt32% freResult);
		bool getBool(FREObjectCLR freObject, UInt32% freResult);
		double getDouble(FREObjectCLR freObject, UInt32% freResult);
		void dispatchEvent(FREContextCLR freContext, String^ name, String^ value);
		FREObjectCLR getProperty(FREObjectCLR freObject, String^ propertyName, UInt32% freResult);
		FREObjectCLR setProperty(FREObjectCLR freObject, String^ name, FREObjectCLR value, UInt32% freResult);

		FREObjectCLR getFREObject(String^ value, UInt32% freResult);
		FREObjectCLR getFREObject(double value, UInt32% freResult);
		FREObjectCLR getFREObject(bool value, UInt32% freResult);
		FREObjectCLR getFREObject(Int32 value, UInt32% freResult);
		FREObjectCLR getFREObject(UInt32 value, UInt32% freResult);
		FREObjectCLR getFREObject(String^ className, array<FREObjectCLR>^ argv, UInt32 argc, UInt32% freResult);
		FREObjectCLR callMethod(FREObjectCLR freObject, String^ className, array<FREObjectCLR>^ argv, UInt32 argc, 
			UInt32% freResult);
		int getType(FREObjectCLR freObject, UInt32% freResult);

		void acquireBitmapData(FREObjectCLR freObject, FREBitmapDataCLR^ descriptorToSet);
		void releaseBitmapData(FREObjectCLR freObject);
		void invalidateBitmapDataRect(FREObjectCLR freObject, UInt32 x, UInt32 y, UInt32 width, UInt32 height);

		FREObjectCLR getFREObject(Bitmap^ value, FREBitmapDataCLR^ descriptorToSet, UInt32% freResult);

		void acquireByteArrayData(FREObjectCLR freObject, FREByteArrayCLR^ byteArrayToSet);
		void releaseByteArrayData(FREObjectCLR freObject);

		FREObjectCLR getActionScriptData(FREContextCLR freContext, UInt32% freResult);
		void setActionScriptData(FREContextCLR freContext, FREObjectCLR freObject, UInt32% freResult);
	};

}