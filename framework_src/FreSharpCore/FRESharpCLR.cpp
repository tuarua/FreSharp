/* Copyright 2017 Tua Rua Ltd.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.*/

#include "FRESharpCLR.h"
using namespace System;
using FREObjectCLR = IntPtr;

#include <codecvt>
namespace FRESharpCore {
	FRESharpCLR::FRESharpCLR() {
	}

	std::string wstring_to_utf8(const std::wstring& str) {
		std::wstring_convert<std::codecvt_utf8<wchar_t>> myconv;
		return myconv.to_bytes(str);
	}

	void MarshalString(String ^ s, std::wstring& os) {
		using namespace Runtime::InteropServices;
		const wchar_t* chars = (const wchar_t*)(Marshal::StringToHGlobalUni(s)).ToPointer();
		os = chars;
		Marshal::FreeHGlobal(IntPtr((void*)chars));
	}

	void MarshalString(String^ s, std::string& os) {
		using namespace Runtime::InteropServices;
		const char* chars = (const char*)(Marshal::StringToHGlobalAnsi(s)).ToPointer();
		os = chars;
		Marshal::FreeHGlobal(IntPtr((void*)chars));
	}

	FREObjectCLR FRESharpCLR::getFREObject(String^ value, UInt32% freresult) {
		std::wstring valueWstr = L"";
		MarshalString(value, valueWstr);
		std::string valueStr = wstring_to_utf8(valueWstr);
		FREObject result;
		freresult = FRENewObjectFromUTF8(uint32_t(valueStr.length()), reinterpret_cast<const uint8_t *>(valueStr.data()), &result);
		return FREObjectCLR(result);
	}

	FREObjectCLR FRESharpCLR::getFREObject(double value, UInt32% freresult) {
		FREObject result;
		freresult = FRENewObjectFromDouble(value, &result);
		return FREObjectCLR(result);
	}

	FREObjectCLR FRESharpCLR::getProperty(FREObjectCLR freObject, String^ propertyName, UInt32% freresult) {
		std::string propertyNameStr = "";
		MarshalString(propertyName, propertyNameStr);
		FREObject ret = nullptr;
		FREObject thrownException = nullptr;
		freresult = FREGetObjectProperty(freObject.ToPointer(), reinterpret_cast<const uint8_t *>(propertyNameStr.data()),
			&ret, &thrownException);

		if (FRE_OK == freresult) {
			return FREObjectCLR(ret);
		} else {
			return FREObjectCLR(thrownException);
		}
	}

	FREObjectCLR FRESharpCLR::setProperty(FREObjectCLR freObject, String^ name, FREObjectCLR value, UInt32% freresult) {
		std::string nameStr = "";
		MarshalString(name, nameStr);
		FREObject ret = nullptr;
		FREObject thrownException = nullptr;
		freresult = FRESetObjectProperty(freObject.ToPointer(), reinterpret_cast<const uint8_t *>(nameStr.c_str()),
			value.ToPointer(), &thrownException);

		if (FRE_OK == freresult) {
			return FREObjectCLR(ret);
		} else {
			return FREObjectCLR(thrownException);
		}
	}

	FREObjectCLR FRESharpCLR::getFREObject(bool value, UInt32% freresult) {
		FREObject result;
		freresult = FRENewObjectFromBool(value, &result);
		return FREObjectCLR(result);
	}

	FREObjectCLR FRESharpCLR::getFREObject(Int32 value, UInt32% freresult) {
		FREObject result;
		freresult = FRENewObjectFromInt32(value, &result);
		return FREObjectCLR(result);
	}

	FREObjectCLR FRESharpCLR::getFREObject(UInt32 value, UInt32% freresult) {
		FREObject result;
		freresult = FRENewObjectFromUint32(value, &result);
		return FREObjectCLR(result);
	}

	FREObjectCLR FRESharpCLR::getFREObject(String^ className, array<FREObjectCLR>^ argv, UInt32 argc, UInt32% freresult) {
		std::string classNameStr = "";
		MarshalString(className, classNameStr);

		array<FREObject>^ mArr = gcnew array<FREObject>(argc);
		for (uint32_t i = 0; i < argc; i++) {
			mArr[i] = argv[i].ToPointer();
		}

		FREObject ret;
		FREObject thrownException = nullptr;

		if (argc == 0) {
			freresult = FRENewObject(reinterpret_cast<const uint8_t *>(classNameStr.data()), 0, nullptr, &ret, &thrownException);
		} else {
			pin_ptr<FREObject> argvM = &mArr[0];
			freresult = FRENewObject(reinterpret_cast<const uint8_t *>(classNameStr.data()), argc, argvM, &ret, &thrownException);
		}
		if (FRE_OK == freresult) {
			return FREObjectCLR(ret);
		} else {
			return FREObjectCLR(thrownException);
		}
	}

