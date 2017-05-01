#include "FreSharpExampleANE.h"
#include "FlashRuntimeExtensions.h"
#include <vector>

std::vector<std::string> funcArray;

#include "stdafx.h"

namespace ManagedCode {
	using namespace System;
	using namespace System::Windows;
	using namespace System::Windows::Interop;
	using namespace System::Windows::Media;
	using namespace System::Collections::Generic;
	using FREObjectSharp = IntPtr;
	using FREContextSharp = IntPtr;
	using FREArgvSharp = array<FREObjectSharp>^;

	ref class ManagedGlobals {
	public:
		static FreExampleSharpLib::MainController^ controller = nullptr;
	};

	array<FREObjectSharp>^ MarshalFREArray(array<FREObject>^ argv, uint32_t argc) {
		array<FREObjectSharp>^ arr = gcnew array<FREObjectSharp>(argc);
		for (uint32_t i = 0; i < argc; i++) {
			arr[i] = FREObjectSharp(argv[i]);
		}
		return arr;
	}

	void MarshalString(String ^ s, std::string& os) {
		using namespace Runtime::InteropServices;
		const char* chars =
			(const char*)(Marshal::StringToHGlobalAnsi(s)).ToPointer();
		os = chars;
		Marshal::FreeHGlobal(FREObjectSharp((void*)chars));
	}


	FREObject CallSharpFunction(String^ name, FREContext context, array<FREObject>^ argv, uint32_t argc) {
		return (FREObject)ManagedGlobals::controller->CallSharpFunction(name, FREContextSharp(context), argc, MarshalFREArray(argv, argc));
	}

	void SetFREContext(FREContext freContext) {
		ManagedGlobals::controller->SetFreContext(FREContextSharp(freContext));
	}

	void InitController() {
		ManagedGlobals::controller = gcnew FreExampleSharpLib::MainController();
	}


	std::vector<std::string> GetFunctions() {
		std::vector<std::string> ret;
		array<String^>^ mArray = ManagedGlobals::controller->GetFunctions();
		int i = 0;
		for (i = 0; i < mArray->Length; ++i) {
			std::string itemStr = "";
			MarshalString(mArray[i], itemStr);
			ret.push_back(itemStr);
		}
		return ret;
	}

}

extern "C" {

#define FRE_FUNCTION(fn) FREObject (fn)(FREContext context, void* functionData, uint32_t argc, FREObject argv[])

	array<FREObject>^ getArgvAsArray(FREObject argv[], uint32_t argc) {
		array<FREObject>^ arr = gcnew array<FREObject>(argc);
		for (uint32_t i = 0; i < argc; i++) {
			arr[i] = argv[i];
		}
		return arr;
	}

	FRE_FUNCTION(callSharpFunction) {
		std::string fName = std::string((const char*)functionData);
		return ManagedCode::CallSharpFunction(gcnew System::String(fName.c_str()), context, getArgvAsArray(argv, argc), argc);
	}

	[System::STAThreadAttribute]
	BOOL APIENTRY WebViewANEMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved) {
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

		ManagedCode::InitController();
		ManagedCode::SetFREContext(ctx);
		funcArray = ManagedCode::GetFunctions();

		//TODO how to pass functionData without losing the string reference

		static FRENamedFunction extensionFunctions[] = {
			{ (const uint8_t *) "runStringTests","runStringTests", &callSharpFunction }
			,{ (const uint8_t *) "runNumberTests","runNumberTests", &callSharpFunction }
			,{ (const uint8_t *) "runIntTests","runIntTests", &callSharpFunction }
			,{ (const uint8_t *) "runArrayTests","runArrayTests", &callSharpFunction }
			,{ (const uint8_t *) "runObjectTests","runObjectTests", &callSharpFunction }
			,{ (const uint8_t *) "runBitmapTests","runBitmapTests", &callSharpFunction }
			,{ (const uint8_t *) "runByteArrayTests","runByteArrayTests", &callSharpFunction }
			,{ (const uint8_t *) "runErrorTests","runErrorTests", &callSharpFunction }
			,{ (const uint8_t *) "runDataTests","runDataTests", &callSharpFunction }
			,{ (const uint8_t *) "runErrorTests2","runErrorTests2", &callSharpFunction }
			
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
