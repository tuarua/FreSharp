/**
 * Created by Eoin Landy on 18/05/2017.
 */
package com.tuarua.fre {
import com.tuarua.*;
import com.tuarua.fre.display.NativeDisplayObject;

import flash.display.Stage;
import flash.events.FullScreenEvent;

import flash.geom.Rectangle;

[RemoteClass(alias="com.tuarua.fre.NativeStage")]
public final class NativeStage {
    private static var _viewPort:Rectangle;
    private static var _visible:Boolean = true;
    private static const _id:String = "root";

    public function NativeStage() {
    }

    public static function addChild(nativeDisplayObject:NativeDisplayObject):void {
        if (ANEContext.ctx) {
            try {
                ANEContext.ctx.call("addNativeChild", _id, nativeDisplayObject);
                nativeDisplayObject.isAdded = true;
            } catch (e:Error) {
                trace(e.message);
            }
        }
    }

    //TODO add stage as param, add FullSevent
    public static function init(stage:Stage, viewPort:Rectangle, visible:Boolean, transparent:Boolean, backgroundColor:uint = 0):void {
        stage.addEventListener(FullScreenEvent.FULL_SCREEN, onFullScreenEvent);
        _viewPort = viewPort;
        _visible = visible;
        if (ANEContext.ctx) {
            try {
                ANEContext.ctx.call("initNativeStage", _viewPort, _visible, transparent, backgroundColor);
            } catch (e:Error) {
                trace(e.message);
            }
        }
    }

    private static function onFullScreenEvent(event:FullScreenEvent):void {
        trace("gone fullscreen", event.fullScreen);
    }

    public static function add():void {
        if (ANEContext.ctx) {
            try {
                ANEContext.ctx.call("addNativeStage");
            } catch (e:Error) {
                trace(e.message);
            }
        }
    }

    public static function get viewPort():Rectangle {
        return _viewPort;
    }

    public static function set viewPort(value:Rectangle):void {
        _viewPort = value;
        update("viewPort", value);
    }

    public static function get visible():Boolean {
        return _visible;
    }

    public static function set visible(value:Boolean):void {
        _visible = value;
        update("visible", value);
    }

    private static function update(type:String, value:*):void {
        if (ANEContext.ctx) {
            try {
                ANEContext.ctx.call("updateNativeStage", type, value);
            } catch (e:Error) {
                trace(e.message);
            }
        }
    }

}
}
