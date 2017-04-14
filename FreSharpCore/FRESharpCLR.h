#pragma once
#include "FRESharpLib.h"
#include "FlashRuntimeExtensions.h"
using namespace System;
using FREObjectCLR = IntPtr;
namespace FRESharpCore {
	public ref class FRESharpCLR {
	public:
		FRESharpCLR();
		void setFREContext(FREObjectCLR freContext);
		String^ getString(FREObjectCLR freObject);
		Int32 getInt32(FREObjectCLR freObject);
		UInt32 getUInt32(FREObjectCLR freObject);
		UInt32 getArrayLength(FREObjectCLR freObject);
		FREObjectCLR getObjectAt(FREObjectCLR freObject, UInt32 i);
		bool getBool(FREObjectCLR freObject);
		double getDouble(FREObjectCLR freObject);
		void dispatchEvent (String^ name, String^ value);
		FREObjectCLR getProperty(FREObjectCLR freObject, String^ propertyName);

		void setProperty(FREObjectCLR freObject, String^ name, FREObjectCLR value);
		void setProperty(FREObjectCLR freObject, String^ name, String^ value);
		void setProperty(FREObjectCLR freObject, String^ name, double value);
		void setProperty(FREObjectCLR freObject, String^ name, bool value);
		void setProperty(FREObjectCLR freObject, String^ name, Int32 value);
		void setProperty(FREObjectCLR freObject, String^ name, UInt32 value);

		FREObjectCLR getFREObject(String^ value);
		FREObjectCLR getFREObject(double value);
		FREObjectCLR getFREObject(bool value);
		FREObjectCLR getFREObject(Int32 value);
		FREObjectCLR getFREObject(UInt32 value);
		FREObjectCLR getFREObject(String^ className, array<FREObjectCLR>^ argv, UInt32 argc);
		FREObjectCLR callMethod(FREObjectCLR freObject, String^ className, array<FREObjectCLR>^ argv, UInt32 argc);
		int getType(FREObjectCLR freObject);

	private:
		FRESharpLib* mFRESharpLib;
	};

}