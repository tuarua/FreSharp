#include "FreSharpMacros.h"
#include "FreSharpANE.h"
#include "stdafx.h"

#define FRE_FUNCTION(fn) FREObject (fn)(FREContext context, void* functionData, uint32_t argc, FREObject argv[])
namespace AssemblyBridge {
	using namespace System;
	using namespace Windows;
	using namespace Interop;
	using namespace Windows::Media;
	using namespace Collections::Generic;
	void ResolveAssemblies();
	Reflection::Assembly^ GetAssembly(String^ baseDir, String^ dllName) {
		String^ platform = Environment::Is64BitProcess ? "x86-64" : "x86";
		auto directories = IO::Directory::GetDirectories(baseDir);
		for each (String^ dir in directories) {
			auto fileName = dir + "\\META-INF\\ANE\\Windows-" + platform + "\\" + dllName;
			if (IO::File::Exists(fileName)) {
				return Reflection::Assembly::LoadFile(fileName);
			}
		}
		return nullptr;
	}
	Reflection::Assembly ^ ResolveHandler(Object^ Sender, ResolveEventArgs^ args) {
		auto CommandLineArgs = Environment::GetCommandLineArgs();
		bool isAdl = false;
		String^ baseDir;
		for (auto i = 0; i < CommandLineArgs->Length; i++) {
			auto item = CommandLineArgs[i];
			if (item->Equals("-extdir")) {
				isAdl = true;
				baseDir = CommandLineArgs[i + 1];
				break;
			}
		}
		auto dllName = args->Name->Split(',', 2)[0] + ".dll";
		if (isAdl) {
			return GetAssembly(baseDir, dllName);
		}
		else {
			auto fullPath = System::Diagnostics::Process::GetCurrentProcess()->MainModule->FileName;
			baseDir = IO::Path::GetDirectoryName(fullPath) + "\\META-INF\\AIR\\extensions";
			return GetAssembly(baseDir, dllName);
		}
		return nullptr;
	}
	void ResolveAssemblies() {
		AppDomain::CurrentDomain->AssemblyResolve += gcnew ResolveEventHandler(ResolveHandler);
	}
	
}

extern "C" {

	FRE_FUNCTION(initFreSharp) {
		return NULL;
	}

	CONTEXT_INIT(TRFSHRP) {
		static FRENamedFunction extensionFunctions[] = {
			{reinterpret_cast<const uint8_t *>("initFreSharp"), NULL, &initFreSharp}
		};
		SET_FUNCTIONS
	}

	CONTEXT_FIN(TRFSHRP) {
		//any clean up code here
	}
	
	void (TRFSHRPExtInizer) (void **extData, FREContextInitializer *ctxInitializer, FREContextFinalizer *ctxFinalizer) {
		AssemblyBridge::ResolveAssemblies();
		*ctxInitializer = &TRFSHRP_contextInitializer;
		*ctxFinalizer = &TRFSHRP_contextFinalizer;
	}

	EXTENSION_FIN(TRFSHRP)
}
