using GXPEngine;
using System.Linq;
using TiledMapParser;

namespace gxpengine_template
{
    public class Hazard : AnimationSprite
    {
        public Hazard(TiledObject data) : base ("square.png", 1, 1) 
        {
            visible = false;
        }
        void Update()
        {
            if (!(GetCollisions(false).FirstOrDefault(c => c is IHealthHolder) is IHealthHolder healthHolder)) return;

            healthHolder.Health -= 99999;
        }

    }
}
