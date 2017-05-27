using System;
using System.Collections.Generic;
using TuaRua.FreSharp.Display;
using FREObject = System.IntPtr;
using FREContext = System.IntPtr;
using System.Windows;

namespace TuaRua.FreSharp.Internal {
    /// <summary>
    /// 
    /// </summary>
    public class FreDisplayList {
        //id, <parent_id,fredisplayobject>
        /// <summary>
        /// 
        /// </summary>
        public static readonly Dictionary<string, Tuple<string, object>> Children =
            new Dictionary<string, Tuple<string, object>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="argc"></param>
        /// <param name="argv"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static FREObject AddChild(FREContext ctx, uint argc, FREObject[] argv) {
            var inFre0 = argv[0];
            if (inFre0 == FREObject.Zero) return FREObject.Zero;

            var inFre1 = argv[1];
            if (inFre1 == FREObject.Zero) return FREObject.Zero;

            var parentId = new FreObjectSharp(inFre0).Value as string;
            var child = new FreObjectSharp(inFre1);
            var id = Convert.ToString(child.GetProperty("id").Value);
            var type = (FreStageSharp.FreNativeType) Convert.ToInt32(child.GetProperty("type").Value);

            switch (type) {
                case FreStageSharp.FreNativeType.Image:
                    var nativeImage = new FreNativeImage(child);
                    AddToParent(parentId, nativeImage);
                    Children.Add(id, new Tuple<string, object>(parentId, nativeImage));
                    break;
                case FreStageSharp.FreNativeType.Button:
                    var nativeButton = new FreNativeButton(child, id, ref ctx);
                    AddToParent(parentId, nativeButton);
                    Children.Add(id, new Tuple<string, object>(parentId, nativeButton));
                    break;
                case FreStageSharp.FreNativeType.Sprite:
                    var nativeSprite = new FreNativeSprite(child);
                    AddToParent(parentId, nativeSprite);
                    Children.Add(id, new Tuple<string, object>(parentId, nativeSprite));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            return FREObject.Zero;
        }

        private static void AddToParent(string parentId, UIElement child) {
            if (parentId == "root") {
                var nativeRoot = FreStageSharp.GetRootView() as FreNativeRoot;
                nativeRoot?.AddChild(child);
            } else {
                var nativeView = Children[parentId].Item2 as FreNativeSprite;
                nativeView?.AddChild(child);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="argc"></param>
        /// <param name="argv"></param>
        /// <returns></returns>
        public static FREObject UpdateChild(FREContext ctx, uint argc, FREObject[] argv) {
            //id, type, value
            var inFre0 = argv[0];
            if (inFre0 == FREObject.Zero) return FREObject.Zero;

            var inFre1 = argv[1];
            if (inFre1 == FREObject.Zero) return FREObject.Zero;

            var inFre2 = argv[2];
            if (inFre2 == FREObject.Zero) return FREObject.Zero;

            var id = Convert.ToString(new FreObjectSharp(inFre0).Value);
            var t = Children[id].Item2.GetType();
            if (t == typeof(FreNativeImage)) {
                var child = Children[id].Item2 as FreNativeImage;
                child?.Update(inFre1, inFre2);
            }
            if (t == typeof(FreNativeButton)) {
                var child = Children[id].Item2 as FreNativeButton;
                child?.Update(inFre1, inFre2);
            }

            if (t == typeof(FreNativeSprite)) {
                var child = Children[id].Item2 as FreNativeSprite;
                child?.Update(inFre1, inFre2);
            }

            return FREObject.Zero;
        }
    }
}