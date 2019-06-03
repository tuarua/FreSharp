package com.tuarua {
import flash.external.ExtensionContext;
import flash.system.Capabilities;

public class FreSharp {
    private static const NAME:String = "FreSharp";
    private var ctx:ExtensionContext;

    public function FreSharp() {
        if (!isWindows()) return;
        trace("[" + NAME + "] Initializing ANE...");
        try {
            ctx = ExtensionContext.createExtensionContext("com.tuarua." + NAME, null);
            ctx.call("initFreSharp");
        } catch (e:Error) {
            trace(e.message);
            trace(e.getStackTrace());
            trace(e.errorID);
            trace(e.name);
            trace("[" + NAME + "] ANE Not loaded properly.  Future calls will fail.");
        }
    }

    public function dispose():void {
        if (!isWindows()) return;
        if (!ctx) {
            trace("[" + NAME + "] Error. ANE Already in a disposed or failed state...");
            return;
        }

        trace("[" + NAME + "] Unloading ANE...");
        ctx.dispose();
        ctx = null;
    }

    private static function isWindows():Boolean {
        return Capabilities.version.substr(0, 3).toLowerCase() == "win";
    }
}
}
