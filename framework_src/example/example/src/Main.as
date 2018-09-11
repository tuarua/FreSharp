package {
import com.tuarua.Person;
import com.tuarua.FreSharpExampleANE;
import flash.desktop.NativeApplication;
import flash.display.Bitmap;
import flash.display.BitmapData;

import flash.display.Loader;

import flash.display.Sprite;
import flash.display.StageAlign;
import flash.display.StageScaleMode;
import flash.events.Event;
import flash.geom.Rectangle;
import flash.net.URLRequest;
import flash.text.TextField;
import flash.text.TextFormat;
import flash.text.TextFormatAlign;
import flash.utils.ByteArray;

[SWF(width="640", height="640", frameRate="60", backgroundColor="#F1F1F1")]
public class Main extends Sprite {
    [Embed(source="adobeair.png")]
    public static const TestImage:Class;

    public static const GREEN:uint = 0xFF00FF00;
    public static const HALF_GREEN:uint = 0x8000FF00;

    private var ane:FreSharpExampleANE = new FreSharpExampleANE();
    private var hasActivated:Boolean = false;
    private var textField:TextField = new TextField();

    public function Main() {
        super();
        stage.align = StageAlign.TOP_LEFT;
        stage.scaleMode = StageScaleMode.NO_SCALE;

        NativeApplication.nativeApplication.addEventListener(Event.EXITING, onExiting);
        this.addEventListener(Event.ACTIVATE, onActivated);

    }
    
    private function onActivated(event:Event):void {
        if (!hasActivated) {
            var tf:TextFormat = new TextFormat();
            tf.size = 24;
            tf.color = 0x333333;
            tf.align = TextFormatAlign.LEFT;
            textField.defaultTextFormat = tf;
            textField.width = 640;
            textField.height = 250;
            textField.multiline = true;
            textField.wordWrap = true;

            var person:Person = new Person();
            person.age = 21;
            person.name = "Tom";
            person.city.name = "Portland";

            var resultString:String = ane.runStringTests("Björk Guðmundsdóttir Sinéad O’Connor 久保田  " +
                    "利伸 Михаил Горбачёв Садриддин Айнӣ Tor Åge Bringsværd 章子怡 €");

            textField.text += resultString + "\n";

            var resultNumber:Number = ane.runNumberTests(31.99);
            textField.text += "Number: " + resultNumber + "\n";

            var resultInt:int = ane.runIntTests(-54, 66);
            textField.text += "Int: " + resultInt + "\n";


            var intArray:Array = [];
            intArray.push(3, 1, 4, 2, 6, 5);

            var stringVec:Vector.<String> = new Vector.<String>();
            stringVec.push("a", "b", "c", "d");

            var numberVec:Vector.<Number> = new Vector.<Number>();
            numberVec.push(1, 0.5, 2.0, 3.3);

            var booleanVec:Vector.<Boolean> = new Vector.<Boolean>();
            booleanVec.push(true, true, false, true);

            var resultArray:Array = ane.runArrayTests(intArray, stringVec, numberVec, booleanVec);
            if (resultArray) {
                textField.text += "Array: " + resultArray.toString() + "\n";
            }

            var resultObject:Person = ane.runObjectTests(person) as Person;
            if (resultObject) {
                textField.text += "Person.age: " + resultObject.age.toString() + "\n";
            }


            var inRect:Rectangle = new Rectangle(50.9, 60, 70, 80);
            var resultRectangle:Rectangle = ane.runExtensibleTests(inRect) as Rectangle;
            trace("resultRectangle", resultRectangle);

            const IMAGE_URL:String = "http://www.ibasoglu.com/wp-content/uploads/2015/07/visual_csharp_logo1.png";

            var ldr:Loader = new Loader();
            ldr.contentLoaderInfo.addEventListener(Event.COMPLETE, ldr_complete);
            ldr.load(new URLRequest(IMAGE_URL));

            function ldr_complete(evt:Event):void {
                var spr:Sprite = new Sprite();
                var bmp:Bitmap = ldr.content as Bitmap;
                spr.addChild(bmp);
                var overlay:Sprite = new Sprite();
                overlay.graphics.beginFill(0x33FF00);
                overlay.graphics.drawCircle(120, 100, 50);
                overlay.graphics.endFill();
                spr.addChild(overlay);

                var bmd:BitmapData = new BitmapData(320, 320, true, 0xFFFFFF);
                bmd.draw(spr);
                var sprBmp:Bitmap = new Bitmap(bmd, "auto", true);

                sprBmp.y = 250;
                addChild(sprBmp);

                ane.runBitmapTests(sprBmp.bitmapData);
            }

            var myByteArray:ByteArray = new ByteArray();
            myByteArray.writeUTFBytes("C# in an ANE. Say whaaaat!");
            ane.runByteArrayTests(myByteArray);
            ane.runErrorTests(person, "test string", 78);

            const inData:String = "Saved and returned";
            trace("getActionScriptData returned is same", inData == ane.runDataTests(inData) as String ? "✅" : "❌");

            var testDate:Date = new Date(1990, 5, 13, 8, 59, 3);

            trace("Date returned is same", testDate.time == ane.runDateTests(testDate).time ? "✅" : "❌");
            trace("GREEN", GREEN, GREEN == ane.runColorTests(GREEN) ? "✅" : "❌");
            trace("HALF_GREEN", HALF_GREEN, HALF_GREEN == ane.runColorTests(HALF_GREEN) ? "✅" : "❌");

            addChild(textField);


        }
        hasActivated = true;
    }

    private function onExiting(event:Event):void {
        ane.dispose();
    }

}
}