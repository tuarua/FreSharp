package {
import com.tuarua.Person;
import com.tuarua.FreSharpExampleANE;
import com.tuarua.fre.ANEError;
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
            textField.width = 400;
            textField.height = 150;
            textField.multiline = true;
            textField.wordWrap = true;

            var person:Person = new Person();
            person.age = 21;
            person.name = "Tom";
            person.city.name = "Dunleer";

            var resultString:String = ane.runStringTests("I am a string from AIR with new interface");
            textField.text += resultString + "\n";

            var resultNumber:Number = ane.runNumberTests(31.99);
            textField.text += "Number: " + resultNumber + "\n";

            var resultInt:int = ane.runIntTests(-54, 66);
            textField.text += "Int: " + resultInt + "\n";

            var myArray:Array = [];
            myArray.push(3, 1, 4, 2, 6, 5);
            var resultArray:Array = ane.runArrayTests(myArray);
            if (resultArray)
                textField.text += "Array: " + resultArray.toString() + "\n";


            var resultObject:Person = ane.runObjectTests(person) as Person;
            if (resultObject) {
                textField.text += "Person.age: " + resultObject.age.toString() + "\n";
            }

           try {
                var inRect:Rectangle = new Rectangle(50, 60, 70, 80);
                var resultRectangle:Rectangle = ane.runExtensibleTests(inRect) as Rectangle;
                trace("resultRectangle", resultRectangle);
            } catch (e:ANEError) {
                trace(e.message);
                trace(e.type);
                trace(e.errorID);
                trace(e.getStackTrace());
            }


            const IMAGE_URL:String = "https://scontent.cdninstagram.com/t/s320x320/17126819_1827746530776184_5999931637335326720_n.jpg";

            var ldr:Loader = new Loader();
            ldr.contentLoaderInfo.addEventListener(Event.COMPLETE, ldr_complete);
            ldr.load(new URLRequest(IMAGE_URL));

            function ldr_complete(evt:Event):void {

                var spr:Sprite = new Sprite();


                var bmp:Bitmap = ldr.content as Bitmap;
                spr.addChild(bmp);
                var overlay:Sprite = new Sprite();
                overlay.graphics.beginFill(0x33FFCC);
                overlay.graphics.drawCircle(120, 100, 50);
                overlay.graphics.endFill();
                spr.addChild(overlay);

                var bmd:BitmapData = new BitmapData(320, 320, true, 0xFFFFFF);
                bmd.draw(spr);
                var sprBmp:Bitmap = new Bitmap(bmd, "auto", true);


                sprBmp.y = 150;
                addChild(sprBmp);

                ane.runBitmapTests(sprBmp.bitmapData);
            }

            var myByteArray:ByteArray = new ByteArray();
            myByteArray.writeUTFBytes("C# in an ANE. Say whaaaat!");
            ane.runByteArrayTests(myByteArray);

            //catch the error in C# only
            ane.runErrorTests(person, "test string", 78);

            //catch the error in as

            try {
                ane.runErrorTests2("abc");
            } catch (e:ANEError) {
                //trace("e is",e)
                trace("e.message: ", e.message);
                trace("e.type: ", e.type);
                trace("e.errorID", e.errorID);
                trace("e.getStackTrace", e.getStackTrace());
            }


            /*var inData:String = "Saved and returned"; //TODO
             var outData:String = ane.runDataTests(inData) as String;
             textField.text += outData + "\n";*/

            addChild(textField);


        }
        hasActivated = true;
    }

    private function onExiting(event:Event):void {
        ane.dispose();
    }

}
}