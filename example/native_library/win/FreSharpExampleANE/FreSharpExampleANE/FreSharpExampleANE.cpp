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
		}
		return true;
	}

	void contextInitializer(void* extData, const uint8_t* ctxType, FREContext ctx, uint32_t* numFunctionsToSet, const FRENamedFunction** functionsToSet) {

		

		FreSharpBridge::InitController();
		FreSharpBridge::SetFREContext(ctx);
		FreSharpBridge::GetFunctions();

		//TODO how to pass functionData without losing the string reference

		static FRENamedFunction extensionFunctions[] = {
			{ (const uint8_t *) "runStringTests","runStringTests", &callSharpFunction }
			,{ (const uint8_t *) "runNumberTests","runNumberTests", &callSharpFunction }
			,{ (const uint8_t *) "runIntTests","runIntTests", &callSharpFunction }
			,{ (const uint8_t *) "runArrayTests","runArrayTests", &callSharpFunction }
			,{ (const uint8_t *) "runObjectTests","runObjectTests", &callSharpFunction }
			,{ (const uint8_t *) "runExtensibleTests","runExtensibleTests", &callSharpFunction }
			,{ (const uint8_t *) "runBitmapTests","runBitmapTests", &callSharpFunction }
			,{ (const uint8_t *) "runByteArrayTests","runByteArrayTests", &callSharpFunction }
			,{ (const uint8_t *) "runErrorTests","runErrorTests", &callSharpFunction }
			,{ (const uint8_t *) "runDataTests","runDataTests", &callSharpFunction }
			,{ (const uint8_t *) "runErrorTests2","runErrorTests2", &callSharpFunction }
			// Here are the functions for FreNativeStage
			,{ (const uint8_t *) "initNativeStage","initNativeStage", &callSharpFunction }
			,{ (const uint8_t *) "addNativeStage","addNativeStage", &callSharpFunction }
			,{ (const uint8_t *) "updateNativeStage","updateNativeStage", &callSharpFunction }
			,{ (const uint8_t *) "addNativeChild","addNativeChild", &callSharpFunction }
			,{ (const uint8_t *) "updateNativeChild","updateNativeChild", &callSharpFunction }
			//
			
		};


		*numFunctionsToSet = sizeof(extensionFunctions) / sizeof(FRENamedFunction);
		*functionsToSet = extensionFunctions;

	}

	void contextFinalizer(FREContext ctx) {
		return;
	}

	void TRFSExtInizer(void** extData, FREContextInitializer* ctxInitializer, FREContextFinalizer* ctxFinalizer) {
		*ctxInitializer = &contextInitializer;
		*ctxFinalizer = &contextFinalizer;
	}

	void TRFSExtFinizer(void* extData) {
		FREContext nullCTX;
		nullCTX = 0;
		contextFinalizer(nullCTX);
		return;
	}
}
