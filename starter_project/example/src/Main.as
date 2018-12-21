package {

import com.mycompany.CustomEvent;
import com.mycompany.HelloWorldANE;

import flash.desktop.NativeApplication;

import flash.display.Sprite;
import flash.events.Event;
import flash.text.TextField;

public class Main extends Sprite {
    private var ane:HelloWorldANE;

    public function Main() {
        NativeApplication.nativeApplication.addEventListener(Event.EXITING, onExiting);
        ane = HelloWorldANE.helloWorld;
        ane.addEventListener("MY_EVENT", onANEEvent);

        var myString:String = ane.sayHello("Hey there", true, 5);

        var textField:TextField = new TextField();
        textField.text = myString;
        addChild(textField);
    }

    private static function onANEEvent(event:CustomEvent):void {
        trace(event);
    }

    private static function onExiting(event:Event):void {
        HelloWorldANE.dispose();
    }
}
}
