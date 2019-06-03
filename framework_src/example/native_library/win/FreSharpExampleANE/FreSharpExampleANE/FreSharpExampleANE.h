#pragma once
#include "FreSharpMacros.h"
#include "FlashRuntimeExtensions.h"
extern "C" {
	__declspec(dllexport) EXTENSION_INIT_DECL(TRFS);
	__declspec(dllexport) EXTENSION_FIN_DECL(TRFS);
}