	FREObjectCLR FRESharpCLR::callMethod(FREObjectCLR freObject, String ^ methodName, array<FREObjectCLR>^ argv, UInt32 argc, UInt32% freresult) {
		std::string methodNameStr = "";
		MarshalString(methodName, methodNameStr);
		array<FREObject>^ mArr = gcnew array<FREObject>(argc);
		for (uint32_t i = 0; i < argc; i++) {
			mArr[i] = argv[i].ToPointer();
		}
		FREObject ret;
		FREObject thrownException = nullptr;

		if (argc == 0) {
			freresult = FRECallObjectMethod(freObject.ToPointer(), reinterpret_cast<const uint8_t *>(methodNameStr.data()), 0, nullptr, &ret, &thrownException);
		} else {
			pin_ptr<FREObject> argvM = &mArr[0];
			freresult = FRECallObjectMethod(freObject.ToPointer(), reinterpret_cast<const uint8_t *>(methodNameStr.data()), argc, argvM, &ret, &thrownException);
		}
		
		if (FRE_OK == freresult) {
			return FREObjectCLR(ret);
		} else {
			return FREObjectCLR(thrownException);
		}

	}

	String ^ FRESharpCLR::getString(FREObjectCLR freObject, UInt32% freresult) {
		uint32_t string1Length;
		const uint8_t *val;
		freresult = FREGetObjectAsUTF8(freObject.ToPointer(), &string1Length, &val);
		std::string ret;
		if (FRE_OK == freresult) {
			ret = std::string(val, val + string1Length);
			return gcnew String(ret.c_str());
		}else{
			return gcnew String("");
		}
	}

	Int32 FRESharpCLR::getInt32(FREObjectCLR freObject, UInt32% freresult) {
		int32_t val = 0;
		freresult = FREGetObjectAsInt32(freObject.ToPointer(), &val);
		return val;
	}

	UInt32 FRESharpCLR::getUInt32(FREObjectCLR freObject, UInt32% freresult) {
		uint32_t val = 0;
		freresult = FREGetObjectAsUint32(freObject.ToPointer(), &val);
		return val;
	}

	UInt32 FRESharpCLR::getArrayLength(FREObjectCLR freObject, UInt32% freresult) {
		auto arrayLengthAS = getProperty(freObject, "length", freresult);
		return getUInt32(arrayLengthAS, freresult);
	}

	FREObjectCLR FRESharpCLR::getObjectAt(FREObjectCLR freObject, UInt32 i, UInt32% freresult) {
		FREObject val = nullptr;
		freresult = FREGetArrayElementAt(freObject.ToPointer(), i, &val);
		return FREObjectCLR(val);
	}

	void FRESharpCLR::setObjectAt(FREObjectCLR freObject, UInt32 i, FREObjectCLR value, UInt32% freresult) {
		freresult = FRESetArrayElementAt(freObject.ToPointer(), i, value.ToPointer());
	}

	bool FRESharpCLR::getBool(FREObjectCLR freObject, UInt32% freresult) {
		uint32_t result = 0;
		auto ret = false;
		freresult = FREGetObjectAsBool(freObject.ToPointer(), &result);
		if (result > 0) ret = true;
		return ret;
	}

	double FRESharpCLR::getDouble(FREObjectCLR freObject, UInt32% freresult) {
		auto val = 0.0;
		freresult = FREGetObjectAsDouble(freObject.ToPointer(), &val);
		return val;
	}

	void FRESharpCLR::dispatchEvent(FREContextCLR freContext, String^ name, String^ value) {
		std::string nameStr = "";
		MarshalString(name, nameStr);
		std::wstring valueWstr = L"";
		MarshalString(value, valueWstr);
		std::string valueStr = wstring_to_utf8(valueWstr);
		FREDispatchStatusEventAsync(freContext.ToPointer(), reinterpret_cast<const uint8_t *>(valueStr.data()),
			reinterpret_cast<const uint8_t *>(nameStr.data()));
	}

	int FRESharpCLR::getType(FREObjectCLR freObject, UInt32% freresult) {
		auto val = FRE_TYPE_NULL;
		freresult = FREGetObjectType(freObject.ToPointer(), &val);
		return val;
	}

	void FRESharpCLR::acquireBitmapData(FREObjectCLR freObject, FREBitmapDataCLR^ descriptorToSet) {
		FREBitmapData2 bitmapData;
		FREAcquireBitmapData2(freObject.ToPointer(), &bitmapData);
		descriptorToSet->width = bitmapData.width;
		descriptorToSet->height = bitmapData.height;
		uint32_t* input = bitmapData.bits32;
		descriptorToSet->bits32 = IntPtr(input);
		descriptorToSet->isInvertedY = bitmapData.isInvertedY;
		descriptorToSet->isPremultiplied = bitmapData.isPremultiplied;
		descriptorToSet->lineStride32 = bitmapData.lineStride32;
	}

