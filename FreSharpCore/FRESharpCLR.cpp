#include "FRESharpCLR.h"
using namespace System;
using FREObjectCLR = IntPtr;
#include <iostream>
#include <stdlib.h>
namespace FRESharpCore {
	FRESharpCLR::FRESharpCLR() {
		mFRESharpLib = new FRESharpLib();
	}

	void MarshalString(String^ s, std::string& os) {
		using namespace Runtime::InteropServices;
		const char* chars = (const char*)(Marshal::StringToHGlobalAnsi(s)).ToPointer();
		os = chars;
		Marshal::FreeHGlobal(IntPtr((void*)chars));
	}

	FREObjectCLR FRESharpCLR::getFREObject(String^ value) {
		std::string valueStr = "";
		MarshalString(value, valueStr);
		return FREObjectCLR(mFRESharpLib->getFREObject(valueStr));
	}

	FREObjectCLR FRESharpCLR::getFREObject(double value) {
		return FREObjectCLR(mFRESharpLib->getFREObject(value));
	}

	FREObjectCLR FRESharpCLR::getProperty(FREObjectCLR freObject, String^ propertyName) {
		std::string propertyNameStr = "";
		MarshalString(propertyName, propertyNameStr);
		return FREObjectCLR(mFRESharpLib->getProperty(freObject.ToPointer(), propertyNameStr));
	}

	void FRESharpCLR::setProperty(FREObjectCLR freObject, String^ name, FREObjectCLR value) {
		std::string nameStr = "";
		MarshalString(name, nameStr);
		mFRESharpLib->setProperty(freObject.ToPointer(), nameStr, value.ToPointer());
	}

	void FRESharpCLR::setProperty(FREObjectCLR freObject, String^ name, String^ value) {
		std::string nameStr = "";
		MarshalString(name, nameStr);
		std::string valueStr = "";
		MarshalString(value, valueStr);
		mFRESharpLib->setProperty(freObject.ToPointer(), nameStr, valueStr);
	}

	void FRESharpCLR::setProperty(FREObjectCLR freObject, String^ name, double value) {
		std::string nameStr = "";
		MarshalString(name, nameStr);
		mFRESharpLib->setProperty(freObject.ToPointer(), nameStr, value);
	}

	void FRESharpCLR::setProperty(FREObjectCLR freObject, String^ name, bool value) {
		std::string nameStr = "";
		MarshalString(name, nameStr);
		mFRESharpLib->setProperty(freObject.ToPointer(), nameStr, value);
	}

	void FRESharpCLR::setProperty(FREObjectCLR freObject, String^ name, Int32 value) {
		std::string nameStr = "";
		MarshalString(name, nameStr);
		mFRESharpLib->setProperty(freObject.ToPointer(), nameStr, value);
	}

	void FRESharpCLR::setProperty(FREObjectCLR freObject, String^ name, UInt32 value) {
		std::string nameStr = "";
		MarshalString(name, nameStr);
		mFRESharpLib->setProperty(freObject.ToPointer(), nameStr, value);
	}

	FREObjectCLR FRESharpCLR::getFREObject(bool value) {
		return FREObjectCLR(mFRESharpLib->getFREObject(value));
	}

	FREObjectCLR FRESharpCLR::getFREObject(Int32 value) {
		return FREObjectCLR(mFRESharpLib->getFREObject(value));
	}

	FREObjectCLR FRESharpCLR::getFREObject(UInt32 value) {
		return FREObjectCLR(mFRESharpLib->getFREObject(value));
	}

	FREObjectCLR FRESharpCLR::getFREObject(String^ className, array<FREObjectCLR>^ argv, uint32_t argc) {
		std::string classNameStr = "";
		MarshalString(className, classNameStr);

		array<FREObject>^ mArr = gcnew array<FREObject>(argc);
		for (uint32_t i = 0; i < argc; i++) {
			mArr[i] = argv[i].ToPointer();
		}
		if (argc == 0) {
			return FREObjectCLR(mFRESharpLib->getFREObject(classNameStr, nullptr, 0));
		}
		else {
			pin_ptr<FREObject> argvM = &mArr[0];
			return FREObjectCLR(mFRESharpLib->getFREObject(classNameStr, argvM, argc));
		}

	}

	FREObjectCLR FRESharpCLR::callMethod(FREObjectCLR freObject, String^ className, array<FREObjectCLR>^ argv, uint32_t argc) {
		std::string classNameStr = "";
		MarshalString(className, classNameStr);

		array<FREObject>^ mArr = gcnew array<FREObject>(argc);
		for (uint32_t i = 0; i < argc; i++) {
			mArr[i] = argv[i].ToPointer();
		}
		pin_ptr<FREObject> argvM = &mArr[0];
		return FREObjectCLR(mFRESharpLib->callMethod(freObject.ToPointer(), classNameStr, argvM, argc));
	}


	String^ FRESharpCLR::getString(FREObjectCLR freObject) {
		std::string cString = mFRESharpLib->getString(freObject.ToPointer());
		return gcnew String(cString.c_str());
	}

	Int32 FRESharpCLR::getInt32(FREObjectCLR freObject) {
		return  mFRESharpLib->getInt32(freObject.ToPointer());
	}

	UInt32 FRESharpCLR::getUInt32(FREObjectCLR freObject) {
		return mFRESharpLib->getUInt32(freObject.ToPointer());
	}

	UInt32 FRESharpCLR::getArrayLength(FREObjectCLR freObject) {
		return mFRESharpLib->getArrayLength(freObject.ToPointer());
	}

	FREObjectCLR FRESharpCLR::getObjectAt(FREObjectCLR freObject, UInt32 i) {
		return FREObjectCLR(mFRESharpLib->getObjectAt(freObject.ToPointer(), i));
	}

	bool FRESharpCLR::getBool(FREObjectCLR freObject) {
		return mFRESharpLib->getBool(freObject.ToPointer());
	}

	double FRESharpCLR::getDouble(FREObjectCLR freObject) {
		return mFRESharpLib->getDouble(freObject.ToPointer());
	}

	void FRESharpCLR::dispatchEvent(String^ name, String^ value) {
		std::string nameStr = "";
		MarshalString(name, nameStr);

		std::string valueStr = "";
		MarshalString(value, valueStr);

		mFRESharpLib->dispatchEvent(nameStr, valueStr);
	}

	int FRESharpCLR::getType(FREObjectCLR freObject) {
		return mFRESharpLib->getType(freObject.ToPointer());
	}

	void FRESharpCLR::setFREContext(FREObjectCLR freContext) {
		mFRESharpLib->setFREContext(freContext.ToPointer());
	}
}