#include "FRESharpLib.h"
#include <iostream>
#include <vector>

using namespace std;

FRESharpLib::FRESharpLib() : dllContext(nullptr) {}

FRESharpLib::~FRESharpLib() {}

FREObject FRESharpLib::getFREObject(string value) const {
	FREObject result;
	auto status = FRENewObjectFromUTF8(uint32_t(value.length()), reinterpret_cast<const uint8_t *>(value.data()), &result);
	isFREResultOK(status, "Could not convert string to FREObject.");
	return result;
}

FREObject FRESharpLib::getFREObject(double value) const {
	FREObject result;
	auto status = FRENewObjectFromDouble(value, &result);
	isFREResultOK(status, "Could not convert double to FREObject.");
	return result;
}

FREObject FRESharpLib::getFREObject(bool value) const {
	FREObject result;
	auto status = FRENewObjectFromBool(value, &result);
	isFREResultOK(status, "Could not convert bool to FREObject.");
	return result;
}

FREObject FRESharpLib::getFREObject(int32_t value) const {
	FREObject result;
	auto status = FRENewObjectFromInt32(value, &result);
	isFREResultOK(status, "Could not convert int32_t to FREObject.");
	return result;
}

FREObject FRESharpLib::getFREObject(uint32_t value) const {
	FREObject result;
	auto status = FRENewObjectFromUint32(value, &result);
	isFREResultOK(status, "Could not convert uint32_t to FREObject.");
	return result;
}

FREObject FRESharpLib::getFREObject(string className, FREObject* argv, int32_t argc) const {
	FREObject ret;
	FREObject thrownException = nullptr;
	auto status = FRENewObject(reinterpret_cast<const uint8_t *>(className.data()), argc, argv, &ret, &thrownException);
	isFREResultOK(status, "Could not create FREObject.");
	if (FRE_OK != status)
		hasThrownException(thrownException);
	return ret;
}

FREObject FRESharpLib::callMethod(FREObject freObject, string className, FREObject* argv, int32_t argc) const {
	FREObject ret;
	FREObject thrownException = nullptr;
	auto status = FRECallObjectMethod(freObject, reinterpret_cast<const uint8_t *>(className.data()), argc, argv, &ret, &thrownException);
	isFREResultOK(status, "Could not call method on FREObject.");
	if (FRE_OK != status)
		hasThrownException(thrownException);
	return ret;
}

string FRESharpLib::getString(FREObject freObject) const {
	uint32_t string1Length;
	const uint8_t *val;
	auto status = FREGetObjectAsUTF8(freObject, &string1Length, &val);
	if (isFREResultOK(status, "Could not convert UTF8."))
		return string(val, val + string1Length);
	return "";
}

uint32_t FRESharpLib::getUInt32(FREObject freObject) const {
	uint32_t result = 0;
	auto status = FREGetObjectAsUint32(freObject, &result);
	isFREResultOK(status, "Could not convert FREObject to uint32_t.");
	return result;
}

int32_t FRESharpLib::getInt32(FREObject freObject) const {
	int32_t result = 0;
	auto status = FREGetObjectAsInt32(freObject, &result);
	isFREResultOK(status, "Could not convert FREObject to int32_t.");
	return result;
}

double FRESharpLib::getDouble(FREObject freObject) const {
	auto result = 0.0;
	auto status = FREGetObjectAsDouble(freObject, &result);
	isFREResultOK(status, "Could not convert FREObject to double.");
	return result;
}

FREObject FRESharpLib::getProperty(FREObject freObject, string propertyName) const
{
	FREObject result = nullptr;
	FREObject thrownException = nullptr;
	auto status = FREGetObjectProperty(freObject, reinterpret_cast<const uint8_t *>(propertyName.data()), &result, &thrownException);
	isFREResultOK(status, "Could not get FREObject property.");
	if (FRE_OK != status)
		hasThrownException(thrownException);
	return result;
}

void FRESharpLib::setProperty(FREObject freObject, string name, FREObject value) const {
	FREObject thrownException = nullptr;
	auto status = FRESetObjectProperty(freObject, reinterpret_cast<const uint8_t *>(name.c_str()), value, &thrownException);
	isFREResultOK(status, "Could not set FREObject property.");
	if (FRE_OK != status)
		hasThrownException(thrownException);
}

void FRESharpLib::setProperty(FREObject freObject, string name, string value) const {
	FREObject thrownException = nullptr;
	auto status = FRESetObjectProperty(freObject, reinterpret_cast<const uint8_t *>(name.c_str()), getFREObject(value), &thrownException);
	isFREResultOK(status, "Could not set FREObject property.");
	if (FRE_OK != status)
		hasThrownException(thrownException);
}

