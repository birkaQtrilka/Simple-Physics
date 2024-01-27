using GXPEngine;
using TiledMapParser;

namespace gxpengine_template
{
    public class Ground : AnimationSprite
    {
        public Ground( TiledObject data = null) : base("square.png",1,1,-1,true,true)
        {
            visible = false;
        }
    }
}
