using GXPEngine;
using GXPEngine.Core;
using System;

namespace gxpengine_template
{
    public static class MyUtils
    {
        public static MyGame MyGame => (MyGame)MyGame.main;
        public static void SetCenterOrigin(this Sprite sprite)
        {
            if (sprite is AnimationSprite)
                Console.WriteLine("Warning! you are setting the origin of an animation sprite, which means width and height are not accurate for each animation frame!");
            sprite.SetOrigin(sprite.texture.width / 2, sprite.texture.height / 2);
        }

        public static void ResizePreservingAspectRatio(this Sprite sprite, int byWidth)
        {
            if (sprite is AnimationSprite)
                Console.WriteLine("Warning! you are setting the origin of an animation sprite, which means width and height are not accurate for each animation frame!");

            var aspectRatio = (float)sprite.texture.height / sprite.texture.width;
            sprite.width = byWidth;
            sprite.height = Mathf.Floor(byWidth * aspectRatio);
        }
        
    }
}
