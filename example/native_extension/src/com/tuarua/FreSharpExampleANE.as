/**
 * Created by Eoin Landy on 20/04/2017.
 */
package com.tuarua {
import flash.display.BitmapData;
import flash.events.EventDispatcher;
import flash.events.StatusEvent;
import flash.external.ExtensionContext;
import flash.geom.Rectangle;
import flash.utils.ByteArray;

public class FreSharpExampleANE extends EventDispatcher {
    private static const NAME:String = "FreSharpExampleANE";
    private var extensionContext:ExtensionContext;
    public function FreSharpExampleANE() {
        initiate()
    }

    private function initiate():void {
        trace("[" + NAME + "] Initalizing ANE...");
        try {
            extensionContext = ExtensionContext.createExtensionContext("com.tuarua.FreSharpExampleANE", null);
            extensionContext.addEventListener(StatusEvent.STATUS, gotEvent);
        } catch (e:Error) {
            trace("[" + NAME + "] ANE Not loaded properly.  Future calls will fail.");
        }
    }

    private function gotEvent(event:StatusEvent):void {
        var pObj:Object;
        switch (event.level) {
            case "TRACE":
                trace(event.code);
                break;
        }
    }

    public function runStringTests(value:String):String {
        return extensionContext.call("runStringTests", value) as String;
    }

    public function runNumberTests(value:Number):Number {
        return extensionContext.call("runNumberTests", value) as Number;
    }

    public function runIntTests(value:int, value2:uint):int {
        return extensionContext.call("runIntTests", value, value2) as int;
    }

    public function runArrayTests(value:Array):Array {
        return extensionContext.call("runArrayTests", value) as Array;
    }

    public function runObjectTests(value:Person):Person {
        return extensionContext.call("runObjectTests", value) as Person;
    }

    public function runExtensibleTests():Rectangle {
        return extensionContext.call("runExtensibleTests") as Rectangle;
    }

    public function runBitmapTests(bmd:BitmapData):BitmapData {
       return extensionContext.call("runBitmapTests", bmd) as BitmapData;
    }

    public function runByteArrayTests(byteArray:ByteArray):void {
        extensionContext.call("runByteArrayTests", byteArray);
    }

    public function runDataTests(value:String):String {
        return extensionContext.call("runDataTests", value) as String;
    }

    public function runErrorTests(value:Person, string:String, int:int):void {
        extensionContext.call("runErrorTests", value, string, int);
    }

    public function runErrorTests2(string:String):void {
        var theRet:* = extensionContext.call("runErrorTests2",string);
        if(theRet is ANEError){
            throw theRet as ANEError;
        }
    }

    public function dispose():void {
        if (!extensionContext) {
            trace("[" + NAME + "] Error. ANE Already in a disposed or failed state...");
            return;
        }

        trace("[" + NAME + "] Unloading ANE...");
        extensionContext.removeEventListener(StatusEvent.STATUS, gotEvent);
        extensionContext.dispose();
        extensionContext = null;
    }
}
}
