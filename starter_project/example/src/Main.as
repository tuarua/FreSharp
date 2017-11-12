package {

import com.mycompany.CustomEvent;
import com.mycompany.HelloWorldANE;

import flash.display.Sprite;
import flash.text.TextField;

public class Main extends Sprite {
    public function Main() {

        var ane:HelloWorldANE = new HelloWorldANE();
        ane.addEventListener("MY_EVENT", onANEEvent);
        ane.init();

        var myString:String = ane.sayHello("Hey there", true, 5);

        var textField:TextField = new TextField();
        textField.text = myString;
        addChild(textField);
    }

    private function onANEEvent(event:CustomEvent):void {
        trace(event);
    }
}
}
