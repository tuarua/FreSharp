#include "FreSharpMacros.h"
#include "HelloWorldANE.h"
#include "FlashRuntimeExtensions.h"
#include "stdafx.h"
#include "FreSharpBridge.h"

// XXXX is a placeholder for a unique identifier. Replace with, for example, your company initials and ANE ID eg MCHW
// This must also be set in HelloWorldANE.h !
// This must also be set in starter_project\native_extension\ane\extension_win.xml !

extern "C" {
	CONTEXT_INIT(XXXX) {
		FREBRIDGE_INIT

		/**************************************************************************/
		/******* MAKE SURE TO ADD FUNCTIONS HERE THE SAME AS MAINCONTROLLER.CS *****/
		/**************************************************************************/

		static FRENamedFunction extensionFunctions[] = {
			 MAP_FUNCTION(init)
			,MAP_FUNCTION(sayHello)
		};

		SET_FUNCTIONS
	}

	CONTEXT_FIN(XXXX) {
	}
	EXTENSION_INIT(XXXX)
	EXTENSION_FIN(XXXX)

}

