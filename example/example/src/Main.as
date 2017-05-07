package {

import com.tuarua.ANEError;
import com.tuarua.Person;
import com.tuarua.FreSharpExampleANE;

import flash.display.Bitmap;
import flash.display.BitmapData;

import flash.display.Loader;

import flash.display.Sprite;
import flash.display.StageAlign;
import flash.display.StageScaleMode;
import flash.events.Event;
import flash.geom.Point;
import flash.geom.Rectangle;
import flash.net.URLRequest;
import flash.text.TextField;
import flash.text.TextFormat;
import flash.text.TextFormatAlign;
import flash.utils.ByteArray;

public class Main extends Sprite {
    private var ane:FreSharpExampleANE = new FreSharpExampleANE();
    private var hasActivated:Boolean = false;

    public function Main() {
        super();
        stage.align = StageAlign.TOP_LEFT;
        stage.scaleMode = StageScaleMode.NO_SCALE;

        this.addEventListener(Event.ACTIVATE, onActivated);


        var textField:TextField = new TextField();
        var tf:TextFormat = new TextFormat();
        tf.size = 24;
        tf.color = 0x333333;
        tf.align = TextFormatAlign.LEFT;
        textField.defaultTextFormat = tf;
        textField.width = 800;
        textField.height = 800;
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

        var myArray:Array = new Array();
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
            var pnt:Point;
            trace("resultRectangle", resultRectangle);
        } catch (e:ANEError) {
            trace(e.message);
            trace(e.type);
            trace(e.errorID);
            trace(e.getStackTrace());
        }


        const IMAGE_URL:String = "http://tinyurl.com/zaky3n4";

        var ldr:Loader = new Loader();
        ldr.contentLoaderInfo.addEventListener(Event.COMPLETE, ldr_complete);
        ldr.load(new URLRequest(IMAGE_URL));

        function ldr_complete(evt:Event):void {
            var bmp:Bitmap = ldr.content as Bitmap;
            var bmd:BitmapData = ane.runBitmapTests(bmp.bitmapData);
            if (bmd) {
                var bitmap:Bitmap = new Bitmap(bmd);
                bitmap.y = 150;
                addChild(bitmap);
            }
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
            trace("e is",e)
            trace(e.message);
            trace(e.type);
            trace(e.errorID);
            trace(e.getStackTrace());
        }


        /*var inData:String = "Saved and returned"; //TODO
         var outData:String = ane.runDataTests(inData) as String;
         textField.text += outData + "\n";*/


        addChild(textField);
    }

    private function onActivated(event:Event):void {
        if (!hasActivated) {
            // adds a native window over our AIR window and draws an ellipse
            // Supports transparency on Windows 8.1+ requires .NET4.6+
            ane.runNativeTests();
        }
        hasActivated = true;
    }
}
}