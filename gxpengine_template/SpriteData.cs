using GXPEngine;

namespace gxpengine_template
{
    public readonly struct SpriteData
    {
        public readonly int w; 
        public readonly int h;
        public readonly bool preserveAspectRatio;
        public readonly bool addCollider;
        public readonly bool keepInChache;
        public readonly string fileName;
        public readonly bool mirrorX;
        readonly bool customSize;
        public SpriteData(string fileName, bool addCollider = true, bool keepInChache = false, bool mirrorX = false)
        {
            w = -1;
            h = -1;
            customSize = false;
            preserveAspectRatio = false;
            this.fileName = fileName;
            this.addCollider = addCollider;
            this.keepInChache = keepInChache;
            this.mirrorX = mirrorX;
        }
        public SpriteData(string fileName, int w, int h, bool preserveAspectRatio = false, bool addCollider = true, bool keepInChache = false, bool mirrorX = false)
        {
            this.w = w;
            this.h = h;
            customSize = true;
            this.preserveAspectRatio = preserveAspectRatio;
            this.fileName = fileName;
            this.addCollider = addCollider;
            this.keepInChache = keepInChache;
            this.mirrorX = mirrorX;

        }
        public Sprite CreateSprite()
        {
            var sprite = new Sprite(fileName,keepInChache,addCollider);
            sprite.Mirror(mirrorX,false);
            if(!customSize) return sprite;
            if(preserveAspectRatio)
            {
                
                //mentain aspect ratio
                var aspectRatio = (float)sprite.height / sprite.width;
                sprite.width = w;
                sprite.height = Mathf.Floor(sprite.width * aspectRatio);
            }
            else 
            {
                sprite.width = w;
                sprite.height = h;
            }
            return sprite;
        }
        public Sprite CreateSprite(int differentW, int differentH, bool preserveAR)
        {
            var sprite = new Sprite(fileName, keepInChache, addCollider);
            if (preserveAR)
            {
                var aspectRatio = (float)sprite.height / sprite.width;
                sprite.width = differentW;
                sprite.height = Mathf.Floor(sprite.width * aspectRatio);
            }
            else
            {
                sprite.width = differentH;
                sprite.height = differentW;
            }
            return sprite;
        }
    }
}
