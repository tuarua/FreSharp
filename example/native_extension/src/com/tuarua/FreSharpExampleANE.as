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
    public function FreSharpExampleANE() {
        initiate()
    }

    private function initiate():void {
        trace("[" + NAME + "] Initalizing ANE...");
        try {
            ANEContext.ctx = ExtensionContext.createExtensionContext("com.tuarua.FreSharpExampleANE", null);
            ANEContext.ctx.addEventListener(StatusEvent.STATUS, gotEvent);
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
        return ANEContext.ctx.call("runStringTests", value) as String;
    }

    public function runNumberTests(value:Number):Number {
        return ANEContext.ctx.call("runNumberTests", value) as Number;
    }

    public function runIntTests(value:int, value2:uint):int {
        return ANEContext.ctx.call("runIntTests", value, value2) as int;
    }

    public function runArrayTests(value:Array):Array {
        return ANEContext.ctx.call("runArrayTests", value) as Array;
    }

    public function runObjectTests(value:Person):Person {
        return ANEContext.ctx.call("runObjectTests", value) as Person;
    }

    public function runExtensibleTests(value:Rectangle):Rectangle {
        return ANEContext.ctx.call("runExtensibleTests", value) as Rectangle;
    }

    public function runBitmapTests(bmd:BitmapData):BitmapData {
       return ANEContext.ctx.call("runBitmapTests", bmd) as BitmapData;
    }

    public function runByteArrayTests(byteArray:ByteArray):void {
        ANEContext.ctx.call("runByteArrayTests", byteArray);
    }

    public function runDataTests(value:String):String {
        return ANEContext.ctx.call("runDataTests", value) as String;
    }

    public function runErrorTests(value:Person, string:String, int:int):void {
        ANEContext.ctx.call("runErrorTests", value, string, int);
    }

    public function runErrorTests2(string:String):void {
        var theRet:* = ANEContext.ctx.call("runErrorTests2",string);
        if(theRet is ANEError){
            throw theRet as ANEError;
        }
    }

    public function runNativeTests():void {
        ANEContext.ctx.call("runNativeTests");
    }

    public function dispose():void {
        if (!ANEContext.ctx) {
            trace("[" + NAME + "] Error. ANE Already in a disposed or failed state...");
            return;
        }

        trace("[" + NAME + "] Unloading ANE...");
        ANEContext.ctx.removeEventListener(StatusEvent.STATUS, gotEvent);
        ANEContext.ctx.dispose();
        ANEContext.ctx = null;
    }
}
}
