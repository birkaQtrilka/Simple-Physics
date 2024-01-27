using GXPEngine;
using TiledMapParser;

namespace gxpengine_template
{
    public class Door : AnimationSprite, IUnlockable
    {
        public Door(TiledObject data) : base("door.png", 1, 1, -1, false)
        {

        }

        public void Unlock()
        {
            LateDestroy();
        }
    }
}