	void FRESharpCLR::releaseBitmapData(FREObjectCLR freObject) {
		FREReleaseBitmapData(freObject.ToPointer());
	}

	FREObjectCLR FRESharpCLR::getFREObject(Bitmap^ value, FREBitmapDataCLR^ descriptorToSet, UInt32% freresult) {
		FREObject freBitmap;
		FREObjectCLR width = getFREObject(UInt32(value->Width), freresult);
		if (FRE_OK != freresult) return FREObjectCLR(nullptr);
		FREObjectCLR height = getFREObject(UInt32(value->Height), freresult);
		if (FRE_OK != freresult) return FREObjectCLR(nullptr);
		FREObjectCLR transparent = getFREObject(false, freresult);
		if (FRE_OK != freresult) return FREObjectCLR(nullptr);
		FREObjectCLR fillColor = getFREObject(UInt32(0xFFFFFF), freresult);
		if (FRE_OK != freresult) return FREObjectCLR(nullptr);
		FREObject obs[4] = { width.ToPointer(), height.ToPointer(), transparent.ToPointer(), fillColor.ToPointer() };
		FRENewObject((uint8_t *)"flash.display.BitmapData", 4, obs, &freBitmap, NULL);
		FREBitmapData2 bitmapData;
		FREResult acquireResult = FREAcquireBitmapData2(freBitmap, &bitmapData);
		if (FRE_OK != acquireResult) {
			FREReleaseBitmapData(freBitmap);
			return FREObjectCLR(freBitmap);
		}
		if (&bitmapData.isInvertedY != (uint32_t*)(0)) value->RotateFlip(RotateFlipType::RotateNoneFlipY);
		const int pixelSize = 4;
		Rectangle rect(0, 0, value->Width, value->Height);
		BitmapData^ windowsBitmapData = value->LockBits(rect, ImageLockMode::ReadOnly, PixelFormat::Format32bppArgb);
		 for (int y = 0; y < value->Height; y++) {
			 Byte* oRow;
			 if (Environment::Is64BitProcess) {
				 oRow = (Byte*)windowsBitmapData->Scan0.ToInt64() + (y * windowsBitmapData->Stride);
			 } else {
				 oRow = (Byte*)windowsBitmapData->Scan0.ToInt32() + (y * windowsBitmapData->Stride);
			 }
			Byte* nRow = (Byte*)bitmapData.bits32 + (y * bitmapData.lineStride32 * 4);
			for (int x = 0; x < value->Width; x++) {
				nRow[x * pixelSize] = oRow[x * pixelSize]; //B
				nRow[x * pixelSize + 1] = oRow[x * pixelSize + 1]; //G
				nRow[x * pixelSize + 2] = oRow[x * pixelSize + 2]; //R
			}
		}
		descriptorToSet->width = bitmapData.width;
		descriptorToSet->height = bitmapData.height;
		uint32_t* input = bitmapData.bits32;
		descriptorToSet->bits32 = IntPtr(input);
		descriptorToSet->isInvertedY = bitmapData.isInvertedY;
		descriptorToSet->isPremultiplied = bitmapData.isPremultiplied;
		descriptorToSet->lineStride32 = bitmapData.lineStride32;
		// Free resources
		releaseBitmapData(FREObjectCLR(freBitmap));
		invalidateBitmapDataRect(FREObjectCLR(freBitmap), 0, 0, value->Width, value->Height);

		value->UnlockBits(windowsBitmapData);
		delete windowsBitmapData;
		return FREObjectCLR(freBitmap);
	}

	void FRESharpCLR::invalidateBitmapDataRect(FREObjectCLR freObject, UInt32 x, UInt32 y, UInt32 width, UInt32 height) {
		FREInvalidateBitmapDataRect(freObject.ToPointer(), x, y, width, height);
	}

	void FRESharpCLR::acquireByteArrayData(FREObjectCLR freObject, FREByteArrayCLR^ byteArrayToSet) {
		using namespace Runtime::InteropServices;
		FREByteArray byteArray;
		FREAcquireByteArray(freObject.ToPointer(), &byteArray);
		uint8_t* input = byteArray.bytes;
		byteArrayToSet->length = byteArray.length;
		byteArrayToSet->bytes = IntPtr(input);
	}

	void FRESharpCLR::releaseByteArrayData(FREObjectCLR freObject) {
		FREReleaseByteArray(freObject.ToPointer());
	}

}