void FRESharpLib::setProperty(FREObject freObject, string name, double value) const {
	FREObject thrownException = nullptr;
	auto status = FRESetObjectProperty(freObject, reinterpret_cast<const uint8_t *>(name.c_str()), getFREObject(value), &thrownException);
	isFREResultOK(status, "Could not set FREObject property.");
	if (FRE_OK != status)
		hasThrownException(thrownException);
}

void FRESharpLib::setProperty(FREObject freObject, string name, bool value) const {
	FREObject thrownException = nullptr;
	auto status = FRESetObjectProperty(freObject, reinterpret_cast<const uint8_t *>(name.c_str()), getFREObject(value), &thrownException);
	isFREResultOK(status, "Could not set FREObject property.");
	if (FRE_OK != status)
		hasThrownException(thrownException);
}

void FRESharpLib::setProperty(FREObject freObject, string name, int32_t value) const {
	FREObject thrownException = nullptr;
	auto status = FRESetObjectProperty(freObject, reinterpret_cast<const uint8_t *>(name.c_str()), getFREObject(value), &thrownException);
	isFREResultOK(status, "Could not set FREObject property.");
	if (FRE_OK != status)
		hasThrownException(thrownException);
}

void FRESharpLib::setProperty(FREObject freObject, string name, uint32_t value) const {
	FREObject thrownException = nullptr;
	auto status = FRESetObjectProperty(freObject, reinterpret_cast<const uint8_t *>(name.c_str()), getFREObject(value), &thrownException);
	isFREResultOK(status, "Could not set FREObject property.");
	if (FRE_OK != status)
		hasThrownException(thrownException);
}

bool FRESharpLib::getBool(FREObject freObject) {
	uint32_t result = 0;
	auto ret = false;
	FREGetObjectAsBool(freObject, &result);
	if (result > 0) ret = true;
	return ret;
}

bool FRESharpLib::isFREResultOK(FREResult errorCode, string errorMessage) const {
	if (FRE_OK == errorCode) return true;
	auto messageToReport = errorMessage + " " + friendlyFREResult(errorCode);
	trace(messageToReport);
	return false;
}

string FRESharpLib::friendlyFREResult(FREResult errorCode) {
	switch (errorCode) {
	case FRE_OK:
		return "FRE_OK";
	case FRE_NO_SUCH_NAME:
		return "FRE_NO_SUCH_NAME";
	case FRE_INVALID_OBJECT:
		return "FRE_INVALID_OBJECT";
	case FRE_TYPE_MISMATCH:
		return "FRE_TYPE_MISMATCH";
	case FRE_ACTIONSCRIPT_ERROR:
		return "FRE_ACTIONSCRIPT_ERROR";
	case FRE_INVALID_ARGUMENT:
		return "FRE_INVALID_ARGUMENT";
	case FRE_READ_ONLY:
		return "FRE_READ_ONLY";
	case FRE_WRONG_THREAD:
		return "FRE_WRONG_THREAD";
	case FRE_ILLEGAL_STATE:
		return "FRE_ILLEGAL_STATE";
	case FRE_INSUFFICIENT_MEMORY:
		return "FRE_INSUFFICIENT_MEMORY";
	default:
		return "";
	}
}


void FRESharpLib::setFREContext(FREContext ctx) {
	dllContext = ctx;
}

void FRESharpLib::trace(string message) const
{
	dispatchEvent("TRACE", message);
}
void FRESharpLib::dispatchEvent(string name, string value) const {
	FREDispatchStatusEventAsync(dllContext, reinterpret_cast<const uint8_t *>(value.data()), reinterpret_cast<const uint8_t *>(name.data()));
}

bool FRESharpLib::hasThrownException(FREObject thrownException) const {
	if (thrownException == nullptr) return false;

	FREObjectType objectType;
	if (FRE_OK != FREGetObjectType(thrownException, &objectType)) {
		trace("Exception was thrown, but failed to obtain information about it");
		return true;
	}

	if (FRE_TYPE_OBJECT == objectType) {
		FREObject exceptionTextAS;
		FREObject newException;
		if (FRE_OK != FRECallObjectMethod(thrownException, reinterpret_cast<const uint8_t *>("toString"), 0, nullptr, &exceptionTextAS, &newException)) {
			trace("Exception was thrown, but failed to obtain information about it");
			return true;
		}
		return true;
	}

	return false;
}

int FRESharpLib::getType(FREObject freObject) {
	auto objectType = FRE_TYPE_NULL;
	FREGetObjectType(freObject, &objectType);
	return objectType;
}

uint32_t FRESharpLib::getArrayLength(FREObject freObject) {
	auto arrayLengthAS = getProperty(freObject, "length");
	return getUInt32(arrayLengthAS);
}

FREObject FRESharpLib::getObjectAt(FREObject freObject, uint32_t i) {
	FREObject ret = nullptr;
	FREGetArrayElementAt(freObject, i, &ret);
	return ret;
}
