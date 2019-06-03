package {

import com.mycompany.CustomEvent;
import com.mycompany.HelloWorldANE;
import com.tuarua.FreSharp;

import flash.desktop.NativeApplication;
import flash.display.Sprite;
import flash.events.Event;
import flash.text.TextField;

public class Main extends Sprite {
    private var freSharpANE:FreSharp = new FreSharp();//must create before all others

    public function Main() {
        NativeApplication.nativeApplication.addEventListener(Event.EXITING, onExiting);
        var ane:HelloWorldANE = HelloWorldANE.helloWorld;
        ane.addEventListener("MY_EVENT", onANEEvent);

        var myString:String = ane.sayHello("Hey there", true, 5);

        var textField:TextField = new TextField();
        textField.text = myString;
        addChild(textField);
    }

    private static function onANEEvent(event:CustomEvent):void {
        trace(event);
    }

    private function onExiting(event:Event):void {
        HelloWorldANE.dispose();
        freSharpANE.dispose();
    }
}
}
