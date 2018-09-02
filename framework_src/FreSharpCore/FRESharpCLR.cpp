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

	std::string wideStrToUtf8(const std::wstring& str) {
		std::wstring_convert<std::codecvt_utf8<wchar_t>> converted;
		return converted.to_bytes(str);
	}

	void MarshalString(String ^ s, std::wstring& os) {
		using namespace Runtime::InteropServices;
		const wchar_t* chars = static_cast<const wchar_t*>(Marshal::StringToHGlobalUni(s).ToPointer());
		os = chars;
		// ReSharper disable once CppCStyleCast
		Marshal::FreeHGlobal(IntPtr((void*)chars));
	}

	void MarshalString(String^ s, std::string& os) {
		using namespace Runtime::InteropServices;
		const char* chars = static_cast<const char*>(Marshal::StringToHGlobalAnsi(s).ToPointer());
		os = chars;
		// ReSharper disable once CppCStyleCast
		Marshal::FreeHGlobal(IntPtr((void*)chars));
	}

	FREObjectCLR FRESharpCLR::getFREObject(String^ value, UInt32% freResult) {
		std::wstring valueWideStr = L"";
		MarshalString(value, valueWideStr);
		auto valueStr = wideStrToUtf8(valueWideStr);
		FREObject result;
		freResult = FRENewObjectFromUTF8(uint32_t(valueStr.length()), 
			reinterpret_cast<const uint8_t *>(valueStr.data()), &result);
		return FREObjectCLR(result);
	}

	// ReSharper disable once CppMemberFunctionMayBeStatic
	FREObjectCLR FRESharpCLR::getFREObject(const double value, UInt32% freResult) {
		FREObject result;
		freResult = FRENewObjectFromDouble(value, &result);
		return FREObjectCLR(result);
	}

	FREObjectCLR FRESharpCLR::getProperty(FREObjectCLR freObject, String^ propertyName, UInt32% freResult) {
		std::string propertyNameStr = "";
		MarshalString(propertyName, propertyNameStr);
		FREObject ret = nullptr;
		FREObject thrownException = nullptr;
		freResult = FREGetObjectProperty(freObject.ToPointer(), reinterpret_cast<const uint8_t *>(propertyNameStr.data()),
			&ret, &thrownException);

		if (FRE_OK == freResult) {
			return FREObjectCLR(ret);
		}
		return FREObjectCLR(thrownException);
	}

	FREObjectCLR FRESharpCLR::setProperty(FREObjectCLR freObject, String^ name, FREObjectCLR value, UInt32% freResult) {
		std::string nameStr = "";
		MarshalString(name, nameStr);
		FREObject ret = nullptr;
		FREObject thrownException = nullptr;
		freResult = FRESetObjectProperty(freObject.ToPointer(), reinterpret_cast<const uint8_t *>(nameStr.c_str()),
			value.ToPointer(), &thrownException);

		if (FRE_OK == freResult) {
			return FREObjectCLR(ret);
		}
		return FREObjectCLR(thrownException);
	}

	// ReSharper disable once CppMemberFunctionMayBeStatic
	FREObjectCLR FRESharpCLR::getFREObject(const bool value, UInt32% freResult) {
		FREObject result;
		freResult = FRENewObjectFromBool(value, &result);
		return FREObjectCLR(result);
	}

	// ReSharper disable once CppMemberFunctionMayBeStatic
	FREObjectCLR FRESharpCLR::getFREObject(const Int32 value, UInt32% freResult) {
		FREObject result;
		freResult = FRENewObjectFromInt32(value, &result);
		return FREObjectCLR(result);
	}

	// ReSharper disable once CppMemberFunctionMayBeStatic
	FREObjectCLR FRESharpCLR::getFREObject(const UInt32 value, UInt32% freResult) {
		FREObject result;
		freResult = FRENewObjectFromUint32(value, &result);
		return FREObjectCLR(result);
	}

	FREObjectCLR FRESharpCLR::getFREObject(String^ className, array<FREObjectCLR>^ argv, 
		const UInt32 argc, UInt32% freResult) {
		std::string classNameStr = "";
		MarshalString(className, classNameStr);

		auto mArr = gcnew array<FREObject>(argc);
		for (uint32_t i = 0; i < argc; i++) {
			mArr[i] = argv[i].ToPointer();
		}

		FREObject ret;
		FREObject thrownException = nullptr;

		if (argc == 0) {
			freResult = FRENewObject(reinterpret_cast<const uint8_t *>(classNameStr.data()), 0, nullptr, 
				&ret, &thrownException);
		} else {
			const pin_ptr<FREObject> argvM = &mArr[0];
			freResult = FRENewObject(reinterpret_cast<const uint8_t *>(classNameStr.data()), argc, argvM, 
				&ret, &thrownException);
		}
		if (FRE_OK == freResult) {
			return FREObjectCLR(ret);
		}
		return FREObjectCLR(thrownException);
	}

	FREObjectCLR FRESharpCLR::callMethod(FREObjectCLR freObject, String ^ methodName, 
		array<FREObjectCLR>^ argv, const UInt32 argc, UInt32% freResult) {
		std::string methodNameStr = "";
		MarshalString(methodName, methodNameStr);
		auto mArr = gcnew array<FREObject>(argc);
		for (uint32_t i = 0; i < argc; i++) {
			mArr[i] = argv[i].ToPointer();
		}
		FREObject ret;
		FREObject thrownException = nullptr;

		if (argc == 0) {
			freResult = FRECallObjectMethod(freObject.ToPointer(), 
				reinterpret_cast<const uint8_t *>(methodNameStr.data()), 0, 
				nullptr, &ret, &thrownException);
		} else {
			const pin_ptr<FREObject> argvM = &mArr[0];
			freResult = FRECallObjectMethod(freObject.ToPointer(), 
				reinterpret_cast<const uint8_t *>(methodNameStr.data()), argc, 
				argvM, &ret, &thrownException);
		}
		
		if (FRE_OK == freResult) {
			return FREObjectCLR(ret);
		}
		return FREObjectCLR(thrownException);

	}

	// ReSharper disable once CppMemberFunctionMayBeStatic
	String ^ FRESharpCLR::getString(FREObjectCLR freObject, UInt32% freResult) {
		uint32_t string1Length;
		const uint8_t *val;
		freResult = FREGetObjectAsUTF8(freObject.ToPointer(), &string1Length, &val);
		std::string ret;
		if (FRE_OK == freResult) {
			ret = std::string(val, val + string1Length);
			return gcnew String(ret.c_str());
		}
		return gcnew String("");
	}

	// ReSharper disable once CppMemberFunctionMayBeStatic
	Int32 FRESharpCLR::getInt32(FREObjectCLR freObject, UInt32% freResult) {
		auto val = 0;
		freResult = FREGetObjectAsInt32(freObject.ToPointer(), &val);
		return val;
	}

	// ReSharper disable once CppMemberFunctionMayBeStatic
	UInt32 FRESharpCLR::getUInt32(FREObjectCLR freObject, UInt32% freResult) {
		uint32_t val = 0;
		freResult = FREGetObjectAsUint32(freObject.ToPointer(), &val);
		return val;
	}

	UInt32 FRESharpCLR::getArrayLength(FREObjectCLR freObject, UInt32% freResult) {
		auto arrayLengthAS = getProperty(freObject, "length", freResult);
		return getUInt32(arrayLengthAS, freResult);
	}

	// ReSharper disable once CppMemberFunctionMayBeStatic
	FREObjectCLR FRESharpCLR::getObjectAt(FREObjectCLR freObject, const UInt32 i, UInt32% freResult) {
		FREObject val = nullptr;
		freResult = FREGetArrayElementAt(freObject.ToPointer(), i, &val);
		return FREObjectCLR(val);
	}

	// ReSharper disable once CppMemberFunctionMayBeStatic
	void FRESharpCLR::setObjectAt(FREObjectCLR freObject, const UInt32 i, FREObjectCLR value, UInt32% freResult) {
		freResult = FRESetArrayElementAt(freObject.ToPointer(), i, value.ToPointer());
	}

	// ReSharper disable once CppMemberFunctionMayBeStatic
	bool FRESharpCLR::getBool(FREObjectCLR freObject, UInt32% freResult) {
		uint32_t result = 0;
		auto ret = false;
		freResult = FREGetObjectAsBool(freObject.ToPointer(), &result);
		if (result > 0) ret = true;
		return ret;
	}

	// ReSharper disable once CppMemberFunctionMayBeStatic
	double FRESharpCLR::getDouble(FREObjectCLR freObject, UInt32% freResult) {
		auto val = 0.0;
		freResult = FREGetObjectAsDouble(freObject.ToPointer(), &val);
		return val;
	}

	void FRESharpCLR::dispatchEvent(FREContextCLR freContext, String^ name, String^ value) {
		std::string nameStr = "";
		MarshalString(name, nameStr);
		std::wstring valueWideStr = L"";
		MarshalString(value, valueWideStr);
		auto valueStr = wideStrToUtf8(valueWideStr);
		FREDispatchStatusEventAsync(freContext.ToPointer(), reinterpret_cast<const uint8_t *>(valueStr.data()),
			reinterpret_cast<const uint8_t *>(nameStr.data()));
	}

	// ReSharper disable once CppMemberFunctionMayBeStatic
	int FRESharpCLR::getType(FREObjectCLR freObject, UInt32% freResult) {
		auto val = FRE_TYPE_NULL;
		freResult = FREGetObjectType(freObject.ToPointer(), &val);
		return val;
	}

	// ReSharper disable once CppMemberFunctionMayBeStatic
	void FRESharpCLR::acquireBitmapData(FREObjectCLR freObject, FREBitmapDataCLR^ descriptorToSet) {
		FREBitmapData2 bitmapData;
		FREAcquireBitmapData2(freObject.ToPointer(), &bitmapData);
		descriptorToSet->width = bitmapData.width;
		descriptorToSet->height = bitmapData.height;
		auto input = bitmapData.bits32;
		descriptorToSet->bits32 = IntPtr(input);
		descriptorToSet->isInvertedY = bitmapData.isInvertedY;
		descriptorToSet->isPremultiplied = bitmapData.isPremultiplied;
		descriptorToSet->lineStride32 = bitmapData.lineStride32;
	}

	// ReSharper disable once CppMemberFunctionMayBeStatic
	void FRESharpCLR::releaseBitmapData(FREObjectCLR freObject) {
		FREReleaseBitmapData(freObject.ToPointer());
	}

	FREObjectCLR FRESharpCLR::getFREObject(Bitmap^ value, FREBitmapDataCLR^ descriptorToSet, UInt32% freResult) {
		FREObject freBitmap;
		auto width = getFREObject(UInt32(value->Width), freResult);
		if (FRE_OK != freResult) return FREObjectCLR(nullptr);
		auto height = getFREObject(UInt32(value->Height), freResult);
		if (FRE_OK != freResult) return FREObjectCLR(nullptr);
		auto transparent = getFREObject(false, freResult);
		if (FRE_OK != freResult) return FREObjectCLR(nullptr);
		auto fillColor = getFREObject(UInt32(0xFFFFFF), freResult);
		if (FRE_OK != freResult) return FREObjectCLR(nullptr);
		FREObject obs[4] = { width.ToPointer(), height.ToPointer(), transparent.ToPointer(), fillColor.ToPointer() };
		FRENewObject(reinterpret_cast<const uint8_t *>("flash.display.BitmapData"), 4, obs, &freBitmap, nullptr);
		FREBitmapData2 bitmapData;
		const auto acquireResult = FREAcquireBitmapData2(freBitmap, &bitmapData);
		if (FRE_OK != acquireResult) {
			FREReleaseBitmapData(freBitmap);
			return FREObjectCLR(freBitmap);
		}
		if (&bitmapData.isInvertedY != nullptr) value->RotateFlip(RotateFlipType::RotateNoneFlipY);
		const auto pixelSize = 4;
		Rectangle rect(0, 0, value->Width, value->Height);
		auto windowsBitmapData = value->LockBits(rect, ImageLockMode::ReadOnly, PixelFormat::Format32bppArgb);
		 for (auto y = 0; y < value->Height; y++) {
			 Byte* oRow;
			 if (Environment::Is64BitProcess) {
				 oRow = reinterpret_cast<Byte*>(windowsBitmapData->Scan0.ToInt64()) + y * windowsBitmapData->Stride;
			 } else {
				 oRow = reinterpret_cast<Byte*>(windowsBitmapData->Scan0.ToInt32()) + y * windowsBitmapData->Stride;
			 }
			 const auto nRow = reinterpret_cast<Byte*>(bitmapData.bits32) + y * bitmapData.lineStride32 * 4;
			for (auto x = 0; x < value->Width; x++) {
				nRow[x * pixelSize] = oRow[x * pixelSize]; //B
				nRow[x * pixelSize + 1] = oRow[x * pixelSize + 1]; //G
				nRow[x * pixelSize + 2] = oRow[x * pixelSize + 2]; //R
			}
		}
		descriptorToSet->width = bitmapData.width;
		descriptorToSet->height = bitmapData.height;
		auto input = bitmapData.bits32;
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

	
	// ReSharper disable once CppMemberFunctionMayBeStatic
	void FRESharpCLR::invalidateBitmapDataRect(FREObjectCLR freObject, const UInt32 x, const UInt32 y, const UInt32 width, const UInt32 height) {
		FREInvalidateBitmapDataRect(freObject.ToPointer(), x, y, width, height);
	}

	// ReSharper disable CppMemberFunctionMayBeStatic
	void FRESharpCLR::acquireByteArrayData(FREObjectCLR freObject, FREByteArrayCLR^ byteArrayToSet) {
		using namespace Runtime::InteropServices;
		FREByteArray byteArray;
		FREAcquireByteArray(freObject.ToPointer(), &byteArray);
		auto input = byteArray.bytes;
		byteArrayToSet->length = byteArray.length;
		byteArrayToSet->bytes = IntPtr(input);
	}

	// ReSharper disable CppMemberFunctionMayBeStatic
	void FRESharpCLR::releaseByteArrayData(FREObjectCLR freObject) {
		// ReSharper restore CppMemberFunctionMayBeStatic
		FREReleaseByteArray(freObject.ToPointer());
	}

}
