#pragma once
#include <string>
#include "FlashRuntimeExtensions.h"
#include <vector>

class FRESharpLib {
public:
	FRESharpLib();
	~FRESharpLib();

	
	FREObject getFREObject(std::string value) const;
	FREObject getFREObject(double value) const;
	FREObject getFREObject(bool value) const;
	FREObject getFREObject(int32_t value) const;
	FREObject getFREObject(uint32_t value) const;
	FREObject getFREObject(std::string className, FREObject* argv, int32_t argc) const;
	
	FREObject callMethod(FREObject freObject, std::string className, FREObject* argv, int32_t argc) const;

	std::string getString(FREObject freObject) const;
	uint32_t getUInt32(FREObject freObject) const;
	int32_t getInt32(FREObject freObject) const;
	static bool getBool(FREObject freObject);
	double getDouble(FREObject freObject) const;
	FREObject getProperty(FREObject freObject, std::string propertyName) const;
	

	void setProperty(FREObject freObject, std::string name, FREObject value) const;
	void setProperty(FREObject freObject, std::string name, std::string value) const;
	void setProperty(FREObject freObject, std::string name, double value) const;
	void setProperty(FREObject freObject, std::string name, bool value) const;
	void setProperty(FREObject freObject, std::string name, int32_t value) const;
	void setProperty(FREObject freObject, std::string name, uint32_t value) const;

	uint32_t getArrayLength(FREObject freObject) const;
	static FREObject getObjectAt(FREObject freObject, uint32_t i);
	void setObjectAt(FREObject freObject, uint32_t i, FREObject value) const;
	void trace(std::string message) const;
	void dispatchEvent(std::string name, std::string value) const;
	void setFREContext(FREContext ctx);
	static int getType(FREObject freObject);
	FREContext dllContext;
private:
	bool isFREResultOK(FREResult errorCode, std::string errorMessage) const;
	static std::string friendlyFREResult(FREResult errorCode);
	bool hasThrownException(FREObject thrownException) const;
};
