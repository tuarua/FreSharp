/**
 * Created by Local Eoin Landy on 25/05/2017.
 */
package com.tuarua.fre.display {
import com.tuarua.fre.ANEContext;

[RemoteClass(alias="com.tuarua.fre.display.NativeSprite")]
public class NativeSprite extends NativeDisplayObject {
    public function NativeSprite() {
        super();
        this.type = SPRITE_TYPE;
    }

    public function addChild(nativeDisplayObject:NativeDisplayObject):void {
        if (ANEContext.ctx) {
            try {
                ANEContext.ctx.call("addNativeChild", id, nativeDisplayObject);
                nativeDisplayObject.isAdded = true;
            } catch (e:Error) {
                trace(e.message);
            }
        }
    }
}
}
