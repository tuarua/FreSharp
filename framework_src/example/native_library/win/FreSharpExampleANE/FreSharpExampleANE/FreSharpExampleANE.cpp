#include "FreSharpMacros.h"
#include "FreSharpExampleANE.h"
#include "FlashRuntimeExtensions.h"
#include "stdafx.h"
#include "FreSharpBridge.h"

extern "C" {

	[System::STAThreadAttribute]
	BOOL APIENTRY FreSharpExampleANEMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved) {
		switch (ul_reason_for_call) {
		case DLL_PROCESS_ATTACH:
		case DLL_THREAD_ATTACH:
		case DLL_THREAD_DETACH:
		case DLL_PROCESS_DETACH:
			break;
		default: ;
		}
		return true;
	}

	CONTEXT_INIT(TRFS) {

		FREBRIDGE_INIT

		static FRENamedFunction extensionFunctions[] = {
			 MAP_FUNCTION(runStringTests)
			,MAP_FUNCTION(runNumberTests)
			,MAP_FUNCTION(runIntTests)
			,MAP_FUNCTION(runArrayTests)
			,MAP_FUNCTION(runObjectTests)
			,MAP_FUNCTION(runExtensibleTests)
			,MAP_FUNCTION(runBitmapTests)
			,MAP_FUNCTION(runByteArrayTests)
			,MAP_FUNCTION(runErrorTests)
			,MAP_FUNCTION(runDataTests)
			,MAP_FUNCTION(runErrorTests2)
		};

		SET_FUNCTIONS

	}

	CONTEXT_FIN(TRFS) {
		FreSharpBridge::GetController()->OnFinalize();
	}

	EXTENSION_INIT(TRFS)

	EXTENSION_FIN(TRFS)

}
