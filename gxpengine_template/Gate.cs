using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace gxpengine_template
{
    public class Gate : AnimationSprite
    {
        readonly TiledObject _data;
        Player _player;
        public Gate(TiledObject data = null) : base("circle.png", 1,1,-1,true,true)
        {
            _data = data;
            SetFrame(1);
            alpha = 0;
            collider.isTrigger = true;
        }
        protected void Update()
        {
            if (_player == null)
                _player = MyUtils.MyGame.CurrentLevel.Player;
            if(_player.CurrentColliders != null && _player.CurrentColliders.Contains(this))
                MyUtils.MyGame.LoadLevel(_data.Name);

        }

    }
}
