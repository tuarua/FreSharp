#pragma once
#include "FlashRuntimeExtensions.h"
extern "C" {
	__declspec(dllexport) void TRFSExtInizer(void** extData, FREContextInitializer* ctxInitializer, FREContextFinalizer* ctxFinalizer);
	__declspec(dllexport) void TRFSExtFinizer(void* extData);
}

